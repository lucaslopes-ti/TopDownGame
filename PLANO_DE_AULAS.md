# PLANO DE AULAS - CODIFICAÇÃO DE JOGOS DIGITAIS
## Curso Técnico SENAI
### Disciplina: Estrutura de Dados, Design Patterns e Banco de Dados em Jogos

---

## 📋 ESTRUTURA DO CURSO

**Duração Total:** 5 semanas (20 dias úteis)  
**Carga Horária:** 4 horas por dia = 80 horas totais  
**Projeto Final:** Jogo "Dungeon of Algorithms" (Top-Down Roguelike)

---

## 🎯 OBJETIVOS GERAIS

Ao final do curso, o aluno será capaz de:
- Implementar estruturas de dados eficientes em jogos digitais
- Aplicar padrões de design (Design Patterns) para organizar código de jogos
- Integrar banco de dados para persistência de dados em jogos
- Desenvolver um jogo completo utilizando C# e MonoGame

---

# MÓDULO 1: ESTRUTURA DE DADOS (Semana 1-2)

## 📅 SEMANA 1: Fundamentos de Estruturas de Dados

### **AULA 1 - Introdução e Arrays (4h)**

#### **Objetivos:**
- Compreender a importância das estruturas de dados em jogos
- Dominar o uso de arrays unidimensionais e multidimensionais
- Implementar arrays no contexto de jogos

#### **Teoria (1h30min):**

**1.1 O que são Estruturas de Dados?**
- Definição: Forma de organizar e armazenar dados na memória
- Por que são importantes em jogos?
  - Performance: Acesso rápido a dados
  - Organização: Código mais limpo e manutenível
  - Escalabilidade: Suportar muitos objetos simultaneamente

**1.2 Arrays Unidimensionais**
```csharp
// Declaração
int[] scores = new int[10];
string[] playerNames = new string[4];

// Inicialização
int[] health = {100, 80, 50, 30};

// Acesso
health[0] = 100; // Primeiro elemento
int tamanho = health.Length; // Tamanho do array
```

**Características:**
- Tamanho fixo (definido na criação)
- Acesso direto por índice (O(1))
- Memória contígua
- Índice começa em 0

**1.3 Arrays Multidimensionais**
```csharp
// Matriz 2D (linha, coluna)
int[,] grid = new int[10, 10];

// Inicialização
int[,] map = {
    {1, 1, 1, 1},
    {1, 0, 0, 1},
    {1, 0, 0, 1},
    {1, 1, 1, 1}
};

// Acesso
int tile = map[2, 3]; // Linha 2, Coluna 3
map[1, 1] = 5; // Modificar valor
```

**Aplicações em Jogos:**
- Mapas/Tilemaps (grid de tiles)
- Matrizes de transformação
- Tabuleiros de jogos

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Sistema de Pontuação**
```csharp
// Crie um sistema que armazena as 10 melhores pontuações
class ScoreManager
{
    private int[] topScores = new int[10];
    
    // Implemente métodos para:
    // - Adicionar nova pontuação
    // - Ordenar pontuações (maior para menor)
    // - Retornar a melhor pontuação
}
```

**Exercício 2: Grid de Jogo**
```csharp
// Crie um grid 8x8 que representa um tabuleiro
class GameBoard
{
    private int[,] board = new int[8, 8];
    
    // Implemente:
    // - Inicializar tabuleiro vazio (0)
    // - Colocar peça em posição (x, y)
    // - Verificar se posição está vazia
    // - Contar peças no tabuleiro
}
```

**Exercício 3: Matriz de Transformação**
```csharp
// Crie uma matriz 3x3 para transformações 2D
class Transform2D
{
    private float[,] matrix = new float[3, 3];
    
    // Implemente:
    // - Matriz identidade
    // - Multiplicação de matrizes
    // - Aplicar transformação a um ponto (x, y)
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Criar Sistema de Tilemap**
- Criar classe `Tilemap` que usa array 2D para armazenar índices de tiles
- Carregar mapa de arquivo CSV
- Renderizar tiles na tela

```csharp
public class Tilemap
{
    private int[,] mapData; // Array 2D para armazenar índices
    private Texture2D tileset;
    private int tileWidth;
    private int tileHeight;
    
    public Tilemap(Texture2D tileset, int[,] mapData, int tileWidth, int tileHeight)
    {
        this.tileset = tileset;
        this.mapData = mapData;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Iterar pelo array e desenhar cada tile
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                int tileIndex = mapData[y, x];
                // Calcular posição na tela e no tileset
                // Desenhar tile
            }
        }
    }
}
```

---

### **AULA 2 - Listas e Coleções (4h)**

#### **Objetivos:**
- Dominar o uso de List<T> e suas operações
- Entender quando usar List vs Array
- Implementar coleções dinâmicas em jogos

#### **Teoria (1h30min):**

**2.1 List<T> - Listas Dinâmicas**
```csharp
// Declaração e inicialização
List<int> scores = new List<int>();
List<string> inventory = new List<string>();

// Adicionar elementos
scores.Add(100);
scores.Add(85);
scores.Insert(0, 120); // Insere na posição 0

// Acesso
int first = scores[0];
int count = scores.Count;

// Remover
scores.Remove(100); // Remove primeira ocorrência
scores.RemoveAt(0); // Remove por índice
scores.Clear(); // Remove todos

