using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private const string BufferName = "Render Camera";
    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    private CommandBuffer buffer = new CommandBuffer()
    {
        name = BufferName
    };

    private ScriptableRenderContext context;
    private Camera camera;
    private CullingResults cullingResults;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;
        if (!Cull())
        {
            return;
        }

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void Setup()
    {
        context.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(BufferName);
        ExecuteBuffer();
    }

    private void DrawVisibleGeometry()
    {
        var sortingSettings = new SortingSettings(camera);
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.all);
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        context.DrawSkybox(camera);
    }

    private void Submit()
    {
        buffer.EndSample(BufferName);
        ExecuteBuffer();
        context.Submit();
    }

    private void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    private bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
}