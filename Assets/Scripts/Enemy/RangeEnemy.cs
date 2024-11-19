using Enemy.Controller;
using UnityEngine;

namespace Enemy
{
    public class RangeEnemy : EnemyController
    {
        [SerializeField] private GameObject slowProjectilePrefab;

        // protected override void Update()
        // {
        //     base.Update(); // 부모 클래스의 Update 호출 (기본 이동 및 공격 로직 유지)
        // }

        protected override void AttackPlayer()
        {
            if (slowProjectilePrefab == null || Player == null) return;

            // 발사체 생성
            var projectile = Instantiate(slowProjectilePrefab, transform.position, Quaternion.identity);

            // 발사체의 Rigidbody2D를 사용하여 플레이어를 향해 이동시킴
            var direction = (Player.position - transform.position).normalized;
            var projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * 10f; // 발사체 속도 조절
            }

            Debug.Log("RangedEnemy: 느려지는 발사체 발사!");
        }
    }
}