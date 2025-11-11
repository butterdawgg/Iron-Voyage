using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;

    public void ControlGuns(Gun[] guns, Vector3 aimPos, Vector3 aimDir)
    {
        foreach (var gun in guns)
        {
            gun.SetFriendly(false);
            gun.SetAim(aimPos, aimDir);
            gun.SetCanShoot(gun.CheckVisibility(obstacleMask));
        }
    }

    public void DisableGuns(Gun[] guns)
    {
        foreach (var gun in guns)
        {
            gun.SetCanShoot(false);
        }
    }
}