using UnityEngine;

[CreateAssetMenu(fileName = "ShipPartDefault", menuName = "Player/Parts/Default/ShipPartDefault")]
public class ShipPartDefault : PlayerPart
{
    public PlayerShip shipPrefab;

    public override bool UnlockedByDefault() { return true; }
    public override bool SelectedByDefault() { return true; }

    public override void Apply(Player player)
    {
        PlayerShipController controller = player.GetComponent<PlayerShipController>();
        controller.Add(shipPrefab);
    }
}