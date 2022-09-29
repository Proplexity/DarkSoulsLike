using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{

    public class CameraHandler : MonoBehaviour
    {
        ControllerInput controller;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;
        

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimunPivot = -35.0f;
        public float maximunPivot = 35.0f;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        [SerializeField] public Transform currentLockOnTarget;
        public Transform nearestLockOnTraget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        [SerializeField] List<CharacterManager> availableTargtes = new List<CharacterManager>();
        public float maximunLockOnDistance = 30;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            controller = FindObjectOfType<ControllerInput>();
        }

        public void FollowTarget(float delta)
        {
            Vector3 tragetPos = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position,
                ref cameraFollowVelocity,
                delta / followSpeed);
            myTransform.position = tragetPos;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {

            if (/*controller.lockOnFlag == false && */ currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimunPivot, maximunPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                float velocity = 0;

                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit Hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out Hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraTransform.position, Hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;

        }

        public void HandleLockOn()
        {
            availableTargtes.Clear();
            float shortestDistanceFromTarget = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 50);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();
                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximunLockOnDistance)
                    {
                        availableTargtes.Add(character);
                    }
                }
            }

            for (int k = 0; k < availableTargtes.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargtes[k].transform.position);

                if (distanceFromTarget < shortestDistanceFromTarget)
                {
                    shortestDistanceFromTarget = distanceFromTarget;
                    nearestLockOnTraget = availableTargtes[k].lockOnTransform;
                }

               if (controller.lockOnFlag)
                {
                    Vector3 relativeEnemyPos = currentLockOnTarget.InverseTransformPoint(availableTargtes[k].transform.position);
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargtes[k].transform.position.x;
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargtes[k].transform.position.x;

                    if (relativeEnemyPos.x < 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargtes[k].lockOnTransform;
                    }

                    if (relativeEnemyPos.x > 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargtes[k].lockOnTransform;
                    }
                } 
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargtes.Clear();
            nearestLockOnTraget = null;
            currentLockOnTarget = null;
            
        }
    }
}

