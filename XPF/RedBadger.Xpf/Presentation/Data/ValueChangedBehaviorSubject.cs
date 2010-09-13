namespace RedBadger.Xpf.Presentation.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    internal class ValueChangedBehaviorSubject<T> : ISubject<T>
    {
        private readonly BehaviorSubject<T> subject;

        public ValueChangedBehaviorSubject(T value)
        {
            this.subject = new BehaviorSubject<T>(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this.subject.Subscribe(observer);
        }

        public void OnCompleted()
        {
            this.subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            this.subject.OnError(error);
        }

        public void OnNext(T value)
        {
            if (!Equals(value, this.subject.First()))
            {
                this.subject.OnNext(value);
            }
        }
    }
}