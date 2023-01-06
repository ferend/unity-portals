using UnityEngine;

namespace _Project.Scripts.Helpers.Mechanics
{

    [RequireComponent(typeof(CharacterController))]
    public class FirstPerson : Mechanic {

        public bool lockCursor;
        public float gravity = 12f;
        public float jumpForce = 20;
        public float moveSpeed = 5;
        public Vector2 mouseSensitivity;
        public Vector2 verticalLookMinMax;
        public Transform cam;
        CharacterController _controller;
        float _pitch;
        float _velocityY;
        Vector3 _dirOld;

        void Start () {
            _controller = GetComponent<CharacterController> ();

            if (lockCursor) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        public override void KeyboardControls()
        {
            FirstPersonMovement();
        }
        
        private void FirstPersonMovement()
        {
            Vector3 moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"),0,Input.GetAxisRaw ("Vertical")).normalized;
            Vector2 mouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

            if (_controller.isGrounded) {
                _velocityY = 0;

                if (Input.GetKeyDown (KeyCode.Space)) {
                    _velocityY = jumpForce;
                }
            } else {
                moveDir = _dirOld;
            }

            transform.Rotate (Vector3.up * mouseInput.x * mouseSensitivity.x);
            _pitch += mouseInput.y * mouseSensitivity.y;
            _pitch = ClampAngle (_pitch, verticalLookMinMax.x, verticalLookMinMax.y);
            Quaternion yQuaternion = Quaternion.AngleAxis (_pitch, Vector3.left);
            cam.localRotation =  yQuaternion;

            _velocityY -= gravity * Time.deltaTime;
            Vector3 velocity = transform.TransformDirection(moveDir) * moveSpeed + Vector3.up * _velocityY;
            _controller.Move (velocity * Time.deltaTime);
            _dirOld = moveDir;
        }

        static float ClampAngle (float angle, float min, float max) {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp (angle, min, max);
        }
    }
}
