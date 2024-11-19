using System.Collections;
using Enemy.Controller;
using UnityEngine;

namespace Enemy
{
    public class SpeedEnemy : EnemyController
    {
        [SerializeField] private float speedBoostMultiplier = 2f;
        [SerializeField] private float boostDuration = 1f;
        [SerializeField] private float boostCooldown = 5f;
        private float _boostTimer;
        private bool _isBoosting;

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