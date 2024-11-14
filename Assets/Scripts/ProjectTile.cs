using Enemy;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 발사체 속도
    [SerializeField] private int damage = 1; // 발사체 데미지
    private Vector2 _direction; // 발사체 이동 방향
    
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
        // 충돌한 대상이 적이 아닌 경우 반환
        if (!collision.CompareTag("Enemy")) return;

        // 적의 체력 컴포넌트를 가져와 데미지를 입힘
        var enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        // 적의 체력에 데미지를 가하고 발사체 파괴
        enemyHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}