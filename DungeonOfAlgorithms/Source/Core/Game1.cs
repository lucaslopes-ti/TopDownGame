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
    
    // Room transition effect
    private Texture2D _fadeTexture;
    private float _fadeAlpha = 0f;
    private bool _isFading = false;
    private bool _fadeOut = true; // true = fading to black, false = fading from black
    private const float FADE_SPEED = 3f;
    private int _pendingRoom = -1;
    private Vector2 _pendingSpawnPosition;

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
        // Room 1 - Coins scattered
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(80, 80)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(150, 150)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(300, 100)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(200, 200)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(400, 150)));
        
        // Room 2 - More coins
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(100, 100)));
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(200, 150)));
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(350, 200)));
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(150, 250)));
        room2.AddItem(ItemFactory.CreateItem("Coin", new Vector2(400, 100)));
        
        // Room 3 - Treasure room with many coins
        room3.AddItem(ItemFactory.CreateItem("Coin", new Vector2(100, 150)));
        room3.AddItem(ItemFactory.CreateItem("Coin", new Vector2(150, 200)));
        room3.AddItem(ItemFactory.CreateItem("Coin", new Vector2(200, 250)));
        room3.AddItem(ItemFactory.CreateItem("Coin", new Vector2(250, 150)));
        room3.AddItem(ItemFactory.CreateItem("Coin", new Vector2(300, 200)));

        // --- Add Enemies ---
        EnemyFactory.Initialize(Content);

        // Room 1: Patrol Slimes - positioned inside the walkable area (right side of room)
        room1.AddEnemy(EnemyFactory.CreateEnemy("Slime", new Vector2(350, 270)));
        room1.AddEnemy(EnemyFactory.CreateEnemy("Slime", new Vector2(400, 200)));

        // Room 2: Ghosts (Fast) - positioned in walkable areas
        room2.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(200, 200)));
        room2.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(350, 150)));
        
        // Room 3: Ghosts guarding the chest
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(150, 200)));
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(200, 200)));
        room3.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(250, 200)));
        
        // Room 3: Victory Chest!
        room3.AddItem(ItemFactory.CreateItem("Chest", new Vector2(200, 250)));

        // --- Add Decor Objects ---
        var objectsTexture = Content.Load<Texture2D>("Items/Objects");
        
        // Objects.png sprite mappings (16x16 tiles):
        // Crate: (0,0), Barrel: (0,16), Box: (16,0), Pot: (32,0)
        
        // Room 1 - Objects in walkable area (right portion of map)
        room1.AddDecor(new DecorObject(objectsTexture, new Vector2(320, 280), new Rectangle(0, 0, 16, 16)));
        room1.AddDecor(new DecorObject(objectsTexture, new Vector2(340, 280), new Rectangle(0, 0, 16, 16)));
        room1.AddDecor(new DecorObject(objectsTexture, new Vector2(420, 270), new Rectangle(0, 16, 16, 16)));
        room1.AddDecor(new DecorObject(objectsTexture, new Vector2(440, 270), new Rectangle(0, 16, 16, 16)));
        
        // Room 2 - Spread around walkable floor
        room2.AddDecor(new DecorObject(objectsTexture, new Vector2(100, 280), new Rectangle(0, 0, 16, 16)));
        // room2.AddDecor(new DecorObject(objectsTexture, new Vector2(300, 280), new Rectangle(16, 0, 16, 16))); // Removed blocking box
        room2.AddDecor(new DecorObject(objectsTexture, new Vector2(400, 280), new Rectangle(0, 16, 16, 16)));
        
        // Room 3 - Treasure room with barrels and crates
        room3.AddDecor(new DecorObject(objectsTexture, new Vector2(100, 280), new Rectangle(0, 16, 16, 16)));
        room3.AddDecor(new DecorObject(objectsTexture, new Vector2(120, 280), new Rectangle(0, 16, 16, 16)));
        room3.AddDecor(new DecorObject(objectsTexture, new Vector2(300, 280), new Rectangle(0, 0, 16, 16)));
        room3.AddDecor(new DecorObject(objectsTexture, new Vector2(320, 280), new Rectangle(0, 0, 16, 16)));


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
        
        // Create fade texture (solid black)
        _fadeTexture = new Texture2D(GraphicsDevice, 1, 1);
        _fadeTexture.SetData(new[] { Color.Black });
        
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

        // Toggle God Mode
        if (Keyboard.GetState().IsKeyDown(Keys.G) && _lastKeyboardState.IsKeyUp(Keys.G))
        {
            _player.IsGodMode = !_player.IsGodMode;
            System.Console.WriteLine($"God Mode: {_player.IsGodMode}");
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
        var currentRoom = DungeonManager.Instance.CurrentRoom;
        
        Vector2 prevPosition = _player.Position;
        _player.Update(gameTime, tilemap);
        
        // Check collision with decor objects and push back if needed
        if (currentRoom.IsCollidingWithDecor(_player.Bounds))
        {
            _player.SetPosition(prevPosition);
        }
        
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
        string godText = _player.IsGodMode ? " | GOD MODE" : "";
        Window.Title = $"HP: {_player.Health} | Score: {_player.Score} | Room: {DungeonManager.Instance.CurrentRoom.Id} | Pos: {(int)_player.Position.X},{(int)_player.Position.Y}{godText}";

        // --- Fade Transition Logic ---
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_isFading)
        {
            if (_fadeOut)
            {
                // Fading to black
                _fadeAlpha += FADE_SPEED * deltaTime;
                if (_fadeAlpha >= 1f)
                {
                    _fadeAlpha = 1f;
                    // Now change the room
                    if (_pendingRoom >= 0)
                    {
                        DungeonManager.Instance.ChangeRoom(_pendingRoom);
                        _player.SetPosition(_pendingSpawnPosition);
                        System.Console.WriteLine($"Transição para Room {_pendingRoom}");
                        _pendingRoom = -1;
                    }
                    _fadeOut = false; // Start fading in
                }
            }
            else
            {
                // Fading from black
                _fadeAlpha -= FADE_SPEED * deltaTime;
                if (_fadeAlpha <= 0f)
                {
                    _fadeAlpha = 0f;
                    _isFading = false;
                    _fadeOut = true; // Reset for next transition
                }
            }
            // Skip normal update during fade
            base.Update(gameTime);
            return;
        }

        // Check Room Transition (use tilemap dimensions)
        int mapWidth = currentRoom.Tilemap.MapWidth;
        int mapHeight = currentRoom.Tilemap.MapHeight;

        // Check for door tiles under player
        int playerCenterX = (int)(_player.Position.X + 8);
        int playerCenterY = (int)(_player.Position.Y + 8);
        int tileUnderPlayer = currentRoom.Tilemap.GetTileAt(playerCenterX, playerCenterY);
        
        // Door transitions - trigger when player walks on empty tile (-1) near an edge
        // This makes openings in the walls act as doors
        bool isOnEmptyTile = tileUnderPlayer == -1;
        bool nearEastEdge = _player.Position.X >= mapWidth - 48;
        bool nearWestEdge = _player.Position.X <= 32;
        bool nearNorthEdge = _player.Position.Y <= 32;
        bool nearSouthEdge = _player.Position.Y >= mapHeight - 48;
        
        if (isOnEmptyTile && nearEastEdge && currentRoom.Connections.ContainsKey("East") && !_isFading)
        {
            _pendingRoom = currentRoom.Connections["East"];
            // Room 3 needs lower Y spawn position
            float spawnY = _pendingRoom == 3 ? 200 : _player.Position.Y;
            _pendingSpawnPosition = new Vector2(80, spawnY);
            _isFading = true;
            _fadeOut = true;
        }
        else if (isOnEmptyTile && nearWestEdge && currentRoom.Connections.ContainsKey("West") && !_isFading)
        {
            _pendingRoom = currentRoom.Connections["West"];
            _pendingSpawnPosition = new Vector2(mapWidth - 100, _player.Position.Y);
            _isFading = true;
            _fadeOut = true;
        }
        else if (isOnEmptyTile && nearNorthEdge && currentRoom.Connections.ContainsKey("North") && !_isFading)
        {
            _pendingRoom = currentRoom.Connections["North"];
            _pendingSpawnPosition = new Vector2(_player.Position.X, mapHeight - 100);
            _isFading = true;
            _fadeOut = true;
        }
        else if (isOnEmptyTile && nearSouthEdge && currentRoom.Connections.ContainsKey("South") && !_isFading)
        {
            _pendingRoom = currentRoom.Connections["South"];
            _pendingSpawnPosition = new Vector2(_player.Position.X, 80);
            _isFading = true;
            _fadeOut = true;
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
            if (_player.IsGodMode)
            {
                 DrawShadowString(_font, "GOD MODE", new Vector2(10, 50), Color.Yellow);
            }
        }
        
        _spriteBatch.End();
        
        // Draw fade overlay (on top of everything)
        if (_fadeAlpha > 0)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_fadeTexture, new Rectangle(0, 0, 800, 600), Color.White * _fadeAlpha);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }

    private void DrawShadowString(SpriteFont font, string text, Vector2 position, Color color)
    {
        _spriteBatch.DrawString(font, text, position + new Vector2(1, 1), Color.Black);
        _spriteBatch.DrawString(font, text, position, color);
    }
}
