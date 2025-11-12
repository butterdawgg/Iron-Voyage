using UnityEngine;

public class Border : MonoBehaviour
{
    private MeshRenderer mr;

    private void RecalculateMaterialTiling(Vector2 size)
    {
        Vector2 tiling = mr.sharedMaterial.mainTextureScale;

        mr.material = new Material(mr.sharedMaterial);
        mr.material.mainTextureScale = tiling * size;
    }

    public void Place(Vector3 pos, Vector3 normal, Vector2 size)
    {
        mr = GetComponentInChildren<MeshRenderer>();

        RecalculateMaterialTiling(size);

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(normal);
        transform.localScale = new Vector3(size.x, size.y, 1f);
    }
}