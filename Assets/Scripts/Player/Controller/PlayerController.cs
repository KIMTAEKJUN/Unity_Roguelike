using System.Collections;
using UnityEngine;

namespace Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f; // 이동 속도 설정
        [SerializeField] private float attackRange = 10f; // 공격 범위
        [SerializeField] private float attackCooldown = 1f; // 공격 쿨다운
        private float _originalMoveSpeed; // 원래 이동 속도
        private bool _isSlowed; // 이동 속도 감소 중인지 여부
        public GameObject projectilePrefab;

        private float _attackTimer = 0f; // 공격 쿨다운 타이머
        private bool _isFrozen = false; // 플레이어가 얼어있는지 여부

        
        private void Update()
        {
            if (_isFrozen)
            {
                Debug.Log("Player is frozen! Cannot move.");
                return;
            }
            Move();
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
        
        public void ApplyFreeze(float duration)
        {
            if (_isFrozen)
            {
                Debug.Log("Player already frozen!");
                return;
            }

            Debug.Log($"Player frozen for {duration} seconds!");
            _isFrozen = true;
            Invoke(nameof(Unfreeze), duration);
        }

        private void Unfreeze()
        {
            _isFrozen = false; // 멈춤 상태 해제
        }
    }
}
