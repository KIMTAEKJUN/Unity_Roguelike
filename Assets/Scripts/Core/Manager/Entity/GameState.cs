using System.Collections.Generic;
using Core.Pattern;

namespace Core.Manager.Entity
{
    public class GameState : Singleton<GameState>
    {
        public List<SkillData> AppliedSkills { get; private set; } = new List<SkillData>(); // 적용된 스킬 목록

        // 스킬 적용
        public void ApplySkill(SkillData skill)
        {
            if (!AppliedSkills.Contains(skill)) // 중복 방지
            {
                AppliedSkills.Add(skill);
            }
        }

        // 스킬 적용 해제
        public void ResetState()
        {
            AppliedSkills.Clear();
        }
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}