// Buscar
bool contains = scores.Contains(100);
int index = scores.IndexOf(100);
```

**Vantagens sobre Arrays:**
- Tamanho dinâmico (cresce conforme necessário)
- Métodos úteis (Add, Remove, Find, etc.)
- Mais flexível

**Desvantagens:**
- Pode ser mais lento para acesso direto
- Usa mais memória (overhead)

**2.2 Dictionary<TKey, TValue> - Dicionários**
```csharp
// Declaração
Dictionary<string, int> playerStats = new Dictionary<string, int>();

// Adicionar
playerStats["Health"] = 100;
playerStats["Mana"] = 50;
playerStats.Add("Stamina", 80);

// Acesso
int health = playerStats["Health"];

// Verificar existência
if (playerStats.ContainsKey("Health"))
{
    // Chave existe
}

// Iterar
foreach (var kvp in playerStats)
{
    string key = kvp.Key;
    int value = kvp.Value;
}
```

**Aplicações em Jogos:**
- Inventário (ID do item → Objeto)
- Sistema de stats (nome → valor)
- Cache de recursos (caminho → textura)

**2.3 HashSet<T> - Conjuntos**
```csharp
HashSet<string> collectedItems = new HashSet<string>();

collectedItems.Add("Coin");
collectedItems.Add("Key");
bool hasCoin = collectedItems.Contains("Coin"); // O(1) - muito rápido!
```

**Quando usar:**
- Verificar existência rapidamente
- Evitar duplicatas
- Operações de conjunto (união, interseção)

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Sistema de Inventário**
```csharp
class Inventory
{
    private List<Item> items = new List<Item>();
    
    // Implemente:
    // - Adicionar item
    // - Remover item por nome
    // - Buscar item por nome
    // - Listar todos os itens
    // - Limpar inventário
    // - Contar itens
}
```

**Exercício 2: Sistema de Stats**
```csharp
class PlayerStats
{
    private Dictionary<string, int> stats = new Dictionary<string, int>();
    
    // Implemente:
    // - Adicionar/modificar stat
    // - Obter valor de stat
    // - Aumentar stat (ex: +10 health)
    // - Reduzir stat
    // - Listar todos os stats
}
```

**Exercício 3: Sistema de Coleta**
```csharp
class CollectionSystem
{
    private HashSet<string> collectedItems = new HashSet<string>();
    
    // Implemente:
    // - Coletar item (adicionar ao conjunto)
    // - Verificar se item foi coletado
    // - Contar itens coletados
    // - Resetar coleção
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Sistema de Itens e Inimigos**
- Modificar classe `Room` para usar `List<Item>` e `List<Enemy>`
- Implementar coleta de itens
- Gerenciar lista de inimigos ativos

```csharp
public class Room
{
    public List<Item> Items { get; private set; } = new List<Item>();
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
    
    public void AddItem(Item item)
    {
        Items.Add(item);
    }
    
    public void Update(GameTime gameTime, Player player)
    {
        // Atualizar itens
        foreach (var item in Items.ToList()) // ToList() para evitar modificação durante iteração
        {
            if (item.IsActive && player.Bounds.Intersects(item.Bounds))
            {
                item.Collect();
                Items.Remove(item);
            }
        }
        
        // Atualizar inimigos
        foreach (var enemy in Enemies)
        {
            enemy.Update(gameTime, player);
        }
    }
}
```

---

### **AULA 3 - Pilhas e Filas (4h)**

#### **Objetivos:**
- Compreender estruturas LIFO (Stack) e FIFO (Queue)
- Aplicar pilhas e filas em contextos de jogos
- Implementar sistemas de undo/redo e comandos

#### **Teoria (1h30min):**

**3.1 Stack<T> - Pilha (LIFO: Last In, First Out)**
```csharp
Stack<string> commandHistory = new Stack<string>();

// Adicionar (Push)
commandHistory.Push("Move Right");
commandHistory.Push("Attack");
commandHistory.Push("Jump");

// Remover (Pop) - remove o último adicionado
string lastCommand = commandHistory.Pop(); // "Jump"

// Ver sem remover (Peek)
string top = commandHistory.Peek(); // "Attack"

// Verificar se está vazia
bool isEmpty = commandHistory.Count == 0;
```

**Aplicações em Jogos:**
- Sistema de undo/redo
- Histórico de ações
- Sistema de estados (menu → jogo → pause)
- Navegação de menus

**3.2 Queue<T> - Fila (FIFO: First In, First Out)**
```csharp
Queue<string> messageQueue = new Queue<string>();

// Adicionar (Enqueue)
messageQueue.Enqueue("Player joined");
messageQueue.Enqueue("Enemy defeated");
messageQueue.Enqueue("Level complete");

// Remover (Dequeue) - remove o primeiro adicionado
string first = messageQueue.Dequeue(); // "Player joined"

// Ver sem remover (Peek)
string next = messageQueue.Peek(); // "Enemy defeated"
```

**Aplicações em Jogos:**
- Sistema de mensagens/notificações
- Fila de comandos
- Sistema de eventos
- Processamento de tarefas em ordem

**3.3 Comparação Visual:**

**Pilha (Stack):**
```
    [3] ← Topo (último adicionado)
    [2]
    [1] ← Base (primeiro adicionado)
```

**Fila (Queue):**
```
    [1] → [2] → [3]
    ↑              ↑
  Front          Back
```

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Sistema de Undo/Redo**
```csharp
class UndoRedoSystem
{
    private Stack<GameState> undoStack = new Stack<GameState>();
    private Stack<GameState> redoStack = new Stack<GameState>();
    
    // Implemente:
    // - Salvar estado atual
    // - Desfazer ação (undo)
    // - Refazer ação (redo)
    // - Limpar histórico
}
```

**Exercício 2: Sistema de Mensagens**
```csharp
class MessageSystem
{
    private Queue<string> messages = new Queue<string>();
    
    // Implemente:
    // - Adicionar mensagem
    // - Mostrar próxima mensagem
    // - Verificar se há mensagens
    // - Limpar fila
}
```

**Exercício 3: Sistema de Estados de Jogo**
```csharp
class GameStateManager
{
    private Stack<GameState> stateStack = new Stack<GameState>();
    
    // Implemente:
    // - Empilhar novo estado (ex: abrir menu)
    // - Desempilhar estado (ex: fechar menu)
    // - Obter estado atual
    // - Limpar todos os estados
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Sistema de Estados**
- Implementar gerenciador de estados usando Stack
- Estados: MainMenu → Playing → Paused → GameOver

```csharp
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Victory
}

