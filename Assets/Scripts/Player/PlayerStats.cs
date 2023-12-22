using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int PlayerHealth;
    private int maxPlayerHealth = 4;
    [SerializeField] UIAnimationHandler uIAnimationHandler;

    public bool isPlayerDead = false;

    private void Awake()
    {
        PlayerHealth = maxPlayerHealth;
    }

    public void TakeDamage(int damage)
    {
        if (PlayerHealth > 0)
        {
            uIAnimationHandler.TakeDamageAnimation();
            PlayerHealth -= damage;
        }
    }

    public void GainHealth(int health)
    {
        if (PlayerHealth < maxPlayerHealth)
        {
            uIAnimationHandler.GainHealthAnimation();
            PlayerHealth += health;
        }
    }

    public void DealDamage(int damage)
    {
        //TODO Implement this in the near future
    }
}
