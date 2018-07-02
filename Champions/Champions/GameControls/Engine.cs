using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Champions.GameControls
{
    /// <summary>
    /// Engine class: Controls all functionality of the game.
    /// </summary>
    class Engine
    {
        /// <summary>Mana cost for casting the 5th mage spell.</summary>
        private const int MANACOST_MAGE_5 = 3;
        /// <summary>Mana cost for casting the 2nd warrior spell.</summary>
        private const int MANACOST_WARRIOR_2 = 5;
        /// <summary>Mana cost for casting the 2nd mage spell.</summary>
        private const int MANACOST_MAGE_2 = 2;
        /// <summary>Mana cost for casting the 3rd mage spell.</summary>
        private const int MANACOST_MAGE_3 = 10;
        /// <summary>Mana cost for casting the 3rd warrior spell.</summary>
        private const int MANACOST_WARRIOR_3 = 5;
        /// <summary>Mana cost for casting the 4th mage spell.</summary>
        private const int MANACOST_MAGE_4 = 20;
        /// <summary>The gamestate. Controls the various states of the game.</summary>
        public enum GameState { Title, NewGame, LoadGame, ClassSelect, Playing, Pause, Status, Quit,
                                Multiplayer, Completion, GameOver }
        /// <summary>The current gamestate.</summary>
        private GameState m_gameState;
        /// <summary>The current keyboard state.</summary>
        KeyboardState m_currentKeyboardState;
        /// <summary>The previous keyboard state.</summary>
        KeyboardState m_previousKeyboardState;
        /// <summary>The current gamepad state.</summary>
        GamePadState m_previousGamepadState;
        /// <summary>The previous gamepad state.</summary>
        GamePadState m_currentGamepadState;
        /// <summary>A menu class.</summary>
        GameControls.Menu m_menu;
        /// <summary>A new game class.</summary>
        GameControls.NewGame m_newgame;
        /// <summary>A load game menu class.</summary>
        GameControls.LoadGame m_loadgame;
        /// <summary>A class select menu class.</summary>
        GameControls.ClassSelect m_classselect;
        /// <summary>A pause game menu class.</summary>
        GameControls.PauseMulti m_pauseMulti;
        /// <summary>Save slot 1.</summary>
        Entities.Player m_slot1;
        /// <summary>Save slot 2.</summary>
        Entities.Player m_slot2;
        /// <summary>Save slot 3.</summary>
        Entities.Player m_slot3;
        /// <summary>The currently active player.</summary>
        Entities.Player m_activePlayer;
        /// <summary>The current gameslot.</summary>
        int m_gameSlot;
        /// <summary>A map object. The current map, pooly named.</summary>
        LevelObjects.Map m_mapOne;
        /// <summary>A random number.</summary>
        Random rand;
        /// <summary>The cooldown for the potion timer.</summary>
        int potiontimer = 0;
        /// <summary>Texture for the game completed.</summary>
        Texture2D m_complete;
        /// <summary>Texture for the game over screen.</summary>
        Texture2D m_gameover;

        /// <summary>
        /// Constructor for the game. Initializes all class objects.
        /// </summary>
        public Engine()
        {
            m_gameState = GameState.Title;

            m_menu = new GameControls.Menu();
            m_newgame = new GameControls.NewGame();
            m_loadgame = new GameControls.LoadGame();
            m_classselect = new ClassSelect();
            m_activePlayer = new Entities.Player();
            m_mapOne = new LevelObjects.Map();
            m_pauseMulti = new GameControls.PauseMulti();
            m_slot1 = new Entities.Player();
            m_slot2 = new Entities.Player();
            m_slot3 = new Entities.Player();
            rand = new Random();

            ReadSaveString();
        }

        /// <summary>
        /// Gets or sets the current gamestate.
        /// </summary>
        public GameState State
        {
            get
            {
                return m_gameState;
            }
            set
            {
                m_gameState = value;
            }
        }
        /// <summary>
        /// Loads the menu object.
        /// </summary>
        /// <param name="a_menuTitle">The menu title screen.</param>
        /// <param name="a_menuTitleOptions">The options for the screen.</param>
        /// <param name="a_selectorGraphic">The selector for the menu.</param>
        /// <param name="a_titleSong">The song for the menu.</param>
        public void LoadMenu(Texture2D a_menuTitle, Texture2D a_menuTitleOptions, Texture2D a_selectorGraphic,
                        Song a_titleSong)
        {
            m_menu.InitializeMenu(a_menuTitle, a_menuTitleOptions, a_selectorGraphic, a_titleSong);
            m_menu.PlayTitleMusic();
        }
        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing to the screen.</param>
        public void DrawMenu(SpriteBatch a_spriteBatch)
        {
            m_menu.DrawMenu(a_spriteBatch);
        }
        /// <summary>
        /// Contains the controls from the Menu to navigate to the other sections, using user input.
        /// </summary>
        /// <param name="a_gameTime">Used for updating objects.</param>
        public void MenuUpdate(GameTime a_gameTime)
        {
            m_previousGamepadState = m_currentGamepadState;
            m_currentGamepadState = GamePad.GetState(PlayerIndex.One);
            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();

            if (m_currentGamepadState == m_previousGamepadState)
            {
                //Do nothing: Game too fast.
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickDown)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadDown))
            {
                m_menu.NextSelection();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickUp)
                        || m_currentGamepadState.IsButtonDown(Buttons.DPadUp))
            {
                m_menu.PreviousSelection();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.A))
            {
                switch (m_menu.GetSelection())
                {
                    case Menu.MenuSelection.Quit:
                        m_gameState = GameState.Quit;
                        break;
                    case Menu.MenuSelection.NewGame:
                        m_gameState = GameState.NewGame;
                        break;
                    case Menu.MenuSelection.LoadGame:
                        m_gameState = GameState.LoadGame;
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
            
            if (m_previousKeyboardState == m_currentKeyboardState)
            //|| m_currentGamepadState == m_previousGamepadState)
            {
                //Do nothing; game moving too fast!
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Down))
            {
                m_menu.NextSelection();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Up))
            {
                m_menu.PreviousSelection();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                switch (m_menu.GetSelection())
                {
                    case Menu.MenuSelection.Quit:
                        m_gameState = GameState.Quit;
                        break;
                    case Menu.MenuSelection.NewGame:
                        m_gameState = GameState.NewGame;
                        break;
                    case Menu.MenuSelection.LoadGame:
                        m_gameState = GameState.LoadGame;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //Do Nothing; no other event controls are neccessary on this screen
            }
        }
        /// <summary>
        /// Loads the new game object textures.
        /// </summary>
        /// <param name="a_newScreen">Texture for the newgame screen.</param>
        /// <param name="a_newSelector">Texture for the newgame selector.</param>
        public void LoadNewGame(Texture2D a_newScreen, Texture2D a_newSelector)
        {
            m_newgame.LoadTextures(a_newScreen, a_newSelector);
        }
        /// <summary>
        /// Draws newgame objects to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used to draw to the screen.</param>
        public void DrawNewGame(SpriteBatch a_spriteBatch)
        {
            m_newgame.DrawTextures(a_spriteBatch);
        }

        /// <summary>
        /// Updates the new game class.
        /// </summary>
        /// <param name="a_gametime">The gametime used for updates.</param>
        public void NewUpdate(GameTime a_gametime)
        {
            m_previousGamepadState = m_currentGamepadState;
            m_currentGamepadState = GamePad.GetState(PlayerIndex.One);
            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();

            if (m_currentGamepadState == m_previousGamepadState)
            {
                //Do nothing: too fast!
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft) 
                    || m_currentGamepadState.IsButtonDown(Buttons.DPadLeft))
            {
                m_newgame.PrevSelection();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadRight))
            {
                m_newgame.NextSelection();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.A))
            {
                m_gameSlot = m_newgame.GetSelection();
                m_activePlayer.InitializeWizard(m_gameSlot);
                State = GameState.ClassSelect;
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.B))
            {
                m_gameState = GameState.Title;
            }
            else
            {
                //nothing to do!
            }

            if (m_previousKeyboardState == m_currentKeyboardState)
            {
                //Do nothing; game moving too fast!
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Left))
            {
                m_newgame.PrevSelection();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Right))
            {
               m_newgame.NextSelection();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                m_gameSlot = m_newgame.GetSelection();
                m_activePlayer.InitializeWizard(m_gameSlot);
                State = GameState.ClassSelect;
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                m_gameState = GameState.Title;
            }
            else
            {
                //Do Nothing; no other event controls are neccessary on this screen
            }
        }

        /// <summary>
        /// Updates the load game object.
        /// </summary>
        /// <param name="a_gameTime">Gametime used for updating objects.</param>
        public void UpdateLoadGame(GameTime a_gameTime)
        {
            m_previousGamepadState = m_currentGamepadState;
            m_currentGamepadState = GamePad.GetState(PlayerIndex.One);
            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();

            if (m_previousGamepadState == m_currentGamepadState)
            {
                //Do nothing: game moving too fast!
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadLeft))
            {
                m_loadgame.Previous();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadRight))
            {
                m_loadgame.Next();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.A))
            {
                int choice = m_loadgame.GetLoadSlot();
                if (choice == 0)
                {
                    //Do nothing, the slot is empty
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            m_activePlayer = m_slot1;
                            break;
                        case 2:
                            m_activePlayer = m_slot2;
                            break;
                        case 3:
                            m_activePlayer = m_slot3;
                            break;
                    }
                    MediaPlayer.Stop();
                    m_gameState = GameState.Playing;
                }
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.B))
            {

            }
            else
            {
                //Do nothing!
            }



            if (m_previousKeyboardState == m_currentKeyboardState)
            {
                //Do nothing; game moving too fast!
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Left))
            {
                m_loadgame.Previous();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Right))
            {
                m_loadgame.Next();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                int choice = m_loadgame.GetLoadSlot();
                if (choice == 0)
                {
                    //Do nothing, the slot is empty
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            m_activePlayer = m_slot1;
                            break;
                        case 2:
                            m_activePlayer = m_slot2;
                            break;
                        case 3:
                            m_activePlayer = m_slot3;
                            break;
                    }
                    MediaPlayer.Stop();
                    m_gameState = GameState.Playing;
                }
                
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                m_gameState = GameState.Title;
            }
            else
            {
                //Nothing to do!
            }
        }

        /// <summary>
        /// Loads the class select object textures.
        /// </summary>
        /// <param name="a_classSelect">Class select screen.</param>
        /// <param name="a_classWarrior">Warrior selector.</param>
        /// <param name="a_classMage">Mage selector.</param>
        public void LoadClassSelect(Texture2D a_classSelect, Texture2D a_classWarrior, Texture2D a_classMage)
        {
            m_classselect.LoadClasses(a_classSelect, a_classWarrior, a_classMage);
        }
        /// <summary>
        /// Draws the class select object to the screen.
        /// </summary>
        /// <param name="a_spriteBatch"></param>
        public void DrawClassSelect(SpriteBatch a_spriteBatch)
        {
            m_classselect.DrawClasses(a_spriteBatch);
        }
        /// <summary>
        /// Updates the class select object.
        /// </summary>
        /// <param name="a_gameTime">Gametime used for updating.</param>
        public void UpdateClassSelect(GameTime a_gameTime)
        {

            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();
            m_previousGamepadState = m_currentGamepadState;
            m_currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (m_previousGamepadState == m_currentGamepadState)
            {
                //Do nothing: game moving too fast!
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)
                ||m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
                ||m_currentGamepadState.IsButtonDown(Buttons.DPadLeft)
                ||m_currentGamepadState.IsButtonDown(Buttons.DPadRight))
            {
                m_classselect.Next();
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.B))
            {
                m_gameState = GameState.Title;
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.A))
            {
                if (m_classselect.GetClass() == Entities.Player.Class.Warrior)
                {
                    m_activePlayer.InitializeWarrior(m_gameSlot);
                }
                else
                {
                    m_activePlayer.InitializeWizard(m_gameSlot);
                }
                m_mapOne.ChooseMap(1);
                m_gameState = GameState.Playing;
                MediaPlayer.Stop();
            }
            else
            {
                //nothing to do!
            }

            if (m_previousKeyboardState == m_currentKeyboardState)
            {
                //Do nothing; game moving too fast!
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Left) || m_currentKeyboardState.IsKeyDown(Keys.Right))
            {
                m_classselect.Next();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (m_classselect.GetClass() == Entities.Player.Class.Warrior)
                {
                    m_activePlayer.InitializeWarrior(m_gameSlot);
                }
                else
                {
                    m_activePlayer.InitializeWizard(m_gameSlot);
                }
                m_mapOne.ChooseMap(1);
                m_gameState = GameState.Playing;
                MediaPlayer.Stop();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                m_gameState = GameState.Title;
            }
            else
            {
                //Do Nothing; no other event controls are neccessary on this screen
            }
        }

        /// <summary>
        /// Draws the load gmae object to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">The spritebatch used for drawing.</param>
        public void DrawLoadGame(SpriteBatch a_spriteBatch)
        {
            m_loadgame.DrawLoadToScreen(a_spriteBatch, m_slot1, m_slot2, m_slot3);
        }
        /// <summary>
        /// Loads the loadgame object textures.
        /// </summary>
        /// <param name="a_fileSelect">The file select texture.</param>
        /// <param name="a_mage">The mage texture.</param>
        /// <param name="a_none">the none texture.</param>
        /// <param name="a_selector">The selector texture.</param>
        /// <param name="a_warrior">The warrior texture.</param>
        /// <param name="a_menu">The Spritefont for drawing.</param>
        public void LoadLoadGame(Texture2D a_fileSelect, Texture2D a_mage, Texture2D a_none,
                                Texture2D a_selector, Texture2D a_warrior, SpriteFont a_menu)
        {
            m_loadgame.DrawLoad(a_fileSelect, a_mage, a_none, a_selector, a_warrior, a_menu);
        }
        /// <summary>
        /// Loads the map object.
        /// </summary>
        /// <param name="a_mapOne">Map #1.</param>
        /// <param name="a_mapTwo">Map #2.</param>
        /// <param name="a_zombie">The zombie enemy texture.</param>
        /// <param name="a_enemyFont">The enemy SpriteFont.</param>
        /// <param name="a_tree_red">Red tree texture.</param>
        /// <param name="a_tree_green">Green tree texture.</param>
        /// <param name="a_tree_yellow">Yellow tree texture.</param>
        /// <param name="a_rock">Rock texture.</param>
        /// <param name="a_wall1">A wall texture.</param>
        /// <param name="a_wall2">2nd wall texture.</param>
        /// <param name="a_statue">Statue texture.</param>
        /// <param name="a_levelchanger">Level changing texture.</param>
        /// <param name="a_dragon">Dragon enemy texture.</param>
        /// <param name="a_doctor">Doctor enemy texture.</param>
        /// <param name="a_loot">Loot texture.</param>
        public void LoadMaps(Texture2D a_mapOne, Texture2D a_mapTwo, Texture2D a_zombie, SpriteFont a_enemyFont,
                            Texture2D a_tree_red, Texture2D a_tree_green, Texture2D a_tree_yellow, Texture2D a_rock,
                            Texture2D a_wall1, Texture2D a_wall2, Texture2D a_statue, Texture2D a_levelchanger,
                            Texture2D a_dragon, Texture2D a_doctor, Texture2D a_loot)
        {
            m_mapOne.LoadMaps(a_mapOne, a_mapTwo, a_zombie, a_enemyFont, a_tree_red, a_tree_green,
                a_tree_yellow, a_rock, a_wall1, a_wall2, a_statue, a_levelchanger, a_dragon, a_doctor, a_loot);
        }

        /// <summary>
        /// Reads the save string for saving.
        /// </summary>
        public void ReadSaveString()
        {
            Entities.Player[] tempPlayer = new Entities.Player[3];

            string [] attributes = new string[3];
            for (int i = 0; i < 3; i++)
            {
                string file = "slot" + (i+1);
                if (!File.Exists(file))
                {
                    tempPlayer[i] = null;
                }
                else
                {
                    TextReader tr = new StreamReader(file);
                    string player = tr.ReadLine();
                    if (player != null)
                    {
                        tempPlayer[i] = new Entities.Player(player);
                    }
                    else
                    {
                        tempPlayer[i] = null;
                    }
                    tr.Close();
                }
            }


            if (tempPlayer[0] != null) m_slot1.LoadPlayerAttributes(tempPlayer[0].SavePlayerString());

            if (tempPlayer[1] != null) m_slot2.LoadPlayerAttributes(tempPlayer[1].SavePlayerString());

            if (tempPlayer[2] != null) m_slot3.LoadPlayerAttributes(tempPlayer[2].SavePlayerString());
        }

        /// <summary>
        /// Saves game to a file from the player string.
        /// </summary>
        public void SaveGame()
        {
            string file = "slot" + m_gameSlot;
            TextWriter tw = new StreamWriter(file);
            tw.WriteLine(m_activePlayer.SavePlayerString());
            tw.Close();
        }

        /// <summary>
        /// Loads the player class textures and resources.
        /// </summary>
        /// <param name="a_warrior_Back_Idle">Back idle animation.</param>
        /// <param name="a_warrior_Back_Walk_X">X for 1-4, the backwards animation for warrior.</param>
        /// <param name="a_warrior_Front_Idle">Front idle animation for warrior.</param>
        /// <param name="a_warrior_Front_Walk_X">X for 1-4, the forwards animation for warrior.</param>
        /// <param name="a_warrior_Left_Idle">Left idle animation for warrior.</param>
        /// <param name="a_warrior_Left_Walk_X">X for 1-4, the left animation for warrior.</param>
        /// <param name="a_warrior_Right_Idle">Right idle animation for warrior.</param>
        /// <param name="a_warrior_Right_Walk_X">X for 1-4, the right animation for warrior.</param>
        /// <param name="a_wizard_Front_Idle">Front idle animation for wizard.</param>
        /// <param name="a_wizard_Front_Walk_X">X for 1-5, the front animation for wizard.</param>
        /// <param name="a_wizard_Back_Idle">Back idle animation for wizard.</param>
        /// <param name="a_wizard_Back_Walk_X">X for 1-5, the back animation for wizard.</param>
        /// <param name="a_wizard_Left_Idle">Left idle animation for wizard.</param>
        /// <param name="a_wizard_Left_Walk_X">X for 1-5, the left animation for wizard.</param>
        /// <param name="a_wizard_Right_Idle">Right idle animation for wizard.</param>
        /// <param name="a_wizard_Right_Walk_X">X for 1-5, the right animation for wizard.</param>
        /// <param name="a_UI_Basic">The UI to draw to the screen.</param>
        /// <param name="a_UI_Font">The spritefont to use for drawing.</param>
        /// <param name="a_UI_ExpHealth">Health bar spritefont.</param>
        /// <param name="a_UI_Healthbar">Healthbar sprite.</param>
        /// <param name="a_UI_Manabar">Mana bar sprite.</param>
        /// <param name="a_UI_Mage_1">Mage spell 1.</param>
        /// <param name="a_UI_Warrior_1">Warrior spell 1.</param>
        /// <param name="a_PauseScreen">Pause screen texture.</param>
        /// <param name="a_pauseMenu">Pause menu objects.</param>
        /// <param name="a_cursor">Cursor texture.</param>
        /// <param name="a_Addattribute">Addattribute texture.</param>
        /// <param name="a_A1">Warrior weapon 1 texture</param>
        /// <param name="a_A2">Warrior weapon 2 texture</param>
        /// <param name="a_A3">Warrior weapon 3 texture</param>
        /// <param name="a_S1">Spell weapon 1 texture</param>
        /// <param name="a_S2">Wizard weapon 2 texture</param>
        /// <param name="a_S3">Wizard weapon 3 texture</param>
        /// <param name="a_D1">Warrior armor 1 texture</param>
        /// <param name="a_D2">Warrior armor 2 texture</param>
        /// <param name="a_D3">Warrior armor 3 texture</param>
        /// <param name="a_D4">Wizard armor 1 texture</param>
        /// <param name="a_D5">Wizard armor 2texture</param>
        /// <param name="a_D6">Wizard armor 3 texture</param>
        public void LoadPlayerResources(Texture2D a_warrior_Back_Idle, 
                                        Texture2D a_warrior_Back_Walk_1,
                                        Texture2D a_warrior_Back_Walk_2,
                                        Texture2D a_warrior_Back_Walk_3,
                                        Texture2D a_warrior_Back_Walk_4,
                                        Texture2D a_warrior_Front_Idle,
                                        Texture2D a_warrior_Front_Walk_1,
                                        Texture2D a_warrior_Front_Walk_2,
                                        Texture2D a_warrior_Front_Walk_3,
                                        Texture2D a_warrior_Front_Walk_4,
                                        Texture2D a_warrior_Left_Idle,
                                        Texture2D a_warrior_Left_Walk_1,
                                        Texture2D a_warrior_Left_Walk_2,
                                        Texture2D a_warrior_Left_Walk_3,
                                        Texture2D a_warrior_Left_Walk_4,
                                        Texture2D a_warrior_Right_Idle,
                                        Texture2D a_warrior_Right_Walk_1,
                                        Texture2D a_warrior_Right_Walk_2,
                                        Texture2D a_warrior_Right_Walk_3,
                                        Texture2D a_warrior_Right_Walk_4,
                                        Texture2D a_wizard_Front_Idle,
                                        Texture2D a_wizard_Front_Walk_1,
                                        Texture2D a_wizard_Front_Walk_2,
                                        Texture2D a_wizard_Front_Walk_3,
                                        Texture2D a_wizard_Front_Walk_4,
                                        Texture2D a_wizard_Front_Walk_5,
                                        Texture2D a_wizard_Back_Idle,
                                        Texture2D a_wizard_Back_Walk_1,
                                        Texture2D a_wizard_Back_Walk_2,
                                        Texture2D a_wizard_Back_Walk_3,
                                        Texture2D a_wizard_Back_Walk_4,
                                        Texture2D a_wizard_Back_Walk_5,
                                        Texture2D a_wizard_Left_Idle,
                                        Texture2D a_wizard_Left_Walk_1,
                                        Texture2D a_wizard_Left_Walk_2,
                                        Texture2D a_wizard_Left_Walk_3,
                                        Texture2D a_wizard_Left_Walk_4,
                                        Texture2D a_wizard_Left_Walk_5,
                                        Texture2D a_wizard_Right_Idle,
                                        Texture2D a_wizard_Right_Walk_1,
                                        Texture2D a_wizard_Right_Walk_2,
                                        Texture2D a_wizard_Right_Walk_3,
                                        Texture2D a_wizard_Right_Walk_4,
                                        Texture2D a_wizard_Right_Walk_5,
                                        Texture2D a_UI_Basic,
                                        SpriteFont a_UI_Font,
                                        SpriteFont a_UI_ExpHealth,
                                        Texture2D a_UI_Healthbar,
                                        Texture2D a_UI_Manabar,
                                        Texture2D a_UI_Mage_1,
                                        Texture2D a_UI_Mage_2,
                                        Texture2D a_UI_Mage_3,
                                        Texture2D a_UI_Mage_4,
                                        Texture2D a_UI_Mage_5,
                                        Texture2D a_UI_Warrior_1,
                                        Texture2D a_UI_Warrior_2,
                                        Texture2D a_UI_Warrior_3,
                                        Texture2D a_UI_Warrior_4,
                                        Texture2D a_UI_Warrior_5,
                                        Texture2D a_PauseScreen,
                                        SpriteFont a_pauseMenu,
                                        Texture2D a_cursor,
                                        Texture2D a_Addattribute,
                                        Texture2D a_A1,     
                                        Texture2D a_A2, 
                                        Texture2D a_A3, 
                                        Texture2D a_S1, 
                                        Texture2D a_S2, 
                                        Texture2D a_S3,
                                        Texture2D a_D1, 
                                        Texture2D a_D2,
                                        Texture2D a_D3, 
                                        Texture2D a_D4, 
                                        Texture2D a_D5, 
                                        Texture2D a_D6) //End of Parameters
        {
            m_activePlayer.LoadPlayerTextures( a_warrior_Back_Idle, a_warrior_Back_Walk_1, a_warrior_Back_Walk_2,
                                         a_warrior_Back_Walk_3, a_warrior_Back_Walk_4, a_warrior_Front_Idle,
                                         a_warrior_Front_Walk_1, a_warrior_Front_Walk_2, a_warrior_Front_Walk_3,
                                         a_warrior_Front_Walk_4, a_warrior_Left_Idle, a_warrior_Left_Walk_1,
                                         a_warrior_Left_Walk_2, a_warrior_Left_Walk_3, a_warrior_Left_Walk_4,
                                         a_warrior_Right_Idle, a_warrior_Right_Walk_1, a_warrior_Right_Walk_2,
                                         a_warrior_Right_Walk_3, a_warrior_Right_Walk_4, a_wizard_Front_Idle,
                                         a_wizard_Front_Walk_1, a_wizard_Front_Walk_2, a_wizard_Front_Walk_3,
                                         a_wizard_Front_Walk_4, a_wizard_Front_Walk_5, a_wizard_Back_Idle,
                                         a_wizard_Back_Walk_1, a_wizard_Back_Walk_2, a_wizard_Back_Walk_3,
                                         a_wizard_Back_Walk_4, a_wizard_Back_Walk_5, a_wizard_Left_Idle,
                                         a_wizard_Left_Walk_1, a_wizard_Left_Walk_2, a_wizard_Left_Walk_3,
                                         a_wizard_Left_Walk_4, a_wizard_Left_Walk_5, a_wizard_Right_Idle,
                                         a_wizard_Right_Walk_1, a_wizard_Right_Walk_2, a_wizard_Right_Walk_3,
                                         a_wizard_Right_Walk_4, a_wizard_Right_Walk_5 );
            m_activePlayer.LoadUI(a_UI_Basic, a_UI_Font, a_UI_ExpHealth, a_UI_Healthbar, a_UI_Manabar,
                                    a_UI_Mage_1, a_UI_Mage_2, a_UI_Mage_3, a_UI_Mage_4, a_UI_Mage_5,
                                    a_UI_Warrior_1, a_UI_Warrior_2, a_UI_Warrior_3, a_UI_Warrior_4, a_UI_Warrior_5);
            

            m_pauseMulti.LoadPause(a_PauseScreen, a_pauseMenu, a_cursor, a_Addattribute, 
                                    a_A1, a_A2, a_A3, a_S1, a_S2, a_S3, a_D1, a_D2, a_D3, a_D4, a_D5, a_D6);

            m_slot1.LoadPlayerTextures(a_warrior_Back_Idle, a_warrior_Back_Walk_1, a_warrior_Back_Walk_2,
                             a_warrior_Back_Walk_3, a_warrior_Back_Walk_4, a_warrior_Front_Idle,
                             a_warrior_Front_Walk_1, a_warrior_Front_Walk_2, a_warrior_Front_Walk_3,
                             a_warrior_Front_Walk_4, a_warrior_Left_Idle, a_warrior_Left_Walk_1,
                             a_warrior_Left_Walk_2, a_warrior_Left_Walk_3, a_warrior_Left_Walk_4,
                             a_warrior_Right_Idle, a_warrior_Right_Walk_1, a_warrior_Right_Walk_2,
                             a_warrior_Right_Walk_3, a_warrior_Right_Walk_4, a_wizard_Front_Idle,
                             a_wizard_Front_Walk_1, a_wizard_Front_Walk_2, a_wizard_Front_Walk_3,
                             a_wizard_Front_Walk_4, a_wizard_Front_Walk_5, a_wizard_Back_Idle,
                             a_wizard_Back_Walk_1, a_wizard_Back_Walk_2, a_wizard_Back_Walk_3,
                             a_wizard_Back_Walk_4, a_wizard_Back_Walk_5, a_wizard_Left_Idle,
                             a_wizard_Left_Walk_1, a_wizard_Left_Walk_2, a_wizard_Left_Walk_3,
                             a_wizard_Left_Walk_4, a_wizard_Left_Walk_5, a_wizard_Right_Idle,
                             a_wizard_Right_Walk_1, a_wizard_Right_Walk_2, a_wizard_Right_Walk_3,
                             a_wizard_Right_Walk_4, a_wizard_Right_Walk_5);
            m_slot1.LoadUI(a_UI_Basic, a_UI_Font, a_UI_ExpHealth, a_UI_Healthbar, a_UI_Manabar,
                                    a_UI_Mage_1, a_UI_Mage_2, a_UI_Mage_3, a_UI_Mage_4, a_UI_Mage_5,
                                    a_UI_Warrior_1, a_UI_Warrior_2, a_UI_Warrior_3, a_UI_Warrior_4, a_UI_Warrior_5);
            m_slot2.LoadPlayerTextures(a_warrior_Back_Idle, a_warrior_Back_Walk_1, a_warrior_Back_Walk_2,
                 a_warrior_Back_Walk_3, a_warrior_Back_Walk_4, a_warrior_Front_Idle,
                 a_warrior_Front_Walk_1, a_warrior_Front_Walk_2, a_warrior_Front_Walk_3,
                 a_warrior_Front_Walk_4, a_warrior_Left_Idle, a_warrior_Left_Walk_1,
                 a_warrior_Left_Walk_2, a_warrior_Left_Walk_3, a_warrior_Left_Walk_4,
                 a_warrior_Right_Idle, a_warrior_Right_Walk_1, a_warrior_Right_Walk_2,
                 a_warrior_Right_Walk_3, a_warrior_Right_Walk_4, a_wizard_Front_Idle,
                 a_wizard_Front_Walk_1, a_wizard_Front_Walk_2, a_wizard_Front_Walk_3,
                 a_wizard_Front_Walk_4, a_wizard_Front_Walk_5, a_wizard_Back_Idle,
                 a_wizard_Back_Walk_1, a_wizard_Back_Walk_2, a_wizard_Back_Walk_3,
                 a_wizard_Back_Walk_4, a_wizard_Back_Walk_5, a_wizard_Left_Idle,
                 a_wizard_Left_Walk_1, a_wizard_Left_Walk_2, a_wizard_Left_Walk_3,
                 a_wizard_Left_Walk_4, a_wizard_Left_Walk_5, a_wizard_Right_Idle,
                 a_wizard_Right_Walk_1, a_wizard_Right_Walk_2, a_wizard_Right_Walk_3,
                 a_wizard_Right_Walk_4, a_wizard_Right_Walk_5);
            m_slot2.LoadUI(a_UI_Basic, a_UI_Font, a_UI_ExpHealth, a_UI_Healthbar, a_UI_Manabar,
                                    a_UI_Mage_1, a_UI_Mage_2, a_UI_Mage_3, a_UI_Mage_4, a_UI_Mage_5,
                                    a_UI_Warrior_1, a_UI_Warrior_2, a_UI_Warrior_3, a_UI_Warrior_4, a_UI_Warrior_5);
            m_slot3.LoadPlayerTextures(a_warrior_Back_Idle, a_warrior_Back_Walk_1, a_warrior_Back_Walk_2,
                 a_warrior_Back_Walk_3, a_warrior_Back_Walk_4, a_warrior_Front_Idle,
                 a_warrior_Front_Walk_1, a_warrior_Front_Walk_2, a_warrior_Front_Walk_3,
                 a_warrior_Front_Walk_4, a_warrior_Left_Idle, a_warrior_Left_Walk_1,
                 a_warrior_Left_Walk_2, a_warrior_Left_Walk_3, a_warrior_Left_Walk_4,
                 a_warrior_Right_Idle, a_warrior_Right_Walk_1, a_warrior_Right_Walk_2,
                 a_warrior_Right_Walk_3, a_warrior_Right_Walk_4, a_wizard_Front_Idle,
                 a_wizard_Front_Walk_1, a_wizard_Front_Walk_2, a_wizard_Front_Walk_3,
                 a_wizard_Front_Walk_4, a_wizard_Front_Walk_5, a_wizard_Back_Idle,
                 a_wizard_Back_Walk_1, a_wizard_Back_Walk_2, a_wizard_Back_Walk_3,
                 a_wizard_Back_Walk_4, a_wizard_Back_Walk_5, a_wizard_Left_Idle,
                 a_wizard_Left_Walk_1, a_wizard_Left_Walk_2, a_wizard_Left_Walk_3,
                 a_wizard_Left_Walk_4, a_wizard_Left_Walk_5, a_wizard_Right_Idle,
                 a_wizard_Right_Walk_1, a_wizard_Right_Walk_2, a_wizard_Right_Walk_3,
                 a_wizard_Right_Walk_4, a_wizard_Right_Walk_5);
            m_slot3.LoadUI(a_UI_Basic, a_UI_Font, a_UI_ExpHealth, a_UI_Healthbar, a_UI_Manabar,
                                    a_UI_Mage_1, a_UI_Mage_2, a_UI_Mage_3, a_UI_Mage_4, a_UI_Mage_5,
                                    a_UI_Warrior_1, a_UI_Warrior_2, a_UI_Warrior_3, a_UI_Warrior_4, a_UI_Warrior_5);
        }
        /// <summary>
        /// Draws the game assets to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="a_gameTime">Gametime object for updating.</param>
        public void DrawGameResources(SpriteBatch a_spriteBatch, GameTime a_gameTime)
        {
            //m_mapOne.ChooseMap(m_activePlayer.GetMap());
            m_mapOne.DrawCurrentMap(a_spriteBatch, m_activePlayer.GetPosition());

            m_activePlayer.DrawPlayerTextures(a_spriteBatch);
            m_activePlayer.DrawUI(a_spriteBatch);

            if (m_activePlayer.GetClass() == Entities.Player.Class.Warrior)
            {
                m_activePlayer.AnimateWarrior(a_gameTime);
            }
            else
            {
                m_activePlayer.AnimateWizard(a_gameTime);
            }
        }

        /// <summary>
        /// Game manager is the central updater for the game.
        /// </summary>
        /// <param name="a_gameTime">Gametime for updating objects.</param>
        public void GameManager(GameTime a_gameTime)
        {
            //m_mapOne.ChooseMap(m_activePlayer.GetMap());
            //m_mapOne.UpdateMusic();
            m_mapOne.UpdateObjects(m_activePlayer.GetRectangle());
            m_mapOne.CheckAttackCollision(ref m_activePlayer);
            m_activePlayer.UpdateManaRegen();
            if (m_mapOne.CheckWinGame()) m_gameState = GameState.Completion;

            if (m_activePlayer.GetCurrentHealth() <= 0) m_gameState = GameState.GameOver;

            if (MultiplayerManager.IsMulti() && !MultiplayerManager.IsServer())
            {
                MultiplayerManager.ListenForServerMessages();
                //MultiplayerManager.SendToServerClientInfo();
            }
            else if (MultiplayerManager.IsMulti() && MultiplayerManager.IsServer())
            {
                //MultiplayerManager.ListenForClientMessages();
                //MultiplayerManager.SendToClientServerInfo();
            }

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                DoGamepadInput();
            }
            else
            {
                DoKeyboardInput();
            }
        }

        /// <summary>
        /// Manages input for gamepads.
        /// </summary>
        public void DoGamepadInput()
        {
            int speed = m_activePlayer.GetSpeed();
            m_previousGamepadState = m_currentGamepadState;
            m_currentGamepadState = GamePad.GetState(PlayerIndex.One);


            //Manage the keyboardstate

            //Move the player diagonal down-left.
            if ((m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickDown)
               && m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)) ||
                m_currentGamepadState.IsButtonDown(Buttons.DPadLeft) &&
                m_currentGamepadState.IsButtonDown(Buttons.DPadDown))
            {
                Controls_MoveLeft();
                Controls_MoveDown();
            }
            //Move the player diagonal down-right
            else if ((m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickDown)
               && m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)) ||
                m_currentGamepadState.IsButtonDown(Buttons.DPadRight) &&
                m_currentGamepadState.IsButtonDown(Buttons.DPadDown))
            {
                Controls_MoveRight();
                Controls_MoveDown();
            }
            //Move the player diagonal up-right
            else if ((m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
               && m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickUp)) ||
                m_currentGamepadState.IsButtonDown(Buttons.DPadRight) &&
                m_currentGamepadState.IsButtonDown(Buttons.DPadUp))
            {
                Controls_MoveRight();
                Controls_MoveUp();
            }
            //Move the player diagonal up-Left
            else if ((m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)
               && m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickUp)) ||
                m_currentGamepadState.IsButtonDown(Buttons.DPadLeft) &&
                m_currentGamepadState.IsButtonDown(Buttons.DPadUp))
            {
                Controls_MoveLeft();
                Controls_MoveUp();
            }
            //Move the player to the left.
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickLeft)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadLeft))
            {
                Controls_MoveLeft();
            }
            //Move the player to the right.
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickRight)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadRight))
            {
                Controls_MoveRight();
            }
            //Move the player up.
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickUp)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadUp))
            {
                Controls_MoveUp();
            }
            //Move the player down.
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftThumbstickDown)
                || m_currentGamepadState.IsButtonDown(Buttons.DPadDown))
            {
                Controls_MoveDown();
            }
            else
            {
                m_activePlayer.PlayerIdle();
                //Do Nothing; no other event controls are neccessary on this screen
            }
            //Slow down player input.
            if (m_previousGamepadState == m_currentGamepadState)
            {
                //Too fast!
            }
            //Use a health potion if available
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftShoulder))
            {
                m_activePlayer.AddHealth(5 * m_activePlayer.GetLevel());
                potiontimer = 0;
            }
            //Use a mana potion if available
            else if (m_currentGamepadState.IsButtonDown(Buttons.RightShoulder))
            {
                m_activePlayer.AddMana(5 * m_activePlayer.GetLevel());
                potiontimer = 0;
            }
            //Use the spell in slot #1
            else if (m_currentGamepadState.IsButtonDown(Buttons.RightTrigger))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    CombatControl.IssueAttack(m_activePlayer, 1);
                }
                else // class is warrior
                {
                    CombatControl.IssueAttack(m_activePlayer, 0);
                }
            }
            //Use the spell in slot #2
            else if (m_currentGamepadState.IsButtonDown(Buttons.X))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_2))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 2);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_2))
                    {
                        m_activePlayer.RemoveAllTempStats();
                        m_activePlayer.AddTempStat(Entities.Character.Stat.Defense, 5);
                    }
                }
            }
            //Use the spell in slot #3
            else if (m_currentGamepadState.IsButtonDown(Buttons.Y))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 3);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 5);
                    }
                }
            }
            //Use the spell in slot #4
            else if (m_currentGamepadState.IsButtonDown(Buttons.B))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 4);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 6);
                    }
                }
            }
            //Use the spell in slot #5
            else if (m_currentGamepadState.IsButtonDown(Buttons.LeftTrigger))
            {
                //Mage Spell: Blink
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_5))
                    {
                        switch (m_activePlayer.GetState())
                        {
                            case Entities.Player.State.IdleDown:
                                for (int i = 0; i < 50; i++) Controls_MoveDown();
                                break;
                            case Entities.Player.State.MoveDown:
                                for (int i = 0; i < 50; i++) Controls_MoveDown();
                                break;
                            case Entities.Player.State.IdleUp:
                                for (int i = 0; i < 50; i++) Controls_MoveUp();
                                break;
                            case Entities.Player.State.MoveUp:
                                for (int i = 0; i < 50; i++) Controls_MoveUp();
                                break;
                            case Entities.Player.State.IdleLeft:
                                for (int i = 0; i < 50; i++) Controls_MoveLeft();
                                break;
                            case Entities.Player.State.MoveLeft:
                                for (int i = 0; i < 50; i++) Controls_MoveLeft();
                                break;
                            case Entities.Player.State.IdleRight:
                                for (int i = 0; i < 50; i++) Controls_MoveRight();
                                break;
                            case Entities.Player.State.MoveRight:
                                for (int i = 0; i < 50; i++) Controls_MoveRight();
                                break;
                        }
                    }
                }
                //Warrior spell: Heal
                else //current class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(1 + m_activePlayer.GetLevel()))
                    {
                        m_activePlayer.AddHealth(m_activePlayer.GetLevel() * 3);
                    }
                }
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.Start))
            {
                m_gameState = GameState.Pause;
            }
            else if (m_currentGamepadState.IsButtonDown(Buttons.A))
            {
                if (m_mapOne.CheckChests(m_activePlayer.GetRectangle()))
                {
                    int item = rand.Next(1, 64);
                    if (item <= 10) m_activePlayer.GiveItem(0);
                    else if (item <= 20) m_activePlayer.GiveItem(3);
                    else if (item <= 30) m_activePlayer.GiveItem(6);
                    else if (item <= 40) m_activePlayer.GiveItem(9);
                    else if (item <= 45) m_activePlayer.GiveItem(1);
                    else if (item <= 50) m_activePlayer.GiveItem(4);
                    else if (item <= 55) m_activePlayer.GiveItem(7);
                    else if (item <= 60) m_activePlayer.GiveItem(10);
                    else if (item == 61) m_activePlayer.GiveItem(2);
                    else if (item == 62) m_activePlayer.GiveItem(5);
                    else if (item == 63) m_activePlayer.GiveItem(8);
                    else m_activePlayer.GiveItem(11);
                }
            }
            else
            {
                m_activePlayer.PlayerIdle();
                //Do Nothing; no other event controls are neccessary on this screen
            }
        }

        /// <summary>
        /// Manages input for keyboards.
        /// </summary>
        public void DoKeyboardInput()
        {
            int speed = m_activePlayer.GetSpeed();
            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();
            //Manage the keyboardstate
            //Check to see if we are typing
            if (Chat.IsChatMode())
            {
                Chat.UseChatInput();
            }
            //Move the player diagonal down-left.
            else if ((m_currentKeyboardState.IsKeyDown(Keys.Down) && m_currentKeyboardState.IsKeyDown(Keys.Left)) ||
                        m_currentKeyboardState.IsKeyDown(Keys.A) && m_currentKeyboardState.IsKeyDown(Keys.S))
            {
                Controls_MoveLeft();
                Controls_MoveDown();
            }
            //Move the player diagonal down-right
            else if ((m_currentKeyboardState.IsKeyDown(Keys.Down) && m_previousKeyboardState.IsKeyDown(Keys.Right)) ||
                    m_currentKeyboardState.IsKeyDown(Keys.D) && m_previousKeyboardState.IsKeyDown(Keys.S))
            {
                Controls_MoveRight();
                Controls_MoveDown();
            }
            //Move the player diagonal up-right
            else if ((m_currentKeyboardState.IsKeyDown(Keys.Right) && m_previousKeyboardState.IsKeyDown(Keys.Up)) ||
                    m_currentKeyboardState.IsKeyDown(Keys.D) && m_previousKeyboardState.IsKeyDown(Keys.W))
            {
                Controls_MoveRight();
                Controls_MoveUp();
            }
            //Move the player diagonal up-Left
            else if ((m_currentKeyboardState.IsKeyDown(Keys.Left) && m_previousKeyboardState.IsKeyDown(Keys.Up)) ||
                    m_currentKeyboardState.IsKeyDown(Keys.A) && m_previousKeyboardState.IsKeyDown(Keys.W))
            {
                Controls_MoveLeft();
                Controls_MoveUp();
            }
            //Move the player to the left.
            else if (m_currentKeyboardState.IsKeyDown(Keys.Left) || m_currentKeyboardState.IsKeyDown(Keys.A))
            {
                Controls_MoveLeft();
            }
            //Move the player to the right.
            else if (m_currentKeyboardState.IsKeyDown(Keys.Right) || m_currentKeyboardState.IsKeyDown(Keys.D))
            {
                Controls_MoveRight();
            }
            //Move the player up.
            else if (m_currentKeyboardState.IsKeyDown(Keys.Up) || m_currentKeyboardState.IsKeyDown(Keys.W))
            {
                Controls_MoveUp();
            }
            //Move the player down.
            else if (m_currentKeyboardState.IsKeyDown(Keys.Down) || m_currentKeyboardState.IsKeyDown(Keys.S))
            {
                Controls_MoveDown();
            }
            else
            {
                m_activePlayer.PlayerIdle();
                //Do Nothing; no other event controls are neccessary on this screen
            }
            //Slow down player input.
            if (m_currentKeyboardState == m_previousKeyboardState)
            {
                //Too fast!
            }
            //Use a health potion if available
            else if (m_currentKeyboardState.IsKeyDown(Keys.Q))
            {
                m_activePlayer.AddHealth(5 * m_activePlayer.GetLevel());
                potiontimer = 0;
            }
            //Use a mana potion if available
            else if (m_currentKeyboardState.IsKeyDown(Keys.E))
            {
                m_activePlayer.AddMana(5 * m_activePlayer.GetLevel());
                potiontimer = 0;
            }
            //Use the spell in slot #1
            else if (m_currentKeyboardState.IsKeyDown(Keys.D1))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    CombatControl.IssueAttack(m_activePlayer, 1);
                }
                else // class is warrior
                {
                    CombatControl.IssueAttack(m_activePlayer, 0);
                }
            }
            //Use the spell in slot #2
            else if (m_currentKeyboardState.IsKeyDown(Keys.D2))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_2))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 2);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_2))
                    {
                        m_activePlayer.RemoveAllTempStats();
                        m_activePlayer.AddTempStat(Entities.Character.Stat.Defense, 5);
                    }
                }
            }
            //Use the spell in slot #3
            else if (m_currentKeyboardState.IsKeyDown(Keys.D3))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 3);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 5);
                    }
                }
            }
            //Use the spell in slot #4
            else if (m_currentKeyboardState.IsKeyDown(Keys.D4))
            {
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 4);
                    }
                }
                else // class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_WARRIOR_3))
                    {
                        CombatControl.IssueAttack(m_activePlayer, 6);
                    }
                }
            }
            //Use the spell in slot #5
            else if (m_currentKeyboardState.IsKeyDown(Keys.D5))
            {
                //Mage Spell: Blink
                if (m_activePlayer.GetClass() == Entities.Player.Class.Wizard)
                {
                    if (m_activePlayer.ReduceCurrentMana(MANACOST_MAGE_5))
                    {
                        switch (m_activePlayer.GetState())
                        {
                            case Entities.Player.State.IdleDown:
                                for (int i = 0; i < 50; i++) Controls_MoveDown();
                                break;
                            case Entities.Player.State.MoveDown:
                                for (int i = 0; i < 50; i++) Controls_MoveDown();
                                break;
                            case Entities.Player.State.IdleUp:
                                for (int i = 0; i < 50; i++) Controls_MoveUp();
                                break;
                            case Entities.Player.State.MoveUp:
                                for (int i = 0; i < 50; i++) Controls_MoveUp();
                                break;
                            case Entities.Player.State.IdleLeft:
                                for (int i = 0; i < 50; i++) Controls_MoveLeft();
                                break;
                            case Entities.Player.State.MoveLeft:
                                for (int i = 0; i < 50; i++) Controls_MoveLeft();
                                break;
                            case Entities.Player.State.IdleRight:
                                for (int i = 0; i < 50; i++) Controls_MoveRight();
                                break;
                            case Entities.Player.State.MoveRight:
                                for (int i = 0; i < 50; i++) Controls_MoveRight();
                                break;
                        }
                    }
                }
                //Warrior spell: Heal
                else //current class is warrior
                {
                    if (m_activePlayer.ReduceCurrentMana(1 + m_activePlayer.GetLevel()))
                    {
                        m_activePlayer.AddHealth(m_activePlayer.GetLevel() * 3);
                    }
                }
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                m_gameState = GameState.Pause;
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                Chat.EnterChatMode();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.Space))
            {
                if (m_mapOne.CheckChests(m_activePlayer.GetRectangle()))
                {
                    int item = rand.Next(1, 64);
                    if (item <= 10) m_activePlayer.GiveItem(0);
                    else if (item <= 20) m_activePlayer.GiveItem(3);
                    else if (item <= 30) m_activePlayer.GiveItem(6);
                    else if (item <= 40) m_activePlayer.GiveItem(9);
                    else if (item <= 45) m_activePlayer.GiveItem(1);
                    else if (item <= 50) m_activePlayer.GiveItem(4);
                    else if (item <= 55) m_activePlayer.GiveItem(7);
                    else if (item <= 60) m_activePlayer.GiveItem(10);
                    else if (item == 61) m_activePlayer.GiveItem(2);
                    else if (item == 62) m_activePlayer.GiveItem(5);
                    else if (item == 63) m_activePlayer.GiveItem(8);
                    else m_activePlayer.GiveItem(11);
                }
            }
            else if ( m_currentKeyboardState.IsKeyDown(Keys.F1))
            {
                MultiplayerManager.SetupServer("127.0.0.1");
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.F2))
            {
                MultiplayerManager.SendDebug();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.F3))
            {
                MultiplayerManager.SetupClient();
            }
            else if (m_currentKeyboardState.IsKeyDown(Keys.F4))
            {
            }
            else
            {
                m_activePlayer.PlayerIdle();
                //Do Nothing; no other event controls are neccessary on this screen
            }
        }

        /// <summary>
        /// Moves left, checking collision.
        /// </summary>
        public void Controls_MoveLeft()
        {
            //Check map collision
            if ( m_mapOne.CheckCollision(new Rectangle(
                                         m_activePlayer.GetRectangle().X - m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Y, 
                                         m_activePlayer.GetRectangle().Width, 
                                         m_activePlayer.GetRectangle().Height)))
            {
                //Do nothing
            }
            else if (m_mapOne.CheckLevelChange(new Rectangle(
                                         m_activePlayer.GetRectangle().X - m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Y,
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {
                m_activePlayer.ChangeMap(2);
            }
            //If the map is on the left edge, move the player to the left
            else if (m_mapOne.MapEdgeLeft())
            {
                m_activePlayer.MovePlayerLeft(true);
            }
            //If the player is on the right quadrant of the screen, move the player left
            else if (m_activePlayer.PlayerUncenteredRight())
            {
                m_activePlayer.MovePlayerLeft(true);
            }
            //Otherwise, move the map
            else
            {
                m_mapOne.MoveRight(m_activePlayer.GetSpeed());
                m_activePlayer.MovePlayerLeft(false);
            }
        }
        /// <summary>
        /// Moves right, checks collision.
        /// </summary>
        public void Controls_MoveRight()
        {
            //Check map collision
            if (m_mapOne.CheckCollision(new Rectangle(
                                         m_activePlayer.GetRectangle().X + m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Y,
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {
                //Do nothing
            }
            //Check change level condition
            else if (m_mapOne.CheckLevelChange(new Rectangle(
                                         m_activePlayer.GetRectangle().X + m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Y,
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {

            }
            //If the map is on the right edge, move the player to the right
            else if (m_mapOne.MapEdgeRight())
            {
                m_activePlayer.MovePlayerRight(true);
            }
            //If the player is on the right quadrant of the screen, move the player right
            else if (m_activePlayer.PlayerUncenteredLeft())
            {
                m_activePlayer.MovePlayerRight(true);
            }
            //Otherwise, move the map
            else
            {
                m_mapOne.MoveLeft(m_activePlayer.GetSpeed());
                m_activePlayer.MovePlayerRight(false);
            }
        }
        /// <summary>
        /// Moves up, checks collision.
        /// </summary>
        public void Controls_MoveUp()
        {
            //Check map collision
            if (m_mapOne.CheckCollision(new Rectangle(
                                         m_activePlayer.GetRectangle().X,
                                         m_activePlayer.GetRectangle().Y - m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {
                //Do nothing
            }
            //Check change level condition
            else if (m_mapOne.CheckLevelChange(new Rectangle(
                                         m_activePlayer.GetRectangle().X,
                                         m_activePlayer.GetRectangle().Y - m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {

            }
            //If the map is on the top edge, move the player to the up
            else if (m_mapOne.MapEdgeUp())
            {
                m_activePlayer.MovePlayerUp(true);
            }
            //If the player is on the bottom quadrant of the screen, move them up
            else if (m_activePlayer.PlayerUncenteredDown())
            {
                m_activePlayer.MovePlayerUp(true);
            }
            //Otherwise, move the map
            else
            {
                m_mapOne.MoveDown(m_activePlayer.GetSpeed());
                m_activePlayer.MovePlayerUp(false);
            }
        }
        /// <summary>
        /// Moves down, checks collision.
        /// </summary>
        public void Controls_MoveDown()
        {
            //Check map collision
            if (m_mapOne.CheckCollision(new Rectangle(
                                         m_activePlayer.GetRectangle().X,
                                         m_activePlayer.GetRectangle().Y + m_activePlayer.GetSpeed(),
                                         m_activePlayer.GetRectangle().Width,
                                         m_activePlayer.GetRectangle().Height)))
            {
                //Do nothing
            }
                //Check change level condition
            else if (m_mapOne.CheckLevelChange(new Rectangle(
                                     m_activePlayer.GetRectangle().X,
                                     m_activePlayer.GetRectangle().Y + m_activePlayer.GetSpeed(),
                                     m_activePlayer.GetRectangle().Width,
                                     m_activePlayer.GetRectangle().Height)))
            {
                
            }
            //If the map is on the bottom edge, move player down
            else if (m_mapOne.MapEdgeDown())
            {
                m_activePlayer.MovePlayerDown(true);
            }
            //If the player is on the top quadrant of the screen, move them down
            else if (m_activePlayer.PlayerUncenteredUp())
            {
                m_activePlayer.MovePlayerDown(true);
            }
            //Otherwise, move the map
            else
            {
                m_mapOne.MoveUp(m_activePlayer.GetSpeed());
                m_activePlayer.MovePlayerDown(false);
            }
        }

        /// <summary>
        /// Loads music soundtracks for random selection.
        /// </summary>
        /// <param name="a_0">Song 1.</param>
        /// <param name="a_1">Song 2.</param>
        /// <param name="a_2">Song 3.</param>
        /// <param name="a_3">Song 4.</param>
        /// <param name="a_4">Song 5.</param>
        /// <param name="a_5">Song 6.</param>
        public void LoadMusic(Song a_0, Song a_1, Song a_2, Song a_3, Song a_4, Song a_5)
        {
            m_mapOne.LoadMusic(a_0, a_1, a_2, a_3, a_4, a_5);
        }

        /// <summary>
        /// Draws the pause game textures to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing.</param>
        public void PauseGame(SpriteBatch a_spriteBatch)
        {
            m_activePlayer.RemoveAllTempStats();
            m_activePlayer.CreateAbstracts();

            m_pauseMulti.DrawPause(a_spriteBatch, ref m_activePlayer);
        }
        /// <summary>
        /// Updates the pause screen.
        /// </summary>
        /// <param name="a_gametime">Object used for updating the pause state.</param>
        public void UpdatePause(GameTime a_gametime)
        {
            m_gameState = m_pauseMulti.HandlePause(a_gametime, ref m_activePlayer);

            switch (m_pauseMulti.CheckAttributes())
            {
                case 0:
                    break;
                case 1:
                    m_activePlayer.AddStrength();
                    break;
                case 2:
                    m_activePlayer.AddSpeed();
                    break;
                case 3:
                    m_activePlayer.AddAgility();
                    break;
                case 4:
                    m_activePlayer.AddIntelligence();
                    break;
                default:
                    break;
            }

            if (m_pauseMulti.CheckSave())
            {
                SaveGame();
            }
        }
        /// <summary>
        /// Loads the texture for victory.
        /// </summary>
        /// <param name="a_complete">Completegame Texture.</param>
        public void LoadCompleteGame(Texture2D a_complete)
        {
            m_complete = a_complete;
        }
        /// <summary>
        /// Manages the completegame and draws it to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing.</param>
        public void CompleteGame(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Draw(m_complete, new Vector2(0, 0), Color.White);

            m_currentKeyboardState = Keyboard.GetState();
            if ( m_currentKeyboardState.IsKeyDown(Keys.Escape)) m_gameState = GameState.Quit;
        }
        /// <summary>
        /// Loads the gameover texture to the variable.
        /// </summary>
        /// <param name="a_gameover">Gameover texture.</param>
        public void LoadGameOver(Texture2D a_gameover)
        {
            m_gameover = a_gameover;
        }

        /// <summary>
        /// Draws and updates the gameover texture to the screen.
        /// </summary>
        /// <param name="a_spritebatch">Spritebatch used for drawing.</param>
        public void GameOver(SpriteBatch a_spritebatch)
        {
            a_spritebatch.Draw(m_gameover, new Vector2(0, 0), Color.White);
            m_currentKeyboardState = Keyboard.GetState();
            if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                m_gameState = GameState.Quit;
            }
        }

        //**MULTI:009**// This section currently incomplete.
        //public String PullMultiplayerClientUpdate()
        //{

        //}
        //public String PullMultiplayerServerUpdate()
        //{

        //}
        //public void ParseMultiplayerUpdateFromClient(String a_parse)
        //{

        //}
        //public void ParseMultiplayerUpdateFromServer(String a_parse)
        //{

        //}
    }   //End of Class Definition
}   //end of Namespace Definition
