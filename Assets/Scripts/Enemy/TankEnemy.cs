using System.Collections;
using Enemy.Controller;
using UnityEngine;

namespace Enemy
{
    public class TankEnemy : EnemyController
    {
        [SerializeField] private float shieldDuration = 3f;
        [SerializeField] private float shieldCooldown = 10f;
        private bool _isShielded;
        private float _shieldTimer;

        protected override void Start()
        {
            base.Start();
            _shieldTimer = shieldCooldown;
        }

        protected override void Update()
        {
            base.Update();
            _shieldTimer += Time.deltaTime;

            if (_shieldTimer >= shieldCooldown && !_isShielded)
            {
                StartCoroutine(ActivateShield());
            }
        }

        private IEnumerator ActivateShield()
        {
            _isShielded = true;
            Debug.Log("TankEnemy 방어막 활성화!");
            yield return new WaitForSeconds(shieldDuration);
            _isShielded = false;
            _shieldTimer = 0;
            Debug.Log("TankEnemy 방어막 비활성화");
        }

        protected override void AttackPlayer()
        {
            if (_isShielded)
            {
                Debug.Log("TankEnemy 방어막으로 인해 공격 무시");
                return;
            }

            base.AttackPlayer();
        }
    }
}