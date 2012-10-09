using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    class Event
    {
        public enum Type
        {
            GAME_OVER
        }
        private Type type;

        public Event(Type t)
        {
            type = t;
        }

        public Type EventType
        {
            get { return type; }
        }
    }
}
