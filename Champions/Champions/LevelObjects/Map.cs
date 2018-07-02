using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Champions.LevelObjects
{
    /// <summary>
    /// The map object. Controls all objects on the map.
    /// </summary>
    class Map
    {
        /// <summary>Texture2d Spritesheet containing the zombie.</summary>
        Texture2D m_zombieSheet;
        /// <summary>Spritesheet containing the dragon.</summary>
        Texture2D m_dragonSheet;
        /// <summary>Spritesheet containing the doctor.</summary>
        Texture2D m_doctorSheet;
        /// <summary>Font for drawing names above enemies.</summary>
        SpriteFont m_npcFont;
        /// <summary>All of the map objects on map 1.</summary>
        Vector2[] m_mapObjectCoords1;
        /// <summary>All of the map objects on map 2.</summary>
        Vector2[] m_mapObjectCoords2;
        /// <summary>the coordinates for looting objects.</summary>
        List<Vector2> m_lootCoords;
        /// <summary>Current amount of loot.</summary>
        int m_lootListSize = 0;
        /// <summary>List of all enemies on map 1.</summary>
        List<Entities.NPCBad> m_EnemyMasterList1;
        /// <summary>List of all enemies on map 2.</summary>
        List<Entities.NPCBad> m_EnemyMasterList2;
        /// <summary>Texture for red trees.</summary>
        Texture2D m_tree_red;
        /// <summary>Texture for green trees.</summary>
        Texture2D m_tree_green;
        /// <summary>texture for yellow trees.</summary>
        Texture2D m_tree_yellow;
        /// <summary>Texture for drawing rocks.</summary>
        Texture2D m_rock;
        /// <summary>Texture for wall 1.</summary>
        Texture2D m_wall1;
        /// <summary>Texture for wall 2.</summary>
        Texture2D m_wall2;
        /// <summary>Statue texture.</summary>
        Texture2D m_statue;
        /// <summary>Texture for changing levels.</summary>
        Texture2D m_level1Changer;
        /// <summary>Texture for loot.</summary>
        Texture2D m_loot;
        /// <summary>LARGE texture for map 1.</summary>
        Texture2D m_mapOne;
        /// <summary>LARGE texture for map 2.</summary>
        Texture2D m_mapTwo;
        /// <summary>Currently active map texture.</summary>
        Texture2D m_activeMap;
        /// <summary>currently active map position.</summary>
        Vector2 m_mapPos;
        /// <summary>Random number.</summary>
        Random m_rand;
        /// <summary>Number of the current map.</summary>
        private int m_currentMap;
        /// <summary>Song1.</summary>
        Song m_0;
        /// <summary>Song2.</summary>
        Song m_1;
        /// <summary>Song3.</summary>
        Song m_2;
        /// <summary>Song4.</summary>
        Song m_3;
        /// <summary>Song5.</summary>
        Song m_4;
        /// <summary>Song6.</summary>
        Song m_5;

        /// <summary>
        /// constructor for the map. Initializes lists and entities.
        /// </summary>
        public Map()
        {
            m_mapPos = new Vector2(-2700, -3200);
            m_EnemyMasterList1 = new List<Entities.NPCBad>();
            m_EnemyMasterList2 = new List<Entities.NPCBad>();
            m_lootCoords = new List<Vector2>();
            AssignCoordinates1();
            AssignCoordinates2();
            m_currentMap = 1;
            m_rand = new Random();
        }
        /// <summary>
        /// Loads the map and all the objects.
        /// </summary>
        /// <param name="a_mapOne">First map texture.</param>
        /// <param name="a_mapTwo">Second map texture.</param>
        /// <param name="a_zombie">Zombie enemy texture.</param>
        /// <param name="a_enemyFont">Font for drawing to screen.</param>
        /// <param name="a_tree_red">Red tree texture.</param>
        /// <param name="a_tree_green">Green tree texture.</param>
        /// <param name="a_tree_yellow">Yellow tree texture.</param>
        /// <param name="a_rock">Rock texture.</param>
        /// <param name="a_wall1">Wall texture 1.</param>
        /// <param name="a_wall2">Wall texture 2.</param>
        /// <param name="a_statue">Statue texture.</param>
        /// <param name="a_levelChanger">Texture for changing levle.</param>
        /// <param name="a_dragon">Dragon enemy texture.</param>
        /// <param name="a_doctor">Doctor enemy texture.</param>
        /// <param name="a_loot">Loot texture.</param>
        public void LoadMaps(Texture2D a_mapOne, Texture2D a_mapTwo, Texture2D a_zombie, SpriteFont a_enemyFont,
                            Texture2D a_tree_red, Texture2D a_tree_green, Texture2D a_tree_yellow, Texture2D a_rock,
                            Texture2D a_wall1, Texture2D a_wall2, Texture2D a_statue, Texture2D a_levelChanger,
                            Texture2D a_dragon, Texture2D a_doctor, Texture2D a_loot)
        {
            m_mapOne = a_mapOne;
            m_mapTwo = a_mapTwo;

            m_tree_green = a_tree_green;
            m_tree_red = a_tree_red;
            m_tree_yellow = a_tree_yellow;
            m_rock = a_rock;

            m_wall1 = a_wall1;
            m_wall2 = a_wall2;
            m_statue = a_statue;
            m_level1Changer = a_levelChanger;
            m_loot = a_loot;

            m_zombieSheet = a_zombie;
            m_dragonSheet = a_dragon;
            m_doctorSheet = a_doctor;
            m_npcFont = a_enemyFont;

            AssignEnemyCoordinates1();
            AssignEnemyCoordinates2();
        }

        /// <summary>
        /// chooses the map to draw to the screen. Resets map position.
        /// </summary>
        /// <param name="a_choice">map number.</param>
        public void ChooseMap(int a_choice)
        {
            switch (a_choice)
            {
                case 1:
                    m_currentMap = 1;
                    m_activeMap = m_mapOne;
                    m_mapPos = new Vector2(-2700, -3200);
                    break;
                case 2:
                    m_currentMap = 2;
                    m_activeMap = m_mapTwo;
                    m_mapPos = new Vector2(-2700, -3200);
                    break;
                default:
                    break;
            }
            m_lootCoords = new List<Vector2>();
            m_lootListSize = 0;
        }
        /// <summary>
        /// Draws the current map to the screen and all relevent objects.
        /// </summary>
        /// <param name="a_spriteBatch">The spritebatch for drawing objects.</param>
        /// <param name="a_playerPos">The player position.</param>
        public void DrawCurrentMap(SpriteBatch a_spriteBatch, Vector2 a_playerPos)
        {
            if (m_currentMap == 1)
            {
                //Draw the map objects for map 1
                a_spriteBatch.Draw(m_mapOne, m_mapPos, Color.White);
                //Draw all of the zombies.
                if( m_EnemyMasterList1 != null )
                {
                    int counter = 0; 
                    foreach ( Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        if ( m_EnemyMasterList1[counter].IsAlive())
                        {
                            m_EnemyMasterList1[counter].Draw(a_spriteBatch);
                        }
                        else if (m_EnemyMasterList1[counter].NeedsLooted())
                        {
                            if (m_rand.Next(1, 2) == 1)
                            {
                                m_lootCoords.Add(m_EnemyMasterList1[counter].GetPosition());
                                m_lootListSize++;
                            }
                        }
                        counter++;
                    }
                }
                
                //Draw the Green trees.
                for (int i = 0; i < 10; i++)
                {
                    a_spriteBatch.Draw(m_tree_green, m_mapObjectCoords1[i], Color.White);
                }
                //Draw the Red trees.
                for (int i = 10; i < 16; i++)
                {
                    a_spriteBatch.Draw(m_tree_red, m_mapObjectCoords1[i], Color.White);
                }
                //Draw the Yellow Trees.
                for (int i = 16; i < 19; i++)
                {
                    a_spriteBatch.Draw(m_tree_yellow, m_mapObjectCoords1[i], Color.White);
                }
                //Draw the Rocks.
                for (int i = 19; i < 24; i++)
                {
                    a_spriteBatch.Draw(m_rock, m_mapObjectCoords1[i], Color.White);
                }
                //Draw the level changer
                a_spriteBatch.Draw(m_level1Changer, m_mapObjectCoords1[24], Color.White);
            }
            else //(m_currentMap == 2)
            {
                a_spriteBatch.Draw(m_activeMap, m_mapPos, Color.White);
                a_spriteBatch.Draw(m_wall1, m_mapObjectCoords2[0], Color.White);
                a_spriteBatch.Draw(m_wall2, m_mapObjectCoords2[1], Color.White);
                for (int i = 2; i < 6; i++)
                {
                    a_spriteBatch.Draw(m_statue, m_mapObjectCoords2[i], Color.White);
                }
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        if (m_EnemyMasterList2[counter].IsAlive())
                        {
                            m_EnemyMasterList2[counter].Draw(a_spriteBatch);
                        }
                        else if (m_EnemyMasterList2[counter].NeedsLooted())
                        {
                            if (m_rand.Next(1, 2) == 1)
                            {
                                m_lootCoords.Add(m_EnemyMasterList2[counter].GetPosition());
                                m_lootListSize++;
                            }
                        }
                        counter++;
                    }
                }
            }

            if (m_lootListSize > 0) 
            {
                for ( int i = 0; i < m_lootListSize; i++) 
                {
                    a_spriteBatch.Draw(m_loot, m_lootCoords[i], Color.White);
                    if (GetDistance(a_playerPos, m_lootCoords[i]) <= 50)
                    {
                        a_spriteBatch.DrawString(m_npcFont, "Press space to loot!",
                            new Vector2(640, 500), Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Moves the map left.
        /// </summary>
        /// <param name="a_playerSpeed">The distance to move.</param>
        public void MoveLeft(int a_playerSpeed)
        {
            m_mapPos.X -= a_playerSpeed;
            //Move the map objects.
            if (m_currentMap == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    m_mapObjectCoords1[i].X -= a_playerSpeed;
                }

                if (m_EnemyMasterList1 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        m_EnemyMasterList1[counter].MoveLeft(a_playerSpeed);
                        counter++;
                    }
                }
            }
            //Map #2
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    m_mapObjectCoords2[i].X -= a_playerSpeed;
                }
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        m_EnemyMasterList2[counter].MoveLeft(a_playerSpeed);
                        counter++;
                    }
                }
            }

            if (m_lootCoords != null)
            {
                for ( int i = 0; i < m_lootListSize; i++) 
                {
                    m_lootCoords[i] = new Vector2
                        (m_lootCoords[i].X - a_playerSpeed, m_lootCoords[i].Y);
                }
            }
            CombatControl.MoveObjectsLeft(a_playerSpeed);
        }
        /// <summary>
        /// Moves the map right.
        /// </summary>
        /// <param name="a_playerSpeed">The player speed.</param>
        public void MoveRight(int a_playerSpeed)
        {
            m_mapPos.X += a_playerSpeed;
            if (m_currentMap == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    m_mapObjectCoords1[i].X += a_playerSpeed;
                }
                if (m_EnemyMasterList1 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        m_EnemyMasterList1[counter].MoveRight(a_playerSpeed);
                        counter++;
                    }
                }

            }
            //Map #2
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    m_mapObjectCoords2[i].X += a_playerSpeed;
                }
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        m_EnemyMasterList2[counter].MoveRight(a_playerSpeed);
                        counter++;
                    }
                }
            }

            if (m_lootCoords != null)
            {
                for ( int i = 0; i < m_lootListSize; i++) 
                {
                    m_lootCoords[i] = new Vector2
                        (m_lootCoords[i].X + a_playerSpeed, m_lootCoords[i].Y);
                }
            }
            CombatControl.MoveObjectsRight(a_playerSpeed);
        }
        /// <summary>
        /// Moves the map and all objects up.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move the map.</param>
        public void MoveUp(int a_playerSpeed)
        {
            m_mapPos.Y -= a_playerSpeed;
            if (m_currentMap == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    m_mapObjectCoords1[i].Y -= a_playerSpeed;
                }
                if (m_EnemyMasterList1 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        m_EnemyMasterList1[counter].MoveUp(a_playerSpeed);
                        counter++;
                    }
                }

            }
            //Map #2
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    m_mapObjectCoords2[i].Y -= a_playerSpeed;
                }
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        m_EnemyMasterList2[counter].MoveUp(a_playerSpeed);
                        counter++;
                    }
                }
            }

            if (m_lootCoords != null)
            {
                for ( int i = 0; i < m_lootListSize; i++) 
                {
                    m_lootCoords[i] = new Vector2
                        (m_lootCoords[i].X, m_lootCoords[i].Y - a_playerSpeed);
                }
            }

            CombatControl.MoveObjectsUp(a_playerSpeed);
        }
        /// <summary>
        /// Moves the map and all objects down.
        /// </summary>
        /// <param name="a_playerSpeed">Distance to move the map.</param>
        public void MoveDown(int a_playerSpeed)
        {
            m_mapPos.Y += a_playerSpeed;
            if (m_currentMap == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    m_mapObjectCoords1[i].Y += a_playerSpeed;
                }
                if (m_EnemyMasterList1 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        m_EnemyMasterList1[counter].MoveDown(a_playerSpeed);
                        counter++;
                    }
                }
            }
            //Map #2
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    m_mapObjectCoords2[i].Y += a_playerSpeed;
                }
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        m_EnemyMasterList2[counter].MoveDown(a_playerSpeed);
                        counter++;
                    }
                }
            }

            if (m_lootCoords != null)
            {
                for ( int i = 0; i < m_lootListSize; i++) 
                {
                    m_lootCoords[i] = new Vector2
                        (m_lootCoords[i].X, m_lootCoords[i].Y + a_playerSpeed);
                }
            }
            CombatControl.MoveObjectsDown(a_playerSpeed);
        }
        /// <summary>Returns true if the map is at the left edge of the screen.</summary>
        public bool MapEdgeLeft()
        {
            if (m_mapPos.X >= 0)
            {
                return true;
            }
            else return false;
        }
        /// <summary>Returns true if the map is at the right edge of the screen.</summary>
        public bool MapEdgeRight()
        {
            if (m_mapPos.X < -m_mapOne.Width + 1290)
            {
                return true;
            }
            else return false;
        }
        /// <summary>Returns true if the map is at the top edge of the screen.</summary>
        public bool MapEdgeUp()
        {
            if (m_mapPos.Y >= 0)
            {
                return true;
            }
            else return false;
        }
        /// <summary>Returns true if the map is at the down edge of the screen.</summary>
        public bool MapEdgeDown()
        {
            if (m_mapPos.Y <= -m_mapOne.Height + 720)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Loads the music to the song files.
        /// </summary>
        /// <param name="a_0">Song 1.</param>
        /// <param name="a_1">Song 2.</param>
        /// <param name="a_2">Song 3.</param>
        /// <param name="a_3">Song 4.</param>
        /// <param name="a_4">Song 5.</param>
        /// <param name="a_5">Song 6.</param>
        public void LoadMusic(Song a_0, Song a_1, Song a_2, Song a_3,
                        Song a_4, Song a_5)
        {
            m_0 = a_0;
            m_1 = a_1;
            m_2 = a_2;
            m_3 = a_3;
            m_4 = a_4;
            m_5 = a_5;
        }
        /// <summary>
        /// Updates the currently playing music. 
        /// Note: This is a very taxing call, tabbing out will crash the game.
        /// </summary>
        public void UpdateMusic()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                Random songnum = new Random();
                int song = songnum.Next(0, 5);
                switch (song)
                {
                    case 0:
                        MediaPlayer.Play(m_0);
                        break;
                    case 1:
                        MediaPlayer.Play(m_1);
                        break;
                    case 2:
                        MediaPlayer.Play(m_2);
                        break;
                    case 3:
                        MediaPlayer.Play(m_3);
                        break;
                    case 4:
                        MediaPlayer.Play(m_4);
                        break;
                    case 5:
                        MediaPlayer.Play(m_5);
                        break;                        
                }
            }
        }
        /// <summary>
        /// Assigns all enemy coordinates on map 1.
        /// </summary>
        public void AssignEnemyCoordinates1()
        {
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3000, 3200), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3130, 3610), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3420, 2920), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2700, 3420), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(2970, 2720), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2759, 2700), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2680, 2700), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2500, 2820), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2450, 2540), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2050, 2780), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(1900, 3100), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2250, 3600), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2080, 3800), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(1420, 3500), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(600, 2480), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(2480, 2000), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3190, 2000), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3400, 1850), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3000, 1580), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3050, 1580), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3100, 1580), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3600, 1300), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3600, 1350), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3600, 1400), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3650, 1100), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(3550, 600), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(2700, 850), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(2810, 200), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(2250, 150), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(1800, 550), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(2200, 950), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(1850, 2200), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(1400, 2300), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet,new Vector2(820, 280), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(300, 500), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(3700, 200), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(900, 250), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(400, 250), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(200, 3200), m_mapPos, m_npcFont));
            m_EnemyMasterList1.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(2500, 1400), m_mapPos, m_npcFont));
        }
        /// <summary>
        /// Assigns all enemy coordinates on map 2.
        /// </summary>
        public void AssignEnemyCoordinates2()
        {
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3325, 3725), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3300, 3400), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3325, 2900), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3725, 2850), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(3425, 2200), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(3350, 1375), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(2030, 850), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(2700, 1450), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(2075, 2900), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(475, 2500), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(650, 1775), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Doctor(m_doctorSheet, new Vector2(1075, 1775), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(3550, 400), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(3275, 400), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(2275, 400), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(2525, 3450), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(450, 3200), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Dragon(m_dragonSheet, new Vector2(1250, 2600), m_mapPos, m_npcFont));
            m_EnemyMasterList2.Add(new Enemies.Zombie(m_zombieSheet, new Vector2(900, 720), m_mapPos, m_npcFont));
        }
        /// <summary>
        /// Checks if the game is won (Map 2, positional based).
        /// </summary>
        /// <returns>True if we won the game.</returns>
        public bool CheckWinGame()
        {
            if (GetDistance((new Vector2(0,0)), m_mapPos)<= 100 && m_currentMap == 2)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Updates objects and enemies on the screen.
        /// </summary>
        /// <param name="a_player">The positional rectangle of the player.</param>
        public void UpdateObjects(Rectangle a_player)
        {
            if (m_currentMap == 1)
            {
                if (m_EnemyMasterList1 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList1)
                    {
                        if (m_EnemyMasterList1[counter].IsAlive())
                        {
                            m_EnemyMasterList1[counter].SimulateAI(a_player);
                        }
                        counter++;
                    }

                }
            }
            else
            {
                if (m_EnemyMasterList2 != null)
                {
                    int counter = 0;
                    foreach (Entities.NPCBad enemy in m_EnemyMasterList2)
                    {
                        if (m_EnemyMasterList2[counter].IsAlive())
                        {
                            m_EnemyMasterList2[counter].SimulateAI(a_player);
                        }
                        counter++;
                    }
                }
            }
        }
        /// <summary>
        /// Assigns the object coordinates on map 1.
        /// </summary>
        public void AssignCoordinates1()
        {
            m_mapObjectCoords1 = new Vector2[25];
            //Green tree coordinates
            m_mapObjectCoords1[0] = (new Vector2(2210, 2650)) + m_mapPos;
            m_mapObjectCoords1[1] = (new Vector2(2500, 2000)) + m_mapPos;
            m_mapObjectCoords1[2] = (new Vector2(2900, 2300)) + m_mapPos;
            m_mapObjectCoords1[3] = (new Vector2(3250, 2700)) + m_mapPos;
            m_mapObjectCoords1[4] = (new Vector2(3300, 1725)) + m_mapPos;
            m_mapObjectCoords1[5] = (new Vector2(2900, 1300)) + m_mapPos;
            m_mapObjectCoords1[6] = (new Vector2(2925, 825)) + m_mapPos;
            m_mapObjectCoords1[7] = (new Vector2(2550, 450)) + m_mapPos;
            m_mapObjectCoords1[8] = (new Vector2(2200, 475)) + m_mapPos;
            m_mapObjectCoords1[9] = (new Vector2(2000, 1100)) + m_mapPos;
            //Red tree coordinates
            m_mapObjectCoords1[10] = (new Vector2(1500, 1175)) + m_mapPos;
            m_mapObjectCoords1[11] = (new Vector2(1850, 1875)) + m_mapPos;
            m_mapObjectCoords1[12] = (new Vector2(1575, 2700)) + m_mapPos;
            m_mapObjectCoords1[13] = (new Vector2(1125, 2725)) + m_mapPos;
            m_mapObjectCoords1[14] = (new Vector2(700, 3150)) + m_mapPos;
            m_mapObjectCoords1[15] = (new Vector2(675, 2650)) + m_mapPos;
            //Yellow tree coordinates
            m_mapObjectCoords1[16] = (new Vector2(500, 2175)) + m_mapPos;
            m_mapObjectCoords1[17] = (new Vector2(200, 1100)) + m_mapPos;
            m_mapObjectCoords1[18] = (new Vector2(975, 510)) + m_mapPos;
            //Rock coordinates
            m_mapObjectCoords1[19] = (new Vector2(320, 0)) + m_mapPos;
            m_mapObjectCoords1[20] = (new Vector2(800, 0)) + m_mapPos;
            m_mapObjectCoords1[21] = (new Vector2(1375, 2925)) + m_mapPos;
            m_mapObjectCoords1[22] = (new Vector2(3150, 600)) + m_mapPos;
            m_mapObjectCoords1[23] = (new Vector2(3800, 0)) + m_mapPos;
            m_mapObjectCoords1[24] = (new Vector2(545, 30)) + m_mapPos;
        }
        /// <summary>
        /// Assigns the object coordinates on map 2.
        /// </summary>
        public void AssignCoordinates2()
        {
            m_mapObjectCoords2 = new Vector2[6];
            m_mapObjectCoords2[0] = (new Vector2(3000, 725)) + m_mapPos;
            m_mapObjectCoords2[1] = (new Vector2(1690, 0)) + m_mapPos;
            //Statues
            m_mapObjectCoords2[2] = (new Vector2(2975, 50)) + m_mapPos;
            m_mapObjectCoords2[3] = (new Vector2(1675, 3550)) + m_mapPos;
            m_mapObjectCoords2[4] = (new Vector2(375, 1675)) + m_mapPos;
            m_mapObjectCoords2[5] = (new Vector2(1200, 1675)) + m_mapPos;
        }
        /// <summary>
        /// Checks if the next movement will collide with an object.
        /// </summary>
        /// <param name="a_player">Rectangle based position of the player.</param>
        /// <returns>True if collision.</returns>
        public bool CheckCollision(Rectangle a_player)
        {
            bool collision = false;

            //Map #1
            if (m_currentMap == 1)
            {
                //Check for collision in the green trees.
                for (int i = 0; i < 10; i++)
                {
                    if (new Rectangle(a_player.X, a_player.Y,
                                        a_player.Width, a_player.Height)
                        .Intersects(new Rectangle((int)m_mapObjectCoords1[i].X,
                                                  (int)m_mapObjectCoords1[i].Y,
                                                   m_tree_green.Width,
                                                   m_tree_green.Height)))
                    {
                        collision = true;
                    }
                }
                //Check for collision in the red trees.
                for (int i = 10; i < 16; i++)
                {
                    if (new Rectangle(a_player.X, a_player.Y,
                                       a_player.Width, a_player.Height)
                         .Intersects(new Rectangle((int)m_mapObjectCoords1[i].X,
                                    (int)m_mapObjectCoords1[i].Y,
                                     m_tree_red.Width,
                                     m_tree_red.Height)))
                    {
                        collision = true;
                    }
                }
                //Check for collision in the yellow trees.
                for (int i = 16; i < 19; i++)
                {
                    if (new Rectangle(a_player.X, a_player.Y,
                                       a_player.Width, a_player.Height)
                         .Intersects(new Rectangle((int)m_mapObjectCoords1[i].X,
                                  (int)m_mapObjectCoords1[i].Y,
                                   m_tree_yellow.Width,
                                   m_tree_yellow.Height)))
                    {
                        collision = true;
                    }
                }
                //Check for collision in the rocks.
                for (int i = 19; i < 24; i++)
                {
                    if (new Rectangle(a_player.X, a_player.Y,
                                        a_player.Width, a_player.Height)
                     .Intersects(new Rectangle((int)m_mapObjectCoords1[i].X,
                                  (int)m_mapObjectCoords1[i].Y,
                                   m_rock.Width,
                                   m_rock.Height)))
                    {
                        collision = true;
                    }
                }

            }
            else //Map #2
            {
                //Wall #1
                if (new Rectangle(a_player.X, a_player.Y,
                                        a_player.Width, a_player.Height)
                     .Intersects(new Rectangle((int)m_mapObjectCoords2[0].X,
                                  (int)m_mapObjectCoords2[0].Y,
                                   m_wall1.Width,
                                   m_wall1.Height)))
                    {
                        collision = true;
                    }
                //Wall #2
                if (new Rectangle(a_player.X, a_player.Y,
                        a_player.Width, a_player.Height)
                    .Intersects(new Rectangle((int)m_mapObjectCoords2[1].X,
                          (int)m_mapObjectCoords2[1].Y,
                           m_wall2.Width,
                           m_wall2.Height)))
                    {
                        collision = true;
                    }
                //statues 
                for (int i = 2; i < 6; i++)
                {
                    if (new Rectangle(a_player.X, a_player.Y,
                        a_player.Width, a_player.Height)
                    .Intersects(new Rectangle((int)m_mapObjectCoords2[i].X,
                          (int)m_mapObjectCoords2[i].Y,
                           m_statue.Width,
                           m_statue.Height)))
                    {
                        collision = true;
                    }
                }
                CheckLevelChange(a_player);
            }
            return collision;
        }
        /// <summary>
        /// Checks if any attacks are colliding with players or enemies.
        /// </summary>
        /// <param name="a_char">reference to the currently active player.</param>
        public void CheckAttackCollision(ref Entities.Player a_char)
        {
            if (m_currentMap == 1)
            {
                CombatControl.CheckCollision(ref m_EnemyMasterList1, ref a_char);
            }
            else
            {
                CombatControl.CheckCollision(ref m_EnemyMasterList2, ref a_char);
            }
        }
        /// <summary>
        /// Checks if the player has hit the level change square on map one.
        /// </summary>
        /// <param name="a_player">Rectangle containing the player position and size.</param>
        /// <returns></returns>
        public bool CheckLevelChange(Rectangle a_player)
        {
            if (m_currentMap == 1)
            {
                //Check for level change!
                if (new Rectangle(a_player.X, a_player.Y,
                                        a_player.Width, a_player.Height)
                     .Intersects(new Rectangle((int)m_mapObjectCoords1[24].X,
                                  (int)m_mapObjectCoords1[24].Y,
                                   m_level1Changer.Width,
                                   m_level1Changer.Height)))
                {
                    ChooseMap(2);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// Gets the distance between two vectors.
        /// </summary>
        /// <param name="a_point1">Vector 1.</param>
        /// <param name="a_point2">Vector 2.</param>
        /// <returns></returns>
        public double GetDistance(Vector2 a_point1, Vector2 a_point2)
        {
            return Math.Sqrt((a_point1.X - a_point2.X) * (a_point1.X - a_point2.X)
                                + (a_point1.Y - a_point2.Y) * (a_point1.Y - a_point2.Y));
        }
        /// <summary>
        /// Removes the closest chest to the player and moves it off-screen.
        /// </summary>
        /// <param name="a_player">The player rectangle.</param>
        /// <returns>Returns true if the action was completed.</returns>
        public bool CheckChests(Rectangle a_player)
        {
            bool chest = false;
            if (!(m_lootListSize == 0))
            {
                for (int i = 0; i < m_lootListSize; i++)
                {
                    if (GetDistance(m_lootCoords[i], new Vector2(a_player.X, a_player.Y)) <= 50)
                    {
                        m_lootCoords[i] = new Vector2(10000, 10000);
                        chest =  true;
                    }
                }
            }
            return chest;
        }

    }
}
