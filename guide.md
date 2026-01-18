Walkthrough: Fundação do Projeto "Dungeon of Algorithms"
O Que Foi Feito
Implementamos a base (Week 1) e parte do movimento (Week 2) do cronograma acelerado.

1. Estrutura do Projeto
O projeto foi criado usando o template mgdesktopgl e estruturado em pastas para facilitar o ensino:

Source/Core: Classes fundamentais (
Game1
, 
GameManager
, 
InputManager
, 
IGameEntity
).
Source/Entities: Objetos de jogo (
Player
).
Source/Systems: (Futuro) Colisão, Audio.
Content
: Assets do jogo.
2. Arquitetura Implementada
GameManager (Singleton): Garante acesso global aos sistemas principais.
InputManager (Adapter): Abstrai a entrada (teclado) permitindo movimentação vetorial normalizada.
IGameEntity: Interface que força todo objeto a ter 
Update
 e 
Draw
.
Player: Implementa 
IGameEntity
 e usa 
InputManager
 para se mover.
3. Como Rodar
Abra o terminal na pasta do projeto:
cd "/Users/lucaslopes/Library/Mobile Documents/com~apple~CloudDocs/TopDownGame/DungeonOfAlgorithms"
Compile e execute:
dotnet run
Controles: Use W, A, S, D ou setas para mover o quadrado branco (Player). ESC para sair.
Próximos Passos
Implementar carga de mapas (Tilemaps).
Adicionar câmera que segue o player.
Criar inimigos com comportamento básico (Strategy Pattern).

Walkthrough: Fundação do Projeto "Dungeon of Algorithms"
O Que Foi Feito
Implementamos a base (Week 1) e parte do movimento (Week 2) do cronograma acelerado.

1. Estrutura do Projeto
O projeto foi criado usando o template mgdesktopgl e estruturado em pastas para facilitar o ensino:

Source/Core: Classes fundamentais (
Game1
, 
GameManager
, 
InputManager
, 
IGameEntity
).
Source/Entities: Objetos de jogo (
Player
).
Source/Systems: (Futuro) Colisão, Audio.
Content
: Assets do jogo.
2. Arquitetura Implementada
GameManager (Singleton): Garante acesso global aos sistemas principais.
InputManager (Adapter): Abstrai a entrada (teclado) permitindo movimentação vetorial normalizada.
IGameEntity: Interface que força todo objeto a ter 
Update
 e 
Draw
.
Player: Implementa 
IGameEntity
 e usa 
InputManager
 para se mover.
Camera: Segue o jogador mantendo-o centralizado na tela.
Tilemap: Renderiza o mapa baseado em uma array de índices e um tileset (atlas).
3. Como Rodar
Abra o terminal na pasta do projeto:
cd "/Users/lucaslopes/Library/Mobile Documents/com~apple~CloudDocs/TopDownGame/DungeonOfAlgorithms"
Compile e execute:
dotnet run
Controles: Use W, A, S, D ou setas para mover o quadrado branco (Player). ESC para sair.
Novidade: O cenário (tiles coloridos aleatórios) deve ficar fixo enquanto a câmera segue o player.
Próximos Passos
Implementar carga de mapas (Tilemaps).
Adicionar câmera que segue o player.
Criar inimigos com comportamento básico (Strategy Pattern).
