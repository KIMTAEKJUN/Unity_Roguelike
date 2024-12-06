using Features.Player;
using UnityEngine;

namespace Features.Projectiles.EnemyProjectiles
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f; // 발사체 속도
        [SerializeField] private int damage = 1; // 발사체 데미지
        private Vector2 _direction; // 이동 방향

        public void SetDirection(Vector2 direction)
        {
            _direction = direction.normalized;
        }

        private void Update()
        {
            // 발사체 이동
            transform.Translate(_direction * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 플레이어에게만 데미지를 입힘
            if (collision.CompareTag("Player"))
            {
                var playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                Destroy(gameObject); // 충돌 후 발사체 파괴
            }
        }
    }    
}