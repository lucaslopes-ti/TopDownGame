# EXERCÍCIOS PRÁTICOS - CODIFICAÇÃO DE JOGOS DIGITAIS
## Material de Apoio para o Professor

---

## MÓDULO 1: ESTRUTURA DE DADOS

### AULA 1 - Arrays

#### Exercício 1.1: Sistema de Pontuação
**Objetivo:** Criar um sistema que armazena e gerencia as 10 melhores pontuações usando array.

**Especificações:**
- Array de tamanho fixo (10 elementos)
- Manter pontuações ordenadas (maior para menor)
- Adicionar nova pontuação apenas se for maior que a menor do top 10
- Método para obter a melhor pontuação
- Método para listar todas as pontuações

**Solução Sugerida:**
```csharp
public class ScoreManager
{
    private int[] topScores = new int[10];
    private int count = 0;
    
    public void AddScore(int score)
    {
        if (count < 10)
        {
            topScores[count] = score;
            count++;
            SortScores();
        }
        else if (score > topScores[9])
        {
            topScores[9] = score;
            SortScores();
        }
    }
    
    private void SortScores()
    {
        // Bubble sort simples (ou usar Array.Sort)
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (topScores[j] < topScores[j + 1])
                {
                    int temp = topScores[j];
                    topScores[j] = topScores[j + 1];
                    topScores[j + 1] = temp;
                }
            }
        }
    }
    
    public int GetBestScore()
    {
        return count > 0 ? topScores[0] : 0;
    }
    
    public int[] GetAllScores()
    {
        int[] result = new int[count];
        Array.Copy(topScores, result, count);
        return result;
    }
}
```

**Variações para Dificuldade:**
- Fácil: Apenas adicionar e listar
- Médio: Ordenar automaticamente
- Difícil: Persistir em arquivo

---

#### Exercício 1.2: Grid de Jogo
**Objetivo:** Criar um tabuleiro 8x8 usando array bidimensional.

**Especificações:**
- Grid 8x8 (valores: 0 = vazio, 1 = peça do jogador, 2 = peça do inimigo)
- Método para colocar peça em posição (x, y)
- Método para verificar se posição está vazia
- Método para contar peças de cada tipo
- Método para limpar tabuleiro

**Solução Sugerida:**
```csharp
public class GameBoard
{
    private int[,] board = new int[8, 8];
    
    public void Initialize()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                board[y, x] = 0; // Vazio
            }
        }
    }
    
    public bool PlacePiece(int x, int y, int pieceType)
    {
        if (IsValidPosition(x, y) && IsEmpty(x, y))
        {
            board[y, x] = pieceType;
            return true;
        }
        return false;
    }
    
    public bool IsEmpty(int x, int y)
    {
        return IsValidPosition(x, y) && board[y, x] == 0;
    }
    
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }
    
    public int CountPieces(int pieceType)
    {
        int count = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board[y, x] == pieceType)
                    count++;
            }
        }
        return count;
    }
    
    public void Clear()
    {
        Initialize();
    }
}
```

---

### AULA 2 - Listas e Coleções

#### Exercício 2.1: Sistema de Inventário
**Objetivo:** Criar sistema de inventário usando List<T>.

**Especificações:**
- Classe Item com propriedades: Name, Quantity, Value
- Lista para armazenar itens
- Adicionar item (se já existe, aumenta quantidade)
- Remover item
- Buscar item por nome
- Calcular valor total do inventário
- Limpar inventário

**Solução Sugerida:**
```csharp
public class Item
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Value { get; set; }
    
    public Item(string name, int quantity, int value)
    {
        Name = name;
        Quantity = quantity;
        Value = value;
    }
}

public class Inventory
{
    private List<Item> items = new List<Item>();
    
    public void AddItem(string name, int quantity, int value)
    {
        Item existingItem = items.FirstOrDefault(i => i.Name == name);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            items.Add(new Item(name, quantity, value));
        }
    }
    
    public bool RemoveItem(string name, int quantity = 1)
    {
        Item item = items.FirstOrDefault(i => i.Name == name);
        
        if (item != null)
        {
            item.Quantity -= quantity;
            if (item.Quantity <= 0)
            {
                items.Remove(item);
            }
            return true;
        }
        return false;
    }
    
    public Item FindItem(string name)
    {
        return items.FirstOrDefault(i => i.Name == name);
    }
    
    public int GetTotalValue()
    {
        return items.Sum(i => i.Quantity * i.Value);
    }
    
    public void Clear()
    {
        items.Clear();
    }
    
    public List<Item> GetAllItems()
    {
        return new List<Item>(items);
    }
}
```

