using System;

namespace Asteroids
{
    public class MenuEvent : Event
    {
        public enum MenuItem
        {
            NEW_GAME,
            QUIT
        };
        private MenuItem selection;

        public MenuEvent(MenuItem item)
        {
            selection = item;
        }

        public MenuItem Selection
        {
            get { return selection; }
        }
    }
}
