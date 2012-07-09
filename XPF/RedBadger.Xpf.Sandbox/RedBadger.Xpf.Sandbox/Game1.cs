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

namespace RedBadger.Xpf.Sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using NekoCake.Crimson.Xpf;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    using Color = Microsoft.Xna.Framework.Color;

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private readonly List<NinePatch> ninePatches = new List<NinePatch>();

        private ObservableCollection<Chunk> chunks;

        private SpriteFontAdapter font;

        private RootElement root;

        private SpriteBatchAdapter spriteBatch;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferHeight = 900;
            this.graphics.PreferredBackBufferWidth = 1400;
            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.root.Draw();
            base.Draw(gameTime);
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var primitiveService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatch, primitiveService);
            var input = new InputManager();
            this.root = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, input);

            this.font = new SpriteFontAdapter(this.Content.Load<SpriteFont>(@"SpriteFont"));
            this.chunks = new ObservableCollection<Chunk>();

            string[] files = Directory.GetFiles(Environment.CurrentDirectory + @"\Content\Textures");

            foreach (string file in files)
            {
                var chunk = new Chunk
                    {
                        Name = Path.GetFileNameWithoutExtension(file), 
                        Texture = this.Content.Load<Texture2D>(@"Textures/" + Path.GetFileNameWithoutExtension(file))
                    };
                this.chunks.Add(chunk);
            }

            var items = new ItemsControl
                {
                    ItemTemplate = _ =>
                        {
                            var textBlock = new TextBlock(this.font) { Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center };
                            textBlock.Bind(
                                TextBlock.TextProperty, BindingFactory.CreateOneWay<Chunk, string>(o => o.Name));

                            var image = new Image { Stretch = Stretch.Fill, Width = 100, };
                            image.Bind(
                                Image.SourceProperty, BindingFactory.CreateOneWay<Chunk, ImageSource>(o => o.XnaImage));
                            
                            var panel = new StackPanel
                                {
                                    Orientation = Orientation.Vertical, 
                                    Background = new SolidColorBrush(new Media.Color(0, 0, 0, 100)), 
                                };

                            panel.Children.Add(image);
                            panel.Children.Add(textBlock);

                            var border = new Border
                                {
                                    BorderBrush = new SolidColorBrush(Colors.Black), 
                                    BorderThickness = new Thickness(2, 2, 2, 2), 
                                    Margin = new Thickness(5, 5, 5, 5), 
                                    Child = panel, 
                                };

                            var button = new Button { Content = border, Margin = new Thickness(5, 5, 5, 5), };

                            return button;
                        }, 
                    ItemsSource = this.chunks, 
                };

            items.ItemsPanel.Margin = new Thickness(0, 0, 25, 0);

            var scrollViewer = new ScrollViewer { Content = items };

            var canvas = new Canvas { };

            var chunkPallet = new NinePatch(this.Content, canvas, "Chunk Pallet", this.font)
                {
                   Width = 280, Height = 550, 
                };
            this.ninePatches.Add(chunkPallet);

            chunkPallet.Children.Add(scrollViewer);
            canvas.Children.Add(chunkPallet);

            Grid.SetColumn(scrollViewer, 1);
            Grid.SetRow(scrollViewer, 1);

            Canvas.SetLeft(chunkPallet, 740);
            Canvas.SetTop(chunkPallet, 20);

            this.root.Content = canvas;
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name = "gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.root.Update();
            foreach (NinePatch a in this.ninePatches)
            {
                a.Update();
            }

            base.Update(gameTime);
        }

        public class Chunk
        {
            public string Name { get; set; }

            public Texture2D Texture { get; set; }

            public TextureImage XnaImage
            {
                get
                {
                    return new TextureImage(new Texture2DAdapter(this.Texture));
                }
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}
