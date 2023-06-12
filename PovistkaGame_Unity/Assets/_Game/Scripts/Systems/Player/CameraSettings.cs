using UnityEngine;

[CreateAssetMenu(menuName = "Data/create CameraSettings", fileName = "new Camera Settings", order = 0)]
public class CameraSettings : ScriptableObject
{
    [field: SerializeField]
    private float referenceResolution = 1080.0f;
    public float ReferenceResolution => referenceResolution;

    [field: SerializeField, Tooltip("PPU stands for pixels per unit")]
    private float referencePPU = 16.0f;
    public float ReferencePPU => referencePPU;

    [field: SerializeField, Tooltip("Scale")]
    private float magickNumber = 8.0f;
    public float MagickNumber => magickNumber;
}
