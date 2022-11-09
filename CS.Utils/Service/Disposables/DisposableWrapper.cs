using System;

namespace ArsuLeo.CS.Utils.Service.Disposables
{
    public class DisposableWrapper<T> : IDisposable
    {
        private readonly T WrappingObject;
        private readonly Action<T> DisposeCallback;

        public T Value => disposedValue
                    ? throw new ObjectDisposedException(nameof(WrappingObject))
                    : WrappingObject;

        public DisposableWrapper(T wrappingObject, Action<T> disposeCallback)
        {
            WrappingObject = wrappingObject;
            DisposeCallback = disposeCallback;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeCallback(Value);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
