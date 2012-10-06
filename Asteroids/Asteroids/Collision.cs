using Microsoft.Xna.Framework;

namespace Asteroids
{
    class Collision
    {
        public static bool AABB(Collidable a, Collidable b)
        {
            return false;
        }

        public static bool OBB(Collidable a, Collidable b)
        {
            return false;
        }

        public static bool BoundingSphere(Collidable a, Collidable b)
        {
            BoundingSphere objA = new BoundingSphere(a.GetPosition(), a.GetRadius());
            BoundingSphere objB = new BoundingSphere(b.GetPosition(), b.GetRadius());

            return objA.Intersects(objB);
        }

        public static bool Intersects(Collidable a, Collidable b)
        {
            return false;
        }
    }
}
