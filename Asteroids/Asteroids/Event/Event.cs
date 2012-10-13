using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    public enum EventType
    {
        // Screen Events
        //ACTIVATE,
        //DEACTIVATE,

        // Menus
        NAVIGATE_SPLASH_SCREEN,
        NAVIGATE_MAIN_MENU,
        NAVIGATE_MULTIPLAYER_MENU,

        // Multiplayer
        FIND_MULTIPLAYER_GAME,
        HOST_MULTIPLAYER_GAME,

        // Game States
        NEW_GAME,
        GAME_OVER,
        QUIT
    }

    public class Event
    {
        private EventType type;

        public Event(EventType t)
        {
            type = t;
        }

        public EventType EventType
        {
            get { return type; }
        }
    }
}
