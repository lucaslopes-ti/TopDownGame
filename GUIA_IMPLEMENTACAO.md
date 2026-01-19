# GUIA DE IMPLEMENTAÇÃO PASSO A PASSO
## Dungeon of Algorithms - Roteiro para o Professor

---

## 📋 VISÃO GERAL

Este guia fornece um roteiro detalhado para implementar o jogo "Dungeon of Algorithms" junto com os alunos, seguindo a ordem das aulas e aplicando os conceitos aprendidos.

---

## FASE 1: SETUP INICIAL (Aula 1)

### Passo 1.1: Criar Projeto MonoGame

```bash
# Instalar template MonoGame (se necessário)
dotnet new --install MonoGame.Templates.CSharp

# Criar novo projeto
dotnet new mgdesktopgl -n DungeonOfAlgorithms
cd DungeonOfAlgorithms
```

### Passo 1.2: Estrutura de Pastas

Criar a seguinte estrutura:
```
DungeonOfAlgorithms/
├── Source/
│   ├── Core/
│   │   ├── Game1.cs
│   │   ├── GameManager.cs
│   │   └── InputManager.cs
│   └── Entities/
│       └── Player.cs
├── Content/
└── Program.cs
```

### Passo 1.3: Implementar GameManager (Singleton)

**Arquivo:** `Source/Core/GameManager.cs`

```csharp
namespace DungeonOfAlgorithms.Source.Core;

public class GameManager
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ??= new GameManager();
    
    private GameManager() { }
    
    public void Initialize()
    {
        // Inicialização futura
    }
}
```

**Explicação para os alunos:**
- Por que usar Singleton? Para ter acesso global sem criar múltiplas instâncias
- Como funciona? Construtor privado + propriedade estática

---

## FASE 2: INPUT E MOVIMENTO (Aula 2)

### Passo 2.1: InputManager (Singleton + Adapter Pattern)

**Arquivo:** `Source/Core/InputManager.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonOfAlgorithms.Source.Core;

public class InputManager
{
    private static InputManager _instance;
    public static InputManager Instance => _instance ??= new InputManager();
    
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    
    private InputManager() { }
    
    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
    }
    
    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && 
               _previousKeyboardState.IsKeyUp(key);
    }
    
    public bool IsKeyDown(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key);
    }
    
    public Vector2 GetMovementDirection()
    {
        Vector2 direction = Vector2.Zero;
        
        if (IsKeyDown(Keys.W) || IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (IsKeyDown(Keys.S) || IsKeyDown(Keys.Down))
            direction.Y += 1;
        if (IsKeyDown(Keys.A) || IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (IsKeyDown(Keys.D) || IsKeyDown(Keys.Right))
            direction.X += 1;
            
        if (direction.Length() > 0)
            direction.Normalize();
            
        return direction;
    }
}
```

**Explicação:**
- Adapter Pattern: Adapta entrada do teclado para direção vetorial
- Normalização: Garante movimento consistente em todas as direções

### Passo 2.2: Interface IGameEntity

**Arquivo:** `Source/Core/IGameEntity.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonOfAlgorithms.Source.Core;

public interface IGameEntity
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
```

**Explicação:**
- Interface garante que todas as entidades tenham Update e Draw
- Facilita gerenciamento de múltiplas entidades

### Passo 2.3: Player Básico

**Arquivo:** `Source/Entities/Player.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class Player : IGameEntity
{
    public Vector2 Position { get; set; }
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
    
    private float speed = 100f;
    private Texture2D texture;
    
    public Player(Texture2D texture, Vector2 startPosition)
    {
        this.texture = texture;
        Position = startPosition;
    }
    
    public void Update(GameTime gameTime)
    {
        Vector2 direction = InputManager.Instance.GetMovementDirection();
        Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, Color.White);
    }
}
```

---

## FASE 3: TILEMAP (Aula 1 - Arrays 2D)

### Passo 3.1: Classe Tilemap

