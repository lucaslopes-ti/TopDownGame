using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonOfAlgorithms.Source.Core;

public class Item : IGameEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Vector2 Position { get; private set; }
    public bool IsActive { get; set; } = true;
    public int Value { get; private set; } = 10; // Points value
    
    private Texture2D _texture;
    
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 16, 16);

    public Item(int id, string name, Texture2D texture, Vector2 position)
    {
        Id = id;
        Name = name;
        _texture = texture;
        Position = position;
    }

    public void Update(GameTime gameTime)
    {
        // Simple bobbing animation
        // float yOffset = (float)System.Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5) * 2;
        // Draw position could be adjusted here, but actual logical position stays same
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            spriteBatch.Draw(_texture, Position, Color.White);
    }
}
