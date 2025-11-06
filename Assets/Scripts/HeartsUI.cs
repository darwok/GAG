using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image heartMain;
    [SerializeField] private Image heartArmor;

    void Awake()
    {
        if (!player) player = Object.FindFirstObjectByType<Player>();
        if (player) player.UpdateHeartsUI += OnUpdateHearts;
    }

    void OnDestroy()
    {
        if (player) player.UpdateHeartsUI -= OnUpdateHearts;
    }

    void Start()
    {
        if (player) OnUpdateHearts(player.Health, player.HasArmor);
    }

    private void OnUpdateHearts(int health, bool hasArmor)
    {
        if (heartMain) heartMain.enabled = health > 0;
        if (heartArmor) heartArmor.enabled = hasArmor;
    }
}