**Arquivo:** `Source/Core/Tilemap.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DungeonOfAlgorithms.Source.Core;

public class Tilemap
{
    private int[,] mapData; // Array 2D
    private Texture2D tileset;
    private int tileWidth;
    private int tileHeight;
    private int tilesPerRow;
    
    public int MapWidth => mapData.GetLength(1) * tileWidth;
    public int MapHeight => mapData.GetLength(0) * tileHeight;
    
    public Tilemap(Texture2D tileset, int[,] mapData, int tileWidth, int tileHeight)
    {
        this.tileset = tileset;
        this.mapData = mapData;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
        this.tilesPerRow = tileset.Width / tileWidth;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                int tileIndex = mapData[y, x];
                
                if (tileIndex > 0) // 0 = tile vazio
                {
                    // Calcular posição no tileset
                    int tilesetX = (tileIndex - 1) % tilesPerRow;
                    int tilesetY = (tileIndex - 1) / tilesPerRow;
                    
                    Rectangle sourceRect = new Rectangle(
                        tilesetX * tileWidth,
                        tilesetY * tileHeight,
                        tileWidth,
                        tileHeight
                    );
                    
                    Rectangle destRect = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight
                    );
                    
                    spriteBatch.Draw(tileset, destRect, sourceRect, Color.White);
                }
            }
        }
    }
    
    public int GetTileAt(int worldX, int worldY)
    {
        int tileX = worldX / tileWidth;
        int tileY = worldY / tileHeight;
        
        if (tileY >= 0 && tileY < mapData.GetLength(0) &&
            tileX >= 0 && tileX < mapData.GetLength(1))
        {
            return mapData[tileY, tileX];
        }
        
        return -1; // Fora dos limites
    }
}
```

**Explicação:**
- Array 2D armazena índices de tiles
- Cada índice corresponde a uma posição no tileset
- Cálculo de posição no tileset usando módulo e divisão

### Passo 3.2: Carregar Mapa de CSV

**Arquivo:** `Source/Core/DungeonManager.cs` (parcial)

```csharp
public int[,] LoadMapFromCSV(string filePath)
{
    if (!File.Exists(filePath))
    {
        string altPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
        if (File.Exists(altPath)) filePath = altPath;
    }
    
    string[] lines = File.ReadAllLines(filePath);
    int height = lines.Length;
    int width = lines[0].Split(',').Length;
    
    int[,] mapData = new int[height, width];
    
    for (int y = 0; y < height; y++)
    {
        string[] values = lines[y].Split(',');
        for (int x = 0; x < width; x++)
        {
            if (int.TryParse(values[x], out int tileIndex))
            {
                mapData[y, x] = tileIndex < 0 ? 0 : tileIndex;
            }
        }
    }
    
    return mapData;
}
```

---

## FASE 4: SISTEMA DE SALAS (Aula 4 - Grafos)

### Passo 4.1: Classe Room

**Arquivo:** `Source/Core/Room.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public class Room
{
    public int Id { get; private set; }
    public Tilemap Tilemap { get; private set; }
    public Dictionary<string, int> Connections { get; private set; } = new Dictionary<string, int>();
    
    public List<Item> Items { get; private set; } = new List<Item>();
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    
    public Room(int id, Tilemap tilemap)
    {
        Id = id;
        Tilemap = tilemap;
    }
    
    public void Connect(string direction, int roomId)
    {
        if (!Connections.ContainsKey(direction))
            Connections.Add(direction, roomId);
    }
    
    public void AddItem(Item item)
    {
        Items.Add(item);
    }
    
    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }
    
    public void Update(GameTime gameTime, Player player, System.Action<Item> onItemCollected = null)
    {
        // Atualizar itens
        foreach (var item in Items.ToList())
        {
            if (item.IsActive && player.Bounds.Intersects(item.Bounds))
            {
                item.IsActive = false;
                player.AddScore(item.Value);
                onItemCollected?.Invoke(item);
                Items.Remove(item);
            }
        }
        
        // Atualizar inimigos
        foreach (var enemy in Enemies)
        {
            enemy.Update(gameTime, player, Tilemap);
            
            if (player.Bounds.Intersects(enemy.Bounds))
            {
                player.TakeDamage(enemy.Damage);
            }
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);
        
        foreach (var item in Items)
        {
            if (item.IsActive)
                item.Draw(spriteBatch);
        }
        
        foreach (var enemy in Enemies)
        {
            enemy.Draw(spriteBatch);
        }
    }
}
```

**Explicação:**
- Dictionary para conexões (grafo)
- List para itens e inimigos
- Métodos para adicionar e atualizar entidades

### Passo 4.2: DungeonManager (Singleton)

**Arquivo:** `Source/Core/DungeonManager.cs`

```csharp
using System.Collections.Generic;

namespace DungeonOfAlgorithms.Source.Core;

public class DungeonManager
{
    private static DungeonManager _instance;
    public static DungeonManager Instance => _instance ??= new DungeonManager();
    
    public Dictionary<int, Room> Rooms { get; private set; } = new Dictionary<int, Room>();
    public Room CurrentRoom { get; private set; }
    
    private DungeonManager() { }
    
    public void AddRoom(Room room)
    {
        if (!Rooms.ContainsKey(room.Id))
            Rooms.Add(room.Id, room);
    }
    
    public void ChangeRoom(int roomId)
    {
        if (Rooms.ContainsKey(roomId))
            CurrentRoom = Rooms[roomId];
    }
    
    // ... LoadMapFromCSV (já mostrado anteriormente)
}
```

---

