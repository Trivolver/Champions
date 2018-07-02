using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Champions.Attacks
{
    /// <summary>
    /// The hammer attack.
    /// </summary>
    class Hammer:Attack
    {
        /// <summary>
        /// Constructor for the hammer attack.
        /// </summary>
        /// <param name="a_sheet">Texture2D to draw from.</param>
        /// <param name="a_character">The character which launched the attack.</param>
        public Hammer(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(3, 1, 1, 4, 20, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(0, 0, a_sheet.Width, a_sheet.Height);
            m_position = a_character.GetPosition();

            switch (a_character.GetState())
            {
                case Entities.Character.State.IdleLeft:
                    m_position = new Vector2(a_character.GetPosition().X - 100, a_character.GetPosition().Y - 100);
                    break;
                case Entities.Character.State.MoveLeft:
                    m_position = new Vector2(a_character.GetPosition().X - 100, a_character.GetPosition().Y - 100);
                    break;
                case Entities.Character.State.IdleUp:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y -150);
                    break;
                case Entities.Character.State.MoveUp:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y - 150);
                    break;
                case Entities.Character.State.MoveRight:
                    m_position = new Vector2(a_character.GetPosition().X + 100, a_character.GetPosition().Y - 100);
                    break;
                case Entities.Character.State.IdleRight:
                    m_position = new Vector2(a_character.GetPosition().X + 100, a_character.GetPosition().Y - 100);
                    break;
                case Entities.Character.State.IdleDown:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y  + 50);
                    break;
                case Entities.Character.State.MoveDown:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y +500);
                    break;
            }
        }

        /// <summary>
        /// Updates the position of the attack, checks if the attack is alive.
        /// </summary>
        public override void  Update()
        {
            m_durationTimer++;
            if (m_durationTimer >= m_duration) Die();
            MoveDown();
        }
    }
}
