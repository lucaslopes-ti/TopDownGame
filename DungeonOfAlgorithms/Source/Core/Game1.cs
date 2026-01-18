using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        // --- Create Room 1 from Tiled CSV ---
        int[,] map1 = DungeonManager.Instance.LoadMapFromCSV("Content/Maps/Room_01.csv");
        var room1 = new Room(1, new Tilemap(tilesetTexture, map1, 16, 16));

        // --- Create Room 2 from Tiled CSV ---
        int[,] map2 = DungeonManager.Instance.LoadMapFromCSV("Content/Maps/Room_02.csv");
        var room2 = new Room(2, new Tilemap(tilesetTexture, map2, 16, 16));

        // --- Connect Rooms (Graph Edges) ---
        // Room 1 (East) -> Room 2
        room1.Connect("East", 2);
        // Room 2 (West) -> Room 1
        room2.Connect("West", 1);

        // --- Add Items (Factory Pattern) ---
        ItemFactory.Initialize(Content);
        
        // Spawn Coins in Room 1
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(200, 200)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(250, 200)));
        room1.AddItem(ItemFactory.CreateItem("Coin", new Vector2(300, 200)));

        // --- Add Enemies (Strategy Pattern) ---
        EnemyFactory.Initialize(Content);

        // Room 1: Slime (Patrols)
        room1.AddEnemy(EnemyFactory.CreateEnemy("Slime", new Vector2(150, 150)));

        // Room 2: Ghost (Chases)
        room2.AddEnemy(EnemyFactory.CreateEnemy("Ghost", new Vector2(200, 200)));
        
        // Room 2: Victory Chest!
        room2.AddItem(ItemFactory.CreateItem("Chest", new Vector2(150, 100)));

        // Setup Manager
        DungeonManager.Instance.AddRoom(room1);
        DungeonManager.Instance.AddRoom(room2);
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

        // Instantiate Player
        _player = new Player(_playerTextures, new Vector2(100, 100));
        
        // Load Font and Create HUD
        _font = Content.Load<SpriteFont>("Fonts/GameFont");
        _hud = new HUD(_font);
        
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
        
        if (_gameState == GameState.MainMenu)
        {
            _spriteBatch.DrawString(_font, "DUNGEON OF ALGORITHMS", new Vector2(200, 150), Color.Gold);
            _spriteBatch.DrawString(_font, "Press ENTER to Start", new Vector2(220, 200), Color.White);
            _spriteBatch.DrawString(_font, "WASD to Move | F5 Save | F9 Load", new Vector2(180, 250), Color.Gray);
        }
        else if (_gameState == GameState.GameOver)
        {
            _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(280, 180), Color.Red);
            _spriteBatch.DrawString(_font, $"Final Score: {_player.Score}", new Vector2(260, 220), Color.White);
            _spriteBatch.DrawString(_font, "Press R to Restart", new Vector2(250, 260), Color.Gray);
        }
        else if (_gameState == GameState.Victory)
        {
            _spriteBatch.DrawString(_font, "VICTORY!", new Vector2(300, 150), Color.Gold);
            _spriteBatch.DrawString(_font, "You found the Treasure!", new Vector2(200, 200), Color.White);
            _spriteBatch.DrawString(_font, $"Final Score: {_player.Score}", new Vector2(260, 250), Color.White);
            _spriteBatch.DrawString(_font, "Press R to Reset", new Vector2(260, 300), Color.Gray);
        }
        else
        {
            _hud.Draw(_spriteBatch, _player);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
