using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;

    [SerializeField] private List<AudioSource> mainAudioSources = new List<AudioSource>();

    private float currentAudioMusic;
    private float currentAudio;

    private FileManager fileManager;

    // Start is called before the first frame update
    void Start()
    {
        fileManager = this.GetComponent<FileManager>();

        float rawValueMusic = fileManager.readMusic();
        float rawValueMain = fileManager.readMain();

        currentAudioMusic = (rawValueMusic / 100);
        currentAudio = (rawValueMain / 100);

        SetAudioValues();
    }

    private void SetAudioValues()
    {
        if(musicAudio != null)
        {
            musicAudio.volume = currentAudioMusic;
        }

        if(mainAudioSources != null)
        {
            foreach(AudioSource item in mainAudioSources)
            {
                item.volume = currentAudio;
            }
        }
    }
}
