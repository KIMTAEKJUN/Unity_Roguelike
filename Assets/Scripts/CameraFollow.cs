using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 따라갈 플레이어
    public float smoothSpeed = 0.125f; // 카메라 이동 속도
    public Vector3 offset; // 카메라와 플레이어 간의 오프셋 거리

    private void LateUpdate()
    {
        if (player == null) return;

        // 카메라의 목표 위치 설정
        var desiredPosition = player.position + offset;
        
        // 부드럽게 목표 위치로 이동
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}