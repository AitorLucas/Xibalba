using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : ISingleton<MusicManager> {

    private AudioSource audioSource;
    private float volume;
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    protected override void Awake() {
        base.Awake();
        
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
    
}
