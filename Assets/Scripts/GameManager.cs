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
    private GameObject Player;
    private Vector3 startingPos;

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
        startingPos = Player.transform.position;
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
        currentState = GameStatus.play;
        Cursor.lockState = CursorLockMode.Locked;
        PlayBtn.gameObject.SetActive(false);
        QuitBtn.gameObject.SetActive(false);
    }

    public void GameOver(bool win)
    {
        Cursor.lockState = CursorLockMode.None;
        if (win)
        {
            FinTxt.text = "Congratulations!\nYou win!";
        } else
        {
            FinTxt.text = "Congratulations!\nYou win!";
        }
        currentState = GameStatus.gameover;
        MenuBtn.gameObject.SetActive(true);
    }

    public void ToMenu()
    {
        currentState = GameStatus.menu;
        Time.timeScale = 1;
        Player.transform.position = startingPos;
        Cursor.lockState = CursorLockMode.None;
        PlayBtn.gameObject.SetActive(true);
        QuitBtn.gameObject.SetActive(true);
        ResumeBtn.gameObject.SetActive(false);
        MenuBtn.gameObject.SetActive(false);
        FinTxt.text = "";
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            currentState = GameStatus.play;
            ResumeBtn.gameObject.SetActive(false);
            MenuBtn.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Time.timeScale = 0;
            currentState = GameStatus.pause;
            ResumeBtn.gameObject.SetActive(true);
            MenuBtn.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
