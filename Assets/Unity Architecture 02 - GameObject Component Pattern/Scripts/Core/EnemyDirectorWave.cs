using System.Collections.Generic;
using UnityEngine;

namespace UnityArchitecture.GameObjectComponentPattern
{
    public class EnemyDirectorWave : MonoBehaviour
    {
        public int enemiesToKill = 10;
        public List<EnemyType> enemyTypes = new();
        public List<EnemyType> bossEnemyTypes = new();
        public List<EnemyType> bossTypes = new();
        public float healthMultiplier = 1f;
        public float damageMultiplier = 1f;
        public int maxEnemiesAlive = 3;
    }
}