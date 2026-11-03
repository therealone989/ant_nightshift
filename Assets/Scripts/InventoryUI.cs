using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] TMP_Text berriesText;
    [SerializeField] TMP_Text stonesText;
    [SerializeField] TMP_Text mushroomsText;
    [SerializeField] Inventory playerInv;

    private void OnEnable()
    {
        playerInv.OnInventoryChanged += UpdateInventoryUI;
    }
    private void OnDisable()
    {
        playerInv.OnInventoryChanged -= UpdateInventoryUI;
    }

    private void Start()
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        berriesText.text = playerInv.GetBerries().ToString();
        stonesText.text = playerInv.GetStones().ToString(); 
        mushroomsText.text = playerInv.GetMushrooms().ToString();
    }
}
