using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject target;

    [SerializeField] float projectileSpeed = 50;
    [SerializeField] float damage;

    private LevelManager levelManager;
    private Vector3 directionToTarget;
    private const float xBound = 300;
    private const float zBound = 300;

    void Start()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        if (checkDestroyConditions()) return;
        Vector3 enemyPos = target.transform.position;
        Vector3 enemyDir = target.GetComponent<EnemyController>().moveDirection;
        float enemySpeed = target.GetComponent<EnemyController>().moveSpeed;

        Vector3 towerPos = transform.position;
        directionToTarget = predictShot3D(enemyPos, enemyDir, enemySpeed, towerPos, projectileSpeed);
        transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkDestroyConditions()) return;
        moveProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if projectile collides with enemy, damage it and destroy projectile and enemy(if health <= 0)
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().currentHealth -= damage;
            if(collision.gameObject.GetComponent<EnemyController>().currentHealth <= 0)
            {
                levelManager.gold += collision.gameObject.GetComponent<EnemyController>().killGold;
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void moveProjectile()
    {
        //directionToTarget = target.GetComponent("Transform").transform.position - transform.position;
        //directionToTarget.Normalize();
        transform.Translate(directionToTarget * Time.deltaTime * projectileSpeed, Space.World);
        //transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
    }

    // returns true if projectile has to be destroyed
    bool checkDestroyConditions()
    {
        if (transform.position.x > xBound || transform.position.z > zBound)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    // returns direction of projectile so the movement of the enemy is predicted;
    // uses equations of the form:    enemyPos + t * enemySpeed * enemyDir = towerPos + t * towerSpeed * towerDir   -> equation to find intersection of two lines with same t(because time has to be the same)
    //                                sqrt(towerDir.x*towerDir.x + towerDir.y*towerDir.y + towerDir.z*towerDir.z) = 1   -> length of returned vector has to be the length of 1
    Vector3 predictShot3D(Vector3 enemyPos, Vector3 enemyDir, float enemySpeed, Vector3 towerPos, float towerSpeed)
    {
        Vector3 towerDir = new Vector3(0, 0, 0);
        float t1 = 2 * (towerSpeed * towerSpeed - enemySpeed * enemySpeed * enemyDir.y * enemyDir.y
            - enemySpeed * enemySpeed * enemyDir.x * enemyDir.x - enemySpeed * enemySpeed * enemyDir.z * enemyDir.z);
        float t2 = -2 * enemyPos.x * enemyDir.x + 2 * towerPos.x * enemyDir.x - 2 * enemyPos.y * enemyDir.y
            + 2 * towerPos.y * enemyDir.y - 2 * enemyPos.z * enemyDir.z + 2 * towerPos.z * enemyDir.z;
        float t = 1 / t1;
        t *= -enemySpeed * t2 + Mathf.Sqrt(enemySpeed * enemySpeed * Mathf.Pow(t2, 2) - 2 * (-enemyPos.x * enemyPos.x + 2 * enemyPos.x * towerPos.x - towerPos.x * towerPos.x -
            enemyPos.y * enemyPos.y + 2 * enemyPos.y * towerPos.y - towerPos.y * towerPos.y - enemyPos.z * enemyPos.z + 2 * enemyPos.z * towerPos.z - towerPos.z * towerPos.z) * t1);

        towerDir.x = (enemyPos.x - towerPos.x) / (t * towerSpeed) + (enemySpeed / towerSpeed) * enemyDir.x;
        towerDir.y = (enemyPos.y - towerPos.y) / (t * towerSpeed) + (enemySpeed / towerSpeed) * enemyDir.y;
        towerDir.z = (enemyPos.z - towerPos.z) / (t * towerSpeed) + (enemySpeed / towerSpeed) * enemyDir.z;

        Debug.Log("x: " + towerDir.x + " y: " + towerDir.y + " z: " + towerDir.z + " t: " + t);
        return towerDir;
    }
}
