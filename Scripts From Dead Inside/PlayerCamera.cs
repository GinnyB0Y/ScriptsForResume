using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] Transform body;

    [SerializeField] float minX = -60f;
    [SerializeField] float maxX = 60f;
    [SerializeField] float speed;
    [SerializeField] float sensitivity;
    [SerializeField] Transform cam;
    [SerializeField] float rotY = 0f;
    [SerializeField] float rotX = 0f;

    private void Start()
    {
        Cursor.visible = false;
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;
        rotX = Mathf.Clamp(rotX, minX, maxX);

        Quaternion target = Quaternion.Euler(-rotX, rotY, 0);

        // On/Off camera while game is paused
        if(PauseMenu.gameIsPaused == false)
            body.localRotation = Quaternion.Slerp(body.rotation, target, Time.deltaTime * speed);
        else
            transform.localRotation = Quaternion.Slerp(body.rotation, target, Time.deltaTime * speed);
    }
}
