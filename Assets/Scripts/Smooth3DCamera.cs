using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Smooth3DCamera : MonoBehaviour
{
    public Transform pivot;
    public Transform aa;
    public Transform bb;
    public float distance = 10.0f;
    public float minDistance = 2f;
    public float maxDistance = 15f;
    public float zoomSpeed = 1f;
    public float xSpeed = 250.0f;
    public float ySpeed = 250.0f;
    public bool allowYTilt = true;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    private float x = 0.0f;
    private float y = 0.0f;
    private float targetX = 0f;
    private float targetY = 0f;
    public float targetDistance = 0f;
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    private float zoomVelocity = 1f;

    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    public float moveSpeed = 1f;

    private void Start()
    {
        var angles = transform.eulerAngles;
        targetX = x = angles.y;
        targetY = y = angles.x;
        targetDistance = distance;
      //  targetPosition = pivot.position;
    }


    private void LateUpdate()
    {

        if (!pivot) return;

        // 缩放控制
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            targetDistance -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            targetDistance += zoomSpeed;
        }
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        // 旋转控制
        if (Input.GetMouseButton(0))
        {
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            if (allowYTilt)
            {
                targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
            }
        }

        // 平滑过渡到目标旋转
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f) : targetY;

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 1f);

        // 计算位置和旋转
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position;
        transform.rotation = rotation;
        transform.position = position;
    }


    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            if (allowYTilt)
            {
                targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
            }
        }
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f) : targetY;
    }

    private void HandleZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            targetDistance -= zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            targetDistance += zoomSpeed;
        }
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
    }
    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 1f);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position;
        transform.rotation = rotation;
        transform.position = position;
    }



    public void MovePivotToAA()
    {
        if (aa != null)
        {
            targetPosition = aa.position;
            isMovingToTarget = true;
        }
    }
    public void   yidong (int a)
    {
        switch (a)
        {
            case (1):
                targetPosition = aa.position;
                isMovingToTarget = true;
                targetDistance = 108.0f;
                targetX = x = 180f;
                targetY = y = 30.0f;
                break;

        }
        switch (a)
        {
            case (2):
                targetPosition = bb.position;
                isMovingToTarget = true;
                targetDistance = 150.0f;
                targetX = x = 90.0f;
                targetY = y = 30.0f;
                break;

        }

    }
}
