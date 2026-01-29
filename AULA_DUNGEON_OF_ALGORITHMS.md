# 🎮 Dungeon of Algorithms
## Uma Jornada de Desenvolvimento - Construindo Juntos

---

> *"Todo grande jogo começa com uma tela azul e um sonho."*

---

## 🗺️ Nossa Jornada

Neste material, vamos construir um jogo **do zero**, passo a passo, exatamente como acontece no mundo real. Não vamos mostrar o código "perfeito" de cara - vamos **evoluir** junto com o projeto, tomando decisões, enfrentando problemas e melhorando nossa solução.

**O jogo que vamos criar:**
- Um dungeon crawler top-down
- Player que se move e coleta moedas
- Inimigos com comportamentos diferentes
- Salas conectadas (como um labirinto)
- Sistema de vida, pontuação e game over

Vamos lá? 🚀

---

# 📅 DIA 1: O Primeiro Passo

## "Vamos fazer um jogo!"

Todo projeto começa com uma ideia e... uma tela vazia.

### Criando o Projeto

Abra o terminal e digite:

```bash
dotnet new mgdesktopgl -n DungeonOfAlgorithms
cd DungeonOfAlgorithms
dotnet run
```

**O que aconteceu?**
- Criamos um projeto MonoGame
- Ele já vem com uma estrutura básica
- Ao rodar, aparece uma tela azul (CornflowerBlue)

🎉 **Parabéns!** Você já tem um "jogo" rodando. Ele não faz nada ainda, mas é um começo!

### Entendendo o que o MonoGame nos deu

Abra o arquivo `Game1.cs`. Você vai ver algo assim:

```csharp
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() { base.Initialize(); }
    
    protected override void LoadContent() 
    { 
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
    }
}
```

**🤔 Momento de reflexão com a turma:**

> "O que vocês acham que esses métodos fazem?"

- `Initialize()` - Preparar as coisas antes de carregar
- `LoadContent()` - Carregar imagens, sons, fontes
- `Update()` - Lógica do jogo (roda ~60x por segundo!)
- `Draw()` - Desenhar na tela (roda ~60x por segundo!)

Isso é o **Game Loop** - o coração de todo jogo!

```
┌─────────────────────────────────────┐
│                                     │
│    ┌──────────┐                     │
│    │  UPDATE  │◄─── Processar input │
│    │  (Lógica)│     Física, IA...   │
│    └────┬─────┘                     │
│         │                           │
│         ▼                           │
│    ┌──────────┐                     │
│    │   DRAW   │◄─── Desenhar tudo   │
│    │(Renderiza)│                    │
│    └────┬─────┘                     │
│         │                           │
│         └──────────► Repetir!       │
│                                     │
└─────────────────────────────────────┘
```

---

## Primeiro Desafio: "Quero um quadrado na tela!"

Antes de fazer um personagem animado, vamos começar simples: **um quadrado branco**.

```csharp
// No topo da classe, adicione:
private Texture2D _pixelTexture;
private Vector2 _playerPosition = new Vector2(100, 100);

// Em LoadContent(), adicione:
_pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
_pixelTexture.SetData(new[] { Color.White });

// Em Draw(), entre Begin e End:
_spriteBatch.Begin();
_spriteBatch.Draw(_pixelTexture, 
    new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32), 
    Color.White);
_spriteBatch.End();
```

**Rode o jogo!**

Você deve ver um quadrado branco no canto superior esquerdo. Não é muito emocionante, mas... ele existe! 🎉

---

## Segundo Desafio: "Quero mover o quadrado!"

Agora vem a parte legal. No método `Update()`, vamos ler o teclado:

```csharp
protected override void Update(GameTime gameTime)
{
    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    // NOVO: Ler direção do movimento
    var keyboardState = Keyboard.GetState();
    
    if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        _playerPosition.Y -= 5;
    if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        _playerPosition.Y += 5;
    if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        _playerPosition.X -= 5;
    if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        _playerPosition.X += 5;

    base.Update(gameTime);
}
```

**Rode e teste!** Use WASD ou as setas para mover.

### 🐛 Opa, tem um problema...

**Pergunte à turma:** *"Alguém percebeu algo estranho quando move na diagonal?"*

Isso mesmo! Na diagonal, o quadrado anda **mais rápido**! 

**Por quê?** 
- Horizontal: velocidade = 5
- Vertical: velocidade = 5  
- Diagonal: velocidade = √(5² + 5²) ≈ 7.07

### A Solução: Normalização

```csharp
// Versão corrigida:
Vector2 direction = Vector2.Zero;

if (keyboardState.IsKeyDown(Keys.W)) direction.Y -= 1;
if (keyboardState.IsKeyDown(Keys.S)) direction.Y += 1;
if (keyboardState.IsKeyDown(Keys.A)) direction.X -= 1;
if (keyboardState.IsKeyDown(Keys.D)) direction.X += 1;

// Normalizar = fazer o vetor ter tamanho 1
if (direction != Vector2.Zero)
    direction.Normalize();

_playerPosition += direction * 5; // Agora sim!
```

