using System;
using UI.Camera;
using UnityEngine;

namespace UI.Player
{
    public class PlayerSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] characterModels; // 캐릭터 모델 배열
        private int _currentModelIndex = 0;
        
        [SerializeField] private CameraFollow cameraFollow;

        public static event Action<GameObject> OnPlayerSwitched; // 플레이어 변경 이벤트

        public void SwitchPlayer(int index)
        {
            if (index < 0 || index >= characterModels.Length)
            {
                Debug.LogError("유효하지 않은 캐릭터 모델 인덱스");
                return;
            }

            // 이전 플레이어 비활성화
            characterModels[_currentModelIndex].SetActive(false);

            // 새로운 플레이어 활성화
            _currentModelIndex = index;
            var newPlayer = characterModels[_currentModelIndex];
            newPlayer.SetActive(true);

            if (cameraFollow != null)
            {
                cameraFollow.player = characterModels[_currentModelIndex].transform;
            }
            else
            {
                Debug.LogError("CameraFollow 스크립트가 연결되어 있지 않습니다.");
            }

            // 적들이 새로운 플레이어를 타겟으로 갱신
            OnPlayerSwitched?.Invoke(newPlayer);

            Debug.Log($"플레이어 변경: {newPlayer.name}");
        }
    }
}