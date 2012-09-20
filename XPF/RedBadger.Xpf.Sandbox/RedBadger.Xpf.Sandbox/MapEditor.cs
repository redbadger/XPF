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

/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NekoCake.Crimson.Engine.Core.Assets;
using NekoCake.Crimson.Engine.Core2D.Scene;
using NekoCake.Crimson.Tool.MapEdit.Utilities;
using NekoCake.Crimson.Xpf;
using RedBadger.Xpf;
using RedBadger.Xpf.Adapters.Xna.Graphics;
using RedBadger.Xpf.Controls;
using RedBadger.Xpf.Data;
using RedBadger.Xpf.Media;
using RedBadger.Xpf.Media.Imaging;
using Color = Microsoft.Xna.Framework.Color;
using Point = RedBadger.Xpf.Point;

namespace NekoCake.Crimson.Tool.MapEdit
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class MapEditor : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatchAdapter spriteBatch;
        PrimitiveBatch primitiveBatch;
        SpriteFontAdapter font;
        RootElement root;

        KeyboardState keyboad, oldKeyboard;
        MouseState mouse, oldMouse;

        private List<NinePatch> ninePatches = new List<NinePatch>();
        private CrimsonScene2D scene;

        public MapEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        ObservableCollection<ChunkTemplate> chunks;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatchAdapter(new SpriteBatch(GraphicsDevice));
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            var primitiveService = new PrimitivesService(GraphicsDevice);
            var renderer = new Renderer(spriteBatch, primitiveService);
            var input = new InputManagerWindows();
            this.root = new RootElement(GraphicsDevice.Viewport.ToRect(), renderer, input);

            this.scene = new CrimsonScene2D(GraphicsDevice);
            this.scene.AssetManager = new AssetManager(this.Content);

            font = new SpriteFontAdapter(Content.Load<SpriteFont>(@"Fonts\DevFont"));
            chunks = new ObservableCollection<ChunkTemplate>();

            var files = Directory.GetFiles(Environment.CurrentDirectory + @"\Content\Textures", "*.areas");

            foreach (string file in files)
            {
                scene.AssetManager.LoadTexture(@"Textures/" + Path.GetFileNameWithoutExtension(file), Path.GetFileNameWithoutExtension(file));
                var crimsonTexture = scene.AssetManager.GetTexture(Path.GetFileNameWithoutExtension(file));
                foreach (string chunkName in crimsonTexture.Areas.Keys)
                {
                    var chunk = new ChunkTemplate(crimsonTexture, chunkName);
                    chunks.Add(chunk);
                }
            }

            var items = new ItemsControl
            {
                ItemTemplate = () =>
                {
                    var textBlock = new TextBlock(font)
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                    };
                    textBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<ChunkTemplate, string>(o => o.Name));

                    Image image = new Image()
                                      {
                                          Stretch = Stretch.Fill,
                                          Width = 100,
                                      };
                    image.Bind(Image.SourceProperty, BindingFactory.CreateOneWay<ChunkTemplate, ImageSource>(o => o.XpfImage));
                    StackPanel panel = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        Background = new SolidColorBrush(new RedBadger.Xpf.Media.Color(0, 0, 0, 100)),
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

                    var button = new Button()
                                     {
                                         Content = border,
                                         Margin = new Thickness(5, 5, 5, 5),
                                     };
                    button.Click += (sender, args) =>
                                        {
                                            var sprite = new CrimsonSprite();
                                            sprite.Texture = ((ChunkTemplate)((Button)sender).DataContext).CrimsonTexture;
                                            sprite.TextureArea = ((ChunkTemplate)((Button)sender).DataContext).Name;
                                            sprite.Position = new Vector2(100, 100);
                                            sprite.Layer = this.selectedLayer;
                                            this.scene.RegisterObject(sprite);
                                        };

                    return button;
                },
                ItemsSource = chunks,
            };

            items.ItemsPanel.Margin = new Thickness(0, 0, 25, 0);

            var canvas = new Canvas()
                                    {
                                    };


            var chunkPallet = new NinePatch(Content, canvas, "Chunk Pallet", font)
                                         {
                                             Width = 280,
                                             Height = 550,                                 
                                         };
            this.ninePatches.Add(chunkPallet);

            chunkPallet.Children.Add(items);
            canvas.Children.Add(chunkPallet);

            Grid.SetColumn(items, 1);
            Grid.SetRow(items, 1);

            Canvas.SetLeft(chunkPallet, 740);
            Canvas.SetTop(chunkPallet, 20);

            this.root.Content = canvas;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.scene.Update(gameTime);
            this.root.Update();

            mouse = Mouse.GetState();
            keyboad = Keyboard.GetState();

            if (selectionBuffer == null)
            {
                this.TryGetSelection();
            }
            else if (!TestForUnselect())
            {
                if (mouse.LeftButton == ButtonState.Pressed && potentialSelected.BoundingRectangle.Contains(new Vector2(mouse.X, mouse.Y)))
                {
                    Vector2 delta = new Vector2(mouse.X - oldMouse.X, mouse.Y - oldMouse.Y);
                    selectionBuffer.Position += delta;
                }
            }

            foreach (NinePatch a in ninePatches)
            {
                a.Update();
            }

            oldMouse = mouse;
            oldKeyboard = keyboad;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.scene.Draw(gameTime);
            this.DrawSelection();
            this.root.Draw();

            base.Draw(gameTime);
        }
    }
}
*/

