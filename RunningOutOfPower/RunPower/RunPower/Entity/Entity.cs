using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;

namespace RunPower.Entity
{
    public abstract class Entity
    {
        public bool IsDestroyed { get; private set; }

        protected Entity()
        {
            IsDestroyed = false;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Destroy()
        {
            IsDestroyed = true;
        }
    }

    public class Player : Entity
    {
        private readonly AnimatedSprite _sprite;
        public RectangleF BoundingBox;
        private readonly IBulletFactory _bulletFactory;
        private float _fireCooldown;
        public Vector2 Direction => Vector2.UnitX.Rotate(Rotation);

        public Vector2 Position
        {
            get { return _sprite.Position; }
            set
            {
                _sprite.Position = value;
                BoundingBox = new RectangleF(_sprite.Position, _sprite.BoundingRectangle.Size);
                //BoundingBox = new RectangleF(_sprite.Position.X - (_sprite.BoundingRectangle.Width / 2), _sprite.Position.Y, _sprite.BoundingRectangle.Width, _sprite.BoundingRectangle.Height);
            }
        }
        public float Rotation
        {
            get { return _sprite.Rotation - MathHelper.ToRadians(90); }
            set { _sprite.Rotation = value + MathHelper.ToRadians(90); }
        }
        public Vector2 Velocity { get; set; }

        public Player(SpriteSheetAnimationFactory spriteFactory, IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
            _sprite = new AnimatedSprite(spriteFactory);
            //_sprite.Scale = Vector2.One * .05f;
            Position = new Vector2(10, 10);
            _sprite.Play("walksouth").IsLooping = true;
            
            _sprite.Position = Position;
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime;
            Velocity *= 0.98f;
            if (_fireCooldown > 0)
            {
                _fireCooldown -= deltaTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite);
        }

        public void Accelerate(float acceleration)
        {
            Velocity += Direction * acceleration;
        }

        public void LookAt(Vector2 point)
        {
            Rotation = (float)Math.Atan2(point.Y - Position.Y, point.X - Position.X);
        }

        public void Fire()
        {
            if (_fireCooldown > 0)
            {
                return;
            }

            var angle = Rotation + MathHelper.ToRadians(90);
            var muzzle1Position = Position + new Vector2(14, 0).Rotate(angle);
            var muzzle2Position = Position + new Vector2(-14, 0).Rotate(angle);
            _bulletFactory.Create(muzzle1Position, Direction, angle, 550f);
            _bulletFactory.Create(muzzle2Position, Direction, angle, 550f);
            _fireCooldown = 0.2f;
        }


    }
}
