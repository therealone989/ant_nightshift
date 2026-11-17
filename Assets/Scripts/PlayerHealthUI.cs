using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Healthbar")]
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] float duration = 2f;


    
    private float elapsedTime = 0f;
    private float barFullWidth;
    private float startWidth;

    private float currentHealth;
    private float maxHealth;

    private void OnEnable()
    {
        playerHealth.OnPlayerHealthChanged += UpdatePlayerUI;
    }
    private void OnDisable()
    {
        playerHealth.OnPlayerHealthChanged -= UpdatePlayerUI;
    }

    private void Start()
    {
        barFullWidth = topBar.rect.width;
        currentHealth = (float)playerHealth.GetPlayerHealth();
        maxHealth = (float)playerHealth.GetPlayerMaxHealth();
        healthText.text = currentHealth.ToString();
        UpdatePlayerUI();
    }

    void UpdatePlayerUI()
    {
        currentHealth = (float)playerHealth.GetPlayerHealth();
        maxHealth = (float)playerHealth.GetPlayerMaxHealth();
        healthText.text = currentHealth.ToString();
        float newWidth = (currentHealth / maxHealth) * barFullWidth;
        newWidth = Mathf.Clamp(newWidth, 0f, barFullWidth);
        topBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        startWidth = bottomBar.rect.width;
        elapsedTime = 0f;
        
    }

    private void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;
            float backBarWidth = Mathf.Lerp(startWidth, topBar.rect.width, t);
            bottomBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, backBarWidth);
        }
    }
}
