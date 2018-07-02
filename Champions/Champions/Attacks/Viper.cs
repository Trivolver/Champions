using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Attacks
{
    /// <summary>
    /// The viper attack for the mage class.
    /// </summary>
    class Viper:Attack
    {
        /// <summary>
        /// Constructor for the mage attack. Creates texture, rectangles and position.
        /// </summary>
        /// <param name="a_sheet">The texture to draw from.</param>
        /// <param name="a_character">The character source.</param>
        public Viper(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(3, 2, 2, 10, 40, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(0, 0, 56, a_sheet.Height);
            m_anim[1] = new Rectangle(56, 0, 62, a_sheet.Height);
            m_position = a_character.GetPosition();
            UpdateDirection(a_character);
        }
        /// <summary>
        /// Updates the timer and direction of the attack.
        /// </summary>
        public override void Update()
        {
            m_durationTimer++;
            if (m_durationTimer >= m_duration)
            {
                Die();
            }
            if (m_isActive)
            {
                switch (m_direction)
                {
                    case Direction.Up:
                        MoveUp();
                        break;
                    case Direction.Down:
                        MoveDown();
                        break;
                    case Direction.Left:
                        MoveLeft();
                        break;
                    case Direction.Right:
                        MoveRight();
                        break;
                }
            }
        }
    }
}
