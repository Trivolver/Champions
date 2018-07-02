using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Attacks
{
    /// <summary>
    /// Draws the arcane bolt animation from the attack base class.
    /// </summary>
    class Arcanebolt:Attack
    {
        /// <summary>
        /// The constructor for the object Arcanebolt attack.
        /// </summary>
        /// <param name="a_sheet">The texture2D to draw from.</param>
        /// <param name="a_character">The player. Acts as the source.</param>
        public Arcanebolt(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(1, 5, 2, 6, 100, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(0, 0, 32, 32);
            m_anim[1] = new Rectangle(32, 0, 32, 32);
            m_position = a_character.GetPosition();
            UpdateDirection(a_character);
        }
        /// <summary>
        /// Updates the animation frame for the attack.
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
