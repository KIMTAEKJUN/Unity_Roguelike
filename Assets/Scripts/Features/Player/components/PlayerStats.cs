using System.Collections;
using System.Collections.Generic;
using Core.enums;
using Core.Manager.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.components
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Image imageScreen;
        [SerializeField] private float maxHealth = 100; // 최대 체력
        [SerializeField] private float moveSpeed = 5f; // 이동 속도
        [SerializeField] private float attackSpeed = 1f; // 공격 속도
        [SerializeField] private float attackRange = 10f; // 공격 범위
        [SerializeField] private float attackCooldown = 1f; // 공격 쿨다운
        [SerializeField] private float attackDamage = 10f; // 공격력
        [SerializeField] private int projectileCount = 1; // 발사체 개수
        private int _baseProjectileCount = 1;
        private readonly List<SkillData> _appliedSkills = new List<SkillData>();
        
        private float _currentHealth;

        private void Start()
        {
            InitializeStats();
            projectileCount = _baseProjectileCount;
        }

        private void InitializeStats()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log($"플레이어가 {damage}의 피해를 입었습니다. 현재 체력: {_currentHealth}");
            
            StopCoroutine(HitAlphaAnimation());
            StartCoroutine(HitAlphaAnimation());

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void IncreaseMaxHealth(float amount)
        {
            maxHealth += amount;
            _currentHealth += amount;
            Debug.Log($"플레이어 최대 체력이 {maxHealth}로 증가했습니다. 현재 체력: {_currentHealth}");
        }

        private void IncreaseAttackSpeed(float multiplier)
        {
            attackSpeed *= multiplier;
            Debug.Log($"플레이어 공격 속도가 {attackSpeed}로 증가했습니다.");
        }

        private void IncreaseMoveSpeed(float amount)
        {
            moveSpeed += amount;
            Debug.Log($"플레이어 이동 속도가 {moveSpeed}로 증가했습니다.");
        }

        private void IncreaseProjectileCount(int count)
        {
            _baseProjectileCount += count;
            projectileCount = _baseProjectileCount;
            Debug.Log($"플레이어 발사체 개수가 {projectileCount}로 증가했습니다.");
        }

        private void IncreaseDamage(float amount)
        {
            attackDamage += amount;
            Debug.Log($"플레이어 공격력이 {attackDamage}로 증가했습니다.");
        }

        private static void Die()
        {
            Debug.Log("플레이어가 사망했습니다!");
            Time.timeScale = 0f;
        }
        
        private IEnumerator HitAlphaAnimation()
        {
            if (imageScreen == null) yield break;

            var color = imageScreen.color;
            color.a = 0.4f;
            imageScreen.color = color;

            while (color.a > 0.0f)
            {
                color.a -= Time.deltaTime;
                imageScreen.color = color;
                
                yield return null;
            }
        }
        
        public void ApplySkill(SkillData skill)
        {
            // 이미 적용된 스킬인지 확인
            if (_appliedSkills.Contains(skill))
            {
                Debug.LogWarning($"스킬 {skill.skillName}이 이미 적용되었습니다.");
                return;
            }
            
            _appliedSkills.Add(skill);

            switch (skill.type)
            {
                case SkillType.AttackSpeed:
                    IncreaseAttackSpeed(skill.value);
                    break;
                case SkillType.MaxHealth:
                    IncreaseMaxHealth(skill.value);
                    break;
                case SkillType.MoveSpeed:
                    IncreaseMoveSpeed(skill.value);
                    break;
                case SkillType.ProjectileCount:
                    IncreaseProjectileCount(Mathf.RoundToInt(skill.value));
                    break;
                case SkillType.Damage:
                    IncreaseDamage(skill.value);
                    break;
                default:
                    Debug.LogWarning($"알 수 없는 스킬 타입: {skill.type}");
                    break;
            }
            
            _appliedSkills.Add(skill);
            Debug.Log($"스킬 선택: {skill.skillName} 적용");
        }

        public List<SkillData> GetAppliedSkills()
        {
            return new List<SkillData>(_appliedSkills);
        }

        public float GetMoveSpeed() => moveSpeed;
        public float GetAttackRange() => attackRange;
        public float GetAttackCooldown() => attackCooldown / attackSpeed;
        public float GetAttackDamage() => attackDamage;
        public int GetProjectileCount() => projectileCount;
    }
}