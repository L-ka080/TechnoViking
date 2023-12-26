using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    [SerializeField] private int healthRestored;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.TryGetComponent<PlayerStats>(out PlayerStats playerStats);
            playerStats.GainHealth(healthRestored);
            Destroy(gameObject);
        }
    }
}
