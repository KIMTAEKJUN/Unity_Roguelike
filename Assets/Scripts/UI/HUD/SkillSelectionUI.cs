using System.Collections.Generic;
using Core.Manager.Entity;
using Features.Player.components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class SkillSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button[] skillButtons; // 3개의 스킬 버튼
        [SerializeField] private TextMeshProUGUI[] skillDescriptions; // 스킬 설명 텍스트
        [SerializeField] private PlayerStats playerStats; // 플레이어의 스탯 관리
        private List<SkillData> _availableSkills; // 사용 가능한 스킬 리스트

        private void Awake()
        {
            InitializeUI();
            HideUI();
        }

        private void InitializeUI()
        {
            if (skillButtons.Length != skillDescriptions.Length)
            {
                Debug.LogError("SkillSelectionUI: 버튼과 설명 텍스트의 개수가 일치하지 않습니다!");
                return;
            }

            foreach (var button in skillButtons)
            {
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }

            foreach (var text in skillDescriptions)
            {
                if (text != null)
                {
                    text.text = string.Empty;
                }
            }
        }
        
        public void ShowUI(List<SkillData> skills)
        {
            if (skills == null || skills.Count == 0)
            {
                Debug.LogError("ShowUI: 전달된 스킬 리스트가 비어있습니다!");
                HideUI();
                return;
            }

            gameObject.SetActive(true);
            _availableSkills = skills;

            for (var i = 0; i < skillButtons.Length; i++)
            {
                if (i < skills.Count)
                {
                    skillButtons[i].gameObject.SetActive(true);

                    if (skillDescriptions[i] != null)
                    {
                        skillDescriptions[i].text = $"<size=20><b>{skills[i].skillName}</b></size>\n<size=14>{skills[i].description}</size>";
                        skillDescriptions[i].fontSize = 18;
                        skillDescriptions[i].enableWordWrapping = true;
                    }
                    else
                    {
                        Debug.LogError($"SkillDescriptions[{i}]가 할당되지 않았습니다!");
                    }
                }
                else
                {
                    skillButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void HideUI()
        {
            gameObject.SetActive(false);
            foreach (var button in skillButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        public void SelectSkill(int index)
        {
            if (_availableSkills == null || index < 0 || index >= _availableSkills.Count)
            {
                Debug.LogError("SelectSkill: 유효하지 않은 인덱스 또는 스킬 리스트가 초기화되지 않았습니다!");
                return;
            }

            ApplySkill(_availableSkills[index]);
            HideUI();
        }

        private void ApplySkill(SkillData skill)
        {
            GameState.Instance.ApplySkill(skill);
            playerStats.ApplySkill(skill);
        }
    }
}