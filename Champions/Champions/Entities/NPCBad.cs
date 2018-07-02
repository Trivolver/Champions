using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Champions.Entities
{
    /// <summary>
    /// This is an enemy class on top of the Character base class.
    /// </summary>
    class NPCBad : Character
    {
        /// <summary> The rectangle to draw to the screen. </summary>
        protected Rectangle m_activeRec;
        /// <summary> Directional state of the character. </summary>
        protected enum State {Idle, IdleLeft, IdleUp, IdleDown, IdleRight, MoveLeft, MoveRight, MoveUp, MoveDown };
        protected bool m_active;
        /// <summary>Set to true if the enemy is chasing the player.</summary>
        protected bool m_pursuePlayer;
        /// <summary>Vector-based original position of the enemy.</summary>
        protected Vector2 m_origin;
        /// <summary>Current direction of this character. Numbers correspond as follows:
        /// 0  1  2   0 = NW, 1 = N, 2 = NE 
        /// 3  4  5   3 = W, 4 = Idle, 5 = E
        /// 6  7  8   6 = SW, 7 = S, 8 = SE</summary>
        protected int m_direction;
        /// <summary>Distance to be able to roam.</summary>
        protected int m_wanderDistance;
        /// <summary>Distance the enemy can detect the character from.</summary>
        int m_detectDistance;
        /// <summary>Distance the enemy will pursue the character.</summary>
        int m_pursueDistance;
        /// <summary>Current direction of the character, based on the enum "State".</summary>
        protected State m_state;
        /// <summary>Set to true if this character has a melee attack.</summary>
        protected bool m_isMelee;
        /// <summary>Set to true if this character has a ranged attack.</summary>
        protected bool m_isRanged;
        /// <summary>Distance to be able to attack from ranged.</summary>
        protected int m_rangedAttackDistance;
        /// <summary>Distance to be able to attack from melee.</summary>
        protected int m_meleeAttackDistance;
        /// <summary>Set to true if this character returns to the origin.</summary>
        protected bool m_returnToOrigin;
        /// <summary>Random number.</summary>
        Random rand;
        /// <summary>The texture2d containing the sprite.</summary>
        protected Texture2D m_sheet;
        /// <summary>Primary frame position to use to draw.</summary>
        protected Rectangle m_framePosition;
        /// <summary>Spritefont to use to draw this enemy's name.</summary>
        protected SpriteFont m_npcName;
        /// <summary>The position to draw the character's name.</summary>
        protected Vector2 m_namePos;
        /// <summary>String containing this enemy's name.</summary>
        protected String m_name;
        /// <summary>Number of frames to draw.</summary>
        protected int m_frames = 4;
        /// <summary>Timer to draw with.</summary>
        protected int m_timer = 0;
        /// <summary>Interval to switch frames.</summary>
        protected int m_interval = 7;
        /// <summary>Current frame to draw.</summary>
        protected int m_currentFrame = 0;
        /// <summary>Rectangle containing the backwards animation of the sprite.</summary>
        protected Rectangle[] m_backwards;
        /// <summary>Rectangle containing the forwards animation of the sprite.</summary>
        protected Rectangle[] m_forwards;
        /// <summary>Rectangle containing the left animation of the sprite.</summary>
        protected Rectangle[] m_left;
        /// <summary>Rectangle containing the right side animation of the sprite.</summary>
        protected Rectangle[] m_right;
        /// <summary>If this enemy needs to drop loot, it will be set to true.</summary>
        private bool m_needsLooted = true;
        /// <summary>Total time of the attack cooldown.</summary>
        private int m_attackTime = 300;
        /// <summary>Current timer to compare to the attack time.</summary>
        private int m_attackTimer = 0;

        /// <summary>
        /// Constructor for this enemy object. Instatiates some variables.
        /// </summary>
        public NPCBad()
        {
            rand = new Random();
            m_active = false;
            m_pursuePlayer = false;
            m_direction = 5;
            m_state = State.Idle;
            m_returnToOrigin = false;
            m_isEnemy = true;
        }

        /// <summary>
        /// Sets attributes for the Artificial Intellignce.
        /// </summary>
        /// <param name="a_wanderDistance">Maximum distance that enemy will wander.</param>
        /// <param name="a_detectDistance">Distance when the enemy will detect the player.</param>
        /// <param name="a_pursueDistance">Maximum distance the enemy will chase the player.</param>
        /// <param name="a_isRanged">Flags true if the enemy has a ranged attack.</param>
        /// <param name="a_isMelee">Flags true if the enemy has a melee attack.</param>
        /// <param name="a_rangedDistance">Distance where an enemy will attack the player with a ranged attack.</param>
        /// <param name="a_meleeDistance">Distance where an enemy will attack the player with a melee attack.</param>
        public void SetNPCAttributes(int a_wanderDistance, int a_detectDistance, int a_pursueDistance,
                                    bool a_isRanged, bool a_isMelee, int a_rangedDistance, int a_meleeDistance)
        {
            m_wanderDistance = a_wanderDistance;
            m_detectDistance = a_detectDistance;
            m_pursueDistance = a_pursueDistance;
            m_isRanged = a_isRanged;
            m_isMelee = a_isMelee;
            m_rangedAttackDistance = a_rangedDistance;
            m_meleeAttackDistance = a_meleeDistance;
        }
        /// <summary>
        /// Gets the distance between two vectors.
        /// </summary>
        /// <param name="a_point1">Source.</param>
        /// <param name="a_point2">Source 2.</param>
        /// <returns></returns>
        public double GetDistance(Vector2 a_point1, Vector2 a_point2)
        {
            return Math.Sqrt((a_point1.X - a_point2.X) * (a_point1.X - a_point2.X)
                                + (a_point1.Y - a_point2.Y) * (a_point1.Y - a_point2.Y));
        }
        /// <summary>
        /// The NPC will move left according to its speed.
        /// </summary>
        public void MoveLeft()
        {
            m_position.X -= m_speed;
            m_state = State.MoveLeft;
        }
        /// <summary>
        /// The NPC will move left according to the relative player's position.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move.</param>
        public void MoveLeft(int a_playerSpeed)
        {
            m_position.X -= a_playerSpeed;
            m_origin.X -= a_playerSpeed;
        }
        /// <summary>
        /// The NPC will move up according to its speed.
        /// </summary>
        public void MoveUp()
        {
            m_position.Y -= m_speed;
            m_state = State.MoveUp;
        }
        /// <summary>
        /// The NPC will move up according to the relative player's position.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move.</param>
        public void MoveUp(int a_playerSpeed)
        {
            m_position.Y -= a_playerSpeed;
            m_origin.Y -= a_playerSpeed;
        }
        /// <summary>
        /// The NPC will move down according to its speed.
        /// </summary>
        public void MoveDown()
        {
            m_position.Y += m_speed;
            m_state = State.MoveDown;
        }
        /// <summary>
        /// The NPC will move down according to the relative player's position.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move.</param>
        public void MoveDown(int a_playerSpeed)
        {
            m_position.Y += a_playerSpeed;
            m_origin.Y += a_playerSpeed;
        }
        /// <summary>
        /// The NPC will move right according to its speed.
        /// </summary>
        public void MoveRight()
        {
            m_position.X += m_speed;
            m_state = State.MoveRight;
        }
        /// <summary>
        /// The NPC will move right according to the relative player's position.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move.</param>
        public void MoveRight(int a_playerSpeed)
        {
            m_position.X += a_playerSpeed;
            m_origin.X += a_playerSpeed;
        }
        /// <summary>
        /// Enemy will move based on its current direction.
        /// </summary>
        public void MoveCurrentDirection()
        {
            switch (m_direction)
            {
                case 1:
                    MoveUp();
                    MoveLeft();
                    break;
                case 2:
                    MoveUp();
                    break;
                case 3:
                    MoveUp();
                    MoveRight();
                    break;
                case 4:
                    MoveLeft();
                    break;
                case 5:
                    m_state = State.Idle;
                    break;
                case 6:
                    MoveRight();
                    break;
                case 7:
                    MoveLeft();
                    MoveDown();
                    break;
                case 8:
                    MoveDown();
                    break;
                case 9:
                    MoveDown();
                    MoveRight();
                    break;
                default:
                    m_state = State.Idle;
                    m_direction = 5;
                    break;
            }
        }
        /// <summary>
        /// Character will wander. Has a low chance to change its direction.
        /// </summary>
        public void DoWander()
        {
            /* Choose a direction to wander.
                 * The current direction takes great precedence - 
                 * all values above the current rand system are considered the "current"
                 * direction.
                 * 1 - NW  2 - N  3 - NE
                 * 4 - W  5 - Idle  6 - E
                 * 7 - SW  8 - S  9 - SE
                 * */
  
            int direction = rand.Next(1, 200);

            switch (direction)
            {
                case 1:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 1;
                        MoveUp();
                        MoveLeft();
                    }
                    break;
                case 2:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 2;
                        MoveUp();
                    }
                    break;
                case 3:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 3;
                        MoveUp();
                        MoveRight();
                    }       
                    break;
                case 4:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 4;
                        MoveLeft();
                    }
                    break;
                case 5:
                    m_direction = 5;
                    m_state = State.Idle;
                    break;
                case 6:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 6;
                        MoveRight();
                    }
                    break;
                case 7:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 7;
                        MoveLeft();
                        MoveDown();
                    }
                    break;
                case 8:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 8;
                        MoveDown();
                    }
                    break;
                case 9:
                    if (GetDistance(new Vector2(m_position.X, m_position.Y), m_origin) > m_wanderDistance)
                    {
                        m_returnToOrigin = true;
                    }
                    else
                    {
                        m_direction = 9;
                        MoveRight();
                        MoveDown();
                    }
                    break;
                default:
                    if ( GetDistance(m_position, m_origin) > m_wanderDistance )
                    {
                        m_returnToOrigin = true;
                    }
                    MoveCurrentDirection();
                    break;
            }
        }
        /// <summary>
        /// Updates the AI position of this enemy. Will pursue the player, wander, or attack.
        /// </summary>
        /// <param name="a_player">The player position rectangle.</param>
        public void SimulateAI(Rectangle a_player)
        {
            DetectPlayers(new Vector2(a_player.X, a_player.Y));
            double playerdist = GetDistance(new Vector2(a_player.X, a_player.Y), m_position);
            //If too far, return to origin. Will continue to do so until within 50 pixels.
            if (m_returnToOrigin == true)
            {
                ReturnToOrigin();
            }
            //If is in pursuit of player, will chase it.
            else if (m_pursuePlayer == true)
            {
                ChasePlayer(a_player);
            }
            //If none of the above conditions are met, will wander.
            else
            {
                DoWander();
            }
            //Dies if the current health is less than zero.
            if (GetCurrentHealth() <= 0) Die();
        }
        /// <summary>
        /// Chases the player's current position. Updates the attack timer and 
        /// attacks the player if he is close enough.
        /// </summary>
        /// <param name="a_playerPosition">Rectangle of the player position.</param>
        public void ChasePlayer(Rectangle a_playerPosition)
        {
            if (m_attackTimer > 0) m_attackTimer--;

            if (GetDistance(m_position, m_origin) > m_pursueDistance)
            {
                m_returnToOrigin = true;
                m_pursuePlayer = false;
            }
            else if (m_position.X < a_playerPosition.X && m_position.Y < a_playerPosition.Y)
            {
                MoveRight();
                MoveDown();
            }
            else if (m_position.X > a_playerPosition.X && m_position.Y > a_playerPosition.Y)
            {
                MoveLeft();
                MoveUp();
            }
            else if (m_position.X > a_playerPosition.X && m_position.Y < a_playerPosition.Y)
            {
                MoveLeft();
                MoveDown();
            }
            else if (m_position.X < a_playerPosition.X && m_position.Y > a_playerPosition.Y)
            {
                MoveRight();
                MoveUp();
            }
            else if (m_position.X > a_playerPosition.X)
            {
                MoveLeft();
            }
            else if (m_position.X < a_playerPosition.X)
            {
                MoveRight();
            }
            else if (m_position.Y < a_playerPosition.Y)
            {
                MoveDown();
            }
            else //(m_position.Y > a_playerPosition.Y)
            {
                MoveUp();
            }

            if (GetDistance(new Vector2(a_playerPosition.X, a_playerPosition.Y),
                m_position) <= m_meleeAttackDistance && m_attackTimer == 0)
            {
                CombatControl.IssueAttack(this, 0);
                m_attackTimer = 60;
            }
        }
        /// <summary>
        /// Checks if the player is within the detect distance.
        /// </summary>
        /// <param name="a_player">Vector2 position of the player.</param>
        public void DetectPlayers(Vector2 a_player)
        {
            double dist = GetDistance(a_player, m_position);
            if (dist < m_detectDistance) m_pursuePlayer = true;
            else m_pursuePlayer = false;
        }
        /// <summary>
        /// Returns to the origin. When there, resumes wandering.
        /// </summary>
        public void ReturnToOrigin( )
        {
            if (GetDistance(m_origin, m_position) <= m_wanderDistance)
            {
                m_returnToOrigin = false;
            }
            else
            {
                //Move towards origin based on position
                if (m_position.X < m_origin.X && m_position.Y < m_origin.Y)
                {
                    MoveRight();
                    MoveDown();
                }
                else if (m_position.X > m_origin.X && m_position.Y < m_origin.Y)
                {
                    MoveLeft();
                    MoveDown();
                }
                else if (m_position.X < m_origin.X && m_position.Y > m_origin.Y)
                {
                    MoveRight();
                    MoveUp();
                }
                else if (m_position.X > m_origin.X && m_position.Y > m_origin.Y)
                {
                    MoveLeft();
                    MoveUp();
                }
                else if (m_position.X > m_origin.X)
                {
                    MoveLeft();
                }
                else if (m_position.X < m_origin.X)
                {
                    MoveRight();
                }
                else if (m_position.Y < m_origin.Y)
                {
                    MoveDown();
                }
                else
                {
                    MoveUp();
                }
            }

        }
        /// <summary>
        /// Sets the origin based on the map position.
        /// </summary>
        /// <param name="a_origin">Origin to set to.</param>
        /// <param name="a_mapPosition">Current map position.</param>
        public void SetOrigin(Vector2 a_origin, Vector2 a_mapPosition)
        {
            m_origin = a_mapPosition + a_origin;
            m_position = m_origin;
        }
        /// <summary>
        /// Reverses the monster's direction.
        /// </summary>
        public void ReverseDirection()
        {
            switch (m_direction)
            {
                case 1:
                    m_direction = 9;
                    break;
                case 2:
                    m_direction = 8;
                    break;
                case 3:
                    m_direction = 7;
                    break;
                case 4:
                    m_direction = 6;
                    break;
                case 5:
                    m_direction = 5;
                    break;
                case 6:
                    m_direction = 4;
                    break;
                case 7:
                    m_direction = 3;
                    break;
                case 8:
                    m_direction = 2;
                    break;
                case 9:
                    m_direction = 1;
                    break;
                default:
                    m_direction = 5;
                    break;
            }
        }
        /// <summary>
        /// Gets this entitiy's rectangle.
        /// </summary>
        /// <returns>The rectangle of this sprite.</returns>
        public Rectangle GetRectangle()
        {
            return m_activeRec;
        }
        /// <summary>
        /// Sets the attributes used for drawing frames.
        /// </summary>
        /// <param name="a_frames">Number of frames in the animation.</param>
        /// <param name="a_name">Name of this npc.</param>
        public void SetDrawAttributes(int a_frames, String a_name)
        {
            m_currentFrame = 0;
            m_frames = a_frames;
            m_name = a_name;
        }
        /// <summary>
        /// Makes the active rectangle to return.
        /// </summary>
        public void MakeRectangle()
        {
            m_activeRec = new Rectangle((int)m_position.X, (int)m_position.Y, (int)m_forwards[0].Width, (int)m_forwards[0].Height);
        }
        /// <summary>
        /// Draws this animation/sprite to the screen.
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch used for drawing.</param>
        public void Draw(SpriteBatch a_spriteBatch)
        {
            m_timer += 1;
            if (m_timer > m_interval)
            {
                m_currentFrame++;
                m_timer = 0;
            }
            if (m_currentFrame > 3)
            {
                m_currentFrame = 0;
                m_timer = 0;
            }

            switch (m_state)
            {
                case State.Idle:
                    m_framePosition = m_forwards[0];
                    break;
                case State.MoveLeft:
                    m_framePosition = m_left[m_currentFrame];
                    break;
                case State.MoveRight:
                    m_framePosition = m_right[m_currentFrame];
                    break;
                case State.MoveUp:
                    m_framePosition = m_backwards[m_currentFrame];
                    break;
                case State.MoveDown:
                    m_framePosition = m_forwards[m_currentFrame];
                    break;
            }
            m_namePos = new Vector2(m_position.X + 5, m_position.Y - 10);
            a_spriteBatch.Draw(m_sheet, m_position, m_framePosition, Color.White);
            a_spriteBatch.DrawString(m_npcName, m_name, m_namePos, Color.White);
            MakeRectangle();
        }
        /// <summary>
        /// Gets the amount of exp for killing this enemy.
        /// </summary>
        /// <returns>Returns the exp for killing this enemy.</returns>
        public int GetExpForKill()
        {
            return m_expforkill;
        }
        /// <summary>
        /// checks if this enemy needs to be looted. Sets it false.
        /// </summary>
        /// <returns>The boolean value if this needs to be looted.</returns>
        public bool NeedsLooted()
        {
            bool needs;
            needs = m_needsLooted;
            m_needsLooted = false;
            return needs;
        }
    }//End of class definition
}//End of namespace definition
