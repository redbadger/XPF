namespace RedBadger.Xpf
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    internal static class License
    {
        internal static void Validate()
        {
            Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;

            var buildTime =
                new TimeSpan((TimeSpan.TicksPerDay * version.Build) + (TimeSpan.TicksPerSecond * 2 * version.Revision));

            DateTime buildDateTime = new DateTime(2000, 1, 1).Add(buildTime);

            DateTime expiryDate = buildDateTime.AddDays(30).Date;
            DateTime nowDate = DateTime.Now.Date;

            if (expiryDate < nowDate)
            {
                throw new InvalidOperationException(
                    "Your trial license of XPF has expired.  Please visit http://red-badger.com to obtain a license or download the latest nightly build.");
            }

            double remainingDays = expiryDate.Subtract(nowDate).TotalDays;
            Debug.WriteLine("Red Badger XPF Trial Licence: You have {0} days remaining", remainingDays);
        }
    }
}