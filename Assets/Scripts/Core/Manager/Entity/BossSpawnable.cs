using System;
using UnityEngine;

namespace Core.Manager.Entity
{
    [Serializable]
    public class BossSpawnable
    {
        public GameObject bossPrefab;
        [SerializeField] public float spawnRadius = 5f; // 스폰 위치 반경
        [SerializeField] public bool spawnOnce = true; // 단일 스폰 여부
    }
}