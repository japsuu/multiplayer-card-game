using System.Collections.Generic;

namespace Entities.Enemies
{
    public static class EnemyManager
    {
        private static readonly List<EnemyCharacter> AliveEnemies = new();
        
        public static IEnumerable<EnemyCharacter> Enemies => AliveEnemies;


        public static void AddEnemy(EnemyCharacter enemy)
        {
            AliveEnemies.Add(enemy);
        }
        
        
        public static void RemoveEnemy(EnemyCharacter enemy)
        {
            AliveEnemies.Remove(enemy);
        }
    }
}