using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Vector3 min;
    [SerializeField]
    private Vector3 max;
    private List<int> numberOfEnemies = new List<int>() {1, 2, 3};
    private int gangSize;
    private float width;
    private float x;
    private float z;
    private int minHp = 3;
    private int maxHp = 5;

    // Start is called before the first frame update
    void Start()
    {
        gangSize = GetComponentsInChildren<Enemy>().Length;
        width = enemy.GetComponent<Collider>().bounds.size.x + 2;
    }

    // Update is called once per frame
    public void DetectDeath()
    {
        if (--gangSize == 0)
        {
            if (numberOfEnemies.Count == 0)
            {
                GameManager.instance.GameOver(true);
            }
            else
            {
                gangSize = numberOfEnemies[0];
                for (int i = 0; i < gangSize; ++i)
                {
                    GameObject newItem = Instantiate(enemy) as GameObject;
                    newItem.GetComponent<Enemy>().health = Random.Range(minHp,maxHp);
                    newItem.transform.parent = transform;
                    do
                    {
                        x = Random.Range(min.x, max.x);
                        z = Random.Range(min.z, max.z);
                    }
                    while (false);// Physics.OverlapSphere(new Vector3(x, transform.position.y, z), width) != null);
                    newItem.transform.position = new Vector3(x, transform.position.y, z);
                }
                numberOfEnemies.RemoveAt(0);
            }
        }
    }
}
