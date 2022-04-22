using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class cameraOverlaySetter : MonoBehaviour
{
    private Camera camera;
    private UniversalAdditionalCameraData cameraData;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        cameraData = camera.GetUniversalAdditionalCameraData();
        //cameraData.renderType = CameraRenderType.Overlay;

    }

void Update()
{
    if (!cameraData.renderType.Equals(CameraRenderType.Overlay)) {
        Debug.Log("camera set to: " + cameraData.renderType);
        cameraData.renderType = CameraRenderType.Overlay;
    }
}

    void OnEnable() {
        camera = GetComponent<Camera>();
        cameraData = camera.GetUniversalAdditionalCameraData();
        //cameraData.renderType = CameraRenderType.Overlay;
    }
}
