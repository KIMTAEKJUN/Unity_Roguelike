using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.abstracts;
using Core.Manager.Entity;
using UnityEngine;
using Core.Pattern;
using Features.Enemy.components;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Core.Manager
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public List<EnemySpawnable> enemySpawnables = new List<EnemySpawnable>(); // 적 스폰 목록
        public List<BossSpawnable> bossSpawnables = new List<BossSpawnable>(); // 보스 스폰 목록

        private readonly Dictionary<GameObject, List<GameObject>> _activeEnemies =
            new Dictionary<GameObject, List<GameObject>>(); // 활성화된 적 목록

        private readonly Dictionary<GameObject, int> _totalSpawnedEnemies =
            new Dictionary<GameObject, int>(); // 총 스폰된 적 수
        
        private readonly List<GameObject> _activeBosses = new List<GameObject>(); // 활성화된 보스 목록

        private bool _bossSpawned; // 보스 스폰 여부
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            InitializeSpawner();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"씬 로드됨: {scene.name}");

            ResetSpawnerData();
            ConfigureSpawnablesForScene();
            InitializeSpawner();
        }

        // 스폰 데이터 초기화
        private void ResetSpawnerData()
        {
            _activeEnemies.Clear();
            _totalSpawnedEnemies.Clear();
            _activeBosses.Clear();
            _bossSpawned = false;
        }

        // 씬에 따라 스폰
        private void ConfigureSpawnablesForScene()
        {
            bossSpawnables = SceneManager.GetActiveScene().name switch
            {
                "Level1GameScene" => GetLevel1Bosses(),
                "Level2GameScene" => GetLevel2Bosses(),
                "Level3GameScene" => GetLevel3Bosses(),
                "Level4GameScene" => GetLevel4Bosses(),
                _ => new List<BossSpawnable>()
            };

            Debug.Log($"씬 보스 설정 완료: {bossSpawnables.Count}개 보스 로드됨");
        }

        // 스포너 초기화
        private void InitializeSpawner()
        {
            StopAllCoroutines();
            ResetSpawnerData();
            
            // 활성 적 딕셔너리 초기화
            foreach (var spawnable in enemySpawnables.Where(spawnable => !_activeEnemies.ContainsKey(spawnable.prefab)))
            {
                _activeEnemies[spawnable.prefab] = new List<GameObject>();
                _totalSpawnedEnemies[spawnable.prefab] = 0;
            }
            
            foreach (var spawnable in enemySpawnables)
            {
                StartCoroutine(SpawnEnemy(spawnable));
            }

            foreach (var spawnable in bossSpawnables)
            {
                StartCoroutine(SpawnBoss(spawnable));
            }
        }

        // 적 스폰
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
        
        // 보스 스폰
        private IEnumerator SpawnBoss(BossSpawnable spawnable)
        {
            if (spawnable.spawnOnce && _bossSpawned) yield break;

            yield return new WaitForSeconds(10f);

            var spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnable.spawnRadius;
            var boss = Instantiate(spawnable.bossPrefab, spawnPosition, Quaternion.identity);

            // 보스를 리스트에 추가
            _activeBosses.Add(boss);

            var bossBase = boss.GetComponent<BossBase>();
            if (bossBase != null)
            {
                GameManager.Instance.ActivateBoss(bossBase);

                bossBase.OnDestroyed += () =>
                {
                    Debug.Log($"보스 {spawnable.bossPrefab.name} 제거됨");
                    _activeBosses.Remove(boss);
                    CheckRemainingBosses();
                    GameManager.Instance.OnBossDefeated();
                    Destroy(boss);
                };
            }
            else
            {
                Debug.LogError("보스 컴포넌트를 찾을 수 없습니다.");
            }
        }

        // 남은 보스 확인
        private void CheckRemainingBosses()
        {
            if (_activeBosses.Count == 0)
            {
                Debug.Log("모든 보스를 제거했습니다!");
                GameManager.Instance.LoadNextLevel();
            }
            else
            {
                Debug.Log($"남은 보스 수: {_activeBosses.Count}");
            }
        }

        private List<BossSpawnable> GetLevel1Bosses()
        {
            return new List<BossSpawnable>
            {
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("Destroyer"),
                    spawnRadius = 8f,
                    spawnOnce = true
                },
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("FireBeast"),
                    spawnRadius = 8f,
                    spawnOnce = true
                }
            };
        }

        private List<BossSpawnable> GetLevel2Bosses()
        {
            return new List<BossSpawnable>
            {
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("IceQueen"),
                    spawnRadius = 8f,
                    spawnOnce = true
                },
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("Priest"),
                    spawnRadius = 8f,
                    spawnOnce = true
                }
            };
        }

        private List<BossSpawnable> GetLevel3Bosses()
        {
            return new List<BossSpawnable>
            {
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("Shadow"),
                    spawnRadius = 8f,
                    spawnOnce = true
                }
            };
        }
        
        private List<BossSpawnable> GetLevel4Bosses()
        {
            return new List<BossSpawnable>
            {
                new BossSpawnable
                {
                    bossPrefab = GetBossFromAssets("Storm"),
                    spawnRadius = 8f,
                    spawnOnce = true
                }
            };
        }
        
        private GameObject GetBossFromAssets(string bossName)
        {
            var bossPrefab = Resources.Load<GameObject>($"Boss/{bossName}");
            if (bossPrefab == null)
            {
                Debug.LogError($"Boss 프리팹 {bossName}을(를) 찾을 수 없습니다!");
            }
            return bossPrefab;
        }
    }
}