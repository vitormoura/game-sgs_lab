using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components
{
    public static class Constants
    {
        public const float M = 32.0f;
        public const float DEFAULT_FPS = 1.0f / 60.0f;
        public const float DEFAULT_G = (float)10 * M;

        public class SFXAssets
        {
            public const string ACHIEVEMENT = "sounds/sfx_achievement";
            public const string PLAYER_DEATH = "sounds/sfx_knockedout";
            public const string START = "sounds/sfx_startgame";
            public const string BOMB_EXPLOSION = "sounds/sfx_distant_explosion";
            public const string FALLING = "sounds/sfx_cartoon_fall";
        }

        public class SongAssets
        {
            public const string BG_SONG = "sounds/song_advdungeon";
            public const string BG_TITLESCREEN = "sounds/song_titlescreen";
            public const string CREDITS_SONG = "sounds/credits_song";
        }
    }
}
