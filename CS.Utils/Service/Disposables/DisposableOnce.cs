using System;

namespace ArsuLeo.CS.Utils.Service.Disposables
{

    public class DisposableOnce<T> : IDisposable
        where T : IDisposable
    {
        private bool disposedValue;

        private readonly object LockObj = new object();
        public delegate void DisposedDelegate(EventArgs e);
        public event DisposedDelegate OnDisposed = delegate { };

        public bool ShallowOnDisposedEventExceptions { get; set; } = false;
        public T Value { get; }
        
        public DisposableOnce(T value)
        {
            Value = value;
        }

        private void EmitDisposed()
        {
            if (ShallowOnDisposedEventExceptions)
            {
                try
                {
                    OnDisposed?.Invoke(new EventArgs());
                }
                catch (Exception)
                {
                    //Shallow the exception to avoid propagation?
                }
            }
            else
            {
                OnDisposed?.Invoke(new EventArgs());

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (LockObj)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        Value.Dispose();
                    }
                    disposedValue = true;
                    EmitDisposed();
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    //public static class TEst
    //{
    //    public static Bitmap Transform(Bitmap src, RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone,
    //            double scale = 0, int pad = 0, int alterGradient = 0)
    //    {
    //        using Bitmap rotated = src.Rotate(rotate);
    //        using Bitmap scaled = MyImageUtils.ScaleBitmap(rotated, scale);
    //        using Bitmap padded = MyImageUtils.PaddBitmap(scaled, scale);
    //        //The owner is the caller
    //        Bitmap result = MyImageUtils.Gradient(padded, alterGradient);
    //        return result;
    //    }
    //    public static Bitmap Transform(Bitmap src, RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone, double scale = 0, int pad = 0, int alterGradient = 0)
    //    {
    //        using DisposableOnce<Bitmap> rotated = new DisposableOnce<Bitmap>(src.Rotate(rotate));
    //        using DisposableOnce<Bitmap> scaled = scale == 0 ? new DisposableOnce<Bitmap>(rotated) : new DisposableOnce<Bitmap>(MyImageUtils.ScaleBitmap(rotated.Value, scale));
    //        using DisposableOnce<Bitmap> padded = pad == 0 ? new DisposableOnce<Bitmap>(scaled) : new DisposableOnce<Bitmap>(MyImageUtils.PaddBitmap(scaled.Value, scale));
    //        Bitmap result;
    //        if (alterGradient == 0)
    //        {
    //            //Avoid the value being disposed by the wrapper relatives
    //            padded.SetDisposed();
    //            result = padded.Value;
    //        }
    //        else
    //        {
    //            result = MyImageUtils.Gradient(padded.Value, alterGradient);
    //        }
    //        return result;
    //    }
    //}
}
