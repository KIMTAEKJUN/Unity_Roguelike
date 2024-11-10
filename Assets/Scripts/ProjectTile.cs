using Enemy;
using UnityEngine;

namespace DefaultNamespace
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10f;     // 발사체 속도
        public int damage = 1;        // 발사체가 줄 데미지

        private Vector2 direction;    // 발사체 이동 방향

        // 발사체가 이동할 방향을 설정하는 함수
        public void SetDirection(Vector2 dir)
        {
            direction = dir.normalized;
        }

        void Update()
        {
            // 발사체 이동
            transform.Translate(direction * speed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            // 적과 충돌하면 적에게 데미지를 입히고 발사체 파괴
            if (collision.CompareTag("Enemy"))
            {
                // 적의 Health 컴포넌트를 가져와 데미지를 입힘 (적 스크립트에 맞춰 구현 필요)
                collision.GetComponent<EnemyHealth>().TakeDamage(damage);
                Destroy(gameObject); // 발사체 파괴
            }
        }
    }
}