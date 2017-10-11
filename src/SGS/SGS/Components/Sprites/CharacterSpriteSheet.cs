using SGS.Components;
using SGS.Components.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components.Sprites
{
    /// <summary>
    /// Estabelece uma configuração de spritesheet para personagens padrão
    /// </summary>
    public class CharacterSpriteSheet : SpriteSheet
    {
        public CharacterSpriteSheet(String spriteSheetName)
            : base(spriteSheetName, new SpriteSheetParams(0, 0, 832, 1408, 13, 22))
        {
        }        
    }
}
