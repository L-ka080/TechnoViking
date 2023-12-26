using System.Collections.Generic;
using UnityEngine;

public class AimPointerController : MonoBehaviour
{
    public List<EnemyStats> selectedTargets;
    private CircleCollider2D aimArea;
    private PlayerStats playerStats;

    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        aimArea = GetComponent<CircleCollider2D>();
        playerStats = transform.parent.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        adjustAimArea();
    }

    private void adjustAimArea()
    {
        aimArea.offset = (Vector2)transform.localPosition.normalized * -1;
    }

    public void PrimaryAttack()
    {
        foreach (EnemyStats enemy in selectedTargets)
        {
            enemy.TakeDamage(playerStats.primaryWeaponDamage, playerStats.primaryWeaponKnockback, transform.localPosition.normalized);
        }
    }

    public void SecondaryAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.parent.position, Quaternion.identity);
        
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.damage = playerStats.secondaryWeaponDamage;
        projectileController.TTL = playerStats.secondaryWeaponProjectileDistance / playerStats.secondaryWeaponProjectileSpeed;

        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        projectileRB.velocity = transform.localPosition * playerStats.secondaryWeaponProjectileSpeed; //FIXME Targeting with mouse position
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.TryGetComponent<EnemyStats>(out EnemyStats enemy) && !selectedTargets.Contains(enemy))
        {
            selectedTargets.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.TryGetComponent<EnemyStats>(out EnemyStats enemy) && selectedTargets.Contains(enemy))
        {
            selectedTargets.Remove(enemy);
        }
    }
}