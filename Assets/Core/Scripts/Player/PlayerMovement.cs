using System;
using UnityEngine;

namespace CottageCooking
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 4f;
        public float sprintSpeed = 8f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;

        [Header("Look Settings")]
        public Transform cameraTransform;
        public float mouseSensitivity = 2f;
        public float lookClamp = 80f;

        private CharacterController controller;
        private Vector3 velocity;
        private float verticalLookRotation = 0f;
        private float horizontalLookRotation = 0f;

        void Awake()
        {
            controller = GetComponent<CharacterController>();

        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            HandleLook();
            HandleMove();
            HandleMouseLock();
        }

        void HandleMouseLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        void HandleLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -lookClamp, lookClamp);

            transform.Rotate(Vector3.up * mouseX);


            horizontalLookRotation += mouseX;
            cameraTransform.rotation = Quaternion.Euler(verticalLookRotation, horizontalLookRotation, 0f);
        }

        void HandleMove()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;



            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (controller.isGrounded && Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(((move * speed) + velocity) * Time.deltaTime);
        }

    }
}