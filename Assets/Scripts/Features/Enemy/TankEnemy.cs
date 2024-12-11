using System.Collections;
using Core.abstracts;
using UnityEngine;

namespace Features.Enemy
{
    public class TankEnemy : EnemyBase
    {
        [SerializeField] private float shieldDuration = 3f; // 방어막 지속 시간
        [SerializeField] private float shieldCooldown = 10f; // 방어막 재사용 쿨타임
        [SerializeField] private Color enhancedAttackColor;
        [SerializeField] private float effectDuration = 0.5f;
        
        private bool _isShielded; // 방어막 활성화 여부
        private float _shieldTimer; // 방어막 쿨타임 타이머
        
        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();
            _shieldTimer = shieldCooldown;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();

            if (_shieldTimer >= shieldCooldown && !_isShielded)
            {
                StartCoroutine(ActivateShield());
            }

            _shieldTimer += Time.deltaTime;
        }

        // 방어막 활성화 코루틴
        private IEnumerator ActivateShield()
        {
            _isShielded = true;
            Debug.Log("TankEnemy 방어막 활성화!");
            yield return new WaitForSeconds(shieldDuration);
            _isShielded = false;
            _shieldTimer = 0;
            Debug.Log("TankEnemy 방어막 비활성화");
        }

        protected override void AttackPlayer()
        {
            if (_isShielded)
            {
                Debug.Log("TankEnemy 방어막으로 인해 공격 무시");
                return;
            }

            base.AttackPlayer();
            
            if (_spriteRenderer != null)
            {
                StartCoroutine(EnhancedAttackEffect());
            }
        }
        
        private IEnumerator EnhancedAttackEffect()
        {
            var originalColor = _spriteRenderer.color;
            _spriteRenderer.color = enhancedAttackColor;

            yield return new WaitForSeconds(effectDuration);

            _spriteRenderer.color = originalColor;
        }
    }
}