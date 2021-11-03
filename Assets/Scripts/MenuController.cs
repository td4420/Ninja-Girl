using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text volumeSFXSetting, volumeSoundSetting;
    private AudioSource sound, effect;
    public AudioClip background, click;
    public static float gameVolume, SFX;
    public GameObject defaultMenu, volumeSetting;
    public GameObject loadingScreen;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        gameVolume = 100;
        SFX = 100;
        sound = gameObject.AddComponent<AudioSource>();
        effect = gameObject.AddComponent<AudioSource>();
        sound.clip = background;
        sound.loop = true;
        sound.Play();
        effect.clip = click;
        effect.loop = false;
        defaultMenu.SetActive(true);
        volumeSetting.SetActive(false);
        loadingScreen.SetActive(false);
        volumeSFXSetting.text = SFX.ToString();
        volumeSoundSetting.text = gameVolume.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        sound.volume = gameVolume/100;
        effect.volume = SFX/100;
    }
    public void PlayGame()
    {
        effect.Play();
        StartCoroutine(LoadGameScene());
    }
    IEnumerator LoadGameScene()
    {
        loadingScreen.SetActive(true);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(1);
        while(!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
    public void QuitGame()
    {
        effect.Play();
        Application.Quit();
    }
    public void Setting()
    {
        defaultMenu.SetActive(false);
        volumeSetting.SetActive(true);
    }
    public void OnMouseEnter(Text txt)
    {
        txt.color = Color.red;
    }
    public void OnMouseExit(Text txt)
    {
        txt.color = new Color32(0, 97, 226,255);
    }
    public void OnMouseClick(Text txt)
    {
        txt.color = new Color32(0, 97, 226, 255);
    }
    public void subVolumeSFX(Text txt)
    {
        if(SFX > 0) SFX -= 1;
        txt.text = SFX.ToString();
    }
    public void volumeUpSFX(Text txt)
    {
        if (SFX < 100) SFX += 1;
        txt.text = SFX.ToString();
    }
    public void subVolumeSound(Text txt)
    {
        if(gameVolume > 0) gameVolume -= 1;
        txt.text = gameVolume.ToString();
    }
    public void volumeUpSound(Text txt)
    {
        if (gameVolume < 100) gameVolume += 1;
        txt.text = gameVolume.ToString();
    }
    public void Back()
    {
        defaultMenu.SetActive(true);
        volumeSetting.SetActive(false);
    }

}
