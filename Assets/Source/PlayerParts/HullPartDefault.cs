using UnityEngine;

[CreateAssetMenu(fileName = "HullPartDefault", menuName = "Player/Parts/Default/HullPartDefault")]
public class HullPartDefault : PlayerPart
{
    public PlayerHull hullPrefab;

    public override bool UnlockedByDefault() { return true; }
    public override bool SelectedByDefault() { return true; }

    public override void Apply(Player player)
    {
        PlayerHullController controller =
            player.GetComponent<PlayerHullController>();

        controller.Add(hullPrefab);
    }
}