**Teste novamente!** A diagonal agora tem a mesma velocidade.

---

## 🐛 Outro Problema: Velocidade depende do computador!

Em um computador rápido (120 FPS), o personagem anda mais rápido.
Em um computador lento (30 FPS), ele anda mais devagar.

**Solução: deltaTime**

O `gameTime` nos diz quanto tempo passou desde o último frame:

```csharp
float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
float speed = 200f; // pixels por SEGUNDO

_playerPosition += direction * speed * deltaTime;
```

Agora a velocidade é consistente em qualquer computador! 🎯

---

# 📅 DIA 2: Organizando a Bagunça

## "O código está ficando grande demais..."

Olhe só o nosso `Update()`. Ele está fazendo:
- Ler teclado
- Calcular direção
- Normalizar
- Mover o player

E vamos adicionar muito mais! Precisamos **organizar**.

### Criando uma estrutura de pastas

```
DungeonOfAlgorithms/
├── Source/
│   ├── Core/          ← Coisas que fazem o jogo funcionar
│   └── Entities/      ← "Coisas" do jogo (player, inimigos)
└── Content/           ← Imagens, sons, fontes
```

**Por que separar?**
- Código mais fácil de encontrar
- Cada arquivo faz UMA coisa
- Trabalho em equipe fica mais fácil

---

## Extraindo o InputManager

**Problema:** O código de ler teclado está misturado com a lógica do player.

**Solução:** Criar uma classe só para isso!

Crie o arquivo `Source/Core/InputManager.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonOfAlgorithms.Source.Core;

public class InputManager
{
    // Uma única instância para todo o jogo
    private static InputManager _instance;
    public static InputManager Instance => _instance ??= new InputManager();

    private KeyboardState _currentState;
    private KeyboardState _previousState;

    private InputManager() 
    {
        _currentState = Keyboard.GetState();
        _previousState = _currentState;
    }

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }

    public Vector2 GetMovementDirection()
    {
        Vector2 direction = Vector2.Zero;

        if (_currentState.IsKeyDown(Keys.W) || _currentState.IsKeyDown(Keys.Up))
            direction.Y -= 1;
        if (_currentState.IsKeyDown(Keys.S) || _currentState.IsKeyDown(Keys.Down))
            direction.Y += 1;
        if (_currentState.IsKeyDown(Keys.A) || _currentState.IsKeyDown(Keys.Left))
            direction.X -= 1;
        if (_currentState.IsKeyDown(Keys.D) || _currentState.IsKeyDown(Keys.Right))
            direction.X += 1;

        if (direction != Vector2.Zero)
            direction.Normalize();

        return direction;
    }

    // Útil depois: saber se uma tecla FOI pressionada (não está sendo segurada)
    public bool IsKeyPressed(Keys key)
    {
        return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
    }
}
```

**🤔 Discussão:** 

> "Por que guardamos o estado anterior do teclado?"

Resposta: Para detectar o **momento** em que uma tecla foi pressionada, não só se ela está sendo segurada. Útil para menus, ataques, etc.

### Usando o InputManager

No `Game1.cs`:

```csharp
// Em Update(), no início:
InputManager.Instance.Update();

// E agora o movimento fica assim:
var direction = InputManager.Instance.GetMovementDirection();
_playerPosition += direction * speed * deltaTime;
```

Muito mais limpo! ✨

---

## Criando a Classe Player

**Próximo passo:** O player merece sua própria classe.

Crie `Source/Entities/Player.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class Player
{
    public Vector2 Position { get; private set; }
    public float Speed { get; set; } = 200f;
    
    private Texture2D _texture;

    public Player(Texture2D texture, Vector2 startPosition)
    {
        _texture = texture;
        Position = startPosition;
    }

    public void Update(GameTime gameTime)
    {
        var direction = InputManager.Instance.GetMovementDirection();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Position += direction * Speed * deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}
```

**No Game1:**

```csharp
private Player _player;

// Em LoadContent():
_player = new Player(_pixelTexture, new Vector2(100, 100));

// Em Update():
InputManager.Instance.Update();
_player.Update(gameTime);

// Em Draw():
_spriteBatch.Begin();
_player.Draw(_spriteBatch);
_spriteBatch.End();
```

**Olha como ficou limpo!** O `Game1` agora só coordena, não faz o trabalho pesado.

---

# 📅 DIA 3: O Mundo do Jogo

## "Um fundo azul não é muito empolgante..."

Precisamos de um **cenário**! Mas desenhar pixel por pixel seria loucura.

### A Solução: Tilemaps

