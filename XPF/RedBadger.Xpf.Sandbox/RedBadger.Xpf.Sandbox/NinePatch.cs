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

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RedBadger.Xpf;
using RedBadger.Xpf.Adapters.Xna.Graphics;
using RedBadger.Xpf.Controls;
using RedBadger.Xpf.Media;
using RedBadger.Xpf.Media.Imaging;

namespace NekoCake.Crimson.Xpf
{
    public class NinePatch : Grid
    {
        private ContentManager content;
        private SpriteFontAdapter font;
        private Canvas canvas;
        private IElement element;
        private bool isHeaderClicked, isReasizeClicked;

        public IElement Content
        {
            get { return this.element; }
            set
            {
                this.Children.Remove(element);
                this.element = value;
                Grid.SetColumn(element, 1);
                Grid.SetRow(element, 1);
            }
        }

        public NinePatch(ContentManager content, Canvas canvas, string name, SpriteFontAdapter font)
        {
            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(16)});
            this.RowDefinitions.Add(new RowDefinition());
            this.RowDefinitions.Add(new RowDefinition {Height = new GridLength(16)});
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(16)});
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6)});

            this.content = content;
            this.canvas = canvas;

            this.font = font;

            BuildNinePatch(name);
        }

        public void Update()
        {
            var mouse = Mouse.GetState();
            if (isHeaderClicked)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + (mouse.X - oldMouse.X));
                Canvas.SetTop(this, Canvas.GetTop(this) + (mouse.Y - oldMouse.Y));
            }

            if (isReasizeClicked)
            {
                this.Width += (mouse.X - oldMouse.X);
                this.Height += (mouse.Y - oldMouse.Y);
            }

            if (isHeaderClicked || isReasizeClicked)
            {
                if (Canvas.GetLeft(this) < 0)
                {
                    Canvas.SetLeft(this, 0);
                }
                if (Canvas.GetTop(this) < 0)
                {
                    Canvas.SetTop(this, 0);
                }

                if (Canvas.GetLeft(this) + this.ActualWidth > canvas.RenderSize.Width)
                {
                    Canvas.SetLeft(this, canvas.RenderSize.Width - this.Width);
                }
                if (Canvas.GetTop(this) + this.ActualHeight > canvas.RenderSize.Height)
                {
                    Canvas.SetTop(this, canvas.RenderSize.Height - this.Height);
                }
            }

            if (mouse.RightButton == ButtonState.Pressed)
            {
                this.isHeaderClicked = this.isReasizeClicked = false;
            }

            oldMouse = mouse;
        }
        private MouseState oldMouse;

        private void BuildNinePatch(string name)
        {
            var c1 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/c1"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(c1, 0);
            Grid.SetRow(c1, 0);
            this.Children.Add(c1);
            var c2 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/c2"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(c2, 2);
            Grid.SetRow(c2, 0);
            this.Children.Add(c2);
            var c3 = new Button()
            {
                Content = new Grid() { }
            }; 
            c3.Content = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/c3"))),
                Stretch = Stretch.Fill
            };
            c3.Click += (sender, args) => { this.isReasizeClicked = !this.isReasizeClicked; };
            Grid.SetColumn(c3, 2);
            Grid.SetRow(c3, 2);
            this.Children.Add(c3);
            var c4 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/c4"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(c4, 0);
            Grid.SetRow(c4, 2);
            this.Children.Add(c4);

            var e1 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/e1"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(e1, 0);
            Grid.SetRow(e1, 1);
            this.Children.Add(e1);
            var e2 = new Button()
                         {
                             Content = new Grid() {}
                         };
            ((Grid)e2.Content).Children.Add(new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/e2"))),
                Stretch = Stretch.Fill
            });
            ((Grid)e2.Content).Children.Add(new TextBlock(font)
                                                {
                                                    Text = "[" + name + "]",
                                                    Foreground = new SolidColorBrush(Colors.White),
                                                    Margin = new Thickness(0,2),
                                                    HorizontalAlignment = HorizontalAlignment.Center
                                                });
            e2.Click += (sender, args) => { this.isHeaderClicked = !this.isHeaderClicked; };
            Grid.SetColumn(e2, 1);
            Grid.SetRow(e2, 0);
            this.Children.Add(e2);
            var e3 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/e3"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(e3, 2);
            Grid.SetRow(e3, 1);
            this.Children.Add(e3);
            var e4 = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/e4"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(e4, 1);
            Grid.SetRow(e4, 2);
            this.Children.Add(e4);
            var m = new Image
            {
                Source = new TextureImage(new Texture2DAdapter(content.Load<Texture2D>(@"UI/m"))),
                Stretch = Stretch.Fill
            };
            Grid.SetColumn(m, 1);
            Grid.SetRow(m, 1);
            this.Children.Add(m);
        }
    }
}

