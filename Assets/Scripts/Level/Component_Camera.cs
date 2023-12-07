using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Component_Camera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float fieldOfViewMax = 70f;
    [SerializeField] private float fieldOfViewMin = 10f;
    
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform cameraObj;
    [SerializeField] private Transform center;

    private float targetFieldOfView = 70;
    private Vector2 lastMousePos;

    private void FixedUpdate()
    {
        float distance = (cameraObj.position - center.position).magnitude;

        if(distance >= maxDistance)
        {
            transform.position = center.position;
        }
    }

    private void LateUpdate()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoomMain();    
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W)) inputDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) inputDir.x = +1f;
        
        if(Input.GetKey(KeyCode.S)) inputDir.z = +1f;
        if(Input.GetKey(KeyCode.D)) inputDir.x = -1f;
            
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        
        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void HandleCameraZoomMain()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5;
        }
        
        if(Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

        float zoomSpeed = 10f;
       
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
    }

    private void HandleCameraRotation()
    {
        float rotateDir = 0f;
        if(Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if(Input.GetKey(KeyCode.E)) rotateDir = -1f;

        float rotateSpeed = 100f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.fixedDeltaTime, 0);
    }
}