---

#### Exercício 2.2: Sistema de Stats
**Objetivo:** Criar sistema de estatísticas usando Dictionary.

**Especificações:**
- Dictionary<string, int> para armazenar stats
- Adicionar/modificar stat
- Obter valor de stat
- Aumentar stat (adicionar valor)
- Reduzir stat (subtrair valor)
- Listar todos os stats

**Solução Sugerida:**
```csharp
public class PlayerStats
{
    private Dictionary<string, int> stats = new Dictionary<string, int>();
    
    public void SetStat(string statName, int value)
    {
        stats[statName] = value;
    }
    
    public int GetStat(string statName)
    {
        return stats.ContainsKey(statName) ? stats[statName] : 0;
    }
    
    public void IncreaseStat(string statName, int amount)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName] += amount;
        }
        else
        {
            stats[statName] = amount;
        }
    }
    
    public void DecreaseStat(string statName, int amount)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName] = Math.Max(0, stats[statName] - amount);
        }
    }
    
    public Dictionary<string, int> GetAllStats()
    {
        return new Dictionary<string, int>(stats);
    }
}
```

---

### AULA 3 - Pilhas e Filas

#### Exercício 3.1: Sistema de Undo/Redo
**Objetivo:** Implementar sistema de desfazer/refazer usando Stack.

**Especificações:**
- Classe GameState para armazenar estado
- Stack para undo
- Stack para redo
- Salvar estado atual
- Desfazer (undo)
- Refazer (redo)
- Limpar histórico

**Solução Sugerida:**
```csharp
public class GameState
{
    public int Score { get; set; }
    public int Level { get; set; }
    public Vector2 PlayerPosition { get; set; }
    
    public GameState(int score, int level, Vector2 position)
    {
        Score = score;
        Level = level;
        PlayerPosition = position;
    }
    
    public GameState Clone()
    {
        return new GameState(Score, Level, PlayerPosition);
    }
}

public class UndoRedoSystem
{
    private Stack<GameState> undoStack = new Stack<GameState>();
    private Stack<GameState> redoStack = new Stack<GameState>();
    private GameState currentState;
    
    public void SaveState(GameState state)
    {
        if (currentState != null)
        {
            undoStack.Push(currentState.Clone());
        }
        currentState = state.Clone();
        redoStack.Clear(); // Limpar redo quando nova ação é feita
    }
    
    public bool Undo()
    {
        if (undoStack.Count == 0)
            return false;
            
        if (currentState != null)
        {
            redoStack.Push(currentState.Clone());
        }
        
        currentState = undoStack.Pop();
        return true;
    }
    
    public bool Redo()
    {
        if (redoStack.Count == 0)
            return false;
            
        if (currentState != null)
        {
            undoStack.Push(currentState.Clone());
        }
        
        currentState = redoStack.Pop();
        return true;
    }
    
    public GameState GetCurrentState()
    {
        return currentState;
    }
    
    public void Clear()
    {
        undoStack.Clear();
        redoStack.Clear();
        currentState = null;
    }
}
```

---

#### Exercício 3.2: Sistema de Mensagens
**Objetivo:** Criar sistema de fila de mensagens usando Queue.

**Especificações:**
- Queue para armazenar mensagens
- Adicionar mensagem
- Obter próxima mensagem
- Verificar se há mensagens
- Limpar fila
- Limitar tamanho máximo da fila

**Solução Sugerida:**
```csharp
public class MessageSystem
{
    private Queue<string> messages = new Queue<string>();
    private int maxMessages = 10;
    
    public void AddMessage(string message)
    {
        messages.Enqueue(message);
        
        // Limitar tamanho
        while (messages.Count > maxMessages)
        {
            messages.Dequeue();
        }
    }
    
    public string GetNextMessage()
    {
        if (messages.Count > 0)
        {
            return messages.Dequeue();
        }
        return null;
    }
    
    public bool HasMessages()
    {
        return messages.Count > 0;
    }
    
    public void Clear()
    {
        messages.Clear();
    }
    
    public int Count()
    {
        return messages.Count;
    }
}
```

