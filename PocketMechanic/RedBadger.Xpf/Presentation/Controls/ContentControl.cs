namespace RedBadger.Xpf.Presentation.Controls
{
    using System.Windows;

    using UIElement = RedBadger.Xpf.Presentation.UIElement;

    /// <summary>
    ///   Represents a control with a single piece of content.
    /// </summary>
    public class ContentControl : UIElement
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(IElement), typeof(ContentControl), new PropertyMetadata(null));

        public IElement Content
        {
            get
            {
                return (IElement)this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }
    }
}