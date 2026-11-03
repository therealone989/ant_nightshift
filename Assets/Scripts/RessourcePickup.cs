using UnityEngine;

public class RessourcePickup : MonoBehaviour
{
    Ressource pickedItem;
    Inventory playerInventory;

    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ressource>() == null) return;
        pickedItem = collision.GetComponent<Ressource>();
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pickedItem == null) return;
        if(collision.gameObject == pickedItem.gameObject)
        {
            pickedItem = null;
        }
    }

    private void Update()
    {
        if(pickedItem != null && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Ressource Eingesammelt!!");
            playerInventory.addRessource(pickedItem.ressourceType);
            // UI Text anzeigen "E - Pickup"
            // Event an UI Script - richtiger enum type stat hochzählen.
            Destroy(pickedItem.gameObject);
            pickedItem = null;
            
        }
    }

}
