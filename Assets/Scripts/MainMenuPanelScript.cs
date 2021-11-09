using UnityEngine;


public class MainMenuPanelScript : MonoBehaviour
{
    private static MainMenuPanelScript _instance;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _resetStatButton;
   
    public static MainMenuPanelScript Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Main Menu Panel is null!");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (_uiManager.statsPanel.activeSelf)
        {
            _resetStatButton.SetActive(true);
        }
        else
        {
            _resetStatButton.SetActive(false);
        }
    }
    //==============================================
    //New Game
    //==============================================
    public void NewGame()
    {
        Debug.Log("Loading New Game...");
        GameManager.Instance.PickWord();
        GameManager.Instance.gameOver = false;
        GameManager.Instance.gameWon = false;
       
        if (GameManager.Instance.customZombie == "false")
        {
            GameManager.Instance.zombie = Random.Range(0, 3);
        }
        _uiManager.HideTitlePanel();
        _uiManager.HideMainMenuPanel();
        _uiManager.HideStatsPanel();
        _uiManager.ShowHudPanel();
        HudPanelScript.Instance.UpdateTries();
        HudPanelScript.Instance.UpdateWord();
        HudPanelScript.Instance.ResetLetterGrid();
        ZombiePanelScript.Instance.SetupZombie();
    }
    //==============================================
    //Quit Game
    //==============================================
    public void QuitGame()
    {
        GameManager.Instance.SavePlayerData();
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
    //==============================================
    //Game Difficulty
    //==============================================
    public void GameLevel(int arg0)
    {
        int level = arg0;
        if (level == 2)
        {
            GameManager.Instance.gameLevel = 2;
            GameManager.Instance.SavePlayerData();
            GameManager.Instance.tries = 0;
            GameManager.Instance.AddTries(6);
        }
        else if (level == 1)
        {
            GameManager.Instance.gameLevel = 1;
            GameManager.Instance.SavePlayerData();
            GameManager.Instance.tries = 0;
            GameManager.Instance.AddTries(8);
        }
        else
        {
            GameManager.Instance.gameLevel = 0;
            GameManager.Instance.SavePlayerData();
            GameManager.Instance.tries = 0;
            GameManager.Instance.AddTries(10);
        }
    }
}
