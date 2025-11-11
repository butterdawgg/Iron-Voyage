using UnityEngine;

[CreateAssetMenu(fileName = "GunPartDefault", menuName = "Player/Parts/Default/GunPartDefault")]
public class GunPartDefault : PlayerPart
{
    public Gun gunPrefab;

    public override bool UnlockedByDefault() { return true; }
    public override bool SelectedByDefault() { return true; }

    public override void Apply(Player player)
    {
        PlayerGunController controller = player.GetComponent<PlayerGunController>();

        controller.Add(gunPrefab);
    }
}