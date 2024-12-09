using System.Collections;
using System.Collections.Generic;
using Core.abstracts;
using Core.Manager.Entity;
using UnityEngine;
using Core.Pattern;
using Features.Enemies.components;
using Random = UnityEngine.Random;

namespace Core.Manager
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public List<EnemySpawnable> enemySpawnables = new List<EnemySpawnable>();
        public BossSpawnable bossSpawnable; // 보스 스폰 정보

        private readonly Dictionary<GameObject, List<GameObject>> _activeEnemies =
            new Dictionary<GameObject, List<GameObject>>();

        private readonly Dictionary<GameObject, int> _totalSpawnedEnemies =
            new Dictionary<GameObject, int>();

        private bool _bossSpawned; // 보스 스폰 여부

        private void Start()
        {
            foreach (var enemySpawnable in enemySpawnables)
            {
                _activeEnemies[enemySpawnable.prefab] = new List<GameObject>();
                _totalSpawnedEnemies[enemySpawnable.prefab] = 0;
                StartCoroutine(SpawnEnemy(enemySpawnable));
            }

            if (bossSpawnable != null && !_bossSpawned)
            {
                StartCoroutine(SpawnBoss(bossSpawnable));
            }
        }

        private IEnumerator SpawnEnemy(EnemySpawnable spawnable)
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnable.spawnInterval);

                _activeEnemies[spawnable.prefab].RemoveAll(obj => obj == null);

                if (_totalSpawnedEnemies[spawnable.prefab] >= spawnable.totalSpawnLimit) yield break;

                if (_activeEnemies[spawnable.prefab].Count >= spawnable.maxCount) continue;

                var spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnable.spawnRadius;
                var spawnedObject = Instantiate(spawnable.prefab, spawnPosition, Quaternion.identity);

                _activeEnemies[spawnable.prefab].Add(spawnedObject);
                _totalSpawnedEnemies[spawnable.prefab]++;

                spawnedObject.GetComponent<EnemyHealth>().OnDestroyed += () =>
                {
                    _activeEnemies[spawnable.prefab].Remove(spawnedObject);
                    Destroy(spawnedObject);
                };
            }
        }

        private IEnumerator SpawnBoss(BossSpawnable spawnable)
        {
            // 단일 스폰 조건 확인
            if (spawnable.spawnOnce && _bossSpawned) yield break;

            yield return new WaitForSeconds(10f); // 보스 스폰 대기 시간 (예시)

            var spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnable.spawnRadius;
            var boss = Instantiate(spawnable.bossPrefab, spawnPosition, Quaternion.identity);

            _bossSpawned = true;

            // 보스 체력바 활성화
            var bossBase = boss.GetComponent<BossBase>();
            if (bossBase != null)
            {
                GameManager.Instance.ActivateBoss(bossBase); // GameManager에 보스 등록

                bossBase.OnDestroyed += () =>
                {
                    Debug.Log($"Boss {spawnable.bossPrefab.name} 제거됨");
                    GameManager.Instance.OnBossDefeated(); // 보스 사망 처리
                    Destroy(boss);
                };
            }
            else
            {
                Debug.LogError("BossPrefab is missing a BossBase component.");
            }
        }
    }
}