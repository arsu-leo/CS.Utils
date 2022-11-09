using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Service.App
{
    public class ApplicationArguments
    {
        protected readonly List<string> Args;
        public readonly string[] Prepends;
        public ApplicationArguments(string[] args) : this(args, new string[] { "" }) { }
        
        public ApplicationArguments(string[] args, string[] prepends)
        {
            Args = new List<string>(args);
            if (prepends.Length == 0)
            {
                prepends = new string[] { "" };
            }
            Prepends = prepends;
        }

        public int GetArgIndex(string arg)
        {
            return ApplicationArgumentsUtil.GetArgIndexPrepends(Args, arg, Prepends);
        }

        public bool HasArg(string arg)
        {
            return GetArgIndex(arg) >= 0;
        }

        public string? GetArgParamVal(string findArg, string glue = "=")
        {

            for (int j = 0; j < Prepends.Length; j++)
            {
                string findArgPrep = Prepends[j] + findArg + glue;
                for (int i = 0; i < Args.Count; i++)
                {
                    string arg = Args[i];
                    int index = arg.IndexOf(findArgPrep);
                    if (index >= 0)
                    {
                        string val = arg.Substring(arg.IndexOf(glue) + 1);
                        return val;
                    }
                }
            }
            return null;
        }

        public void AddArg(string arg)
        {
            if (!HasArg(arg))
            {
                Args.Add(arg);
            }
        }
    }
}
