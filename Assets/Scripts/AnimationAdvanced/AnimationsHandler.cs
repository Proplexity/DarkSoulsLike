using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class AnimationsHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        public Animator _anim;
         ControllerInput _input;
         PlayerLandController _playerController;
        public int _vertical;
        int _horizontal;
        public bool canRotate = false;
        public bool isRolling = false; 

        public void Initialize()
        {
            _anim = GetComponent<Animator>();
            _input = GetComponentInParent<ControllerInput>();
            _playerController = GetComponentInParent<PlayerLandController>();
            playerManager = GetComponentInParent<PlayerManager>();
             _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Horizontal
            float h = 0.0f;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1.0f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1.0f;
            }
            else
            {
                h = 0;
            }
            #endregion

            #region Vertical

            float v = 0.0f;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1.0f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1.0f;
            }
            else 
            {
                v = 0;
            }

            
            #endregion

             if(_input._moveAmount > 0)
             {
                 if (isSprinting)
                 {
                     v = 2;
                     h = 0;
                 }

             } 

           

            _anim.SetFloat(_vertical, v, 0.1f, Time.deltaTime);
            _anim.SetFloat(_horizontal, h, 0.1f, Time.deltaTime);
        }


       
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            


            _anim.applyRootMotion = isInteracting;
            _anim.SetBool("IsInteracting", isInteracting);
           
            if(targetAnim == "StepBack")
            {
                _anim.CrossFade(targetAnim, .01f);
            }
            else
            {
                _anim.CrossFade(targetAnim, .5f);
            }

            if (targetAnim == "Roll")
            {
                isRolling = true;
                Invoke(nameof(PutFalse), 1);
                
            }
           
        }

        private void PutFalse()
        {
            isRolling = false;
        }

        public void EnableCombo()
        {
            _anim.SetBool("CanDoCombo", true);
        }

        public void DisableCombo()
        {
            _anim.SetBool("CanDoCombo", false);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {

            if (playerManager.isInteracting == false)
                return;
            

           
            float delta = Time.deltaTime;
            _playerController._rb.drag = 0;
            Vector3 deltaPos = _anim.deltaPosition;
            deltaPos.y = 0;
            Vector3 velocity = deltaPos / delta;
            _playerController._rb.velocity = velocity;
           
        } 
    }
}
