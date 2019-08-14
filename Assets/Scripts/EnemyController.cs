using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy; //for Instantiation
    private GameObject player;
    //Bounds of room:
    public Vector3 min; 
    public Vector3 max;

    //List defines what number of enemies will appear next
    private List<int> numberOfEnemies = new List<int>() {1, 2, 3};
    private int gangSize; //number of alive enemies

    //Coordinates to spawn enemy:
    private float x;
    private float z;
    //Bounds of possible health points:
    private int minHp = 3;
    private int maxHp = 5;
    
    void Start()
    {
        player = Player.instance.gameObject;
        gangSize = GetComponentsInChildren<Enemy>().Length;
    }
    
    //Works when one of the enemies dies
    public void DetectDeath()
    {
        if (--gangSize == 0)//matters only in case no more enemies left on the scene
        {
            if (numberOfEnemies.Count == 0) //if no more groups are planned
            {
                GameManager.instance.GameOver(true);
            }
            else
            {
                gangSize = numberOfEnemies[0]; //get next gang size
                numberOfEnemies.RemoveAt(0);
                Vector3 pos;
                for (int i = 0; i < gangSize; ++i)
                {
                    GameObject newItem = Instantiate(enemy) as GameObject;
                    newItem.GetComponent<Enemy>().health = Random.Range(minHp, maxHp);
                    newItem.transform.parent = transform;
                    do
                    {
                        x = Random.Range(min.x, max.x);
                        z = Random.Range(min.z, max.z);
                        pos = new Vector3(x, transform.position.y, z);
                    } //check if it is far enough from player
                    while (Vector3.Distance(player.transform.position, pos) < 10);

                    newItem.transform.position = pos;
                }
            }
        }
    }
}
