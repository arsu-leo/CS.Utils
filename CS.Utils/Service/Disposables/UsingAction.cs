using System;

namespace ArsuLeo.CS.Utils.Service.Disposables
{
    public class UsingAction : IDisposable
    {
        private readonly Action DisposeAction;

        // Track whether Dispose has been called. 
        private bool disposed = false;

        public UsingAction(Action disposeAction, Action doAtStartAction) : this(disposeAction)
        {
            doAtStartAction?.Invoke();
        }

        public UsingAction(Action disposeAction)
        {
            DisposeAction = disposeAction;
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!disposed)
            {
                disposed = true;
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    DisposeAction?.Invoke();
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() ?? "";
        }
    }

    public class UsingAction<T> : IDisposable
    {
        private readonly Action<T> DisposeAction;

        // Track whether Dispose has been called. 
        private bool disposed = false;
        public T Value { get; }
        public UsingAction(Func<T> factory, Action<T> disposeAction) : this(factory.Invoke(), disposeAction)
        {
        }

        public UsingAction(T val, Action<T> disposeAction)
        {
            Value = val;
            DisposeAction = disposeAction;
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!disposed)
            {
                disposed = true;
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    DisposeAction?.Invoke(Value);
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() ?? "";
        }
    }
}
