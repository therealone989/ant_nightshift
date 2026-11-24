using TMPro;
using UnityEngine;

public class NestInventoryUI : MonoBehaviour
{
    [SerializeField] TMP_Text nestBerriesText;
    [SerializeField] TMP_Text nestStonesText;
    [SerializeField] TMP_Text nestMushroomsText;
    [SerializeField] NestInventory nestInv;

    private void OnEnable()
    {
        nestInv.OnNestInventoryChanged += UpdateNestInventoryUI;
    }
    private void OnDisable()
    {
        nestInv.OnNestInventoryChanged -= UpdateNestInventoryUI;
    }

    private void Start()
    {
        UpdateNestInventoryUI();
    }

    private void UpdateNestInventoryUI()
    {
        nestBerriesText.text = nestInv.GetNestBerries().ToString();
        nestStonesText.text = nestInv.GetNestStones().ToString();
        nestMushroomsText.text = nestInv.GetNestMushrooms().ToString();
    }
}
