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

//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.PointSpecs
{
    using Machine.Specifications;

    [Subject(typeof(Point))]
    public class when_two_points_are_equal
    {
        private static Point point;

        private static bool result;

        private static Point subject;

        private Establish context = () =>
            {
                subject = new Point(10, 20);
                point = new Point(10, 20);
            };

        private Because of = () => result = point == subject;

        private It should_show_them_as_equal = () => result.ShouldBeTrue();
    }

    [Subject(typeof(Point))]
    public class when_two_points_are_not_equal_in_the_x
    {
        private static Point point;

        private static bool result;

        private static Point subject;

        private Establish context = () =>
            {
                subject = new Point(10, 20);
                point = new Point(11, 20);
            };

        private Because of = () => result = point == subject;

        private It should_show_them_as_equal = () => result.ShouldBeFalse();
    }

    [Subject(typeof(Point))]
    public class when_two_points_are_not_equal_in_the_y
    {
        private static Point point;

        private static bool result;

        private static Point subject;

        private Establish context = () =>
            {
                subject = new Point(10, 20);
                point = new Point(10, 21);
            };

        private Because of = () => result = point == subject;

        private It should_show_them_as_equal = () => result.ShouldBeFalse();
    }

    [Subject(typeof(Point))]
    public class when_a_vector_is_added
    {
        private static Point result;

        private static Point subject;

        private Establish context = () => subject = new Point(10, 20);

        private Because of = () => result = subject + new Vector(11, 12);

        private It should_add_the_vector_to_the_point = () => result.ShouldEqual(new Point(21, 32));
    }

    [Subject(typeof(Point))]
    public class when_a_vector_is_subtracted
    {
        private static Point result;

        private static Point subject;

        private Establish context = () => subject = new Point(10, 20);

        private Because of = () => result = subject - new Vector(11, 12);

        private It should_subtract_the_vector_from_the_point = () => result.ShouldEqual(new Point(-1, 8));
    }

    [Subject(typeof(Point))]
    public class when_a_point_is_subtracted
    {
        private static Vector result;

        private static Point subject;

        private Establish context = () => subject = new Point(10, 20);

        private Because of = () => result = subject - new Point(11, 12);

        private It should_subtract_the_vector_from_the_point = () => result.ShouldEqual(new Vector(-1, 8));
    }

    [Subject(typeof(Point))]
    public class when_a_point_is_cast_to_a_Size
    {
        private static Size result;

        private static Point subject;

        private Establish context = () => subject = new Point(10, 20);

        private Because of = () => result = (Size)subject;

        private It should_result_in_an_equivalent_Size = () => result.ShouldEqual(new Size(10, 20));
    }

    [Subject(typeof(Point))]
    public class when_a_point_is_cast_to_a_Vector
    {
        private static Vector result;

        private static Point subject;

        private Establish context = () => subject = new Point(10, 20);

        private Because of = () => result = (Vector)subject;

        private It should_result_in_an_equivalent_Size = () => result.ShouldEqual(new Vector(10, 20));
    }
}
