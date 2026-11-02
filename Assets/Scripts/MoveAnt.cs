using UnityEngine;

public class MoveAnt : MonoBehaviour
{

    [SerializeField] bool walking = false;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed = 10f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)){direction += Vector3.up;}
        if (Input.GetKey(KeyCode.D)){direction += Vector3.right;}
        if (Input.GetKey(KeyCode.A)){direction += Vector3.left;}
        if (Input.GetKey(KeyCode.S)){direction += Vector3.down;}

        if (direction != Vector3.zero)
        {
            walking = true;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        } else
        {
            walking = false;
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        animator.SetBool("isWalking", walking);
    }
}
