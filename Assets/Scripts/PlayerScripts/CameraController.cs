using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera mainCamera;
    public float zoomSpeed = 5f;

    private Transform _cameraTransform;

    private float _cameraDistance = 5f;
    private float _cameraDistanceMin = 1f;
    private float _cameraDistanceMax = 20f;
    private float _sensitivity = 100f;

    private bool _lockCursor = true;

    void Start() {
        _cameraTransform = mainCamera.transform;
        _cameraTransform.position = transform.position - transform.forward * 5;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate() {
        LookAroud();
        ZoomCamera();

    }

    void LookAroud() {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        
        transform.Rotate(transform.up * rotateHorizontal * _sensitivity * Time.deltaTime);
        mainCamera.transform.RotateAround(transform.position, transform.right, -rotateVertical * _sensitivity * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.Escape)) {
            _lockCursor = !_lockCursor;
            
            if (!_lockCursor) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            } else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    void ZoomCamera() {
        _cameraDistance -= Input.GetAxis("Mouse ScrollWheel");
        _cameraDistance = Mathf.Clamp(_cameraDistance, _cameraDistanceMin, _cameraDistanceMax);
        //_cameraDistance *= zoomSpeed;

        _cameraTransform.position = (transform.position - _cameraTransform.forward * _cameraDistance);
    }

}
