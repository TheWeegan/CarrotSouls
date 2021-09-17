using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 playerVelocity;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private int _health = 500;
    public int Health { get => _health; set { _health = value; } }

    [SerializeField]
    private Text _healthText;
    public Text HealthText { get => _healthText; set { _healthText = value; } }

    [SerializeField]
    GameObject _eventSystem;

    private void Awake() {
        _characterController = gameObject.AddComponent<CharacterController>();

        _healthText.text = _health.ToString();
    }

    private void Update() {
        ManageMovement();
        
    }

    void FixedUpdate()
    {
    }

    void ManageMovement() {
        if (_characterController.isGrounded && playerVelocity.y < 0) {
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

        if (Input.GetButtonDown("Jump") && _characterController.isGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Defeat() {
        if(_eventSystem.TryGetComponent(out CanvasHandler canvasHandler)) {
            canvasHandler.Defeat();
        }
    }
    public void Victory() {
        if (_eventSystem.TryGetComponent(out CanvasHandler canvasHandler)) {
            canvasHandler.Victory();

        }
    }
}
