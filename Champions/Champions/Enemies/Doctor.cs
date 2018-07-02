using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Enemies
{
    /// <summary>
    /// The Doctor enemy from NPCBad base class. 
    /// </summary>
    class Doctor:Entities.NPCBad
    {
        /// <summary>
        /// The constructor for the doctor. Creates it with specified parameters.
        /// </summary>
        /// <param name="a_sheet">Texture to draw the doctor from.</param>
        /// <param name="a_position">Position to draw the doctor.</param>
        /// <param name="a_mapPosition">Map position to draw to.</param>
        /// <param name="a_npcfont">The SpriteFont object to draw with.</param>
        public Doctor(Texture2D a_sheet, Vector2 a_position, Vector2 a_mapPosition, SpriteFont a_npcfont)
        {
            /** Create a Doctor with the folowing stats:
             * 30 Strength, 5 Agility, 6 Speed, 10 Intelligence,
             * 500 Health, 0 Mana, At level 15, with 500 exp for a kill.
             **/
            CreateStats(30, 6, 5, 10, 500, 0, 1, 15, 500);
            CreateAbstracts();
            // Dragon will wander 200 pixels, detect from 500 pixels, and pursue for 1000 pixels
            SetNPCAttributes(0, 300, 1000, false, true, 0, 20);
            SetDrawAttributes(4, "Doctor");

            //Had to do this manually because the sprite sheet is created poorly.
            m_backwards = new Rectangle[m_frames];
            m_backwards[0] = new Rectangle(0, 249, 52, 78);
            m_backwards[1] = new Rectangle(52, 288, 52, 78);
            m_backwards[2] = new Rectangle(103, 294, 52, 78);
            m_backwards[3] = new Rectangle(155, 289, 52, 78);
            m_forwards = new Rectangle[m_frames];
            m_forwards[0] = new Rectangle(0, 0, 49, 78);
            m_forwards[1] = new Rectangle(49, 9, 49, 78);
            m_forwards[2] = new Rectangle(98, 12, 49, 78);
            m_forwards[3] = new Rectangle(148, 10, 49, 78);
            m_left = new Rectangle[m_frames];
            m_left[0] = new Rectangle(0, 80, 49, 78);
            m_left[1] = new Rectangle(49, 80, 49, 78);
            m_left[2] = new Rectangle(98, 80, 49, 78);
            m_left[3] = new Rectangle(149, 80, 49, 78);
            m_right = new Rectangle[m_frames];
            m_right[0] = new Rectangle(0, 165, 49, 78);
            m_right[1] = new Rectangle(49, 165, 49, 78);
            m_right[2] = new Rectangle(98, 165, 49, 78);
            m_right[3] = new Rectangle(148, 165, 49, 78);
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
