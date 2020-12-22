using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    //Gameobjects
    [SerializeField]
    private GameObject heart;
    private GameObject instantiatedHeart;

    //int and floats
    public int currentHealth { get; private set; }
    private const int maxHealth = 5;
    private const float heartPositionIncrament = 1.32f;

    //bools
    private bool isHit = false;
    private bool isDead = false;

    //Vectors
    [SerializeField]
    private Vector2 initialHeartTransform;
    private Vector2 previousHeartPosition;

    //Others
    private List<GameObject> hearts = new List<GameObject>();

    //References
    private Animator animator;
    private PlayerInput playerInput;
    private PlayerDashing playerDashing;
    private PlayerDashingDown playerDashingDown;
    private PlayerDashingUp playerDashingUp;

    private void Awake()
    {
        currentHealth = maxHealth;
        PlaceHearts();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerDashing = GetComponent<PlayerDashing>();
        playerDashingDown = GetComponent<PlayerDashingDown>();
        playerDashingUp = GetComponent<PlayerDashingUp>();
    }

    //Checks to see if player has lost enough health to be considered "died"
    public void CheckDeath()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
        else
        {
            return;
        }
    }

    //Function for the player to die
    private void Die()
    {
        playerInput.enabled = false;
        playerDashing.enabled = false;
        playerDashingDown.enabled = false;
        playerDashingUp.enabled = false;
        isDead = true;
        animator.SetBool("isDead", isDead);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
        Debug.Log("Testing....");
        }
    }

    //Place hearts at the start in the correct position, based on the initial set position of the first heart
    private void PlaceHearts()
    {
        for(int i = 0; i < maxHealth; i++)
        {
            if(i == 0)
            {
                instantiatedHeart = Instantiate(heart, initialHeartTransform, Quaternion.identity);
                previousHeartPosition = initialHeartTransform;
                hearts.Add(instantiatedHeart);
            }
            else
            {
                Vector2 nextHeartPosition = new Vector2(previousHeartPosition.x + heartPositionIncrament, previousHeartPosition.y);
                instantiatedHeart = Instantiate(heart, nextHeartPosition, Quaternion.identity);
                previousHeartPosition = nextHeartPosition;
                hearts.Add(instantiatedHeart);
            }
        }
    }
}
