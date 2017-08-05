using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace RunPower
{
    internal class TextureManager
    {

        Dictionary<string, Texture2D> _textures;
        Dictionary<string, Animation> _animationDefinitions;
        ContentManager _content;

        public TextureManager(ContentManager content )
        {
            _textures = new Dictionary<string, Texture2D>();
            _content = content;
            _animationDefinitions = new Dictionary<string, Animation>();
          
        }

        public void AddTexture(string name, string path)
        {
            // load and add texture 
            _textures.Add(name, _content.Load<Texture2D>(path));
        }

        public void AddAnimation(string name, string textureName, Vector2 origin, float rotation,
            float scale, float depth, int frameCount, int framesPerSec)
        {            
            _animationDefinitions.Add(name, new Animation(textureName, origin, rotation, scale, depth, frameCount, framesPerSec));
        }

        public Animation GetAnimation(string name)
        {
            return _animationDefinitions[name];
        }

        public Texture2D GetTexture(string name)
        {
            return _textures[name];
        }
    }
}
