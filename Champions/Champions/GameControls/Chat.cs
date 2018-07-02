using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Champions
{
    /// <summary>
    /// This controls the chat window on the UI of the screen.
    /// </summary>
    static class Chat
    {
        /// <summary>Prefix to add, based on the player number or the system.</summary>
        static String m_prefix;
        /// <summary>Message to add to the chatbox.</summary>
        static String m_Message;
        /// <summary>Message to display while typing.</summary>
        static String m_display;
        /// <summary>Chat overlay to draw to the screen.</summary>
        static Texture2D m_chatOverlay;
        /// <summary>Position to draw the text.</summary>
        static Vector2 m_chatPosition;
        /// <summary>Position of the chat texture.</summary>
        static Vector2 m_windowPosition;
        /// <summary>Font style to draw with.</summary>
        static SpriteFont m_font;
        /// <summary>Queue of messages to draw to the screen.</summary>
        static Queue<String> m_Messages;
        /// <summary>Number of lines currently being drawn to the screen.</summary>
        static int m_currentlines = 0;
        /// <summary>Maximum number of characters per line.</summary>
        static int m_maxChars = 80;
        /// <summary>If true, uses input from this class.</summary>
        static bool m_inChatMode = false;
        /// <summary>Array containing the keyboardstate (slows input).</summary>
        static KeyboardState[] m_keyboardState;
        /// <summary>
        /// Loads the chat window textures to the variables.
        /// </summary>
        /// <param name="a_overlay">Texture of the overlay.</param>
        /// <param name="a_font">Font style to draw to the screen.</param>
        public static void LoadChatWindow(Texture2D a_overlay, SpriteFont a_font)
        {
            m_Messages = new Queue<String>();
            m_chatOverlay = a_overlay;
            m_font = a_font;
            m_windowPosition = new Vector2(1, 370);
            m_chatPosition = new Vector2(1, 320);
            m_prefix = "Player:  ";
            m_Message = "";
            m_display = "";
            m_Messages.Enqueue("Welcome to Champions of Darkness!");
            m_currentlines = 1;
            m_keyboardState = new KeyboardState[7];
        }
        /// <summary>
        /// Draws the chat window and all strings to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch to draw to the screen.</param>
        public static void DrawChatWindow(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Draw(m_chatOverlay, m_windowPosition, Color.White);
            a_spriteBatch.DrawString(m_font, m_display, new Vector2(20, 550), Color.White);

            String[] stringarray = m_Messages.ToArray();

            switch (m_currentlines)
            {
                case 0:
                    break;
                case 1:
                    m_chatPosition = new Vector2(20, 520);
                    break;
                case 2:
                    m_chatPosition = new Vector2(20, 500);
                    break;
                case 3:
                    m_chatPosition = new Vector2(20, 480);
                    break;
                case 4:
                    m_chatPosition = new Vector2(20, 460);
                    break;
                case 5:
                    m_chatPosition = new Vector2(20, 440);
                    break;
                case 6:
                    m_chatPosition = new Vector2(20, 420);
                    break;
                case 7:
                    m_chatPosition = new Vector2(20, 400);
                    break;
                default:
                    m_chatPosition = new Vector2(20, 380);
                    break;
            }

            String display = "";

            for (int i = 0; i < m_currentlines; i++)
            {
                display += stringarray[i];
                display += "\n";
            }

            a_spriteBatch.DrawString(m_font, display, m_chatPosition, Color.White);
        }
        /// <summary>
        /// Recieves a message from a multiplayer. Parses it.
        /// </summary>
        /// <param name="a_message">String containing the message.</param>
        static void RecieveMultiplayerMessage(String a_message)
        {

        }
        /// <summary>
        /// Sends a message across the network to another chatbox.
        /// </summary>
        /// <param name="a_message">Message to send.</param>
        static void SendMultiplayerMessage(String a_message)
        {

        }
        /// <summary>
        /// Checks if this is a command or not. Parses as a command if it is.
        /// </summary>
        /// <param name="a_message">Message to check.</param>
        static void ParseMessageType(String a_message)
        {
            if (a_message[0] == '.')
            {
                ParseCommand(a_message);
            }
            else
            {
                AddMessage(a_message);
            }
        }
        /// <summary>
        /// Checks if the game is using chat mode input.
        /// </summary>
        /// <returns>A boolean representing the chat input mode - true if so.</returns>
        public static bool IsChatMode()
        {
            return m_inChatMode;
        }
        /// <summary>
        /// Uses the keyboard as a state of input to take / relay text.
        /// </summary>
        public static void UseChatInput()
        {
            m_keyboardState[1] = m_keyboardState[0];
            m_keyboardState[2] = m_keyboardState[1];
            m_keyboardState[3] = m_keyboardState[2];
            m_keyboardState[4] = m_keyboardState[3];
            m_keyboardState[5] = m_keyboardState[4];
            m_keyboardState[6] = m_keyboardState[5];
            m_keyboardState[0] = Keyboard.GetState();


            if (m_keyboardState[0] == m_keyboardState[6])
            {
                //Game moving too fast!
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.Enter))
            {
                if (m_Message.Trim() != "")
                {
                    ParseMessageType(m_Message);
                }
                m_inChatMode = false;
                m_Message = "";
                m_display = "";
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.Escape))
            {
                m_inChatMode = false;
                m_Message = "";
                m_display = "";
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.Back))
            {
                if (m_Message != "" || m_Message.Length > 0)
                {
                    m_Message = m_Message.Remove(m_Message.Length - 1);
                    m_display = m_display.Remove(m_display.Length - 1);
                }
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.A))
            {
                AddCharToString('A');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.A))
            {
                AddCharToString('a');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.B))
            {
                AddCharToString('B');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.B))
            {
                AddCharToString('b');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.C))
            {
                AddCharToString('C');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.C))
            {
                AddCharToString('c');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.D))
            {
                AddCharToString('D');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.D))
            {
                AddCharToString('d');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.E))
            {
                AddCharToString('E');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.E))
            {
                AddCharToString('e');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.F))
            {
                AddCharToString('F');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.F))
            {
                AddCharToString('f');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.G))
            {
                AddCharToString('G');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.G))
            {
                AddCharToString('g');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.H))
            {
                AddCharToString('H');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.H))
            {
                AddCharToString('h');
            }
              else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.I))
            {
                AddCharToString('I');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.I))
            {
                AddCharToString('i');
            }
             else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.J))
            {
                AddCharToString('J');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.J))
            {
                AddCharToString('j');
            }
             else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.K))
            {
                AddCharToString('K');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.K))
            {
                AddCharToString('k');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.L))
            {
                AddCharToString('L');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.L))
            {
                AddCharToString('l');
            }
              else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.M))
            {
                AddCharToString('M');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.M))
            {
                AddCharToString('m');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.N))
            {
                AddCharToString('N');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.N))
            {
                AddCharToString('n');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.O))
            {
                AddCharToString('O');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.O))
            {
                AddCharToString('o');
            }
             else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.P))
            {
                AddCharToString('P');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.P))
            {
                AddCharToString('p');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.Q))
            {
                AddCharToString('Q');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.Q))
            {
                AddCharToString('q');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.R))
            {
                AddCharToString('R');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.R))
            {
                AddCharToString('r');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.S))
            {
                AddCharToString('S');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.S))
            {
                AddCharToString('s');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.T))
            {
                AddCharToString('T');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.T))
            {
                AddCharToString('t');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.U))
            {
                AddCharToString('U');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.U))
            {
                AddCharToString('u');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.V))
            {
                AddCharToString('V');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.V))
            {
                AddCharToString('v');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.W))
            {
                AddCharToString('W');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.W))
            {
                AddCharToString('w');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.X))
            {
                AddCharToString('X');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.X))
            {
                AddCharToString('x');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.Y))
            {
                AddCharToString('Y');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.Y))
            {
                AddCharToString('y');
            }
            else if ((m_keyboardState[0].IsKeyDown(Keys.LeftShift) || m_keyboardState[0].IsKeyDown(Keys.RightShift) ||
                m_keyboardState[0].IsKeyDown(Keys.CapsLock)) &&
                m_keyboardState[0].IsKeyDown(Keys.Z))
            {
                AddCharToString('Z');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.Z))
            {
                AddCharToString('z');
            }
            else if ( m_keyboardState[0].IsKeyDown(Keys.Space))
            {
                AddCharToString(' ');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D1))
            {
                AddCharToString('1');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D2))
            {
                AddCharToString('2');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D3))
            {
                AddCharToString('3');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D4))
            {
                AddCharToString('4');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D5))
            {
                AddCharToString('5');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D6))
            {
                AddCharToString('6');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D7))
            {
                AddCharToString('7');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D8))
            {
                AddCharToString('8');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D9))
            {
                AddCharToString('9');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.D0))
            {
                AddCharToString('0');
            }
            else if (m_keyboardState[0].IsKeyDown(Keys.OemPeriod))
            {
                AddCharToString('.');
            }
            else
            {
                //Nothing to do, wait for input.
            }
        }
        /// <summary>
        /// Flags the game to use input controls from the Chat.
        /// </summary>
        public static void EnterChatMode()
        {
            m_inChatMode = true;
        }
        /// <summary>
        /// Parses a command message.
        /// </summary>
        /// <param name="a_message">String to check.</param>
        public static void ParseCommand(string a_message)
        {
            if (a_message.Trim().ToLower() == ".levelup")
            {

            }
            else
            {
                AddMessage("Invalid Command!");
            }
        }
        /// <summary>
        /// Adds a character to the current display string after given input.
        /// </summary>
        /// <param name="a_key">Character to add.</param>
        static void AddCharToString(char a_key)
        {
            if (m_Message.Length == m_maxChars)
            {
                return;
            }
            else
            {
                m_Message += a_key;
                if (m_display.Length == 40)
                {
                    m_display = m_display.Remove(0, 1);
                    m_display += a_key;
                }
                else
                {
                    m_display += a_key;
                }
            }
        }
        /// <summary>
        /// Adds a message to the screen queue.
        /// </summary>
        /// <param name="a_message">String to add to queue.</param>
        static public void AddMessage(String a_message)
        {
            //If current lines are less than seven, add a line.
            if (m_currentlines < 7)
            {
                if (a_message.Length > 40)
                {
                    m_Messages.Enqueue(m_prefix + a_message.Substring(0, 40));
                    m_Messages.Enqueue(a_message.Substring(39, a_message.Length - 40));
                    m_currentlines += 2;
                }
                else
                {
                    m_Messages.Enqueue(m_prefix + a_message);
                    m_currentlines++;
                }
            }
            //If there are 7 lines, add last line. Check if we need to remove a line.
            else if (m_currentlines == 7)
            {
                if (a_message.Length > 40)
                {
                    m_Messages.Dequeue();
                    m_Messages.Enqueue(m_prefix + a_message.Substring(0,40));
                    m_Messages.Enqueue(a_message.Substring(39, a_message.Length - 40));
                    m_currentlines += 2;
                }
                else
                {
                    m_Messages.Enqueue(m_prefix + a_message);
                    m_currentlines++;
                }
            }
            //If there are 8 lines, add line, and remove unneccesary lines.
            else
            {
                if (a_message.Length > 40)
                {
                    m_Messages.Dequeue();
                    m_Messages.Dequeue();
                    m_Messages.Enqueue(m_prefix + a_message.Substring(0, 40));
                    m_Messages.Enqueue(a_message.Substring(39, a_message.Length - 40));
                }
                else
                {
                    m_Messages.Dequeue();
                    m_Messages.Enqueue(m_prefix + a_message);
                }
            }
        }
    }
}
