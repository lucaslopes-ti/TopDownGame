using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonOfAlgorithms.Source.Core;

public class Tilemap
{
    private readonly Texture2D _tilesetTexture;
    public Texture2D Texture => _tilesetTexture;
    private readonly int[,] _mapData;
    private readonly int _tileWidth;
    private readonly int _tileHeight;
    private readonly int _tilesPerRow;
    
    // Solid tile indices (walls, obstacles) - customize based on your tileset
    private static readonly HashSet<int> SolidTiles = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // First row usually walls
    
    // Door tile indices - tiles that trigger room transitions
    // 99 = East door, 98 = West door, 97 = North door, 96 = South door (example indices)
    public static readonly int DoorEast = 99;
    public static readonly int DoorWest = 98;
    public static readonly int DoorNorth = 97;
    public static readonly int DoorSouth = 96;

    public int TileWidth => _tileWidth;
    public int TileHeight => _tileHeight;
    public int MapWidth => _mapData.GetLength(1) * _tileWidth;
    public int MapHeight => _mapData.GetLength(0) * _tileHeight;

    public Tilemap(Texture2D tileset, int[,] mapData, int tileWidth, int tileHeight)
    {
        _tilesetTexture = tileset;
        _mapData = mapData;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _tilesPerRow = tileset.Width / tileWidth;
    }

    public int GetTileAt(int worldX, int worldY)
    {
        int tileX = worldX / _tileWidth;
        int tileY = worldY / _tileHeight;
        
        if (tileX < 0 || tileY < 0 || tileY >= _mapData.GetLength(0) || tileX >= _mapData.GetLength(1))
            return -1; // Out of bounds
            
        return _mapData[tileY, tileX];
    }

    public bool IsSolid(int worldX, int worldY)
    {
        int tile = GetTileAt(worldX, worldY);
        return tile < 0 || SolidTiles.Contains(tile); // Out of bounds or solid tile
    }

    public bool IsColliding(Rectangle bounds)
    {
        // Check all 4 corners of the bounding box
        return IsSolid(bounds.Left, bounds.Top) ||
               IsSolid(bounds.Right - 1, bounds.Top) ||
               IsSolid(bounds.Left, bounds.Bottom - 1) ||
               IsSolid(bounds.Right - 1, bounds.Bottom - 1);
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
                if (tileIndex < 0) continue; // -1 for empty

                // Calculate source rectangle from tileset
                int tileX = (tileIndex % _tilesPerRow) * _tileWidth;
                int tileY = (tileIndex / _tilesPerRow) * _tileHeight;
                Rectangle sourceRect = new Rectangle(tileX, tileY, _tileWidth, _tileHeight);

                // Calculate position in world
                Vector2 position = new Vector2(x * _tileWidth, y * _tileHeight);

                spriteBatch.Draw(_tilesetTexture, position, sourceRect, Color.White);
            }
        }
    }
}

