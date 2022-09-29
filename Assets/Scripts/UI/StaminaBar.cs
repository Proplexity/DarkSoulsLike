using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proplexity
{
    public class StaminaBar : MonoBehaviour
    {
        Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        public void SetCurrentStamina(int currentStamina)
        {
            slider.value = currentStamina;
        }

        public void UpdateStamina(float a)
        {
            slider.value += a;
        }
    }
}
