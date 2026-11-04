using UnityEngine;
using System.Collections.Generic;
public class RessourcePickup : MonoBehaviour
{
    Ressource pickedItem;
    Inventory playerInventory;
    [SerializeField] List<Ressource> nearbyRessources = new List<Ressource>();

    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ressource>() == null) return;
        pickedItem = collision.GetComponent<Ressource>();
        nearbyRessources.Add(pickedItem);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyRessources.Count == 0) return;
        Ressource leavingItem = collision.GetComponent<Ressource>();
        nearbyRessources.Remove(leavingItem);
    }

    private void Update()
    {
        if(nearbyRessources.Count > 0 && Input.GetKeyDown(KeyCode.E)) {
            
            // UI Text anzeigen "E - Pickup"
            // Event an UI Script - richtiger enum type stat hochzählen.
            Ressource nowPickedItem = nearbyRessources[0];
            playerInventory.addRessource(nowPickedItem.ressourceType);
            Destroy(nowPickedItem.gameObject);
            nearbyRessources.Remove(nowPickedItem);
        }
    }

}
