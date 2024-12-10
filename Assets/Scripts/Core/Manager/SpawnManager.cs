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
        public List<BossSpawnable> bossSpawnables = new List<BossSpawnable>();

        private readonly Dictionary<GameObject, List<GameObject>> _activeEnemies =
            new Dictionary<GameObject, List<GameObject>>();

        private readonly Dictionary<GameObject, int> _totalSpawnedEnemies =
            new Dictionary<GameObject, int>();
        
        private readonly List<GameObject> _activeBosses = new List<GameObject>();

        private bool _bossSpawned; // 보스 스폰 여부

        private void Start()
        {
            foreach (var enemySpawnable in enemySpawnables)
            {
                _activeEnemies[enemySpawnable.prefab] = new List<GameObject>();
                _totalSpawnedEnemies[enemySpawnable.prefab] = 0;
                StartCoroutine(SpawnEnemy(enemySpawnable));
            }

            foreach (var bossSpawnable in bossSpawnables)
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
            if (spawnable.spawnOnce && _bossSpawned) yield break;

            yield return new WaitForSeconds(10f); // 보스 스폰 대기 시간

            var spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnable.spawnRadius;
            var boss = Instantiate(spawnable.bossPrefab, spawnPosition, Quaternion.identity);

            _bossSpawned = true;
            _activeBosses.Add(boss);
            
            var bossBase = boss.GetComponent<BossBase>();
            if (bossBase != null)
            {
                GameManager.Instance.ActivateBoss(bossBase);

                bossBase.OnDestroyed += () =>
                {
                    Debug.Log($"보스 {spawnable.bossPrefab.name} 제거됨");
                    _activeBosses.Remove(boss);
                    GameManager.Instance.OnBossDefeated();

                    if (_activeBosses.Count == 0) // 모든 보스 처치
                    {
                        Debug.Log("모든 보스 처치 완료! 다음 레벨로 이동.");
                        GameManager.Instance.LoadNextLevel();
                    }

                    Destroy(boss);
                };
            }
            else
            {
                Debug.LogError("보스 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }
}