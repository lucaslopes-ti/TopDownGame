using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class Enemy : IGameEntity
{
    public int Id { get; private set; }
    public Vector2 Position { get; set; } // Public set for Behavior to modify
    public float Speed { get; set; } = 50f;
    public int Damage { get; set; } = 10;
    
    private Texture2D _texture;
    private IEnemyBehavior _behavior;
    
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

    public Enemy(int id, Texture2D texture, Vector2 startPosition, IEnemyBehavior behavior)
    {
        Id = id;
        _texture = texture;
        Position = startPosition;
        _behavior = behavior;
    }

    public void ChangeBehavior(IEnemyBehavior newBehavior)
    {
        _behavior = newBehavior;
    }

    public void Update(GameTime gameTime)
    {
        // Need access to Player for behavior. 
        // In a real engine we'd use a GameplayScene or Manager to find player.
        // For now, let's assume valid instance or pass it in.
        // Assuming GameManager has Player reference or we pass it.
        // HACK: Let's find player via a temporary static reference or assume checking distance in behavior
        // Improving: Let's pass Player from Update loop or store ref.
    }
    
    // Overload Update to accept Player to make Strategy easy
    public void Update(GameTime gameTime, Player player)
    {
        _behavior.Update(this, player, gameTime);
    }

    // Default IGameEntity Implementation (ignored if not calling overload)
    void IGameEntity.Update(GameTime gameTime) 
    {
        // This won't work well without player. 
        // We will call the overloaded Update manually.
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.Red); // Tint Red for danger
    }
}
