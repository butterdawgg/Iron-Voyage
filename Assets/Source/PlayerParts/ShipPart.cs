using UnityEngine;

[CreateAssetMenu(fileName = "ShipPart", menuName = "Player/Parts/ShipPart")]
public class ShipPart : PlayerPart
{
    public PlayerShip shipPrefab;

    public override void Apply(Player player)
    {
        PlayerShipController controller = player.GetComponent<PlayerShipController>();
        controller.Add(shipPrefab);
    }
}