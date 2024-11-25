using Core.abstracts;
using Features.Enemies.components;
using UnityEngine;

namespace Features.Enemies.Bosses
{
    public class Priest : BossBase
    {
        [SerializeField] private float healAmount = 10f; // 회복량
        [SerializeField] private float healCooldown = 5f; // 회복 주기

        private float _healTimer; // 회복 타이머

        protected override void Update()
        {
            base.Update();

            _healTimer += Time.deltaTime;
            if (_healTimer < healCooldown) return;

            Heal();
            _healTimer = 0f;
        }

        private void Heal()
        {
            Debug.Log("Priest: 체력 회복!");
            
            // 힐 범위 내 아군 적만 힐
            var allies = new Collider2D[10]; // 버퍼 생성
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, 5f, allies);

            for (var i = 0; i < count; i++)
            {
                var ally = allies[i];
                if (ally.CompareTag("Enemy"))
                {
                    var allyHealth = ally.GetComponent<EnemyHealth>();
                    if (allyHealth != null)
                    {
                        allyHealth.TakeDamage(-healAmount); // 힐량은 음수로 처리
                
                        // 힐링 이펙트 추가
                        // var healEffect = Instantiate(healEffectPrefab, ally.transform.position, Quaternion.identity);
                        // Destroy(healEffect, 2f); // 2초 뒤 힐 이펙트 제거
                    }
                }
            }
        }
        
        protected override void OnBossStart()
        {
            Debug.Log("Priest: 전투 시작과 함께 성스러운 오라 활성화!");
            healCooldown *= 1.1f; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            healCooldown -= 1f; // 회복 주기 단축
            Debug.Log("Priest: 페이즈 2 전환 - 회복 속도 증가!");
        }
    }
}