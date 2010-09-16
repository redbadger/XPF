namespace XpfSamples.S02ApplicationBar
{
    using System;
    using System.Collections.Generic;

    using RedBadger.Xpf.Presentation;
    using RedBadger.Xpf.Presentation.Controls;
    using RedBadger.Xpf.Presentation.Data;
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
            if (this.buttons.Count > 4)
            {
                throw new NotSupportedException("Too many buttons - the maximum is 4.");
            }

            this.Height = 70;

            var containingBorder = new Border { Background = new SolidColorBrush(new Color(31, 31, 31, 1)) };

            var itemsControl = new ItemsControl
                {
                    ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal }, 
                    ItemsSource = this.buttons, 
                    ItemTemplate = () =>
                        {
                            var button = new Button();
                            button.Click += ButtonOnClick;

                            var image = new Image { Stretch = Stretch.None, Margin = new Thickness(18, 0, 18, 0) };
                            image.Bind(DataContextProperty, BindingFactory.CreateOneWay<object>());

                            IObservable<ImageSource> imageBindingSource =
                                BindingFactory.CreateOneWay<ApplicationBarIconButton, ImageSource>(o => o.IconImageSource);

                            image.Bind(Image.SourceProperty, imageBindingSource);

                            button.Content = image;

                            return button;
                        }, 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            containingBorder.Child = itemsControl;

            this.Content = containingBorder;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
        }
    }
}