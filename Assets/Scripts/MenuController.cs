using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
	public AudioMixer Mixer;
	public Toggle toggleBGM;
	public Toggle toggleSFX;
	public TMP_Dropdown modeSelect;

    public void Awake()
    {
        if (!PlayerPrefs.HasKey("level-1"))
        {
	        PlayerPrefs.SetInt("level-1", 0);
	        PlayerPrefs.SetFloat("mode-factor", 1f);
	        PlayerPrefs.SetInt("mode-select", 1);
	        PlayerPrefs.SetInt("bgm", 1);
	        PlayerPrefs.SetInt("sfx", 1);
        }

	    modeSelect.value = PlayerPrefs.GetInt("mode-select");
	    toggleBGM.isOn = PlayerPrefs.GetInt("bgm") == 1;
	    toggleSFX.isOn = PlayerPrefs.GetInt("sfx") == 1;
	    
	    Mixer.SetFloat("BGM", toggleBGM.isOn ? 0f : -80f);
	    Mixer.SetFloat("SFX", toggleSFX.isOn ? 0f : -80f);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        Initiate.Fade("Menu", Color.black, 1f);
    }
    
	public void ToggleBGM() {
		Mixer.SetFloat("BGM", toggleBGM.isOn ? 0f : -80f);
		PlayerPrefs.SetInt("bgm", toggleBGM.isOn ? 1 : 0);
	}
	
	public void ToggleSFX() {
		Mixer.SetFloat("SFX", toggleSFX.isOn ? 0f : -80f);
		PlayerPrefs.SetInt("sfx", toggleSFX.isOn ? 1 : 0);
	}
	
	public void ChangeMode() {
		switch (modeSelect.value) {
		case 0:
			PlayerPrefs.SetFloat("mode-factor", 0.75f);
			PlayerPrefs.SetInt("mode-select", modeSelect.value);
			break;
		case 1:
			PlayerPrefs.SetFloat("mode-factor", 1f);
			PlayerPrefs.SetInt("mode-select", modeSelect.value);
			break;
		case 2:
			PlayerPrefs.SetFloat("mode-factor", 1.5f);
			PlayerPrefs.SetInt("mode-select", modeSelect.value);
			break;
		}
	}
}
