using UnityEngine;

namespace Core.abstracts
{
    public abstract class BossBase : MonoBehaviour
    {
        [SerializeField] public float maxHealth = 100;
        [SerializeField] protected float phaseChangeHealthPercentage = 0.5f; // 페이즈 전환 체력 비율
        [SerializeField] private float moveSpeed = 1f; // 보스 이동 속도
        [SerializeField] private float detectionRange = 10f; // 추적 범위
        private float _currentHealth; // 현재 체력
        
        public float CurrentHealth // `currentHealth`를 읽기 전용 프로퍼티로 제공
        {
            get => _currentHealth;
            private set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
        
        public delegate void DestroyedAction();
        public event DestroyedAction OnDestroyed;
        
        protected Transform Player; // 플레이어 위치
        public bool isPhaseChanged = false; // 페이즈 전환 여부
        
        protected virtual void Start()
        {
            _currentHealth = maxHealth;
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            OnBossStart();
        }

        protected virtual void Update()
        {
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player")?.transform;
                if (Player == null)
                {
                    Debug.LogError("플레이어를 찾을 수 없습니다.");
                    return;
                }
            }

            FollowPlayer();

            // 체력에 따른 페이즈 전환
            if (!isPhaseChanged && _currentHealth <= maxHealth * phaseChangeHealthPercentage)
            {
                isPhaseChanged = true;
                OnPhaseTransition();
            }
        }
        
        private void FollowPlayer()
        {
            if (Player == null) return;

            var distanceToPlayer = Vector2.Distance(transform.position, Player.position);
            if (distanceToPlayer <= detectionRange)
            {
                Vector2 direction = (Player.position - transform.position).normalized;
                transform.Translate(direction * (moveSpeed * Time.deltaTime));
            }
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            Debug.Log($"{gameObject.name} 데미지 {damage}를 입음. 현재 체력: {CurrentHealth}");
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            Debug.Log($"{gameObject.name} 사망");
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        } 

        protected abstract void OnBossStart(); // 보스 초기화 구현
        protected abstract void OnPhaseTransition(); // 페이즈 전환 구현
    }
}