## FASE 5: FACTORY PATTERN (Aula 12)

### Passo 5.1: EnemyFactory

**Arquivo:** `Source/Core/EnemyFactory.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public static class EnemyFactory
{
    private static ContentManager _content;
    private static Dictionary<string, Dictionary<string, Texture2D>> _enemyTextures = new();
    private static int _nextId = 0;
    
    public static void Initialize(ContentManager content)
    {
        _content = content;
    }
    
    public static Enemy CreateEnemy(string type, Vector2 position)
    {
        if (!_enemyTextures.ContainsKey(type))
        {
            _enemyTextures[type] = LoadEnemyTextures(type);
        }
        
        IEnemyBehavior behavior = type switch
        {
            "Ghost" => new ChaseBehavior(),
            "Slime" => new PatrolBehavior(position, position + new Vector2(100, 0)),
            _ => new PatrolBehavior(position, position + new Vector2(50, 50))
        };
        
        return new Enemy(_nextId++, _enemyTextures[type], position, behavior);
    }
    
    private static Dictionary<string, Texture2D> LoadEnemyTextures(string type)
    {
        var dict = new Dictionary<string, Texture2D>();
        string[] keys = { "Down", "Up", "Side", "Down_Idle", "Up_Idle", "Side_Idle" };
        string[] folderNames = { "D_Walk", "U_Walk", "S_Walk", "D_Idle", "U_Idle", "S_Idle" };
        
        for (int i = 0; i < keys.Length; i++)
        {
            string path = $"Enemies/{type}/{folderNames[i]}";
            try
            {
                dict[keys[i]] = _content.Load<Texture2D>(path);
            }
            catch
            {
                Console.WriteLine($"Missing asset: {path}");
            }
        }
        
        return dict;
    }
}
```

### Passo 5.2: ItemFactory

**Arquivo:** `Source/Core/ItemFactory.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public static class ItemFactory
{
    private static ContentManager _content;
    private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
    
    public static void Initialize(ContentManager content)
    {
        _content = content;
    }
    
    public static Item CreateItem(string type, Vector2 position)
    {
        if (!_textures.ContainsKey(type))
        {
            try
            {
                _textures[type] = _content.Load<Texture2D>($"Items/{type}");
            }
            catch
            {
                Console.WriteLine($"Texture não encontrada: Items/{type}");
                return null;
            }
        }
        
        return type switch
        {
            "Coin" => new Item(1, "Gold Coin", _textures[type], position),
            "Chest" => new ChestItem(_textures[type], position),
            _ => null
        };
    }
}
```

---

## FASE 6: STRATEGY PATTERN (Aula 13)

### Passo 6.1: Interface IEnemyBehavior

**Arquivo:** `Source/Core/IEnemyBehavior.cs`

```csharp
using Microsoft.Xna.Framework;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public interface IEnemyBehavior
{
    void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap);
}
```

### Passo 6.2: Implementações de Comportamento

**Arquivo:** `Source/Core/EnemyBehaviors.cs`

```csharp
using Microsoft.Xna.Framework;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public class PatrolBehavior : IEnemyBehavior
{
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 currentTarget;
    private float speed = 30f;
    
    public PatrolBehavior(Vector2 start, Vector2 end)
    {
        startPosition = start;
        endPosition = end;
        currentTarget = end;
    }
    
    public void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap)
    {
        Vector2 direction = currentTarget - enemy.Position;
        
        if (direction.Length() < 5f)
        {
            currentTarget = currentTarget == startPosition ? endPosition : startPosition;
            direction = currentTarget - enemy.Position;
        }
        
        direction.Normalize();
        enemy.Move(direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}

public class ChaseBehavior : IEnemyBehavior
{
    private float speed = 50f;
    private float detectionRange = 150f;
    
    public void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap)
    {
        Vector2 toPlayer = player.Position - enemy.Position;
        float distance = toPlayer.Length();
        
        if (distance < detectionRange)
        {
            toPlayer.Normalize();
            enemy.Move(toPlayer * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
```

### Passo 6.3: Classe Enemy

**Arquivo:** `Source/Entities/Enemy.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class Enemy : IGameEntity
{
    public int Id { get; private set; }
    public Vector2 Position { get; set; }
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
    public int Damage { get; set; } = 10;
    
    private Dictionary<string, Texture2D> textures;
    private IEnemyBehavior behavior;
    private string currentAnimation = "Down_Idle";
    
    public Enemy(int id, Dictionary<string, Texture2D> textures, Vector2 position, IEnemyBehavior behavior)
    {
        Id = id;
        this.textures = textures;
        Position = position;
        this.behavior = behavior;
    }
    
    public void Move(Vector2 movement)
    {
        Position += movement;
    }
    
    public void Update(GameTime gameTime, Player player, Tilemap tilemap)
    {
        behavior?.Update(this, gameTime, player, tilemap);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (textures.ContainsKey(currentAnimation))
        {
            spriteBatch.Draw(textures[currentAnimation], Position, Color.White);
        }
    }
}
```

