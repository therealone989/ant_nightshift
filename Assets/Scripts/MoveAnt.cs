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
        if(Input.GetKey(KeyCode.W))
        {
            walking = true;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            walking = true;
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        } else if(Input.GetKey(KeyCode.D))
        {
            walking = true;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            walking = true;
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else
        {
            walking = false;
        }
        animator.SetBool("isWalking", walking);
    }
}
