using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private float aimCursorSpeed;

    private List<Gun> guns = new ();
    private Gun activeGun;

    private Vector3 aimDirection;
    private bool autoAimPosition;
    private Vector3 aimLocalPos;
    private Vector3 aimPosition;

    private void Update()
    {
        if (guns.Count <= 0)
            return;

        if (activeGun == null)
            activeGun = guns[0];

        if (autoAimPosition)
        {
            aimLocalPos += aimCursorSpeed * Time.deltaTime * aimDirection;
            aimLocalPos = Vector3.ClampMagnitude(aimLocalPos, activeGun.GetRange());

            aimPosition = transform.position + aimLocalPos;
        }

        activeGun.SetAim(aimPosition, aimDirection);
    }

    // Instatiates a new gun based on the prefab provided
    public void Add(Gun prefab)
    {
        Gun gun = Instantiate(prefab.gameObject, gunPivot.position,
            gunPivot.rotation, gunPivot).GetComponent<Gun>();

        gun.SetFriendly(true);

        guns.Add(gun);
    }

    public void SwitchGun(int id)
    {
        if (id < 0 || id >= guns.Count)
            return;

        activeGun = guns[id];
    }

    public void SetCanShoot(bool canShoot)
    {
        if (activeGun == null)
            return;

        activeGun.SetCanShoot(canShoot);
    }

    public void OnAimDirectionChanged(Vector3 aimDir)
    {
        aimDirection = aimDir;

        if (autoAimPosition == false)
            aimLocalPos = Vector3.zero;

        autoAimPosition = true;
    }

    public void OnAimPositionChanged(Vector3 aimPos)
    {
        autoAimPosition = false;
        aimPosition = aimPos;

        Vector3 pos = new(transform.position.x, 0f, transform.position.z);

        aimDirection = (aimPos - pos).normalized;
    }
}