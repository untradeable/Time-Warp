using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Rotate : MonoBehaviour
{
    public bool isTemplate = true;
    public bool isLockedInObject = false;

    [Space]

    private Vector2 turn;
    private Vector3 pos;
    private Vector3 deltaMove;

    public float sensitivity = 8f;
    public float speed = 3;

    //

    [Space]
    public float rotateSpeed;

    [Tooltip("Set to this rotate X, Y or Z")]
    public bool[]  rotationAxis;

    private void Start()
    {
        pos = transform.position;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && isLockedInObject)
        {
            turn.x -= Input.GetAxis("Mouse X") * sensitivity;
            turn.y -= Input.GetAxis("Mouse Y") * sensitivity;

            transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
        }
        
        if(isLockedInObject && Input.GetMouseButtonUp(0))
        {
            isLockedInObject = false;
        }

        transform.position = pos;
    }

    private void FixedUpdate()
    {
        transform.Rotate(RotateObject(), Space.World);
    }

    public void OnMouseEnter()
    {
        if(!isTemplate)
        {
            return;
        }

        isLockedInObject = true;
    }

    private Vector3 RotateObject()
    {   
        float[] rotations = new float[3];

        for(int axis = 0; axis < rotationAxis.Length; axis++)
        {
            if(rotationAxis[axis])
            {
                rotations[axis] = rotateSpeed * Time.deltaTime;
            }
            else
            {
                rotations[axis] = 0f;
            }
        }
        
        return new Vector3(rotations[0], rotations[1], rotations[2]);
    }
}