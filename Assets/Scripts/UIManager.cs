using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.IO;

public class UIManager : MonoBehaviour
{
    //==============================================
    //Private Variables
    //==============================================
    private static UIManager _instance;

    [SerializeField]
    private GameObject _hudPanelGameOver;
    [SerializeField]
    private GameObject _hudPanelGameWon;
    [SerializeField]
    private GameObject _hudPanelLetterGrid;
    [SerializeField]
    private GameObject _titlePanel;
    [SerializeField]
    private GameObject _mainMenuPanel;

    [SerializeField]
    private Text _easyGamesText;
    [SerializeField]
    private Text _mediumGamesText;
    [SerializeField]
    private Text _hardGamesText;

    //==============================================
    //Public Variables
    //==============================================
    public GameObject statsPanel;
    public GameObject hudPanel;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is null!");
            }
            return _instance;
        }
    }
    //==============================================
    //On Awake
    //==============================================
    private void Awake()
    {
        _instance = this;
        HideHudPanel();
        ShowTitlePanel();
        ShowMainMenuPanel();
    }
    //==============================================
    //Hud Panel
    //==============================================
    public void ShowHudPanel()
    {
        if (GameManager.Instance.gameOver == false)
        {
            _hudPanelGameOver.SetActive(false);
            _hudPanelLetterGrid.SetActive(true);
        }
        if (GameManager.Instance.gameWon == false)
        {
            _hudPanelGameWon.SetActive(false);
            _hudPanelLetterGrid.SetActive(true);
        }
        hudPanel.SetActive(true);
    }
    public void HideHudPanel()
    {
        hudPanel.SetActive(false);
    }
    //==============================================
    //Title Panel
    //==============================================
    public void ShowTitlePanel()
    {
        statsPanel.SetActive(false);
        hudPanel.SetActive(false);
        _titlePanel.SetActive(true);
    }
    public void HideTitlePanel()
    {
        _titlePanel.SetActive(false);
    }
    //==============================================
    //Main Menu Panel
    //==============================================
    public void ShowMainMenuPanel()
    {
        _mainMenuPanel.SetActive(true);
    }
    public void HideMainMenuPanel()
    {
        _mainMenuPanel.SetActive(false);
    }
    //==============================================
    //Stats Panel
    //==============================================
    public void ShowStatsPanel()
    {
        GameManager.Instance.GetPlayerData();
        HideTitlePanel();
        HideHudPanel();
        UpdateStats();
        statsPanel.SetActive(true);

    }
    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }
    public void UpdateStats()
    {
        _easyGamesText.text = "";
        _easyGamesText.text += "EASY GAMES";
        _easyGamesText.text += "\n----------------------------";
        _easyGamesText.text += "\n Games Won:  " + GameManager.Instance.GetPlayerWins(0);
        _easyGamesText.text += "\n Games Lost:  " + GameManager.Instance.GetPlayerLosses(0);
        _easyGamesText.text += "\n Games Played:  " + GameManager.Instance.GetGamesPlayed(0);

        _mediumGamesText.text = "";
        _mediumGamesText.text += "MEDIUM GAMES";
        _mediumGamesText.text += "\n----------------------------";
        _mediumGamesText.text += "\n Games Won:  " + GameManager.Instance.GetPlayerWins(1);
        _mediumGamesText.text += "\n Games Lost:  " + GameManager.Instance.GetPlayerLosses(1);
        _mediumGamesText.text += "\n Games Played:  " + GameManager.Instance.GetGamesPlayed(1);

        _hardGamesText.text = "";
        _hardGamesText.text += "HARD GAMES";
        _hardGamesText.text += "\n----------------------------";
        _hardGamesText.text += "\n Games Won:  " + GameManager.Instance.GetPlayerWins(2);
        _hardGamesText.text += "\n Games Lost:  " + GameManager.Instance.GetPlayerLosses(2);
        _hardGamesText.text += "\n Games Played:  " + GameManager.Instance.GetGamesPlayed(2);
    }
}
