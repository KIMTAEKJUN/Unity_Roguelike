using System.Collections;
using Core.abstracts;
using UnityEngine;

namespace Features.Enemies.Types
{
    public class SpeedEnemy : EnemyBase
    {
        [SerializeField] private float speedBoostMultiplier = 2f; // 이동 속도 증가 배수
        [SerializeField] private float boostDuration = 1f; // 이동 속도 증가 지속 시간
        [SerializeField] private float boostCooldown = 5f; // 이동 속도 증가 쿨타임
        private float _boostTimer; // 이동 속도 증가 쿨타임 타이머
        private bool _isBoosting; // 이동 속도 증가 중인지 여부

        protected override void Start()
        {
            base.Start();
            _boostTimer = boostCooldown;
        }

        protected override void Update()
        {
            base.Update();

            _boostTimer += Time.deltaTime;
            if (_boostTimer >= boostCooldown && !_isBoosting)
            {
                StartCoroutine(SpeedBoost());
                Debug.Log("SpeedEnemy 스킬 사용");
            }
        }

        private IEnumerator SpeedBoost()
        {
            _isBoosting = true;
            moveSpeed *= speedBoostMultiplier;
            yield return new WaitForSeconds(boostDuration);
            moveSpeed /= speedBoostMultiplier;
            _boostTimer = 0;
            _isBoosting = false;
        }
    }
}