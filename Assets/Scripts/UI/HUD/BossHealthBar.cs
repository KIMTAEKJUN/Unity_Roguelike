using Core.abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        private BossBase _bossBase;

        public void SetBoss(BossBase activeBossBase)
        {
            _bossBase = activeBossBase;
            healthSlider.maxValue = _bossBase.maxHealth;
            healthSlider.value = _bossBase.CurrentHealth;
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