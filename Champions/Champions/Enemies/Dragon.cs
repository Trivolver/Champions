using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Enemies
{
    /// <summary>
    /// Dragon enemy from the NPCBad base class.
    /// </summary>
    class Dragon:Entities.NPCBad
    {
        /// <summary>
        /// Constructor for the dragon. Initializes stats and creates basic entities.
        /// </summary>
        /// <param name="a_sheet">Texture to draw from.</param>
        /// <param name="a_position">Vector2 position to draw to.</param>
        /// <param name="a_mapPosition">Vector2 position of the map.</param>
        /// <param name="a_npcfont">SpriteFont to draw text with.</param>
        public Dragon(Texture2D a_sheet, Vector2 a_position, Vector2 a_mapPosition, SpriteFont a_npcfont)
        {
            /** Create a Dragon with the folowing stats:
             * 10 Strength, 5 Agility, 3 Speed, 10 Intelligence,
             * 100 Health, 0 Mana, At level 5, with 100 exp for a kill.
             **/
            CreateStats(10, 3, 5, 10, 100, 0, 100, 5, 100);
            CreateAbstracts();
            // Dragon will wander 200 pixels, detect from 500 pixels, and pursue for 1000 pixels
            SetNPCAttributes(300, 500, 1000, false, true, 0, 20);
            SetDrawAttributes(4, "Dragon");

            //Had to do this manually because the sprite sheet is created poorly.
            m_backwards = new Rectangle[m_frames];
            m_backwards[0] = new Rectangle(0, 292, 96, 80);
            m_backwards[1] = new Rectangle(108, 288, 72, 95);
            m_backwards[2] = new Rectangle(195, 294, 94, 95);
            m_backwards[3] = new Rectangle(305, 289, 70, 95);
            m_forwards = new Rectangle[m_frames];
            m_forwards[0] = new Rectangle(2, 12, 93, 79);
            m_forwards[1] = new Rectangle(111, 9, 66, 81);
            m_forwards[2] = new Rectangle(194, 12, 94, 79);
            m_forwards[3] = new Rectangle(306, 10, 67, 81);
            m_left = new Rectangle[m_frames];
            m_left[0] = new Rectangle(0, 105, 90, 67);
            m_left[1] = new Rectangle(97, 96, 87, 79);
            m_left[2] = new Rectangle(192, 100, 90, 70);
            m_left[3] = new Rectangle(293, 97, 86, 79);
            m_right = new Rectangle[m_frames];
            m_right[0] = new Rectangle(6, 201, 90, 67);
            m_right[1] = new Rectangle(104, 192, 87, 79);
            m_right[2] = new Rectangle(198, 196, 90, 70);
            m_right[3] = new Rectangle(299, 192, 86, 79);
            m_position = a_position;

            m_framePosition = m_forwards[0];

            MakeRectangle();
            SetOrigin(a_position, a_mapPosition);
            m_position = m_origin;
            Live();
            m_sheet = a_sheet;
            m_npcName = a_npcfont;
        }
    }
}
