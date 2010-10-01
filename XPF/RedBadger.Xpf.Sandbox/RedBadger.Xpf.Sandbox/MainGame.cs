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

    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private readonly List<NinePatch> ninePatches = new List<NinePatch>();

        private SpriteFontAdapter font;

        private RootElement rootElement;

        private SpriteBatchAdapter spriteBatch;

        public MainGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferHeight = 300;
            this.graphics.PreferredBackBufferWidth = 300;
            this.IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.rootElement.Draw();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatchAdapter(new SpriteBatch(this.GraphicsDevice));
            var primitiveService = new PrimitivesService(this.GraphicsDevice);
            var renderer = new Renderer(this.spriteBatch, primitiveService);

            this.rootElement = new RootElement(this.GraphicsDevice.Viewport.ToRect(), renderer, new InputManager());

            this.font = new SpriteFontAdapter(this.Content.Load<SpriteFont>(@"SpriteFont"));

            var border = new Border
                {
                    Width = 100,
                    Height = 100,
                    Background = new SolidColorBrush(Colors.Brown),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Child = new Border { Width = 150 }
                };

            var stackPanel = new StackPanel { Width = 350, Children = { border } };

            var canvas = new Canvas { Children = { border } };
            Canvas.SetLeft(border, 250);

            this.rootElement.Content = canvas;
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

            this.rootElement.Update();
            foreach (NinePatch a in this.ninePatches)
            {
                a.Update();
            }

            base.Update(gameTime);
        }
    }
}