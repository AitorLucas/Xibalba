using UnityEngine;

public class SoundManager : ISingleton<SoundManager> {

    // [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    // private Player player;

    private float movingTimer;
    private float movingTimerMax = .1f;
    
    private float volume;
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    protected override void Awake() {
        base.Awake();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start() {
        // player = Player.Instance;

        // player.OnShootFired += Player_OnShootFired;
        // player.OnPlayerCrash += Player_OnPlayerCrash;
        // player.OnPowerUpCatched += Player_OnPowerUpCatched;
    }

    private void Update() {
        // movingTimer -= Time.deltaTime;
        // if (movingTimer < 0f) {
        //     movingTimer = movingTimerMax;

        //     if (player.isMoving) {
        //         PlaySound(audioClipRefsSO.shipMoving, player.transform.position, 0.03f);
        //     }
        // }
    }

    // Sounds
    // private void Player_OnShootFired(object sender, System.EventArgs e) {
    //     PlaySound(audioClipRefsSO.shot, player.transform.position);
    // }

    // private void Player_OnPlayerCrash(object sender, System.EventArgs e) {
    //     PlaySound(audioClipRefsSO.shipExplosion, player.transform.position, volume = 1.2f);
    // }

    // private void Player_OnPowerUpCatched(object sender, System.EventArgs e) {
    //     PlaySound(audioClipRefsSO.powerUp, player.transform.position, volume = 0.8f);
    // }

    // // PlaySound
    // private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
    //     PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    // }

    // private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
    //     AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    // }

    // Volume
    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}