namespace Xpf.Samples.S04BasketballScoreboard.Domain
{
    using System;
    using System.Linq;

    public class Clock
    {
        private readonly IObservable<long> timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1));

        private static TimeSpan periodLength = TimeSpan.FromMinutes(15);

        private int period = 1;

        public int Period
        {
            get
            {
                return this.period;
            }
        }

        public IObservable<string> TimeDisplay
        {
            get
            {
                return this.timer.Select(
                    time =>
                        {
                            TimeSpan remainingTime = periodLength.Subtract(TimeSpan.FromSeconds(time));
                            return string.Format("{0:d2}:{1:d2}", remainingTime.Minutes, remainingTime.Seconds);
                        });
            }
        }
    }
}