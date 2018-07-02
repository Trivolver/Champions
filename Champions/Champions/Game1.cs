using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Champions
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /// <summary> This is the Game Engine device. It controls the game's controls. </summary>
        GameControls.Engine m_champions;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Set the screen size
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Add your initialization logic here
            //Initialize the Game Engine
            
            m_champions = new GameControls.Engine();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here


            //Load the champions menu title
            MediaPlayer.Play(Content.Load<Song>("Menu\\Titlesong"));

            m_champions.LoadMenu(Content.Load<Texture2D>("Menu\\Title"), 
                               Content.Load<Texture2D>("Menu\\menuOptions"),
                               Content.Load<Texture2D>("Menu\\Selector"), 
                               Content.Load<Song>("Menu\\TitleSong"));

            m_champions.LoadLoadGame(Content.Load<Texture2D>("LoadGame\\FileSelect"), 
                                    Content.Load<Texture2D>("LoadGame\\Mage"),
                                    Content.Load<Texture2D>("LoadGame\\None"), 
                                    Content.Load<Texture2D>("LoadGame\\SelectorGraphic"),
                                    Content.Load<Texture2D>("LoadGame\\Warrior"),
                                    Content.Load<SpriteFont>("UI\\Menu"));

            m_champions.LoadNewGame(Content.Load<Texture2D>("NewGame\\NewScreen"), 
                                    Content.Load<Texture2D>("NewGame\\NewSelector"));
            m_champions.LoadClassSelect(Content.Load<Texture2D>("NewGame\\classSelect"),
                                        Content.Load<Texture2D>("NewGame\\classWarrior"),
                                        Content.Load<Texture2D>("NewGame\\classMage"));
            m_champions.LoadPlayerResources(Content.Load<Texture2D>("Characters\\Warrior\\Knight_Back_Idle"), 
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Back_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Back_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Back_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Back_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Front_Idle"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Front_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Front_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Front_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Front_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Left_Idle"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Left_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Left_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Left_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Left_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Right_Idle"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Right_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Right_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Right_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Warrior\\Knight_Right_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Idle"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Front_Walk_5"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Idle"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Back_Walk_5"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Idle"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Left_Walk_5"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Idle"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Walk_1"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Walk_2"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Walk_3"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Walk_4"),
                                            Content.Load<Texture2D>("Characters\\Wizard\\Wizard_Right_Walk_5"),
                                            Content.Load<Texture2D>("UI\\UI_Basic"),
                                            Content.Load<SpriteFont>("UI\\LogoCraft"),
                                            Content.Load<SpriteFont>("UI\\ExpHealth"),
                                            Content.Load<Texture2D>("UI\\UI_Healthbar"),
                                            Content.Load<Texture2D>("UI\\UI_ManaBar"),
                                            Content.Load<Texture2D>("UI\\UI_Mage_1"),
                                            Content.Load<Texture2D>("UI\\UI_Mage_2"),
                                            Content.Load<Texture2D>("UI\\UI_Mage_3"),
                                            Content.Load<Texture2D>("UI\\UI_Mage_4"),
                                            Content.Load<Texture2D>("UI\\UI_Mage_5"),
                                            Content.Load<Texture2D>("UI\\UI_Warrior_1"),
                                            Content.Load<Texture2D>("UI\\UI_Warrior_2"),
                                            Content.Load<Texture2D>("UI\\UI_Warrior_3"),
                                            Content.Load<Texture2D>("UI\\UI_Warrior_4"),
                                            Content.Load<Texture2D>("UI\\UI_Warrior_5"),
                                            Content.Load<Texture2D>("Menu\\Pause"),
                                            Content.Load<SpriteFont>("UI\\Menu"),
                                            Content.Load<Texture2D>("UI\\Cursor"),
                                            Content.Load<Texture2D>("Menu\\AddAttribute"),
                                            Content.Load<Texture2D>("Items\\A1"),
                                            Content.Load<Texture2D>("Items\\A2"),
                                            Content.Load<Texture2D>("Items\\A3"),
                                            Content.Load<Texture2D>("Items\\S1"),
                                            Content.Load<Texture2D>("Items\\S2"),
                                            Content.Load<Texture2D>("Items\\S3"),
                                            Content.Load<Texture2D>("Items\\D1"),
                                            Content.Load<Texture2D>("Items\\D2"),
                                            Content.Load<Texture2D>("Items\\D3"),
                                            Content.Load<Texture2D>("Items\\D4"),
                                            Content.Load<Texture2D>("Items\\D5"),
                                            Content.Load<Texture2D>("Items\\D6"));

            m_champions.LoadMaps(Content.Load<Texture2D>("MapOne\\Map1"),
                                 Content.Load<Texture2D>("MapOne\\Map2"),
                                 Content.Load<Texture2D>("Enemies\\Zombie"),
                                 Content.Load<SpriteFont>("MapOne\\EnemyName"),
                                 Content.Load<Texture2D>("MapOne\\TreeRed"),
                                 Content.Load<Texture2D>("MapOne\\TreeGreen"),
                                 Content.Load<Texture2D>("MapOne\\TreeYellow"),
                                 Content.Load<Texture2D>("MapOne\\Rock"),
                                 Content.Load<Texture2D>("MapTwo\\Wall1"),
                                 Content.Load<Texture2D>("MapTwo\\Wall2"),
                                 Content.Load<Texture2D>("MapTwo\\Statue"),
                                 Content.Load<Texture2D>("MapOne\\LevelChanger"),
                                 Content.Load<Texture2D>("Enemies\\Dragon"),
                                 Content.Load<Texture2D>("Enemies\\Doctor"),
                                 Content.Load<Texture2D>("Items\\Loot"));

            m_champions.LoadMusic(Content.Load<Song>("Music\\0"), 
                                    Content.Load<Song>("Music\\1"), 
                                    Content.Load<Song>("Music\\2"), 
                                    Content.Load<Song>("Music\\3"), 
                                    Content.Load<Song>("Music\\4"), 
                                    Content.Load<Song>("Music\\5"));

            Chat.LoadChatWindow(Content.Load<Texture2D>("Chat\\ChatBox"),
                                Content.Load<SpriteFont>("Chat\\ChatFont"));
            
            CombatControl.LoadGraphics(Content.Load<Texture2D>("Attacks\\ArcaneBolt"),
                                        Content.Load<Texture2D>("Attacks\\Attack"),
                                        Content.Load<Texture2D>("Attacks\\Fireball"),
                                        Content.Load<Texture2D>("Attacks\\Hammer"),
                                        Content.Load<Texture2D>("Attacks\\Lightning"),
                                        Content.Load<Texture2D>("Attacks\\Nuke"),
                                        Content.Load<Texture2D>("Attacks\\Viper"));

            m_champions.LoadCompleteGame(Content.Load<Texture2D>("EndGame"));
            m_champions.LoadGameOver(Content.Load<Texture2D>("GameOver"));
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            if (m_champions.State == GameControls.Engine.GameState.Quit) this.Exit();

            // TODO: Add your update logic here
            if (m_champions.State == GameControls.Engine.GameState.Title)
            {
                m_champions.MenuUpdate(gameTime);
            }
            if (m_champions.State == GameControls.Engine.GameState.NewGame)
            {
                m_champions.NewUpdate(gameTime);
            }
            if (m_champions.State == GameControls.Engine.GameState.ClassSelect)
            {
                m_champions.UpdateClassSelect(gameTime);
            }
            if (m_champions.State == GameControls.Engine.GameState.Playing)
            {
                m_champions.GameManager(gameTime);
                CombatControl.UpdateAttacks();
            }
            if (m_champions.State == GameControls.Engine.GameState.Pause)
            {
                m_champions.UpdatePause(gameTime);
            }
            if (m_champions.State == GameControls.Engine.GameState.LoadGame)
            {
                m_champions.UpdateLoadGame(gameTime);
            }
            
            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (m_champions.State == GameControls.Engine.GameState.Title)
            {
                spriteBatch.Begin();
                m_champions.DrawMenu(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.LoadGame)
            {
                spriteBatch.Begin();
                m_champions.DrawLoadGame(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.NewGame)
            {
                spriteBatch.Begin();
                m_champions.DrawNewGame(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.ClassSelect)
            {
                spriteBatch.Begin();
                m_champions.DrawClassSelect(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.Pause)
            {
                spriteBatch.Begin();
                m_champions.PauseGame(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.Playing)
            {
                spriteBatch.Begin();
                m_champions.DrawGameResources(spriteBatch, gameTime);
                Chat.DrawChatWindow(spriteBatch);
                CombatControl.DrawAttacks(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.Completion)
            {
                spriteBatch.Begin();
                m_champions.CompleteGame(spriteBatch);
                spriteBatch.End();
            }
            if (m_champions.State == GameControls.Engine.GameState.GameOver)
            {
                spriteBatch.Begin();
                m_champions.GameOver(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
