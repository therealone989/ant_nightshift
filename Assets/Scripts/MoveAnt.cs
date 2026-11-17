using UnityEngine;

public class MoveAnt : MonoBehaviour
{

    [SerializeField] bool walking = false;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Transform antVisual;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] float dashCooldown = 2f;

    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    Rigidbody2D rb;
    Vector2 direction = Vector2.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldownTimer = dashCooldown;
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if(isDashing)
        {
            dashTimer += Time.deltaTime;
        }
        if(dashTimer > dashDuration)
        {
            isDashing = false;
        }
        Move();
    }
    private void FixedUpdate()
    {
        if(!isDashing)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        } else
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
        }
        
    }
    public void Move()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W)){direction += Vector2.up;}
        if (Input.GetKey(KeyCode.D)){direction += Vector2.right;}
        if (Input.GetKey(KeyCode.A)){direction += Vector2.left;}
        if (Input.GetKey(KeyCode.S)){direction += Vector2.down;}
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            // DASH
            if (direction != Vector2.zero && !isDashing && cooldownTimer >= dashCooldown) { 
                isDashing = true;
                cooldownTimer = 0f;
                dashTimer = 0f;
                dashDirection = direction.normalized;
                Debug.Log("DAS GESTARTET");
            } 
        }
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

    public float GetDashCooldown()
    {
        float fillAmount = 1 - (cooldownTimer / dashCooldown);
        return fillAmount;
    }
}
