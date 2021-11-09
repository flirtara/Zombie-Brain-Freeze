using UnityEngine;
using UnityEngine.UI;

public class HudPanelScript : MonoBehaviour
{
    //==============================================
    //Private Variables
    //==============================================
    [SerializeField]
    private Text _bonesText;
    [SerializeField]
    private Text _wordText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _gameWonText;
    [SerializeField]
    private bool _letterFound;
    private static HudPanelScript _instance;
    [SerializeField]
    private int _randomMessage;
    [SerializeField]
    private GameManager _gameManager;
    private string _myString;
    [SerializeField]
    private Component[] _letterButtons;
    private int _hintCost;


    //==============================================
    //Public Variables
    //==============================================
    public GameObject LetterGrid;
    public GameObject GameOverPanel;
    public GameObject GameWonPanel;
    public GameObject ZombiePanel;
    public Button _hintBuyButton;
    public Button _hintAdButton;
    public Text triesText;

    //==============================================
    //GameObject Instance
    //==============================================
    public static HudPanelScript Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Hud Panel Script is null!");
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
        UpdateTries();
        UpdateBones();
        UpdateWord();
    }
    //==============================================
    //On Start
    //==============================================
    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _letterButtons = LetterGrid.GetComponentsInChildren<Button>();
        _hintCost = GameManager.Instance.hintCost;
    }
    //==============================================
    //On Update
    //==============================================
    private void Update()
    {
        if(GameManager.Instance.gameOver == false || GameManager.Instance.gameWon == false)
        {
            ShowHintBuyButton();
        }
        if (GameManager.Instance.gameOver == true || GameManager.Instance.gameWon == true)
        {
            HideHintBuyButton();
        }
        var _currBones = GameManager.Instance.bones;
        if ( _currBones >= _hintCost )
        {
            EnableBuyButton();
        }
        else
        {
            DisableBuyButton();
        }
    }
    //==============================================
    //Update Bones
    //==============================================
    public void UpdateBones()
    {
        _bonesText.text = "Bones:  " + GameManager.Instance.bones;
    }
    //==============================================
    //Update Tries
    //==============================================
    public void UpdateTries()
    {
        triesText.text = "Tries Left:  " + GameManager.Instance.tries;
    }
    //==============================================
    //Update Word
    //==============================================
    public void UpdateWord()
    {
        if (GameManager.Instance.gameOver == false)
        {
            _myString = new string(GameManager.Instance.currWordGuess);
        }
        else
        {
            _myString = new string(GameManager.Instance.wordCharacters);
        }
        _wordText.text = _myString;
    }
    //==============================================
    //Show and Hide LetterGrid
    //==============================================
    public void ShowLetterGrid()
    {
        HideGameOver();
        HideGameWon();
        LetterGrid.SetActive(true);
    }
    public void HideLetterGrid()
    {
        LetterGrid.SetActive(false);
    }
    //==============================================
    //Reset Letter Grid For New Game
    //==============================================
    public void ResetLetterGrid()
    {
        foreach (Button myButton in _letterButtons)
            myButton.interactable = true;
    }
    //==============================================
    //Show and Hide Game Over Panel
    //==============================================
    public void ShowGameOver()
    {
        HideLetterGrid();
        GameOverPanel.SetActive(true);
    }
    public void HideGameOver()
    {
        GameOverPanel.SetActive(false);
    }
    //==============================================
    //Show and Hide Game Won Panel
    //==============================================
    public void ShowGameWon()
    {
        HideLetterGrid();
        GameWonPanel.SetActive(true);
    }
    public void HideGameWon()
    {
        GameWonPanel.SetActive(false);
    }
    //==============================================
    //Show and Hide Buy Hint Button
    //==============================================
    public void ShowHintBuyButton()
    {
        _hintBuyButton.gameObject.SetActive(true);
    }
    public void HideHintBuyButton()
    {
        _hintBuyButton.gameObject.SetActive(false);
    }
    public void EnableBuyButton()
    {
        _hintBuyButton.interactable = true;
    }
    public void DisableBuyButton()
    {
        _hintBuyButton.interactable = false;
    }
    //==============================================
    //buy hint button
    //==============================================
    public void BuyHint()
    {
        GameManager.Instance.DeleteBones(GameManager.Instance.hintCost);
        GameManager.Instance.GetHint();
    }
}