using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AudioClip shootSound;
    private AudioSource walkSound;
    [SerializeField]
    private GameObject bullet; //for Instantiation
    private float bulletSpeed = 5;
    private GameObject currentBullet;
    public int health = 3;
    public bool moving = true;
    private EnemyController controller;

    //Where to go and where to shoot:
    private Vector3 aim;
    private Vector3 direction;
    private Vector3 shootDirection;

    private void Start()
    {
        walkSound = GetComponent<AudioSource>();
        controller = transform.parent.gameObject.GetComponent<EnemyController>();
        direction = transform.position;
        if (moving)
        {
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(ChangeDirection());
            StartCoroutine(Shoot());
        }
    }

    private void Update()
    {
        //Play walking sound only for moving enemies and during the Play mode
        if (!walkSound.isPlaying && GameManager.instance.CurrentState == GameStatus.play && direction != transform.position)
        {
            walkSound.Play();
        }
        else if (walkSound.isPlaying && GameManager.instance.CurrentState != GameStatus.play)
        {
            walkSound.Stop();
        }
    }

    private void FixedUpdate()
    {
        if(moving && IsInBounds())
            transform.Translate(direction * Time.deltaTime);
        else if(!IsInBounds())
        {
            StopCoroutine(ChangeDirection());
            StartCoroutine(ChangeDirection());
        }
    }

    //Check if enemy is going too far
    private bool IsInBounds()
    {
        aim = transform.position + (direction * Time.deltaTime);
        return aim.x < controller.max.x && aim.z < controller.max.z &&
                aim.x > controller.min.x && aim.z > controller.min.z;
    }

    public void GetHit(int points)
    {
        StartCoroutine(ColorChange());
        if (health > points)
            health -= points;
        else
        {
            gameObject.GetComponentInParent<EnemyController>().DetectDeath();
            Destroy(gameObject, 0.1f);
        }
    }

    //Shows that enemy was wounded
    IEnumerator ColorChange()
    {
        foreach (Renderer m in gameObject.GetComponentsInChildren<Renderer>())
           m.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        foreach (Renderer m in gameObject.GetComponentsInChildren<Renderer>())
           m.material.color = Color.white;
    }
    
    IEnumerator ChangeDirection()
    {
        direction = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChangeDirection());
    }

    IEnumerator Shoot()
    {
        if (Physics.Linecast(transform.position, Player.instance.transform.position) //if sees player
            && GameManager.instance.CurrentState == GameStatus.play)
        {
            GetComponent<AudioSource>().PlayOneShot(shootSound);
            shootDirection = Player.instance.transform.position - transform.position;
            //Look at the Player:
            transform.forward = shootDirection.normalized;

            currentBullet = Instantiate(bullet) as GameObject;
            currentBullet.transform.parent = transform;
            currentBullet.transform.localPosition = Vector3.forward + Vector3.up;
            currentBullet.transform.parent = null;  
            Destroy(currentBullet, 1f);

            shootDirection += transform.position - currentBullet.transform.position
                            + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)); //a bit of deviation
            currentBullet.GetComponent<Rigidbody>().
                AddForce(shootDirection * bulletSpeed + Vector3.up, ForceMode.Impulse);

            yield return new WaitForSeconds(2.5f);
        } else
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Shoot());
    }
}
