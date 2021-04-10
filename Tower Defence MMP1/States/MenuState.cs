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
using Tower_Defence.Interfaces;

namespace Tower_Defence.States
{
    public class MenuState : State, IUnsubscribable
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
        private Texture2D _manualButton;
        private Texture2D _ropeSmall;
        private Texture2D _descriptionBubble;
        private Texture2D _title;
        private string _manualText = "Read the manual\nbefore your first\nbattle.";
        private Difficulty _difficulty;
        private Song _titleSong;
        private SoundEffect _buttonSound;

        private Vector2 zeroPosition = new Vector2(0, 0);
        private Vector2 _ropeSmallPosition_1 = new Vector2(1350, -70);
        private Vector2 _ropeSmallPosition_2 = new Vector2(1400, -70);
        private Vector2 _descriptionBubblePosition = new Vector2(250, 500);
        private Vector2 _descriptionBubbleTextPosition = new Vector2(330, 550);
        private Vector2 _difficultyTextPosition = new Vector2(Game1.ScreenWidth - 300, Game1.ScreenHeight - 90);
        private Rectangle _currentMouseRectangle = new Rectangle();

        private Texture2D _mouseCursor;

        private SpriteFont _menuFont;

        private List<IGameParts> _gameParts;

        #region Buttons
        MenuButton playButton;
        MenuButton closeGameButton;
        MenuButton settingsButton;
        MenuButton musicButton;
        MenuButton manualButton;
        #endregion

        #endregion
        public MenuState(Game1 game1, GraphicsDeviceManager graphics, ContentManager content, Difficulty difficulty) : base(game1, graphics, content)
        {
            _difficulty = difficulty;
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
            _manualButton = _content.Load<Texture2D>("MenuButtons/manualButton");
            _mouseCursor = _content.Load<Texture2D>("MenuButtons/mouse");
            _descriptionBubble = _content.Load<Texture2D>("MenuItems/descriptionBubble");
            _title = _content.Load<Texture2D>("MenuItems/title");

            _buttonSound = _content.Load<SoundEffect>("MenuSound/buttonSound");

            _titleSong = _content.Load<Song>("MenuSound/titleSong");
            MediaPlayer.Play(_titleSong);
            MediaPlayer.IsRepeating = true;

            playButton = new MenuButton(_playButton, _menuFont)
            {
                Position = new Vector2(
                    Game1.ScreenWidth / 2 - _playButton.Width / 2,
                    Game1.ScreenHeight / 2 - _playButton.Height / 2)
            };

            playButton.menuButtonEventHandler += HandlePlayButtonClicked;

            closeGameButton = new MenuButton(_closeGameButton, _menuFont)
            {
                Position = new Vector2(Game1.ScreenWidth - _closeGameButton.Width, 0)
            };

            closeGameButton.menuButtonEventHandler += HandleCloseButtonClicked;

            settingsButton = new MenuButton(_settingsButton, _menuFont)
            {
                Position = new Vector2(1300, 210)

            };

            settingsButton.menuButtonEventHandler += HandleSettingsButtonClicked;

            musicButton = new MenuButton(_musicButton, _menuFont)
            {
                Position = new Vector2(10, 50),
                IsMusicButton = true,
                MusicIsOn = true

            };

            musicButton.musicButtonEventHandler += HandleMusicButtonClicked;

            manualButton = new MenuButton(_manualButton, _menuFont)
            {
                Position = new Vector2(150, 800),

            };

            manualButton.menuButtonEventHandler += HandleManualButtonClicked;

            _gameParts = new List<IGameParts>()
            {
                playButton,
                closeGameButton,
                settingsButton,
                musicButton,
                manualButton
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
            spriteBatch.Draw(_menuBackground, zeroPosition, Color.White);
            spriteBatch.Draw(_title, new Vector2(200, 20), Color.White);

            foreach (IGameParts gamePart in _gameParts)
            {
                gamePart.Draw(gameTime, spriteBatch);
            }

            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_1, Color.White);
            spriteBatch.Draw(_ropeSmall, _ropeSmallPosition_2, Color.White);
            spriteBatch.Draw(_descriptionBubble, _descriptionBubblePosition,null, Color.White, 0f, zeroPosition, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_menuFont, _manualText, _descriptionBubbleTextPosition, Color.Black);
            string text = $"Difficulty: {_difficulty}";
            spriteBatch.DrawString(_menuFont, text, _difficultyTextPosition, Color.White);

            MouseState currentMouse = Mouse.GetState();
            _currentMouseRectangle.X = currentMouse.X;
            _currentMouseRectangle.Y = currentMouse.Y;
            _currentMouseRectangle.Width = _mouseCursor.Width;
            _currentMouseRectangle.Height = _mouseCursor.Height;

            spriteBatch.Draw(_mouseCursor, _currentMouseRectangle, null, Color.White, -2.0f, zeroPosition, SpriteEffects.None, 0f);
        }
        private void HandleManualButtonClicked(bool clicked)
        {
            Unsubscribe();
            _game1.ChangeState(new ManualState(_game1, _graphics, _content, _difficulty));
        }
        private void HandlePlayButtonClicked(bool clicked)
        {
            MediaPlayer.Stop();
            Unsubscribe();
            _game1.ChangeState(new GameState(_game1, _graphics, _content, _difficulty));
        }

        private void HandleCloseButtonClicked(bool clicked)
        {
            Unsubscribe();
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

        public void Unsubscribe()
        {
            playButton.menuButtonEventHandler -= HandlePlayButtonClicked;
            closeGameButton.menuButtonEventHandler -= HandleCloseButtonClicked;
            settingsButton.menuButtonEventHandler -= HandleSettingsButtonClicked;
            musicButton.musicButtonEventHandler -= HandleMusicButtonClicked;
            manualButton.menuButtonEventHandler -= HandleManualButtonClicked;
        }

        #endregion

    }
}
