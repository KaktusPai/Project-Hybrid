using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class optionsManager : MonoBehaviour
{
    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolInput;

    public Slider mainVolSlider;
    public TextMeshProUGUI mainVolInput;

    public AudioSource musicAudio;

    private float currentAudioMusic;
    private float currentAudio;

    public FileManager fileManager;

    // Start is called before the first frame update
    void Start()
    {
        float rawValueMusic = fileManager.readMusic();
        float rawValueMain = fileManager.readMain();

        currentAudioMusic = (rawValueMusic / 100);
        currentAudio = (rawValueMain / 100);

        mainVolSlider.value = currentAudio;
        updateText();

        musicVolSlider.value = currentAudioMusic;
        updateTextMusic();
        changeMusicVolume();
    }

    public void saveChanges()
    {
        fileManager.UploadSaveValues(currentAudioMusic, currentAudio);
        SceneManager.LoadScene("MainMenuScene");
    }

    public void discardChanges()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void AudioValueChangedMusic()
    {
        currentAudioMusic = musicVolSlider.value;
        updateTextMusic();
        changeMusicVolume();
    }
    
    private void changeMusicVolume()
    {
        musicAudio.volume = currentAudioMusic;
    }

    public void updateTextMusic()
    {
        musicVolInput.text = Mathf.Round((currentAudioMusic * 100)).ToString() + "%";
        if (musicVolSlider.value != currentAudioMusic) musicVolSlider.value = currentAudioMusic;
    }

    public void AudioValueChanged()
    {
        currentAudio = mainVolSlider.value;
        updateText();
    }

    public void updateText()
    {
        mainVolInput.text = Mathf.Round((currentAudio * 100)).ToString() + "%";
        if (mainVolSlider.value != currentAudio) mainVolSlider.value = currentAudio;
    }
}
