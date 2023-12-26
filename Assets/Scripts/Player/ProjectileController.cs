using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float TTL;
    public int damage;

    private void Update()
    {
        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = collider.transform.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

}