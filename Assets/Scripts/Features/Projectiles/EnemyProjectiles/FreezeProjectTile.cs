using Features.Player;
using UnityEngine;

namespace Features.Projectiles.EnemyProjectiles
{
    public class FreezeProjectile : MonoBehaviour
    {
        public float freezeDuration = 5f; // 멈추는 시간

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("FreezeProjectile 충돌 감지!"); // 충돌 감지 로그
            if (!collision.CompareTag("Player"))
            {
                Debug.Log("플레이어가 아닙니다.");
                return;
            }

            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyFreeze(freezeDuration);
                Debug.Log($"Player frozen for {freezeDuration} seconds!");
            }
            else
            {
                Debug.LogWarning("PlayerController를 찾을 수 없습니다.");
            }

            Destroy(gameObject); // 발사체 파괴
        }
    }    
}