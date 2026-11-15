using System;
using UnityEngine;
using UnityEngine.UIElements;

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
    public AttackPhase kaeferState;
    [SerializeField] float chargeDuration = 0.8f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float coolDownDuration = 3f;
    [SerializeField] LineRenderer chargeLine;

    [Header("Collider")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D dashHitbox;
    [SerializeField] Collider2D playerBodyCollider;

    Rigidbody2D rb;
    bool playerDetected = false;
    bool ignorePlayerCollision = false;
    bool hitThisDash = false;

    Vector2 aimDirection;
    Vector2 dashDirection;

    float stateTimer = 0f;
    

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
        if (!playerDetected) return;
        if (kaeferState == AttackPhase.Chasing)
        {
            aimDirection = (player.transform.position - transform.position).normalized;
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

        Debug.Log("Spieler getroffen");
    }


}
