using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public abstract bool CheckVisibility(LayerMask mask);

    public abstract void SetCanShoot(bool canShoot);

    public abstract void SetAim(Vector3 aimPos, Vector3 aimDir);

    public abstract void SetFriendly(bool friendly);

    public abstract float GetRange();

    public abstract bool DrawAimPosCursor();
    public abstract bool DrawAimDirCursor();
}