---

### AULA 4 - Grafos

#### Exercício 4.1: Grafo de Salas
**Objetivo:** Implementar grafo para representar conexões entre salas.

**Especificações:**
- Classe Room com ID e conexões
- Dictionary para armazenar salas
- Conectar duas salas (direção)
- Verificar se salas estão conectadas
- Obter salas adjacentes
- Encontrar caminho entre duas salas (BFS)

**Solução Sugerida:**
```csharp
public class Room
{
    public int Id { get; private set; }
    public Dictionary<string, int> Connections { get; private set; } = new Dictionary<string, int>();
    
    public Room(int id)
    {
        Id = id;
    }
    
    public void Connect(string direction, int roomId)
    {
        if (!Connections.ContainsKey(direction))
        {
            Connections.Add(direction, roomId);
        }
    }
    
    public bool HasConnection(string direction)
    {
        return Connections.ContainsKey(direction);
    }
    
    public int GetConnectedRoom(string direction)
    {
        return Connections.ContainsKey(direction) ? Connections[direction] : -1;
    }
}

public class RoomGraph
{
    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
    
    public void AddRoom(Room room)
    {
        rooms[room.Id] = room;
    }
    
    public void ConnectRooms(int room1Id, string direction, int room2Id)
    {
        if (rooms.ContainsKey(room1Id) && rooms.ContainsKey(room2Id))
        {
            rooms[room1Id].Connect(direction, room2Id);
        }
    }
    
    public bool AreConnected(int room1Id, int room2Id)
    {
        if (!rooms.ContainsKey(room1Id))
            return false;
            
        return rooms[room1Id].Connections.Values.Contains(room2Id);
    }
    
    public List<int> GetNeighbors(int roomId)
    {
        if (!rooms.ContainsKey(roomId))
            return new List<int>();
            
        return new List<int>(rooms[roomId].Connections.Values);
    }
    
    public List<int> FindPath(int start, int target)
    {
        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();
        Dictionary<int, int> parent = new Dictionary<int, int>();
        
        queue.Enqueue(start);
        visited.Add(start);
        
        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            
            if (current == target)
            {
                return ReconstructPath(parent, start, target);
            }
            
            foreach (int neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    parent[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        return null; // Caminho não encontrado
    }
    
    private List<int> ReconstructPath(Dictionary<int, int> parent, int start, int target)
    {
        List<int> path = new List<int>();
        int current = target;
        
        while (current != start)
        {
            path.Add(current);
            current = parent[current];
        }
        path.Add(start);
        path.Reverse();
        
        return path;
    }
}
```

---

## MÓDULO 2: DESIGN PATTERNS

### AULA 11 - Singleton

#### Exercício 11.1: AudioManager Singleton
**Objetivo:** Criar AudioManager usando padrão Singleton.

**Solução Sugerida:**
```csharp
public class AudioManager
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance ??= new AudioManager();
    
    private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
    private float volume = 1.0f;
    
    private AudioManager() { }
    
    public void Initialize(ContentManager content)
    {
        // Carregar sons
        sounds["Jump"] = content.Load<SoundEffect>("Sounds/Jump");
        sounds["Coin"] = content.Load<SoundEffect>("Sounds/Coin");
    }
    
    public void PlaySound(string soundName)
    {
        if (sounds.ContainsKey(soundName))
        {
            sounds[soundName].Play(volume, 0.0f, 0.0f);
        }
    }
    
    public void SetVolume(float newVolume)
    {
        volume = MathHelper.Clamp(newVolume, 0.0f, 1.0f);
    }
}
```

---

### AULA 12 - Factory

#### Exercício 12.1: ItemFactory
**Solução Sugerida:**
```csharp
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
            "Coin" => new Coin(_textures[type], position, 10),
            "Potion" => new Potion(_textures[type], position, 50),
            "Key" => new Key(_textures[type], position),
            "Chest" => new Chest(_textures[type], position),
            _ => null
        };
    }
}
```

---

### AULA 13 - Strategy

