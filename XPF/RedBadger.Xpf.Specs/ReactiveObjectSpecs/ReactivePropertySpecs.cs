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
// ReSharper disable ClassNeverInstantiated.Global

namespace RedBadger.Xpf.Specs.ReactiveObjectSpecs
{
    using Machine.Specifications;

    using Moq;

    using It = Machine.Specifications.It;

    public class TestBindingObject : ReactiveObject
    {
        private static readonly ReactiveProperty<double> WidthProperty = ReactiveProperty<double>.Register(
            "Width", typeof(TestBindingObject), double.NaN, WidthChangedCallback);

        public double Width
        {
            get
            {
                return this.GetValue(WidthProperty);
            }

            set
            {
                this.SetValue(WidthProperty, value);
            }
        }

        public virtual void WidthChangedCallback(double testBindingObject, double newValue)
        {
        }

        private static void WidthChangedCallback(
            IReactiveObject source, ReactivePropertyChangeEventArgs<double> reactivePropertyChange)
        {
            ((TestBindingObject)source).WidthChangedCallback(
                reactivePropertyChange.OldValue, reactivePropertyChange.NewValue);
        }
    }

    public abstract class a_ReactiveObject
    {
        protected static ReactiveObject Subject;

        private Establish context = () => Subject = new ReactiveObject();
    }

    [Subject(typeof(ReactiveObject))]
    public class when_clearing_a_reactive_property_value : a_ReactiveObject
    {
        private static readonly ReactiveProperty<string> TestPropertyProperty;

        private static readonly string defaultValue = string.Empty;

        private Establish context = () => Subject.SetValue(TestPropertyProperty, "A Value");

        private Because of = () => Subject.ClearValue(TestPropertyProperty);

        private It should_revert_to_default = () => Subject.GetValue(TestPropertyProperty).ShouldEqual(defaultValue);

        static when_clearing_a_reactive_property_value()
        {
            TestPropertyProperty = ReactiveProperty<string>.Register("TestProperty", typeof(object), defaultValue);
        }
    }

    [Subject(typeof(ReactiveObject))]
    public class when_changing_a_reactive_property_value : a_ReactiveObject
    {
        private const string ExpectedFinalValue = "Final Value";

        private static readonly ReactiveProperty<string> TestPropertyProperty;

        private static readonly string defaultValue = string.Empty;

        private Establish context = () => Subject.SetValue(TestPropertyProperty, "Initial Value");

        private Because of = () => Subject.SetValue(TestPropertyProperty, ExpectedFinalValue);

        private It should_return_the_correct_value =
            () => Subject.GetValue(TestPropertyProperty).ShouldEqual(ExpectedFinalValue);

        static when_changing_a_reactive_property_value()
        {
            TestPropertyProperty = ReactiveProperty<string>.Register("TestProperty", typeof(object), defaultValue);
        }
    }

    [Subject(typeof(ReactiveObject))]
    public class when_a_value_is_changed_three_times
    {
        private const double ExpectedWidth1 = 1d;

        private const double ExpectedWidth2 = 2d;

        private const double ExpectedWidth3 = 3d;

        private static Mock<TestBindingObject> target;

        private Establish context = () => { target = new Mock<TestBindingObject> { CallBase = true }; };

        private Because of = () =>
            {
                target.Object.Width = ExpectedWidth1;
                target.Object.Width = ExpectedWidth2;
                target.Object.Width = ExpectedWidth3;
            };

        private It should_call_the_target_property_changed_callback1 =
            () =>
            target.Verify(
                o =>
                o.WidthChangedCallback(
                    Moq.It.Is<double>(d => d.Equals(double.NaN)), Moq.It.Is<double>(d => d.Equals(ExpectedWidth1))), 
                Times.Once());

        private It should_call_the_target_property_changed_callback2 =
            () =>
            target.Verify(
                o =>
                o.WidthChangedCallback(
                    Moq.It.Is<double>(d => d.Equals(ExpectedWidth1)), Moq.It.Is<double>(d => d.Equals(ExpectedWidth2))), 
                Times.Once());

        private It should_call_the_target_property_changed_callback3 =
            () =>
            target.Verify(
                o =>
                o.WidthChangedCallback(
                    Moq.It.Is<double>(d => d.Equals(ExpectedWidth2)), Moq.It.Is<double>(d => d.Equals(ExpectedWidth3))), 
                Times.Once());
    }
}
