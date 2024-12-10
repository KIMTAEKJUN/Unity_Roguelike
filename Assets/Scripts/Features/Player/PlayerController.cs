using Features.Player.components;
using Features.Projectiles.PlayerProjectiles;
using UnityEngine;

namespace Features.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject projectilePrefab;
        
        private PlayerStats _playerStats;
        private float _attackTimer = 0f; // 공격 쿨다운 타이머
        private bool _isFrozen = false; // 플레이어가 얼어있는지 여부
        
        private void Start()
        {
            _playerStats = GetComponent<PlayerStats>();
            if (_playerStats == null)
            {
                Debug.LogError("PlayerStats 컴포넌트를 찾을 수 없습니다.");
            }
        }

        private void Update()
        {
            if (_isFrozen)
            {
                Debug.Log("플레이어가 얼어 이동할 수 없습니다.");
                return;
            }

            Move();
            Attack();
        }

        private void Move()
        {
            if (_playerStats == null) return;
            
            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");
            var movement = new Vector2(moveX, moveY).normalized;
            
            transform.Translate(movement * (_playerStats.GetMoveSpeed() * Time.deltaTime));
        }

        private void Attack()
        {
            if (_playerStats == null) return;

            _attackTimer += Time.deltaTime;

            if (_attackTimer < _playerStats.GetAttackCooldown()) return;

            _attackTimer = 0f;

            var target = FindClosestEnemy(_playerStats.GetAttackRange());
            if (target == null) return;

            var direction = (target.transform.position - transform.position).normalized;

            var projectileCount = _playerStats.GetProjectileCount();
            FireProjectiles(direction, projectileCount);
        }

        private void FireProjectiles(Vector2 baseDirection, int projectileCount)
        {
            var angleStep = 10f; // 각도 간격 설정
            var angle = -((projectileCount - 1) * angleStep) / 2; // 첫 발사체 각도 설정

            for (int i = 0; i < projectileCount; i++)
            {
                var projectileDirection = Quaternion.Euler(0, 0, angle) * baseDirection;
                var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                var projectileRb = projectile.GetComponent<Rigidbody2D>();
                var projectileScript = projectile.GetComponent<Projectile>();
                
                if (projectileRb != null)
                {
                    projectileRb.velocity = projectileDirection * 10f;
                }

                if (projectileScript != null)
                {
                    projectileScript.SetDamage(_playerStats.GetAttackDamage()); // 플레이어 공격력 설정
                }

                angle += angleStep; // 다음 발사체 각도 계산
            }
        }

        private GameObject FindClosestEnemy(float range)
        {
            GameObject closestTarget = null;
            var closestDistance = range;

            // 적 탐색
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                var distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestTarget = enemy;
                    closestDistance = distance;
                }
            }

            // 보스 탐색
            foreach (var boss in GameObject.FindGameObjectsWithTag("Boss"))
            {
                var distance = Vector2.Distance(transform.position, boss.transform.position);
                if (distance < closestDistance)
                {
                    closestTarget = boss;
                    closestDistance = distance;
                }
            }

            return closestTarget;
        }

        public void ApplyFreeze(float duration)
        {
            if (_isFrozen)
            {
                Debug.Log("플레이어가 이미 얼어있습니다.");
                return;
            }

            Debug.Log($"플레이어가 {duration}초 동안 얼어있습니다.");
            _isFrozen = true;
            Invoke(nameof(Unfreeze), duration);
        }

        private void Unfreeze()
        {
            _isFrozen = false;
        }
    }
}
