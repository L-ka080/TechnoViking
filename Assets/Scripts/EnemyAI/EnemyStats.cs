using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [SerializeField] private int enemyHealth;
    [field: SerializeField] public float enemySpeed { get; private set; }
    [field: SerializeField] public int enemyDamage { get; private set; }
    [field: SerializeField] public float attackDelay { get; private set; }
    [field: SerializeField] public float attackDistance { get; private set; }
    [SerializeField] public List<GameObject> objectsToDrop;

    [SerializeField] public float deathCountDown;
    private float colorCooldown;
    private bool isDead = false;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDead)
        {
            AdjustColor();
        }
    }


    public void TakeDamage(int damage)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;

            transform.SendMessage("SetStateFoundPlayer");

            if (enemyHealth == 0)
            {
                transform.SendMessage("SetStateDead");
                isDead = true;
                spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f);

                return;
            }

            colorCooldown = 1.2f;
        }
    }

    public void TakeDamage(int damage, float knokback, Vector2 direction) //TODO Implement Knockback system
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;

            transform.SendMessage("SetStateFoundPlayer");

            if (enemyHealth == 0)
            {
                transform.SendMessage("SetStateDead");
                isDead = true;
                spriteRenderer.color = new Color(0.4f, 0.4f, 0.4f);

                return;
            }

            colorCooldown = 1.2f;
        }
    }

    private void AdjustColor()
    {
        float colorFactor = Mathf.Lerp(1f, 0.5f, colorCooldown);

        spriteRenderer.color = new Color(1f, colorFactor, colorFactor);

        if (colorCooldown > 0)
        {
            colorCooldown -= Time.deltaTime;
        }
    }
}