public class GameStateManager
{
    private Stack<GameState> stateStack = new Stack<GameState>();
    
    public void PushState(GameState state)
    {
        stateStack.Push(state);
    }
    
    public void PopState()
    {
        if (stateStack.Count > 0)
            stateStack.Pop();
    }
    
    public GameState CurrentState => stateStack.Count > 0 ? stateStack.Peek() : GameState.MainMenu;
}
```

---

### **AULA 4 - Grafos e Árvores (4h)**

#### **Objetivos:**
- Compreender conceitos de grafos e árvores
- Implementar grafos para representar conexões entre salas
- Aplicar busca em grafos (BFS/DFS)

#### **Teoria (1h30min):**

**4.1 Conceitos de Grafos**
- **Vértice (Node)**: Ponto no grafo
- **Aresta (Edge)**: Conexão entre vértices
- **Grafo Direcionado**: Arestas têm direção
- **Grafo Não-Direcionado**: Arestas bidirecionais

**4.2 Representação de Grafos**

**Lista de Adjacência:**
```csharp
class Graph
{
    private Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();
    
    public void AddVertex(int vertex)
    {
        if (!adjacencyList.ContainsKey(vertex))
            adjacencyList[vertex] = new List<int>();
    }
    
    public void AddEdge(int from, int to)
    {
        if (!adjacencyList.ContainsKey(from))
            AddVertex(from);
        if (!adjacencyList.ContainsKey(to))
            AddVertex(to);
            
        adjacencyList[from].Add(to);
    }
    
    public List<int> GetNeighbors(int vertex)
    {
        return adjacencyList.ContainsKey(vertex) 
            ? adjacencyList[vertex] 
            : new List<int>();
    }
}
```

**4.3 Busca em Grafos**

**BFS (Breadth-First Search) - Busca em Largura:**
```csharp
public List<int> BFS(int start, int target)
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
            // Reconstruir caminho
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
```

**DFS (Depth-First Search) - Busca em Profundidade:**
```csharp
private HashSet<int> visited = new HashSet<int>();

public bool DFS(int current, int target)
{
    if (current == target)
        return true;
        
    visited.Add(current);
    
    foreach (int neighbor in GetNeighbors(current))
    {
        if (!visited.Contains(neighbor))
        {
            if (DFS(neighbor, target))
                return true;
        }
    }
    
    return false;
}
```

**4.4 Aplicações em Jogos:**
- Sistema de salas/conexões (dungeon)
- Pathfinding (encontrar caminho)
- Sistema de waypoints
- Árvore de decisão (IA)

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Grafo de Salas**
```csharp
class RoomGraph
{
    private Dictionary<int, List<int>> connections = new Dictionary<int, List<int>>();
    
    // Implemente:
    // - Adicionar sala
    // - Conectar duas salas
    // - Verificar se duas salas estão conectadas
    // - Obter salas adjacentes
    // - Encontrar caminho entre duas salas (BFS)
}
```

**Exercício 2: Sistema de Waypoints**
```csharp
class WaypointSystem
{
    private Dictionary<Vector2, List<Vector2>> waypoints = new Dictionary<Vector2, List<Vector2>>();
    
    // Implemente:
    // - Adicionar waypoint
    // - Conectar waypoints
    // - Encontrar caminho entre dois waypoints
    // - Verificar se existe caminho
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Sistema de Dungeon com Grafos**
- Criar classe `Room` que representa um nó no grafo
- Implementar conexões entre salas usando Dictionary
- Sistema de transição entre salas

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
            Connections.Add(direction, roomId);
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

public class DungeonManager
{
    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
    public Room CurrentRoom { get; private set; }
    
    public void AddRoom(Room room)
    {
        rooms[room.Id] = room;
    }
    
    public void ChangeRoom(int roomId)
    {
        if (rooms.ContainsKey(roomId))
            CurrentRoom = rooms[roomId];
    }
}
```

---

### **AULA 5 - Revisão e Projeto Parcial (4h)**

#### **Objetivos:**
- Revisar todos os conceitos de estruturas de dados
- Implementar sistema completo de tilemap e salas
- Consolidar aprendizado

#### **Atividades (4h):**

**Parte 1: Revisão Teórica (1h)**
- Quiz sobre estruturas de dados
- Discussão de casos de uso
- Resolução de dúvidas

**Parte 2: Implementação Prática (3h)**
- Completar sistema de Tilemap
- Implementar sistema de Rooms com grafo
- Carregar múltiplas salas de arquivos CSV
- Sistema de transição entre salas

---

## 📅 SEMANA 2: Estruturas de Dados Avançadas

### **AULA 6 - Árvores Binárias e Heaps (4h)**

#### **Objetivos:**
- Compreender árvores binárias
- Implementar heaps para priorização
- Aplicar em sistemas de prioridade em jogos

#### **Teoria (1h30min):**

**6.1 Árvore Binária**
- Estrutura hierárquica
- Cada nó tem no máximo 2 filhos
- Aplicações: Árvore de decisão, organização hierárquica

**6.2 Heap (Fila de Prioridade)**
```csharp
// Em C#, podemos usar PriorityQueue (disponível a partir do .NET 6)
// Ou implementar manualmente

class MinHeap<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();
    
