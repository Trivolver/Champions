using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Attacks
{
    /// <summary>
    /// The fireball attack.
    /// </summary>
    class Fireball:Attack
    {
        /// <summary>
        /// Constructor for the fireball class. Loads texture and position.
        /// </summary>
        /// <param name="a_sheet">The texture to draw from.</param>
        /// <param name="a_character">The character launching the attack.</param>
        public Fireball(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(1, 5, 2, 6, 300, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(0, 0, 32, 32);
            m_anim[1] = new Rectangle(32, 0, 32, 32);
            m_position = a_character.GetPosition();
            UpdateDirection(a_character);
        }
        /// <summary>
        /// Updates the timer and the direction. Kills if the time expires.
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
