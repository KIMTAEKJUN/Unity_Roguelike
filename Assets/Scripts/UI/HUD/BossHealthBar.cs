using Core.abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider; // 체력 슬라이더
        private BossBase _bossBase;

        public void SetBoss(BossBase activeBossBase)
        {
            _bossBase = activeBossBase;
            healthSlider.maxValue = _bossBase.maxHealth;
            healthSlider.value = _bossBase.CurrentHealth;
            gameObject.SetActive(true); // 체력바 활성화
        }

        private void Update()
        {
            if (_bossBase != null)
            {
                healthSlider.value = _bossBase.CurrentHealth;
            }
        }

        public void HideBar()
        {
            gameObject.SetActive(false);
        }
    }
}