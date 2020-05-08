using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gamelib.Common;
using FreeWorms.Environment;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FreeWorms.ContentManager
{
    public class GameContent
    {

        public DIRManager DIRManager;
        public EffectManager EffectManager;
        public ImageManager ImageManager;



        public GameContent(GraphicsDevice graphics)
        {
            DIRManager = new DIRManager();
            EffectManager = new EffectManager();
            ImageManager = new ImageManager(graphics);
            AnimManager.Config(DIRManager);

        }

        public void Init(Microsoft.Xna.Framework.Content.ContentManager content)
        {


            DIRManager.Init();
            AnimManager.Init(content);


        }

    }
}
