namespace XpfSamples.S02ApplicationBar
{
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
            var itemsControl = new ItemsControl
                {
                    ItemsPanel = new StackPanel { Orientation = Orientation.Horizontal }, 
                    ItemsSource = this.buttons, 
                    ItemTemplate =
                        () =>
                            {
                                var border = new Border
                                    {
                                        Height = 100,
                                        Width = 100,
                                        Margin = new Thickness(10),
                                        VerticalAlignment = VerticalAlignment.Bottom
                                    };

                                border.Bind(Border.BackgroundProperty, BindingFactory.CreateOneWay((ApplicationBarIconButton)this.DataContext, ApplicationBarIconButton.ColorProperty));

                                return border;
                            }, 
                    HorizontalAlignment = HorizontalAlignment.Center
                };

            this.Content = itemsControl;
        }
    }

    public class ApplicationBarIconButton : DependencyObject
    {
        public ApplicationBarIconButton(SolidColorBrush color)
        {
            this.Color = color;
        }

        public static readonly Property<Brush, ApplicationBarIconButton> ColorProperty =
            Property<Brush, ApplicationBarIconButton>.Register("Color");

        public Brush Color
        {
            get
            {
                return this.GetValue(ColorProperty);
            }
            
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }
    }
}