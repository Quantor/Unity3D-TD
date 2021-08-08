using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartLevel);
        exitButton.onClick.AddListener(EndGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartLevel()
    {
        SceneManager.LoadScene("Level Scene");
    }

    void EndGame()
    {
        Application.Quit();
    }
}
