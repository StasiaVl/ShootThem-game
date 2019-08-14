using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHit : MonoBehaviour
{
    [SerializeField]
    private int points;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            gameObject.GetComponentInParent<Enemy>().GetHit(points);
            Destroy(collision.gameObject);
        }
    }
}
