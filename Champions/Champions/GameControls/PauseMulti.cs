using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Champions.GameControls
{
    /// <summary>
    /// Class called when the game is paused.
    /// </summary>
    class PauseMulti
    {
        /// <summary>The current keyboard state.</summary>
        KeyboardState m_currentKeyboardState;
        /// <summary>The current mouse state.</summary>
        MouseState m_currentMouseState;
        /// <summary>The previous mouse state.</summary>
        MouseState m_previousMouseState;
        /// <summary>The captured mouse point.</summary>
        Vector2 m_mousePoint;
        /// <summary>The spritefont for drawing to the screen.</summary>
        SpriteFont m_menuType;
        /// <summary>Texture of the cursor.</summary>
        Texture2D m_cursor;
        /// <summary>Position of the cursor.</summary>
        Vector2 m_cursorPos;
        /// <summary>Texture of the pause screen.</summary>
        Texture2D m_PauseScreen;
        /// <summary>Array of the attributes to draw.</summary>
        Texture2D [] m_addAttribute;
        /// <summary>Positions of the attribute boxes.</summary>
        Vector2 [] m_attributePositions;
        /// <summary>Position to click to quit game.</summary>
        Rectangle m_quitPosition;
        /// <summary>Position to click to save the game.</summary>
        Rectangle m_savePosition;
        /// <summary>Position to click to start multiplayer.</summary>
        Rectangle m_multiPosition;
        /// <summary>The flag for adding attributes.</summary>
        bool m_addAttributeFlag;
        /// <summary>The attribute number to add to.</summary>
        int m_attributeNum;
        /// <summary>Textures to draw to the screen.</summary>
        Texture2D[] m_items;
        /// <summary>Position of the textures.</summary>
        Vector2[] m_itemPos;
        /// <summary>Slot texture 1 is the equipped weapon texture.</summary>
        Texture2D m_slot1;
        /// <summary>The currently equipped armor texture.</summary>
        Texture2D m_slot2;
        /// <summary>Set to true if the game needs to be saved.</summary>
        public bool m_saveFlag;

        /// <summary>
        /// Constructor for the pause screen. Initializes positions.
        /// </summary>
        public PauseMulti()
        {
            m_quitPosition = new Rectangle(950, 300, 330, 150);
            m_savePosition = new Rectangle(910, 30, 350, 150);
            m_multiPosition = new Rectangle(375, 600, 680, 120);
            
            m_attributePositions = new Vector2[4];
            m_attributePositions[0] = new Vector2(30, 164);
            m_attributePositions[1] = new Vector2(30, 202);
            m_attributePositions[2] = new Vector2(30, 240);
            m_attributePositions[3] = new Vector2(30, 278);

            m_addAttributeFlag = false;
            m_saveFlag = false;

            m_itemPos = new Vector2[12];
            m_itemPos[0] = new Vector2(525, 45);
            m_itemPos[1] = new Vector2(652, 45);
            m_itemPos[2] = new Vector2(777, 35);
            m_itemPos[3] = new Vector2(525, 170);
            m_itemPos[4] = new Vector2(652, 170);
            m_itemPos[5] = new Vector2(777, 160);
            m_itemPos[6] = new Vector2(525, 295);
            m_itemPos[7] = new Vector2(652, 295);
            m_itemPos[8] = new Vector2(777, 275);
            m_itemPos[9] = new Vector2(525, 420);
            m_itemPos[10] = new Vector2(652, 420);
            m_itemPos[11] = new Vector2(777, 410);
            m_items = new Texture2D[12];

        }
        /// <summary>
        /// Loads textures.
        /// </summary>
        /// <param name="a_PauseScreen">Pause screen texture.</param>
        /// <param name="a_menuType">Spritefront for text.</param>
        /// <param name="a_cursor">Cursor texture.</param>
        /// <param name="a_addAttribute">Texture for adding attributes.</param>
        /// <param name="a_A1">Warrior weapon 1 texture.</param>
        /// <param name="a_A2">Warrior weapon 2 texture.</param>
        /// <param name="a_A3">Warrior weapon 3 texture.</param>
        /// <param name="a_S1">Wizard weapon 1 texture.</param>
        /// <param name="a_S2">Wizard weapon 2 texture.</param>
        /// <param name="a_S3">Wizard weapon 3 texture.</param>
        /// <param name="a_D1">Warrior armor 1 texture.</param>
        /// <param name="a_D2">Warrior armor 2 texture.</param>
        /// <param name="a_D3">Warrior armor 3 texture.</param>
        /// <param name="a_D4">Wizard armor 1 texture.</param>
        /// <param name="a_D5">Wizard armor 2 texture.</param>
        /// <param name="a_D6">Wizard armor 3 texture.</param>
        public void LoadPause(Texture2D a_PauseScreen, SpriteFont a_menuType, Texture2D a_cursor, Texture2D a_addAttribute,
                                Texture2D a_A1, Texture2D a_A2, Texture2D a_A3, Texture2D a_S1, Texture2D a_S2, Texture2D a_S3,
                                Texture2D a_D1, Texture2D a_D2, Texture2D a_D3, Texture2D a_D4, Texture2D a_D5, Texture2D a_D6)
        {
            m_PauseScreen = a_PauseScreen;
            m_menuType = a_menuType;
            m_cursor = a_cursor;

            m_addAttribute = new Texture2D[4] { a_addAttribute, a_addAttribute, a_addAttribute, a_addAttribute};


            m_currentMouseState = Mouse.GetState();
            m_cursorPos = new Vector2(m_currentMouseState.X, m_currentMouseState.Y);

            m_items[0] = a_S1;
            m_items[1] = a_S2;
            m_items[2] = a_S3;
            m_items[3] = a_A1;
            m_items[4] = a_A2;
            m_items[5] = a_A3;
            m_items[6] = a_D1;
            m_items[7] = a_D2;
            m_items[8] = a_D3;
            m_items[9] = a_D4;
            m_items[10] = a_D5;
            m_items[11] = a_D6;            
        }
        /// <summary>
        /// Draws the pause textures to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="a_player">Reference to the currently active player.</param>
        public void DrawPause(SpriteBatch a_spriteBatch, ref Entities.Player a_player)
        {
            a_spriteBatch.Draw(m_PauseScreen, new Vector2(0, 0), Color.White);
            a_spriteBatch.DrawString(m_menuType, "Press Tab to resume...", new Vector2(15, 700), Color.White);

            for (int i = 0; i < 4; i++)
            {
                a_spriteBatch.Draw(m_addAttribute[i], m_attributePositions[i], Color.White);
            }

            string drawString1;
            string drawString2 = "";
            if (a_player.GetClass() == Entities.Player.Class.Warrior)
            {
                drawString1 = "Warrior";
            }
            else
            {
                drawString1 = "Wizard";
            }

            drawString1 += " Level " + a_player.GetLevel() + "\n";
            drawString1 += a_player.GetMenuString1();
            drawString2 += a_player.GetMenuString2();

            a_spriteBatch.DrawString(m_menuType, drawString1, new Vector2(40, 30), Color.Black);
            a_spriteBatch.DrawString(m_menuType, drawString2, new Vector2(250, 100), Color.Black);
            for (int i = 0; i < 12; i++)
            {
                if (a_player.m_HasItems[i])
                {
                    a_spriteBatch.Draw(m_items[i], m_itemPos[i], Color.White);
                }
            }
            m_slot1 = m_items[a_player.GetWeaponIndex()];
            m_slot2 = m_items[a_player.GetArmorIndex()];
            a_spriteBatch.Draw(m_slot1, new Vector2(35, 370), Color.White);
            a_spriteBatch.Draw(m_slot2, new Vector2(195, 360), Color.White);
            a_spriteBatch.Draw(m_cursor, m_cursorPos, Color.White);
        }

        /// <summary>
        /// Handles the gamestate from the pause menu and manages mouse input.
        /// </summary>
        /// <param name="a_gameTime">Gametime for updating.</param>
        /// <param name="a_char">Reference to the currently active character.</param>
        /// <returns>The gamestate.</returns>
        public GameControls.Engine.GameState HandlePause(GameTime a_gameTime, ref Entities.Player a_char)
        {
            GameControls.Engine.GameState state = Engine.GameState.Pause;
            m_currentKeyboardState = Keyboard.GetState();
            m_previousMouseState = m_currentMouseState;
            m_currentMouseState = Mouse.GetState();

            if ( m_currentKeyboardState.IsKeyDown(Keys.Tab))
            {
                state = Engine.GameState.Playing;
            }


            if (m_currentMouseState.LeftButton == ButtonState.Released &&
                m_previousMouseState.LeftButton == ButtonState.Pressed)
            {
                m_mousePoint = new Vector2(m_currentMouseState.X, m_currentMouseState.Y);
                state = CheckMouseClick(m_mousePoint, ref a_char);
            }

            m_cursorPos = new Vector2(m_currentMouseState.X, m_currentMouseState.Y);

            return state;
        }

        /// <summary>
        /// Gets the gamestate from where the mouse was clicked and manages equipment.
        /// </summary>
        /// <param name="a_mousePoint">Vector contianing the click point.</param>
        /// <param name="a_char">Reference to the active character.</param>
        /// <returns></returns>
        public GameControls.Engine.GameState CheckMouseClick(Vector2 a_mousePoint, ref Entities.Player a_char)
        {
            if (m_quitPosition.Contains((int)a_mousePoint.X, (int)a_mousePoint.Y))
            {
                return GameControls.Engine.GameState.Quit;
            }

            if (m_savePosition.Contains((int)a_mousePoint.X, (int)a_mousePoint.Y))
            {
                m_saveFlag = true;
                return GameControls.Engine.GameState.Playing;
            }

            if (m_multiPosition.Contains((int)a_mousePoint.X, (int)a_mousePoint.Y))
            {
                MultiplayerManager.StartMultiplayer();
            }

            for (int i = 0; i < 4; i++)
            {
                if (new Rectangle((int)m_attributePositions[i].X, (int)m_attributePositions[i].Y,
                                        m_addAttribute[i].Width, m_addAttribute[i].Height).Contains
                                        ((int)a_mousePoint.X, (int)a_mousePoint.Y))
                {
                    switch (i)
                    {
                        case 0:
                            m_attributeNum = 1;
                            break;
                        case 1:
                            m_attributeNum = 2;
                            break;
                        case 2:
                            m_attributeNum = 3;
                            break;
                        case 3:
                            m_attributeNum = 4;
                            break;
                    }
                }

            }


            for (int i = 0; i <= 2; i++)
            {
                if (a_char.GetClass() == Entities.Player.Class.Wizard && a_char.HasItem(i))
                {
                    if (new Rectangle((int)m_itemPos[i].X, (int)m_itemPos[i].Y,
                        m_items[i].Width, m_items[i].Height).Contains((int)m_mousePoint.X, (int)m_mousePoint.Y)) 
                    {
                        m_slot1 = m_items[i];
                        a_char.Equip(i);
                    }
                }
            }
            for (int i = 3; i <= 5; i++)
            {
                if (a_char.GetClass() == Entities.Player.Class.Warrior && a_char.HasItem(i))
                {
                    if (new Rectangle((int)m_itemPos[i].X, (int)m_itemPos[i].Y,
                        m_items[i].Width, m_items[i].Height).Contains((int)m_mousePoint.X, (int)m_mousePoint.Y)) 
                    {
                        m_slot1 = m_items[i];
                        a_char.Equip(i);
                    }
                }
            }
            for (int i = 6; i <= 8; i++)
            {
                if (a_char.GetClass() == Entities.Player.Class.Warrior && a_char.HasItem(i))
                {
                    if (new Rectangle((int)m_itemPos[i].X, (int)m_itemPos[i].Y,
                        m_items[i].Width, m_items[i].Height).Contains((int)m_mousePoint.X, (int)m_mousePoint.Y)) 
                    {
                        m_slot2 = m_items[i];
                        a_char.Equip(i);
                    }
                }
            }
            for (int i = 9; i <= 11; i++)
            {
                if (a_char.GetClass() == Entities.Player.Class.Wizard && a_char.HasItem(i))
                {
                    if (new Rectangle((int)m_itemPos[i].X, (int)m_itemPos[i].Y,
                        m_items[i].Width, m_items[i].Height).Contains((int)m_mousePoint.X, (int)m_mousePoint.Y)) 
                    {
                        m_slot2 = m_items[i];
                        a_char.Equip(i);
                    }
                }
            }

            return Engine.GameState.Pause;
        }

        /// <summary>
        /// Checks the attribute from the character.
        /// </summary>
        /// <returns>number of attributes.</returns>
        public int CheckAttributes()
        {
            int attribute = m_attributeNum;
            m_attributeNum = 0;
            return attribute;
        }
        /// <summary>
        /// Checks if we are saving the game.
        /// </summary>
        /// <returns>True if we are saving the game.</returns>
        public bool CheckSave()
        {
            bool newflag = m_saveFlag;
            m_saveFlag = false;
            return newflag;
        }
    }
}
