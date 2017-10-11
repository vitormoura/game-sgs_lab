using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Tiled;
using SGS.Components.Players;

namespace SGS.Components.World
{
    public class GameWorldArea
    {
        private TiledMapObject details;
        private IShapeF bounds;
        private float degenerationRatio;
        private float speedModifier;
        private bool slippery;

        public bool Slippery
        {
            get { return this.slippery; }
        }

        public IShapeF Bounds
        {
            get { return this.bounds; }
        }

        public float SpeedModifier
        {
            get { return this.speedModifier; }
        }

        public float DegenerationRatio
        {
            get { return this.degenerationRatio; }
        }

        public Int32 ID { get; private set; }
        
        public String Name
        {
            get { return this.details.Name; }
        }
                        
        public GameWorldArea(TiledMapObject obj)
        {
            this.details = obj;

            this.ID = obj.Identifier;
            this.bounds = null;
                                    
            if ( obj is TiledMapPolygonObject )
            {
                var polyObj = (TiledMapPolygonObject)obj;
                var polygon = new PolygonF(polyObj.Points.Select( x => new Vector2(x.X, x.Y)));
                polygon.Offset(new Vector2(polyObj.Position.X, polyObj.Position.Y));

                this.bounds = polygon;
            }
            else if (obj is TiledMapRectangleObject)
            {
                var rectObj = (TiledMapRectangleObject)obj;
                this.bounds = new RectangleF((int)rectObj.Position.X, (int)rectObj.Position.Y, (int)rectObj.Size.Width, (int)rectObj.Size.Height);
            }
            else
                throw new ArgumentException("Invalid GameMapArea");
            

            if (obj.Properties != null)
            {
                String lifeDegen, speedModifier, slippery;

                if(obj.Properties.TryGetValue("life_degeneration", out lifeDegen))
                {
                    this.degenerationRatio = float.Parse(lifeDegen, CultureInfo.InvariantCulture);
                }

                if (obj.Properties.TryGetValue("speed_modifier", out speedModifier))
                {
                    this.speedModifier = float.Parse(speedModifier, CultureInfo.InvariantCulture);
                }
                else
                    this.speedModifier = 1.0f;

                if (obj.Properties.TryGetValue("slippery", out slippery))
                {
                    this.slippery = Boolean.Parse(slippery);
                }
            }
        }

        public Boolean Intersects(Player p)
        {
            return this.bounds.Contains(p.Position);
        }        
    }
}
