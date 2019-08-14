using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public bool moving = true;
    private EnemyController controller;

    private Vector3 aim;
    private Vector3 direction;

    private void Start()
    {
        controller = transform.parent.gameObject.GetComponent<EnemyController>();
        direction = transform.position;
        if (moving)
        {
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(Walking());
        }
    }

    private void FixedUpdate()
    {
        if(moving && isInBounds())
            transform.Translate(direction * Time.deltaTime);
    }

    private bool isInBounds()
    {
        aim = transform.position + direction;
        return aim.x < controller.max.x && aim.z < controller.max.z &&
                aim.x > controller.min.x && aim.z > controller.min.z;
    }

    public void GetHit(int points)
    {
        health -= points;
        StartCoroutine(ColorChange());
        if (health <= 0)
        {
            gameObject.GetComponentInParent<EnemyController>().DetectDeath();
            Destroy(gameObject);
        }
    }

    IEnumerator ColorChange()
    {
        foreach (Renderer m in gameObject.GetComponentsInChildren<Renderer>())
           m.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        foreach (Renderer m in gameObject.GetComponentsInChildren<Renderer>())
           m.material.color = Color.white;
    }

    IEnumerator Walking()
    {
        direction = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Walking());
    }
}
