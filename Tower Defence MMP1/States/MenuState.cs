﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Buttons;
using Tower_Defence.Enums;

namespace Tower_Defence.States
{
    public class MenuState : State
    {
        #region Fields
        private Texture2D _menuBackground;
        private Texture2D _playButton;
        private Texture2D _settingsButton;
        private Texture2D _closeGameButton;
        private Texture2D _musicButton;
        private Texture2D _musicOffButton;
        private Texture2D _easyButton;
        private Texture2D _normalButton;
        private Texture2D _hardButton;
        private Texture2D _ropeSmall;
        private Difficulty _difficulty;
        private Song _titleSong;
        private SoundEffect _buttonSound;

        private Texture2D _mouseCursor;

        private SpriteFont _menuFont;

        private List<IGameParts> _gameParts;

        #endregion
        public MenuState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content) : base(game1, graphics, content)
        {

        }

        #region Methods
        public override void LoadContent()
        {
            _game1.IsMouseVisible = false;

            _menuBackground = _content.Load<Texture2D>("Background/menuBackground");
            _playButton = _content.Load<Texture2D>("MenuButtons/playButton");
            _closeGameButton = _content.Load<Texture2D>("MenuButtons/closeButton");
            _settingsButton = _content.Load<Texture2D>("MenuButtons/settingsButton");
            _ropeSmall = _content.Load<Texture2D>("MenuItems/ropeSmall");
            _menuFont = _content.Load<SpriteFont>("MenuFont/menuFont");
            _musicButton = _content.Load<Texture2D>("MenuButtons/musicButton");
            _musicOffButton = _content.Load<Texture2D>("MenuButtons/musicOffButton");
            _easyButton = _content.Load<Texture2D>("MenuButtons/easyButton");
            _normalButton = _content.Load<Texture2D>("MenuButtons/normalButton");
            _hardButton = _content.Load<Texture2D>("MenuButtons/hardButton");
            _mouseCursor = _content.Load<Texture2D>("MenuButtons/mouse");

            _buttonSound = _content.Load<SoundEffect>("MenuSound/buttonSound");

            _titleSong = _content.Load<Song>("MenuSound/titleSong");
            MediaPlayer.Play(_titleSong);
            MediaPlayer.IsRepeating = true;

            _difficulty = Difficulty.easy;

            MenuButton playButton = new MenuButton(_playButton, _menuFont)
            {
                Position = new Vector2(
                    Game1.ScreenWidth / 2 - _playButton.Width / 2,
                    Game1.ScreenHeight / 2 - _playButton.Height / 2)
            };

            playButton.menuButtonEventHandler += HandlePlayButtonClicked;

            MenuButton closeGameButton = new MenuButton(_closeGameButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _closeGameButton.Width, 0)
            };

            closeGameButton.menuButtonEventHandler += HandleCloseButtonClicked;

            MenuButton settingsButton = new MenuButton(_settingsButton, _menuFont)
            {
                Position = new Vector2(1300, 210)

            };

            settingsButton.menuButtonEventHandler += HandleSettingsButtonClicked;

            MenuButton musicButton = new MenuButton(_musicButton, _menuFont)
            {
                Position = new Vector2(10, 50),
                IsMusicButton = true,
                MusicIsOn = true

            };

            musicButton.musicButtonEventHandler += HandleMusicButtonClicked;

            _gameParts = new List<IGameParts>()
            {
                playButton,
                closeGameButton,
                settingsButton,
                musicButton
            };
        }

        public override void Update(GameTime gameTime)
        {
            

            foreach (IGameParts gamePart in _gameParts.ToArray())
            {
                gamePart.Update(gameTime, _gameParts);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBackground, new Vector2(0, 0), Color.White);

            foreach (IGameParts gamePart in _gameParts)
            {
                gamePart.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(_ropeSmall, new Vector2(1350, -70), Color.White);
            spriteBatch.Draw(_ropeSmall, new Vector2(1400, -70), Color.White);
            string text = $"Difficulty: {_difficulty}";
            spriteBatch.DrawString(_menuFont, text, new Vector2(Game1.ScreenWidth - 300, Game1.ScreenHeight - 90), Color.White);

            MouseState currentMouse = Mouse.GetState();
            spriteBatch.Draw(_mouseCursor, new Rectangle(currentMouse.X, currentMouse.Y, _mouseCursor.Width, _mouseCursor.Height), null, Color.White, -2.0f, new Vector2(0,0), SpriteEffects.None, 0f);
        }
        private void HandlePlayButtonClicked(bool clicked)
        {
            MediaPlayer.Stop();
            _game1.ChangeState(new GameState(_game1, _graphics, _content, _difficulty));
        }

        private void HandleCloseButtonClicked(bool clicked)
        {
            _game1.QuitGame();
        }

        private void HandleMusicButtonClicked(MenuButton menubutton, bool isOn)
        {
            _buttonSound.Play();
            if (isOn)
            {
                MediaPlayer.IsMuted = true;
                menubutton.Texture = _musicOffButton;
                menubutton.MusicIsOn = false;
                return;
            }
            MediaPlayer.IsMuted = false;
            menubutton.Texture = _musicButton;
            menubutton.MusicIsOn = true;
        }

        private void HandleSettingsButtonClicked(bool clicked)
        {
            _buttonSound.Play();
            MenuButton easyButton = null;
            MenuButton normalButton = null;
            MenuButton hardButton = null;
            if (clicked)
            {
                easyButton = new MenuButton(_easyButton, _menuFont)
                {
                    Position = new Vector2(1500, 400),
                    Difficulty = Difficulty.easy

                };
                easyButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                normalButton = new MenuButton(_normalButton, _menuFont)
                {
                    Position = new Vector2(1500, 600),
                    Difficulty = Difficulty.normal
                };
                normalButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                hardButton = new MenuButton(_hardButton, _menuFont)
                {
                    Position = new Vector2(1500, 800),
                    Difficulty = Difficulty.hard
                };
                hardButton.difficultyButtonEventHandler += HandleDifficultyButtonClicked;

                _gameParts.Add(easyButton);
                _gameParts.Add(normalButton);
                _gameParts.Add(hardButton);
                return;
            }
            foreach (IGameParts gamePart in _gameParts.ToArray())
            {
                if(gamePart is MenuButton)
                {
                    if(((MenuButton)gamePart).Difficulty == 0) { continue; }
                    _gameParts.Remove(gamePart);
                }
            }
        }
        
        private void HandleDifficultyButtonClicked(Difficulty difficulty)
        {
            
            _buttonSound.Play();
            this._difficulty = difficulty;

        }

        #endregion

    }
}
