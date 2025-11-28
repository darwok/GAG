using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public enum Track { Menu, Gameplay, Victory, GameOver }
    [SerializeField] private Track track;

    void Start()
    {
        if (!AudioManager.I)
        {
            var go = new GameObject("AudioManager");
            go.AddComponent<AudioManager>(); // asume que el SoundList está en este GO o se lo asignas por prefab
        }

        switch (track)
        {
            case Track.Menu:     AudioManager.I.PlayMenuMusic();     break;
            case Track.Gameplay: AudioManager.I.PlayGameplayMusic(); break;
            case Track.Victory:  AudioManager.I.PlayVictoryMusic();  break;
            case Track.GameOver: AudioManager.I.PlayGameOverMusic(); break;
        }
    }
}