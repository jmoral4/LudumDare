using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;
using RunPower.Entity;

namespace RunPower
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class InvincibleGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;        
        private BitmapFont _font;
        private readonly EntityManager _entityManager;
        readonly FramesPerSecondCounter _fpsCounter = new FramesPerSecondCounter();
        private BulletFactory _bulletFactory;
        private Camera2D _camera;
        List<AnimatedSprite> _enemies; //test
        AnimatedSprite _playerSprite;
        Player _player;
        List<AnimatedSprite> _blocks;
        private ViewportAdapter _viewportAdapter;
        private Texture2D _backgroundTexture;
        //1152×648
        public InvincibleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _entityManager = new EntityManager();
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

        

            //_player = new Animation(Vector2.Zero, 0.0f, 1.0f, 0);

            //input listeners
            var gamepadListener = new GamePadListener(new GamePadListenerSettings());
            Components.Add(new InputListenerComponent(this, gamepadListener));

            gamepadListener.ThumbStickMoved += GamepadListener_ThumbStickMoved;
               gamepadListener.ButtonDown += GamepadListenerOnButtonDown;            
            _enemies = new List<AnimatedSprite>();
            _blocks = new List<AnimatedSprite>();

            base.Initialize();

            //BRB
        }


        protected override void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            _font = Content.Load<BitmapFont>("Fonts/montserrat-32");
            _camera = new Camera2D(_viewportAdapter);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundTexture = Content.Load<Texture2D>("Textures/bg");

            var bulletTexture = Content.Load<Texture2D>("Textures/laserBlue03");
            var bulletRegion = new TextureRegion2D(bulletTexture);
            _bulletFactory = new BulletFactory(_entityManager, bulletRegion);

            SpawnPlayer(_bulletFactory);


          
            var block = Content.Load<Texture2D>("Textures/brick");
            var blockAtlas = TextureAtlas.Create("Animations/brick", block, 64, 64);
            var blockFactory = new SpriteSheetAnimationFactory(blockAtlas);
            blockFactory.Add("idle", new SpriteSheetAnimationData(new []{0}));
          

            ////create 10 enemies
            //Random r = new Random();


            //for(int i =0; i < 10; i++)
            //{
            //    _enemies.Add(new AnimatedSprite(p1Animations));
            //    _enemies[i].Color = Color.Red;
            //    _enemies[i].Play("walksouth").IsLooping = true;
            //    _enemies[i].Position = new Vector2(r.Next(50,500), r.Next(200,500));
            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    _blocks.Add(new AnimatedSprite(blockFactory));
            //    _blocks[i].Color = SharedColors[r.Next(0, 3)];
            //    _blocks[i].Play("idle");
            //    _blocks[i].Position = new Vector2(r.Next(20, 600), r.Next(20, 600));
            //}








            // TODO: use this.Content to load your game content here
        }

        public void SpawnPlayer(IBulletFactory factory)
        {

            var p1Texture = Content.Load<Texture2D>("Textures/majormustache");
            var p1atlas = TextureAtlas.Create("Animations/Player1", p1Texture, 64, 64);
            var p1Animations = new SpriteSheetAnimationFactory(p1atlas);
            p1Animations.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            p1Animations.Add("walksouth", new SpriteSheetAnimationData(new[] { 0, 1, 2 }));
            _player = _entityManager.AddEntity(new Player(p1Animations, factory));            
            
        }


        Color[] SharedColors = new Color[]{Color.Red, Color.Blue, Color.Brown, Color.Purple};

        void GamepadListenerOnButtonDown(object sender, GamePadEventArgs gamePadEventArgs)
        {
            if ((gamePadEventArgs.Button & Buttons.X) != 0)
            {
                //_playerSprite.Play("idle");
            }
            //throw new NotImplementedException();
        }

        private void GamepadListener_ThumbStickMoved(object sender, GamePadEventArgs e)
        {

            

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
       

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        GamePadState _previousGamePadState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var deltaTime = gameTime.GetElapsedSeconds();
            var gamepadState = GamePad.GetState(PlayerIndex.One);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X == 0 &&
                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y == 0)
            {

            }

            const float acceleration = 5f;
            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                _player.Accelerate(acceleration);

            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                _player.Accelerate(-acceleration);

            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))               
                _player.Rotation -= deltaTime * 15f;

            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
                _player.Rotation += deltaTime * 15f;


            if (gamepadState.IsButtonDown(Buttons.RightTrigger))
                _player.Fire();

            //if (_previousGamePadState.ThumbSticks.Left.X != gamepadState.ThumbSticks.Left.X || _previousGamePadState.ThumbSticks.Left.Y != gamepadState.ThumbSticks.Left.Y)
            //    _player.LookAt(_camera.ScreenToWorld(new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y)));

            _camera.LookAt(_player.Position + _player.Velocity * 0.2f);
            _camera.Zoom = 1.0f - _player.Velocity.Length() / 500f;


            _entityManager.Update(gameTime);

            _previousGamePadState = gamepadState;
                      

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            var sourceRectangle = new Rectangle(0, 0, _viewportAdapter.VirtualWidth, _viewportAdapter.VirtualHeight);
            sourceRectangle.Offset(_camera.Position * new Vector2(0.1f));

            _spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, sourceRectangle, Color.White);
            _spriteBatch.DrawString(_font, $"V:{_player.Velocity} Pos:{_player.Position.X},{_player.Position.Y} R:{_player.Rotation} ", Vector2.One, Color.White);
            _spriteBatch.End();

            // entities
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            _entityManager.Draw(_spriteBatch);
            _spriteBatch.End();

            //camera
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
