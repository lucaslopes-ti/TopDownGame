using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

// Represents a Node in our Dungeon Graph
public class Room
{
    public int Id { get; private set; }
    public Tilemap Tilemap { get; private set; }
    
    public Dictionary<string, int> Connections { get; private set; } = new();
    
    public List<Item> Items { get; private set; } = new();
    public List<Enemy> Enemies { get; private set; } = new();
    public List<DecorObject> DecorObjects { get; private set; } = new();

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
    
    public void AddDecor(DecorObject decor)
    {
        DecorObjects.Add(decor);
    }
    
    // Check if bounds collide with any decor object
    public bool IsCollidingWithDecor(Rectangle bounds)
    {
        foreach (var decor in DecorObjects)
        {
            if (bounds.Intersects(decor.Bounds))
                return true;
        }
        return false;
    }
    
    public void Update(GameTime gameTime, Player player, System.Action<Item> onItemCollected = null) // Pass Player for Strategy
    {
        // Check Item Collection
        foreach (var item in Items)
        {
            if (item.IsActive && player.Bounds.Intersects(item.Bounds))
            {
                item.IsActive = false;
                player.AddScore(item.Value);
                onItemCollected?.Invoke(item);
            }
        }
        
        // Update Enemies and Check Damage
        foreach (var enemy in Enemies)
        {
            enemy.Update(gameTime, player, Tilemap);
            
            // Check if enemy touches player
            if (player.Bounds.Intersects(enemy.Bounds))
            {
                player.TakeDamage(enemy.Damage);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Tilemap.Draw(spriteBatch);
        
        // Draw decor objects (behind items and enemies)
        foreach (var decor in DecorObjects)
        {
            decor.Draw(spriteBatch);
        }
        
        foreach (var item in Items)
        {
            item.Draw(spriteBatch);
        }
        foreach (var enemy in Enemies)
        {
            enemy.Draw(spriteBatch);
        }
    }
}

