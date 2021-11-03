using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private GameObject character;
    public GameObject winPanel, gameOver, pausePanel, settingPanel;
    public GameObject UI;
    private int numberOfStages;
    private GameObject[] Stage;
    private int NumberOfEnemies;
    private int currentStage;
    public Text Enemies, back;
    private AudioSource sound;
    public AudioClip backgroundSound, gameoverSound, winSound;
    public Slider soundSlider, sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        currentStage = 1;
        setUpListStage();
        Stage[currentStage-1].SetActive(true);
        for(int i = currentStage;i<numberOfStages; i++)
        {
            Stage[i].SetActive(false);
        }
        gameOver.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);
        sound = gameObject.AddComponent<AudioSource>();
        sound.clip = backgroundSound;
        sound.loop = true;
        sound.volume = MenuController.gameVolume;
        sound.Play();
    }
    void setUpListStage()
    {
        Stage = GameObject.FindGameObjectsWithTag("Stage");
        numberOfStages = Stage.Length;
    }
    void resetCharacter()
    {
        character.transform.position = new Vector3(-7.2f, -3.6f, 90);
        character.GetComponent<PlayerController>().haveKey = false;
        character.GetComponent<PlayerController>().reachDestination = false;
        character.GetComponent<PlayerController>().Hp = 100;
        character.GetComponent<PlayerController>().numberOfKunais = 10;
    }
    // Update is called once per frame
    void Update()
    {
        NumberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Enemies.text = "X" + NumberOfEnemies.ToString();
        bool reach = character.GetComponent<PlayerController>().reachDestination;
        bool haveKey = character.GetComponent<PlayerController>().haveKey;
        if (reach && haveKey && NumberOfEnemies == 0)
        {
            clearStage();
        }
        if(character.GetComponent<PlayerController>().Hp <= 0)
        {
            StartCoroutine(GameOver());
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if(settingPanel.activeSelf)
        {
            sound.volume = soundSlider.value;
            character.GetComponent<PlayerController>().effect.volume = sfxSlider.value;
        }
    }
    void clearStage()
    {
        Stage[currentStage - 1].SetActive(false);
        currentStage++;
        if (currentStage > numberOfStages)
        {
            Win();
        }
        else
        {
            Stage[currentStage - 1].SetActive(true);
            resetCharacter();
        }
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        Stage[currentStage - 1].SetActive(false);
        UI.SetActive(false);
        character.SetActive(false);
        gameOver.SetActive(true);
        character.GetComponent<PlayerController>().Hp = 100;
        sound.clip = gameoverSound;
        sound.Play();
    }
    void Win()
    {
        sound.clip = winSound;
        sound.Play();
        Stage[currentStage - 2].SetActive(false);
        UI.SetActive(false);
        character.SetActive(false);
        winPanel.SetActive(true);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
    }
    public void Resum()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Setting()
    {
        settingPanel.SetActive(true);
    }
    public void Back()
    {
        settingPanel.SetActive(false);
    }
}
