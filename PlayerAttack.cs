using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    private PlayerController playerController;
    private float cooldownTimer = Mathf.Infinity;
    private bool hasEncounteredBadTile = false;  // Add this line

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerController.canAttack() && !hasEncounteredBadTile)
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        cooldownTimer = 0;

        bullets[FindBullet()].transform.position = firePoint.position;
        bullets[FindBullet()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
