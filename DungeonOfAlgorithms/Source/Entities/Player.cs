using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class Player : IGameEntity
{
    public Vector2 Position { get; private set; }
    public float Speed { get; set; } = 100f;
    public int Health { get; private set; } = 100;
    public int Score { get; private set; } = 0;
    public bool IsAlive => Health > 0;
    
    // Animation
    private Dictionary<string, Texture2D> _textures;
    private string _currentAnimation = "Down_Idle";
    private SpriteEffects _flipEffect = SpriteEffects.None;
    
    // State
    private enum FaceDirection { Down, Up, Left, Right }
    private FaceDirection _facing = FaceDirection.Down;
    
    private float _invincibilityTimer = 0f;
    private const float INVINCIBILITY_DURATION = 1.0f;
    
    // Frame Control
    private const int FRAME_WIDTH = 32;
    private const int FRAME_HEIGHT = 32;
    private const int IDLE_FRAME_COUNT = 4;
    private const int MOVE_FRAME_COUNT = 6;
    private const float FRAME_TIME = 0.15f;
    private int _currentFrame = 0;
    private float _frameTimer = 0f;
    private bool _isMoving = false;
    private bool _wasMoving = false;

    public Rectangle Bounds => new Rectangle((int)Position.X + 8, (int)Position.Y + 16, 16, 16);

    public Player(Dictionary<string, Texture2D> textures, Vector2 startPosition)
    {
        _textures = textures;
        Position = startPosition;
    }

    public void SetPosition(Vector2 newPosition)
    {
        Position = newPosition;
    }

    public void TakeDamage(int amount)
    {
        if (_invincibilityTimer <= 0)
        {
            Health -= amount;
            _invincibilityTimer = INVINCIBILITY_DURATION;
            System.Console.WriteLine($"Player took {amount} damage! Health: {Health}");
        }
    }

    public void AddScore(int points)
    {
        Score += points;
        System.Console.WriteLine($"Score: {Score}");
    }

    public void Update(GameTime gameTime)
    {
        Update(gameTime, null);
    }
    
    public void Update(GameTime gameTime, Tilemap tilemap)
    {
        var direction = InputManager.Instance.GetMovementDirection();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update invincibility timer
        if (_invincibilityTimer > 0)
            _invincibilityTimer -= deltaTime;

        // --- Animation Logic ---
        _isMoving = direction != Vector2.Zero;
        
        // Determine Direction
        if (direction.Y > 0) _facing = FaceDirection.Down;
        else if (direction.Y < 0) _facing = FaceDirection.Up;
        else if (direction.X > 0) _facing = FaceDirection.Right;
        else if (direction.X < 0) _facing = FaceDirection.Left;
        
        // Determine Animation Key and Effect based on Facing + State
        _flipEffect = SpriteEffects.None;
        string suffix = _isMoving ? "" : "_Idle";
        
        switch (_facing)
        {
            case FaceDirection.Down:
                _currentAnimation = "Down" + suffix;
                break;
            case FaceDirection.Up:
                _currentAnimation = "Up" + suffix;
                break;
            case FaceDirection.Right:
                _currentAnimation = "Side" + suffix;
                // If original sprite looks Left, flip when moving Right
                _flipEffect = SpriteEffects.FlipHorizontally;
                break;
            case FaceDirection.Left:
                _currentAnimation = "Side" + suffix;
                _flipEffect = SpriteEffects.None;
                break;
        }

        // Update Frame Timer
        // Reset frame when changing state
        if (_isMoving != _wasMoving)
        {
            _currentFrame = 0;
            _frameTimer = 0f;
            _wasMoving = _isMoving;
        }
        
        int frameCount = _isMoving ? MOVE_FRAME_COUNT : IDLE_FRAME_COUNT;
        _frameTimer += deltaTime;
        if (_frameTimer >= FRAME_TIME)
        {
            _frameTimer = 0f;
            _currentFrame = (_currentFrame + 1) % frameCount;
        }

        // --- Movement Logic ---
        Vector2 newPosition = Position + direction * Speed * deltaTime;
        
        // Check collision if tilemap provided
        if (tilemap != null)
        {
            // Try X movement
            Rectangle xBounds = new Rectangle((int)newPosition.X + 8, (int)Position.Y + 16, 16, 16);
            if (!tilemap.IsColliding(xBounds))
            {
                Position = new Vector2(newPosition.X, Position.Y);
            }
            
            // Try Y movement
            Rectangle yBounds = new Rectangle((int)Position.X + 8, (int)newPosition.Y + 16, 16, 16);
            if (!tilemap.IsColliding(yBounds))
            {
                Position = new Vector2(Position.X, newPosition.Y);
            }
        }
        else
        {
            Position = newPosition;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Blink when invincible
        if (_invincibilityTimer > 0 && (int)(_invincibilityTimer * 10) % 2 == 0)
            return; // Skip draw frame for blink effect
        
        // Get current texture
        Texture2D texture = _textures[_currentAnimation];
        
        // Calculate source rectangle for current frame
        Rectangle sourceRect = new Rectangle(_currentFrame * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT);
            
        spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, _flipEffect, 0f);
    }
}
