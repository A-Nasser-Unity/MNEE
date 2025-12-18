using UnityEngine;
using UnityEngine.UI;

public class MusicStorePlayer : MonoBehaviour
{
    [System.Serializable]
    public class MusicTrack
    {
        public AudioClip clip;
        public Button playButton;
        public Button pauseButton;
    }

    public AudioSource audioSource;
    public MusicTrack[] tracks;

    private int currentTrackIndex = -1;

    void Start()
    {
        // Initialize buttons
        for (int i = 0; i < tracks.Length; i++)
        {
            int index = i;

            tracks[i].playButton.onClick.AddListener(() => PlayTrack(index));
            tracks[i].pauseButton.onClick.AddListener(() => PauseTrack(index));

            SetPlayState(index, true);
        }
    }

    void Update()
    {
        // If audio finished naturally
        if (currentTrackIndex != -1 && !audioSource.isPlaying)
        {
            ResetAllTracks();
        }
    }

    void PlayTrack(int index)
    {
        // Stop any currently playing track
        ResetAllTracks();

        currentTrackIndex = index;
        audioSource.clip = tracks[index].clip;
        audioSource.Play();

        SetPlayState(index, false);
    }

    void PauseTrack(int index)
    {
        if (currentTrackIndex != index) return;

        audioSource.Stop();
        ResetAllTracks();
    }

    void ResetAllTracks()
    {
        audioSource.Stop();
        currentTrackIndex = -1;

        for (int i = 0; i < tracks.Length; i++)
        {
            SetPlayState(i, true);
        }
    }

    void SetPlayState(int index, bool showPlay)
    {
        tracks[index].playButton.gameObject.SetActive(showPlay);
        tracks[index].pauseButton.gameObject.SetActive(!showPlay);
    }
}
