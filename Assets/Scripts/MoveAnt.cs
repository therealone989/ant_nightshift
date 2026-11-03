using UnityEngine;

public class MoveAnt : MonoBehaviour
{

    [SerializeField] bool walking = false;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 10f;
    Rigidbody2D rb;
    Vector2 direction = Vector2.zero;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
    public void Move()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)){direction += Vector2.up;}
        if (Input.GetKey(KeyCode.D)){direction += Vector2.right;}
        if (Input.GetKey(KeyCode.A)){direction += Vector2.left;}
        if (Input.GetKey(KeyCode.S)){direction += Vector2.down;}

        direction = direction.normalized;

        if (direction != Vector2.zero)
        {
            walking = true;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        } else
        {
            walking = false;
        }
        animator.SetBool("isWalking", walking);
    }
}
