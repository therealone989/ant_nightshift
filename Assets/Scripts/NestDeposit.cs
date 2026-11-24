using UnityEngine;

public class NestDeposit : MonoBehaviour
{

    Inventory playerInv;
    [SerializeField] RectTransform textCollect;
    [SerializeField] RectTransform OpenNestInv;
    [SerializeField] RectTransform NestInvUI;
    [SerializeField] NestInventory nestInventory;
    bool isInArea = false;
    int stones;
    int berries;
    int shrooms;

    private void Start()
    {
        textCollect.gameObject.SetActive(false);
        OpenNestInv.gameObject.SetActive(false);
        NestInvUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(isInArea && Input.GetKeyDown(KeyCode.E))
        {
            AddNestRessources();
            playerInv.ClearInventory();
        }

        if (isInArea && Input.GetKeyDown(KeyCode.F))
        {
            OpenNestInventory(!NestInvUI.gameObject.activeSelf);
        }
    }
    private void OpenNestInventory(bool openNow)
    {
        NestInvUI.gameObject.SetActive(openNow);
    }
    private void AddNestRessources()
    {
        stones = playerInv.GetStones();
        berries = playerInv.GetBerries();
        shrooms = playerInv.GetMushrooms();
        nestInventory.AddRessources(RessourceType.Berry, berries);
        nestInventory.AddRessources(RessourceType.Stone, stones);
        nestInventory.AddRessources(RessourceType.Mushroom, shrooms);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInArea = true;
            playerInv = collision.GetComponent<Inventory>();
            textCollect.gameObject.SetActive(true);
            OpenNestInv.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInArea = false;
            textCollect.gameObject.SetActive(false);
            OpenNestInv.gameObject.SetActive(false);
            OpenNestInventory(false);
        }
    }

}
