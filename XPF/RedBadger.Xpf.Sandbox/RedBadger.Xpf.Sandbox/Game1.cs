namespace RedBadger.Xpf.Sandbox
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using RedBadger.Xpf.Adapters.Xna.Graphics;
    using RedBadger.Xpf.Adapters.Xna.Input;
    using RedBadger.Xpf.Controls;
    using RedBadger.Xpf.Data;
    using RedBadger.Xpf.Media;
    using RedBadger.Xpf.Media.Imaging;

    using Color = RedBadger.Xpf.Media.Color;

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private ObservableCollection<Chunk> chunks;

        private SpriteFontAdapter font;

        private RootElement root;

        private SpriteBatchAdapter spriteBatch;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 600;
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
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
                    ItemTemplate = () =>
                        {
                            var textBlock = new TextBlock(this.font) { Foreground = new SolidColorBrush(Colors.White) };
                            textBlock.Bind(TextBlock.TextProperty, BindingFactory.CreateOneWay<Chunk, string>(o => o.Name));

                            var image = new Image();
                            image.Bind(Image.SourceProperty, BindingFactory.CreateOneWay<Chunk, ImageSource>(o => o.XnaImage));
                            var panel = new StackPanel
                                {
                                    Orientation = Orientation.Vertical, 
                                    Background = new SolidColorBrush(new Color(0, 0, 0, 100)), 
                                };

                            panel.Children.Add(image);
                            panel.Children.Add(textBlock);

                            var border = new Border
                                {
                                    BorderBrush = new SolidColorBrush(Colors.Black), 
                                    BorderThickness = new Thickness(2, 2, 2, 2), 
                                    Margin = new Thickness(10, 10, 10, 10), 
                                    Child = panel
                                };

                            return border;
                        }, 
                    ItemsSource = this.chunks, 
                };

/*
            var scroll = new ScrollViewer
                {
                    CanHorizontallyScroll = true, 
                    CanVerticallyScroll = true, 
                    Margin = new Thickness(0, 0, 25, 0), 
                    IsEnabled = true, 
                    Content = items, 
                    MaxHeight = 600, 
                    Height = 600
                };
*/

            var grid = new Grid
                {
                    Height = 600, 
                    MaxHeight = 600, 
                    Background = new SolidColorBrush(new Color(0, 0, 0, 100)), 
                    RowDefinitions = {
                                        new RowDefinition() 
                                     }, 
                    ColumnDefinitions = {
                                           new ColumnDefinition() 
                                        }, 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Top
                };

            grid.Children.Add(items);

            this.root.Content = grid;
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