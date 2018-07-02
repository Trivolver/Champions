using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Champions.GameControls
{
    /// <summary>
    /// The newgame class.
    /// </summary>
    class NewGame
    {
        /// <summary>Height of the selector.</summary>
        private const int SELECTOR_SCREEN_HEIGHT = 170;
        /// <summary>Newgame screen texture.</summary>
        public Texture2D m_newScreen;
        /// <summary>Selector texture.</summary>
        public Texture2D m_selector;
        /// <summary>Newgame screen position.</summary>
        public Vector2 m_newScreen_position;
        /// <summary>current selector position.</summary>
        public Vector2 m_selector_position;
        /// <summary>Possible selector positions.</summary>
        public Vector2 [] m_selector_position_array;

        /// <summary>
        /// Constructor for newgame. Assigns all possible selector positions.
        /// </summary>
        public NewGame()
        {
            m_selector_position_array = new Vector2[3];
            m_newScreen_position = new Vector2(0, 0);
            m_selector_position_array[0] = new Vector2(20, SELECTOR_SCREEN_HEIGHT);
            m_selector_position_array[1] = new Vector2(450, SELECTOR_SCREEN_HEIGHT);
            m_selector_position_array[2] = new Vector2(865, SELECTOR_SCREEN_HEIGHT);
            m_selector_position = m_selector_position_array[0];
        }
        /// <summary>
        /// Loads all textures.
        /// </summary>
        /// <param name="a_newScreen">Newgame texture.</param>
        /// <param name="a_selector">Selector texture.</param>
        public void LoadTextures(Texture2D a_newScreen, Texture2D a_selector)
        {
            m_newScreen = a_newScreen;
            m_selector = a_selector;
        }
        /// <summary>
        /// Draws textures to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch for drawing to the screen.</param>
        public void DrawTextures(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Draw(m_newScreen, m_newScreen_position, Color.White);
            a_spriteBatch.Draw(m_selector, m_selector_position, Color.White);
        }
        /// <summary>
        /// Moves the selector back one position.
        /// </summary>
        public void PrevSelection()
        {
            if(m_selector_position == m_selector_position_array[2])
            {
                m_selector_position = m_selector_position_array[1];
            }
            else if (m_selector_position == m_selector_position_array[1])
            {
                m_selector_position = m_selector_position_array[0];
            }
            else
            {
                m_selector_position = m_selector_position_array[2];
            }
        }
        /// <summary>
        /// Moves the selector forward one position.
        /// </summary>
        public void NextSelection()
        {
            if (m_selector_position == m_selector_position_array[0])
            {
                m_selector_position = m_selector_position_array[1];
            }
            else if (m_selector_position == m_selector_position_array[1])
            {
                m_selector_position = m_selector_position_array[2];
            }
            else
            {
                m_selector_position = m_selector_position_array[0];
            }
        }
        /// <summary>
        /// Gets the current selection.
        /// </summary>
        /// <returns>Returns the integer choice of the selector.</returns>
        public int GetSelection()
        {
            if (m_selector_position == m_selector_position_array[0])
            {
                return 1;
            }
            else if (m_selector_position == m_selector_position_array[1])
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }
}
