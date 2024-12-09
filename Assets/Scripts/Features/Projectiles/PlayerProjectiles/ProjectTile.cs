using Core.abstracts;
using Features.Enemies.components;
using UnityEngine;

namespace Features.Projectiles.PlayerProjectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f; // 발사체 속도
        [SerializeField] private int damage = 1; // 발사체 데미지
        private Vector2 _direction; // 발사체 이동 방향
    
        public void SetDirection(Vector2 direction)
        {
            _direction = direction.normalized;
        }
    
        private void Start()
        {
            // 발사체 생성 시 플레이어를 향하는 방향으로 설정
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _direction = (player.transform.position - transform.position).normalized;   
            }
        }

        private void Update()
        {
            // 발사체 이동
            transform.Translate(_direction * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 적 또는 보스에 데미지
            if (collision.CompareTag("Enemy"))
            {
                var enemyHealth = collision.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log($"{collision.name}에게 {damage} 데미지를 입힘 (Enemy)");
                }
                
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Boss"))
            {
                var bossBase = collision.GetComponent<BossBase>();
                if (bossBase != null)
                {
                    bossBase.TakeDamage(damage);
                    Debug.Log($"{collision.name}에게 {damage} 데미지를 입힘 (Boss)");
                }
                
                Destroy(gameObject);
            }
        }
    }    
}