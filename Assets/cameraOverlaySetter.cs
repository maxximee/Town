using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class cameraOverlaySetter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var camera = GetComponent<Camera>();
        var cameraData = camera.GetUniversalAdditionalCameraData();
        cameraData.renderType = CameraRenderType.Overlay;

    }



    void OnEnable() {
        var camera = GetComponent<Camera>();
        var cameraData = camera.GetUniversalAdditionalCameraData();
        cameraData.renderType = CameraRenderType.Overlay;
    }
}
