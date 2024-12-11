using System.Collections.Generic;
using Core.abstracts;
using Core.Manager.Entity;
using Core.Pattern;
using Features.Player.components;
using UI.HUD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private ExperienceBarUI experienceBarUI; // 경험치바 UI
        [SerializeField] private SkillSelectionUI skillSelectionUI; // 스킬 선택 UI
        [SerializeField] private List<SkillData> allSkills; // 모든 스킬 데이터

        private BossBase _currentBoss; // 현재 보스
        private PlayerLevel _playerLevel; // 플레이어 레벨
        private PlayerStats _playerStats; // 플레이어 스탯

        private bool _isGameInitialized; // 게임 초기화 여부
        
        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            if (_isGameInitialized) return;

            InitializeGame();
            _isGameInitialized = true;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeLevel();
        }

        // 게임 초기화
        private void InitializeGame()
        {
            Debug.Log("게임 초기화 중...");
            InitializeLevel();
        }

        // 레벨 초기화
        private void InitializeLevel()
        {
            Debug.Log($"씬 초기화: {SceneManager.GetActiveScene().name}");

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerLevel = player.GetComponent<PlayerLevel>();
                _playerStats = player.GetComponent<PlayerStats>();

                if (_playerLevel != null)
                {
                    _playerLevel.OnLevelUp += HandleLevelUp;
                    UpdateExperienceBar();
                }

                ApplySavedSkills();
            }
            else
            {
                Debug.LogError("Player 객체를 찾을 수 없습니다!");
            }
        }

        // 경험치 추가
        public void AddExperience(float exp)
        {
            if (_playerLevel == null) return;
            _playerLevel.AddExperience(exp);
            UpdateExperienceBar();
        }

        // 경험치바 업데이트
        private void UpdateExperienceBar()
        {
            if (experienceBarUI == null || _playerLevel == null) return;
            experienceBarUI.UpdateExpBar(_playerLevel.GetExpProgress());
        }

        // 레벨업 처리
        private void HandleLevelUp(int newLevel)
        {
            UpdateExperienceBar();
            ShowSkillSelectionUI();
        }

        // 스킬 선택 UI 표시
        private void ShowSkillSelectionUI()
        {
            if (skillSelectionUI == null)
            {
                Debug.LogError("SkillSelectionUI가 할당되지 않았습니다!");
                return;
            }

            var randomSkills = GetRandomSkills(3);
            skillSelectionUI.ShowUI(randomSkills);
        }

        // 랜덤 스킬 데이터 반환
        private List<SkillData> GetRandomSkills(int count)
        {
            if (allSkills == null || allSkills.Count < count)
                return new List<SkillData>();

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

        // 저장된 스킬 적용
        private void ApplySavedSkills()
        {
            if (_playerStats == null) return;

            foreach (var skill in GameState.Instance.AppliedSkills)
                _playerStats.ApplySkill(skill);

            Debug.Log("GameState에서 스킬 상태를 적용했습니다.");
        }

        // 플레이어 상태 저장
        private void SavePlayerState()
        {
            if (_playerStats == null) return;

            GameState.Instance.ResetState();
            foreach (var skill in _playerStats.GetAppliedSkills())
                GameState.Instance.ApplySkill(skill);

            Debug.Log("GameState에 현재 스킬 상태를 저장했습니다.");
        }

        // 다음 레벨로 이동
        public void LoadNextLevel()
        {
            SavePlayerState();
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextSceneIndex);
        }

        // 보스 활성화
        public void ActivateBoss(BossBase boss)
        {
            _currentBoss = boss;
            _currentBoss.OnDestroyed += HandleBossDefeat;
        }

        // 보스 처치 시 호출
        public void OnBossDefeated()
        {
            _currentBoss = null;
        }

        // 보스 처치 처리
        private void HandleBossDefeat()
        {
            Debug.Log("보스 처치! 다음 레벨로 이동합니다.");
            LoadNextLevel();
        }
    }
}