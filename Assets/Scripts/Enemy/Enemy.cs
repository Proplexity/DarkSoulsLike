using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class Enemy : MonoBehaviour
    {
        private new Collider collider;
        

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        [SerializeField] int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats =  other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
            if (playerStats.currentHealth <= 0)
            {
                collider.enabled = !collider.enabled;
            }
        }
    }
}
