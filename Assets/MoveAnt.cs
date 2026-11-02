using UnityEngine;

public class MoveAnt : MonoBehaviour
{

    private bool walking = false;
    [SerializeField] Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            walking = true;
            animator.SetBool("isWalking", true);
        }else
        {
            walking = false;
            animator.SetBool("isWalking", false);
        }
    }
}
