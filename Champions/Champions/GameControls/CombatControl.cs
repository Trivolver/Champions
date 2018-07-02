using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions
{
    /// <summary>
    /// Controls all combat functions within the game.
    /// </summary>
    static class CombatControl
    {
        /// <summary>A random number.</summary>
        static Random rand;
        /// <summary>First position is spell number, 2nd position is value of the mod.</summary>
        static double [,] Spell_ID_Mods;
        /// <summary>Texture of an arcane blast.</summary>
        static Texture2D m_sheet_arcane;
        /// <summary>Texture of a fireball.</summary>
        static Texture2D m_sheet_fireball;
        /// <summary>Texture of a basic attack.</summary>
        static Texture2D m_sheet_attack;
        /// <summary>Texture of a hammer attack.</summary>
        static Texture2D m_sheet_hammer;
        /// <summary>Texture for lightning.</summary>
        static Texture2D m_sheet_lightning;
        /// <summary>Texture for a nuclear attack.</summary>
        static Texture2D m_sheet_nuke;
        /// <summary>Texture for a viper blast.</summary>
        static Texture2D m_sheet_viper;
        /// <summary>Interval to update frame.</summary>
        static int m_interval = 7;
        /// <summary>Timer to count with.</summary>
        static int m_timer = 0;
        /// <summary>A list of all attacks on the screen.</summary>
        static List<Attacks.Attack> m_attacks;

        /// <summary>
        /// Deals damage based on attacker and defender's stats. Returns the damage amount.
        /// </summary>
        /// <param name="a_attacker">Attacking character.</param>
        /// <param name="a_defender">Defending character.</param>
        /// <param name="a_isMelee">Set to false if this is a magic attack.</param>
        /// <param name="a_multiplier">Value to multiply damage by.</param>
        /// <returns></returns>
        static public int DealDamage(Entities.Character a_attacker, Entities.Character a_defender, bool a_isMelee, int a_multiplier)
        {
            int result = 0;
            int attack = a_attacker.GetTotalAttack();
            int defense = a_defender.GetTotalDefense();
            int spellPower = a_attacker.GetTotalSpellpower();
            double percent = attack / defense;

            if (a_isMelee)
            {
                /**To have a true "100%" attack, you must have double the enemy's defense.
                 * At equal attack and defense, you deal 50% of your actual damage.
                 **/
                if (attack > defense)
                {
                    if (percent >= 2)
                    {
                        result = rand.Next((int)(attack * .9), (int)(attack * 1.3));
                    }
                    else

                    {
                        result = rand.Next((int)((attack / percent) * .8), (int)((attack / percent) * 1.1));
                    }
                }
                else if (attack == defense)
                {
                    result = rand.Next((int)(attack / 2 * .8), (int)((attack / 2) * 1.2));
                }
                else //a_attacker.GetAttack() < a_defender.GetDefense())
                {
                    result = 1 + rand.Next((int)((attack / percent) * .8), (int)((attack / percent) * 1.1));
                }
                result *= a_multiplier;
            }
            else //Is a spell
            {
                result = (int)(spellPower * a_multiplier);
            }
            if (result < 1) result = 1;
            return result;
        }
        /// <summary>
        /// Loads graphics into variables for drawing later.
        /// </summary>
        /// <param name="a_arcane">Texture2d of the arcane blast.</param>
        /// <param name="a_attack">Texture2d of the basic attack.</param>
        /// <param name="a_fireball">Texture2d of a fireball.</param>
        /// <param name="a_hammer">Texture2d of the hammer attack.</param>
        /// <param name="a_lightning">texture2d of the lightning attack.</param>
        /// <param name="a_nuke">Texture2d of the nuke attack.</param>
        /// <param name="a_viper">Texture2d of the viper attack.</param>
        public static void LoadGraphics(Texture2D a_arcane, Texture2D a_attack, Texture2D a_fireball, Texture2D a_hammer,
             Texture2D a_lightning, Texture2D a_nuke, Texture2D a_viper)
        {
            m_sheet_arcane = a_arcane;
            m_sheet_attack = a_attack;
            m_sheet_fireball = a_fireball;
            m_sheet_hammer = a_hammer;
            m_sheet_lightning = a_lightning;
            m_sheet_nuke = a_nuke;
            m_sheet_viper = a_viper;

            rand = new Random();
            rand.Next(1, 100);
            Spell_ID_Mods = new Double[6, 1];
            m_attacks = new List<Attacks.Attack>();
        }
        /// <summary>
        /// Checks spell ids to issue an attack.
        /// </summary>
        /// <param name="a_character">Character to pull stats from.</param>
        /// <param name="a_id">ID of the spell to add.</param>
        public static void IssueAttack(Entities.Character a_character, int a_id)
        {
            switch (a_id)
            {
                case 0:
                    m_attacks.Add(new Attacks.Attack_Basic(m_sheet_attack, a_character));
                    break;
                case 1:
                    m_attacks.Add(new Attacks.Arcanebolt(m_sheet_arcane, a_character));
                    break;
                case 2:
                    m_attacks.Add(new Attacks.Fireball(m_sheet_fireball, a_character));
                    break;
                case 3:
                    m_attacks.Add(new Attacks.Viper(m_sheet_viper, a_character));
                    break;
                case 4:
                    m_attacks.Add(new Attacks.Nuke(m_sheet_nuke, a_character));
                    break;
                case 5:
                    m_attacks.Add(new Attacks.Hammer(m_sheet_hammer, a_character));
                    break;
                case 6:
                    m_attacks.Add(new Attacks.Lightning(m_sheet_lightning, a_character));
                    break;
            }
        }
        /// <summary>
        /// Draws all attacks to the screen in the attacks list.
        /// </summary>
        /// <param name="a_spritebatch">Spritebatch used for drawing.</param>
        public static void DrawAttacks(SpriteBatch a_spritebatch)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].Draw(a_spritebatch);
                    counter++;
                }
            }
        }
        /// <summary>
        /// Updates all attacks to draw in the attack list.
        /// </summary>
        public static void UpdateAttacks()
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].Update();
                    counter++;
                }
            }
        }
        /// <summary>
        /// Checks collision of all attacks to all enemies/players.
        /// </summary>
        /// <param name="a_npcs">Reference to an npc list.</param>
        /// <param name="a_char">Reference to the active character.</param>
        public static void CheckCollision(ref List<Entities.NPCBad> a_npcs, ref Entities.Player a_char)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].CheckCollision(ref a_npcs, ref a_char);
                    counter++;
                }
            }
        }
        /// <summary>
        /// Moves all objects on the screen to the left according to the map move speed.
        /// </summary>
        /// <param name="a_speed">Distance to move.</param>
        public static void MoveObjectsLeft(int a_speed)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].MoveLeft(a_speed);
                    counter++;
                }
            }
        }
        /// <summary>
        /// Moves all objects on the screen to the right according to the map move speed.
        /// </summary>
        /// <param name="a_speed">Distance to move.</param>
        public static void MoveObjectsRight(int a_speed)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].MoveRight(a_speed);
                    counter++;
                }
            }
        }
        /// <summary>
        /// Moves all objects on the screen up according to the map move speed.
        /// </summary>
        /// <param name="a_speed">Distance to move.</param>
        public static void MoveObjectsUp(int a_speed)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].MoveUp(a_speed);
                    counter++;
                }
            }
        }
        /// <summary>
        /// Moves all objects on the screen down according to the map move speed.
        /// </summary>
        /// <param name="a_speed">Distance to move.</param>
        public static void MoveObjectsDown(int a_speed)
        {
            int counter = 0;
            if (m_attacks != null)
            {
                foreach (Attacks.Attack i in m_attacks)
                {
                    m_attacks[counter].MoveDown(a_speed);
                    counter++;
                }
            }
        }
    }
}
