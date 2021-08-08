using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public int currentCheckpoint = 0;
    public int killGold;
    public int lives; // number that gets subtracted from lives(in Level Manager) if enemy comes through 
    public float moveSpeed = 30;
    public float maxHealth;
    public float currentHealth;
    public Vector3 moveDirection;

    [SerializeField] Canvas healthBarCanvas;
    [SerializeField] Image healthBarImage;
    private LevelManager levelManager;
    private List<Vector3> pathCheckpoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        currentHealth = maxHealth;
        pathCheckpoints.Add(new Vector3(100, 0, 0));
        pathCheckpoints.Add(new Vector3(0, 0, 0));
        pathCheckpoints.Add(new Vector3(0, 0, -10));
        pathCheckpoints.Add(new Vector3(80, 0, -10));
        transform.position = pathCheckpoints[0];
        moveDirection = pathCheckpoints[1] - transform.position;
        moveDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {     
        MoveEnemy();
        updateHealthBar();
    }

    void updateHealthBar()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
        healthBarCanvas.transform.LookAt(Camera.main.transform);
        /*healthBarCanvas.transform
        healthBarCanvas.transform.rotation.eulerAngles = new Vector3(0,0,0);*/
    } 


    // moves enemy along path
    private void MoveEnemy()
    {
        Vector3 movementInThisFrame = moveDirection * Time.deltaTime * moveSpeed;
        //clip to checkpoint if near, otherwise move
        if ((transform.position - pathCheckpoints[currentCheckpoint]).magnitude < (movementInThisFrame).magnitude)
        {
            transform.position = pathCheckpoints[currentCheckpoint];
        }
        else
        {
            transform.Translate(movementInThisFrame, Space.World);
        }

        // if checkpoint is reached, update move direction or destroy it, if last checkpoint reached
        if (transform.position == pathCheckpoints[currentCheckpoint])
        {
            currentCheckpoint++;
            if (currentCheckpoint == pathCheckpoints.Count)
            {
                levelManager.lives -= lives;
                Destroy(gameObject);
                return;
            }
            moveDirection = pathCheckpoints[currentCheckpoint] - transform.position;
            moveDirection.Normalize();
            if(moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }
}
