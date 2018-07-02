using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Attacks
{
    /// <summary>
    /// This class launches the warrior ability, lightning.
    /// </summary>
    class Lightning:Attack
    {
        /// <summary>
        /// Constructor for the lightning attack. Assigns the texture and the frames.
        /// </summary>
        /// <param name="a_sheet">The source texture for the lightning attack.</param>
        /// <param name="a_character">The character which launched the attack.</param>
        public Lightning(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(2, 3, 6, 7, 40, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_position = a_character.GetPosition();
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(3, 0, 99, 83);
            m_anim[1] = new Rectangle(118, 0, 100, 90);
            m_anim[2] = new Rectangle(232, 0, 102, 91);
            m_anim[3] = new Rectangle(346, 0, 106, 91);
            m_anim[4] = new Rectangle(463, 0, 109, 93);
            m_anim[5] = new Rectangle(583, 11, 104, 86);
        }
        /// <summary>
        /// Updates the timer of the lightning attack while it cycles the frames.
        /// </summary>
        public override void Update()
        {
            m_durationTimer++;
            if (m_durationTimer >= m_duration)
            {
                Die();
            }
        }
    }
}
