using UnityEngine;

public class MoveAnt : MonoBehaviour
{

    [SerializeField] bool walking = false;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Transform antVisual;
    Rigidbody2D rb;
    Vector2 direction = Vector2.zero;
    void Start()
    {
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
        } else
        {
            walking = false;
        }
        LookAtMouse();
        animator.SetBool("isWalking", walking);
    }

    public void LookAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = antVisual.position.z -  Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 aimDirection = mouseWorldPos - antVisual.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        antVisual.rotation = Quaternion.Euler(0f, 0f, angle -90f);
    }
}
