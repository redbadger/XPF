namespace XpfSamples.S03ApplicationBar
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Phone.Reactive;

    using RedBadger.Xpf;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;

    public class ApplicationBar : ContentControl
    {
        private readonly IList<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();

        private readonly ISubject<ApplicationBarIconButton> clicks = new Subject<ApplicationBarIconButton>();

        public IList<ApplicationBarIconButton> Buttons
        {
            get
            {
                return this.buttons;
            }
        }

        public IObservable<ApplicationBarIconButton> Clicks
        {
            get
            {
                return this.clicks;
            }
        }

        public override void OnApplyTemplate()
        {
            if (this.buttons.Count > 4)
            {
                throw new NotSupportedException("Too many buttons - the maximum is 4.");
            }

            this.Height = 70;

            var containingBorder = new Border { Background = new SolidColorBrush(new Color(31, 31, 31, 255)) };

            var itemsControl = new ItemsControl
                {
                    ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal }, 
                    ItemsSource = this.buttons,
                    ItemTemplate = dataContext =>
                        {
                            var image = new Image { Stretch = Stretch.None };
                            image.Bind(
                                Image.SourceProperty, 
                                BindingFactory.CreateOneWay<ApplicationBarIconButton, ImageSource>(
                                    iconButton => iconButton.IconImageSource));

                            var button = new Button { Content = image, Margin = new Thickness(18, 0, 18, 0) };

                            Observable.FromEvent<EventArgs>(
                                handler => button.Click += handler, handler => button.Click -= handler).Select(
                                    eventArgs => (ApplicationBarIconButton)((Button)eventArgs.Sender).DataContext).
                                Subscribe(this.clicks);

                            return button;
                        }, 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            containingBorder.Child = itemsControl;

            this.Content = containingBorder;
        }
    }
}