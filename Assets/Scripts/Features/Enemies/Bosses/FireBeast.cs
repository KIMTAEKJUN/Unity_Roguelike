using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.Bosses
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

        private void CreateFire()
        {
            Debug.Log("FireBeast: 불길 생성!");
            
            // 불길을 생성하여 플레이어와 적에게 지속 데미지를 입힘
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, fireRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];

                if (target.CompareTag("Player"))
                {
                    var playerHealth = target.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(fireDamage);
                    }
                }
            }

            Debug.Log($"{hitCount}개 대상에게 불길 데미지 적용");
        }
        
        public override void OnBossStart()
        {
            Debug.Log("FireBeast: 전투 시작과 함께 불길 보호막 활성화!");
            fireCooldown *= 0.9f; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            fireCooldown -= 1f;
            Debug.Log("FireBeast: 페이즈 2 전환 - 불길 이동 속도 증가!");
            // 추후 추가 전환 로직
        }
    }
}