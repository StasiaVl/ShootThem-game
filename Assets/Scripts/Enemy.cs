using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 3;

    public void GetHit(int points)
    {
        health -= points;
        if (health <= 0)
        {
            gameObject.GetComponentInParent<EnemyController>().DetectDeath();
            Destroy(gameObject);
        }
    }
}
