using UnityEngine;

public class CameraController
{
    private readonly Camera _camera;
    private readonly CameraSettings _settings;
    
    private Transform _target;

    public CameraController(CameraSettings settings, Camera mainCamera)
    {
        _camera = mainCamera;
        _settings = settings;

        _camera.orthographicSize = _settings.ReferenceResolution / _settings.ReferencePPU / _settings.MagickNumber;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void UpdateCamera()
    {
        var pPos = _target.position;
        _camera.transform.position = new Vector3(pPos.x, pPos.y, -10);
    }
}
