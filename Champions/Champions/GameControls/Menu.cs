using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Champions.GameControls
{
    class Menu
    {
        /// <summary> Graphic representing the title. </summary>
        private Texture2D m_titleGraphic;
        /// <summary> Position of the title graphic at its top-left origin. </summary>
        private Vector2 m_titleGraphic_position;
        /// <summary>Graphic representing the title.</summary>
        private Texture2D m_titleOptionsGraphic;
        /// <summary>Position of the title graphic.</summary>
        private Vector2 m_titleOptionsGraphic_Position;
        /// <summary>Selector graphic texture.</summary>
        private Texture2D m_selectorGraphic;
        /// <summary>selector graphic position.</summary>
        private Vector2 m_selectorGraphic_Position;
        /// <summary>array of possible selector position.</summary>
        private Vector2 [] m_selectorGraphic_PositionArray;
        /// <summary>Song to play on the title screen.</summary>
        private Song m_titleSong;
        /// <summary>The menu selection.</summary>
        public enum MenuSelection { NewGame, LoadGame, Quit, None }

        /// <summary>
        /// Constructor for the Menu Class. Initializes the position of the Selector graphic 
        /// so that it can be in front of the corresponding positions on the menu. Also creates
        /// all other positions of the graphic objects.
        /// </summary>
        public Menu()
        {
            m_titleGraphic_position = new Vector2(0, 0);
            m_titleOptionsGraphic_Position = new Vector2(540, 330);
            m_selectorGraphic_PositionArray = new Vector2[3];
            m_selectorGraphic_PositionArray[0] = new Vector2(m_titleOptionsGraphic_Position.X - 100,
                                                        m_titleOptionsGraphic_Position.Y + 20);
            m_selectorGraphic_PositionArray[1] = new Vector2(m_titleOptionsGraphic_Position.X - 100,
                                                        m_titleOptionsGraphic_Position.Y + 140);
            m_selectorGraphic_PositionArray[2] = new Vector2(m_titleOptionsGraphic_Position.X - 100,
                                                        m_titleOptionsGraphic_Position.Y + 260);
            m_selectorGraphic_Position = m_selectorGraphic_PositionArray[0];
        }

        /// <summary>
        /// Assigns all graphics from the Game.cs class to their corresponding member variables.
        /// </summary>
        /// <param name="a_titleGraphic">The Texture for the Title Graphic.</param>
        /// <param name="a_titleOptionsGraphic">The Texture for the options of the title (new game, etc)</param>
        /// <param name="a_selectorGraphic">The texture for the selector, the skull.</param>
        /// <param name="a_titleSong">The song for the title screen.</param>
        public void InitializeMenu(Texture2D a_titleGraphic, Texture2D a_titleOptionsGraphic,
                                   Texture2D a_selectorGraphic, Song a_titleSong)
        {
            m_titleGraphic = a_titleGraphic;
            m_titleOptionsGraphic = a_titleOptionsGraphic;
            m_selectorGraphic = a_selectorGraphic;
            m_titleSong = a_titleSong;
        }

        /// <summary>
        /// Calls the draw method to draw all objects to the screen.
        /// </summary>
        /// <param name="a_spriteBatch"></param>
        public void DrawMenu(SpriteBatch a_spriteBatch)
        {

            a_spriteBatch.Draw(m_titleGraphic, m_titleGraphic_position, Color.White);
            a_spriteBatch.Draw(m_titleOptionsGraphic, m_titleOptionsGraphic_Position, Color.White);
            a_spriteBatch.Draw(m_selectorGraphic, m_selectorGraphic_Position, Color.White);
        }

        /// <summary>
        /// Plays the music during the title screen.
        /// </summary>
        public void PlayTitleMusic()
        {
            try
            {
                MediaPlayer.Volume = 0.1f;
                MediaPlayer.Play(m_titleSong);
            }
            catch { }
        }
        /// <summary>
        /// Gets the next posiiton and moves the selector.s
        /// </summary>
        public void NextSelection()
        {
            if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[0]) //Position "NewGame"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[1];
            }
            else if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[1]) //Position "LoadGame"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[2];
            }
            else //Position "Quit"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[0];
            }
        }
        /// <summary>
        /// Moves the selector back 1 position.
        /// </summary>
        public void PreviousSelection()
        {
            if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[0]) //Position "NewGame"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[2];
            }
            else if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[1]) //Position "LoadGame"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[0];
            }
            else //Position "Quit"
            {
                m_selectorGraphic_Position = m_selectorGraphic_PositionArray[1];
            }
        }
        /// <summary>
        /// Returns the menuselection of the current position.
        /// </summary>
        /// <returns>The selected position choice.</returns>
        public MenuSelection GetSelection()
        {
            if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[0]) //Position "NewGame"
            {
                return MenuSelection.NewGame;   
            }
            else if (m_selectorGraphic_Position == m_selectorGraphic_PositionArray[1]) //Position "LoadGame"
            {
                return MenuSelection.LoadGame;
            }
            else //Position "Quit"
            {
                return MenuSelection.Quit;
            }
        }
        /// <summary>
        /// Unloads all graphics.
        /// </summary>
        public void UnloadGraphics()
        {
            m_selectorGraphic.Dispose();
            m_titleGraphic.Dispose();
            m_titleOptionsGraphic.Dispose();
            m_titleSong.Dispose();
        }

    }//End of class definition
}
