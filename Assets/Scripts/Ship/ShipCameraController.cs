using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform shipBody;

    [Header("Camera Values")]
    [SerializeField] private float mouseSensMulti;
    [SerializeField] private float cameraDistance;
    [SerializeField] private float maxRotX;
    [SerializeField] private float camLerpSpeed;

    private Vector2 mouseInput;
    private float xRotation, yRotation;

    private void Start()
    {
        CursorManager.CursorState(true);
    }

    private void Update()
    {
        ThirdPersonCamera();
    }

    private void ThirdPersonCamera()
    {
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensMulti;

        yRotation += mouseInput.x;
        xRotation += mouseInput.y;

        xRotation = Mathf.Clamp(xRotation, -maxRotX, maxRotX);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //transform.position = Vector3.Lerp(transform.position, shipBody.position + transform.forward * -cameraDistance, camLerpSpeed * Time.deltaTime);
        transform.position = shipBody.position + transform.forward * -cameraDistance;
    }
}