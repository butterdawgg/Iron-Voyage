using UnityEngine;

public class Border : MonoBehaviour
{
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }

    private void RecalculateMaterialTiling(Vector2 size)
    {
        if (mr == null)
            return;

        Vector2 tiling = mr.sharedMaterial.mainTextureScale;

        mr.material.mainTextureScale = tiling * size;
    }

    public void Place(Vector3 pos, Vector3 normal, Vector2 size)
    {
        RecalculateMaterialTiling(size);

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(normal);
        transform.localScale = new Vector3(size.x, size.y, 1f);
    }
}