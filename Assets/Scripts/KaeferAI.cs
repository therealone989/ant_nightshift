using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KaeferAI : MonoBehaviour
{

    public LayerMask playerMask;
    public float range = 5f;
    public float moveSpeed = 3f;
    public GameObject player;
    public AttackPhase kaeferState;

    private Rigidbody2D rb;
    bool playerDetected = false;
    Vector2 aimDirection;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        if (playerDetected == false) { DetectPlayerInRange(); }
        if (playerDetected) { LookAtPlayer(); } // Verfolgen

        // Aufladen
        // Sturmangriff - collider? - Schnell? - zieht an ihn vorbei ein paar units damit es aussieht als ob er schnell stürmt
        // Erschöpft - keine bewegung - zeit zum zubeißen


    }

    private void DetectPlayerInRange()
    {
        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, range, playerMask);
        if (playerCol != null && playerDetected == false)
        {
            playerDetected = true;
            kaeferState = AttackPhase.Chasing;
            player = playerCol.gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (playerDetected && kaeferState == AttackPhase.Chasing)
        {
            aimDirection = player.transform.position - transform.position;
            aimDirection.Normalize();
            rb.MovePosition(rb.position + aimDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("GATCHAAAAA");
        }
    }


}
