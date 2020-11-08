using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView]
public class NightVisionCamera : MonoBehaviour
{
    private Camera myCamera;
    public Material graphicsMaterial;

    private bool NightVisionEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();
        myCamera.depthTextureMode = DepthTextureMode.Depth;

        graphicsMaterial.SetFloat("_Enabled", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (NightVisionEnabled)
            {
                graphicsMaterial.SetFloat("_Enabled", 0);
            }
            else
            {
                graphicsMaterial.SetFloat("_Enabled", 1);
            }
            NightVisionEnabled = !NightVisionEnabled;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, graphicsMaterial);
    }
}
