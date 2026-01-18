using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonOfAlgorithms.Source.Core;

namespace DungeonOfAlgorithms.Source.Entities;

public class ChestItem : Item
{
    public ChestItem(Texture2D texture, Vector2 position) : base(999, "Treasure Chest", texture, position)
    {
    }
}
