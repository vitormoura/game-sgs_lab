using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Shapes;
using SGS.Components.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled.Graphics;
using SGS.Components.Enemies;
using SGS.Components.Scenes;
using Microsoft.Xna.Framework.Graphics;
using SGS.Components.Cameras;

namespace SGS.Components.World
{
    public class GameWorld : DrawableGameObject
    {
        private const String OBJS_REGIONS = "Regions";
        private const String OBJS_ENEMIES = "Enemies";

        private const String REGION_TP_GROUNDEFFECT = "GroundEffect";
        private const String REGION_TP_GOAL = "Goal";
        private const String REGION_TP_START = "Start";

        

        private const String LAYER_COLLISION = "Collision";
        private const String LAYER_PLAYABLE = "PlayableZone";

        private String mapName;
        private TiledMap map;
        private TiledMapRenderer renderer;
        private IEnumerable<GameWorldArea> playableZones;

        private IEnumerable<GameWorldArea> specialAreas;
        private IEnumerable<DrawableGameObject> enemies;
        
        private CollisionWorld collisionWorld;
        private CollisionGrid collisionGrid;
        private List<IActorTarget> collisionActors;

#if DEBUG
        private IVisualDebugService debug;
        
#endif

        public GameWorldArea Goal
        {
            get;
            private set;
        }

        public GameWorldArea Start
        {
            get;
            private set;
        }
       

        public IEnumerable<GameWorldArea> SpecialAreas
        {
            get { return this.specialAreas; }
        }

        public GameWorld(String mapAssetName)
        {
            this.mapName = mapAssetName;
        }

        public override void LoadContent()
        {
            this.map = GameManager.Content.Load<TiledMap>(this.mapName);
            this.Bounds = new Rectangle(0, 0, this.map.WidthInPixels, this.map.HeightInPixels);
            this.renderer = new TiledMapRenderer(GameManager.GraphicsDevice);
            this.collisionWorld = new CollisionWorld(Vector2.Zero);
            
            var collisionLayer = this.map.GetLayer<TiledMapTileLayer>(LAYER_COLLISION);


            if (collisionLayer != null)
                PrepareCollisionGrid(collisionLayer);

            this.collisionActors = new List<IActorTarget>();

#if DEBUG
            this.debug = GameManager.Services.GetService<IVisualDebugService>();
            
#endif
            this.PrepareSpecialAreas();
            this.PrepareEnemiesList();
            this.PreparePlayableZones();
        }

        private void PreparePlayableZones()
        {
            var playableLayer = this.map.GetLayer<TiledMapObjectLayer>(LAYER_PLAYABLE);

            if (playableLayer != null)
            {
                this.playableZones = this.map.GetLayer<TiledMapObjectLayer>(LAYER_PLAYABLE).Objects.Select(x =>
                {
                    return new GameWorldArea(x);
                }).ToList();
            }
        }

        public override void Unload()
        {
            this.specialAreas = null;
            this.collisionActors.Clear();
        }

        public override void Initialize()
        {
            foreach (var e in this.enemies)
                e.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.renderer.Update(this.map, gameTime);

            foreach (var e in this.enemies)
            {
                if (e.Enabled)
                    e.Update(gameTime);
            }

            

            this.collisionWorld.Update(gameTime);
            
            for (int i = 0; i < this.collisionActors.Count; i++)
            {
                var current = this.collisionActors[i];

                for (int j = (i + 1); j < this.collisionActors.Count; j++)
                {
                    var other = this.collisionActors[j];
                    var intersection = RectangleF.Intersect(current.BoundingBox, other.BoundingBox);

                    if (intersection.IsEmpty)
                        continue;

                    current.OnCollision(new ActorsCollisionInfo(other, current.BoundingBox.Center - other.BoundingBox.Center));
                    other.OnCollision(new ActorsCollisionInfo(current, other.BoundingBox.Center - current.BoundingBox.Center));
                }
            }

            //Caso o player não esteja presente em nenhuma playablezone vamos destruí-lo
            if (this.playableZones != null)
            {
                if (GameManager.CurrentScene.Player.State.Alive && !this.playableZones.Any(x => x.Intersects(GameManager.CurrentScene.Player)))
                    GameManager.CurrentScene.Player.FallIntoVoid();
            }
                        
#if DEBUG
            this.debug.AddShapes(this.SpecialAreas.Select(x => ((IShapeF)x.Bounds)));
            this.debug.AddShapes(this.collisionActors.Select(x => (IShapeF)x.BoundingBox), Color.Green);
            this.debug.AddShape(this.Goal.Bounds, Color.Gold);
            this.debug.AddShape(this.Start.Bounds, Color.Fuchsia);

            if (this.collisionGrid != null)
            {
                this.debug.AddShapes(this.collisionGrid.GetCollidables(new RectangleF(0, 0, this.map.WidthInPixels, map.HeightInPixels)).Select(x => (IShapeF)x.BoundingBox), Color.Green);
            }
#endif

        }

