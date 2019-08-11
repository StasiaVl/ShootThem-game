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
    private float xRotation;
    private float yRotation;
    private float minXCamera = -45;
    private float maxXCamera =  45;
    private float cameraSens =  5;
    private Vector3 direction;

    void Awake()
    {
        rb  = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.CurrentState == GameStatus.play)
        {
            xMovement = Input.GetAxis("Horizontal");
            zMovement = Input.GetAxis("Vertical");
            xRotation += Input.GetAxis("Mouse Y") * cameraSens;
            yRotation += Input.GetAxis("Mouse X") * cameraSens;

            xRotation = Mathf.Clamp(xRotation, minXCamera, maxXCamera);
            transform.localEulerAngles = new Vector3(-xRotation, yRotation, 0);

            direction = (xMovement * transform.right + zMovement * transform.forward).normalized;
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.CurrentState == GameStatus.play)
        {
            rb.velocity = direction * speed * Time.deltaTime;
        }
    }
}