    private int Parent(int i) => (i - 1) / 2;
    private int Left(int i) => 2 * i + 1;
    private int Right(int i) => 2 * i + 2;
    
    public void Insert(T item)
    {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }
    
    public T ExtractMin()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Heap vazio");
            
        T min = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);
        
        return min;
    }
    
    private void HeapifyUp(int index) { /* ... */ }
    private void HeapifyDown(int index) { /* ... */ }
}
```

**Aplicações:**
- Sistema de eventos com prioridade
- Pathfinding (A* algorithm)
- Sistema de spawn de inimigos por prioridade

#### **Exercícios Práticos (1h30min):**

**Exercício: Sistema de Eventos com Prioridade**
```csharp
class PriorityEventSystem
{
    private PriorityQueue<GameEvent, int> eventQueue = new PriorityQueue<GameEvent, int>();
    
    // Implemente:
    // - Adicionar evento com prioridade
    // - Processar próximo evento (maior prioridade)
    // - Verificar se há eventos pendentes
}
```

#### **Implementação no Jogo (1h):**
- Sistema de spawn de inimigos por prioridade
- Sistema de atualização de entidades por prioridade

---

### **AULA 7 - Hash Tables e Otimização (4h)**

#### **Objetivos:**
- Entender funcionamento interno de hash tables
- Aplicar técnicas de otimização
- Implementar cache eficiente

#### **Teoria (1h30min):**

**7.1 Hash Tables - Funcionamento Interno**
- Função hash: converte chave em índice
- Colisões: quando duas chaves geram mesmo hash
- Resolução de colisões: chaining, linear probing

**7.2 Otimização em Jogos**
- Object pooling (reutilizar objetos)
- Spatial partitioning (dividir espaço)
- Cache de recursos

#### **Exercícios Práticos (1h30min):**

**Exercício: Object Pool**
```csharp
class ObjectPool<T> where T : new()
{
    private Stack<T> pool = new Stack<T>();
    
    public T Get()
    {
        if (pool.Count > 0)
            return pool.Pop();
        return new T();
    }
    
    public void Return(T obj)
    {
        // Resetar objeto
        pool.Push(obj);
    }
}
```

#### **Implementação no Jogo (1h):**
- Implementar object pool para projéteis
- Cache de texturas

---

### **AULA 8-10: Consolidação e Projeto - Sistema de Dungeon Completo**

Implementação completa do sistema de dungeon usando todas as estruturas aprendidas:
- Arrays 2D para tilemaps
- Listas para itens e inimigos
- Dicionários para conexões entre salas
- Grafos para navegação
- Stack para estados de jogo

---

# MÓDULO 2: DESIGN PATTERNS PARA JOGOS (Semana 3)

## 📅 SEMANA 3: Padrões de Design Fundamentais

### **AULA 11 - Singleton Pattern (4h)**

#### **Objetivos:**
- Compreender o padrão Singleton
- Identificar quando usar Singleton
- Implementar Singleton thread-safe

#### **Teoria (1h30min):**

**11.1 O que é Singleton?**
- Garante que uma classe tenha apenas uma instância
- Fornece ponto de acesso global
- Útil para managers e sistemas globais

**11.2 Implementação Básica:**
```csharp
public class GameManager
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }
    
    private GameManager() { } // Construtor privado
    
    public void Initialize()
    {
        // Inicialização
    }
}
```

**11.3 Implementação Thread-Safe (Lazy):**
```csharp
public class GameManager
{
    private static readonly Lazy<GameManager> _instance = 
        new Lazy<GameManager>(() => new GameManager());
    
    public static GameManager Instance => _instance.Value;
    
