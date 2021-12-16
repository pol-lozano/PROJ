using System;
using UnityEngine;

namespace NewCamera {
        
    [Serializable]
    [CreateAssetMenu(menuName = "Camera/Camera Behaviours/Base Behaviour", fileName = "Base Behaviour")]
    public class BaseCameraBehaviour : ScriptableObject {
        
        [SerializeField] protected BehaviourData behaviourValues;
        
        protected Vector3 referenceVelocity;
        protected Quaternion previousRotation;
        protected Transform thisTransform;
        protected Transform pivotTarget;
        protected Transform characterModel;

        //"Constructor"
        public virtual void InjectReferences(Transform transform, Transform pivotTarget, Transform characterModel) {
            thisTransform = transform;
            this.pivotTarget = pivotTarget;
            this.characterModel = characterModel;
        }

        public virtual void EnterBehaviour() {
            previousRotation = pivotTarget.rotation;
            pivotTarget.localRotation = Quaternion.Euler(0, 0, 0);
        }
 


        public virtual Vector3 ExecuteMove(Vector3 calculatedOffset) {
            return Vector3.SmoothDamp(thisTransform.position, pivotTarget.position + calculatedOffset, ref referenceVelocity, behaviourValues.FollowSpeed);
        }

        public virtual Quaternion ExecuteRotate() {
            
            Quaternion targetRotation = Quaternion.LookRotation((pivotTarget.position - thisTransform.position) + pivotTarget.forward * behaviourValues.CameraLookAhead);
            
            return Quaternion.Slerp(thisTransform.rotation, targetRotation, Time.deltaTime * behaviourValues.RotationSpeed);
        }

        public virtual Vector3 ExecuteCollision(GlobalCameraSettings data) {
            
            Vector3 collisionOffset = pivotTarget.rotation * behaviourValues.Offset;
            
            if (Physics.SphereCast(pivotTarget.position, data.CollisionRadius, collisionOffset.normalized, out var hitInfo, collisionOffset.magnitude, data.CollisionMask))
                collisionOffset = collisionOffset.normalized * hitInfo.distance;

            return collisionOffset;
        }

        public virtual void ManipulatePivotTarget(CustomInput input) {

            Vector3 desiredRotation = previousRotation.eulerAngles;
            
            if (input.aim != Vector2.zero) {

                desiredRotation = pivotTarget.eulerAngles + (Vector3)input.aim;

                desiredRotation = PreventCircleReset(desiredRotation);

                desiredRotation.x = Mathf.Clamp(desiredRotation.x, behaviourValues.ClampValues.x, behaviourValues.ClampValues.y);
            }
            else {
                desiredRotation = previousRotation.eulerAngles;
            }
            
            pivotTarget.eulerAngles = desiredRotation;
            previousRotation = pivotTarget.rotation;
        }

        protected T BehaviourData<T>() where T : BehaviourData => behaviourValues as T;

        protected Vector3 PreventCircleReset(Vector3 input) {
            if (input.x > 180)
                input.x -= 360;
            if (input.y > 180)
                input.y -= 360;

            return input;
        }
    }
}