using Core.abstracts;
using UnityEngine;

namespace Features.Enemies.Bosses
{
    public class Shadow : BossBase
    {
        [SerializeField] private float teleportCooldown = 4f; // 순간 이동 쿨타임
        [SerializeField] private float teleportRange = 5f; // 순간 이동 범위

        private float _teleportTimer;

        protected override void Update()
        {
            base.Update();

            _teleportTimer += Time.deltaTime;
            if (_teleportTimer < teleportCooldown) return;

            PerformTeleport();
            _teleportTimer = 0f;
        }

        private void PerformTeleport()
        {
            Debug.Log("Shadow: 순간 이동!");

            // Shadow 보스를 무작위 위치로 순간 이동
            var randomPosition = (Vector2)transform.position + Random.insideUnitCircle * teleportRange;
            transform.position = randomPosition;

            Debug.Log($"Shadow: {randomPosition} 위치로 순간 이동 완료!");
        }

        public override void OnBossStart()
        {
            Debug.Log("Shadow: 전투 시작과 함께 순간 이동 활성화!");
            teleportCooldown = 4f;
        }

        protected override void OnPhaseTransition()
        {
            teleportCooldown -= 1f; // 페이즈 전환 시 순간 이동 주기 단축
            Debug.Log("Shadow: 페이즈 2 전환 - 순간 이동 속도 증가!");
        }
    }
}