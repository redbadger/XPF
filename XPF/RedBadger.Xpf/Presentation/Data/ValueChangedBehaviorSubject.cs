namespace RedBadger.Xpf.Presentation.Data
{
#if WINDOWS_PHONE

    using System;

    using Microsoft.Phone.Reactive;

#else
    using System;
    using System.Collections.Generic;
    using System.Concurrency;
    using System.Linq;

#endif

    internal class ValueChangedBehaviorSubject<T> : ISubject<T>
    {
        private readonly BehaviorSubject<T> subject;

        public ValueChangedBehaviorSubject(T value, IScheduler scheduler)
        {
            this.subject = new BehaviorSubject<T>(value, scheduler);
        }

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