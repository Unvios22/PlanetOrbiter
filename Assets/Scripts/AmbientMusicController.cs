using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientMusicController : MonoBehaviour {
    [SerializeField] private List<AudioClip> ambientClips;
    [SerializeField] private AudioSource audioSource;

    private List<AudioClip> _remainingClipsToPlay;

    private void Start() {
        _remainingClipsToPlay = new List<AudioClip>(ambientClips);
        PlayNextClip();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)) {
            PlayNextClip();
        }
    }

    private void PlayNextClip() {
        audioSource.Stop();
        audioSource.clip = GetNextAudioClip();
        audioSource.Play();
    }
    
    private AudioClip GetNextAudioClip() {
        var clipToPlay = _remainingClipsToPlay[Random.Range(0, _remainingClipsToPlay.Count)];
        _remainingClipsToPlay.Remove(clipToPlay);
        
        if (_remainingClipsToPlay.Count == 0) {
            _remainingClipsToPlay = new List<AudioClip>(ambientClips);
        }

        return clipToPlay;
    }
}