        public override void Draw(SpriteBatch canvas)
        {
            foreach (var e in this.enemies)
            {
                if (e.Visible)
                    e.Draw(canvas);
            }

            //RectangleF cameraBounds = GameManager.MainCamera.BoundingRectangle;
            Matrix viewMatrix = GameManager.MainCamera.GetViewMatrix();
            //Matrix projMatrix = Matrix.CreateOrthographicOffCenter(20, cameraBounds.Width, cameraBounds.Height, 0, -1f, 0f);
            
            this.renderer.Draw(this.map, viewMatrix: viewMatrix);
        }


        public void RegisterCollisionActor(IActorTarget actor)
        {
            var registredActor = this.collisionWorld.CreateActor(actor);
            this.collisionActors.Add(registredActor);
        }

        private void PrepareEnemiesList()
        {
            this.enemies = this.map.GetLayer<TiledMapObjectLayer>(OBJS_ENEMIES).Objects.Select(e =>
            {
                var param = EnemyCreationParams.Parse(e.Properties);
                    param.Position = e.Position;

                return EnemyFactory.Create(this, param);

            }).ToList();

            foreach (var e in this.enemies)
            {
                e.LoadContent();
            }
        }

        private void PrepareCollisionGrid(TiledMapTileLayer collisionLayer)
        {
            //Mapa de tiles colidíveis por padrão é 0 em todas as posições
            var collisionGridData = new int[this.map.Width * this.map.Height];

            //Só mudamos o valor da célula para os tiles colidíveis da camada informada
            foreach (var c in collisionLayer.Tiles)
            {
                collisionGridData[c.X + (c.Y * this.map.Width)] = c.GlobalIdentifier;
            }

            this.collisionGrid = this.collisionWorld.CreateGrid(collisionGridData, collisionLayer.Width, collisionLayer.Height, collisionLayer.TileWidth, collisionLayer.TileHeight);
        }

        private void PrepareSpecialAreas()
        {
            var areas = this.map.GetLayer<TiledMapObjectLayer>(OBJS_REGIONS).Objects.Select(x =>
            {
                //TODO: Criar tipos específicos de áreas

                if (x.Type == REGION_TP_GROUNDEFFECT)
                    return new GameWorldArea(x);
                
                else if (x.Type == REGION_TP_GOAL)
                    return new GameWorldArea(x);
                else if (x.Type == REGION_TP_START)
                    return new GameWorldArea(x);
                else
                    throw new ArgumentException("Invalid region type: " + x.Type);

            }).ToList();


            //Localizando ponto de finalização
            Int32 posGoalArea = areas.FindIndex(a => a.Name == REGION_TP_GOAL);

            if (posGoalArea >= 0)
            {
                this.Goal = areas[posGoalArea];
                areas.RemoveAt(posGoalArea);
            }

            //Localizando ponto de partida
            Int32 posPartida = areas.FindIndex(a => a.Name == REGION_TP_START);

            if (posPartida >= 0)
            {
                this.Start = areas[posPartida];
                areas.RemoveAt(posPartida);
            }

            this.specialAreas = areas;
        }
    }

    public class ActorsCollisionInfo : CollisionInfo
    {
        private ICollidable other;
        private Vector2 penetrationVector;

        public override ICollidable Other
        {
            get
            {
                return this.other;
            }
        }

        public override Vector2 PenetrationVector
        {
            get
            {
                return this.penetrationVector;
            }
        }

        public ActorsCollisionInfo(ICollidable other, Vector2 penetrationVector)
            : base()
        {
            this.other = other;
            this.penetrationVector = penetrationVector;
        }
    }
}

