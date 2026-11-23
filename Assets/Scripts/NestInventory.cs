
using UnityEngine;

public class NestInventory : MonoBehaviour
{

    int Nestberries = 0;
    int Neststones = 0;
    int Nestmushrooms = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeResources(RessourceType type, int amount)
    {
        if (type == RessourceType.Berry) Nestberries += amount;
        if (type == RessourceType.Stone) Neststones += amount;
        if (type == RessourceType.Mushroom) Nestmushrooms += amount;
    }
}