Imagine um tabuleiro de xadrez onde cada quadrado pode ter uma imagem diferente:
- Quadrado 1: Chão de pedra
- Quadrado 2: Parede
- Quadrado 3: Porta
- ...

Isso é um **Tilemap**!

```
Matriz do mapa:          Resultado visual:
┌─┬─┬─┬─┬─┐              ┌────────────────┐
│1│1│1│1│1│              │▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓│ Parede
├─┼─┼─┼─┼─┤              │▓              ▓│
│1│0│0│0│1│              │▓  Chão        ▓│
├─┼─┼─┼─┼─┤              │▓              ▓│
│1│0│0│0│1│              │▓              ▓│
├─┼─┼─┼─┼─┤              │▓              ▓│
│1│1│1│1│1│              │▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓│ Parede
└─┴─┴─┴─┴─┘              └────────────────┘

0 = chão, 1 = parede
```

### Criando a classe Tilemap

Crie `Source/Core/Tilemap.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonOfAlgorithms.Source.Core;

public class Tilemap
{
    private Texture2D _tileset;      // Imagem com todos os tiles
    private int[,] _mapData;         // Matriz: qual tile vai onde
    private int _tileSize = 16;      // Cada tile tem 16x16 pixels
    private int _tilesPerRow;        // Quantos tiles cabem em uma linha do tileset
    
    // Quais tiles são paredes (não pode atravessar)
    private HashSet<int> _solidTiles = new() { 96, 97, 98, 99, 100 };

    public int MapWidth => _mapData.GetLength(1) * _tileSize;
    public int MapHeight => _mapData.GetLength(0) * _tileSize;

    public Tilemap(Texture2D tileset, int[,] mapData)
    {
        _tileset = tileset;
        _mapData = mapData;
        _tilesPerRow = tileset.Width / _tileSize;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int rows = _mapData.GetLength(0);
        int cols = _mapData.GetLength(1);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int tileIndex = _mapData[y, x];
                if (tileIndex < 0) continue; // -1 = vazio

                // Onde está esse tile no tileset?
                int sourceX = (tileIndex % _tilesPerRow) * _tileSize;
                int sourceY = (tileIndex / _tilesPerRow) * _tileSize;
                
                Rectangle source = new Rectangle(sourceX, sourceY, _tileSize, _tileSize);
                Vector2 position = new Vector2(x * _tileSize, y * _tileSize);

                spriteBatch.Draw(_tileset, position, source, Color.White);
            }
        }
    }
}
```

**🤔 Momento de pausa:**

> "Alguém pode explicar o que `tileIndex % _tilesPerRow` faz?"

Se o tileset tem 10 tiles por linha:
- Tile 0: coluna 0 (0 % 10 = 0)
- Tile 5: coluna 5 (5 % 10 = 5)
- Tile 12: coluna 2 (12 % 10 = 2)
- Tile 23: coluna 3 (23 % 10 = 3)

E `tileIndex / tilesPerRow` dá a linha!

### Carregando mapas de arquivos

Poderíamos escrever a matriz no código, mas fica difícil de editar. Melhor usar arquivos CSV!

**Room_01.csv:**
```
96,97,97,97,97,97,97,98
77,0,0,0,0,0,0,58
77,0,0,0,0,0,0,58
77,0,0,0,0,0,0,58
77,0,0,0,0,0,0,58
20,21,21,21,21,21,21,22
```

**Método para carregar:**

```csharp
public static int[,] LoadFromCSV(string path)
{
    string[] lines = System.IO.File.ReadAllLines(path);
    
    int rows = lines.Length;
    int cols = lines[0].Split(',').Length;
    int[,] data = new int[rows, cols];
    
    for (int y = 0; y < rows; y++)
    {
        string[] values = lines[y].Split(',');
        for (int x = 0; x < cols; x++)
        {
            data[y, x] = int.Parse(values[x]);
        }
    }
    
    return data;
}
```

