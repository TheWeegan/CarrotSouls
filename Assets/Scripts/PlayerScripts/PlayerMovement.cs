using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Awake() {
        _characterController = gameObject.AddComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        ManageMovement();
    }

    void ManageMovement() {
        groundedPlayer = _characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }
        if (Input.GetKey(KeyCode.W)) {
            _characterController.Move(transform.forward * Time.deltaTime * playerSpeed);

        } else if (Input.GetKey(KeyCode.S)) {
            _characterController.Move(-transform.forward * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            _characterController.Move(-transform.right * Time.deltaTime * playerSpeed);

        } else if (Input.GetKey(KeyCode.D)) {
            _characterController.Move(transform.right * Time.deltaTime * playerSpeed);
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _characterController.Move(playerVelocity * Time.deltaTime);
    }
}
