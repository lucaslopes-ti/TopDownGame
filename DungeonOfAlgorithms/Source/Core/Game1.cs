using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    // Entities & Systems
    private Player _player;
    private Camera _camera;
    private HUD _hud;
    private GameState _gameState = GameState.MainMenu;
    private SpriteFont _font;
    private KeyboardState _lastKeyboardState;
    private System.Collections.Generic.Dictionary<string, Texture2D> _playerTextures;
    private Texture2D _vignetteTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        GameManager.Instance.Initialize();
        _camera = new Camera();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load Tileset
        var tilesetTexture = Content.Load<Texture2D>("Tiles/Tileset");

        // --- Create Room 1 (The Stack) ---
        int[,] map1 = DungeonManager.Instance.LoadMapFromCSV("Content/Maps/Room_01.csv");
        var room1 = new Room(1, new Tilemap(tilesetTexture, map1, 16, 16));

        // --- Create Room 2 (The Heap) ---
        int[,] map2 = DungeonManager.Instance.LoadMapFromCSV("Content/Maps/Room_02.csv");
        var room2 = new Room(2, new Tilemap(tilesetTexture, map2, 16, 16));

        // --- Create Room 3 (Kernel Panic) ---
        int[,] map3 = DungeonManager.Instance.LoadMapFromCSV("Content/Maps/Room_03.csv");
        var room3 = new Room(3, new Tilemap(tilesetTexture, map3, 16, 16));

        // --- Connect Rooms (Graph Edges) ---
        // Room 1 (East) -> Room 2
        room1.Connect("East", 2); 
        // Room 2 (West) -> Room 1, (East) -> Room 3
        room2.Connect("West", 1);
        room2.Connect("East", 3);
        // Room 3 (West) -> Room 2
        room3.Connect("West", 2);

        // --- Add Items ---
        ItemFactory.Initialize(Content);
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(80, 80))); // Hint
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(100, 100)));

        // --- Add Enemies ---
        EnemyFactory.Initialize(Content);

        // Room 1: Patrol Slimes (Safe spots - Left side)
        room1.AddEnemy(EnemyFactory.CreateEnemy("Slime", new Vector2(32, 50)));
        room1.AddEnemy(EnemyFactory.CreateEnemy("Slime", new Vector2(32, 120)));

        // Room 2: Ghosts (Fast)
        room2.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(150, 50)));
        room2.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(150, 150)));
        
        // Room 3: Boss? For now a bunch of Ghosts guarding the chest
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(100, 80)));
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(150, 80)));
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(200, 80)));
        
        // Room 3: Victory Chest!
        room3.AddItem(ItemFactory.CreateItem("Chest", new Vector2(200, 100)));

        // Setup Manager
        DungeonManager.Instance.AddRoom(room1);
        DungeonManager.Instance.AddRoom(room2);
        DungeonManager.Instance.AddRoom(room3);
        DungeonManager.Instance.ChangeRoom(1);


        // Load Player Sprite
        _playerTextures = new System.Collections.Generic.Dictionary<string, Texture2D>
        {
            { "Down", Content.Load<Texture2D>("Player/Player_Down") },
            { "Side", Content.Load<Texture2D>("Player/Player_Side") },
            { "Up", Content.Load<Texture2D>("Player/Player_Up") },
            { "Down_Idle", Content.Load<Texture2D>("Player/Player_Down_Idle") },
            { "Side_Idle", Content.Load<Texture2D>("Player/Player_Side_Idle") },
            { "Up_Idle", Content.Load<Texture2D>("Player/Player_Up_Idle") }
        };
        // _playerTexture = playerTextures["Down"]; // Default for ref, but unused by new constructor

        // Instantiate Player (Spawn at safe location in imported Room 1)
        _player = new Player(_playerTextures, new Vector2(50, 80));
        
        // Load Font and Create HUD
        _font = Content.Load<SpriteFont>("Fonts/GameFont");
        _hud = new HUD(_font);
        
        // --- Atmosphere: Generate Vignette Texture ---
        _vignetteTexture = new Texture2D(GraphicsDevice, 800, 600);
        Color[] data = new Color[800 * 600];
        Vector2 center = new Vector2(400, 300);
        float maxDist = Vector2.Distance(center, Vector2.Zero);
        
        for (int i = 0; i < data.Length; i++)
        {
            int x = i % 800;
            int y = i / 800;
            float dist = Vector2.Distance(new Vector2(x, y), center);
            float factor = dist / maxDist;
            factor = (float)Math.Pow(factor, 2.0); // Non-linear fade
            
            // Dark vignette
            data[i] = Color.Black * Math.Min(factor * 1.8f, 0.85f);
        }
        _vignetteTexture.SetData(data);
        
        _gameState = GameState.MainMenu;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        InputManager.Instance.Update();
        
        // Handle game states
        if (_gameState == GameState.MainMenu)
        {
            Window.Title = "Dungeon of Algorithms - Press ENTER to Start";
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _gameState = GameState.Playing;
            }
            return;
        }
        
        if (_gameState == GameState.GameOver)
        {
            Window.Title = "GAME OVER - Press R to Restart";
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Reset player
                _player = new Player(_playerTextures, new Vector2(100, 100));
                DungeonManager.Instance.ChangeRoom(1);
                _gameState = GameState.Playing;
            }
            return;
        }
        
        // Check for Pause toggle
        if (Keyboard.GetState().IsKeyDown(Keys.P) && _lastKeyboardState.IsKeyUp(Keys.P))
        {
            _gameState = _gameState == GameState.Paused ? GameState.Playing : GameState.Paused;
        }
        _lastKeyboardState = Keyboard.GetState();
        
        if (_gameState == GameState.Paused)
        {
            Window.Title = "PAUSED - Press P to Resume";
            return;
        }
        
        if (_gameState == GameState.Victory)
        {
             Window.Title = "VICTORY! - Press R to Restart";
             if (Keyboard.GetState().IsKeyDown(Keys.R))
             {
                 // Reset player
                 _player = new Player(_playerTextures, new Vector2(100, 100));
                 DungeonManager.Instance.ChangeRoom(1);
                 _gameState = GameState.Playing;
             }
             return;
        }

        // Playing state
        var tilemap = DungeonManager.Instance.CurrentRoom.Tilemap;
        _player.Update(gameTime, tilemap);
        
        // Check if player died
        if (!_player.IsAlive)
        {
            _gameState = GameState.GameOver;
            return;
        }

        // Update Enemies in Current Room (Strategy)
        DungeonManager.Instance.CurrentRoom.Update(gameTime, _player, (item) => 
        {
             if (item is DungeonOfAlgorithms.Source.Entities.ChestItem)
             {
                 _gameState = GameState.Victory;
             }
        });

        // DEBUG: Show position in Title
        Window.Title = $"HP: {_player.Health} | Score: {_player.Score} | Room: {DungeonManager.Instance.CurrentRoom.Id}";

        // Check Room Transition (use tilemap dimensions)
        var currentRoom = DungeonManager.Instance.CurrentRoom;
        int mapWidth = currentRoom.Tilemap.MapWidth;
        int mapHeight = currentRoom.Tilemap.MapHeight;

        // Check for door tiles under player
        int playerCenterX = (int)(_player.Position.X + 8);
        int playerCenterY = (int)(_player.Position.Y + 8);
        int tileUnderPlayer = currentRoom.Tilemap.GetTileAt(playerCenterX, playerCenterY);
        
        // Door transitions
        if (tileUnderPlayer == Tilemap.DoorEast && currentRoom.Connections.ContainsKey("East"))
        {
            DungeonManager.Instance.ChangeRoom(currentRoom.Connections["East"]);
            _player.SetPosition(new Vector2(32, _player.Position.Y)); // Spawn near west side of new room
        }
        else if (tileUnderPlayer == Tilemap.DoorWest && currentRoom.Connections.ContainsKey("West"))
        {
            DungeonManager.Instance.ChangeRoom(currentRoom.Connections["West"]);
            _player.SetPosition(new Vector2(mapWidth - 48, _player.Position.Y)); // Spawn near east side
        }
        else if (tileUnderPlayer == Tilemap.DoorNorth && currentRoom.Connections.ContainsKey("North"))
        {
            DungeonManager.Instance.ChangeRoom(currentRoom.Connections["North"]);
            _player.SetPosition(new Vector2(_player.Position.X, mapHeight - 48));
        }
        else if (tileUnderPlayer == Tilemap.DoorSouth && currentRoom.Connections.ContainsKey("South"))
        {
            DungeonManager.Instance.ChangeRoom(currentRoom.Connections["South"]);
            _player.SetPosition(new Vector2(_player.Position.X, 32));
        }
        
        // --- Fallback: Edge-based Transitions (if map triggers fail) ---
        if (_player.Position.X >= mapWidth - 32 && currentRoom.Connections.ContainsKey("East"))
        {
             DungeonManager.Instance.ChangeRoom(currentRoom.Connections["East"]);
             _player.SetPosition(new Vector2(50, _player.Position.Y));
        }
        else if (_player.Position.X <= 0 && currentRoom.Connections.ContainsKey("West"))
        {
             DungeonManager.Instance.ChangeRoom(currentRoom.Connections["West"]);
             _player.SetPosition(new Vector2(mapWidth - 50, _player.Position.Y));
        }
        
        try
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F5))
            {
                DatabaseManager.Instance.SaveGame(DungeonOfAlgorithms.Source.Core.DungeonManager.Instance.CurrentRoom.Id, _player.Position, 0);
                Window.Title = "Game Saved!";
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F9))
            {
                var data = DatabaseManager.Instance.LoadGame();
                if (data != null)
                {
                    DungeonOfAlgorithms.Source.Core.DungeonManager.Instance.ChangeRoom(data.Value.Level);
                    _player.SetPosition(data.Value.Position);
                    Window.Title = "Game Loaded!";
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("CRITICAL ERROR: " + ex.Message);
            Window.Title = "Error: " + ex.Message;
        }

        // Update Camera
        _camera.Follow(_player.Position, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin with Camera Transform
        _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
        
        DungeonManager.Instance.CurrentRoom.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        
        _spriteBatch.End();

        // Draw HUD (no camera transform - screen space)
        _spriteBatch.Begin();
        
        // Draw Vignette (Atmosphere)
        if (_vignetteTexture != null)
             _spriteBatch.Draw(_vignetteTexture, Vector2.Zero, Color.White);

        if (_gameState == GameState.MainMenu)
        {
            DrawShadowString(_font, "DUNGEON OF ALGORITHMS", new Vector2(200, 150), Color.Gold);
            DrawShadowString(_font, "The Memory Leak Chronicle", new Vector2(250, 180), Color.LightGreen); // Subtitle
            DrawShadowString(_font, "Press ENTER to Start Debugging", new Vector2(220, 300), Color.White);
            DrawShadowString(_font, "WASD to Move | F5 Save | F9 Load", new Vector2(180, 500), Color.Gray);
        }
        else if (_gameState == GameState.GameOver)
        {
            DrawShadowString(_font, "KERNEL PANIC (GAME OVER)", new Vector2(260, 180), Color.Red);
            DrawShadowString(_font, $"Leaked Objects Cleaned: {_player.Score}", new Vector2(260, 220), Color.White);
            DrawShadowString(_font, "Press R to Reboot System", new Vector2(250, 260), Color.Gray);
        }
        else if (_gameState == GameState.Victory)
        {
            DrawShadowString(_font, "SYSTEM RESTORED (VICTORY!)", new Vector2(260, 150), Color.Gold);
            DrawShadowString(_font, "You patched the memory leak!", new Vector2(240, 200), Color.White);
            DrawShadowString(_font, $"Final Score: {_player.Score}", new Vector2(260, 250), Color.White);
            DrawShadowString(_font, "Press R to Reboot", new Vector2(320, 300), Color.Gray);
        }
        else
        {
            _hud.Draw(_spriteBatch, _player);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawShadowString(SpriteFont font, string text, Vector2 position, Color color)
    {
        _spriteBatch.DrawString(font, text, position + new Vector2(1, 1), Color.Black);
        _spriteBatch.DrawString(font, text, position, color);
    }
}
