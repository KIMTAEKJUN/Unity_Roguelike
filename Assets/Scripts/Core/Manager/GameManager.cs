using Core.abstracts;
using Core.Pattern;
using UI.HUD;
using UnityEngine;

namespace Core.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private BossHealthBar bossHealthBar; // 체력바 UI
        private BossBase _currentBoss;

        // 보스 활성화 로직
        public void ActivateBoss(BossBase boss)
        {
            _currentBoss = boss;
            bossHealthBar.SetBoss(_currentBoss); // 보스 체력바 설정
        }

        // 보스 사망 처리
        public void OnBossDefeated()
        {
            bossHealthBar.HideBar(); // 체력바 숨기기
            _currentBoss = null;
        }
    }
}