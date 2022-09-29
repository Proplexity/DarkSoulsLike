using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Proplexity
{
    public class ControllerInput : MonoBehaviour
    {
        public float _horizontal;
        public float _vertical;
        public float _moveAmount;
        public float _mouseX;
        public float _mouseY;

        
        public bool b_input = false;
        public bool a_input = true;
        public bool RB_input = false;
        public bool RT_input = false;
        public bool jump_input;
        public bool inventory_input;
        public bool lockOn_Input;
        public bool right_Stick_Left;
        public bool right_Stick_Right;


        public bool d_Up = false;
        public bool d_Down = false;
        public bool d_Left = false;
        public bool d_LeftPressedLastFrame = false;
        public bool d_Right = false;
        public bool d_RightPressedLastFrame = false;


        public bool rollFlag = false;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool sprintFlag = false;
        public bool inventoryFlag;
        public float rollInputTimer;
        

        InputActions _input = null;
        PlayerLandController _landController;
        PlayerAttacker _attacker;
        PlayerInventory _inventory;
        PlayerManager _playerManager = null;
        UIManager _uiManager;
        public CameraHandler _cameraHandler;
        

        Vector2 _movementInput;
        Vector2 _cameraInput;



        private void Awake()
        {
            _attacker = GetComponent<PlayerAttacker>();
            _inventory = GetComponent<PlayerInventory>();
            _playerManager = GetComponent<PlayerManager>();
            _uiManager = FindObjectOfType<UIManager>();
            _cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void OnEnable()
        {
            if (_input == null)
            {
                _input = new InputActions();
                _input.playerLandController.Enable();
                _input.PlayerActions.Enable();
                _input.Dpad.Enable();
               

                _input.playerLandController.Move.performed += SetMove;
                _input.playerLandController.Move.canceled += SetMove;

                _input.playerLandController.Camera.performed += SetLook;
                _input.playerLandController.Camera.canceled += SetLook;

                _input.playerLandController.LockOnTargetRight.started += SetTargetSwitchRight;
                _input.playerLandController.LockOnTargetRight.canceled += SetTargetSwitchRight;

                _input.playerLandController.LockOnTargetLeft.started += SetTargetSwitchLeft;
                _input.playerLandController.LockOnTargetLeft.canceled += SetTargetSwitchLeft;

                _input.PlayerActions.Roll.started += SetRoll;
                _input.PlayerActions.Roll.canceled += SetRoll;

                _input.PlayerActions.RB.started += SetRB;
                _input.PlayerActions.RB.canceled += SetRB;

                _input.PlayerActions.RT.started += SetRT;
                _input.PlayerActions.RT.canceled += SetRT;

                _input.PlayerActions.Jump.started += SetJump;
                _input.PlayerActions.Jump.canceled += SetJump;

                _input.PlayerActions.Interact.started += SetInteract;
                _input.PlayerActions.Interact.canceled += SetInteract;

                _input.PlayerActions.Inventory.started += SetInventory;
                _input.PlayerActions.Inventory.canceled += SetInventory;

                _input.PlayerActions.LockOn.started += SetLockOn;
                _input.PlayerActions.LockOn.canceled += SetLockOn;


                _input.Dpad.DpadUp.started += SetDpadUP;
                _input.Dpad.DpadUp.canceled += SetDpadUP;

                _input.Dpad.DpadRight.started += SetDpadRight;
                _input.Dpad.DpadRight.canceled += SetDpadRight;

                _input.Dpad.DpadLeft.started += SetDpadLeft;
                _input.Dpad.DpadLeft.canceled += SetDpadLeft;

                _input.Dpad.DpadDown.started += SetDpadDown;
                _input.Dpad.DpadDown.canceled += SetDpadDown;

            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.playerLandController.Move.performed -= SetMove;
                _input.playerLandController.Move.canceled -= SetMove;

                _input.playerLandController.Camera.performed -= SetLook;
                _input.playerLandController.Camera.canceled -= SetLook;

                _input.playerLandController.LockOnTargetRight.started -= SetTargetSwitchRight;
                _input.playerLandController.LockOnTargetRight.canceled -= SetTargetSwitchRight;

                _input.playerLandController.LockOnTargetLeft.started -= SetTargetSwitchLeft;
                _input.playerLandController.LockOnTargetLeft.canceled -= SetTargetSwitchLeft;

                _input.PlayerActions.Roll.started -= SetRoll;
                _input.PlayerActions.Roll.canceled -= SetRoll;

                _input.PlayerActions.RB.started -= SetRB;
                _input.PlayerActions.RB.canceled -= SetRB;

                _input.PlayerActions.RT.started -= SetRT;
                _input.PlayerActions.RT.canceled -= SetRT;

                _input.PlayerActions.Jump.started -= SetJump;
                _input.PlayerActions.Jump.canceled -= SetJump;

                _input.PlayerActions.Interact.started -= SetInteract;
                _input.PlayerActions.Interact.canceled -= SetInteract;

                _input.PlayerActions.Inventory.started -= SetInventory;
                _input.PlayerActions.Inventory.canceled -= SetInventory;

                _input.PlayerActions.LockOn.started -= SetLockOn;
                _input.PlayerActions.LockOn.canceled -= SetLockOn;

                _input.Dpad.DpadUp.started -= SetDpadUP;
                _input.Dpad.DpadUp.canceled -= SetDpadUP;

                _input.Dpad.DpadRight.started -= SetDpadRight;
                _input.Dpad.DpadRight.canceled -= SetDpadRight;

                _input.Dpad.DpadLeft.started -= SetDpadLeft;
                _input.Dpad.DpadLeft.canceled -= SetDpadLeft;

                _input.Dpad.DpadDown.started -= SetDpadDown;
                _input.Dpad.DpadDown.canceled -= SetDpadDown;

                _input.playerLandController.Disable();
                _input.PlayerActions.Disable();
                _input.Dpad.Disable();

            }
        }

        private void SetMove(InputAction.CallbackContext ctx)
        {
            _movementInput = ctx.ReadValue<Vector2>();
           
        }

        private void SetLook(InputAction.CallbackContext ctx)
        {
            _cameraInput = ctx.ReadValue<Vector2>();
        }

        private void SetRoll(InputAction.CallbackContext ctx)
        {
            b_input = ctx.started;
        }

        private void SetRB(InputAction.CallbackContext ctx)
        {
            RB_input = ctx.started;
        }

        private void SetRT(InputAction.CallbackContext ctx)
        {
            RT_input = ctx.started;
        }

        private void SetJump(InputAction.CallbackContext ctx)
        {
            jump_input = ctx.started;
        }

        private void SetInteract(InputAction.CallbackContext ctx)
        {
            a_input = ctx.started;
            Debug.Log(a_input);
           
        }

        private void SetLockOn(InputAction.CallbackContext ctx)
        {
            lockOn_Input = ctx.started;
        }

        private void SetTargetSwitchLeft(InputAction.CallbackContext ctx)
        {
            right_Stick_Left = ctx.started;
        }

        private void SetTargetSwitchRight(InputAction.CallbackContext ctx)
        {
            right_Stick_Right = ctx.started;
        }

        private void SetDpadUP(InputAction.CallbackContext ctx)
        {
            d_Up = ctx.started;
        }

        private void SetDpadDown(InputAction.CallbackContext ctx)
        {
            d_Down = ctx.started;
        }

        private void SetDpadLeft(InputAction.CallbackContext ctx)
        {
            d_Left = ctx.started;
            Debug.Log("left");
        }

        private void SetDpadRight(InputAction.CallbackContext ctx)
        {
            d_Right = ctx.started;
            Debug.Log("right");
        }

        private void SetInventory(InputAction.CallbackContext ctx)
        {
            inventory_input = ctx.started;
        }


        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            QuickChangeItem(delta);
            HandleInventoryInput(delta);
            HandleLockOnInput();
        }

        private void MoveInput(float delta)
        {
            _horizontal = _movementInput.x;
            _vertical = _movementInput.y;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
            _mouseX = _cameraInput.x;
            _mouseY = _cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            if (_playerManager != null)
            {
                if (_playerManager.isGrounded)
                {
                   // Debug.Log(b_input);
                }
            }
            sprintFlag = b_input;


            if (b_input)
            {
              //  rollFlag = true;
                rollInputTimer += Time.deltaTime;
                
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
            
        }

        private void HandleAttackInput(float delta)
        {
            if (RB_input)
            {
                
                if (_playerManager.canDoCombo)
                {
                    comboFlag = true;
                    _attacker.HandleWeaponCombo(_inventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (_playerManager.isInteracting)
                        return;
                    if (_playerManager.canDoCombo)
                        return;
                    _attacker.HandleLightAttack(_inventory.rightWeapon); // the model will freeze because the code is telling it to play an animation it doesnt have and the error freezes and overrides the models exit animation. set an exeption for unarmed
                    //_attacker.HandleLightAttack(_inventory.leftWeapon);
                }
            }

            if (RT_input)
            {
                if (_playerManager.canDoCombo)
                {
                    comboFlag = true;
                    _attacker.HandleWeaponCombo(_inventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (_playerManager.isInteracting)
                        return;
                    if (_playerManager.canDoCombo)
                        return;

                    _attacker.HandleHeavyAttack(_inventory.rightWeapon);
                  //  _attacker.HandleHeavyAttack(_inventory.leftWeapon);
                }
            }
        }

        private void QuickChangeItem(float delta)
        {
            if (d_Right && !d_RightPressedLastFrame)
            {
                _inventory.ChangeWeaponInRightHand();
                
            }
            d_RightPressedLastFrame = d_Right;
            if (d_Left && !d_LeftPressedLastFrame)
            {
                _inventory.ChangeWeaponInLeftHand();
                
            }
            d_LeftPressedLastFrame = d_Left;
        }

        private void HandleInventoryInput(float delta)
        {
            if (inventory_input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    _uiManager.OpenSlectWindow();
                    _uiManager.UpdateUI();
                    _uiManager.hudWindow.SetActive(false);
                    Debug.Log("1");
                }
                else
                {
                    _uiManager.CloseSelectWindow();
                    _uiManager.CloseAllIinventoryWindows();
                    _uiManager.hudWindow.SetActive(true);
                    
                    Debug.Log("2");
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                
                lockOn_Input = false;
                _cameraHandler.HandleLockOn();
                
                if (_cameraHandler.nearestLockOnTraget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.nearestLockOnTraget;
                    lockOnFlag = true;
                }
                


            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                _cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left)
            {
                right_Stick_Left = false;
                _cameraHandler.HandleLockOn();
                if (_cameraHandler.leftLockTarget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.leftLockTarget;
                }
            }
            else if (lockOnFlag && right_Stick_Right)
            {
                right_Stick_Right = false;
                _cameraHandler.HandleLockOn();
                if (_cameraHandler.rightLockTarget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.rightLockTarget;
                }

            }

            Debug.Log("lockOnFlag:" + lockOnFlag);
            Debug.Log("lockOnInput:" + lockOn_Input);
        }

        
    }
}