    private GameManager() { }
}
```

**11.4 Quando Usar:**
- ✅ Managers globais (GameManager, AudioManager, InputManager)
- ✅ Sistemas únicos (DatabaseManager, SaveSystem)
- ❌ Evitar para objetos que podem ter múltiplas instâncias
- ❌ Não usar como "variável global disfarçada"

**11.5 Vantagens e Desvantagens:**
- ✅ Acesso global fácil
- ✅ Garante única instância
- ❌ Dificulta testes unitários
- ❌ Pode criar acoplamento forte

#### **Exercícios Práticos (1h30min):**

**Exercício 1: AudioManager Singleton**
```csharp
class AudioManager
{
    // Implemente como Singleton:
    // - Reproduzir som
    // - Parar som
    // - Ajustar volume
    // - Inicializar sistema de áudio
}
```

**Exercício 2: InputManager Singleton**
```csharp
class InputManager
{
    // Implemente como Singleton:
    // - Verificar se tecla está pressionada
    // - Obter direção de movimento
    // - Atualizar estado de input
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Implementar Managers como Singleton**
- Converter `GameManager` para Singleton
- Criar `InputManager` como Singleton
- Criar `DatabaseManager` como Singleton

```csharp
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
    
    public Vector2 GetMovementDirection()
    {
        Vector2 direction = Vector2.Zero;
        
        if (_currentKeyboardState.IsKeyDown(Keys.W) || 
            _currentKeyboardState.IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.S) || 
            _currentKeyboardState.IsKeyDown(Keys.Down))
            direction.Y += 1;
        if (_currentKeyboardState.IsKeyDown(Keys.A) || 
            _currentKeyboardState.IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (_currentKeyboardState.IsKeyDown(Keys.D) || 
            _currentKeyboardState.IsKeyDown(Keys.Right))
            direction.X += 1;
            
        if (direction.Length() > 0)
            direction.Normalize();
            
        return direction;
    }
}
```

---

### **AULA 12 - Factory Pattern (4h)**

#### **Objetivos:**
- Compreender Factory Pattern
- Implementar Factory Method e Abstract Factory
- Aplicar em criação de entidades de jogo

#### **Teoria (1h30min):**

**12.1 O que é Factory Pattern?**
- Encapsula a criação de objetos
- Centraliza lógica de criação
- Facilita adicionar novos tipos

**12.2 Factory Method:**
```csharp
public abstract class EnemyFactory
{
    public abstract Enemy CreateEnemy(Vector2 position);
}

public class SlimeFactory : EnemyFactory
{
    public override Enemy CreateEnemy(Vector2 position)
    {
        return new Slime(position);
    }
}

public class GhostFactory : EnemyFactory
{
    public override Enemy CreateEnemy(Vector2 position)
    {
        return new Ghost(position);
    }
}
```

**12.3 Simple Factory (Static Factory):**
```csharp
public static class EnemyFactory
{
    public static Enemy CreateEnemy(string type, Vector2 position)
    {
        switch (type)
        {
            case "Slime":
                return new Slime(position);
            case "Ghost":
                return new Ghost(position);
            default:
                throw new ArgumentException($"Unknown enemy type: {type}");
        }
    }
}
```

**12.4 Quando Usar:**
- ✅ Criação complexa de objetos
- ✅ Múltiplos tipos relacionados
- ✅ Quando a lógica de criação pode mudar
- ✅ Quando queremos desacoplar criação do uso

#### **Exercícios Práticos (1h30min):**

**Exercício 1: ItemFactory**
```csharp
class ItemFactory
{
    // Implemente factory para criar:
    // - Coin (moeda)
    // - Potion (poção)
    // - Key (chave)
    // - Weapon (arma)
    
    // Use enum ou string para tipo
}
```

**Exercício 2: WeaponFactory**
```csharp
class WeaponFactory
{
    // Implemente factory para criar armas:
    // - Sword
    // - Bow
    // - Staff
    
    // Cada arma tem stats diferentes
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Implementar Factories**
- Criar `EnemyFactory` para criar inimigos
- Criar `ItemFactory` para criar itens
- Usar factories no `Room` para spawnar entidades

```csharp
public static class EnemyFactory
{
    private static ContentManager _content;
    private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
    
    public static void Initialize(ContentManager content)
    {
        _content = content;
    }
    
    public static Enemy CreateEnemy(string type, Vector2 position)
    {
        if (!_textures.ContainsKey(type))
        {
            _textures[type] = _content.Load<Texture2D>($"Enemies/{type}");
        }
        
        IEnemyBehavior behavior = type switch
        {
            "Ghost" => new ChaseBehavior(),
            "Slime" => new PatrolBehavior(),
            _ => new PatrolBehavior()
        };
        
        return new Enemy(_textures[type], position, behavior);
    }
}

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
            _textures[type] = _content.Load<Texture2D>($"Items/{type}");
        }
        
        return type switch
        {
            "Coin" => new Coin(_textures[type], position),
            "Chest" => new Chest(_textures[type], position),
            _ => throw new ArgumentException($"Unknown item type: {type}")
        };
    }
}
```

---

### **AULA 13 - Strategy Pattern (4h)**

#### **Objetivos:**
- Compreender Strategy Pattern
- Implementar comportamentos intercambiáveis
- Aplicar em IA de inimigos

#### **Teoria (1h30min):**

**13.1 O que é Strategy Pattern?**
- Define família de algoritmos
- Encapsula cada algoritmo
- Torna algoritmos intercambiáveis

**13.2 Estrutura:**
```csharp
// Interface da estratégia
public interface IEnemyBehavior
{
    void Update(Enemy enemy, GameTime gameTime, Player player);
}

// Estratégias concretas
public class PatrolBehavior : IEnemyBehavior
{
    public void Update(Enemy enemy, GameTime gameTime, Player player)
    {
        // Lógica de patrulha
    }
}

public class ChaseBehavior : IEnemyBehavior
{
    public void Update(Enemy enemy, GameTime gameTime, Player player)
    {
        // Lógica de perseguição
    }
}

// Contexto que usa a estratégia
public class Enemy
{
    private IEnemyBehavior _behavior;
    
    public Enemy(IEnemyBehavior behavior)
    {
        _behavior = behavior;
    }
    
    public void Update(GameTime gameTime, Player player)
    {
        _behavior.Update(this, gameTime, player);
    }
    
    public void SetBehavior(IEnemyBehavior newBehavior)
    {
        _behavior = newBehavior;
    }
}
```

**13.3 Quando Usar:**
- ✅ Múltiplas formas de realizar uma tarefa
- ✅ Quando queremos trocar algoritmo em runtime
- ✅ Evitar muitos if/else ou switch
- ✅ IA com diferentes comportamentos

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Comportamentos de Inimigo**
```csharp
// Implemente diferentes comportamentos:
// - IdleBehavior (parado)
// - PatrolBehavior (patrulha entre pontos)
// - ChaseBehavior (persegue player)
// - FleeBehavior (foge do player)
// - AttackBehavior (ataca quando próximo)
```

**Exercício 2: Sistema de Movimento**
```csharp
interface IMovementStrategy
{
    Vector2 CalculateMovement(Vector2 currentPosition, Vector2 target);
}