#### Exercício 13.1: Comportamentos de Inimigo
**Solução Sugerida:**
```csharp
public interface IEnemyBehavior
{
    void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap);
}

public class IdleBehavior : IEnemyBehavior
{
    public void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap)
    {
        // Inimigo fica parado
    }
}

public class PatrolBehavior : IEnemyBehavior
{
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 currentTarget;
    private float speed = 30f;
    
    public PatrolBehavior(Vector2 start, Vector2 end)
    {
        startPos = start;
        endPos = end;
        currentTarget = end;
    }
    
    public void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap)
    {
        Vector2 direction = currentTarget - enemy.Position;
        
        if (direction.Length() < 5f)
        {
            currentTarget = currentTarget == startPos ? endPos : startPos;
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

public class FleeBehavior : IEnemyBehavior
{
    private float speed = 40f;
    private float fleeRange = 100f;
    
    public void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap)
    {
        Vector2 fromPlayer = enemy.Position - player.Position;
        float distance = fromPlayer.Length();
        
        if (distance < fleeRange)
        {
            fromPlayer.Normalize();
            enemy.Move(fromPlayer * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
```

---

## MÓDULO 3: BANCO DE DADOS

### AULA 16-17 - CRUD

#### Exercício: Sistema de Save/Load Completo
**Solução Sugerida:**
```csharp
public class DatabaseManager
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance ??= new DatabaseManager();
    
    private string connectionString;
    
    private DatabaseManager()
    {
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "game.db");
        connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }
    
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = @"
            CREATE TABLE IF NOT EXISTS SaveSlots (
                Id INTEGER PRIMARY KEY,
                Level INTEGER NOT NULL,
                PlayerX REAL NOT NULL,
                PlayerY REAL NOT NULL,
                Score INTEGER NOT NULL,
                Timestamp TEXT NOT NULL
            )";
            
        using var command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();
    }
    
    public void SaveGame(int slotId, int level, Vector2 position, int score)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = @"
            INSERT OR REPLACE INTO SaveSlots 
            (Id, Level, PlayerX, PlayerY, Score, Timestamp) 
            VALUES (@Id, @Level, @X, @Y, @Score, @Timestamp)";
            
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", slotId);
        command.Parameters.AddWithValue("@Level", level);
        command.Parameters.AddWithValue("@X", position.X);
        command.Parameters.AddWithValue("@Y", position.Y);
        command.Parameters.AddWithValue("@Score", score);
        command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString());
        
        command.ExecuteNonQuery();
    }
    
    public (int Level, Vector2 Position, int Score)? LoadGame(int slotId)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = "SELECT Level, PlayerX, PlayerY, Score FROM SaveSlots WHERE Id = @Id";
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", slotId);
        
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
    
    public void DeleteSave(int slotId)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = "DELETE FROM SaveSlots WHERE Id = @Id";
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", slotId);
        
        command.ExecuteNonQuery();
    }
    
    public List<(int Id, int Level, int Score, string Timestamp)> ListAllSaves()
    {
        List<(int, int, int, string)> saves = new List<(int, int, int, string)>();
        
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = "SELECT Id, Level, Score, Timestamp FROM SaveSlots ORDER BY Score DESC";
        using var command = new SqliteCommand(sql, connection);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            saves.Add((
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetString(3)
            ));
        }
        
        return saves;
    }
    
    public int GetBestScore()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        string sql = "SELECT MAX(Score) FROM SaveSlots";
        using var command = new SqliteCommand(sql, connection);
        
        object result = command.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
    }
}
```

---

# DICAS PARA O PROFESSOR

1. **Adapte a Dificuldade:** Ajuste os exercícios conforme o nível da turma
2. **Code Review:** Revise o código dos alunos em conjunto
3. **Debugging:** Ensine a usar o debugger do Visual Studio
4. **Testes:** Peça para testarem cada funcionalidade isoladamente
5. **Documentação:** Incentive comentários no código
6. **Pair Programming:** Em alguns exercícios, faça em duplas

---

# CHECKLIST DE AVALIAÇÃO

Para cada exercício, verifique:
- [ ] Código compila sem erros
- [ ] Funcionalidade implementada corretamente
- [ ] Código organizado e legível
- [ ] Comentários explicativos quando necessário
- [ ] Tratamento de erros básico
- [ ] Uso correto da estrutura de dados/pattern
