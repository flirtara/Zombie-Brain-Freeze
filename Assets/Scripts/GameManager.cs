using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //==============================================
    //Private Variables
    //==============================================
    private static GameManager _instance;
    private UIManager _uiManager;
    [SerializeField]
    private int[] _gamesLost;
    [SerializeField]
    private int[] _gamesWon;
    [SerializeField]
    private int[] _gamesPlayed;
    private int _rand;
    [SerializeField]
    private string _difficulty;
    private int _difficultyValue;
    private string[] _wordArray;
    private Button hintUsedButton;
    [SerializeField]
    private GameObject _zombiePanel;

    //==============================================
    //Public Variables
    //==============================================
    public int tries;
    public int bones;
    public int zombie;
    public int gameLevel;
    public string word;
    public TextAsset[] wordFiles;
    public List<char> hintLetters = new List<char>();
    public string wordLine;
    public char[] currWordGuess;
    public char[] wordCharacters;
    public int charLeft;
    public bool gameOver;
    public bool gameWon;
    public bool zombieHurt;
    public bool zombieDead;
    public string customZombie;
    public int hintCost = 900;

    //==============================================
    //GameObject Instance
    //==============================================
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
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
    }
    //==============================================
    //On Start
    //==============================================
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        GetPlayerData();
        _uiManager.HideHudPanel();
        _uiManager.ShowTitlePanel();
        _uiManager.ShowMainMenuPanel();
        gameOver = false;
        gameWon = false;
        zombieHurt = false;
        zombieDead = false;
    }
    //==============================================
    //Pick Random Word
    //==============================================
    public void PickWord()
    {

        _wordArray = wordFiles[gameLevel].text.Split('\n');
        Debug.Log("WordFile:  " + wordFiles[gameLevel]);
        _rand = Random.Range(0, _wordArray.Length);
        wordLine = _wordArray[_rand];
        string[] splitArray = wordLine.Split(char.Parse("|"));
        word = splitArray[0];
        _difficulty = splitArray[1];
        _difficultyValue = System.Convert.ToInt32(_difficulty);
        Debug.Log("Game Level:  " + gameLevel + "  Word:  " + word + "  Difficulty Level:  " + _difficulty);
        wordCharacters = new char[word.Length];
        currWordGuess = new char[word.Length];
        charLeft = word.Length;
        Debug.Log("Loading New Word....." + word + " character count:  " + word.Length);

        //==============================================
        //Load Word Characters 
        //==============================================
        for (int i = 0; i < word.Length; i++)
        {
            wordCharacters[i] = word[i];
            currWordGuess[i] = '?';
            char upperChar = char.ToUpper(wordCharacters[i]);
            char lowerChar = char.ToLower(wordCharacters[i]);
            if (!hintLetters.Contains(upperChar) && !hintLetters.Contains(lowerChar))
            {
                hintLetters.Add(wordCharacters[i]);
            }
        }
        System.Array.Clear(_wordArray, 0, _wordArray.Length);
    }
    //==============================================
    //Letter Grid Letter Button Pressed
    //==============================================
    public void LetterButtonClick(string letter)
    {
        char[] myChars = letter.ToCharArray();
        bool letterFound = false;
        int number = 0;
        for (int i = 0; i < wordCharacters.Length; i++)
        {
            if (wordCharacters[i] == char.ToUpper(myChars[0]) || wordCharacters[i] == char.ToLower(myChars[0]))
            {
                //Letter Found
                letterFound = true;
                currWordGuess[i] = wordCharacters[i];
                charLeft--;

            }
            number++;
        }
        if (letterFound == false)
        {
            DeleteTries(1);
        }
        else
        {
            HudPanelScript.Instance.UpdateWord();
            zombieHurt = true;
            char upperChar = char.ToUpper(myChars[0]);
            char lowerChar = char.ToLower(myChars[0]);
            hintLetters.Remove(upperChar);
            hintLetters.Remove(lowerChar);
            if (charLeft == 0)
            {
                gameWon = true;
                AddWins();
                int newBones;
                //Update Bone Count
                if (gameLevel == 2)
                {
                    newBones = _difficultyValue * 3;
                }
                else if (gameLevel == 1)
                {
                    newBones = _difficultyValue * 2;
                }
                else
                {
                    newBones = _difficultyValue * 1;
                }
                AddBones(newBones);
                zombieDead = true;
                Debug.Log("Game Won!");
                _uiManager.ShowMainMenuPanel();
            }
        }
    }
    //==============================================
    //Update Bones
    //==============================================
    public void AddBones(int number)
    {
        bones = bones + number;
        HudPanelScript.Instance.UpdateBones();
        PlayerPrefs.SetInt("Bones", bones);
        PlayerPrefs.Save();
    }
    public void DeleteBones(int number)
    {
        Debug.Log("Deleting " + number + "bones");
        bones -= number;
        HudPanelScript.Instance.UpdateBones();
        PlayerPrefs.SetInt("Bones", bones);
        PlayerPrefs.Save();
    }
    //==============================================
    //Update Tries
    //==============================================
    public void AddTries(int number)
    {
        tries += number;
        PlayerPrefs.SetInt("Tries", tries);
        PlayerPrefs.Save();
    }
    public void DeleteTries(int number)
    {
        tries = tries - number;
        HudPanelScript.Instance.UpdateTries();
        PlayerPrefs.SetInt("Tries", tries);
        PlayerPrefs.Save();
        if (tries == 0)
        {
            gameOver = true;
            AddLosses();
            Debug.Log("Game Lost!");
            HudPanelScript.Instance.ShowGameOver();
            HudPanelScript.Instance.UpdateWord();
            _uiManager.ShowMainMenuPanel();
        }
    }
    //==============================================
    //Update Statistics
    //==============================================
    public void AddWins()
    {
        _gamesWon[gameLevel]++;
        _gamesPlayed[gameLevel]++;
        SavePlayerData();
    }
    public void AddLosses()
    {
        _gamesLost[gameLevel]++;
        _gamesPlayed[gameLevel]++;
        SavePlayerData();
    }
    public void ClearStats()
    {
        for (int i = 0; i < 3; i++)
        {
            _gamesPlayed[i] = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            _gamesWon[i] = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            _gamesLost[i] = 0;
        }
        SavePlayerData();
        _uiManager.UpdateStats();
    }
    //==============================================
    //Update Player Saved Data
    //==============================================
    public void GetPlayerData()
    {
        Debug.Log("Getting Player Data....");
        gameLevel = PlayerPrefs.GetInt("GameLevel");
        tries = PlayerPrefs.GetInt("Tries", 6);
        bones = PlayerPrefs.GetInt("Bones", 0);
        if (bones < 0)
        {
            bones = 0;
        }
        zombie = PlayerPrefs.GetInt("Zombie", 1);
        customZombie = PlayerPrefs.GetString("CustomZombie", "false");
        for (int i = 0; i < 3; i++)
        {
            _gamesPlayed[i] = PlayerPrefs.GetInt("GamesPlayed" + i, 0);
        }
        for (int i = 0; i < 3; i++)
        {
            _gamesWon[i] = PlayerPrefs.GetInt("GamesWon" + i, 0);
        }
        for (int i = 0; i < 3; i++)
        {
            _gamesLost[i] = PlayerPrefs.GetInt("GamesLost" + i, 0);
        }
    }
    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("Zombie", zombie);
        PlayerPrefs.SetInt("Tries", tries);
        PlayerPrefs.SetInt("Bones", bones);
        PlayerPrefs.SetInt("GameLevel", gameLevel);
        PlayerPrefs.SetString("CustomZombie", customZombie);

        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("GamesPlayed" + i, _gamesPlayed[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("GamesWon" + i, _gamesWon[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("GamesLost" + i, _gamesLost[i]);
        }
        PlayerPrefs.Save();
    }
    //==============================================
    //Get Player Stat Data
    //==============================================
    public int GetPlayerLosses(int level)
    {
        return _gamesLost[level];
    }
    public int GetPlayerWins(int level)
    {
        return _gamesWon[level];
    }
    public int GetGamesPlayed(int level)
    {
        return _gamesPlayed[level];
    }
    //==============================================
    //Give Hint
    //==============================================
    public void GetHint()
    {
        int sizeOfList = hintLetters.Count;
        int index = Random.Range(0, sizeOfList);
        char hint = hintLetters[index];
        hintLetters.Remove(hint);
        string buttonName = "Button" + char.ToUpper(hint);
        hintUsedButton = GameObject.Find(buttonName).GetComponent<Button>();
        if (hintUsedButton != null)
        {
            hintUsedButton.onClick.Invoke();
        }
    }
}