using SequencerSystem;
using UnityEngine;
using System.Collections;

public class AudioPlaybackObserver : BaseObserver
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on AudioPlaybackObserver. Please add an AudioSource component.");
            }
        }
    }

    protected override void RegisterEvents()
    {
        ActionEvents.OnPlayAudio += HandlePlayAudio;
    }

    protected override void UnregisterEvents()
    {
        ActionEvents.OnPlayAudio -= HandlePlayAudio;
    }

    private void HandlePlayAudio(int actionChannel, AudioClip clip, float delay)
    {
        if (actionChannel == channel && audioSource != null && clip != null)
        {
            StartCoroutine(PlayAudioCoroutine(clip, delay));
        }
        else if (audioSource == null)
        {
            Debug.LogError("Cannot play audio: AudioSource is not assigned.");
        }
        else if (clip == null)
        {
            Debug.LogWarning("Cannot play audio: AudioClip is null.");
        }
    }

    private IEnumerator PlayAudioCoroutine(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.clip = clip;
        audioSource.Play();
    }
}