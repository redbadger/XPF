#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

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
