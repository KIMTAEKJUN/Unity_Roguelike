using System.Collections;
using Features.Player.components;
using UnityEngine;

namespace Core.abstracts
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 3f; // 이동 속도
        [SerializeField] protected float detectionRange = 8f; // 플레이어 감지 범위
        [SerializeField] protected float attackRange = 1.5f; // 공격 범위
        [SerializeField] protected int damage = 1; // 적의 공격력
        [SerializeField] protected float attackCooldown = 1f; // 공격 쿨다운
        
        protected Transform Player; // 플레이어 위치
        private float _attackTimer; // 공격 타이머

        private bool _isFrozen; // 적이 얼어있는 상태인지 확인
        private float _originalMoveSpeed; // 원래 이동 속도 저장

        // 적 초기화
        protected virtual void Start()
        {
            FindPlayer();
            _originalMoveSpeed = moveSpeed;
        }
        
        protected virtual void Update()
        {
            if (_isFrozen || Player == null) return; // 얼어있거나 플레이어가 없으면 중지

            _attackTimer += Time.deltaTime;
            HandlePlayerInteraction();
        }

        // 플레이어를 찾는 메서드
        private void FindPlayer()
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (Player == null)
            {
                Debug.LogWarning("플레이어를 찾을 수 없습니다.");
            }
        }

        // 플레이어와의 거리 계산 및 상호작용 처리
        private void HandlePlayerInteraction()
        {
            var distanceToPlayer = Vector2.Distance(transform.position, Player.position);

            if (distanceToPlayer > detectionRange) return; // 감지 범위 밖이면 중지

            if (distanceToPlayer <= attackRange && _attackTimer >= attackCooldown)
            {
                AttackPlayer();
                _attackTimer = 0f;
            }
            else if (distanceToPlayer > attackRange)
            {
                FollowPlayer();
            }
        }

        // 플레이어를 따라가는 로직
        private void FollowPlayer()
        {
            var direction = (Player.position - transform.position).normalized;
            transform.Translate(direction * (Time.deltaTime * moveSpeed));
        }

        // 플레이어를 공격하는 로직
        protected virtual void AttackPlayer()
        {
            var playerHealth = Player.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // 적이 얼음 효과를 받을 때 호출
        public void ApplyFreeze(float duration)
        {
            if (_isFrozen) return; // 이미 얼어있는 상태면 중지
            StartCoroutine(FreezeCoroutine(duration));
        }

        // 얼음 효과를 처리하는 코루틴
        private IEnumerator FreezeCoroutine(float duration)
        {
            _isFrozen = true;
            moveSpeed = 0; // 이동 속도 0으로 설정
            yield return new WaitForSeconds(duration);
            _isFrozen = false;
            moveSpeed = _originalMoveSpeed; // 원래 이동 속도로 복원
        }
    }
}