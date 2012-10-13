using System;

namespace Asteroids
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameBase game = new GameBase())
            {
                game.Run();
            }
        }
    }
#endif
}

