using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;
    [SerializeField] EnemyHealth enemyHealth;

    [SerializeField] float duration = 2f;
    private float elapsedTime = 0f;
    private float barFullWidth;
    private float startWidth;

    private void OnEnable()
    {
        enemyHealth.OnEnemyHealthChanged += UpdateEnemyUI;
    }
    private void OnDisable()
    {
        enemyHealth.OnEnemyHealthChanged -= UpdateEnemyUI;
    }

    private void Start()
    {
        barFullWidth = topBar.rect.width;
        UpdateEnemyUI();
    }

    void UpdateEnemyUI()
    {
        float newWidth = ((float)enemyHealth.GetEnemyHealth() / enemyHealth.GetEnemyMaxHealth()) * barFullWidth;
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