// Implemente:
// - WalkMovement
// - RunMovement
// - TeleportMovement
```

#### **Implementação no Jogo (1h):**

**Atividade: Implementar Strategy para Inimigos**
- Criar interface `IEnemyBehavior`
- Implementar `PatrolBehavior` e `ChaseBehavior`
- Aplicar em classe `Enemy`

```csharp
public interface IEnemyBehavior
{
    void Update(Enemy enemy, GameTime gameTime, Player player, Tilemap tilemap);
}

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

---

### **AULA 14 - Observer Pattern (4h)**

#### **Objetivos:**
- Compreender Observer Pattern
- Implementar sistema de eventos
- Aplicar em notificações e atualizações

#### **Teoria (1h30min):**

**14.1 O que é Observer Pattern?**
- Define dependência um-para-muitos
- Quando um objeto muda, todos dependentes são notificados
- Desacopla objetos

**14.2 Implementação:**
```csharp
// Interface do Observer
public interface IObserver
{
    void OnNotify(string eventType, object data);
}

// Subject (Observable)
public class GameEventManager
{
    private List<IObserver> observers = new List<IObserver>();
    
    public void Subscribe(IObserver observer)
    {
        observers.Add(observer);
    }
    
    public void Unsubscribe(IObserver observer)
    {
        observers.Remove(observer);
    }
    
    public void Notify(string eventType, object data)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(eventType, data);
        }
    }
}

// Observer concreto
public class HUD : IObserver
{
    public void OnNotify(string eventType, object data)
    {
        switch (eventType)
        {
            case "PlayerHealthChanged":
                UpdateHealthBar((int)data);
                break;
            case "ScoreChanged":
                UpdateScore((int)data);
                break;
        }
    }
}
```

**14.3 Eventos C# (Alternativa Moderna):**
```csharp
public class Player
{
    public event Action<int> OnHealthChanged;
    public event Action<int> OnScoreChanged;
    
    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChanged?.Invoke(health);
        }
    }
}

// Uso
player.OnHealthChanged += (newHealth) => {
    hud.UpdateHealthBar(newHealth);
};
```

**14.4 Quando Usar:**
- ✅ Sistema de eventos
- ✅ UI que precisa atualizar quando dados mudam
- ✅ Sistema de achievements
- ✅ Logging e debug

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Sistema de Eventos de Jogo**
```csharp
class GameEventSystem
{
    // Implemente:
    // - Registrar observer para evento
    // - Remover observer
    // - Disparar evento
    // - Eventos: EnemyDefeated, ItemCollected, LevelComplete
}
```

**Exercício 2: Sistema de Achievements**
```csharp
class AchievementSystem : IObserver
{
    // Implemente:
    // - Observar eventos do jogo
    // - Verificar condições de achievements
    // - Desbloquear achievements
    // - Notificar player
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Sistema de Eventos**
- Criar sistema de eventos usando delegates/events
- HUD observa mudanças no player
- Sistema de achievements básico

```csharp
public class Player
{
    public event Action<int> OnHealthChanged;
    public event Action<int> OnScoreChanged;
    public event Action OnDeath;
    
    private int health = 100;
    private int score = 0;
    
    public int Health
    {
        get => health;
        private set
        {
            health = Math.Max(0, value);
            OnHealthChanged?.Invoke(health);
            
            if (health <= 0)
                OnDeath?.Invoke();
        }
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    
    public void AddScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(score);
    }
}
```

---

### **AULA 15 - State Pattern e Command Pattern (4h)**

#### **Objetivos:**
- Compreender State e Command Patterns
- Implementar máquina de estados
- Sistema de comandos para undo/redo

#### **Teoria (1h30min):**

**15.1 State Pattern:**
```csharp
public interface IPlayerState
{
    void Enter(Player player);
    void Update(Player player, GameTime gameTime);
    void Exit(Player player);
}

public class IdleState : IPlayerState
{
    public void Enter(Player player) { }
    public void Update(Player player, GameTime gameTime)
    {
        if (InputManager.Instance.GetMovementDirection().Length() > 0)
            player.ChangeState(new WalkingState());
    }
    public void Exit(Player player) { }
}

public class Player
{
    private IPlayerState currentState;
    
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }
}
```

**15.2 Command Pattern:**
```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class MoveCommand : ICommand
{
    private Player player;
    private Vector2 previousPosition;
    private Vector2 newPosition;
    
    public MoveCommand(Player player, Vector2 newPosition)
    {
        this.player = player;
        this.newPosition = newPosition;
    }
    
    public void Execute()
    {
        previousPosition = player.Position;
        player.Position = newPosition;
    }
    
    public void Undo()
    {
        player.Position = previousPosition;
    }
}

