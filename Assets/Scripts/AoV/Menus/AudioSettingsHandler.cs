using UnityEngine;
using UnityEngine.Audio;

namespace AoV.Menus
{
    public class AudioSettingsHandler : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private float _defaultAudioLevel = 0.8f;

        private void Start()
        {
            if (_audioMixer != null)
            {
                if (PlayerPrefs.HasKey("MasterVolume")) UpdateMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
                else PlayerPrefs.SetFloat("MasterVolume", _defaultAudioLevel);
                if (PlayerPrefs.HasKey("MusicVolume")) UpdateMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
                else PlayerPrefs.SetFloat("MusicVolume", _defaultAudioLevel);
                if (PlayerPrefs.HasKey("SFXVolume")) UpdateSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
                else PlayerPrefs.SetFloat("SFXVolume", _defaultAudioLevel);
            }
        }

        public void UpdateMasterVolume(float sliderValue)
        {
            if (_audioMixer == null)
                return;
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        }

        public void UpdateSFXVolume(float sliderValue)
        {
            if (_audioMixer == null)
                return;
            _audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }

        public void UpdateMusicVolume(float sliderValue)
        {
            if (_audioMixer == null)
                return;
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
    }

}