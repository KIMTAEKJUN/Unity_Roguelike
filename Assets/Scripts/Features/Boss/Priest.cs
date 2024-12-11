using Core.abstracts;
using Features.Enemy.components;
using UnityEngine;

namespace Features.Boss
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

        // 힐 스킬
        private void Heal()
        {
            var allies = new Collider2D[10];
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, 5f, allies);

            for (var i = 0; i < count; i++)
            {
                var ally = allies[i];
                if (ally == null || !ally.CompareTag("Enemy")) continue;
                
                var allyHealth = ally.GetComponent<EnemyHealth>();
                allyHealth?.TakeDamage(-healAmount); // 힐량은 음수로 처리
            }

            Debug.Log($"{count}개의 아군이 힐을 받았습니다.");
        }
        
        protected override void OnBossStart()
        {
            healCooldown *= 1.1f;
        }

        protected override void OnPhaseTransition()
        {
            healCooldown -= 1f;
            Debug.Log("Priest: 페이즈 2 전환 - 회복 속도 증가!");
        }
    }
}