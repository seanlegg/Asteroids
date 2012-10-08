using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class MainMenuScreen : GameScreen
    {
        private SpriteFont textFont;

        private List<Text> menuOptions;

        private int menuSelection;

        public MainMenuScreen(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            textFont = content.Load<SpriteFont>("font/Menu");

            menuOptions = new List<Text>();
            menuOptions.Add(new Text("New Game", textFont));
            menuOptions.Add(new Text("Quit",     textFont));

            menuSelection = 0;
        }

        public override void Update(GameTime dt)
        {
            if (InputManager.Instance.IsKeyPressed(Keys.Up) || InputManager.Instance.IsButtonPressed(Buttons.DPadUp))
            {
                menuSelection--;
                if (menuSelection < 0)
                {
                    menuSelection = menuOptions.Count - 1;
                }
            }

            if (InputManager.Instance.IsKeyPressed(Keys.Down) || InputManager.Instance.IsButtonPressed(Buttons.DPadDown))
            {
                menuSelection++;
                if (menuSelection > menuOptions.Count - 1)
                {
                    menuSelection = 0;
                }
            }

            if (InputManager.Instance.IsKeyPressed(Keys.Enter) || InputManager.Instance.IsButtonPressed(Buttons.A))
            {
                screenEvent.Invoke(this, new MenuEvent(MenuEvent.MenuItem.NEW_GAME));
            }
            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    Text option = menuOptions[i];

                    spriteBatch.DrawString(textFont, option.title, new Vector2(option.center.X, option.size.Y * i), menuSelection == i ? Color.Green : Color.DarkGray);
                }                
            }
            spriteBatch.End();
        }

    }
}
