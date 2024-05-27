using System.Collections.Generic;
using UnityEngine;

namespace Entities.Players
{
    public static class PlayerManager
    {
        private static readonly List<PlayerCharacter> AlivePlayers = new();

        public static IEnumerable<PlayerCharacter> Players => AlivePlayers;


        public static void Add(PlayerCharacter enemy)
        {
            AlivePlayers.Add(enemy);
        }


        public static void Remove(PlayerCharacter enemy)
        {
            AlivePlayers.Remove(enemy);
        }
        
        
        public static PlayerCharacter FindNearestHorizontalPlayer(Vector2Int position)
        {
            PlayerCharacter nearestPlayer = null;
            int nearestDistance = int.MaxValue;
            foreach (PlayerCharacter player in AlivePlayers)
            {
                int distance = Mathf.Abs(player.BoardPosition.x - position.x);
                if (distance >= nearestDistance)
                    continue;
                
                nearestPlayer = player;
                nearestDistance = distance;
            }

            return nearestPlayer;
        }
    }
}