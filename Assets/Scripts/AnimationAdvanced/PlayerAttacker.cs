using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class PlayerAttacker : MonoBehaviour
    {

        AnimationsHandler animationsHandler;
        ControllerInput _input;
        WeaponSlotManager _weaponSlotManager;

        public string lastAttack;

        private void Awake()
        {
            animationsHandler = GetComponentInChildren<AnimationsHandler>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _input = GetComponent<ControllerInput>();
            
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (_input.comboFlag)
            {
                animationsHandler._anim.SetBool("CanDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1 && _input.RB_input)
                {
                    animationsHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }

                if (lastAttack == weapon.OH_Heavy_Attack_1 && _input.RT_input)
                {
                    animationsHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_2, true);
                }
            }
        } 

        public void HandleLightAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;
            animationsHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            _weaponSlotManager.attackingWeapon = weapon;
            animationsHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
}
