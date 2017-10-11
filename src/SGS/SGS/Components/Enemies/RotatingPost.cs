using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using SGS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.Scenes;
using SGS.Components.World;

namespace SGS.Components.Enemies
{
    public class RotatingPost : DrawableGameObject
    {
        private const int QTD_NODES = 4;
        private const int NODE_SIZE = 32;
                        
                
        private Matrix rotation;
        private Matrix translate;
        private GameWorld world;

        private Texture2D spike;
        private RotatingNode[] nodes;
        private float speed;
                
        public RotatingPost(GameWorld world, Vector2 pos, float speed)
        {
            this.world = world;
            this.Position = pos;
            this.speed = speed;
            this.rotation = Matrix.Identity;
        }

        public override void Initialize()
        {
            this.nodes = new RotatingNode[QTD_NODES];

            for (int i = 1; i <= QTD_NODES; i++)
            {
                this.nodes[i - 1] = new RotatingNode(Vector2.One * (i * NODE_SIZE / 1.5f));
                this.world.RegisterCollisionActor(this.nodes[i - 1]);
            }
        }

        public override void LoadContent()
        {
            this.spike = GameManager.Content.Load<Texture2D>("sprites/spikeball");
        }

        public override void Update(GameTime t)
        {
            this.translate = Matrix.CreateTranslation(new Vector3(this.Position, 0));
            this.rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(360.0f * this.speed * (float)t.ElapsedGameTime.TotalSeconds));

            for (int i = 0; i < nodes.Length; i++)
            {
                this.nodes[i].Position = Vector2.Transform(this.nodes[i].Position, this.rotation );
                this.nodes[i].BoundingBox = new RectangleF( Vector2.Transform(this.nodes[i].Position, this.translate), new Size2(NODE_SIZE, NODE_SIZE));
            }
        }

        public override void Draw(SpriteBatch canvas)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                var pos = Vector2.Transform(nodes[i].Position, translate);
                canvas.Draw(this.spike, pos, Color.White);

                var centerPos = this.Position;
                centerPos.Y += NODE_SIZE / 2;
                centerPos.X += NODE_SIZE / 2;

                canvas.DrawPoint(centerPos, Color.Black, 10);
            }
        }
    }

    public class RotatingNode : DrawableGameObject, IActorTarget
    {
        public RectangleF BoundingBox
        {
            get;
            set;
        }
                
        public Vector2 Velocity
        {
            get;
            set;
        }

        public RotatingNode(Vector2 pos)
        {
            this.Position = pos;
            this.Tag = Enemy.TAG;
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            
        }
    }
}
