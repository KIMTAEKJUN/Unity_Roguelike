using Features.Player;
using Features.Player.components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player
{
    public class PlayerExperienceUI : MonoBehaviour
    {
        [SerializeField] private PlayerLevel playerLevel;
        [SerializeField] private Slider experienceBar;
        [SerializeField] private Text levelText;

        private void Start()
        {
            if (playerLevel != null)
            {
                playerLevel.OnLevelUp += UpdateUI;
            }

            UpdateUI(playerLevel.GetLevel());
        }

        private void Update()
        {
            if (playerLevel != null)
            {
                experienceBar.value = playerLevel.GetExpProgress();
            }
        }

        private void UpdateUI(int newLevel)
        {
            if (levelText != null)
            {
                levelText.text = $"Lv. {newLevel}";
            }

            if (experienceBar != null)
            {
                experienceBar.maxValue = 1f;
            }
        }
    }
}