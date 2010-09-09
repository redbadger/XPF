namespace XpfSamples.S02ApplicationBar
{
    using System.Collections.Generic;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Media;

    public class ApplicationBar : ContentControl
    {
        private readonly IList<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();

        public IList<ApplicationBarIconButton> Buttons
        {
            get
            {
                return this.buttons;
            }
        }

        public override void OnApplyTemplate()
        {
            var itemsControl = new ItemsControl
                {
                    ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal }, 
                    ItemsSource = new [] {"", ""}, 
                    ItemTemplate = () =>
                        {
                            var border = new Border
                                {
                                    Background = new SolidColorBrush(Colors.Red), 
                                    Height = 100, 
                                    Width = 100, 
                                    Margin = new Thickness(10), 
                                    // VerticalAlignment = VerticalAlignment.Bottom
                                };

                            /*border.Bind(
                                Border.BackgroundProperty, 
                                BindingFactory.CreateOneWay<ApplicationBarIconButton, Brush>(button => button.Color));*/
                            return border;
                        }, 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            this.Content = itemsControl;
        }
    }
}