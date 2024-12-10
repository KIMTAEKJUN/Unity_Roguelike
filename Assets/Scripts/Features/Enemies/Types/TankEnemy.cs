using System.Collections;
using Core.abstracts;
using UnityEngine;

namespace Features.Enemies.Types
{
    public class TankEnemy : EnemyBase
    {
        [SerializeField] private float shieldDuration = 3f;
        [SerializeField] private float shieldCooldown = 10f;
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
            _shieldTimer += Time.deltaTime;

            if (_shieldTimer >= shieldCooldown && !_isShielded)
            {
                StartCoroutine(ActivateShield());
            }
        }

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
            
            // 색상 이펙트 코루틴 실행
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