public class CommandManager
{
    private Stack<ICommand> commandHistory = new Stack<ICommand>();
    
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }
    
    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            var command = commandHistory.Pop();
            command.Undo();
        }
    }
}
```

#### **Exercícios Práticos (1h30min):**

**Exercício: Máquina de Estados para Player**
```csharp
// Implemente estados:
// - IdleState
// - WalkingState
// - AttackingState
// - DeadState
```

#### **Implementação no Jogo (1h):**
- Implementar State Pattern para estados do jogo
- Sistema básico de comandos

---

# MÓDULO 3: BANCO DE DADOS (Semana 4)

## 📅 SEMANA 4: Persistência de Dados

### **AULA 16 - Introdução a Banco de Dados (4h)**

#### **Objetivos:**
- Compreender conceitos de banco de dados
- Entender SQL básico
- Configurar SQLite no projeto

#### **Teoria (1h30min):**

**16.1 O que é Banco de Dados?**
- Sistema de armazenamento persistente
- Organizado em tabelas (linhas e colunas)
- Permite consultas eficientes

**16.2 SQLite:**
- Banco de dados embutido
- Arquivo único (.db)
- Não requer servidor
- Perfeito para jogos

**16.3 SQL Básico:**

**CREATE TABLE:**
```sql
CREATE TABLE Players (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Level INTEGER DEFAULT 1,
    Experience INTEGER DEFAULT 0
);
```

**INSERT:**
```sql
INSERT INTO Players (Name, Level, Experience) 
VALUES ('Player1', 1, 0);
```

**SELECT:**
```sql
SELECT * FROM Players;
SELECT Name, Level FROM Players WHERE Level > 5;
```

**UPDATE:**
```sql
UPDATE Players 
SET Level = 2, Experience = 100 
WHERE Id = 1;
```

**DELETE:**
```sql
DELETE FROM Players WHERE Id = 1;
```

**16.4 Tipos de Dados SQLite:**
- INTEGER: Números inteiros
- REAL: Números decimais
- TEXT: Strings
- BLOB: Dados binários

#### **Exercícios Práticos (1h30min):**

**Exercício 1: Criar Tabela de Save Game**
```sql
-- Crie uma tabela para salvar progresso do jogo
-- Campos: Id, Level, PlayerX, PlayerY, Score, Timestamp
```

**Exercício 2: Queries Básicas**
```sql
-- 1. Inserir novo save
-- 2. Buscar save por ID
-- 3. Atualizar score de um save
-- 4. Listar todos os saves ordenados por score
-- 5. Deletar save antigo
```

#### **Implementação no Jogo (1h):**

**Atividade: Configurar SQLite**
- Adicionar pacote Microsoft.Data.Sqlite
- Criar classe DatabaseManager
- Criar tabela de saves

```csharp
// No .csproj, adicionar:
<PackageReference Include="Microsoft.Data.Sqlite" Version="7.0.0" />

public class DatabaseManager
{
    private string connectionString;
    
    public DatabaseManager()
    {
        connectionString = "Data Source=game.db";
        InitializeDatabase();
    }
    
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(connectionString);
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
}
```

---

### **AULA 17 - CRUD Completo (4h)**

#### **Objetivos:**
- Implementar operações CRUD completas
- Trabalhar com parâmetros SQL
- Tratar erros e exceções

#### **Teoria (1h30min):**

**17.1 CRUD:**
- **C**reate: INSERT
- **R**ead: SELECT
- **U**pdate: UPDATE
- **D**elete: DELETE

**17.2 Parâmetros SQL (Prevenção de SQL Injection):**
```csharp
// ❌ ERRADO (vulnerável a SQL Injection)
string sql = $"SELECT * FROM Players WHERE Name = '{playerName}'";

