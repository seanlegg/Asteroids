using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    class ScreenManager : Manager
    {
        public  GameScreen currentScreen;

        private Game game;
        private SplashScreen splashScreen;
        private MainMenuScreen menuScreen;
        private GameOverScreen gameOverScreen;

        public ScreenManager(ContentManager content)
        {
            game           = new Game(content);
            splashScreen   = new SplashScreen(content);
            menuScreen     = new MainMenuScreen(content);
            gameOverScreen = new GameOverScreen(content);
            
            // Set the currently active screen
            currentScreen = splashScreen;

            // Subscribe to Events
            EventManager eventManager = EventManager.Instance;

            eventManager.Subscribe(EventType.NAVIGATE_MAIN_MENU, this);
            eventManager.Subscribe(EventType.NEW_GAME,           this);
            eventManager.Subscribe(EventType.QUIT,               this);
            eventManager.Subscribe(EventType.GAME_OVER,          this);
        }

        public override void OnEvent(Event e)
        {
            switch (e.EventType)
            {
                case EventType.NAVIGATE_MAIN_MENU:
                    {
                        currentScreen = menuScreen;
                    }
                    break;
                case EventType.NEW_GAME:
                    {
                        currentScreen = game;
                    }
                    break;
                case EventType.GAME_OVER:
                    {
                        currentScreen = gameOverScreen;
                    }
                    break;
                case EventType.QUIT:
                    {
                        AsteroidsGame.isRunning = false;
                    }
                    break;
            }
        }
    }
}
