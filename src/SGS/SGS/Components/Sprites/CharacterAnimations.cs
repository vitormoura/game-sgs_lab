using Microsoft.Xna.Framework;
using System;
using D = System.Diagnostics.Debug;

namespace SGS.Components.Sprites
{
    /// <summary>
    /// Representação de um conjunto de animações padrão de um personagem criado
    /// a partir de um spritesheet padrão para personagens
    /// </summary>
    public class CharacterAnimations : GameObject
    {
        private const int
            WALK = 0,
            SPELLCAST = 1,
            STAND = 2,
            THRUST = 3,
            SLASH = 4,
            SHOOT = 5,
            HURT = 6,
            SPAWN = 7,
            FALL_VOID = 8;
                
        private const int 
            UP = 0, 
            RIGHT = 1,
            DOWN = 2, 
            LEFT = 3;
                
        private const float FACING_ANGLE_TRIGGER = 0.98f;

        private SpriteAnimation[,] animations;
        private CharacterSpriteSheet spriteSheet;
                
        public CharacterAnimations(CharacterSpriteSheet sheet)
        {
            D.Assert(sheet != null);

            this.spriteSheet = sheet;
        }
                
        public override void Initialize()
        {
            this.animations = new SpriteAnimation[9, 4];

            this.SetAnimation(WALK, UP, 8, 9);
            this.SetAnimation(WALK, LEFT, 9, 9);
            this.SetAnimation(WALK, DOWN, 10, 9);
            this.SetAnimation(WALK, RIGHT, 11, 9);

            this.SetAnimation(SPELLCAST, UP, 0, 7, false);
            this.SetAnimation(SPELLCAST, LEFT, 1, 7, false);
            this.SetAnimation(SPELLCAST, DOWN, 2, 7, false);
            this.SetAnimation(SPELLCAST, RIGHT, 3, 7, false);

            this.SetAnimation(SHOOT, UP, 16, 13, false);
            this.SetAnimation(SHOOT, LEFT, 17, 13, false);
            this.SetAnimation(SHOOT, DOWN, 18, 13, false);
            this.SetAnimation(SHOOT, RIGHT, 19, 13, false);

            this.SetAnimation(HURT, DOWN, 20, 6, false);

            this.SetAnimation(SPAWN, DOWN, 20, 6, false, true);
                        
            this.animations[STAND, UP] = new SpriteAnimation(new Sprite[] { this.animations[WALK, UP].GetFrame(0) });
            this.animations[STAND, LEFT] = new SpriteAnimation(new Sprite[] { this.animations[WALK, LEFT].GetFrame(0) });
            this.animations[STAND, DOWN] = new SpriteAnimation(new Sprite[] { this.animations[WALK, DOWN].GetFrame(0) });
            this.animations[STAND, RIGHT] = new SpriteAnimation(new Sprite[] { this.animations[WALK, RIGHT].GetFrame(0) });
        }

        /// <summary>
        /// Recupera referência a animação WALK na direção informada
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public SpriteAnimation Walk(Vector2 dir)
        {
            return this.animations[WALK, ConvertToDirIndex(dir)];
        }

        /// <summary>
        /// Recupera referência a animação WALK na direção informada
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public SpriteAnimation Stand(Vector2 dir)
        {
            return this.animations[STAND, ConvertToDirIndex(dir)];
        }

        public SpriteAnimation CastSpell(Vector2 dir)
        {
            return this.animations[SPELLCAST, ConvertToDirIndex(dir)];
        }

        public SpriteAnimation Shoot(Vector2 dir)
        {
            return this.animations[SHOOT, ConvertToDirIndex(dir)];
        }

        public SpriteAnimation Die()
        {
            return this.animations[HURT, DOWN];
        }

        public SpriteAnimation Spawn()
        {
            return this.animations[SPAWN, DOWN];
        }

        public SpriteAnimation FallIntoVoid()
        {
            if( this.animations[FALL_VOID, UP] == null)
                this.SetAnimation(FALL_VOID, UP, 21, 9, false);

            return this.animations[FALL_VOID, UP];
        }

        private int ConvertToDirIndex(Vector2 dir)
        {
            D.Assert(dir.X != 0 || dir.Y != 0);

            var facingAngle = Vector2.Dot(dir, new Vector2(0.0f, 1.0f));

            if (facingAngle <= -FACING_ANGLE_TRIGGER)
                return UP;
            else if (facingAngle >= FACING_ANGLE_TRIGGER)
                return DOWN;

            if (dir.X > 0)
                return RIGHT;
            else if(dir.X < 0)
                return LEFT;
            
            return -1;
        }

        private void SetAnimation(Int32 animationIndex, Int32 directionIndex, Int32 spriteSheetLine, Int32 qtdeFrames, Boolean loop = true, bool backwards = false)
        {
            this.animations[animationIndex, directionIndex] = new SpriteAnimation(this.spriteSheet.GetSpriteSequenceOfLine(spriteSheetLine, qtdeFrames, backwards), qtdeFrames, loop);
        }
    }
}
