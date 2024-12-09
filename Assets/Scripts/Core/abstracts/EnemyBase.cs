using System.Collections;
using Features.Player;
using UnityEngine;

namespace Core.abstracts
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f; // 이동 속도 설정
        [SerializeField] protected float detectionRange = 8f; // 감지 범위
        [SerializeField] protected float attackRange = 1.5f; // 플레이어를 공격하는 범위
        [SerializeField] protected int damage = 1; // 적이 입히는 데미지
        [SerializeField] protected float attackCooldown = 1f; // 공격 쿨다운
        
        protected Transform Player; // 플레이어 위치
        private float _attackTimer; // 공격 쿨다운 타이머
        
        private bool _isFrozen; // 적이 얼어있는지 여부
        private float _originalMoveSpeed; // 원래 이동 속도 저장

        protected virtual void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            _originalMoveSpeed = moveSpeed;
        }

        protected virtual void Update()
        {
            if (_isFrozen) return; // 얼어있으면 이동 및 공격 불가
            
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
        
        public void ApplyFreeze(float duration)
        {
            if (_isFrozen) return; // 이미 얼어있는 상태라면 중복 방지
            StartCoroutine(FreezeCoroutine(duration));
        }

        private IEnumerator FreezeCoroutine(float duration)
        {
            Debug.Log($"{gameObject.name} 얼음 효과 시작: {duration}초 동안 이동 정지");
            _isFrozen = true;
            moveSpeed = 0; // 이동 속도 0으로 설정
            yield return new WaitForSeconds(duration);
            _isFrozen = false;
            moveSpeed = _originalMoveSpeed; // 원래 이동 속도로 복원
            Debug.Log($"{gameObject.name} 얼음 효과 종료");
        }
    }
}