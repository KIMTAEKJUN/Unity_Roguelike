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

        void Update()
        {
            Move();
            Attack();
        }

        void Move()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(moveX, moveY).normalized;
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }

        void Attack()
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer >= attackCooldown)
            {
                GameObject target = FindClosestEnemy();
                if (target != null)
                {
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
                }

                _attackTimer = 0f;
            }
        }

        private GameObject FindClosestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float closestDistance = attackRange;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            return closestEnemy;
        }
    }
}
