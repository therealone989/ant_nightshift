using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] int playerDamage = 10;
    [SerializeField] Animator anim;
    [SerializeField] float attackCooldown = 0.5f;
    private float elapsedTime = 0f;
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && elapsedTime >= attackCooldown) 
        {
            elapsedTime = 0f;
            anim.SetTrigger("Attack");
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, 1f, hitLayer);

            if (hit.collider != null)
            {
                
                if (hit.collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
                {
                    enemy.TakeDamage(playerDamage);
                }
            }
        }
    }
}
