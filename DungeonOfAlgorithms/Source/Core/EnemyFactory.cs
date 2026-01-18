using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

public static class EnemyFactory
{
    private static ContentManager _content;
    private static Dictionary<string, Texture2D> _textures = new();

    public static void Initialize(ContentManager content)
    {
        _content = content;
    }

    public static Enemy CreateEnemy(string type, Vector2 position)
    {
        if (!_textures.ContainsKey("Ghost")) _textures["Ghost"] = _content.Load<Texture2D>("Enemies/Ghost");
        if (!_textures.ContainsKey("Slime")) _textures["Slime"] = _content.Load<Texture2D>("Enemies/Slime");

        switch (type)
        {
            case "Ghost":
                // Ghosts Chase
                return new Enemy(1, _textures["Ghost"], position, new ChaseBehavior());
            case "Slime":
                // Slimes Patrol
                return new Enemy(2, _textures["Slime"], position, new PatrolBehavior());
            default:
                return null;
        }
    }
}
