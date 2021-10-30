using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject character;
    public GameObject stage1, stage2, stage3, gameOver;
    public int numberOfStages;
    private GameObject[] Stage;
    public int NumberOfEnemies;
    private int currentStage;
    public Text Enemies;
    // Start is called before the first frame update
    void Start()
    {
        currentStage = 1;
        setUpListStage();
        Stage[currentStage-1].SetActive(true);
        for(int i = currentStage;i<numberOfStages; i++)
        {
            Stage[i].SetActive(false);
        }
        gameOver.SetActive(false);
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
        yield return new WaitForSeconds(0.5f);
        for(int i=0;i<numberOfStages;i++)
        {
            Stage[i].SetActive(false);
        }    
        gameOver.SetActive(true); 
    }
    void Win()
    {
        Debug.Log("WIN");
    }
}
