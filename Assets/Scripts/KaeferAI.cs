using UnityEngine;
using System.Collections;
public class KaeferAI : MonoBehaviour
{
    [Header("Spielererkennung")]
    public LayerMask playerMask;
    public float range = 5f;
    [SerializeField] float attackRange = 2f;
    public GameObject player;

    [Header("Bewegung")]
    public float moveSpeed = 3f;
    [SerializeField] float dashSpeed = 16f;

    [Header("Angriff")]
    [SerializeField] int damage = 5;
    public AttackPhase kaeferState;
    [SerializeField] float chargeDuration = 0.8f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float coolDownDuration = 3f;
    [SerializeField] LineRenderer chargeLine;

    [Header("Collider")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D dashHitbox;
    [SerializeField] Collider2D playerBodyCollider;

    [Header("Health")]
    [SerializeField] EnemyHealth enemyHealth;

    [Header("Todesanimation")]
    [SerializeField] SpriteRenderer kaeferSprite;
    [SerializeField] Transform kaeferVisual;
    [SerializeField] float squashDuration = 0.2f;
    [SerializeField] float fadeDuration = 0.8f;

    [Header("Käfer AI")]
    [SerializeField] LayerMask enemyMask;

    Rigidbody2D rb;
    bool playerDetected = false;
    bool ignorePlayerCollision = false;
    bool hitThisDash = false;
    private bool isDead = false;

    Vector2 aimDirection;
    Vector2 dashDirection;

    float stateTimer = 0f;

    private void OnEnable()
    {
        enemyHealth.OnEnemyDied += OnKaeferDied;
    }

    private void OnDisable()
    {
        enemyHealth.OnEnemyDied -= OnKaeferDied;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chargeLine.positionCount = 2;
        chargeLine.useWorldSpace = true;
        chargeLine.enabled = false;

        dashHitbox.enabled = false;
    }

    void Update()
    {
        if(isDead) return;
        TryRestoreCollision();
        if (!playerDetected) { DetectPlayerInRange(); }
        if (!playerDetected) return;

        if(kaeferState == AttackPhase.Chasing)
        {
            LookAtPlayer();
            CheckAttackRange();
        }
        else if (kaeferState == AttackPhase.Charging)   { StartCharge(); }
        else if (kaeferState == AttackPhase.Dashing)    { DashAttack(); }
        else if (kaeferState == AttackPhase.Cooldown)   { CoolDown(); }
    }

    private void FixedUpdate()
    {
        if (!playerDetected || isDead) return;
        if (kaeferState == AttackPhase.Chasing)
        {
            aimDirection = player.transform.position - transform.position;
            aimDirection.Normalize();
            rb.MovePosition(rb.position + aimDirection * moveSpeed * Time.fixedDeltaTime);
        } else if (kaeferState == AttackPhase.Dashing)
        {
            // nur beim dash, damit es kein "autoaim" gibt wegen Lookat.
            rb.MovePosition(rb.position + dashDirection * dashSpeed  * Time.fixedDeltaTime);
        }
    }

    private void DetectPlayerInRange()
    {
        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, range, playerMask);
        if (playerCol != null)
        {
            playerDetected = true;
            kaeferState = AttackPhase.Chasing;
            player = playerCol.gameObject;
        }
    }

    private void CheckAttackRange()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            kaeferState = AttackPhase.Charging;
            stateTimer = 0f;
            chargeLine.enabled = true;
        }
    }

    private void StartCharge()
    {
        stateTimer += Time.deltaTime;

        aimDirection = (player.transform.position - transform.position).normalized;

        LookAtPlayer();

        Vector2 lineStart = transform.position;
        Vector2 lineEnd = lineStart + aimDirection * (dashSpeed * dashDuration);
        chargeLine.SetPosition(0, lineStart);
        chargeLine.SetPosition(1, lineEnd);

        if(stateTimer >= chargeDuration)
        {
            dashDirection = aimDirection;
            hitThisDash = false;
            kaeferState = AttackPhase.Dashing;
            stateTimer = 0f;
            chargeLine.enabled = false;

            Physics2D.IgnoreCollision(bodyCollider, playerBodyCollider, true);

            ignorePlayerCollision = true;
            dashHitbox.enabled = true;
        }

    }

    private void DashAttack()
    {
        stateTimer += Time.deltaTime;

        if(stateTimer >= dashDuration)
        {
            kaeferState = AttackPhase.Cooldown;
            stateTimer = 0f;

            dashHitbox.enabled = false;
        }
    }

    private void CoolDown()
    {
        stateTimer += Time.deltaTime;

        if(stateTimer >= coolDownDuration)
        {
            kaeferState = AttackPhase.Chasing;
            stateTimer = 0f;
        }
    }
    private void TryRestoreCollision()
    {
        if (!ignorePlayerCollision) return;

        if (kaeferState == AttackPhase.Dashing) return;

        ColliderDistance2D distance = bodyCollider.Distance(playerBodyCollider);

        if (!distance.isOverlapped)
        {
            Physics2D.IgnoreCollision(bodyCollider,playerBodyCollider,false);
            ignorePlayerCollision = false;
        }
    }
    private void LookAtPlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (kaeferState != AttackPhase.Dashing) return;
        if (hitThisDash) return;
        if (collision != playerBodyCollider) return;

        hitThisDash = true;

        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }



    private void OnKaeferDied()
    {
        isDead = true;
        Debug.Log("Käfer ist besiegt");
        chargeLine.enabled = false;
        dashHitbox.enabled = false;
        bodyCollider.enabled = false;
        StartCoroutine(DeathAnimation());
    }


    private IEnumerator DeathAnimation()
    {
        Vector3 originalScale = kaeferVisual.localScale;
        Color originalColor = kaeferSprite.color;

        // Phase 1: Käfer zusammendrücken
        float timer = 0f;

        while (timer < squashDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / squashDuration);

            Vector3 newScale = originalScale;
            newScale.y = Mathf.Lerp(originalScale.y, originalScale.y * 0.4f,t);

            kaeferVisual.localScale = newScale;

            yield return null;
        }


        // Phase 2: Langsam transparent werden
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / fadeDuration);

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(originalColor.a, 0f, t);

            kaeferSprite.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }



}
