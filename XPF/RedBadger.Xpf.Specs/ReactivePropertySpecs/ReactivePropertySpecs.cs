//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.ReactivePropertySpecs
{
    using System;

    using Machine.Specifications;

    [Subject(typeof(ReactiveProperty<>), "Default Values")]
    public class when_a_reactive_property_of_reference_type_is_registered_with_no_default
    {
        private static ReactiveProperty<object> SubjectProperty;

        private Because of = () => SubjectProperty = ReactiveProperty<object>.Register("Subject", typeof(object));

        private It should_have_a_default_value_of_null = () => SubjectProperty.DefaultValue.ShouldEqual(null);
    }

    [Subject(typeof(ReactiveProperty<>), "Default Values")]
    public class when_a_reactive_property_of_value_type_is_registered_with_no_default
    {
        private static ReactiveProperty<int> SubjectProperty;

        private Because of = () => SubjectProperty = ReactiveProperty<int>.Register("Subject", typeof(object));

        private It should_have_a_default_value_of_zero = () => SubjectProperty.DefaultValue.ShouldEqual(0);
    }

    [Subject(typeof(ReactiveProperty<>), "Default Values")]
    public class when_a_reactive_property_of_reference_type_is_registered_with_a_default_value
    {
        private const string DefaultValue = "Default Value";

        private static ReactiveProperty<string> SubjectProperty;

        private Because of =
            () => SubjectProperty = ReactiveProperty<string>.Register("Subject", typeof(object), DefaultValue);

        private It should_have_the_registered_default_value =
            () => SubjectProperty.DefaultValue.ShouldEqual(DefaultValue);
    }

    [Subject(typeof(ReactiveProperty<>), "Default Values")]
    public class when_a_reactive_property_of_value_type_is_registered_with_a_default_value
    {
        private static ReactiveProperty<int> SubjectProperty;

        private Because of =
            () => SubjectProperty = ReactiveProperty<int>.Register("Subject", typeof(object), Int32.MaxValue);

        private It should_have_the_registered_default_value =
            () => SubjectProperty.DefaultValue.ShouldEqual(Int32.MaxValue);
    }
}