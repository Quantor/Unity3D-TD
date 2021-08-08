using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int gold;
    public int lives;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] List<GameObject> enemyTypes = new List<GameObject>();
    [SerializeField] GameObject testTower;
    [SerializeField] Button buildTowerButton;
    [SerializeField] Button lostButton; // button that appears if the player has lost the level(lives=0)
    [SerializeField] int numberOfEnemies;
    [SerializeField] float spawnDelayAtStart;
    [SerializeField] float spawnDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelayAtStart, spawnDelay);
        buildTowerButton.onClick.AddListener(BuildTower);
        lostButton.onClick.AddListener(BackToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if(lives <= 0)
        {
            lostButton.gameObject.SetActive(true);
        }
        goldText.text = "Gold: " + gold;
        livesText.text = "Lives: " + lives;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyTypes[0]);
        numberOfEnemies--;
        if(numberOfEnemies == 0)
        {
            CancelInvoke();
        }
    }

    void BuildTower()
    {
        Instantiate(testTower);
        testTower.GetComponent<TowerController>().active = false;
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
