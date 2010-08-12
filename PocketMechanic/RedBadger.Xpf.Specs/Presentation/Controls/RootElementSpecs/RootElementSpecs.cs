//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace RedBadger.Xpf.Specs.Presentation.Controls.RootElementSpecs
{
    using Machine.Specifications;

    using Microsoft.Xna.Framework;

    using Moq;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    using It = Machine.Specifications.It;

    public abstract class a_RootElement
    {
        protected static RootElement RootElement;

        protected static Rect ViewPort = new Rect(new Vector2(10, 20), new Size(100, 200));

        private Establish context = () =>
            {
                Renderer = new Mock<IRenderer>();
                RootElement = new RootElement(Renderer.Object, ViewPort);
            };

        protected static Mock<IRenderer> Renderer;
    }

    [Subject(typeof(RootElement))]
    public class after_update : a_RootElement
    {
        private Because of = () => RootElement.Update();

        private It should_have_the_correct_visual_offset = () => RootElement.VisualOffset.ShouldEqual(ViewPort.Position);
    }

    [Subject(typeof(RootElement))]
    public class when_arrange_is_invalid : a_RootElement
    {
        private Because of = () =>
            {
                RootElement.InvalidateArrange();
                RootElement.Update();
            };

        private It should_clear_the_renderer = () => Renderer.Verify(renderer => renderer.ClearInvalidDrawingContexts());
    }
}