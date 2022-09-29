using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Proplexity
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel;
        public int maxStamina;
        public int currentStamina;

        [SerializeField] [Range(0, 1)] float staminaInterger;


        public HealthBar healthBar;
        public StaminaBar staminaBar;

        AnimationsHandler animationsHandler;

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
            animationsHandler = GetComponentInChildren<AnimationsHandler>();
        }

        private void Update()
        {

         //   staminaBar.UpdateStamina(staminaInterger);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }


        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);
            animationsHandler.PlayTargetAnimation("TakingDamage", true);

            if (currentHealth <= 0)
            {
               // currentHealth = 0;
                animationsHandler.PlayTargetAnimation("Dead", true);
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }


    }
}
