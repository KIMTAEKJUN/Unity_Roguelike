using Core.abstracts;
using UnityEngine;

namespace Features.Enemies.Types
{
    public class RangeEnemy : EnemyController
    {
        [SerializeField] private GameObject freezeProjectilePrefab;

        protected override void AttackPlayer()
        {
            if (freezeProjectilePrefab == null || Player == null)
            {
                Debug.LogWarning("발사체 프리팹이나 플레이어가 없습니다.");
                return;
            }

            var projectile = Instantiate(freezeProjectilePrefab, transform.position, Quaternion.identity);

            var direction = (Player.position - transform.position).normalized;
            var projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * 10f; // 발사체 속도 조절
                Debug.Log("발사체가 플레이어를 향해 발사되었습니다.");
            }
            else
            {
                Debug.LogWarning("발사체에 Rigidbody2D가 없습니다.");
            }
        }
    }
}