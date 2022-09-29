using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnamred;

        [Header("Idle Animation")]
        public string Right_Hand_Idle;
        public string Left_Hand_Idle;

        [Header("One Handed Attack Animation")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;

        [Header("Stamina Cost")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}
