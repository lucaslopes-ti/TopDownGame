using DungeonOfAlgorithms.Source.Entities;

namespace DungeonOfAlgorithms.Source.Core;

// Strategy Interface
public interface IEnemyBehavior
{
    void Update(Enemy enemy, Player player, Microsoft.Xna.Framework.GameTime gameTime);
}
