using System;
using UnityEngine;

public class SoundManager : ISingleton<SoundManager> {

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private Player player;
    private float movingTimer;
    private float movingTimerMax = .1f;
    private float volume;
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    protected override void Awake() {
        base.Awake();
        volume = 1; // PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start() {
        player = Player.Instance;
        player.OnPlayerDeath += Player_OnPlayerDeath;
        
        PlayerController playerController = player.GetPlayerController();
        playerController.OnSpellTypeChanged += PlayerController_OnSpellTypeChanged;

        PlayerShot playerShot = playerController.GetPlayerShot();
        playerShot.OnPlayerShot += PlayerShot_OnPlayerShot;
        playerShot.OnPlayerCastBreath += PlayerShot_OnPlayerCastBreath;
        playerShot.OnPlayerCastLaser += PlayerShot_OnPlayerCastLaser;
        playerShot.OnPlayerCastExplosion += PlayerShot_OnPlayerCastExplosion;
    }


    private void Update() {
        movingTimer -= Time.deltaTime;
        if (movingTimer < 0f) {
            movingTimer = movingTimerMax;

            if (player.GetPlayerController().isMoving) {
                PlaySound(audioClipRefsSO.footSteps, player.transform.position, volumeMultiplier: .8f);
            }

            // player.PlaySound(audioClipRefsSO.footSteps, volume: .8f);
        }
    }

    // PlaySound
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
        Debug.Log("Lenght: " + audioClipArray.Length);
        var rand = UnityEngine.Random.Range(0, audioClipArray.Length - 1);
        Debug.Log("Random: " + rand);
        PlaySound(audioClipArray[rand], position, volumeMultiplier);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        Debug.Log("PlaySound");
        Debug.Log(audioClipRefsSO.ToString());
        Debug.Log(audioClip.ToString());
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

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

    // Sounds
    private void PlayerController_OnSpellTypeChanged(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.changeSpell, player.transform.position, volumeMultiplier: .4f);
    }

    private void Player_OnPlayerDeath(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.playerDeath, player.transform.position, volumeMultiplier: 1);
    }

    private void PlayerShot_OnPlayerShot(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.shotFire, player.transform.position, volumeMultiplier: .5f);
    }   

    private void PlayerShot_OnPlayerCastBreath(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.castFireBreath, player.transform.position, volumeMultiplier: .7f);
    }

    private void PlayerShot_OnPlayerCastLaser(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.castLaser, player.transform.position, volumeMultiplier: .7f);
    }

    private void PlayerShot_OnPlayerCastExplosion(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.castExplosion, player.transform.position, volumeMultiplier: .5f);
    }

    private void  Projectile_OnProjectileHit(object sender, EventArgs args) {
        PlaySound(audioClipRefsSO.shotHit, player.transform.position, volumeMultiplier: .2f);
    }

    // Register and Unregister
    public void RegisterNewProjectile(Projectile projectile) {
        projectile.OnProjectileHit += Projectile_OnProjectileHit;
    }

    public void UnregisterProjectile(Projectile projectile) {
        projectile.OnProjectileHit -= Projectile_OnProjectileHit;
    }

}