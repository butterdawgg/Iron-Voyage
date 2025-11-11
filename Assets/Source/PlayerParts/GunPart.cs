using UnityEngine;

[CreateAssetMenu(fileName = "GunPart", menuName = "Player/Parts/GunPart")]
public class GunPart : PlayerPart
{
    public Gun gunPrefab;

    public override void Apply(Player player)
    {
        PlayerGunController controller = player.GetComponent<PlayerGunController>();

        controller.Add(gunPrefab);
    }
}