💡 **Dica:** Use o programa [Tiled](https://www.mapeditor.org/) para desenhar mapas visualmente e exportar como CSV!

---

## Colisão com as paredes

**Problema:** O player atravessa as paredes!

**Solução:** Verificar se o próximo passo colide com um tile sólido.

Adicione ao `Tilemap`:

```csharp
public bool IsSolid(int worldX, int worldY)
{
    int tileX = worldX / _tileSize;
    int tileY = worldY / _tileSize;
    
    // Fora do mapa = sólido
    if (tileX < 0 || tileY < 0 || 
        tileY >= _mapData.GetLength(0) || 
        tileX >= _mapData.GetLength(1))
        return true;
        
    int tile = _mapData[tileY, tileX];
    return _solidTiles.Contains(tile);
}

public bool IsColliding(Rectangle bounds)
{
    // Verificar os 4 cantos
    return IsSolid(bounds.Left, bounds.Top) ||
           IsSolid(bounds.Right - 1, bounds.Top) ||
           IsSolid(bounds.Left, bounds.Bottom - 1) ||
           IsSolid(bounds.Right - 1, bounds.Bottom - 1);
}
```

No `Player.Update()`:

```csharp
public void Update(GameTime gameTime, Tilemap tilemap)
{
    var direction = InputManager.Instance.GetMovementDirection();
    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    Vector2 newPosition = Position + direction * Speed * deltaTime;
    
    // Criar um retângulo de colisão (menor que o sprite)
    Rectangle newBounds = new Rectangle(
        (int)newPosition.X + 4, 
        (int)newPosition.Y + 4, 
        24, 24
    );
    
    // Só mover se não colidir!
    if (!tilemap.IsColliding(newBounds))
    {
        Position = newPosition;
    }
}
```

**Teste!** Agora você não atravessa mais as paredes. 🧱

### 🐛 Problema: Fico preso nas quinas!

Quando chego perto de uma parede na diagonal, fico completamente travado.

**Solução:** Verificar X e Y separadamente!

```csharp
// Tentar movimento em X
Rectangle xBounds = new Rectangle((int)newPosition.X + 4, (int)Position.Y + 4, 24, 24);
if (!tilemap.IsColliding(xBounds))
    Position = new Vector2(newPosition.X, Position.Y);

// Tentar movimento em Y  
Rectangle yBounds = new Rectangle((int)Position.X + 4, (int)newPosition.Y + 4, 24, 24);
if (!tilemap.IsColliding(yBounds))
    Position = new Vector2(Position.X, newPosition.Y);
```

Agora você "desliza" nas paredes em vez de travar! ✨

---

# 📅 DIA 4: A Câmera

## "O player some da tela quando anda muito..."

Quando o mapa é maior que a tela, precisamos de uma **câmera** que segue o player.

### Como funciona?

A câmera é uma **ilusão**. Não movemos a câmera - movemos TODO o mundo na direção oposta!

```
Se o player está em (200, 100)
E queremos ele no centro da tela (400, 300)

Movemos TUDO por (-200, -100) + (400, 300) = (200, 200)
```

### Criando a Camera

Crie `Source/Core/Camera.cs`:

```csharp
using Microsoft.Xna.Framework;

namespace DungeonOfAlgorithms.Source.Core;

public class Camera
{
    public Matrix Transform { get; private set; }
    public float Zoom { get; set; } = 3.0f;  // Zoom para pixel art ficar grande

    public void Follow(Vector2 target, int screenWidth, int screenHeight)
    {
        // 1. Mover o mundo para que o alvo fique na origem
        var position = Matrix.CreateTranslation(-target.X, -target.Y, 0);

        // 2. Aplicar zoom
        var scale = Matrix.CreateScale(Zoom, Zoom, 1);

        // 3. Centralizar na tela
        var offset = Matrix.CreateTranslation(screenWidth / 2f, screenHeight / 2f, 0);

        // Ordem importa! posição → escala → offset
        Transform = position * scale * offset;
    }
}
```

### Usando a câmera

No `Game1`:

```csharp
private Camera _camera;

// Em Initialize():
_camera = new Camera();

// Em Update():
_camera.Follow(_player.Position, 
    _graphics.PreferredBackBufferWidth, 
    _graphics.PreferredBackBufferHeight);

// Em Draw() - O SEGREDO!
_spriteBatch.Begin(
    transformMatrix: _camera.Transform,
    samplerState: SamplerState.PointClamp  // Pixels nítidos!
);
```

**Rode o jogo!** Agora a câmera segue o player automaticamente! 🎥

---

# 📅 DIA 5: Múltiplas Salas

## "Uma sala só é meio entediante..."

Vamos criar um sistema de **salas conectadas** - como um grafo!

```
┌─────────┐         ┌─────────┐         ┌─────────┐
│         │         │         │         │         │
│  Sala 1 │──East──►│  Sala 2 │──East──►│  Sala 3 │
│         │◄──West──│         │◄──West──│ (Tesouro)│
└─────────┘         └─────────┘         └─────────┘
```

### A Classe Room

Crie `Source/Core/Room.cs`:

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonOfAlgorithms.Source.Core;

public class Room
{
    public int Id { get; private set; }
    public Tilemap Tilemap { get; private set; }
    
    // Conexões: "East" -> 2 significa "ir para leste leva à sala 2"
    public Dictionary<string, int> Connections { get; private set; } = new();

    public Room(int id, Tilemap tilemap)
    {
        Id = id;
        Tilemap = tilemap;
    }

    public void Connect(string direction, int targetRoomId)
    {
        Connections[direction] = targetRoomId;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);
    }
}
```

### O DungeonManager

Precisamos de algo para gerenciar todas as salas:

```csharp
using System.Collections.Generic;

