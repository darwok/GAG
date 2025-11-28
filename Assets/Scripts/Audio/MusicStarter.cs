using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public enum Track { Menu, Gameplay, Victory, GameOver }
    [SerializeField] private Track track;
    [SerializeField] private MusicBank bank;

    void Start()
    {
        if (!MusicPlayer.I)
        {
            var go = new GameObject("MusicPlayer");
            MusicPlayer mp = go.AddComponent<MusicPlayer>();
            mp.SetBank(bank);
        }
        else
        {
            MusicPlayer.I.SetBank(bank);
        }

        switch (track)
        {
            case Track.Menu: MusicPlayer.I.PlayMenu(); break;
            case Track.Gameplay: MusicPlayer.I.PlayGameplay(); break;
            case Track.Victory: MusicPlayer.I.PlayVictory(); break;
            case Track.GameOver: MusicPlayer.I.PlayGameOver(); break;
        }
    }
}