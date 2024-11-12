using System.Collections;
using System.Collections.Generic;
using Enemy;
using Manager.Entity;
using UnityEngine;
using Pattern;
using Random = UnityEngine.Random;

namespace Manager
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public List<Spawnable> spawnables = new List<Spawnable>();

        private readonly Dictionary<GameObject, List<GameObject>> _activeObjects =
            new Dictionary<GameObject, List<GameObject>>();

        private readonly Dictionary<GameObject, int> _totalSpawnedCount = new Dictionary<GameObject, int>();

        private void Start()
        {
            foreach (var spawnable in spawnables)
            {
                _activeObjects[spawnable.prefab] = new List<GameObject>();
                _totalSpawnedCount[spawnable.prefab] = 0;
                StartCoroutine(SpawnObject(spawnable));
            }
        }

        private IEnumerator SpawnObject(Spawnable spawnable)
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnable.spawnInterval);

                // 파괴된 오브젝트(null) 제거
                _activeObjects[spawnable.prefab].RemoveAll(obj => obj == null);

                // 총 스폰 제한에 도달한 경우 코루틴 종료
                if (_totalSpawnedCount[spawnable.prefab] >= spawnable.totalSpawnLimit)
                {
                    yield break;
                }

                // 현재 활성화된 오브젝트 수가 maxCount 이상이면 스폰하지 않고 루프 유지
                if (_activeObjects[spawnable.prefab].Count >= spawnable.maxCount)
                {
                    continue;
                }

                // 스폰 위치와 새로운 오브젝트 생성
                var spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnable.spawnRadius;
                var spawnedObject = Instantiate(spawnable.prefab, spawnPosition, Quaternion.identity);
                _activeObjects[spawnable.prefab].Add(spawnedObject);
                _totalSpawnedCount[spawnable.prefab]++;

                // 오브젝트가 파괴될 때 리스트에서 제거
                spawnedObject.GetComponent<EnemyHealth>().OnDestroyed += () =>
                {
                    _activeObjects[spawnable.prefab].Remove(spawnedObject);
                    Destroy(spawnedObject);
                };
            }
        }
    }
}