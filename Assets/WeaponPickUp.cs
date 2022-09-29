using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Proplexity
{
    public class WeaponPickUp : Interactable
    {

        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            PickUpItem(playerManager);
        }


        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLandController playerLandController;
            AnimationsHandler animationsHandler;
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLandController = playerManager.GetComponent<PlayerLandController>();
            animationsHandler = playerManager.GetComponentInChildren<AnimationsHandler>();

            playerLandController._rb.velocity = Vector3.zero;
            animationsHandler.PlayTargetAnimation("PickUpItem", true);
            playerInventory.weaponInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }






    } 
}
