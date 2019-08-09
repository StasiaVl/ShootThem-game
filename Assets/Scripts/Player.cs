using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody rb;
    private float xMovement;
    private float zMovement;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        direction = (xMovement * transform.right + zMovement * transform.forward).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.deltaTime;
    }
}
