using UI.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button[] playerButtons;
        [FormerlySerializedAs("playerSwitcher")] [SerializeField] private PlayerSwitcher switcher;

        private void Start()
        {
            for (var i = 0; i < playerButtons.Length; i++)
            {
                var index = i; // Closure 방지
                playerButtons[i].onClick.AddListener(() => SelectPlayer(index));
            }
        }

        public void SelectPlayer(int index)
        {
            switcher.SwitchPlayer(index);
            Debug.Log($"플레이어 {index} 선택");
        }
    }
}