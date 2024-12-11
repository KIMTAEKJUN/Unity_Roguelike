using Core.abstracts;
using Features.Player.components;
using UnityEngine;

namespace Features.Boss
{
    public class FireBeast : BossBase
    {
        [SerializeField] private float fireCooldown = 4f; // 불길 생성 주기
        [SerializeField] private float fireDamage = 15f; // 불길 데미지
        [SerializeField] private float fireRange = 3f; // 불길 범위

        private float _fireTimer; // 불길 생성 타이머
        private readonly Collider2D[] _targetsBuffer = new Collider2D[20]; // 충돌 처리 버퍼

        protected override void Update()
        {
            base.Update();

            _fireTimer += Time.deltaTime;
            if (_fireTimer < fireCooldown) return;

            CreateFire();
            _fireTimer = 0f;
        }

        // 불길 생성 스킬
        private void CreateFire()
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, fireRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];
                if (target.CompareTag("Player"))
                {
                    target.GetComponent<PlayerStats>()?.TakeDamage(fireDamage);
                }
            }

            Debug.Log($"FireBeast: {hitCount}개 대상에게 불길 데미지 적용");
        }
        
        protected override void OnBossStart()
        {
            fireCooldown *= 0.9f;
        }

        protected override void OnPhaseTransition()
        {
            Debug.Log("FireBeast: 페이즈 2 전환 - 불길 이동 속도 증가!");
            fireCooldown -= 1f;
        }
    }
}