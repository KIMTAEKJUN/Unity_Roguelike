using UnityEngine;

namespace Enemy.Controller
{
    public class EnemyController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float detectionRange = 8f;
        private Transform _player;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer <= detectionRange)
            {
                Vector2 direction = (_player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
    }
    
}