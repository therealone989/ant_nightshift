
using System;
using UnityEngine;

public class NestInventory : MonoBehaviour
{

    [SerializeField] int Nestberries = 0;
    [SerializeField] int Neststones = 0;
    [SerializeField] int Nestmushrooms = 0;

    public event Action OnNestInventoryChanged;


    public void AddRessources(RessourceType type, int amount)
    {
        if (type == RessourceType.Berry) Nestberries += amount;
        if (type == RessourceType.Stone) Neststones += amount;
        if (type == RessourceType.Mushroom) Nestmushrooms += amount;
        OnNestInventoryChanged?.Invoke();
    }

    public int GetNestStones()
    {
        return Neststones;
    }

    public int GetNestBerries()
    {
        return Nestberries;
    }

    public int GetNestMushrooms()
    {
        return Nestmushrooms;
    }
}
