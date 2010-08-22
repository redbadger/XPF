//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.Controls.VirtualizingStackPanelSpecs
{
    using Machine.Specifications;

    using RedBadger.Xpf.Presentation.Controls;

    public abstract class a_VirtualizingStackPanel
    {
        protected static VirtualizingStackPanel virtualizingStackPanel;

        private Establish context = () => virtualizingStackPanel = new VirtualizingStackPanel();
    }

    [Subject(typeof(VirtualizingStackPanel), "Scrolling")]
    public class when_placed_in_a_ScrollViewer : a_VirtualizingStackPanel
    {
        private static ScrollViewer scrollViewer;

        private Establish context = () => scrollViewer = new ScrollViewer();

        private Because of = () => scrollViewer.Content = virtualizingStackPanel;

        private It should_not_need_a_ScrollContentPresenter =
            () => virtualizingStackPanel.VisualParent.ShouldBeOfType<ScrollViewer>();
    }
}