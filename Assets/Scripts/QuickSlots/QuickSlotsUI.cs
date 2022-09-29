using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Proplexity
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWepaonIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotUI(bool isleft, WeaponItem weapon)
        {
            if (isleft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon = null;
                    rightWeaponIcon.enabled = false;
                }

            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWepaonIcon.sprite = weapon.itemIcon;
                    leftWepaonIcon.enabled = true;
                }
                else
                {
                    leftWepaonIcon = null;
                    leftWepaonIcon.enabled = false;
                }
                
            }
        }



    }
}
