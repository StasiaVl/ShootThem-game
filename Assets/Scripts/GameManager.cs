using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public enum GameStatus
{
    menu, play, pause, gameover
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button ResumeBtn;
    [SerializeField]
    private Button PlayBtn;
    [SerializeField]
    private Button QuitBtn;
    [SerializeField]
    private Button MenuBtn;
    [SerializeField]
    private Text FinTxt;
    [SerializeField]
    private Image MenuImage;
    [SerializeField]
    private GameObject Aim;
    [SerializeField]
    private GameObject enemyController;

    private GameObject evil;

    private GameStatus currentState = GameStatus.menu;
    public GameStatus CurrentState { get { return currentState; } }

    public static GameManager instance = null;
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
    }

    // Use this for initialization
    void Start()
    {
        ToMenu();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("space"))
       {
           Pause();
       }
    }

    public void BeginGame()
    {
        Time.timeScale = 1;
        currentState = GameStatus.play;
        Cursor.lockState = CursorLockMode.Locked;
        PlayBtn.gameObject.SetActive(false);
        QuitBtn.gameObject.SetActive(false);
        MenuImage.gameObject.SetActive(false);
        Aim.gameObject.SetActive(true);
        evil = Instantiate(enemyController) as GameObject;
    }

    public void GameOver(bool win)
    {
        Cursor.lockState = CursorLockMode.None;
        if (win)
        {
            FinTxt.text = "Congratulations!\nYou win!";
        } else
        {
            FinTxt.text = "Fail!\nYou lose!";
        }
        currentState = GameStatus.gameover;
        MenuBtn.gameObject.SetActive(true);
        Aim.gameObject.SetActive(false);
    }

    public void ToMenu()
    {
        currentState = GameStatus.menu;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Player.instance.Restart();
        PlayBtn.gameObject.SetActive(true);
        QuitBtn.gameObject.SetActive(true);
        ResumeBtn.gameObject.SetActive(false);
        MenuBtn.gameObject.SetActive(false);
        MenuImage.gameObject.SetActive(true);
        Aim.gameObject.SetActive(false);
        FinTxt.text = "";
        Destroy(evil);
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            currentState = GameStatus.play;
            ResumeBtn.gameObject.SetActive(false);
            MenuBtn.gameObject.SetActive(false);
            MenuImage.gameObject.SetActive(false);
            Aim.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Time.timeScale = 0;
            currentState = GameStatus.pause;
            ResumeBtn.gameObject.SetActive(true);
            MenuBtn.gameObject.SetActive(true);
            MenuImage.gameObject.SetActive(true);
            Aim.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
