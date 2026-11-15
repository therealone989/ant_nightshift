using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KaeferAI : MonoBehaviour
{

    public LayerMask playerMask;
    public float range = 5f;
    [SerializeField] float attackRange = 2f;
    public float moveSpeed = 3f;
    public GameObject player;
    public AttackPhase kaeferState;
    [SerializeField] LineRenderer chargeLine;
    [SerializeField] float chargeDuration = 0.8f;
    [SerializeField] float dashSpeed = 16f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float coolDownDuration = 3f;

    private Rigidbody2D rb;
    public bool playerDetected = false;

    Vector2 aimDirection;
    Vector2 dashDirection;

    float stateTimer = 0f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chargeLine.positionCount = 2;
        chargeLine.useWorldSpace = true;
        chargeLine.enabled = false;
    }

    void Update()
    {
        if (playerDetected == false) { DetectPlayerInRange(); }
        if (!playerDetected) return;

        if(kaeferState == AttackPhase.Chasing)
        {
            LookAtPlayer();
            CheckAttackRange();
        }
        else if (kaeferState == AttackPhase.Charging) {
            ChargeAttack();
        }
        else if (kaeferState == AttackPhase.Dashing)
        {
            DashAttack();
        }
        else if (kaeferState == AttackPhase.Cooldown)
        {
            CoolDown();
        }

        // Aufladen
        // Sturmangriff - collider? - Schnell? - zieht an ihn vorbei ein paar units damit es aussieht als ob er schnell stürmt
        // Erschöpft - keine bewegung - zeit zum zubeißen


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

    private void ChargeAttack()
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
            kaeferState = AttackPhase.Dashing;
            stateTimer = 0f;
            chargeLine.enabled = false;
        }

    }

    private void DashAttack()
    {
        stateTimer += Time.deltaTime;

        if(stateTimer >= dashDuration)
        {
            kaeferState = AttackPhase.Cooldown;
            stateTimer = 0f;
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

    private void LookAtPlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("GATCHAAAAA");
        }
    }


}
