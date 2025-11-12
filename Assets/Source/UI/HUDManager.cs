using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI enemyKillCountText;

    public void SetHealthFraction(float fraction)
    {
        healthBar.rectTransform.localScale = new Vector3(fraction, 1f, 1f);
    }

    public void SetEnemyKillCountText(string text)
    {
        enemyKillCountText.text = text;
    }

    public void SetRoundText(string text)
    {
        roundText.text = text;
    }
}