namespace DungeonOfAlgorithms.Source.Core;

public class DungeonManager
{
    private static DungeonManager _instance;
    public static DungeonManager Instance => _instance ??= new DungeonManager();

    public Dictionary<int, Room> Rooms { get; private set; } = new();
    public Room CurrentRoom { get; private set; }

    public void AddRoom(Room room)
    {
        Rooms[room.Id] = room;
    }

    public void ChangeRoom(int roomId)
    {
        if (Rooms.ContainsKey(roomId))
        {
            CurrentRoom = Rooms[roomId];
            System.Console.WriteLine($"Entrou na Sala {roomId}!");
        }
    }
}
```

### Configurando as Salas

No `LoadContent()`:

```csharp
var tileset = Content.Load<Texture2D>("Tiles/Tileset");

// Criar as salas
var map1 = LoadMapFromCSV("Content/Maps/Room_01.csv");
var room1 = new Room(1, new Tilemap(tileset, map1));

var map2 = LoadMapFromCSV("Content/Maps/Room_02.csv");
var room2 = new Room(2, new Tilemap(tileset, map2));

var map3 = LoadMapFromCSV("Content/Maps/Room_03.csv");
var room3 = new Room(3, new Tilemap(tileset, map3));

// Conectar (criar o grafo!)
room1.Connect("East", 2);
room2.Connect("West", 1);
room2.Connect("East", 3);
room3.Connect("West", 2);

// Registrar
DungeonManager.Instance.AddRoom(room1);
DungeonManager.Instance.AddRoom(room2);
DungeonManager.Instance.AddRoom(room3);
DungeonManager.Instance.ChangeRoom(1);  // Começar na sala 1
```

### Transição de Salas

Quando o player chega na borda do mapa em uma direção que tem conexão:

```csharp
// Em Update(), verificar transição:
var room = DungeonManager.Instance.CurrentRoom;

// Chegou na borda leste?
if (_player.Position.X > room.Tilemap.MapWidth - 30 && 
    room.Connections.ContainsKey("East"))
{
    int nextRoom = room.Connections["East"];
    DungeonManager.Instance.ChangeRoom(nextRoom);
    _player.SetPosition(new Vector2(50, _player.Position.Y));  // Aparecer do outro lado
}

// Chegou na borda oeste?
if (_player.Position.X < 30 && room.Connections.ContainsKey("West"))
{
    int nextRoom = room.Connections["West"];
    DungeonManager.Instance.ChangeRoom(nextRoom);
    _player.SetPosition(new Vector2(room.Tilemap.MapWidth - 50, _player.Position.Y));
}
```

**Teste!** Agora você pode explorar múltiplas salas! 🚪

---

# 📅 DIA 6: Itens Coletáveis

## "Precisa ter algo para coletar!"

Vamos adicionar **moedas** que dão pontos.

### A Classe Item

Crie `Source/Entities/Item.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonOfAlgorithms.Source.Entities;

public class Item
{
    public Vector2 Position { get; private set; }
    public bool IsActive { get; set; } = true;
    public int Value { get; private set; } = 10;
    
    private Texture2D _texture;
    
    public Rectangle Bounds => new Rectangle(
        (int)Position.X, (int)Position.Y, 16, 16
    );

    public Item(Texture2D texture, Vector2 position, int value = 10)
    {
        _texture = texture;
        Position = position;
        Value = value;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            spriteBatch.Draw(_texture, Position, Color.White);
    }
}
```

### Adicionando Score ao Player

```csharp
// Na classe Player, adicione:
public int Score { get; private set; } = 0;

public void AddScore(int points)
{
    Score += points;
    System.Console.WriteLine($"Score: {Score}");
}
```

### Coletando Itens

Na classe `Room`, adicione uma lista de itens e a verificação de coleta:

```csharp
public List<Item> Items { get; private set; } = new();

public void AddItem(Item item) => Items.Add(item);

public void Update(Player player)
{
    foreach (var item in Items)
    {
        if (item.IsActive && player.Bounds.Intersects(item.Bounds))
        {
            item.IsActive = false;
            player.AddScore(item.Value);
        }
    }
}

public void Draw(SpriteBatch spriteBatch)
{
    Tilemap.Draw(spriteBatch);
    
    foreach (var item in Items)
        item.Draw(spriteBatch);
}
```

### Espalhando Moedas

```csharp
// Em LoadContent():
var coinTexture = Content.Load<Texture2D>("Items/Coin");

room1.AddItem(new Item(coinTexture, new Vector2(80, 80)));
room1.AddItem(new Item(coinTexture, new Vector2(150, 150)));
room1.AddItem(new Item(coinTexture, new Vector2(200, 100)));

