using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class MenuScreen : GameScreen
    {
        protected SpriteFont       textFont;
        protected List<MenuOption> menuOptions;
        protected int              menuSelection;

        public virtual void GetMenuSelection () { }
    }
}
