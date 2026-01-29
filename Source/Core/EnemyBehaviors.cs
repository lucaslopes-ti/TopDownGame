using Microsoft.Xna.Framework;
using DungeonOfAlgorithms.Source.Entities;
using System;

namespace DungeonOfAlgorithms.Source.Core;

// Strategy 1: Patrol back and forth
public class PatrolBehavior : IEnemyBehavior
{
    private float _timer;
    private Vector2 _direction = new Vector2(1, 0);

    public void Update(Enemy enemy, Player player, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _timer += deltaTime;

        // Change direction every 2 seconds
        if (_timer > 2f)
        {
            _direction *= -1;
            _timer = 0;
        }

        enemy.Position += _direction * enemy.Speed * deltaTime;
        
        // Simple check to see if player is close -> Switch to Chase?
        // In a full game, we'd do: if (Vector2.Distance(enemy.Position, player.Position) < 100) enemy.ChangeBehavior(new ChaseBehavior());
    }
}

// Strategy 2: Chase the player
public class ChaseBehavior : IEnemyBehavior
{
    public void Update(Enemy enemy, Player player, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = player.Position - enemy.Position;
        if (direction != Vector2.Zero)
            direction.Normalize();

        enemy.Position += direction * (enemy.Speed * 0.8f) * deltaTime; // Chase is slightly slower
    }
}
