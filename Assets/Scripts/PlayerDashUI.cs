using UnityEngine;
using UnityEngine.UI;

public class PlayerDashUI : MonoBehaviour
{
    [SerializeField] MoveAnt moveAnt;
    [SerializeField] Image topDash;

    void Update()
    {
        float fillAmount = moveAnt.GetDashCooldown();
        fillAmount = Mathf.Clamp01(fillAmount);
        topDash.fillAmount = fillAmount;
    }
}
