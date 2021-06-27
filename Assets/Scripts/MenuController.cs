using UnityEngine;

public class MenuController : MonoBehaviour
{

    public void Awake()
    {
        if (!PlayerPrefs.HasKey($"level-1"))
        {
            PlayerPrefs.SetInt($"level-1", 0);
        }
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
}
