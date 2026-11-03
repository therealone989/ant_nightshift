using UnityEngine;

public class Inventory : MonoBehaviour
{
    int berries = 0;
    int stones = 0;
    int mushrooms = 0;

    public void addRessource(RessourceType type)
    {
        if (type == RessourceType.Berry) berries++;
        if (type == RessourceType.Stone) stones++;
        if (type == RessourceType.Mushroom) mushrooms++;
    }

}
