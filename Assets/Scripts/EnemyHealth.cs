using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int health = 100;
    [SerializeField] private int maxhealth = 100;
    public event Action OnEnemyHealthChanged;
    public event Action OnEnemyDied;
    private bool enemyDead = false;

    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] float flashDuration = 0.1f;

    private Color originalColor;

    private void Start()
    {
        originalColor = enemySprite.color;
    }
    public void TakeDamage(int damage)
    {
        if (enemyDead) return;
        health = Mathf.Clamp(health - damage, 0, maxhealth);
        if (health > 0)
        {
            StartCoroutine(DamageFlash());
        }
        OnEnemyHealthChanged?.Invoke();

        if(health <= 0 && !enemyDead)
        {
            enemyDead = true;
            OnEnemyDied?.Invoke();
        }
    }

    private IEnumerator DamageFlash()
    {
        enemySprite.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        if (health > 0)
        {
            enemySprite.color = originalColor;
        }
    }

    public int GetEnemyHealth()
    {
        return health;
    }

    public int GetEnemyMaxHealth()
    {
        return maxhealth;
    }
}
