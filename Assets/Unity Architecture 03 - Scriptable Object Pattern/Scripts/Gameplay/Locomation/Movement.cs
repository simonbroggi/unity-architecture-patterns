using UnityEngine;

namespace UnityArchitecture.ScriptableObjectPattern
{
    public class Movement : MonoBehaviour
    {
        [field:SerializeField] public Level Level { get; private set;}

        public void Construct(Level newLevel)
        {
            Level = newLevel;
        }
        
        public Vector3 velocity { get; private set; }
        public Vector3 lookDirection { get; private set; }
        
        public float acceleration { get; private set; }

        private UnityEngine.Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }
        
        public void LateUpdate()
        {
            var position = _transform.position;
            
            // clamp newPosition to level bounds
            var x = Mathf.Clamp(position.x, -Level.Bounds.x, Level.Bounds.x);
            var y = Mathf.Clamp(position.z, -Level.Bounds.y, Level.Bounds.y);
            
            var newPosition = new Vector3(x, position.y, y);

            if (velocity.magnitude > 0.01f)
            {
                _transform.position = newPosition + velocity * Time.deltaTime;    
            }

            if (lookDirection.magnitude > 0.01f)
            {
                _transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        
        public void SetVelocity(Vector3 newVelocity)
        {
            velocity = newVelocity;
        }

        public void SetLookDirection(Vector3 newDirection)
        {
            lookDirection = newDirection;
        }

        public void SetVelocityAndLookDirection(Vector3 combinedVelocity)
        {
            velocity = combinedVelocity;
            lookDirection = combinedVelocity.normalized;
        }
        
        public void SetVelocityAndLookDirection(Vector3 newVelocity, Vector3 newLookDirection)
        {
            velocity = newVelocity;
            lookDirection = newLookDirection;
        }
        
        public void AddVelocity(Vector3 newVelocity)
        {
            velocity += newVelocity;
        }
        
        public void SetAcceleration(float newAcceleration)
        {
            acceleration = newAcceleration;
        }
    }
}