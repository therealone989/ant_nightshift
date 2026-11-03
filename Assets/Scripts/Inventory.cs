using UnityEngine;
using System;
public class Inventory : MonoBehaviour
{
    int berries = 0;
    int stones = 0;
    int mushrooms = 0;
    public event Action OnInventoryChanged;

    public void addRessource(RessourceType type)
    {
        if (type == RessourceType.Berry) berries++;
        if (type == RessourceType.Stone) stones++;
        if (type == RessourceType.Mushroom) mushrooms++;
        OnInventoryChanged?.Invoke();
    }

    public int GetBerries()
    {
        return berries;
    }

    public int GetStones()
    {
        return stones;
    }

    public int GetMushrooms()
    {
        return mushrooms;
    }

}
