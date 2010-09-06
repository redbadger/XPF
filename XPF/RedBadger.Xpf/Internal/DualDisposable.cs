namespace RedBadger.Xpf.Internal
{
    using System;

    internal class DualDisposable : IDisposable
    {
        private readonly IDisposable firstDisposable;

        private readonly IDisposable secondDisposable;

        private bool isDisposed;

        public DualDisposable(IDisposable firstDisposable, IDisposable secondDisposable)
        {
            this.firstDisposable = firstDisposable;
            this.secondDisposable = secondDisposable;
        }

        ~DualDisposable()
        {
            this.Dispose(false);
        }

        public void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    this.firstDisposable.Dispose();
                    this.secondDisposable.Dispose();
                }
            }

            this.isDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}