// ✅ CORRETO (usando parâmetros)
string sql = "SELECT * FROM Players WHERE Name = @Name";
command.Parameters.AddWithValue("@Name", playerName);
```

**17.3 Tratamento de Erros:**
```csharp
try
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    // Operações...
}
catch (SqliteException ex)
{
    Console.WriteLine($"Erro no banco de dados: {ex.Message}");
    // Tratar erro apropriadamente
}
```

#### **Exercícios Práticos (1h30min):**

**Exercício: Sistema de Save/Load Completo**
```csharp
class SaveSystem
{
    // Implemente:
    // - SaveGame(int level, Vector2 position, int score)
    // - LoadGame(int saveSlotId) -> retorna SaveData?
    // - DeleteSave(int saveSlotId)
    // - ListAllSaves() -> retorna List<SaveData>
    // - GetBestScore() -> retorna int
}
```

#### **Implementação no Jogo (1h):**

**Atividade: Implementar Save/Load**
- Completar DatabaseManager com CRUD
- Integrar com sistema de jogo
- Testar save/load

```csharp
public class DatabaseManager
{
    public void SaveGame(int level, Vector2 position, int score)
    {
        using var connection = new SqliteConnection(connectionString);
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
        using var connection = new SqliteConnection(connectionString);
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

---

### **AULA 18 - Relacionamentos e Queries Avançadas (4h)**

#### **Objetivos:**
- Compreender relacionamentos entre tabelas
- Implementar JOINs
- Criar queries complexas

#### **Teoria (1h30min):**

**18.1 Relacionamentos:**
- **1 para 1**: Um registro em uma tabela relaciona com um em outra
- **1 para Muitos**: Um registro relaciona com vários
- **Muitos para Muitos**: Vários registros relacionam com vários

**18.2 JOIN:**
```sql
-- INNER JOIN: Retorna apenas registros que têm correspondência
SELECT Players.Name, Scores.Score
FROM Players
INNER JOIN Scores ON Players.Id = Scores.PlayerId;

-- LEFT JOIN: Retorna todos da tabela esquerda, mesmo sem correspondência
SELECT Players.Name, Scores.Score
FROM Players
LEFT JOIN Scores ON Players.Id = Scores.PlayerId;
```

**18.3 Agregações:**
```sql
-- COUNT: Contar registros
SELECT COUNT(*) FROM Players;

-- SUM: Soma
SELECT SUM(Score) FROM Scores;

-- AVG: Média
SELECT AVG(Score) FROM Scores;

-- MAX/MIN: Máximo/Mínimo
SELECT MAX(Score) FROM Scores;
```

#### **Exercícios Práticos (1h30min):**

**Exercício: Sistema de Estatísticas**
```sql
-- Crie tabelas:
-- Players (Id, Name)
-- GameSessions (Id, PlayerId, StartTime, EndTime, Score)
-- ItemsCollected (Id, SessionId, ItemName, Quantity)

-- Queries:
-- 1. Listar top 10 players por score total
-- 2. Média de score por player
-- 3. Items mais coletados
-- 4. Tempo médio de jogo por player
```

#### **Implementação no Jogo (1h):**
- Criar sistema de estatísticas
- Tabela de histórico de partidas
- Queries para leaderboard

---

### **AULA 19 - Otimização e Boas Práticas (4h)**

#### **Objetivos:**
- Aprender técnicas de otimização
- Implementar índices
- Boas práticas de banco de dados

#### **Teoria (1h30min):**

**19.1 Índices:**
```sql
-- Criar índice para acelerar buscas
CREATE INDEX idx_player_name ON Players(Name);
CREATE INDEX idx_score ON Scores(Score DESC);
```

**19.2 Transações:**
```csharp
using var connection = new SqliteConnection(connectionString);
connection.Open();

using var transaction = connection.BeginTransaction();
try
{
    // Múltiplas operações
    // ...
    
    transaction.Commit();
}
catch
{
    transaction.Rollback();
    throw;
}
```

**19.3 Boas Práticas:**
- Sempre usar parâmetros (prevenir SQL Injection)
- Fechar conexões (usar `using`)
- Usar transações para operações múltiplas
- Criar índices para campos frequentemente consultados
- Não fazer queries dentro de loops

#### **Exercícios Práticos (1h30min):**

**Exercício: Otimizar Sistema de Save**
- Adicionar índices
- Implementar transações
- Batch inserts para múltiplos saves

#### **Implementação no Jogo (1h):**
- Otimizar DatabaseManager
- Adicionar índices
- Implementar sistema de múltiplos save slots

---

### **AULA 20 - Projeto Final: Integração Completa (4h)**

#### **Objetivos:**
- Integrar todos os sistemas
- Testar funcionalidades completas
- Apresentar projeto

#### **Atividades (4h):**

**Parte 1: Integração (2h)**
- Garantir que todos os sistemas funcionam juntos
- Estrutura de Dados: Tilemap, Rooms, Lists, Dictionaries
- Design Patterns: Singleton, Factory, Strategy
- Banco de Dados: Save/Load funcionando

**Parte 2: Testes e Debug (1h)**
- Testar todas as funcionalidades
- Corrigir bugs
- Otimizar performance

**Parte 3: Apresentação (1h)**
- Preparar apresentação do projeto
- Documentar código
- Demonstrar funcionalidades

---

# SEMANA 5: PROJETO FINAL E APRESENTAÇÕES

## 📅 SEMANA 5: Consolidação

### **AULA 21-25: Desenvolvimento do Projeto Final**

Os alunos trabalharão no projeto completo "Dungeon of Algorithms" aplicando todos os conceitos aprendidos:

**Checklist do Projeto:**
- [ ] Sistema de Tilemap usando arrays 2D
- [ ] Sistema de Rooms usando grafos (Dictionary)
- [ ] Listas para gerenciar itens e inimigos
- [ ] Singleton para managers (GameManager, InputManager, DatabaseManager)
- [ ] Factory para criar inimigos e itens
- [ ] Strategy para comportamentos de inimigos
- [ ] Observer/Events para atualizar HUD
- [ ] Banco de dados SQLite para save/load
- [ ] Sistema completo funcional

**Entregáveis:**
1. Código fonte completo e comentado
2. Documentação do projeto
3. Apresentação (10-15 minutos)
4. Demonstração do jogo funcionando

---

# 📚 RECURSOS E MATERIAIS

## Bibliografia Recomendada:
1. "Design Patterns: Elements of Reusable Object-Oriented Software" - Gang of Four
2. "Game Programming Patterns" - Robert Nystrom
3. "Data Structures and Algorithms in C#" - Michael McMillan
4. Documentação oficial: Microsoft Learn (C#, .NET, SQLite)

## Ferramentas:
- Visual Studio / Visual Studio Code
- .NET SDK
- MonoGame Framework
- SQLite Browser (para visualizar banco de dados)

## Links Úteis:
- MonoGame Documentation: https://docs.monogame.net/
- Microsoft Data.Sqlite: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/
- C# Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/

---

# 📊 AVALIAÇÃO

## Critérios de Avaliação:

**Teoria (30%):**
- Quizzes e exercícios teóricos
- Participação em aula

**Prática (40%):**
- Exercícios práticos
- Implementações durante as aulas
- Qualidade do código

**Projeto Final (30%):**
- Funcionalidade completa
- Aplicação correta dos conceitos
- Organização e documentação
- Apresentação

---

# 🎯 CONCLUSÃO

Este plano de aulas foi desenvolvido para proporcionar uma experiência de aprendizado completa e prática, onde os alunos não apenas aprendem conceitos teóricos, mas os aplicam diretamente no desenvolvimento de um jogo real. A estrutura progressiva garante que cada conceito seja bem compreendido antes de avançar para o próximo, e a integração final demonstra como todos os conceitos trabalham juntos em um projeto real.

**Boa sorte com o curso! 🎮**
