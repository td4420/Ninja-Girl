using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject character;
    public int NumberOfEnemies;
    public Text Enemies;
    // Start is called before the first frame update
    void Start()
    {
        
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
            GameOver();
        }
    }
    void clearStage()
    {
        Debug.Log("Winner");
    }
    void GameOver()
    {
        Debug.Log("You Lose");
    }
}
