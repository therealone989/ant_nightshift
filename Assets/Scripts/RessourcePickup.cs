using UnityEngine;
using System.Collections.Generic;
public class RessourcePickup : MonoBehaviour
{
    Ressource pickedItem;
    Inventory playerInventory;
    [SerializeField] List<Ressource> nearbyRessources = new List<Ressource>();
    [SerializeField] Canvas pickUpCanvas;

    private void Start()
    {
        pickUpCanvas.gameObject.SetActive(false);
        playerInventory = GetComponent<Inventory>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ressource>() == null) return;

        pickedItem = collision.GetComponent<Ressource>();
        nearbyRessources.Add(pickedItem);
        UpdatePickupUI(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyRessources.Count == 0) return;

        Ressource leavingItem = collision.GetComponent<Ressource>();
        nearbyRessources.Remove(leavingItem);
        UpdatePickupUI(false);
    }

    private void Update()
    {
        if(nearbyRessources.Count > 0 && Input.GetKeyDown(KeyCode.E)) {
            Ressource nowPickedItem = nearbyRessources[0];
            playerInventory.addRessource(nowPickedItem.ressourceType);
            Destroy(nowPickedItem.gameObject);
            nearbyRessources.Remove(nowPickedItem);
        } 
    }
    public void UpdatePickupUI(bool active)
    {
        pickUpCanvas.gameObject.SetActive(active);
    }
}
