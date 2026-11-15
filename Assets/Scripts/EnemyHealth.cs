using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int health = 100;
    [SerializeField] private int maxhealth = 100;
    public event Action OnEnemyHealthChanged;

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxhealth);
        OnEnemyHealthChanged?.Invoke();
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
