using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] int playerDamage = 10;
    [SerializeField] Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, 1f, hitLayer);

            if (hit.collider != null)
            {
                
                if (hit.collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
                {
                    enemy.TakeDamage(playerDamage);
                    anim.SetTrigger("Attack");
                }
            }
        }
    }
}
