using System.Collections.Generic;

namespace DungeonOfAlgorithms.Source.Core;

public class DungeonManager
{
    private static DungeonManager _instance;
    public static DungeonManager Instance => _instance ??= new DungeonManager();

    public Dictionary<int, Room> Rooms { get; private set; } = new();
    public Room CurrentRoom { get; private set; }

    private DungeonManager() { }

    public void AddRoom(Room room)
    {
        if (!Rooms.ContainsKey(room.Id))
            Rooms.Add(room.Id, room);
    }

    public void CheckTransition(int playerX, int playerY, int mapWidthPixel, int mapHeightPixel)
    {
        // Simple logic: if outside bounds, try to find neighbor
        string direction = null;
        
        if (playerX > mapWidthPixel) direction = "East";
        else if (playerX < 0) direction = "West";
        else if (playerY > mapHeightPixel) direction = "South";
        else if (playerY < 0) direction = "North";

        if (direction != null && CurrentRoom.Connections.ContainsKey(direction))
        {
            ChangeRoom(CurrentRoom.Connections[direction]);
            // TODO: Teleport player to opposite side? 
            // For now let's just change room concept.
        }
    }

    public void ChangeRoom(int roomId)
    {
        if (Rooms.ContainsKey(roomId))
            CurrentRoom = Rooms[roomId];
    }

    public int[,] LoadMapFromCSV(string filePath)
    {
        // Check if file exists to assume it's valid path, if not try relative to content or base dir
        if (!System.IO.File.Exists(filePath))
        { 
             // Debug fallback: try base directory
             string altPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);
             if (System.IO.File.Exists(altPath)) filePath = altPath;
        }

        string[] lines = System.IO.File.ReadAllLines(filePath);
        
        int height = lines.Length;
        int width = lines[0].Split(',').Length;
        
        
        int[,] mapData = new int[height, width];
        
        for (int y = 0; y < height; y++)
        {
            string[] values = lines[y].Split(',');
            for (int x = 0; x < width; x++)
            {
                if (int.TryParse(values[x], out int tileIndex))
                {
                    // Tiled sometimes exports -1 for empty, treat as 0 or handling needed
                    mapData[y, x] = tileIndex < 0 ? 0 : tileIndex;
                }
            }
        }
        
        return mapData;
    }
}
