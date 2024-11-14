using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private Image imageScreen;
        [SerializeField] private float maxHealth = 3;
        private float _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log("플레이어 캐릭터의 체력이 " + _currentHealth + "로 감소했습니다.");
            
            StopCoroutine(HitAlphaAnimation());
            StartCoroutine(HitAlphaAnimation());

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private static void Die()
        {
            Debug.Log("플레이어 캐릭터가 죽었습니다!");
            Time.timeScale = 0f;
        }
        
        private IEnumerator HitAlphaAnimation()
        {
            if (imageScreen == null) yield break;

            var color = imageScreen.color;
            color.a = 0.4f;
            imageScreen.color = color;

            while (color.a > 0.0f)
            {
                color.a -= Time.deltaTime;
                imageScreen.color = color;
                
                yield return null;
            }
        }
    }
}