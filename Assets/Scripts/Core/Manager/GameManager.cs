using System.Collections.Generic;
using Core.abstracts;
using Core.Manager.Entity;
using Core.Pattern;
using Features.Player.components;
using UI.HUD;
using UI.Skill;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Core.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [FormerlySerializedAs("experienceBar")] [SerializeField] private ExperienceBarUI experienceBarUI;
        [SerializeField] private SkillSelectionUI skillSelectionUI;
        [SerializeField] private List<SkillData> allSkills;
        
        private BossBase _currentBoss;
        private PlayerLevel _playerLevel;

        // 보스 활성화 로직
        public void ActivateBoss(BossBase boss)
        {
            _currentBoss = boss;
            _currentBoss.OnDestroyed += HandleBossDefeat;
        }

        // 보스 사망 처리
        public void OnBossDefeated()
        {
            _currentBoss = null;
        }
        
        // 보스 처치 시 다음 레벨로 이동
        private void HandleBossDefeat()
        {
            Debug.Log("보스 처치! 다음 레벨로 이동합니다.");
            LoadNextLevel();
        }

        public void LoadNextLevel()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("마지막 레벨에 도달했습니다!");
                // 엔딩 씬 로직 추가 가능
            }
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerLevel = player.GetComponent<PlayerLevel>();
                if (_playerLevel != null)
                {
                    _playerLevel.OnLevelUp += HandleLevelUp;
                    UpdateExperienceBar();
                }
            }
            else
            {
                Debug.LogError("Player 객체를 찾을 수 없습니다!");
            }
        }

        // 경험치 추가
        public void AddExperience(float exp)
        {
            if (_playerLevel != null)
            {
                _playerLevel.AddExperience(exp);
                UpdateExperienceBar();
            }
        }

        // 경험치 바 업데이트
        private void UpdateExperienceBar()
        {
            if (experienceBarUI != null && _playerLevel != null)
            {
                experienceBarUI.UpdateExpBar(_playerLevel.GetExpProgress());
            }
        }

        // 레벨업 이벤트 핸들러
        private void HandleLevelUp(int newLevel)
        {
            Debug.Log($"GameManager: 플레이어 레벨업! 새로운 레벨: {newLevel}");
            UpdateExperienceBar();
            
            Debug.Log($"레벨 {newLevel} 달성! 스킬 선택 UI를 표시합니다.");
            ShowSkillSelectionUI();
        }
        
        private void ShowSkillSelectionUI()
        {
            if (skillSelectionUI == null)
            {
                Debug.LogError("GameManager: SkillSelectionUI가 할당되지 않았습니다!");
                return;
            }

            var randomSkills = GetRandomSkills(3); // 무작위로 3개의 스킬 선택
            Debug.Log($"무작위 스킬 {randomSkills.Count}개 선택됨");
            skillSelectionUI.ShowUI(randomSkills);
        }
        
        private List<SkillData> GetRandomSkills(int count)
        {
            if (allSkills == null || allSkills.Count < count)
            {
                Debug.LogError("GetRandomSkills: 선택 가능한 스킬이 충분하지 않습니다.");
                return new List<SkillData>();
            }
            
            var randomSkills = new List<SkillData>();
            var availableSkills = new List<SkillData>(allSkills);

            for (var i = 0; i < count && availableSkills.Count > 0; i++)
            {
                var randomIndex = Random.Range(0, availableSkills.Count);
                randomSkills.Add(availableSkills[randomIndex]);
                availableSkills.RemoveAt(randomIndex);
            }

            return randomSkills;
        }
    }
}