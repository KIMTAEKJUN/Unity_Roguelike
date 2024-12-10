using Features.Player;
using UnityEngine;

namespace Features.Projectiles.EnemyProjectiles
{
    public class FreezeProjectile : MonoBehaviour
    {
        public float freezeDuration = 5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("FreezeProjectile 충돌 감지!");
            if (!collision.CompareTag("Player"))
            {
                Debug.Log("플레이어가 아닙니다.");
                return;
            }

            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyFreeze(freezeDuration);
                Debug.Log($"플레이어 얼림! {freezeDuration}초 동안 멈춥니다.");
            }
            else
            {
                Debug.LogWarning("PlayerController를 찾을 수 없습니다.");
            }

            Destroy(gameObject); // 발사체 파괴
        }
    }    
}