room2.AddItem(new Item(coinTexture, new Vector2(100, 100)));
room2.AddItem(new Item(coinTexture, new Vector2(200, 200)));
```

**Teste!** Colete as moedas e veja o score aumentar! 💰

---

# 📅 DIA 7: Inimigos!

## "O jogo está muito fácil..."

Hora de adicionar **perigo**! Vamos criar inimigos.

### Primeiro: Um Inimigo Simples

Crie `Source/Entities/Enemy.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonOfAlgorithms.Source.Entities;

public class Enemy
{
    public Vector2 Position { get; set; }
    public float Speed { get; set; } = 50f;
    public int Damage { get; set; } = 10;
    
    private Texture2D _texture;
    
    public Rectangle Bounds => new Rectangle(
        (int)Position.X, (int)Position.Y, 32, 32
    );

    public Enemy(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        Position = position;
    }

    public void Update(GameTime gameTime, Player player)
    {
        // Por enquanto, não faz nada
        // Vamos adicionar comportamento depois!
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}
```

### Adicionando Vida ao Player

```csharp
// Na classe Player:
public int Health { get; private set; } = 100;
public bool IsAlive => Health > 0;

private float _invincibilityTimer = 0f;

public void TakeDamage(int amount)
{
    // Invencibilidade temporária evita dano contínuo
    if (_invincibilityTimer <= 0)
    {
        Health -= amount;
        _invincibilityTimer = 1.0f;  // 1 segundo de invencibilidade
        System.Console.WriteLine($"Ouch! HP: {Health}");
    }
}

// No Update, decrementar o timer:
if (_invincibilityTimer > 0)
    _invincibilityTimer -= deltaTime;
```

### Verificando Dano

Na `Room.Update()`:

```csharp
foreach (var enemy in Enemies)
{
    enemy.Update(gameTime, player);
    
    if (player.Bounds.Intersects(enemy.Bounds))
    {
        player.TakeDamage(enemy.Damage);
    }
}
```

---

## Comportamentos de Inimigos

**Problema:** Todo inimigo faz a mesma coisa (nada!).

**Solução:** Criar diferentes **comportamentos** que podemos trocar!

### A Interface IEnemyBehavior

```csharp
using DungeonOfAlgorithms.Source.Entities;
using Microsoft.Xna.Framework;

namespace DungeonOfAlgorithms.Source.Core;

public interface IEnemyBehavior
{
    void Update(Enemy enemy, Player player, GameTime gameTime);
}
```

### Comportamento: Patrulha

O inimigo vai de um lado pro outro:

```csharp
public class PatrolBehavior : IEnemyBehavior
{
    private float _timer;
    private Vector2 _direction = new Vector2(1, 0);  // Começa indo para direita

    public void Update(Enemy enemy, Player player, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _timer += deltaTime;

        // A cada 2 segundos, inverte a direção
        if (_timer > 2f)
        {
            _direction *= -1;
            _timer = 0;
        }

        enemy.Position += _direction * enemy.Speed * deltaTime;
    }
}
```

### Comportamento: Perseguição

O inimigo segue o player:

```csharp
public class ChaseBehavior : IEnemyBehavior
{
    public void Update(Enemy enemy, Player player, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Calcular direção até o player
        Vector2 direction = player.Position - enemy.Position;
        
        if (direction != Vector2.Zero)
            direction.Normalize();

        enemy.Position += direction * enemy.Speed * deltaTime;
    }
}
```

### Usando Comportamentos

Modifique o `Enemy` para receber um comportamento:

```csharp
public class Enemy
{
    // ... campos existentes ...
    
    private IEnemyBehavior _behavior;

    public Enemy(Texture2D texture, Vector2 position, IEnemyBehavior behavior)
    {
        _texture = texture;
        Position = position;
        _behavior = behavior;
    }

    public void Update(GameTime gameTime, Player player, Tilemap tilemap)
    {
        Vector2 oldPosition = Position;
        
        // Executar o comportamento!
        _behavior.Update(this, player, gameTime);
        
        // Verificar colisão com paredes
        if (tilemap.IsColliding(Bounds))
            Position = oldPosition;
    }
    
    // Permite trocar comportamento em tempo de execução!
    public void ChangeBehavior(IEnemyBehavior newBehavior)
    {
        _behavior = newBehavior;
    }
}
```

### Criando Diferentes Tipos de Inimigos

```csharp
// Slime que patrulha
var slime = new Enemy(slimeTexture, new Vector2(200, 150), new PatrolBehavior());

// Fantasma que persegue
var ghost = new Enemy(ghostTexture, new Vector2(300, 200), new ChaseBehavior());

room1.AddEnemy(slime);
room2.AddEnemy(ghost);
```

**🤔 Discussão com a turma:**

> "Qual a vantagem de separar o comportamento em uma interface?"

- Podemos criar novos comportamentos sem mexer na classe `Enemy`
- Podemos trocar comportamento em tempo de execução
- Código mais organizado e testável

---

# 📅 DIA 8: Estados do Jogo e HUD

## "Precisa de um menu, game over..."

### Estados do Jogo

Crie `Source/Core/GameState.cs`:

```csharp
namespace DungeonOfAlgorithms.Source.Core;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Victory
}
```

### No Game1:

```csharp
private GameState _gameState = GameState.MainMenu;

