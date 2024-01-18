using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer cameraRenderer = new CameraRenderer();

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        throw new NotImplementedException();
    }

    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        for (var i = 0; i < cameras.Count; i++)
        {
            cameraRenderer.Render(context, cameras[i]);
        }
    }
}