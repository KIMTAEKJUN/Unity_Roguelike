using UnityEngine;

namespace Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float attackRange = 10f;
        public float attackCooldown = 1f;
        public GameObject projectilePrefab;

        private float _attackTimer = 0f;

        private void Update()
        {
            Move();
            Attack();
        }

        private void Move()
        {
            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");
            var movement = new Vector2(moveX, moveY).normalized;
            transform.Translate(movement * (moveSpeed * Time.deltaTime));
        }

        private void Attack()
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer < attackCooldown)
                return;

            _attackTimer = 0f;

            var target = FindClosestEnemy();
            if (target == null) return;

            var direction = (target.transform.position - transform.position).normalized;

            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null) projectileRb.velocity = direction * 10f;
        }

        private GameObject FindClosestEnemy()
        {
            GameObject closestEnemy = null;
            var closestDistance = attackRange; // 공격 범위를 기준으로 초기 거리 설정

            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                var distance = Vector2.Distance(transform.position, enemy.transform.position);

                // 공격 범위를 초과하는 경우 스킵
                if (distance >= closestDistance)
                    continue;

                closestEnemy = enemy;
                closestDistance = distance;
            }

            return closestEnemy;
        }
    }
}
