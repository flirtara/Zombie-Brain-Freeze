using UnityEngine;

public class ZombiePanelScript : MonoBehaviour
{

    //==============================================
    //Private Variables
    //==============================================
    private static ZombiePanelScript _instance;
    [SerializeField]
    private int _myZombie;
    [SerializeField]
    private GameObject[] _zombies;
    public GameObject newZombie;
    
    //==============================================
    //GameObject Instance
    //==============================================
    public static ZombiePanelScript Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Zombie Panel is null!");
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
    // Update is called once per frame
    public void SetupZombie()
    {
        Debug.Log("Setting Up Zombie...");
        Destroy(newZombie);
        _myZombie = GameManager.Instance.zombie;
        var pos = _zombies[_myZombie].transform.position;
        newZombie = Instantiate(_zombies[_myZombie]) as GameObject;
        newZombie.transform.SetParent(_instance.transform, false);
        newZombie.transform.localPosition = pos;
    }
}
