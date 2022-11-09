using System;

namespace ArsuLeo.CS.Utils.Service.DataManagers.Expiring
{
    public class LifeTimeValueProvider<T> : ValueProvider<T>
    {
        private readonly object lockObj = new object();

        private bool EverRetrieved = false;
        private T LastValueRetrieved;
        private DateTime EndOfLifeDate = DateTime.MinValue;

        public bool IsExpired => EndOfLifeDate < DateTime.Now;
        public TimeSpan ValidDurationTime;

        public static LifeTimeValueProvider<P> FromProvider<P>(TimeSpan validDurationTime, ValueProvider<P> vp)
            where P: class
        {
            return new LifeTimeValueProvider<P>(validDurationTime, vp.GetValue);
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public LifeTimeValueProvider(TimeSpan validDurationTime, Func<T> valueRetriever) : base(valueRetriever)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ValidDurationTime = validDurationTime;
        }

        public override T GetValue() => GetValue(false);

        public T GetValue(bool forceRefresh)
        {
            if(forceRefresh || IsExpired || !EverRetrieved)
            {
                lock(lockObj)
                {
                    if (forceRefresh || IsExpired || !EverRetrieved)
                    {
                        EverRetrieved = true;
                        LastValueRetrieved = base.GetValue();
                        EndOfLifeDate = DateTime.Now.Add(ValidDurationTime);
                    }
                }
            }
            return LastValueRetrieved;
        }
    }
}
