using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;

namespace Asteroids
{
    public class Base
    {
        public bool isActive;

        public virtual void Init    ()                        { }
        public virtual void Update  (GameTime gameTime)       { }
        public virtual void Draw    (SpriteBatch spriteBatch) { }
    }
}
