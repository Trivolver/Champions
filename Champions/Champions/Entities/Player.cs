using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Champions.Entities
{
    /// <summary>
    /// This class controls all functions having to do with human-controller players.
    /// </summary>
    class Player : Character 
    {
        /// <summary>Total number of items in the game.</summary>
        private const int NUMBER_OF_ITEMS = 12;
        /// <summary>Boolean aray, initialized to number of items. Each number will be set to true or false.</summary>
        public bool[] m_HasItems;
        /** Enumeration to define classes as a Data Type.
         * Warrior Class: Heavy Hitter, melee damage dealer
         * Wizard Class: Spell-caster class
         **/
        public enum Class { Wizard, Warrior };
        /// <summary> This variable represents the in-game class as defined by enumeration.</summary>
        private Class m_class;
        /// <summary> 
        /// The variable representing which save slot to be used. Can be 1, 2, or 3. There is no default, it is initialized by Engine.
        /// </summary>
        public int m_saveSlot;
        /// <summary>
        /// The current map of the game. Can currently only be 1 or 2.
        /// </summary>
        private int m_currentMap;

        /// <summary> Graphic representing the warrior facing away.</summary>
        Texture2D m_warrior_Back_Idle;
        /// <summary> Graphic array representing the warrior walking away.</summary>
        Texture2D [] m_warrior_Back_Walk;
        /// <summary> Graphic representing the warrior facing front.</summary>
        Texture2D m_warrior_Front_Idle;
        /// <summary> Graphic array representing the warrior walking front.</summary>
        Texture2D []m_warrior_Front_Walk;
        /// <summary> Graphic representing the warrior facing left.</summary>
        Texture2D m_warrior_Left_Idle;
        /// <summary> Graphic array representing the warrior walking left.</summary>
        Texture2D []m_warrior_Left_Walk;
        /// <summary> Graphic representing the warrior facing right.</summary>
        Texture2D m_warrior_Right_Idle;
        /// <summary> Graphic array representing the warrior walking right.</summary>
        Texture2D []m_warrior_Right_Walk;

        /// <summary> Graphic representing the wizard facing front.</summary>
        Texture2D m_wizard_Front_Idle;
        /// <summary> Graphic array representing the wizard walking front.</summary>
        Texture2D [] m_wizard_Front_Walk;
        /// <summary> Graphic representing the wizard facing back.</summary>
        Texture2D m_wizard_Back_Idle;
        /// <summary> Graphic array representing the wizard walking back.</summary>
        Texture2D [] m_wizard_Back_Walk;
        /// <summary> Graphic representing the wizard facing left.</summary>
        Texture2D m_wizard_Left_Idle;
        /// <summary> Graphic array representing the wizard walking left.</summary>
        Texture2D [] m_wizard_Left_Walk;
        /// <summary> Graphic representing the wizard facing right.</summary>
        Texture2D m_wizard_Right_Idle;
        /// <summary> Graphic array representing the wizard walking right.</summary>
        Texture2D []m_wizard_Right_Walk;

        /// <summary> Graphic representing the wizard's spell slot 1. </summary>
        Texture2D m_UI_Mage_1;
        /// <summary> Graphic representing the wizard's spell slot 2. </summary>
        Texture2D m_UI_Mage_2;
        /// <summary> Graphic representing the wizard's spell slot 3. </summary>
        Texture2D m_UI_Mage_3;
        /// <summary> Graphic representing the wizard's spell slot 4. </summary>
        Texture2D m_UI_Mage_4;
        /// <summary> Graphic representing the wizard's spell slot 5. </summary>
        Texture2D m_UI_Mage_5;
        /// <summary> Graphic representing the warrior's spell slot 1. </summary>
        Texture2D m_UI_Warrior_1;
        /// <summary> Graphic representing the warrior's spell slot 2. </summary>
        Texture2D m_UI_Warrior_2;
        /// <summary> Graphic representing the warrior's spell slot 3. </summary>
        Texture2D m_UI_Warrior_3;
        /// <summary> Graphic representing the warrior's spell slot 4. </summary>
        Texture2D m_UI_Warrior_4;
        /// <summary> Graphic representing the warrior's spell slot 5. </summary>
        Texture2D m_UI_Warrior_5;

        /// <summary> Graphic representing the user interface. </summary>
        Texture2D m_UI_Basic;
        /// <summary> Graphical font for some of the UI. </summary>
        SpriteFont m_UI_Font;
        /// <summary> Graphical font for the EXP, health and mana bars. </summary>
        SpriteFont m_UI_expHealth;
        /// <summary> Graphic for the health bar. </summary>
        Texture2D m_UI_Healthbar;
        /// <summary> Graphic for the mana bar. </summary>
        Texture2D m_UI_Manabar;
        /// <summary> Graphic holding the active texture for rendering.</summary>
        Texture2D m_activeTexture;

        /// <summary> Timer based on update rate. </summary>
        int m_timer = 0;
        /// <summary> Interval to use for rendering graphic animation. </summary>
        int m_interval = 15;
        /// <summary> Current frame of the animation array.</summary>
        int m_currentFrame;
        /// <summary> Frame count for the total frame array. </summary>
        int m_frameCount;
        /// <summary>Timer used to restore mana.</summary>
        int m_updateManaRegenTimer = 0;
        /// <summary>Timer limit to restore mana.</summary>
        int m_updateManaRegenMax = 300;
        /// <summary>Timer used to restore health.</summary>
        int m_updateHealthRegenTimer = 0;
        /// <summary>Timer limit to restore health.</summary>
        int m_updateHealthRegenMax = 1000;
        /// <summary>Index of the array corresponding to the currently equipped weapon.</summary>
        int m_equippedItem;
        /// <summary>Index of the array corresponding to the currently equipped armor.</summary>
        int m_equippedArmor;

        /// <summary>
        /// Initialize a player using a predetermined player. Centers them on-screen.
        /// </summary>
        /// <param name="a_player">The player to copy.</param>
        public Player(Player a_player)
        {
            m_HasItems = new bool[NUMBER_OF_ITEMS];
            for (int i = 0; i < NUMBER_OF_ITEMS; i++) m_HasItems[i] = false;
            m_currentFrame = 0;
            m_position = new Vector2(640, 360);
            LoadPlayer(a_player);
            CreateAbstracts();

        }
        /// <summary>
        /// Initialize a player center-screen.
        /// </summary>
        public Player()
        {
            m_HasItems = new bool[NUMBER_OF_ITEMS];
            for (int i = 0; i < NUMBER_OF_ITEMS; i++) m_HasItems[i] = false;
            m_currentFrame = 0;
            m_position = new Vector2(640, 360);
        }
        /// <summary>
        /// Initialize a player using a savestring, or a string sent from socket.
        /// </summary>
        /// <param name="a_playerString">The string to parse.</param>
        public Player(String a_playerString)
        {
            m_HasItems = new bool[NUMBER_OF_ITEMS];
            for (int i = 0; i < NUMBER_OF_ITEMS; i++) m_HasItems[i] = false;
            LoadPlayerAttributes( a_playerString );
            CreateAbstracts();
            m_isEnemy = false;
        }

        /// <summary>
        /// Loads a player from a save file using a string.
        /// </summary>
        /// <param name="a_playerString">The string to load the player from.</param>
        public void LoadPlayerAttributes ( String a_playerString )
        {
            m_position = new Vector2(640, 360);
            //Start by breaking the string by slot number
            String[] variables = a_playerString.Split('&');

            switch (variables[0])
            {
                case "slot1":
                    m_saveSlot = 1;
                    break;
                case "slot2":
                    m_saveSlot = 2;
                    break;
                case "slot3":
                    m_saveSlot = 3;
                    break;
                default:
                    break;
            }

            //Split the string into each of its attributes.
            //Parse the attributes by string name.
            String[] attributes = a_playerString.Split(' ');
            foreach (string att in attributes)
            {
                String[] variable = att.Split(':');
                switch (variable[0])
                {
                    case "assignable":
                        m_assignable = Int32.Parse(variable[1]);
                        break;
                    case "maxhealth":
                        m_maxhealth = Int32.Parse(variable[1]);
                        break;
                    case "currenthealth":
                        m_currenthealth = Int32.Parse(variable[1]);
                        break;
                    case "maxmana":
                        m_maxmana = Int32.Parse(variable[1]);
                        break;
                    case "currentmana":
                        m_currentmana = Int32.Parse(variable[1]);
                        break;
                    case "strength":
                        m_strength = Int32.Parse(variable[1]);
                        break;
                    case "speed":
                        m_speed = Int32.Parse(variable[1]);
                        break;
                    case "agility":
                        m_agility = Int32.Parse(variable[1]);
                        break;
                    case "intelligence":
                        m_intelligence = Int32.Parse(variable[1]);
                        break;
                    case "defense":
                        m_defense = Int32.Parse(variable[1]);
                        break;
                    case "attack":
                        m_attack = Int32.Parse(variable[1]);
                        break;
                    case "spellpower":
                        m_spellpower = Int32.Parse(variable[1]);
                        break;
                    case "attackspeed":
                        m_attackspeed = Int32.Parse(variable[1]);
                        break;
                    case "movementspeed":
                        m_movementspeed = Int32.Parse(variable[1]);
                        break;
                    case "class":
                        if (variable[1] == "Warrior")
                        {
                            m_class = Class.Warrior;
                        }
                        else if (variable[1] == "Wizard")
                        {
                            m_class = Class.Wizard;
                        }
                        else
                        {
                            //Do nothing
                        }
                        break;
                    case "tempmaxhealth":
                        m_temp_maxhealth = Int32.Parse(variable[1]);
                        break;
                    case "tempmaxmana":
                        m_temp_maxmana = Int32.Parse(variable[1]);
                        break;
                    case "tempstrength":
                        m_temp_strength = Int32.Parse(variable[1]);
                        break;
                    case "tempspeed":
                        m_temp_speed = Int32.Parse(variable[1]);
                        break;
                    case "tempagility":
                        m_temp_agility = Int32.Parse(variable[1]);
                        break;
                    case "tempintelligence":
                        m_temp_intelligence = Int32.Parse(variable[1]);
                        break;
                    case "tempdefense":
                        m_temp_defense = Int32.Parse(variable[1]);
                        break;
                    case "tempattack":
                        m_temp_attack = Int32.Parse(variable[1]);
                        break;
                    case "tempspellpower":
                        m_temp_spellpower = Int32.Parse(variable[1]);
                        break;
                    case "tempattackspeed":
                        m_temp_attackspeed = Int32.Parse(variable[1]);
                        break;
                    case "tempmovementspeed":
                        m_temp_movementspeed = Int32.Parse(variable[1]);
                        break;
                    case "map":
                        m_currentMap = Int32.Parse(variable[1]);
                        break;
                    case "level":
                        m_currentlevel = Int32.Parse(variable[1]);
                        break;
                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// Gets the current class from the Class enumeration.
        /// </summary>
        /// <returns>Returns the class of the player.</returns>
        public Class GetClass()
        {
            return m_class;
        }

        /// <summary>
        /// This method constructs a string to be saved to a file for the sake of serialization. It loads all of the player 
        /// attributes as well as the save slot to be used. 
        /// </summary>
        /// <returns>The string to be saved to file. Can be recovered here by constructor or the LoadPlayer() function.</returns>
        public String SavePlayerString()
        {
            string saveString = "";

            //Add all stats to the string
            saveString += "slot" + m_saveSlot + "&";
            saveString += "assignable:" + m_assignable + " ";
            saveString += "maxhealth:" + m_maxhealth + " ";
            saveString += "currenthealth:" + m_currenthealth + " ";
            saveString += "maxmana:" + m_maxmana + " ";
            saveString += "currentmana:" + m_currentmana  +" ";
            saveString += "strength:" + m_strength + " ";
            saveString += "speed:" + m_speed + " ";
            saveString += "agility:" + m_agility + " ";
            saveString += "intelligence:" + m_intelligence + " ";
            saveString += "defense:" + m_defense + " ";
            saveString += "attack:" + m_attack + " ";
            saveString += "spellpower:" + m_spellpower + " ";
            saveString += "attackspeed:" + m_attackspeed + " ";
            saveString += "movementspeed:" + m_movementspeed + " ";
            saveString += "class:" + m_class + " ";

            saveString += "tempmaxhealth:" + m_temp_maxhealth + " ";
            saveString += "tempmaxmana:" + m_temp_maxmana + " ";
            saveString += "tempstrength:" + m_temp_strength + " ";
            saveString += "tempspeed:" + m_temp_speed + " ";
            saveString += "tempagility:" + m_temp_agility + " ";
            saveString += "tempintelligence:" + m_temp_intelligence + " ";
            saveString += "tempdefense:" + m_temp_defense + " ";
            saveString += "tempattack:" + m_temp_attack + " ";
            saveString += "tempspellpower:" + m_temp_spellpower + " ";
            saveString += "tempattackspeed:" + m_temp_attackspeed + " ";
            saveString += "tempmovementspeed:" + m_temp_movementspeed + " ";
            saveString += "map:" + m_currentMap + " ";
            saveString += "level:" + m_currentlevel + " ";
            //Add all items to the string
            //saveString += "&";
            //for (int i; i < m_items.Length; i++)
            //{
            //    //If there is no item, don't add it. Else, add it.
            //    if (m_items[i][0] == 0)
            //        break;
            //    else
            //    {
            //        saveString += 
            //    }
            //}

            return saveString;
        }

        /// <summary>
        /// Loads a player using specific stats and attributes.
        /// </summary>
        /// <param name="a_assignable">Assignable points.</param>
        /// <param name="a_maxhealth">Maximum health.</param>
        /// <param name="a_currenthealth">Current health.</param>
        /// <param name="a_maxmana">Maximum Mana.</param>
        /// <param name="a_currentmana">Current Mana.</param>
        /// <param name="a_strength">Player's Strength</param>
        /// <param name="a_speed">Player's Speed</param>
        /// <param name="a_agility">Player's Agility</param>
        /// <param name="a_intelligence">Player's Intelligence</param>
        /// <param name="a_defense">Player's Defense</param>
        /// <param name="a_attack">Player's Attack</param>
        /// <param name="a_spellpower">Player's Spellpower</param>
        /// <param name="a_attackspeed">Player's attack speed</param>
        /// <param name="a_movementspeed">Player's movement speed</param>
        /// <param name="a_class">Player's class</param>
        /// <param name="a_saveSlot">Save slot for saving game</param>
        private void LoadPlayer(
            int a_assignable, int a_maxhealth, int a_currenthealth, int a_maxmana, int a_currentmana,
            int a_strength, int a_speed, int a_agility, int a_intelligence,
            int a_defense, int a_attack, int a_spellpower, int a_attackspeed, int a_movementspeed,
            Class a_class, int a_saveSlot)
        {
            m_assignable = a_assignable;
            m_maxhealth = a_maxhealth;
            m_currenthealth = a_currenthealth;
            m_maxmana = a_maxmana;
            m_currentmana = a_currentmana;
            m_strength = a_strength;
            m_speed = a_speed;
            m_agility = a_agility;
            m_intelligence = a_intelligence;
            m_defense = a_defense;
            m_attack = a_attack;
            m_spellpower = a_spellpower;
            m_attackspeed = a_attackspeed;
            m_movementspeed = a_movementspeed;
            m_class = a_class;
            m_saveSlot = a_saveSlot;

        }

        /// <summary>
        /// Loads a player from another player by extrapolating stats.
        /// </summary>
        /// <param name="a_player"></param>
        private void LoadPlayer(Player a_player)
        {
            LoadPlayer(a_player.m_assignable, a_player.m_maxhealth, a_player.m_currenthealth, a_player.m_maxmana,
                        a_player.m_currentmana, a_player.m_strength, a_player.m_speed, a_player.m_agility, 
                        a_player.m_intelligence, a_player.m_defense, a_player.m_attack, a_player.m_spellpower, 
                        a_player.m_attackspeed, a_player.m_movementspeed, a_player.m_class, a_player.m_saveSlot);
        }

        /// <summary>
        /// Initializes the player as the wizard class using a specific set of starting stats.
        /// </summary>
        /// <param name="a_saveSlot">The slot used for saving.</param>
        public void InitializeWizard(int a_saveSlot)
        {
            m_class = Class.Wizard;

            m_assignable = 5;
            m_strength =5;
            m_agility = 5;
            m_intelligence = 10;
            m_speed = 6;

            m_maxhealth = 25;
            m_maxmana = 40;
            m_currenthealth = m_maxhealth;
            m_currentmana = m_maxmana;
            m_currentlevel = 1;
            m_currentexp = 0;
            m_currentMap = 1;

            m_saveSlot = a_saveSlot;
            m_HasItems[0] = true;
            m_HasItems[9] = true;
            m_activeTexture = m_wizard_Front_Idle;
            CreateAbstracts();

            m_equippedItem = 0;
            m_equippedArmor = 9;
           
        }
        /// <summary>
        /// Initializes the player as the warrior class using a specific set of starting stats.
        /// </summary>
        /// <param name="a_saveSlot">The slot used for saving.</param>
        public void InitializeWarrior(int a_saveSlot)
        {
            m_class = Class.Warrior;

            m_assignable = 5;
            m_strength = 10;
            m_agility = 5;
            m_intelligence = 3;
            m_speed = 3;

            m_maxhealth = 40;
            m_maxmana = 25;
            m_currentmana = m_maxmana;
            m_currenthealth = m_maxhealth;
            m_currentexp = 0;

            m_currentlevel = 1;
            m_currentMap = 1;

            m_saveSlot = a_saveSlot;

            m_HasItems[3] = true;
            m_HasItems[6] = true;
            for (int i = 0; i < 12; i++)
            {
                if (i == 3 || i == 6) { }
                else m_HasItems[i] = false;
            }

            m_activeTexture = m_warrior_Front_Idle;
            CreateAbstracts();

            m_equippedItem = 3;
            m_equippedArmor = 6;
        }

        /// <summary>
        /// Increases a player's current exp after completing an event. 
        /// </summary>
        /// <param name="a_expAmount">The amount to increase the exp by.</param>
        public void GainExp(int a_expAmount)
        {
            //Add the exp to the current exp
            m_currentexp += a_expAmount;
            //Check condition for a level up
            if (m_currentexp >= m_exptolevel[m_currentlevel - 1])
            {
                LevelUp();
            }
            //Check if the player is level capped
            else if (m_currentlevel == LEVEL_CAP)
            {
                m_currentexp = EXP_CAP;
            }
            else
            {
                //Nothing to do When you don't meet a level up condition, or level cap
            }
        }

        /// <summary>
        /// Levels up the player if he meets the condition for a level up.
        /// Calls the level up function per class.
        /// </summary>
        private void LevelUp()
        {
            if (m_currentlevel < 20) m_currentlevel++;
            
            m_currentexp = 0;
            if (m_class == Class.Warrior)
            {
                LevelUpWarrior();
            }
            else
            {
                LevelUpWizard();
            }
            m_currenthealth = m_maxhealth;
            m_currentmana = m_maxmana;
        }
        /// <summary>
        /// Levels up the warrior using a specific stat set.
        /// </summary>
        private void LevelUpWarrior()
        {
            AddStats(5, 40, 10, 3, 1, 1, 1);
        }
        /// <summary>
        /// Levels up the wizard using a specific stat set.
        /// </summary>
        private void LevelUpWizard()
        {
            AddStats(5, 20, 40, 1, 1, 1, 3);
        }
        /// <summary>
        /// Adds a set of base stats to a player based on parameters
        /// (use this for leveling up).
        /// </summary>
        /// <param name="a_assignable">Nmber of assignable points to add</param>
        /// <param name="a_maxhealth">Number of health points to add</param>
        /// <param name="a_maxmana">Number of mana points to add</param>
        /// <param name="a_strength">Number of strength points to add</param>
        /// <param name="a_speed">Number of speed points to add</param>
        /// <param name="a_agility">Number of agility points to add</param>
        /// <param name="a_intelligence">Number of intelligence points to add</param>
        public void AddStats(
            int a_assignable, int a_maxhealth, int a_maxmana,
            int a_strength, int a_speed, int a_agility, int a_intelligence)
        {
            m_assignable += a_assignable;
            m_maxhealth += a_maxhealth;
            m_maxmana += a_maxmana;

            m_strength += a_strength;
            m_speed += a_speed;
            m_agility += a_agility;
            m_intelligence += a_intelligence;
        }
        /// <summary>
        /// Use to find the player's save slot.
        /// </summary>
        /// <returns>Integer representing the current save slot (0-3).</returns>
        public int GetSaveSlot()
        {
            return m_saveSlot;
        }
        /// <summary>
        /// Function Assigning all textures to the player.
        /// </summary>
        /// <param name="a_warrior_Back_Idle">Texture2D warrior Backwards idle position.</param>
        /// <param name="a_warrior_Back_Walk_1">Texture warrior Walking backwards 1/4</param>
        /// <param name="a_warrior_Back_Walk_2">Texture warrior Walking backwards 2/4</param>
        /// <param name="a_warrior_Back_Walk_3">Texture warrior Walking backwards 3/4</param>
        /// <param name="a_warrior_Back_Walk_4">Texture warrior Walking backwards 4/4</param>
        /// <param name="a_warrior_Front_Idle">Texture warrior Front idle position.</param>
        /// <param name="a_warrior_Front_Walk_1">Texture warrior Walking forwards 1/4</param>
        /// <param name="a_warrior_Front_Walk_2">Texture warrior Walking forwards 2/4</param>
        /// <param name="a_warrior_Front_Walk_3">Texture warrior Walking forwards 3/4</param>
        /// <param name="a_warrior_Front_Walk_4">Texture warrior Walking forwards 4/4</param>
        /// <param name="a_warrior_Left_Idle">Texture warrior left idle position</param>
        /// <param name="a_warrior_Left_Walk_1">Texture warrior Walking Left 1/4</param>
        /// <param name="a_warrior_Left_Walk_2">Texture warrior Walking Left 2/4</param>
        /// <param name="a_warrior_Left_Walk_3">Texture warrior Walking Left 3/4</param>
        /// <param name="a_warrior_Left_Walk_4">Texture warrior Walking Left 4/4</param>
        /// <param name="a_warrior_Right_Idle">Texture warrior Right idle position</param>
        /// <param name="a_warrior_Right_Walk_1">Texture warrior Walking Right 1/4</param>
        /// <param name="a_warrior_Right_Walk_2">Texture warrior Walking Right 2/4</param>
        /// <param name="a_warrior_Right_Walk_3">Texture warrior Walking Right 3/4</param>
        /// <param name="a_warrior_Right_Walk_4">Texture warrior Walking Right 4/4</param>
        /// <param name="a_wizard_Front_Idle">Texture wizard idle front</param>
        /// <param name="a_wizard_Front_Walk_1">Texture wizard walking forward 1/5</param>
        /// <param name="a_wizard_Front_Walk_2">Texture wizard walking forward 2/5</param>
        /// <param name="a_wizard_Front_Walk_3">Texture wizard walking forward 3/5</param>
        /// <param name="a_wizard_Front_Walk_4">Texture wizard walking forward 4/5</param>
        /// <param name="a_wizard_Front_Walk_5">Texture wizard walking forward 5/5</param>
        /// <param name="a_wizard_Back_Idle">Texture wizard idle back</param>
        /// <param name="a_wizard_Back_Walk_1">Texture wizard walking backwards 1/5</param>
        /// <param name="a_wizard_Back_Walk_2">Texture wizard walking backwards 2/5</param>
        /// <param name="a_wizard_Back_Walk_3">Texture wizard walking backwards 3/5</param>
        /// <param name="a_wizard_Back_Walk_4">Texture wizard walking backwards 4/5</param>
        /// <param name="a_wizard_Back_Walk_5">Texture wizard walking backwards 5/5</param>
        /// <param name="a_wizard_Left_Idle">Texture wizard idle left</param>
        /// <param name="a_wizard_Left_Walk_1">Texture wizard walking left 1/5</param>
        /// <param name="a_wizard_Left_Walk_2">Texture wizard walking left 2/5</param>
        /// <param name="a_wizard_Left_Walk_3">Texture wizard walking left 3/5</param>
        /// <param name="a_wizard_Left_Walk_4">Texture wizard walking left 4/5</param>
        /// <param name="a_wizard_Left_Walk_5">Texture wizard walking left 5/5</param>
        /// <param name="a_wizard_Right_Idle">Texture wizard idle right</param>
        /// <param name="a_wizard_Right_Walk_1">Texture wizard walking right 1/5</param>
        /// <param name="a_wizard_Right_Walk_2">Texture wizard walking right 2/5</param>
        /// <param name="a_wizard_Right_Walk_3">Texture wizard walking right 3/5</param>
        /// <param name="a_wizard_Right_Walk_4">Texture wizard walking right 4/5</param>
        /// <param name="a_wizard_Right_Walk_5">Texture wizard walking right 5/5</param>
        public void LoadPlayerTextures(Texture2D a_warrior_Back_Idle,
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
                                        Texture2D a_wizard_Right_Walk_5)    //End of parameters
        {
            m_warrior_Back_Idle = a_warrior_Back_Idle;
            m_warrior_Back_Walk = new Texture2D [4];
            m_warrior_Back_Walk[0] = a_warrior_Back_Walk_1;
            m_warrior_Back_Walk[1] = a_warrior_Back_Walk_2;
            m_warrior_Back_Walk[2] = a_warrior_Back_Walk_3;
            m_warrior_Back_Walk[3] = a_warrior_Back_Walk_4;

            m_warrior_Front_Idle = a_warrior_Front_Idle;
            m_warrior_Front_Walk = new Texture2D[4];
            m_warrior_Front_Walk[0] = a_warrior_Front_Walk_1;
            m_warrior_Front_Walk[1] = a_warrior_Front_Walk_2;
            m_warrior_Front_Walk[2] = a_warrior_Front_Walk_3;
            m_warrior_Front_Walk[3] = a_warrior_Front_Walk_4;

            m_warrior_Right_Idle = a_warrior_Right_Idle;
            m_warrior_Right_Walk = new Texture2D[4];
            m_warrior_Right_Walk[0] = a_warrior_Right_Walk_1;
            m_warrior_Right_Walk[1] = a_warrior_Right_Walk_2;
            m_warrior_Right_Walk[2] = a_warrior_Right_Walk_3;
            m_warrior_Right_Walk[3] = a_warrior_Right_Walk_4;

            m_warrior_Left_Idle = a_warrior_Left_Idle;
            m_warrior_Left_Walk = new Texture2D[4];
            m_warrior_Left_Walk[0] = a_warrior_Left_Walk_1;
            m_warrior_Left_Walk[1] = a_warrior_Left_Walk_2;
            m_warrior_Left_Walk[2] = a_warrior_Left_Walk_3;
            m_warrior_Left_Walk[3] = a_warrior_Left_Walk_4;

            m_wizard_Front_Idle = a_wizard_Front_Idle;
            m_wizard_Front_Walk = new Texture2D[5];
            m_wizard_Front_Walk[0] = a_wizard_Front_Walk_1;
            m_wizard_Front_Walk[1] = a_wizard_Front_Walk_2;
            m_wizard_Front_Walk[2] = a_wizard_Front_Walk_3;
            m_wizard_Front_Walk[3] = a_wizard_Front_Walk_4;
            m_wizard_Front_Walk[4] = a_wizard_Front_Walk_5;
            m_wizard_Back_Walk = new Texture2D[5];
            m_wizard_Back_Idle = a_wizard_Back_Idle;
            m_wizard_Back_Walk[0] = a_wizard_Back_Walk_1;
            m_wizard_Back_Walk[1] = a_wizard_Back_Walk_2;
            m_wizard_Back_Walk[2] = a_wizard_Back_Walk_3;
            m_wizard_Back_Walk[3] = a_wizard_Back_Walk_4;
            m_wizard_Back_Walk[4] = a_wizard_Back_Walk_5;
            m_wizard_Left_Walk = new Texture2D[5];
            m_wizard_Left_Idle = a_wizard_Left_Idle;
            m_wizard_Left_Walk[0] = a_wizard_Left_Walk_1;
            m_wizard_Left_Walk[1] = a_wizard_Left_Walk_2;
            m_wizard_Left_Walk[2] = a_wizard_Left_Walk_3;
            m_wizard_Left_Walk[3] = a_wizard_Left_Walk_4;
            m_wizard_Left_Walk[4] = a_wizard_Left_Walk_5;
            m_wizard_Right_Idle = a_wizard_Right_Idle;
            m_wizard_Right_Walk = new Texture2D[5];
            m_wizard_Right_Walk[0] = a_wizard_Right_Walk_1;
            m_wizard_Right_Walk[1] = a_wizard_Right_Walk_2;
            m_wizard_Right_Walk[2] = a_wizard_Right_Walk_3;
            m_wizard_Right_Walk[3] = a_wizard_Right_Walk_4;
            m_wizard_Right_Walk[4] = a_wizard_Right_Walk_5;
        }
        /// <summary>
        /// Loads all the graphics for the User Interface.
        /// </summary>
        /// <param name="a_UI_Basic">Texture for the basic user interface.</param>
        /// <param name="a_UI_Font">Spritefont for the screen.</param>
        /// <param name="a_UI_ExpHealth">Spritefront for the EXP, Health, and mana bars.</param>
        /// <param name="a_UI_Healthbar">Texture for the health bar.</param>
        /// <param name="a_UI_Manabar">Texture for the mana bar.</param>
        /// <param name="a_UI_Mage_1">Texture for the mage spell 1/5.</param>
        /// <param name="a_UI_Mage_2">Texture for the mage spell 2/5.</param>
        /// <param name="a_UI_Mage_3">Texture for the mage spell 3/5.</param>
        /// <param name="a_UI_Mage_4">Texture for the mage spell 4/5.</param>
        /// <param name="a_UI_Mage_5">Texture for the mage spell 5/5.</param>
        /// <param name="a_UI_Warrior_1">Texture for the warrior ability 1/5.</param>
        /// <param name="a_UI_Warrior_2">Texture for the warrior ability 2/5.</param>
        /// <param name="a_UI_Warrior_3">Texture for the warrior ability 3/5.</param>
        /// <param name="a_UI_Warrior_4">Texture for the warrior ability 4/5.</param>
        /// <param name="a_UI_Warrior_5">Texture for the warrior ability 5/5.</param>
        public void LoadUI(Texture2D a_UI_Basic, SpriteFont a_UI_Font, SpriteFont a_UI_ExpHealth,
                            Texture2D a_UI_Healthbar, Texture2D a_UI_Manabar,
                            Texture2D a_UI_Mage_1, Texture2D a_UI_Mage_2, Texture2D a_UI_Mage_3,
                            Texture2D a_UI_Mage_4, Texture2D a_UI_Mage_5,
                            Texture2D a_UI_Warrior_1, Texture2D a_UI_Warrior_2, Texture2D a_UI_Warrior_3,
                            Texture2D a_UI_Warrior_4, Texture2D a_UI_Warrior_5)
        {
            m_UI_Basic = a_UI_Basic;
            m_UI_Font = a_UI_Font;
            m_UI_expHealth = a_UI_ExpHealth;
            m_UI_Healthbar = a_UI_Healthbar;
            m_UI_Manabar = a_UI_Manabar;
            m_UI_Warrior_1 = a_UI_Warrior_1;
            m_UI_Warrior_2 = a_UI_Warrior_2;
            m_UI_Warrior_3 = a_UI_Warrior_3;
            m_UI_Warrior_4 = a_UI_Warrior_4;
            m_UI_Warrior_5 = a_UI_Warrior_5;
            m_UI_Mage_1 = a_UI_Mage_1;
            m_UI_Mage_2 = a_UI_Mage_2;
            m_UI_Mage_3 = a_UI_Mage_3;
            m_UI_Mage_4 = a_UI_Mage_4;
            m_UI_Mage_5 = a_UI_Mage_5;
        }

        /// <summary>
        /// Draws UI to screen.
        /// </summary>
        /// <param name="a_spriteBatch">Used to draw textures to screen.</param>
        public void DrawUI(SpriteBatch a_spriteBatch)
        {
            //The percentage of the currnet exp to the next level.
            int expPct = 100 * m_currentexp / m_exptolevel[m_currentlevel - 1];
            int healthPct = 100 * m_currenthealth / m_maxhealth;
            int manaPct = 100 * m_currentmana / m_maxmana;

            //Draw the character spell icons
            if (m_class == Class.Wizard)
            {
                a_spriteBatch.Draw(m_UI_Mage_1, new Vector2(490, 651), Color.White);
                a_spriteBatch.Draw(m_UI_Mage_2, new Vector2(558, 651), Color.White);
                a_spriteBatch.Draw(m_UI_Mage_3, new Vector2(625, 651), Color.White);
                a_spriteBatch.Draw(m_UI_Mage_4, new Vector2(693, 648), Color.White);
                a_spriteBatch.Draw(m_UI_Mage_5, new Vector2(763, 649), Color.White);
            }
            else
            {
                a_spriteBatch.Draw(m_UI_Warrior_1, new Vector2(488, 648), Color.White);
                a_spriteBatch.Draw(m_UI_Warrior_2, new Vector2(556, 647), Color.White);
                a_spriteBatch.Draw(m_UI_Warrior_3, new Vector2(624, 649), Color.White);
                a_spriteBatch.Draw(m_UI_Warrior_4, new Vector2(694, 647), Color.White);
                a_spriteBatch.Draw(m_UI_Warrior_5, new Vector2(760, 648), Color.White);
            }

            //Draw the healthbar
            a_spriteBatch.Draw(m_UI_Healthbar, new Rectangle(74, 7, m_UI_Healthbar.Width * healthPct / 100, 
                                m_UI_Healthbar.Height), Color.White);
            //Draw the manabar
            a_spriteBatch.Draw(m_UI_Manabar, new Rectangle(75, 46, m_UI_Manabar.Width * manaPct / 100,
                                m_UI_Healthbar.Height), Color.White);
            //Draw the UI
            a_spriteBatch.Draw(m_UI_Basic, new Vector2(0, 0), Color.White);
            //Draw the Current Level #
            a_spriteBatch.DrawString(m_UI_Font, m_currentlevel.ToString(), new Vector2(45, 50), Color.White);
            //Draw the Exp Percentage
            a_spriteBatch.DrawString(m_UI_expHealth, expPct + "%", new Vector2(145, 707), Color.White);
            //Draw the current health / max health
            a_spriteBatch.DrawString(m_UI_expHealth, m_currenthealth + " / " + m_maxhealth, new Vector2(90, 9), Color.White);
            //Draw the current mana / max mana
            a_spriteBatch.DrawString(m_UI_expHealth, m_currentmana + " / " + m_maxmana, new Vector2(90, 48), Color.White);


        }
        /// <summary>
        /// Used to draw the currently active player texture to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Used to draw textures to screen.</param>
        public void DrawPlayerTextures(SpriteBatch a_spriteBatch)
        {
            RemoveAllTempStats();

            if (m_class == Entities.Player.Class.Warrior)
            {
                if (m_activeTexture == null) m_activeTexture = m_warrior_Front_Idle;
            }
            else
            {
                if (m_activeTexture == null) m_activeTexture = m_wizard_Front_Idle;
            }

            a_spriteBatch.Draw(m_activeTexture, m_position, Color.White);

        }
        /// <summary>
        /// Moves the player upwards based on parameter. Changes the orientation of the animation.
        /// </summary>
        /// <param name="isMoving">If true, moves the player. If false, only animates.</param>
        public void MovePlayerUp(bool isMoving)
        {
            m_state = State.MoveUp;
            if (isMoving == true)
            {
                if (m_position.Y <= 0)
                {
                    //Do nothing
                }
                else
                {
                    m_position.Y -= m_speed;
                }
            }
            
        }
        /// <summary>
        /// Moves the player downwards based on parameter. Changes the orientation of the animation.
        /// </summary>
        /// <param name="isMoving">If true, moves the player. If false, only animates.</param>
        public void MovePlayerDown(bool isMoving)
        {
            m_state = State.MoveDown;
            if (isMoving == true)
            {
                if (m_position.Y >= 720 - m_activeTexture.Height)
                {
                    //Do nothing
                }
                else
                {
                    m_position.Y += m_speed;
                }
            }
        }
        /// <summary>
        /// Moves the player upwards left on parameter. Changes the orientation of the animation.
        /// </summary>
        /// <param name="isMoving">If true, moves the player. If false, only animates.</param>
        public void MovePlayerLeft(bool isMoving)
        {
            m_state = State.MoveLeft;
            if (isMoving == true)
            {
                if (m_position.X <= 0)
                {
                    //Do nothing
                }
                else
                {
                    m_position.X -= m_speed;
                }
            }
        }
        /// <summary>
        /// Moves the player right based on parameter. Changes the orientation of the animation.
        /// </summary>
        /// <param name="isMoving">If true, moves the player. If false, only animates.</param>
        public void MovePlayerRight(bool isMoving)
        {
            m_state = State.MoveRight;
            if (isMoving == true)
            {
                if (m_position.X >= 1280 - m_activeTexture.Width)
                {
                    //Do nothing
                }
                else
                {
                    m_position.X += m_speed;
                }
            }
        }
        /// <summary>
        /// Changes player state to idle if player has not moved.
        /// </summary>
        public void PlayerIdle()
        {
            switch (m_state)
            {
                case State.MoveLeft:
                    m_state = State.IdleLeft;
                    break;
                case State.MoveRight:
                    m_state = State.IdleRight;
                    break;
                case State.MoveUp:
                    m_state = State.IdleUp;
                    break;
                case State.MoveDown:
                    m_state = State.IdleDown;
                    break;
                default:
                    //Do nothing because they are already in idle state!
                    break;
            }
        }
        /// <summary>
        /// Checks if the player is on the left half of the screen.
        /// </summary>
        /// <returns>True if uncentered left.</returns>
        public bool PlayerUncenteredLeft()
        {
            if (m_position.X < 640)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Checks if the player is on the right half of the screen.
        /// </summary>
        /// <returns>True if uncentered right.</returns>
        public bool PlayerUncenteredRight()
        {
            if (m_position.X > 640)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Checks if the player is on the top half of the screen.
        /// </summary>
        /// <returns>True if uncentered up.</returns>
        public bool PlayerUncenteredUp()
        {
            if (m_position.Y < 360)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Checks if the player is on the bottom half of the screen.
        /// </summary>
        /// <returns>True if uncentered down.</returns>
        public bool PlayerUncenteredDown()
        {
            if (m_position.Y > 360)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Gets the player's speed.
        /// </summary>
        /// <returns>Integer representing player's speed.</returns>
        public int GetSpeed()
        {
            return m_speed;
        }
        /// <summary>
        /// Checked during update to animate the player based on the update time.
        /// Works for the warrior class.
        /// </summary>
        /// <param name="a_gameTime">Snapshot of the game's time values.</param>
        public void AnimateWarrior(GameTime a_gameTime)
        {
            m_frameCount = 4;
            m_interval = 7;

            //Looks at current state to check/advance time values and animations.
            switch (m_state)
            {
                case State.IdleUp:
                    m_activeTexture = m_warrior_Back_Idle;
                    break;
                case State.IdleDown:
                    m_activeTexture = m_warrior_Front_Idle;
                    break;
                case State.IdleLeft:
                    m_activeTexture = m_warrior_Left_Idle;
                    break;
                case State.IdleRight:
                    m_activeTexture = m_warrior_Right_Idle;
                    break;
                case State.MoveLeft:
                    m_timer += 1;
                        if (m_timer > m_interval)
                        {
                            m_timer = 0;
                            m_currentFrame++;
                            if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                            m_activeTexture = m_warrior_Left_Walk[m_currentFrame];
                        }
                    break;
                    
                case State.MoveRight:
                    m_timer += 1;
                        if (m_timer > m_interval)
                        {
                            m_timer = 0;
                            m_currentFrame++;
                            if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                            m_activeTexture = m_warrior_Right_Walk[m_currentFrame];
                        }
                    break;
                case State.MoveUp:
                    m_timer += 1;
                        if (m_timer > m_interval)
                        {
                            m_timer = 0;
                            m_currentFrame++;
                            if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                            m_activeTexture = m_warrior_Back_Walk[m_currentFrame];
                        }
                    break;
                case State.MoveDown:
                    m_timer += 1;
                        if (m_timer > m_interval)
                        {
                            m_timer = 0;
                            m_currentFrame++;
                            if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                            m_activeTexture = m_warrior_Front_Walk[m_currentFrame];
                        }
                    break;
            }
        }
        /// <summary>
        /// Checked during update to animate the player based on the update time.
        /// Works for the wizard class.
        /// </summary>
        /// <param name="a_gameTime">Snapshot of the game's time values.</param>
        public void AnimateWizard(GameTime a_gameTime)
        {
            m_frameCount = 5;
            m_interval = 7;

            switch (m_state)
            {
                case State.IdleUp:
                    m_activeTexture = m_wizard_Back_Idle;
                    break;
                case State.IdleDown:
                    m_activeTexture = m_wizard_Front_Idle;
                    break;
                case State.IdleLeft:
                    m_activeTexture = m_wizard_Left_Idle;
                    break;
                case State.IdleRight:
                    m_activeTexture = m_wizard_Right_Idle;
                    break;
                case State.MoveLeft:
                    m_timer += 1;
                    if (m_timer > m_interval)
                    {
                        m_timer = 0;
                        m_currentFrame++;
                        if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                        m_activeTexture = m_wizard_Left_Walk[m_currentFrame];
                    }
                    break;

                case State.MoveRight:
                    m_timer += 1;
                    if (m_timer > m_interval)
                    {
                        m_timer = 0;
                        m_currentFrame++;
                        if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                        m_activeTexture = m_wizard_Right_Walk[m_currentFrame];
                    }
                    break;
                case State.MoveUp:
                    m_timer += 1;
                    if (m_timer > m_interval)
                    {
                        m_timer = 0;
                        m_currentFrame++;
                        if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                        m_activeTexture = m_wizard_Back_Walk[m_currentFrame];
                    }
                    break;
                case State.MoveDown:
                    m_timer += 1;
                    if (m_timer > m_interval)
                    {
                        m_timer = 0;
                        m_currentFrame++;
                        if (m_currentFrame == m_frameCount) m_currentFrame = 0;
                        m_activeTexture = m_wizard_Front_Walk[m_currentFrame];
                    }
                    break;
            }
        }
        /// <summary>
        /// Returns the player's level.
        /// </summary>
        /// <returns>Integer value returning the player's level.</returns>
        public int GetLevel()
        {
            return m_currentlevel;
        }
        /// <summary>
        /// Adds a single strength.
        /// </summary>
        public void AddStrength()
        {
            if (m_assignable > 0)
            {
                m_strength += 1;
                m_assignable -= 1;
            }
        }
        /// <summary>
        /// Adds a single intelligece.
        /// </summary>
        public void AddIntelligence()
        {
            if (m_assignable > 0)
            {
                m_intelligence += 1;
                m_assignable -= 1;
            }
        }
        /// <summary>
        /// Adds a single speed.
        /// </summary>
        public void AddSpeed()
        {
            if (m_assignable > 0)
            {
                m_speed += 1;
                m_assignable -= 1;
            }
        }
        /// <summary>
        /// Adds a single agility.
        /// </summary>
        public void AddAgility()
        {
            if (m_assignable > 0)
            {
                m_agility += 1;
                m_assignable -= 1;
            }
        }
        /// <summary>
        /// Gets the current map level.
        /// </summary>
        /// <returns>Returns an integer value representing the current map number.</returns>
        public int GetMap()
        {
            return m_currentMap;
        }
        /// <summary>
        /// Gets the rectangle of the current sprite drawn on screen.
        /// </summary>
        /// <returns>Rectangle of the current frame/position.</returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)m_position.X, (int)m_position.Y, 
                                    m_activeTexture.Width, m_activeTexture.Height);
        }
        /// <summary>
        /// Gets the directional state of this character.
        /// </summary>
        /// <returns>Character.State of this player.</returns>
        public State GetState()
        {
            return m_state;
        }
        /// <summary>
        /// Restores mana based on the timer.
        /// </summary>
        public void UpdateManaRegen()
        {
            m_updateManaRegenTimer++;
            if (m_updateManaRegenTimer == m_updateManaRegenMax)
            {
                m_updateManaRegenTimer = 0;
                AddMana(1);
            }
        }
        /// <summary>
        /// Restores health based on the timer.
        /// </summary>
        public void UpdateHealthRegen()
        {
            m_updateHealthRegenTimer++;
            if (m_updateHealthRegenTimer == m_updateHealthRegenMax)
            {
                m_updateHealthRegenTimer = 0;
                AddHealth(1);
            }
        }
        /// <summary>
        /// Changes the player map.
        /// </summary>
        /// <param name="a_map">Map number.</param>
        public void ChangeMap(int a_map)
        {
            m_currentMap = a_map;
        }
        /// <summary>
        /// Gets the currently equipped weapon.
        /// </summary>
        /// <returns>The weapon index number.</returns>
        public int GetWeaponIndex()
        {
            return m_equippedItem;
        }
        /// <summary>
        /// Gets the currently equipped armor.
        /// </summary>
        /// <returns>The equipped armor index number.</returns>
        public int GetArmorIndex()
        {
            return m_equippedArmor;
        }
        /// <summary>
        /// Equips a weapon or an armor.
        /// </summary>
        /// <param name="index">Index of the item to equip.</param>
        public void Equip(int index)
        {
            if (index >= 0 && index <= 5)
            {
                m_equippedItem = index;
            }
            else
            {
                m_equippedArmor = index;
            }
        }
        /// <summary>
        /// Checks if the player has an item at a given index.
        /// </summary>
        /// <param name="index">Index to check.</param>
        /// <returns>Returns true if the item is contained at the index.</returns>
        public bool HasItem(int index)
        {
            return m_HasItems[index];
        }
        /// <summary>
        /// Adds an item at a given index.
        /// </summary>
        /// <param name="index">Index of the item to add.</param>
        public void GiveItem(int index)
        {
            m_HasItems[index] = true;
        }
        /// <summary>
        /// Override for creating abstracts. Adds stats based on weapons.
        /// </summary>
        override public void CreateAbstracts()
        {
            m_attack = m_strength * 2;
            m_defense = m_strength / 2;
            m_movementspeed = m_speed / 2 + 1;
            m_spellpower = m_intelligence * 2;
            m_attackspeed = m_speed / 2 + 1;

            if (GetArmorIndex() == 6 || GetArmorIndex() == 9) AddTempStat(Stat.Defense, 1);
            if (GetArmorIndex() == 7 || GetArmorIndex() == 10) AddTempStat(Stat.Defense, 5);
            if (GetArmorIndex() == 8 || GetArmorIndex() == 11) AddTempStat(Stat.Defense, 10);
            if (GetWeaponIndex() == 0 || GetWeaponIndex() == 3) AddTempStat(Stat.Attack, 1);
            if (GetWeaponIndex() == 1 || GetWeaponIndex() == 4) AddTempStat(Stat.Attack, 5);
            if (GetWeaponIndex() == 2 || GetWeaponIndex() == 5) AddTempStat(Stat.Attack, 10);
        }
    }
}
