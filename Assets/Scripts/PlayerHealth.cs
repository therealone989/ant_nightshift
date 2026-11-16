using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int startHealth = 100;
    [SerializeField] private int maxhealth = 100;
    public event Action OnPlayerHealthChanged;

    void Start()
    {
        health = startHealth;
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxhealth);
        OnPlayerHealthChanged?.Invoke();
    }

    public int GetPlayerHealth()
    {
        return health;
    }

    public int GetPlayerMaxHealth()
    {
        return maxhealth;
    }
}