---

## FASE 7: BANCO DE DADOS (Aula 16-17)

### Passo 7.1: Adicionar Pacote SQLite

**Arquivo:** `DungeonOfAlgorithms.csproj`

```xml
<ItemGroup>
  <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.1.303" />
  <PackageReference Include="MonoGame.ContentPipeline.DesktopGL" Version="3.8.1.303" />
  <PackageReference Include="Microsoft.Data.Sqlite" Version="7.0.0" />
</ItemGroup>
```

### Passo 7.2: DatabaseManager (Singleton)

**Arquivo:** `Source/Core/DatabaseManager.cs`

```csharp
using System;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.Xna.Framework;

namespace DungeonOfAlgorithms.Source.Core;

public class DatabaseManager
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance ??= new DatabaseManager();
    
    private string _connectionString;
    
    private DatabaseManager()
    {
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "game.db");
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }
    
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        string sql = @"
            CREATE TABLE IF NOT EXISTS SaveSlots (
                Id INTEGER PRIMARY KEY,
                Level INTEGER,
                PlayerX REAL,
                PlayerY REAL,
                Score INTEGER
            )";
        
        using var command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();
    }
    
    public void SaveGame(int level, Vector2 position, int score)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        string sql = @"
            INSERT OR REPLACE INTO SaveSlots 
            (Id, Level, PlayerX, PlayerY, Score) 
            VALUES (1, @Level, @X, @Y, @Score)";
        
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Level", level);
        command.Parameters.AddWithValue("@X", position.X);
        command.Parameters.AddWithValue("@Y", position.Y);
        command.Parameters.AddWithValue("@Score", score);
        
        command.ExecuteNonQuery();
    }
    
    public (int Level, Vector2 Position, int Score)? LoadGame()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        string sql = "SELECT Level, PlayerX, PlayerY, Score FROM SaveSlots WHERE Id = 1";
        using var command = new SqliteCommand(sql, connection);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            int level = reader.GetInt32(0);
            float x = reader.GetFloat(1);
            float y = reader.GetFloat(2);
            int score = reader.GetInt32(3);
            return (level, new Vector2(x, y), score);
        }
        
        return null;
    }
}
```

### Passo 7.3: Integrar Save/Load no Game1

**No método Update do Game1.cs:**

```csharp
// Salvar jogo (F5)
if (Keyboard.GetState().IsKeyDown(Keys.F5) && _lastKeyboardState.IsKeyUp(Keys.F5))
{
    DatabaseManager.Instance.SaveGame(
        DungeonManager.Instance.CurrentRoom.Id,
        _player.Position,
        _player.Score
    );
    Window.Title = "Game Saved!";
}

// Carregar jogo (F9)
if (Keyboard.GetState().IsKeyDown(Keys.F9) && _lastKeyboardState.IsKeyUp(Keys.F9))
{
    var data = DatabaseManager.Instance.LoadGame();
    if (data != null)
    {
        DungeonManager.Instance.ChangeRoom(data.Value.Level);
        _player.SetPosition(data.Value.Position);
        _player.Score = data.Value.Score;
        Window.Title = "Game Loaded!";
    }
}
```

---

## FASE 8: INTEGRAÇÃO FINAL

### Passo 8.1: Game1.cs Completo

Integrar todos os sistemas no Game1.cs:
- Inicializar managers
- Carregar salas e conectar
- Adicionar itens e inimigos usando factories
- Sistema de transição entre salas
- Save/Load
- HUD básico

### Passo 8.2: Testes

Testar cada funcionalidade:
- [ ] Movimento do player
- [ ] Transição entre salas
- [ ] Coleta de itens
- [ ] Inimigos com diferentes comportamentos
- [ ] Save/Load funcionando
- [ ] Colisões básicas

---

## DICAS PARA O PROFESSOR

1. **Implemente Incrementalmente:** Não tente fazer tudo de uma vez
2. **Teste Frequentemente:** Teste cada funcionalidade antes de avançar
3. **Debug em Conjunto:** Mostre como usar o debugger
4. **Code Review:** Revise código dos alunos
5. **Refatoração:** Mostre como melhorar código existente
6. **Documentação:** Incentive comentários

---

## CHECKLIST FINAL

Antes de considerar o projeto completo:
- [ ] Todas as estruturas de dados implementadas
- [ ] Todos os design patterns aplicados
- [ ] Banco de dados funcionando
- [ ] Código organizado e comentado
- [ ] Sem erros de compilação
- [ ] Jogo jogável do início ao fim
