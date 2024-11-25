using System;
using UnityEngine;

namespace Core.Manager.Entity
{
    [Serializable]
    public class EnemySpawnable
    {
        public GameObject prefab;
        [SerializeField] public int totalSpawnLimit = 20; // 총 스폰 제한 수
        [SerializeField] public int maxCount = 10; // 현재 활성화된 적의 최대 수
        [SerializeField] public float spawnInterval = 3f; // 스폰 간격
        [SerializeField] public float spawnRadius = 5f; // 스폰 위치 반경
    }
}