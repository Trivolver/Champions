using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.GameControls
{
    /// <summary>
    /// The loadgame class. Manages loading the game.
    /// </summary>
    class LoadGame
    {
        /// <summary>Selector height for the vector.</summary>
        private const int SELECTOR_POS_HEIGHT = 175;
        /// <summary>File select texture.</summary>
        private Texture2D m_fileSelect;
        /// <summary>Vector containing the file selector position.</summary>
        private Vector2 m_fileSelect_position;
        /// <summary>Texture containing the file selector.</summary>
        private Texture2D m_selector;
        /// <summary>Vector array of the selector positions.</summary>
        private Vector2 [] m_selector_position;
        /// <summary>The current selector position.</summary>
        private Vector2 m_selector_position_current;
        /// <summary>Warrior texture.</summary>
        private Texture2D m_warrior;
        /// <summary>Mage texture.</summary>
        private Texture2D m_mage;
        /// <summary>None texture.</summary>
        private Texture2D m_none;
        /// <summary>Spritefont for drawing.</summary>
        private SpriteFont m_font;
        /// <summary>Positions for drawing to screen.</summary>
        private Vector2 [] m_classPosition;

        /// <summary>
        /// Loadgame constructor. Initializes positions.
        /// </summary>
        public LoadGame()
        {
            m_fileSelect_position = new Vector2(70, 50);
            m_selector_position = new Vector2[3];
            m_classPosition = new Vector2[3];

            m_selector_position[0] = new Vector2(10, SELECTOR_POS_HEIGHT);
            m_selector_position[1] = new Vector2(450, SELECTOR_POS_HEIGHT);
            m_selector_position[2] = new Vector2(900, SELECTOR_POS_HEIGHT);
            m_selector_position_current = m_selector_position[0];
        }
        /// <summary>
        /// Draws the loadgame to screen.
        /// </summary>
        /// <param name="a_fileSelect">Fileselect graphic.</param>
        /// <param name="a_mage">Mage texture.</param>
        /// <param name="a_none">None texture.</param>
        /// <param name="a_selector">Selector texture.</param>
        /// <param name="a_warrior">Warrior texture.</param>
        /// <param name="a_menufont">Spritefont for drawing.</param>
        public void DrawLoad(Texture2D a_fileSelect, Texture2D a_mage, Texture2D a_none,
                                Texture2D a_selector, Texture2D a_warrior, SpriteFont a_menufont)
        {
            m_fileSelect = a_fileSelect;
            m_selector = a_selector;
            m_warrior = a_warrior;
            m_mage = a_mage;
            m_none = a_none;
            m_font = a_menufont;
        }
        /// <summary>
        /// Draws this object to the screen.
        /// </summary>
        /// <param name="a_spritebatch">Spritebatch used for drawing to screen.</param>
        /// <param name="a_slot1">Saveslot 1.</param>
        /// <param name="a_slot2">Saveslot 2.</param>
        /// <param name="a_slot3">Saveslot 3.</param>
        public void DrawLoadToScreen(SpriteBatch a_spritebatch, 
            Entities.Player a_slot1, Entities.Player a_slot2, Entities.Player a_slot3 )
        {
            string slot1 = "";
            string slot2 = "";
            string slot3 = "";

            a_spritebatch.Draw(m_fileSelect, m_fileSelect_position, Color.White);
            a_spritebatch.Draw(m_selector, m_selector_position_current, Color.White);

            if (a_slot1 != null) slot1 += a_slot1.GetClass() + " " + a_slot1.GetLevel();
            else slot1 = "Empty";
            if (a_slot2 != null) slot2 += a_slot2.GetClass() + " " + a_slot2.GetLevel();
            else slot2 = "Empty";
            if (a_slot3 != null) slot3 += a_slot3.GetClass() + " " + a_slot3.GetLevel();
            else slot3 = "Empty";

            a_spritebatch.DrawString(m_font, slot1, new Vector2(150, 400), Color.White);
            a_spritebatch.DrawString(m_font, slot2, new Vector2(590, 400), Color.White);
            a_spritebatch.DrawString(m_font, slot3, new Vector2(1050, 400), Color.White);
        }
        /// <summary>
        /// Moves the selector to the next posititon.
        /// </summary>
        public void Next()
        {
            if (m_selector_position_current == m_selector_position[0])
            {
                m_selector_position_current = m_selector_position[1];
            }
            else if (m_selector_position_current == m_selector_position[1])
            {
                m_selector_position_current = m_selector_position[2];
            }
            else //(m_selector_position_current == m_selector_position[2] )
            {
                m_selector_position_current = m_selector_position[0];
            }
        }
        /// <summary>
        /// Moves the selector to the previous position.
        /// </summary>
        public void Previous()
        {
            if (m_selector_position_current == m_selector_position[0])
            {
                m_selector_position_current = m_selector_position[2];
            }
            else if (m_selector_position_current == m_selector_position[1])
            {
                m_selector_position_current = m_selector_position[0];
            }
            else //(m_selector_position_current == m_selector_position[2] )
            {
                m_selector_position_current = m_selector_position[1];
            }
        }
        /// <summary>
        /// Gets the current load game slot.
        /// </summary>
        /// <returns></returns>
        public int GetLoadSlot()
        {
            if (m_selector_position_current == m_selector_position[0]) return 1;
            else if (m_selector_position_current == m_selector_position[1]) return 2;
            else if (m_selector_position_current == m_selector_position[2]) return 3;
            else return 0;
        }
    }
}
