using Player;
using UnityEngine;

namespace Enemy.Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f; // 이동 속도 설정
        [SerializeField] protected float detectionRange = 8f; // 감지 범위
        [SerializeField] protected float attackRange = 1.5f; // 플레이어를 공격하는 범위
        [SerializeField] protected int damage = 1; // 적이 입히는 데미지
        [SerializeField] protected float attackCooldown = 1f; // 공격 쿨다운
        protected Transform Player; // 플레이어 위치
        private float _attackTimer; // 공격 쿨다운 타이머
        

        protected virtual void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected virtual void Update()
        {
            _attackTimer += Time.deltaTime;

            if (Player == null) return;

            var distanceToPlayer = Vector2.Distance(transform.position, Player.position);

            if (distanceToPlayer > detectionRange) return;

            var direction = (Player.position - transform.position).normalized;

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

        protected virtual void AttackPlayer()
        {
            var playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}