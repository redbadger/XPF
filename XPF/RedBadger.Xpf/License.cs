namespace RedBadger.Xpf
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    internal static class License
    {
        internal static void Validate()
        {
            var version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;

            var buildDateTime =
                new DateTime(2000, 1, 1).Add(
                    new TimeSpan(
                        (TimeSpan.TicksPerDay * version.Build) + (TimeSpan.TicksPerSecond * 2 * version.Revision)));

            var expiryDate = buildDateTime.AddDays(30).Date;
            var nowDate = DateTime.Now.Date;

            if (expiryDate < nowDate)
            {
                throw new InvalidOperationException(
                    "Your trial license of XPF has expired.  Please visit http://red-badger.com to obtain a license or download the latest nightly build.");
            }

            var remainingDays = expiryDate.Subtract(nowDate).TotalDays;
            Debug.WriteLine("Red Badger XPF Trial Licence: You have {0} days remaining", remainingDays);
        }
    }
}