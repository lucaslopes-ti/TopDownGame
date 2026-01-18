using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

// Simple HUD - Observes Player state and displays it
public class HUD
{
    private SpriteFont _font;
    
    public HUD(SpriteFont font)
    {
        _font = font;
    }

    public void Draw(SpriteBatch spriteBatch, Player player)
    {
        // Draw Health
        string healthText = $"HP: {player.Health}";
        spriteBatch.DrawString(_font, healthText, new Vector2(10, 10), Color.Red);
        
        // Draw Score
        string scoreText = $"Score: {player.Score}";
        spriteBatch.DrawString(_font, scoreText, new Vector2(10, 30), Color.Gold);
    }
}
