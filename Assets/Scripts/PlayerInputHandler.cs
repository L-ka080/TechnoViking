using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float playerSpeed;
    private Vector2 playerMovement;

    public float dashLeft
    {
        get
        {
            return _dashLeft;
        }
        set
        {
            _dashLeft = value;

            if (_dashLeft > maxDashLeft)
            {
                _dashLeft = maxDashLeft;
            }
            else if (_dashLeft < 0)
            {
                _dashLeft = 0;
            }
        }
    }
    private float _dashLeft;
    [SerializeField] private int maxDashLeft;
    [SerializeField] private float dashPower;
    [SerializeField] private Transform dashPoint;


    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();

        dashLeft = maxDashLeft;
    }

    private void Update()
    {
        dashLeft += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.velocity = playerMovement * playerSpeed * 10f * Time.deltaTime;
        dashPoint.localPosition = playerMovement * dashPower;
    }

    public void OnMove(InputValue inputValue)
    {
        playerMovement = inputValue.Get<Vector2>();
    }

    public void OnPrimaryAction() {
        transform.SendMessage("DealDamage", 1);
    }

    public void OnDash(InputValue inputValue)
    {
        if (dashLeft >= 1)
        {
            rb.MovePosition(dashPoint.position);
            dashLeft -= 1;
        }
    }
}
