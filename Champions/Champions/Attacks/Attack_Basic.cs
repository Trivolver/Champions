using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Champions.Attacks
{
    /// <summary>
    /// Loads and draws the standard attack to the screen.
    /// </summary>
    class Attack_Basic:Attack
    {
        /// <summary>
        /// Constructor for the class. Loads the texture.
        /// </summary>
        /// <param name="a_sheet">The texture sheet to draw to the screen.</param>
        /// <param name="a_character">Character launching the attack.</param>
        public Attack_Basic(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(1, 3, 4, 1, 15, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(168, 59, 29, 29);
            m_anim[1] = new Rectangle(118, 61, 34, 24);
            m_anim[2] = new Rectangle(55, 52, 45, 43);
            m_anim[3] = new Rectangle(5, 41, 49, 50);
            m_position = a_character.GetPosition();
            UpdateDirection(a_character);
            switch (m_direction)
            {
                case Direction.Up:
                    m_position.Y -= 10;
                    break;
                case Direction.Left:
                    m_position.X -= 10;
                    break;
                case Direction.Right:
                    m_position.X += 10;
                    break;
                case Direction.Down:
                    m_position.Y += 10;
                    break;
            }
        }
        /// <summary>
        /// Checks if the attack is still alive.
        /// </summary>
        public override void Update()
        {
            m_durationTimer++;
            if (m_durationTimer >= m_duration) Die();
        }
    }
}
