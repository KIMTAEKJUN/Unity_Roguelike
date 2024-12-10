using UnityEngine;

namespace Features.Player.components
{
    public class PlayerLevel : MonoBehaviour
    {
        [SerializeField] private int level = 1; // 현재 레벨
        [SerializeField] private float currentExp = 0; // 현재 경험치
        [SerializeField] private float requiredExp = 100; // 레벨업에 필요한 경험치
        [SerializeField] private float expGrowthRate = 1.25f; // 필요 경험치 증가율

        public delegate void LevelUpAction(int newLevel);
        public event LevelUpAction OnLevelUp; // 레벨업 이벤트

        public void AddExperience(float exp)
        {
            currentExp += exp;

            if (currentExp >= requiredExp)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentExp -= requiredExp;
            level++;
            requiredExp *= expGrowthRate;

            OnLevelUp?.Invoke(level); // 레벨업 이벤트 호출
            Debug.Log($"레벨업! 현재 플레이어 레벨: {level}");
        }

        public int GetLevel() => level;
        public float GetExpProgress() => currentExp / requiredExp;
    }
}