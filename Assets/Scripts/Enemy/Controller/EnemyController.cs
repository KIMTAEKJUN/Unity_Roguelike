using Player;
using UnityEngine;

namespace Enemy.Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f; // Inspector에서 이동 속도 설정
        [SerializeField] private float detectionRange = 8f; // 감지 범위
        [SerializeField] private float attackRange = 1.5f; // 플레이어를 공격하는 범위
        [SerializeField] private int damage = 1; // 적이 입히는 데미지
        [SerializeField] private float attackCooldown = 1f; // 공격 쿨다운
        private Transform _player;
        private float _attackTimer;
        

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            _attackTimer += Time.deltaTime;

            if (_player == null) return;

            var distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer > detectionRange) return;

            var direction = (_player.position - transform.position).normalized;

            // 플레이어가 공격 범위 내에 있을 때
            if (distanceToPlayer <= attackRange && _attackTimer >= attackCooldown)
            {
                AttackPlayer();
                _attackTimer = 0f;
            }
            else if (distanceToPlayer > attackRange)
            {
                transform.Translate(direction * (Time.deltaTime * moveSpeed));
            }
        }

        private void AttackPlayer()
        {
            var playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}