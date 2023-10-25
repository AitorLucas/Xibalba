using UnityEditor.SceneManagement;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public interface ISoundPlayer {

    GameObject gameObject { get ; }

    public void PlaySound(AudioClip audioClip, float volume) {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = audioClip;
        audioSource.Play();
        GameObject.Destroy(audioSource, audioClip.length);
    }
}
