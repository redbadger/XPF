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

namespace RedBadger.Xpf.Specs.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Machine.Specifications;

    using RedBadger.Xpf.Internal;

    public static class MSpecExtensions
    {
        public static Vector ShouldBeCloseTo(this Vector arg1, Vector arg2)
        {
            if (arg1.IsDifferentFrom(arg2))
            {
                throw NewException("Should be close to {0} but is {1}", arg2, arg1);
            }

            return arg1;
        }

        public static IComparable ShouldBeGreaterThanOrEqualTo(this IComparable arg1, IComparable arg2)
        {
            if (arg2 == null)
            {
                throw new ArgumentNullException("arg2");
            }

            if (arg1 == null)
            {
                throw NewException("Should be greater than {0} but is [null]", arg2);
            }

            if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) < 0)
            {
                throw NewException("Should be greater than {0} but is {1}", arg2, arg1);
            }

            return arg1;
        }

        private static string EachToUsefulString<T>(this IEnumerable<T> enumerable)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.Append(String.Join(",\n", enumerable.Select(x => x.ToUsefulString().Tab()).Take(10).ToArray()));
            if (enumerable.Count() > 10)
            {
                if (enumerable.Count() > 11)
                {
                    sb.AppendLine(String.Format(",\n ...({0} more elements)", enumerable.Count() - 10));
                }
                else
                {
                    sb.AppendLine(",\n" + enumerable.Last().ToUsefulString().Tab());
                }
            }
            else
            {
                sb.AppendLine();
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private static SpecificationException NewException(string message, params object[] parameters)
        {
            if (parameters.Any())
            {
                return
                    new SpecificationException(
                        string.Format(message, parameters.Select(x => x.ToUsefulString()).Cast<object>().ToArray()));
            }

            return new SpecificationException(message);
        }

        private static string Tab(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            string[] split = str.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var sb = new StringBuilder();

            sb.Append(" " + split[0]);
            foreach (string part in split.Skip(1))
            {
                sb.AppendLine();
                sb.Append(" " + part);
            }

            return sb.ToString();
        }

        private static string ToUsefulString(this object obj)
        {
            string str;
            if (obj == null)
            {
                return "[null]";
            }

            if (obj.GetType() == typeof(string))
            {
                str = (string)obj;

                return "\"" + str.Replace("\n", "\\n") + "\"";
            }

            if (obj.GetType().IsValueType)
            {
                return "[" + obj + "]";
            }

            if (obj is IEnumerable)
            {
                IEnumerable<object> enumerable = ((IEnumerable)obj).Cast<object>();

                return obj.GetType() + ":\n" + enumerable.EachToUsefulString();
            }

            str = obj.ToString();

            if (str.Trim() == string.Empty)
            {
                return String.Format("{0}:[]", obj.GetType());
            }

            str = str.Trim();

            if (str.Contains("\n"))
            {
                const string Format = @"{1}:
[
{0}
]";
                return string.Format(Format, str.Tab(), obj.GetType());
            }

            if (obj.GetType().ToString() == str)
            {
                return obj.GetType().ToString();
            }

            return string.Format("{0}:[{1}]", obj.GetType(), str);
        }

        private static object TryToChangeType(this object original, Type type)
        {
            try
            {
                return Convert.ChangeType(original, type);
            }
            catch
            {
                return original;
            }
        }
    }
}
