using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids
{
    /*
     * See: http://msdn.microsoft.com/en-us/library/ff604978.aspx and http://msdn.microsoft.com/en-us/library/ff604979.aspx
     */
    public class Config
    {
        public int ScreenWidth;
        public int ScreenHeight;
        public bool IsFullScreen;
        public bool PreferMultiSampling;

        public KeyboardControls Keyboard = new KeyboardControls();
        public GamepadControls  GamePad  = new GamepadControls();
    }

    public class ControlScheme 
    {
        public String Thrust;
        public String Left;
        public String Right;
        public String Action;  
    }

    public class KeyboardControls : ControlScheme
    {

    }

    public class GamepadControls : ControlScheme
    {

    }
}
