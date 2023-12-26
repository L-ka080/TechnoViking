using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    Rigidbody2D rb;

    private Vector2 playerMovement;
    private SpriteRenderer spriteRenderer;
    private PlayerStats playerStats;

    [SerializeField] private Transform dashPoint;
    [SerializeField] private Transform aimPoint;
    private Vector2 cursorMovement;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        
        playerStats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = playerMovement * playerStats.playerSpeed * 10f * Time.deltaTime;
        dashPoint.localPosition = playerMovement * playerStats.dashPower;
    }

    public void OnMove(InputValue inputValue)
    {
        playerMovement = inputValue.Get<Vector2>();

        if (playerMovement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerMovement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnPrimaryAction()
    {
        if (playerStats.attackCooldown <= 0)
        {
            aimPoint.SendMessage("PrimaryAttack");
            playerStats.attackCooldown = 1;
        }
    }

    public void OnSecondaryAction()
    {
        if (playerStats.energyLeft >= 1)
        {
            aimPoint.SendMessage("SecondaryAttack");
            playerStats.energyLeft -= 1;
        }
    }

    public void OnAim(InputValue inputValue)
    {
        cursorMovement = inputValue.Get<Vector2>();

        Vector3 movementTranslation = Camera.main.ScreenToViewportPoint((Vector3)cursorMovement);

        Vector3 aimPointMovement = new Vector3(movementTranslation.x - 0.5f, movementTranslation.y - 0.5f);

        aimPoint.localPosition = aimPointMovement.normalized * playerStats.attackRadius;
    }

    public void OnDash()
    {
        if (playerStats.energyLeft >= 1)
        {
            rb.MovePosition(dashPoint.position);
            playerStats.energyLeft -= 1;
        }
    }
}
