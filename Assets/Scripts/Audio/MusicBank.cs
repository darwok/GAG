using UnityEngine;

[CreateAssetMenu(menuName = "GAG/Music Bank", fileName = "MusicBank")]
public class MusicBank : ScriptableObject
{
    public AudioClip mainMenu;
    public AudioClip gameplay;
    public AudioClip victory;
    public AudioClip gameOver;
}