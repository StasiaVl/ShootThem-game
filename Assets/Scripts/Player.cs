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
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        xMovement  = Input.GetAxis("Horizontal");
        zMovement  = Input.GetAxis("Vertical");
        xRotation += Input.GetAxis("Mouse Y") * cameraSens;
        yRotation += Input.GetAxis("Mouse X") * cameraSens;

        xRotation = Mathf.Clamp(xRotation, minXCamera, maxXCamera);
        transform.localEulerAngles = new Vector3(-xRotation, yRotation, 0);

        direction = (xMovement * transform.right + zMovement * transform.forward).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.deltaTime;
    }
}
