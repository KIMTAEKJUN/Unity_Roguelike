using Core.enums;
using UnityEngine;

namespace Core.Manager.Entity
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
    public class SkillData : ScriptableObject
    {
        public string skillName; // 스킬 이름
        public string description; // 스킬 설명
        public SkillType type; // 스킬 타입
        public float value; // 증가하는 값
    }
}