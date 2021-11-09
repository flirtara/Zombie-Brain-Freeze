using UnityEngine;

public class Zombies : MonoBehaviour
{
    protected Animator anim;
    public AudioSource deathMoan;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.Rebind();
        anim.SetBool("isDead", false);
    }
    private void Update()
    {
        if ( GameManager.Instance.zombieHurt == true )
        {
            anim.SetTrigger("Hurt");
            GameManager.Instance.zombieHurt = false;
        }
        if (GameManager.Instance.zombieDead == true)
        {
            anim.SetTrigger("Death");
            GameManager.Instance.zombieDead = false;
        }
    }
    public void TriggerHurt()
    {
        anim.SetTrigger("Hurt");
    }
    public void TriggerDeath()
    {
        Debug.Log("Death Triggered!");    
        anim.SetBool("isDead", true);
        anim.SetTrigger("Death");
    }
    public void DeathMoan()
    {
        deathMoan.Play();
    }
    public void ShowGameWon()
    {
        HudPanelScript.Instance.ShowGameWon();
    }
}