protected override void Update(GameTime gameTime)
{
    InputManager.Instance.Update();
    
    switch (_gameState)
    {
        case GameState.MainMenu:
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                _gameState = GameState.Playing;
            break;
            
        case GameState.Playing:
            // Toda a lógica do jogo aqui
            _player.Update(gameTime, currentRoom.Tilemap);
            currentRoom.Update(_player);
            
            if (!_player.IsAlive)
                _gameState = GameState.GameOver;
            break;
            
        case GameState.GameOver:
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Reiniciar
                _player = new Player(_playerTexture, new Vector2(100, 100));
                DungeonManager.Instance.ChangeRoom(1);
                _gameState = GameState.Playing;
            }
            break;
    }
}
```

### HUD (Heads-Up Display)

Crie `Source/Core/HUD.cs`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public class HUD
{
    private SpriteFont _font;

    public HUD(SpriteFont font)
    {
        _font = font;
    }

    public void Draw(SpriteBatch spriteBatch, Player player)
    {
        // Vida em vermelho
        spriteBatch.DrawString(_font, $"HP: {player.Health}", 
            new Vector2(10, 10), Color.Red);
        
        // Score em dourado
        spriteBatch.DrawString(_font, $"Score: {player.Score}", 
            new Vector2(10, 30), Color.Gold);
    }
}
```

### Desenhar HUD sem Transformação de Câmera

O HUD deve ficar fixo na tela, não no mundo:

```csharp
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.Black);

    // PRIMEIRO: Desenhar o mundo (com câmera)
    _spriteBatch.Begin(transformMatrix: _camera.Transform, 
                       samplerState: SamplerState.PointClamp);
    
    DungeonManager.Instance.CurrentRoom.Draw(_spriteBatch);
    _player.Draw(_spriteBatch);
    
    _spriteBatch.End();

    // DEPOIS: Desenhar HUD (sem câmera - espaço de tela)
    _spriteBatch.Begin();
    
    if (_gameState == GameState.MainMenu)
    {
        _spriteBatch.DrawString(_font, "DUNGEON OF ALGORITHMS", 
            new Vector2(200, 150), Color.Gold);
        _spriteBatch.DrawString(_font, "Pressione ENTER", 
            new Vector2(280, 300), Color.White);
    }
    else if (_gameState == GameState.GameOver)
    {
        _spriteBatch.DrawString(_font, "GAME OVER", 
            new Vector2(320, 200), Color.Red);
        _spriteBatch.DrawString(_font, "Pressione R para reiniciar", 
            new Vector2(250, 250), Color.White);
    }
    else
    {
        _hud.Draw(_spriteBatch, _player);
    }
    
    _spriteBatch.End();
}
```

---

# 📅 DIA 9: Animações e Sprites

## "O quadrado branco não é muito bonito..."

Até agora usamos quadrados ou imagens estáticas. Vamos animar!

### Conceito: Sprite Sheet

Uma sprite sheet é uma imagem com vários frames lado a lado:

```
┌────┬────┬────┬────┬────┬────┐
│ F1 │ F2 │ F3 │ F4 │ F5 │ F6 │  ← Animação de andar
└────┴────┴────┴────┴────┴────┘
  0    1    2    3    4    5
```

### Animando o Player

```csharp
// Campos de animação:
private int _currentFrame = 0;
private float _frameTimer = 0f;
private const float FRAME_TIME = 0.15f;  // Tempo por frame
private const int FRAME_WIDTH = 32;
private const int FRAME_HEIGHT = 32;
private const int FRAME_COUNT = 6;

// No Update:
_frameTimer += deltaTime;
if (_frameTimer >= FRAME_TIME)
{
    _frameTimer = 0f;
    _currentFrame = (_currentFrame + 1) % FRAME_COUNT;
}

// No Draw:
Rectangle sourceRect = new Rectangle(
    _currentFrame * FRAME_WIDTH, 
    0, 
    FRAME_WIDTH, 
    FRAME_HEIGHT
);

spriteBatch.Draw(_texture, Position, sourceRect, Color.White);
```

### Múltiplas Animações

O player tem diferentes animações: andar para baixo, para cima, para os lados, parado...

```csharp
private Dictionary<string, Texture2D> _textures;
private string _currentAnimation = "Down_Idle";

// Cada direção tem sua sprite sheet
// "Down" = andando para baixo
// "Down_Idle" = parado olhando para baixo
// "Side" = andando para o lado (espelhamos para esquerda/direita)
```

