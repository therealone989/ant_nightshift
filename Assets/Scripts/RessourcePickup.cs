using UnityEngine;

public class RessourcePickup : MonoBehaviour
{
    Ressource pickedItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ressource>() == null) return;
        pickedItem = collision.GetComponent<Ressource>();
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Ressource>().ressourceType == pickedItem.ressourceType)
        {
            pickedItem = null;
        }
    }

    private void Update()
    {
        if(pickedItem != null && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Ressource Eingesammelt!!");
            // UI Text anzeigen "E - Pickup"
            // Event an UI Script - richtiger enum type stat hochzählen.
            pickedItem = null;
            Destroy(pickedItem.gameObject);
        }
    }

}
