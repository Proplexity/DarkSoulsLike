using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class PlayerManager : CharacterManager
    {

        ControllerInput _input;
        PlayerLandController _controller;
        Animator anim;
        CameraHandler _cameraHandler = null;
        AnimationsHandler _animHandler = null;
        InteractableUI _interactableUI = null;

        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;






        void Start()
        {
            _input = GetComponent<ControllerInput>();
            anim = GetComponentInChildren<Animator>();
            _controller = GetComponent<PlayerLandController>();
            _interactableUI = FindObjectOfType<InteractableUI>();
        }

        private void Awake()
        {
           // Cursor.lockState = CursorLockMode.Locked;
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _animHandler = GetComponentInChildren<AnimationsHandler>();

        }

        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("IsInteracting");
            canDoCombo = anim.GetBool("CanDoCombo");
            anim.SetBool("IsInAir", isInAir);
            _input.TickInput(delta);
            _controller.HandleRollingAndSprinting(delta);
            _controller.HandleJumping();

            if (_animHandler.isRolling)
            {
                anim.SetFloat(_animHandler._vertical, 0.5f, 0.1f, Time.deltaTime);
                // there's an issue where after rolling the player stands up and then starts running instead of standing up still.
                // this just makes him do a walk anim instead of runnig b/c when you roll most likely you vertical will be above 1 having him walk instead of run when he stands up just helps disguise it.
            }

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            _controller.HandleMovement(delta);
            
            _controller.HandleFalling(delta, _controller._moveDirection);
            

        }

        private void LateUpdate()
        {
            isSprinting = _input.b_input;
            _input.rollFlag = false;
           // _input.lockOn_Input = false;
            _input.RB_input = false;
            _input.RT_input = false;
            _input.jump_input = false;
            _input.inventory_input = false;


            float delta = Time.deltaTime;
            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _input._mouseX, _input._mouseY);
            }

            if (isInAir)
            {
                _controller.InAirTimer = _controller.InAirTimer + Time.deltaTime;
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.3f);
            Gizmos.DrawRay(transform.position, transform.forward);
            
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, -transform.forward, out hit, 0.5f, _cameraHandler.ignoreLayers))
            {
                
                if (hit.collider.tag == "interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        _interactableUI.inetractableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (_input.a_input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            _input.a_input = false;

                        }
                    }

                }
            }
            else
            {

                
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && _input.a_input)
                {
                    itemInteractableGameObject.SetActive(false);
                }

                

            }
        }

       



    }
}

