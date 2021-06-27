using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    public int levelId;
    public GameObject LevelNumber;
    public AudioSource WrongAudio;
    public AudioSource TransitionAudio;

    public Sprite LevelLocked;

    private void Start()
    {
        if (!PlayerPrefs.HasKey($"level-{levelId}"))
        {
            PlayerPrefs.SetInt($"level-{levelId}", -1);
        }

        var totalStar = PlayerPrefs.GetInt($"level-{levelId}");

        if (totalStar == -1)
        {
            LevelNumber.gameObject.SetActive(false);
            GetComponent<Image>().overrideSprite = LevelLocked;
        }
    }

    public void OnClick()
    {
        if (PlayerPrefs.GetInt($"level-{levelId}") == -1)
        {
            WrongAudio.Play();
            return;
        }

        TransitionAudio.Play();
        PlayerPrefs.SetInt("SelectedLevel", levelId);
        Initiate.Fade("Game", Color.black, 1.0f);
    }
}
