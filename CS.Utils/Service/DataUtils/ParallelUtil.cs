using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class ParallelUtil
    {
        public delegate ResultT Mapper<SourceT, ResultT>(SourceT source);
        public static List<ResultT> Map<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, Mapper<SourceT, ResultT> fn)
        {

            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
                //Local list init for storing result locally
                () => new List<ResultT>(),
                (toProcess, loopControl, localList) => // map
                {
                    ResultT r = fn(toProcess);
                    localList.Add(r);
                    return localList;
                },
                (localList) => // this is our finaliser, reducing our map result into our result list
                {
                    lock (result)
                    {
                        result.AddRange(localList);
                    }
                }
            );
            return result;
        }

        public delegate IEnumerable<ResultT> MultMapper<SourceT, ResultT>(SourceT source);
        public static List<ResultT> MapMultiple<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, MultMapper<SourceT, ResultT> fn)
        {

            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
                //Local list init for storing result locally
                () => new List<ResultT>(), 
                (toProcess, loopControl, localList) => // map
                {
                    IEnumerable<ResultT> rs = fn(toProcess);
                    localList.AddRange(rs);
                    return localList;
                },
                (localList) => // this is our finaliser, reducing our map result into our result list
                {
                    lock (result)
                    {
                        result.AddRange(localList);
                    }
                }
            );
            return result;
        }

        public delegate void DataListHandler<SourceT, ResultT>(SourceT source, List<ResultT> accumulator);
        public static List<ResultT> MapAction<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, Action<SourceT, List<ResultT>> fn)
        {
            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
            () => new List<ResultT>(), // for storing result locally
            (toProcess, loopControl, localList) => // map
            {
                List<ResultT> tmpList = new List<ResultT>();
                fn(toProcess, tmpList);
                localList.AddRange(localList);
                return localList;
            },
            (localList) => // this is our finaliser, reducing our map result into our result list
            {
                lock (result)
                {
                    result.AddRange(localList);
                }
            });
            return result;
        }

        public delegate bool DataFilter<SourceT>(SourceT data);
        public static List<ResultT> MapReduceStart<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, Mapper<SourceT, ResultT> fn, DataFilter<SourceT> startReduce)
        {
            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
            () => new List<ResultT>(), // for storing result locally
            (toProcess, loopControl, localList) => // map
            {
                if (startReduce(toProcess))
                {
                    ResultT r = fn(toProcess);
                    localList.Add(r);
                }
                return localList;
            },
            (localList) => // this is our finaliser, reducing our map result into our result list
            {
                lock (result)
                {
                    result.AddRange(localList);
                }
            });
            return result;
        }

        public static List<ResultT> MapReduceEnd<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, Mapper<SourceT, ResultT> fn, DataFilter<ResultT> endReduce)
        {
            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
            () => new List<ResultT>(), // for storing result locally
            (toProcess, loopControl, localList) => // map
            {
                ResultT r = fn(toProcess);
                if (endReduce(r))
                {
                    localList.Add(r);
                }
                return localList;
            },
            (localList) => // this is our finaliser, reducing our map result into our result list
            {
                lock (result)
                {
                    result.AddRange(localList);
                }
            });
            return result;
        }
        public static List<ResultT> MapReduce<ResultT, SourceT>(IEnumerable<SourceT> dataToProcess, Mapper<SourceT, ResultT> fn, DataFilter<SourceT> startReduce, DataFilter<ResultT> endReduce)
        {

            List<ResultT> result = new List<ResultT>();
            _ = Parallel.ForEach(dataToProcess,
            () => new List<ResultT>(), // for storing result locally
            (toProcess, loopControl, localList) => // map
            {
                if (startReduce(toProcess))
                {
                    ResultT r = fn(toProcess);
                    if (endReduce(r))
                    {
                        localList.Add(r);
                    }
                }
                return localList;
            },
            (localList) => // this is our finaliser, reducing our map result into our result list
            {
                lock (result)
                {
                    result.AddRange(localList);
                }
            });
            return result;
        }

        public delegate Dictionary<ResultK, ResultV> DictionaryMapAccumulator<ResultK, ResultV, SourceT>(SourceT data, Dictionary<ResultK, ResultV> accumulator)
            where ResultK : notnull;

        public static Dictionary<ResultK, ResultV> MapDictionary<ResultK, ResultV, SourceT>(IEnumerable<SourceT> dataToProcess, DictionaryMapAccumulator<ResultK, ResultV, SourceT> fn)
            where ResultK : notnull
        {

            Dictionary<ResultK, ResultV> result = new Dictionary<ResultK, ResultV>();
            _ = Parallel.ForEach(dataToProcess,
            () => new Dictionary<ResultK, ResultV>(), // for storing result locally
            (toProcess, loopControl, localList) => fn(toProcess, localList),
            (localList) => // this is our finaliser, reducing our map result into our result list
            {
                lock (result)
                {
                   Model.Collections.Dictionary.DictionaryExtensions.MergeIntoMain(result, localList);
                }
            });
            return result;
        }
    }
}
