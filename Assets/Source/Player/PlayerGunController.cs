using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private float aimCursorSpeed;
    [SerializeField] private Transform aimDirCursor;
    [SerializeField] private Transform aimPosCursor;

    private List<Gun> guns = new ();
    private Gun activeGun;

    private Vector3 aimDirection;
    private bool autoAimPosition;
    private bool aimDirStopped;
    private Vector3 aimLocalPos;
    private Vector3 aimPosition;

    private void Update()
    {
        if (guns.Count <= 0)
            return;

        if (activeGun == null)
            activeGun = guns[0];

        if (autoAimPosition && !aimDirStopped)
        {
            aimLocalPos += aimCursorSpeed * Time.deltaTime * aimDirection;
            aimPosition = transform.position + aimLocalPos;
        }

        aimPosition = Vector3.ClampMagnitude(aimPosition -
            transform.position, activeGun.GetRange()) + transform.position;

        activeGun.SetAim(aimPosition, aimDirection);

        ControlCursors();
    }

    private void ControlCursors()
    {
        bool pos = activeGun.DrawAimPosCursor();
        bool dir = activeGun.DrawAimDirCursor();

        if (pos && dir)
        {
            aimPosCursor.gameObject.SetActive(true);
            aimDirCursor.gameObject.SetActive(true);

            aimPosCursor.position = aimPosition;
            aimPosCursor.rotation = Quaternion.identity;

            Vector3 direction = aimPosition - transform.position;
            direction.y = 0f;

            aimDirCursor.rotation = Quaternion.LookRotation(direction);
        }
        else if (pos && !dir)
        {
            aimPosCursor.gameObject.SetActive(true);
            aimDirCursor.gameObject.SetActive(false);

            aimPosCursor.position = aimPosition;
            aimPosCursor.rotation = Quaternion.identity;
        }
        else if (!pos && dir)
        {
            aimPosCursor.gameObject.SetActive(false);
            aimDirCursor.gameObject.SetActive(true);

            aimDirCursor.rotation = Quaternion.LookRotation(aimDirection);
        }
        else if (!pos && !dir)
        {
            aimPosCursor.gameObject.SetActive(false);
            aimDirCursor.gameObject.SetActive(false);
        }
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

    public void SetAimPosition(Vector3 aimPos)
    {
        if (autoAimPosition)
            return;

        aimPosition = aimPos;

        aimDirection = (aimPos - transform.position).normalized;
        aimDirection.y = 0f;
    }

    public void OnAimDirectionChanged(Vector3 aimDir, bool stopped)
    {
        if (stopped)
        {
            aimDirStopped = true;

            return;
        }

        aimDirection = aimDir;

        if (autoAimPosition == false)
            aimLocalPos = Vector3.zero;

        autoAimPosition = true;
        aimDirStopped = false;
    }

    public void OnAimPositionChanged()
    {
        autoAimPosition = false;
    }
}