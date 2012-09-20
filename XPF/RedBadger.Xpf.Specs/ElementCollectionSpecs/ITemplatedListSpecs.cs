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

namespace RedBadger.Xpf.Specs.ElementCollectionSpecs
{
    using System.Collections.Generic;

    using Machine.Specifications;

    using Moq;

    using It = Machine.Specifications.It;

    [Subject(typeof(ElementCollection), "Visual Parent")]
    public class when_adding_an_element_via_a_TemplatedList_reference : an_ElementCollection_as_a_Templated_List
    {
        private static readonly IElement element = new Mock<UIElement> { CallBase = true }.Object;

        private static readonly object item = new object();

        private Establish context = () => Owner.Object.Measure(Size.Empty);

        private Because of = () => Subject.Add(item, _ => element);

        private It should_invalidate_its_owners_measure = () => Owner.Object.IsMeasureValid.ShouldBeFalse();

        private It should_set_the_elements_data_context = () => element.DataContext.ShouldBeTheSameAs(item);

        private It should_set_the_elements_visual_parent = () => element.VisualParent.ShouldBeTheSameAs(Owner.Object);
    }

    [Subject(typeof(ElementCollection), "Visual Parent")]
    public class when_inserting_an_element_via_a_TemplatedList_reference : an_ElementCollection_as_a_Templated_List
    {
        private static readonly IElement element = new Mock<UIElement> { CallBase = true }.Object;

        private static readonly object item = new object();

        private Establish context = () => Owner.Object.Measure(Size.Empty);

        private Because of = () => Subject.Insert(0, item, _ => element);

        private It should_invalidate_its_owners_measure = () => Owner.Object.IsMeasureValid.ShouldBeFalse();

        private It should_set_the_elements_data_context = () => element.DataContext.ShouldBeTheSameAs(item);

        private It should_set_the_elements_visual_parent = () => element.VisualParent.ShouldBeTheSameAs(Owner.Object);
    }

    [Subject(typeof(ElementCollection), "Visual Parent")]
    public class when_clearing_the_collection_via_a_TemplatedList_reference : an_ElementCollection_as_a_Templated_List
    {
        private Establish context = () =>
            {
                Subject.Add(new object(), _ => new Mock<IElement>().Object);
                Owner.Object.Measure(Size.Empty);
            };

        private Because of = () => Subject.Clear();

        private It should_invalidate_its_owners_measure = () => Owner.Object.IsMeasureValid.ShouldBeFalse();
    }

    [Subject(typeof(ElementCollection), "Visual Parent")]
    public class when_moving_an_element_via_a_TemplatedList_reference : an_ElementCollection_as_a_Templated_List
    {
        private static readonly IElement element1 = new Mock<IElement>().Object;

        private static readonly IElement element2 = new Mock<IElement>().Object;

        private Establish context = () =>
            {
                Subject.Add(null, _ => element1);
                Subject.Add(null, _ => element2);
            };

        private Because of = () => Subject.Move(0, 1);

        private It should_move_the_element = () => ((IList<IElement>)Subject)[1].ShouldBeTheSameAs(element1);

        private It should_shift_the_existing_element = () => ((IList<IElement>)Subject)[0].ShouldBeTheSameAs(element2);
    }
}
