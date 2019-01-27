using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FixHorizontalFOV : MonoBehaviour
{
    private Camera Camera;
    private Cinemachine.CinemachineVirtualCamera VirtualCamera;

    public float HorizontalFOV = 75.0f;

    void Awake()
    {
        Camera = GetComponent<Camera>();
        VirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            Camera = GetComponent<Camera>();
            VirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }
        float hFOVrad = HorizontalFOV * Mathf.Deg2Rad;
        float camHeight = Mathf.Tan(hFOVrad * .5f) / Camera.main.aspect;
        float verticalFOVrad = Mathf.Atan(camHeight) * 2f;
        if ( Camera )
        {
            Camera.fieldOfView = verticalFOVrad * Mathf.Rad2Deg;
        }
        if ( VirtualCamera )
        {
            VirtualCamera.m_Lens.FieldOfView = verticalFOVrad * Mathf.Rad2Deg;
        }
    }
}
