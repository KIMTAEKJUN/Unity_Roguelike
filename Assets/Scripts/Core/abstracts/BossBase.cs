using UnityEngine;

namespace Core.abstracts
{
    public abstract class BossBase : MonoBehaviour
    {
        [SerializeField] public float maxHealth = 100; // 보스의 최대 체력
        [SerializeField] protected float phaseChangeHealthPercentage = 0.5f; // 페이즈 전환 체력 비율
        [SerializeField] private float moveSpeed = 1f; // 보스 이동 속도
        [SerializeField] private float detectionRange = 10f; // 플레이어 감지 범위
        private float _currentHealth; // 보스의 현재 체력

        public delegate void DestroyedAction(); // 보스 사망 시 발생하는 이벤트
        public event DestroyedAction OnDestroyed; // 보스 사망 이벤트
        
        private Transform _player; // 플레이어 위치
        public bool isPhaseChanged; // 페이즈 전환 여부
        
        private float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }

        // 보스 초기화
        protected virtual void Start()
        {
            _currentHealth = maxHealth;
            FindPlayer();
            OnBossStart();
        }
        
        protected virtual void Update()
        {
            if (_player == null)
            {
                FindPlayer(); // 플레이어가 없으면 다시 찾음
                if (_player == null) return;
            }

            // 플레이어를 찾았거나 페이즈 전환 여부가 아니라면 플레이어를 따라가기
            if (isPhaseChanged || _currentHealth > maxHealth * phaseChangeHealthPercentage)
            {
                FollowPlayer();
                return;
            }

            // 페이즈 전환 실행
            isPhaseChanged = true;
            OnPhaseTransition();
        }

        // 플레이어를 찾는 메서드
        private void FindPlayer()
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (_player == null)
            {
                Debug.LogError("플레이어를 찾을 수 없습니다.");
            }
        }

        // 플레이어를 따라가는 로직
        private bool FollowPlayer()
        {
            var distanceToPlayer = Vector2.Distance(transform.position, _player.position);
            if (distanceToPlayer > detectionRange) return false; // 감지 범위 밖이면 이동하지 않음

            var direction = (_player.position - transform.position).normalized; 
            transform.Translate(direction * (moveSpeed * Time.deltaTime));
            return true;
        }

        // 데미지를 받았을 때 호출되는 메서드
        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        // 보스 사망
        private void Die()
        {
            Debug.Log($"{gameObject.name} 사망");
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        } 
        
        protected abstract void OnBossStart(); // 보스 시작 시 호출
        protected abstract void OnPhaseTransition(); // 페이즈 전환 시 호출
    }
}