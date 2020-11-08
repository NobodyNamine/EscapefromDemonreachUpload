using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView]
public class NightVisionCamera : MonoBehaviour
{
    private Camera myCamera;
    public Material graphicsMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();
        myCamera.depthTextureMode = DepthTextureMode.Depth;

        graphicsMaterial.SetFloat("_Enabled", 0);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, graphicsMaterial);
    }
}
