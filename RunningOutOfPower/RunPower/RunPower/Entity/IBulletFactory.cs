using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace RunPower.Entity
{
    public interface IBulletFactory
    {
        void Create(Vector2 position, Vector2 direction, float rotation, float speed);
    }

    public class BulletFactory : IBulletFactory
    {
        private readonly TextureRegion2D _bulletRegion;
        private readonly IEntityManager _entityManager;

        public BulletFactory(IEntityManager entityManager, TextureRegion2D bulletRegion)
        {
            _entityManager = entityManager;
            _bulletRegion = bulletRegion;
        }

        public void Create(Vector2 position, Vector2 direction, float rotation, float speed)
        {
            var velocity = direction * speed;
            var laser = new Laser(_bulletRegion, velocity)
            {
                Position = position,
                Rotation = rotation
            };
            _entityManager.AddEntity(laser);
        }
    }
}
