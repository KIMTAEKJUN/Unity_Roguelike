using Core.abstracts;
using Unity.VisualScripting;
using UnityEngine;

namespace Features.Enemies.Bosses
{
    public class Shadow : BossBase
    {
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float teleportCooldown = 4f; // 순간 이동 쿨타임
        [SerializeField] private int cloneCount = 2; // 복제체 개수

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
            Debug.Log("Shadow: 순간 이동 및 복제!");
            
            // Shadow 보스를 무작위 위치로 순간 이동
            var randomPosition = (Vector2)transform.position + Random.insideUnitCircle * 5f;
            transform.position = randomPosition;

            // 복제체 생성
            if (clonePrefab == null)
            {
                Debug.LogWarning("Shadow: 복제체 프리팹이 설정되지 않았습니다!");
                return;
            }

            for (var i = 0; i < cloneCount; i++)
            {
                var clonePosition = randomPosition + Random.insideUnitCircle * 1.5f;
                var clone = Instantiate(clonePrefab, clonePosition, Quaternion.identity);
        
                // 복제체를 플레이어가 구분할 수 있도록 색상 변경
                var spriteRenderer = clone.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.gray; // 복제체의 색상 설정
                }

                Debug.Log("Shadow: 복제체 생성");
            }
        }
        
        public override void OnBossStart()
        {
            Debug.Log("Shadow: 전투 시작과 함께 그림자 복제 활성화!");
            cloneCount = 1; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            cloneCount += 2;
            Debug.Log("Shadow: 페이즈 2 전환 - 복제체 증가!");
            // 추후 추가 전환 로직
        }
    }
}