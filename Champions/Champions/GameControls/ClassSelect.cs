using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.GameControls
{
    /// <summary>
    /// ClassSelect is a menu for choosing the player's class.
    /// </summary>
    class ClassSelect
    {
        /// <summary> Texture for the class select background.</summary>
        Texture2D m_classSelect;
        /// <summary> Vector for the position of the background (should be 0,0)</summary>
        Vector2 m_classSelect_position;
        /// <summary> Texture for the warrior select icon</summary>
        Texture2D m_classWarrior;
        /// <summary> Position of m_classWarrior.</summary>
        Vector2 m_classWarrior_position;
        /// <summary> Texture for the wizard select icon</summary>
        Texture2D m_classMage;
        /// <summary> Position of m_classMage.</summary>
        Vector2 m_classMage_position;
        /// <summary> Indicates whether or not the icon is on the warrior.</summary>
        private bool m_Warrior_isActive;

        /// <summary>
        /// Constructor. Initializes positions for all graphics and sets the m_warrior_isActive flag.
        /// </summary>
        public ClassSelect()
        {
            m_classSelect_position = new Vector2(0, 0);
            m_classWarrior_position = new Vector2(59, 130);
            m_classMage_position = new Vector2(870, 143);

            m_Warrior_isActive = false;
        }

        /// <summary>
        /// Loads the graphics for textures in memory.
        /// </summary>
        /// <param name="a_classSelect">Texture for the class selector icon.</param>
        /// <param name="a_classWarrior">Texture for the warrior class icon.</param>
        /// <param name="a_classMage">Texture for the mage class icon.</param>
        public void LoadClasses(Texture2D a_classSelect, Texture2D a_classWarrior, Texture2D a_classMage)
        {
            m_classSelect = a_classSelect;
            m_classMage = a_classMage;
            m_classWarrior = a_classWarrior;
        }
        /// <summary>
        /// Draws all textures to screen.
        /// </summary>
        /// <param name="a_spriteBatch">Tool used for drawing.</param>
        public void DrawClasses(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Draw(m_classSelect, m_classSelect_position, Color.White);

            if (m_Warrior_isActive == true)
            {
                a_spriteBatch.Draw(m_classWarrior, m_classWarrior_position, Color.White);
            }
            else
            {
                a_spriteBatch.Draw(m_classMage, m_classMage_position, Color.White);
            }

        }
        /// <summary>
        /// Moves the selector to the next active position.
        /// </summary>
        public void Next()
        {
            if (m_Warrior_isActive == true) m_Warrior_isActive = false;
            else m_Warrior_isActive = true;
        }
        /// <summary>
        /// Returns the current player's class based on whether the m_warrior is active.
        /// </summary>
        /// <returns>Player's class from Entities.Player.Class</returns>
        public Entities.Player.Class GetClass()
        {
            if (m_Warrior_isActive)
            {
                return Entities.Player.Class.Warrior;
            }
            else
            {
                return Entities.Player.Class.Wizard;
            }

        }
    }
}
