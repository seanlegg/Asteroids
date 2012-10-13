using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class MultiplayerMenuScreen : MenuScreen
    {
        public MultiplayerMenuScreen(ContentManager content)
        {
            textFont = content.Load<SpriteFont>("font/Menu");

            menuOptions = new List<MenuOption>();
            menuOptions.Add(new MenuOption("Create Game", EventType.HOST_MULTIPLAYER_GAME, textFont));
            menuOptions.Add(new MenuOption("Find Games",  EventType.FIND_MULTIPLAYER_GAME, textFont));
            menuOptions.Add(new MenuOption("Back",        EventType.NAVIGATE_MAIN_MENU,    textFont));
        }

        public override void Update(GameTime dt)
        {
            InputManager input = InputManager.Instance;

            if (input.IsKeyPressed(Keys.Escape) || input.IsButtonPressed(Buttons.B))
            {
                EventManager.Instance.Publish(new Event(EventType.NAVIGATE_MAIN_MENU));
            }

            if (input.IsKeyPressed(Keys.Up) || input.IsButtonPressed(Buttons.DPadUp))
            {
                menuSelection--;
                if (menuSelection < 0)
                {
                    menuSelection = menuOptions.Count - 1;
                }
            }

            if (input.IsKeyPressed(Keys.Down) || input.IsButtonPressed(Buttons.DPadDown))
            {
                menuSelection++;
                if (menuSelection > menuOptions.Count - 1)
                {
                    menuSelection = 0;
                }
            }

            if (input.IsKeyPressed(Keys.Enter) || input.IsButtonPressed(Buttons.A))
            {            
                GetMenuSelection();
                Reset();
            }
            base.Update(dt);
        }

        public void Reset()
        {
            menuSelection = 0;
        }

        public override void GetMenuSelection()
        {
            EventManager.Instance.Publish(new Event(menuOptions[menuSelection].type));
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
