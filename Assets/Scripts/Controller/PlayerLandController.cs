using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class PlayerLandController : MonoBehaviour
    {
        Transform _cameraObject;
        ControllerInput _input;
        PlayerManager _playerManager;
        public Vector3 _moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimationsHandler animationsHandler;

        public Rigidbody _rb;
        public GameObject _normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimunDistanceToBeginFalls = 1f;
        [SerializeField] float grounDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        [SerializeField] public float InAirTimer;
        [SerializeField] float pushOffLedgeAmount;


        [Header("Movement Stats")]
        [SerializeField] float _movementSpeed = 5.0f;
        [SerializeField] float SprintMultiplier;
        [SerializeField] float _rotationSpeed = 10.0f;
        [SerializeField] float sprintSpeed;
        [SerializeField] float fallSpeed = 45;


        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _input = GetComponent<ControllerInput>();
            _playerManager = GetComponent<PlayerManager>();
            animationsHandler = GetComponentInChildren<AnimationsHandler>();
            _cameraObject = Camera.main.transform;
            myTransform = transform;
            animationsHandler.Initialize();

            _playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

        }

      
      

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _input._moveAmount;

            targetDir = -_cameraObject.forward * _input._vertical;
            targetDir += -_cameraObject.right * _input._horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;



        }
        public void HandleMovement(float delta)
        {
            if (_input.rollFlag)
                return;

            if (_playerManager.isInteracting)
                return;

            _moveDirection = _cameraObject.forward * _input._vertical;
            _moveDirection += _cameraObject.right * _input._horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            float speed = _movementSpeed;
             if(_input._moveAmount > 0.5)
             {
                 if (_input.sprintFlag)
                 {
                     speed = sprintSpeed;
                     _playerManager.isSprinting = true;
                     _moveDirection *= speed;
                 }
                 else
                 {
                     _moveDirection *= speed;
                 }
             } 

           


            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, normalVector);
            _rb.velocity = projectedVelocity;

            animationsHandler.UpdateAnimatorValues(_input._moveAmount, 0, _playerManager.isSprinting);
           
            if (animationsHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }
        public void HandleRollingAndSprinting(float delta)
        {
            if (animationsHandler._anim.GetBool("IsInteracting"))
                return;
            if (_input.rollFlag)
            {
                _moveDirection = _cameraObject.forward * _input._vertical;
                _moveDirection = _cameraObject.forward * _input._horizontal;

                if (_input._moveAmount > 0)
                {
                    animationsHandler.PlayTargetAnimation("Roll", true);
                    _moveDirection.y = 0;
                   // Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
                   // myTransform.rotation = rollRotation;
                }
                else
                {
                    animationsHandler.PlayTargetAnimation("StepBack", true);
                }
            }
        }
        public void HandleFalling(float delta, Vector3 moveDirection)
        { 
            _playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y = origin.y + groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if(_playerManager.isInAir)
            {
                _rb.AddForce(-Vector3.up * fallSpeed);
                _rb.AddForce(moveDirection * pushOffLedgeAmount);
            }

            Vector3 direction = moveDirection;
            direction.Normalize();
            origin = origin + direction * grounDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimunDistanceToBeginFalls, Color.red, 0.1f, false);
            if(Physics.Raycast(origin, -Vector3.up, out hit, minimunDistanceToBeginFalls, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                _playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if(_playerManager.isInAir)
                {
                    if (InAirTimer >= 0.4f)
                    {
                        
                        Debug.Log($"You were in the air for  {InAirTimer}");
                        
                        
                        animationsHandler.PlayTargetAnimation("Land", true);
                        InAirTimer = 0;

                    }
                    else
                    {
                        animationsHandler.PlayTargetAnimation("Empty", false);
                        InAirTimer = 0;
                    } 

                    _playerManager.isInAir = false;

                }
            }
            else
            {
                if (_playerManager.isGrounded)
                {
                    _playerManager.isGrounded = false;
                }

                if (_playerManager.isInAir == false)
                {
                    if(_playerManager.isInteracting == false)
                    {
                        animationsHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = _rb.velocity;
                    vel.Normalize();
                    _rb.velocity = vel * (_movementSpeed / 2);
                    _playerManager.isInAir = true;
                }
            }

            if (_playerManager.isInteracting || _input._moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }

            if (_playerManager.isGrounded)
            {
                if(_playerManager.isInteracting || _input._moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, delta);
                }
                else
                {
                    myTransform.position = targetPosition;
                }

            }

        }

        public void HandleJumping()
        {
            if (_playerManager.isInteracting)
                return;
            if (_input.jump_input)
            {
                if (_input._moveAmount > 0)
                {
                    _moveDirection = -_cameraObject.forward * _input._vertical;
                    _moveDirection += -_cameraObject.right * _input._horizontal;
                    animationsHandler.PlayTargetAnimation("Jump", true);
                    _moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(_moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }


        #endregion


    }
}

