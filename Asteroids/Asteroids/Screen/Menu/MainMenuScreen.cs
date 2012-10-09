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

        private List<MenuOption> menuOptions;

        private int menuSelection;

        public MainMenuScreen(ContentManager content)
        {
            textFont = content.Load<SpriteFont>("font/Menu");

            menuOptions = new List<MenuOption>();
            menuOptions.Add(new MenuOption("New Game", EventType.NEW_GAME, textFont));
            menuOptions.Add(new MenuOption("Quit",     EventType.QUIT,     textFont));

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
                GetMenuSelection();
            }
            base.Update(dt);
        }

        public void GetMenuSelection()
        {
            MenuOption option = menuOptions[menuSelection];

            switch (option.type)
            {
                case EventType.NEW_GAME:
                    {
                        EventManager.Instance.Publish(new Event(EventType.NEW_GAME));
                    }
                    break;
                case EventType.QUIT:
                    {
                        EventManager.Instance.Publish(new Event(EventType.QUIT));
                    }
                    break;
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    MenuOption option = menuOptions[i];

                    spriteBatch.DrawString(textFont, option.title, new Vector2(option.center.X, option.size.Y * i), menuSelection == i ? Color.Green : Color.DarkGray);
                }                
            }
            spriteBatch.End();
        }

    }
}
