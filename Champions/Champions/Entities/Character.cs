using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Champions.Entities
{
    /// <summary>
    /// This class is the sub-class for all character-related creatures in the game.
    /// This includes the user, multiplayer-users, enemy AI and friendly AI.
    /// </summary>
    class Character
    {
        /// <summary>
        /// Stat represents the statistic name corresponding to the variables.
        /// <example> CurrentHealth corresponds to m_currenthealth.</example>
        /// </summary>
        public enum Stat
        {
            CurrentHealth, MaxHealth, CurrentMana, MaxMana,
            Strength, Speed, Agility, Intelligence, Defense, SpellPower,
            AttackSpeed, MovementSpeed, Attack
        }
        /// <summary>
        /// Describes the directional state of the character.
        /// </summary>
        public enum State { MoveLeft, MoveRight, MoveUp, MoveDown, IdleLeft, IdleRight, IdleUp, IdleDown };
        /// <summary>
        /// The state of the character. Uses the "State" enumeration.
        /// </summary>
        protected State m_state;
        /// <summary> Constant representing the level cap in the game. </summary>
        protected const int LEVEL_CAP = 20;
        /// <summary> Constant representing the experience cap in the game. </summary>
        protected const int EXP_CAP = 10000;
        /// <summary> Number of assignable points to increase stats. </summary>
        protected int m_assignable;
        /// <summary> Current health of the Character. Cannot exceed the Maximum. </summary>
        protected int m_currenthealth;
        /// <summary> Maximum health of the character. </summary>
        protected int m_maxhealth;
        /// <summary> Current mana of the character. Cannot exceed the Maximum. </summary>
        protected int m_currentmana;
        /// <summary> Maximum mana of the character. </summary>
        protected int m_maxmana;
        /// <summary> Stat number representing the strength of the character. </summary>
        protected int m_strength;
        /// <summary> Speed number representing the quickness of the character. </summary>
        protected int m_speed;
        /// <summary> Agility number representing the dexterity and speed of the character. </summary>
        protected int m_agility;
        /// <summary> Intelligence representing the spellcasting capability of the character. </summary>
        protected int m_intelligence;
        /// <summary> Defense representing the ability to defend the character. </summary>
        protected int m_defense;
        /// <summary> Stat number represending the spellpower of the character. </summary>
        protected int m_spellpower;
        /// <summary> Stat number representing the attack speed of the character. </summary>
        protected int m_attackspeed;
        /// <summary> Stat number representing how fast a character moves. </summary>
        protected int m_movementspeed;

        /// <summary> The current exp of the character. Cannot exceed the maximum. </summary>
        protected int m_currentexp;
        /// <summary> The amount of exp to level. Assign at constructor call. </summary>
        protected int [] m_exptolevel;
        /// <summary> The current level of the character. Cannot exceed the maximum. </summary>
        protected int m_currentlevel;
        /// <summary> The amount of exp awarded for killing this character.  </summary>
        protected int m_expforkill;
        /// <summary> Stat representing how much damage a character does. </summary>
        protected int m_attack;

        //The following are the same descriptors as above, but correspond to their temporary stats only.
        /// <summary> Stat representing the temporary maximum health. </summary>
        protected int m_temp_maxhealth;
        /// <summary> Stat representing the temporary Maximum mana. </summary>
        protected int m_temp_maxmana;
        /// <summary> Stat representing the temporary strength. </summary>
        protected int m_temp_strength;
        /// <summary> Stat representing the temporary speed. </summary>
        protected int m_temp_speed;
        /// <summary> Stat representing the temporary agility. </summary>
        protected int m_temp_agility;
        /// <summary> Stat representing the temporary intelligence. </summary>
        protected int m_temp_intelligence;
        /// <summary> Stat representing the temporary defense. </summary>
        protected int m_temp_defense;
        /// <summary> Stat representing the temporary spellpower. </summary>
        protected int m_temp_spellpower;
        /// <summary> Stat representing the temporary attack speed. </summary>
        protected int m_temp_attackspeed;
        /// <summary> Stat representing the temporary movement speed. </summary>
        protected int m_temp_movementspeed;
        /// <summary> Stat representing the temporary attack. </summary>
        protected int m_temp_attack;
        /// <summary>Yes if the character is currently active or alive.</summary>
        protected bool m_isAlive;
        /// <summary>Position to draw this entity.</summary>
        protected Vector2 m_position;
        /// <summary>Set to true if this character is an enemy.</summary>
        protected bool m_isEnemy;

        /// <summary>
        /// Constructor for the Character class. Initializes the required experience to level,
        /// as well as each of the temporary stats to zero.
        /// </summary>
        public Character ( )
        {
            //Initialize the amount of exp required for each level.
            m_exptolevel = new int[20];
            m_exptolevel[0] = 20;
            m_exptolevel[1] = 40;
            m_exptolevel[2] = 100;
            m_exptolevel[3] = 150;
            m_exptolevel[4] = 200;
            m_exptolevel[5] = 300;
            m_exptolevel[6] = 500;
            m_exptolevel[7] = 750;
            m_exptolevel[8] = 1000;
            m_exptolevel[9] = 1300;
            m_exptolevel[10] = 1700;
            m_exptolevel[11] = 2000;
            m_exptolevel[12] = 2500;
            m_exptolevel[13] = 3000;
            m_exptolevel[14] = 3750;
            m_exptolevel[15] = 4250;
            m_exptolevel[16] = 5000;
            m_exptolevel[17] = 6000;
            m_exptolevel[18] = 8000;
            m_exptolevel[19] = 10000;

            //Initialize all temporary stats to zero.
            m_temp_maxhealth = 0;
            m_temp_maxmana = 0;
            m_temp_strength = 0;
            m_temp_speed = 0;
            m_temp_agility = 0;
            m_temp_intelligence = 0;
            m_temp_defense = 0;
            m_temp_spellpower = 0;
            m_temp_attackspeed = 0;
            m_temp_movementspeed = 0;
            m_temp_attack = 0;

            m_isAlive = true;

        }

        /// <summary>
        /// This function should be called in each derived creature's constructor
        /// to initialize the character. It assigns all arguments to their corresponding
        /// member stats.
        /// </summary>
        /// <param name="a_strength">The strength stat.</param>
        /// <param name="a_speed">The Speed stat.</param>
        /// <param name="a_agility">The agility stat.</param>
        /// <param name="a_intelligence">The intelligence stat.</param>
        /// <param name="a_maxhealth">The maximum health stat.</param>
        /// <param name="a_maxmana">The maximum mana stat.</param>
        /// <param name="a_currentexp">The current experience of the character.</param>
        /// <param name="a_currentlevel">The current level of the character.</param>
        /// <param name="a_expforkill">The amount of exp awarded for killing this character.</param>
        protected void CreateStats(int a_strength, int a_speed, int a_agility, int a_intelligence,
            int a_maxhealth, int a_maxmana, int a_currentexp, int a_currentlevel, int a_expforkill)
        {
            //Initialize all variables according to parameter.
            m_strength = a_strength;
            m_speed = a_speed;
            m_agility = a_agility;
            m_intelligence = a_intelligence;
            m_maxhealth = a_maxhealth;
            m_maxmana = a_maxmana;
            m_currentexp = a_currentexp;
            m_currentlevel = a_currentlevel;
            m_expforkill = a_expforkill;
            //These two variables should be initilized to maximum on creation.
            m_currenthealth = a_maxhealth;
            m_currentmana = a_maxmana;
        }

        /// <summary>
        /// Gets the attack corresponding to this character.
        /// </summary>
        /// <returns>Integer value representing the attack.</returns>
        public int GetAttack()
        {
            return m_attack;
        }
        /// <summary>
        /// Gets the defense corresponding to this character.
        /// </summary>
        /// <returns>Integer value representing the defense.</returns>
        public int GetDefense()
        {
            return m_defense;
        }
        /// <summary>
        /// Gets the number of assignable points.
        /// </summary>
        /// <returns>Integer representing the number of assignable points.</returns>
        public int GetAssignable()
        {
            return m_assignable;
        }
        /// <summary>
        /// Gets the player's current health.
        /// </summary>
        /// <returns>An integer representing the current health.</returns>
        public int GetCurrentHealth()
        {
            return m_currenthealth;
        }
        /// <summary>
        /// Gets the player's maximum health.
        /// </summary>
        /// <returns> An integer representing the maximum health of the player.</returns>
        public int GetMaxHealth()
        {
            return m_maxhealth;
        }
        /// <summary>
        /// This function adds a value to the given temporary stat as defined by the 
        /// "Stat" enum. <see cref="Entities.Character.Stat"/>
        /// </summary>
        /// <param name="a_stat">The stat name.</param>
        /// <param name="a_amount">The amount to increase or decrease.</param>
        public void AddTempStat(Stat a_stat, int a_amount)
        {
            //Find the statname and modify the temporary stat according to the amount.
            switch ( a_stat )
            {
                case Stat.MaxHealth:
                    m_temp_maxhealth += a_amount;
                    break;
                case Stat.MaxMana:
                    m_temp_maxmana += a_amount;
                    break;
                case Stat.Strength:
                    m_temp_strength += a_amount;
                    break;
                case Stat.Speed:
                    m_temp_speed += a_amount;
                    break;
                case Stat.Intelligence:
                    m_temp_intelligence += a_amount;
                    break;
                case Stat.Agility:
                    m_temp_agility += a_amount;
                    break;
                case Stat.SpellPower:
                    m_temp_spellpower += a_amount;
                    break;
                case Stat.AttackSpeed:
                    m_temp_attackspeed += a_amount;
                    break;
                case Stat.MovementSpeed:
                    m_temp_movementspeed += a_amount;
                    break;
                case Stat.Defense:
                    m_temp_defense += a_amount;
                    break;
                case Stat.Attack:
                    m_temp_attack += a_amount;
                    break;
                default :
                    break;
                   
            }
        }
        /// <summary>
        /// Removes the the amount of the temporary stat given the amount as defined
        /// by the amount and the "Stat" enum. "Stat" enum. <see cref="Entities.Character.Stat"/>
        /// </summary>
        /// <param name="a_stat">The stat name to choose. "Stat" enum. <see cref="Entities.Character.Stat"/></param>
        /// <param name="a_amount">The amount to increase or decrease the temporary stat by.</param>
        public void RemoveTempStat(Stat a_stat, int a_amount)
        {
            switch (a_stat)
            {
                case Stat.MaxHealth:
                    m_temp_maxhealth -= a_amount;
                    break;
                case Stat.MaxMana:
                    m_temp_maxmana -= a_amount;
                    break;
                case Stat.Strength:
                    m_temp_strength -= a_amount;
                    break;
                case Stat.Speed:
                    m_temp_speed -= a_amount;
                    break;
                case Stat.Intelligence:
                    m_temp_intelligence -= a_amount;
                    break;
                case Stat.Agility:
                    m_temp_agility -= a_amount;
                    break;
                case Stat.SpellPower:
                    m_temp_spellpower -= a_amount;
                    break;
                case Stat.AttackSpeed:
                    m_temp_attackspeed -= a_amount;
                    break;
                case Stat.MovementSpeed:
                    m_temp_movementspeed -= a_amount;
                    break;
                case Stat.Defense:
                    m_temp_defense -= a_amount;
                    break;
                case Stat.Attack:
                    m_temp_attack -= a_amount;
                    break;
                default:
                    break;

            }
        }
        /// <summary>
        /// Sets all temporary stats to zero.
        /// </summary>
        public void RemoveAllTempStats()
        {
            m_temp_maxhealth = 0;
            m_temp_maxmana = 0;
            m_temp_strength = 0;
            m_temp_speed = 0;
            m_temp_intelligence = 0;
            m_temp_agility = 0;
            m_temp_spellpower = 0;
            m_temp_attackspeed = 0;
            m_temp_movementspeed = 0;
            m_temp_defense = 0;
            m_temp_attack = 0;
        }
        /// <summary>
        /// Gets a string to return to the menu. The string will display 
        /// the player's health, mana, assignables, experience, and basic stats.
        /// </summary>
        /// <returns>A string representing the first part of the menu.</returns>
        public string GetMenuString1()
        {
            string menustring;

            menustring = "";

            menustring += "Assignable Points: " + m_assignable + "\n\n" +
                "Health: " + m_currenthealth + " / " + m_maxhealth + "  (" + m_temp_maxhealth + ")" + "\n" +
                "Mana: " + m_currentmana + " / " + m_maxmana + "  (" + m_temp_maxmana + ")" + "\n" +
                "Experience: " + m_currentexp + " / " + m_exptolevel[m_currentlevel - 1] + "\n" +
                "\n" +
                "           Strength: " + m_strength + "  (" + m_temp_strength + ")" + "\n\n" +
                "           Speed: " + m_speed + "  (" + m_temp_speed + ")" + "\n\n" +
                "           Agility: " + m_agility + "  (" + m_temp_agility + ")" + "\n\n" +
                "           Intelligence: " + m_intelligence + "  (" + m_temp_intelligence + ")" + "\n\n";



            return menustring;
        }
        /// <summary>
        /// This function forms a string for the rest of the stats of the current character.
        /// The stats are attack, defense, spellpower, attackspeed, and movement speed.
        /// </summary>
        /// <returns>A string representing the 2nd part of the menu / stats.</returns>
        public string GetMenuString2()
        {
            string menustring = "";

            menustring += "Attack: " + m_attack + "  (" + m_temp_attack + ")" + "\n" +
                "Defense: " + m_defense + "  (" + m_temp_defense + ")" + "\n" +
                "Spellpower: " + m_spellpower + "  (" + m_temp_spellpower + ")" + "\n" +
                "Attack Speed: " + m_attackspeed + "  (" + m_temp_attackspeed + ")" + "\n" +
                "Movement Speed: " + m_movementspeed + "  (" + m_temp_movementspeed + ")" + "\n";

            return menustring;
        }
        /// <summary>
        /// Gets the current mana of the character.
        /// </summary>
        /// <returns>Current mana as integer value.</returns>
        public int GetCurrentMana()
        {
            return m_currentmana;
        }
        /// <summary>
        /// Reduces the current mana. Cannot precede zero.
        /// </summary>
        /// <param name="a_amount">Integer amount to reduce mana by.</param>
        /// <returns>False if the mana is reduced past zero.</returns>
        public bool ReduceCurrentMana(int a_amount)
        {
            if (a_amount > m_currentmana) return false;
            else
            {
                m_currentmana -= a_amount;
                return true;
            }
        }
        /// <summary>
        /// Reduces the current health. Calls to Die() if less than zero.
        /// </summary>
        /// <param name="a_amount">Integer amount to reduce hp by.</param>
        public void ReduceCurrentHP(int a_amount)
        {
            m_currenthealth -= a_amount;
            if (m_currenthealth <= 0) Die();
        }
        /// <summary>
        /// Adds mana to the currentmana. Cannot exceed the maximum.
        /// </summary>
        /// <param name="a_amount">Integer amount to add to the mana. Cannot exceed maximum.</param>
        public void AddMana(int a_amount)
        {
            if (m_currentmana + a_amount >= m_maxmana)
            {
                m_currentmana = m_maxmana;
            }
            else
            {
                m_currentmana += a_amount;
            }
        }
        /// <summary>
        /// Adds to the current health. Cannot exceed the maximum.
        /// </summary>
        /// <param name="a_amount">Integer amount to increase health by.</param>
        public void AddHealth(int a_amount)
        {
            if (m_currenthealth + a_amount >= m_maxhealth)
            {
                m_currenthealth = m_maxhealth;
            }
            else
            {
                m_currenthealth += a_amount;
            }
        }
        /// <summary>
        /// Creates the Attack, Defense, Movementspeed, Spellpower and Attackspeed stats. 
        /// Can be overridden.
        /// </summary>
        virtual public void CreateAbstracts()
        {
            m_attack = m_strength * 2;
            m_defense = m_strength / 2;
            m_movementspeed = m_speed / 2 + 1;
            m_spellpower = m_intelligence * 2;
            m_attackspeed = m_speed / 2 + 1;
        }
        /// <summary>
        /// Gets the total attack of the character.
        /// </summary>
        /// <returns>Integer value of temp attack + basic attack.</returns>
        public int GetTotalAttack()
        {
            return m_attack + m_temp_attack;
        }
        /// <summary>
        /// Gets the total defense of the character.
        /// </summary>
        /// <returns>Integer value of temp defense + basic defense.</returns>
        public int GetTotalDefense()
        {
            return m_defense + m_temp_defense;
        }
        /// <summary>
        /// Gets the total spellpower of the character.
        /// </summary>
        /// <returns>Integer value of temp spellpower + basic spellpower.</returns>
        public int GetTotalSpellpower()
        {
            return m_spellpower + m_temp_spellpower;
        }
        /// <summary>
        /// Gets the state of the character.
        /// </summary>
        /// <returns>Character.State direction of the character.</returns>
        public State GetState()
        {
            return m_state;
        }
        /// <summary>
        /// Gets the vector2 based position of this character.
        /// </summary>
        /// <returns>Vector2 current position.</returns>
        public Vector2 GetPosition()
        {
            return m_position;
        }
        /// <summary>
        /// Checks if this character is an enemy or a player.
        /// </summary>
        /// <returns>True if the character is an enemy.</returns>
        public bool CheckIsEnemy()
        {
            return m_isEnemy;
        }
        /// <summary>
        /// Checks if this character is alive.
        /// </summary>
        /// <returns>True if the character is alive.</returns>
        public bool IsAlive()
        {
            return m_isAlive;
        }
        /// <summary>
        /// Kills this character.
        /// </summary>
        public void Die()
        {
            m_isAlive = false;
        }
        /// <summary>
        /// Brings this character to life.
        /// </summary>
        public void Live()
        {
            m_isAlive = true;
        }
    }
}
