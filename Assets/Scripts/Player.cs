using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject bullet;
    private float bulletSpeed = 60;
    private GameObject currentBullet;

    [SerializeField]
    private AudioSource walkSound;
    [SerializeField]
    private AudioSource shootSound;
    [SerializeField]
    private AudioSource hitSound;
    [SerializeField]
    private Slider health;
    [SerializeField]
    private Text healthTxt;
    private float lifes = 10;
    private float currentHealth;

    private Rigidbody rb;
    private float xMovement;
    private float zMovement;
    private float xRotation;
    private float yRotation;
    private float minXCamera = -45;
    private float maxXCamera =  45;
    private float cameraSens =  5;
    private Vector3 direction;
    private Vector3 startingPos;
    private Quaternion startingRot;

    public static Player instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        rb  = GetComponent<Rigidbody>();
        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    public void Restart()
    {
        transform.position = startingPos;
        transform.rotation = startingRot;
        currentHealth = lifes;
        health.maxValue = lifes;
        health.value = currentHealth;
        healthTxt.text = "Health: " + currentHealth + '\\' + lifes;
        rb.velocity = Vector3.zero;
        xRotation = 0;
        yRotation = 0;
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


            if(Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
        if (rb.velocity != Vector3.zero && !walkSound.isPlaying)
        {
            walkSound.Play();
        } else if (rb.velocity == Vector3.zero && walkSound.isPlaying)
        {
            walkSound.Stop();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.CurrentState == GameStatus.play)
        {
            rb.velocity = direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            health.value = --currentHealth;
            healthTxt.text = "Health: " + currentHealth + '\\' + lifes;
            if (currentHealth == 0)
                GameManager.instance.GameOver(false);
            hitSound.Play();
        }
    }

    private void Shoot()
    {
        if (currentBullet == null)
        {
            shootSound.Play();
            currentBullet = Instantiate(bullet) as GameObject;
            currentBullet.transform.parent = transform;
            currentBullet.transform.localPosition = Vector3.forward + Vector3.up;
            currentBullet.transform.parent = null;
            Destroy(currentBullet, 1f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            currentBullet.GetComponent<Rigidbody>().AddForce(ray.direction * bulletSpeed + Vector3.up, ForceMode.Impulse);
        }
    }
}
