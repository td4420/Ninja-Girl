using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton, quitButton, settingButton;
    public Text playText, quitText, settingText;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    void QuitGame()
    {
        Application.Quit();
    }
    public void OnMouseEnterPlay()
    {
        playText.color = Color.red;
    }
    public void OnMouseExitPlay()
    {
        playText.color = new Color32(0, 97, 226,255);
    }
    public void OnMouseEnterQuit()
    {
        quitText.color = Color.red;
    }
    public void OnMouseExitQuit()
    {
        quitText.color = new Color32(0, 97, 226, 255);
    }
}
