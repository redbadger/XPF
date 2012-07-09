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

namespace XpfSamples.S03ApplicationBar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
