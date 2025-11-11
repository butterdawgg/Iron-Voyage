using UnityEngine;

[CreateAssetMenu(fileName = "HullPart", menuName = "Player/Parts/HullPart")]
public class HullPart : PlayerPart
{
    public PlayerHull hullPrefab;

    public override void Apply(Player player)
    {
        PlayerHullController controller =
            player.GetComponent<PlayerHullController>();

        controller.Add(hullPrefab);
    }
}