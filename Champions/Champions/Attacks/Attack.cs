using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Champions.Attacks
{
    /// <summary>
    /// Serves as the base class for drawing all attacks to the screen.
    /// </summary>
    abstract class Attack
    {
        /// <summary> Set to true if needs to be drawn to screen. </summary>
        protected bool m_isActive;
        /// <summary> How often the animation will be drawn. </summary>
        protected int m_interval;
        /// <summary> Number of frames to draw in the animation. </summary>
        protected int m_frames;
        /// <summary> the current time to compare to the interval. </summary>
        protected int m_timer;
        /// <summary> The number of the current frame we're on. </summary>
        protected int m_currentframe;
        /// <summary> The value in which the attack modifies our damage. </summary>
        double m_modifier;
        /// <summary> Current posiiton of the attack. </summary>
        protected Vector2 m_position;
        /// <summary> The Texture to draw the attack from. </summary>
        protected Texture2D m_sheet;
        /// <summary> The destinations on the sprite sheet to draw from. </summary>
        protected Rectangle [] m_anim;
        /// <summary> Rotation angle of the sprite. </summary>
        protected float m_rotation;
        /// <summary> Speed of the current attack, if needs to move. </summary>
        protected int m_selfSpeed;
        /// <summary> Direction enumeration to find which way to move.</summary>
        public enum Direction { Left, Up, Down, Right };
        /// <summary> Direction of the attack to travel. </summary>
        protected Direction m_direction;
        /// <summary> How long the attack stays active for. </summary>
        protected long m_duration;
        /// <summary> Current time of the attack. </summary>
        protected long m_durationTimer;
        /// <summary> If the attack is sent by an enemy, will be set to true. </summary>
        protected bool m_toPlayer;

        /// <summary>
        /// Assigns basic values to the current attack. Should be called with every new attack.
        /// </summary>
        /// <param name="a_modifier">Value to modify the attaack by.</param>
        /// <param name="a_interval">Time interval to switch frames.</param>
        /// <param name="a_frames">Number of frames to draw.</param>
        /// <param name="a_selfSpeed">Speed of the attack.</param>
        /// <param name="a_duration">Length of the attack to stay active.</param>
        /// <param name="a_toPlayer">Set to true if from an enemy.</param>
        public void AssignAttributes(int a_modifier, int a_interval, int a_frames, int a_selfSpeed,
            long a_duration, bool a_toPlayer)
        {
            m_modifier = a_modifier;
            m_interval = a_interval;
            m_frames = a_frames;
            m_currentframe = 0;
            m_isActive = true;
            m_timer = 0;
            m_selfSpeed = a_selfSpeed;
            m_duration = a_duration;
            m_rotation = 0;
            m_anim = new Rectangle[m_frames];
            m_durationTimer = 0;
            m_toPlayer = a_toPlayer;
        }
        /// <summary>
        /// Determines if this attack is active.
        /// </summary>
        /// <returns>Boolean: True if the attack is active.</returns>
        public bool IsActive()
        {
            return m_isActive;
        }
        /// <summary>
        /// Returns the value if the attack is set by an enemy.
        /// </summary>
        /// <returns>True if the player should take damage.</returns>
        public bool ToPlayer()
        {
            return m_toPlayer;
        }
        /// <summary>
        /// Sets the attack to active.
        /// </summary>
        protected void Live()
        {
            m_isActive = true;
        }
        /// <summary>
        /// Sets the attack to not active.
        /// </summary>
        protected void Die()
        {
            m_isActive = false;
        }
        /// <summary>
        /// Draws the current animation frame and position to the screen.
        /// </summary>
        /// <param name="a_spritebatch">Used for drawing sprites to the screen.</param>
        public void Draw(SpriteBatch a_spritebatch)
        {
            if (IsActive())
            {
                m_timer += 1;
                if (m_timer > m_interval)
                {
                    m_currentframe++;
                    m_timer = 0;
                }
                if (m_currentframe >= m_frames)
                {
                    m_currentframe = 0;
                }
                Rectangle draw = new Rectangle((int)m_position.X, (int)m_position.Y, (int)m_anim[m_currentframe].Width, (int)m_anim[m_currentframe].Height);
                Vector2 origin = new Vector2(m_anim[m_currentframe].X / 2, m_anim[m_currentframe].Y / 2);
                SpriteEffects effects = new SpriteEffects();
                effects = SpriteEffects.None;
                if ( m_direction == Direction.Left ) effects = SpriteEffects.FlipHorizontally;
                if (m_direction == Direction.Down) effects = SpriteEffects.FlipVertically;

                a_spritebatch.Draw(m_sheet, new Rectangle((int)(draw.X + origin.X), (int)(draw.Y + origin.Y), draw.Width, draw.Height)
                    , m_anim[m_currentframe], Color.White, m_rotation, origin, effects, 0f);
                
            }
        }
        /// <summary>
        /// Moves the attack to the left based on the speed parameter.
        /// </summary>
        /// <param name="a_speed">How far to move the attack.</param>
        public void MoveLeft(int a_speed)
        {
            m_position.X -= a_speed;
        }
        /// <summary>
        /// Moves the attack to the left based on its speed.
        /// </summary>
        public void MoveLeft()
        {
            m_position.X -= m_selfSpeed;
        }
        /// <summary>
        /// Moves the attack to the right based on the speed parameter.
        /// </summary>
        /// <param name="a_speed">How far to move the attack.</param>
        public void MoveRight(int a_speed)
        {
            m_position.X += a_speed;
        }
        /// <summary>
        /// Moves the attack to the right based on its speed.
        /// </summary>
        public void MoveRight()
        {
            m_position.X += m_selfSpeed;
        }
        /// <summary>
        /// Moves the attack up based on the speed parameter.
        /// </summary>
        /// <param name="a_speed">How far to move the attack.</param>
        public void MoveUp(int a_speed)
        {
            m_position.Y -= a_speed;
        }
        /// <summary>
        /// Moves the attack up based on its speed.
        /// </summary>
        public void MoveUp()
        {
            m_position.Y -= m_selfSpeed;
        }
        /// <summary>
        /// Moves the attack to down based on the speed parameter.
        /// </summary>
        /// <param name="a_speed">How far to move the attack.</param>
        public void MoveDown(int a_speed)
        {
            m_position.Y += a_speed;
        }
        /// <summary>
        /// Moves the attack down based on its speed.
        /// </summary>
        public void MoveDown()
        {
            m_position.Y += m_selfSpeed;
        }

        /// <summary>
        /// Used by child classes to update drawing position, frames, etc. based on its needs.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Uses the character state to determine which direction to launch the attack.
        /// </summary>
        /// <param name="a_character">The character launching the attack.</param>
        protected void UpdateDirection(Entities.Character a_character)
        {
            if (a_character.GetState() == Entities.Character.State.MoveUp
                || a_character.GetState() == Entities.Character.State.IdleUp)
            {
                m_direction = Direction.Up;
            }
            else if (a_character.GetState() == Entities.Character.State.MoveDown
                || a_character.GetState() == Entities.Character.State.IdleDown)
            {
                m_direction = Direction.Down;
            }
            else if (a_character.GetState() == Entities.Character.State.IdleLeft
                || a_character.GetState() == Entities.Character.State.MoveLeft)
            {
                m_direction = Direction.Left;
            }
            else
            {
                m_direction = Direction.Right;
            }

            Live();
            UpdateRotate();
        }
        /// <summary>
        /// Rotates the sprite based on its direction.
        /// </summary>
        protected void UpdateRotate()
        {
            switch (m_direction)
            {
                case Direction.Up:
                    m_rotation = 170;
                    break;
                case Direction.Down:
                    m_rotation = 170;
                    break;
                case Direction.Left:
                    m_rotation = 0;
                    break;
                case Direction.Right:
                    m_rotation = 0;
                    break;
            }
        }

        /// <summary>
        /// Checks if the attack hit a player.
        /// </summary>
        /// <param name="a_npcs">List of all active NPCs on the map.</param>
        /// <param name="a_char">The player position, sent by reference.</param>
        public void CheckCollision(ref List<Entities.NPCBad> a_npcs, ref Entities.Player a_char)
        {
            int counter = 0;
            int damage = 0;
            if (a_npcs != null)
            {
                foreach (Entities.NPCBad a in a_npcs)
                {
                    if ( this.IsActive() )
                    {
                        if (new Rectangle((int)m_position.X, (int)m_position.Y, m_anim[m_currentframe].Width, m_anim[m_currentframe].Height)
                            .Intersects(a_npcs[counter].GetRectangle()) && !this.ToPlayer())
                        {
                            bool melee = false;
                            if (a_char.GetClass() == Entities.Player.Class.Warrior) melee = true;
                            if (a_npcs[counter].IsAlive())
                            {
                                damage = CombatControl.DealDamage(a_char, a_npcs[counter], melee, (int)m_modifier);
                                a_npcs[counter].ReduceCurrentHP(damage);
                                if (!a_npcs[counter].IsAlive())
                                {
                                    a_char.GainExp(a_npcs[counter].GetExpForKill());
                                }
                                this.Die();
                            }
                        }
                        if (new Rectangle((int)m_position.X, (int)m_position.Y, m_anim[m_currentframe].Width, m_anim[m_currentframe].Height)
                            .Intersects(a_char.GetRectangle()) && this.ToPlayer())
                        {
                            bool melee = true;
                            damage = CombatControl.DealDamage(a_npcs[counter], a_char, melee, 1);
                            a_char.ReduceCurrentHP(damage);
                            this.Die();
                        }
                    }
                    counter++;
                }  
            }
        }
    }
}
