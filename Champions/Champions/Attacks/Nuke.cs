using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Champions.Attacks
{
    class Nuke:Attack
    {
        /// <summary>
        /// Constructor for the mage attack, Nuke. Loads texture and position.
        /// </summary>
        /// <param name="a_sheet">The texture to draw the nuke from.</param>
        /// <param name="a_character">The character which launched the attack.</param>
        public Nuke(Texture2D a_sheet, Entities.Character a_character)
        {
            AssignAttributes(10, 3, 2, 0, 200, a_character.CheckIsEnemy());
            m_sheet = a_sheet;
            m_anim = new Rectangle[m_frames];
            m_anim[0] = new Rectangle(0, 0, 63, a_sheet.Height);
            m_anim[1] = new Rectangle(64, 0, 94, a_sheet.Height);
            switch (a_character.GetState())
            {
                case Entities.Character.State.IdleLeft:
                    m_position = new Vector2(a_character.GetPosition().X - 200, a_character.GetPosition().Y - 50);
                    break;
                case Entities.Character.State.MoveLeft:
                    m_position = new Vector2(a_character.GetPosition().X - 200, a_character.GetPosition().Y - 50);
                    break;
                case Entities.Character.State.IdleUp:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y - 300);
                    break;
                case Entities.Character.State.MoveUp:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y - 300);
                    break;
                case Entities.Character.State.MoveRight:
                    m_position = new Vector2(a_character.GetPosition().X + 200, a_character.GetPosition().Y - 50);
                    break;
                case Entities.Character.State.IdleRight:
                    m_position = new Vector2(a_character.GetPosition().X + 200, a_character.GetPosition().Y - 50);
                    break;
                case Entities.Character.State.IdleDown:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y + 100);
                    break;
                case Entities.Character.State.MoveDown:
                    m_position = new Vector2(a_character.GetPosition().X, a_character.GetPosition().Y + 100);
                    break;
            }
        }
        /// <summary>
        /// Updates the duration of the attack.
        /// </summary>
        public override void Update()
        {
            m_durationTimer++;
            if (m_durationTimer >= m_duration) Die();
        }
    }
}
