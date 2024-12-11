using Core.abstracts;
using UnityEngine;

namespace Features.Boss
{
    public class Shadow : BossBase
    {
        [SerializeField] private float teleportCooldown = 4f; // 순간 이동 쿨타임
        [SerializeField] private float teleportRange = 5f; // 순간 이동 범위

        private float _teleportTimer; // 순간 이동 타이머

        protected override void Update()
        {
            base.Update();

            _teleportTimer += Time.deltaTime;
            if (_teleportTimer < teleportCooldown) return;

            PerformTeleport();
            _teleportTimer = 0f;
        }

        // 순간 이동 스킬
        private void PerformTeleport()
        {
            // Shadow 보스를 무작위 위치로 순간 이동
            var randomPosition = (Vector2)transform.position + Random.insideUnitCircle * teleportRange;
            transform.position = randomPosition;

            Debug.Log($"Shadow: {randomPosition} 위치로 순간 이동 완료!");
        }

        protected override void OnBossStart()
        {
            teleportCooldown = 4f;
        }

        protected override void OnPhaseTransition()
        {
            teleportCooldown -= 1f;
            Debug.Log("Shadow: 페이즈 2 전환 - 순간 이동 속도 증가!");
        }
    }
}