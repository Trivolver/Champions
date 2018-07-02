using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Enemies
{
    /// <summary>
    /// This class represents the Zombie enemy in the game. 
    /// </summary>
    class Zombie : Entities.NPCBad
    {
        /// <summary>
        /// Constructor for the Zombie. Initializes texture, position and stats.
        /// </summary>
        /// <param name="a_sheet">Texture to draw the zombie from.</param>
        /// <param name="a_position">Vector2 position to draw zombie to.</param>
        /// <param name="a_mapPosition">Vector2 position of the map.</param>
        /// <param name="a_npcfont">SpriteFont to use to draw text above the zombie.</param>
        public Zombie(Texture2D a_sheet, Vector2 a_position, Vector2 a_mapPosition, SpriteFont a_npcfont)
        {
            /** Create a Zombie with the folowing stats:
             * 5 Strength, 1 Agility, 1 Speed, 1 Intelligence,
             * 20 Health, 0 Mana, At level 1, with 5 exp for a kill.
             **/
            CreateStats(5, 1, 1, 1, 20, 0, 0, 1, 5);
            CreateAbstracts();
            // Zombie will wander 200 pixels, detect from 500 pixels, and pursue for 1000 pixels
            SetNPCAttributes(200, 200, 1000, false, true, 0, 20);
            SetDrawAttributes(4, "Zombie");

            m_framePosition = new Rectangle(13, 0, 41, 64);
            //Had to do this manually because the sprite sheet is created poorly.
            m_backwards = new Rectangle[m_frames];
            m_backwards[0] = new Rectangle(22, 1, 28, 65);
            m_backwards[1] = new Rectangle(84, 0, 28, 64);
            m_backwards[2] = new Rectangle(152, 2, 26, 62);
            m_backwards[3] = new Rectangle(212, 0, 28, 64);
            m_forwards = new Rectangle[m_frames];
            m_forwards[0] = new Rectangle(208, 64, 31, 64);
            m_forwards[1] = new Rectangle(14, 67, 29, 61);
            m_forwards[2] = new Rectangle(80, 64, 31, 64);
            m_forwards[3] = new Rectangle(142, 66, 27, 62);
            m_left = new Rectangle[m_frames];
            m_left[0] = new Rectangle(13, 129, 34, 63);
            m_left[1] = new Rectangle(75, 128, 35, 64);
            m_left[2] = new Rectangle(138, 129, 41, 63);
            m_left[3] = new Rectangle(203, 128, 35, 64);
            m_right = new Rectangle[m_frames];
            m_right[0] = new Rectangle(17, 192, 34, 64);
            m_right[1] = new Rectangle(82, 192, 35, 64);
            m_right[2] = new Rectangle(141, 193, 41, 63);
            m_right[3] = new Rectangle(210, 193, 35, 64);
            m_position = a_position;

            MakeRectangle();
            SetOrigin(a_position, a_mapPosition);
            m_position = m_origin;
            Live();
            m_sheet = a_sheet;
            m_npcName = a_npcfont;
        }
    }
}