No Update, escolhemos a animação baseado no movimento:

```csharp
bool isMoving = direction != Vector2.Zero;
string suffix = isMoving ? "" : "_Idle";

if (direction.Y > 0) 
    _currentAnimation = "Down" + suffix;
else if (direction.Y < 0) 
    _currentAnimation = "Up" + suffix;
else if (direction.X != 0) 
    _currentAnimation = "Side" + suffix;
```

Para espelhar quando vai para a direita:

```csharp
SpriteEffects flip = direction.X > 0 
    ? SpriteEffects.FlipHorizontally 
    : SpriteEffects.None;

spriteBatch.Draw(texture, Position, sourceRect, Color.White, 
                 0f, Vector2.Zero, 1f, flip, 0f);
```

---

# 📅 DIA 10: Toques Finais

## Efeitos Visuais

### Piscar quando Invencível

```csharp
// No Draw do Player:
if (_invincibilityTimer > 0 && (int)(_invincibilityTimer * 10) % 2 == 0)
    return;  // Pula o draw nesse frame = pisca!
```

### Transição com Fade

Ao trocar de sala, em vez de cortar direto, fazemos um fade:

```csharp
private float _fadeAlpha = 0f;
private bool _isFading = false;

// Ao detectar transição:
_isFading = true;

// No Update, durante fade:
if (_isFading)
{
    _fadeAlpha += 3f * deltaTime;
    if (_fadeAlpha >= 1f)
    {
        // Agora troca a sala
        DungeonManager.Instance.ChangeRoom(_pendingRoom);
        _fadeAlpha = 1f;
        // Começa a clarear
    }
}

// No Draw, no final:
if (_fadeAlpha > 0)
{
    _spriteBatch.Draw(_blackTexture, 
        new Rectangle(0, 0, 800, 600), 
        Color.White * _fadeAlpha);
}
```

---

# 🎯 Resumo da Jornada

| Dia | O que fizemos | Conceitos |
|-----|---------------|-----------|
| 1 | Quadrado se movendo | Game Loop, Vetores |
| 2 | Organização de código | Classes, Separação de responsabilidades |
| 3 | Tilemap e colisão | Matrizes 2D, HashSet |
| 4 | Câmera | Matriz de transformação |
| 5 | Múltiplas salas | Grafos, Dictionary |
| 6 | Itens coletáveis | Detecção de colisão |
| 7 | Inimigos | Interfaces, Comportamentos |
| 8 | Estados e HUD | Enum, State Machine |
| 9 | Animações | Sprite Sheets |
| 10 | Efeitos | Polish, Game Feel |

---

# 🏆 Exercícios para Praticar

## Nível 1 - Aquecimento 🟢

1. Mude a velocidade do player
2. Adicione mais moedas em posições diferentes
3. Mude a cor de fundo do jogo

## Nível 2 - Desafio 🟡

4. Crie uma nova sala (Room_04.csv) e conecte ao grafo
5. Faça o player perder vida ao cair em lava (tile específico)
6. Adicione um contador de moedas na HUD

## Nível 3 - Boss 🔴

7. Crie um novo comportamento de inimigo (ex: CircleBehavior)
8. Implemente um ataque com a tecla Espaço
9. Adicione som quando coleta moeda (SoundEffect)

## Nível 4 - Lendário 🟣

10. Implemente pathfinding BFS para os fantasmas
11. Crie um sistema de save/load com arquivo
12. Adicione uma sala do boss com condição de vitória

---

# 💬 Reflexões Finais

## O que aprendemos?

1. **Jogos são loops** - Update e Draw infinitamente
2. **Organização importa** - Código bagunçado = dor de cabeça
3. **Comece simples** - Quadrado antes de sprite animado
4. **Itere** - Cada dia melhora um pouco
5. **Debug é seu amigo** - Console.WriteLine salva vidas

## Erros que cometemos (e isso é normal!)

- ❌ Diagonal mais rápida → ✅ Normalização
- ❌ Velocidade inconsistente → ✅ deltaTime
- ❌ Travar nas quinas → ✅ Verificar X e Y separados
- ❌ Dano infinito → ✅ Invencibilidade temporária

## Próximos passos para explorar

- **Áudio** - Música e efeitos sonoros
- **Partículas** - Explosões, poeira
- **IA avançada** - Pathfinding, comportamentos complexos
- **Procedural Generation** - Gerar dungeons automaticamente

---

*"Todo expert já foi um iniciante. Todo jogo incrível começou com um quadrado se movendo."*

**Bom desenvolvimento! 🎮**

---

*Material desenvolvido para fins educacionais*
*Projeto: Dungeon of Algorithms - The Memory Leak Chronicle*
