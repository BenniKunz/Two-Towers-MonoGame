﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tower_Defence.States;

namespace Tower_Defence
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private State _currentState;
        private State _nextState;

        public static int ScreenWidth;
        public static int ScreenHeight;

        public void ChangeState(State state)
        {
            _currentState = state;
            _currentState.LoadContent();
        }

        public void QuitGame()
        {
            this.Exit();
        }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            
            
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window

            _graphics.ApplyChanges();

            _currentState = new MenuState(this, _graphics, Content);
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState.LoadContent();

        }

        protected override void Update(GameTime gameTime)
        {

            _currentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _currentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
