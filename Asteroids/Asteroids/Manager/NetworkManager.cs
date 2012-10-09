using System;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace Asteroids
{
    class NetworkManager : Manager
    {
        private static NetworkManager instance;

        private NetworkSessionProperties sessionProperties;
        private NetworkSession session;

        private const int privateGamerSlots   = 0;
        private const int maximumPlayers      = 4;
        private const int maximumLocalPlayers = 1;

        private NetworkManager()
        {

        }

        public void HostGame()
        {
            Console.WriteLine("Hosting a game session");

            sessionProperties = new NetworkSessionProperties();

            // Create the game session
            session = NetworkSession.Create(NetworkSessionType.Local, maximumLocalPlayers, maximumPlayers, privateGamerSlots, sessionProperties);

            // Game Config
            session.AllowHostMigration = true;
            session.AllowJoinInProgress = true;

            session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(onGamerJoinSession);
        }

        public bool CanHost()
        {
            if (Gamer.SignedInGamers.Count == 0)
            {
                Guide.ShowSignIn(maximumLocalPlayers, false);
                
                return false;
            }
            return true;
        }

        public void onGamerJoinSession(object sender, GamerJoinedEventArgs e)        
        {
            Console.WriteLine("A gamer has joined the session");
        }

        #region singleton
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkManager();
                }
                return instance;
            }
        }
        #endregion
    }
}
