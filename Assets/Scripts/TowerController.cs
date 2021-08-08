using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private LevelManager levelManager;

    [SerializeField] GameObject projectile;
    [SerializeField] float attackspeed;
    [SerializeField] float range;
    [SerializeField] int goldCost;

    public GameObject ground;
    public bool active;
    private float elapsed = 100; // big number so the tower always shoots instantly

    private void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(active)
        {
            elapsed += Time.deltaTime;
            Shoot();
        }
        else
        {
            PlaceTower();
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
            }
        }
    }

    void PlaceTower()
    {
        Vector3 newPosition;
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        this.GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            newPosition = hit.point;
            newPosition.y = 10;
            transform.position = newPosition;
        }
        this.GetComponent<Collider>().enabled = true;
    }

    // gets called when left mouse button is clicked
    private void OnMouseDown()
    {
        if(!active && levelManager.gold >= goldCost)
        {
            active = true;
            levelManager.gold -= goldCost;
        }
    }

    // tower tries to shoot projectile
    void Shoot()
    {
        if (elapsed > 1/attackspeed)
        {
            GameObject target = GetTarget();
            if (target != null)
            {
                Instantiate(projectile, transform.position, projectile.transform.rotation);
                projectile.GetComponent<ProjectileController>().target = target;
                elapsed %= 1/attackspeed;
            }
        }
    }

    // search for enemy in range, if one is found, return it
    GameObject GetTarget()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < allEnemies.Length; i++)
        {
            if((allEnemies[i].transform.position - transform.position).magnitude < range)
            {
                return allEnemies[i];
            }
        }
        return null;
    }
}
