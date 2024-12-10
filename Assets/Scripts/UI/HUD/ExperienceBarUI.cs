using Features.Player.components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class ExperienceBarUI : MonoBehaviour
    {
        [SerializeField] private Slider expSlider;
        private PlayerLevel _playerLevel;

        private void Start()
        {
            _playerLevel = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerLevel>();
            if (_playerLevel == null)
            {
                Debug.LogError("PlayerLevel 컴포넌트를 찾을 수 없습니다.");
            }
        }

        private void Update()
        {
            if (_playerLevel != null)
            {
                expSlider.value = _playerLevel.GetExpProgress();
            }
        }
        
        public void UpdateExpBar(float progress)
        {
            expSlider.value = progress;
        }
    }
}