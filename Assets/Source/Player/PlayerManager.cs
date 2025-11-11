using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private PlayerPartData partData;

    public Player InitializePlayer(Vector3 position)
    {
        Player player = Instantiate(playerPrefab.gameObject,
            position, Quaternion.identity, (Transform)default)
            .GetComponent<Player>();

        PlayerPart[] parts = partData.GetSelectedParts();

        foreach (PlayerPart part in parts)
        {
            part.Apply(player);
        }

        return player;
    }
}