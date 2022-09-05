using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    new public Rigidbody2D rigidbody;
    public Animator animator;
    public GameObject sword;
    public Animator swordAnimator;
    public BoxCollider2D swordCollider;

    public float health;
    public float maxHealth;
    public float speed;
    public float power;
    public float maxPower;

    private Vector2 moveDirection;
    private Vector3 mousePosition;
    private Vector2 swordDirection; 
    
    public void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        swordCollider = sword.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Use GetAxisRaw for Horizontal and Vertical movement and store it in moveX and moveY
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalising direction prevents the player from moving twice as fast on diagonals 
        moveDirection = new Vector2(moveX, moveY).normalized;

        Move();

        // Change animator moving variable to false or true so the Animation Controller knows how to transition between idling animations and walking animations
        if(moveDirection.x == 0 && moveDirection.y == 0)
        {
            animator.SetBool("moving", false);
        } else {
            animator.SetBool("moving", true);
        }
        
        if(health <= 0) 
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(GameUI.Instance.LoadLevelOnDeath(SceneManager.GetActiveScene().buildIndex, 1f));
        }

        SwordLogic();
    }

    void Move()
    {
        // Move the rigidbody
        rigidbody.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);        

        /* The reason that this moveDirection check needs to exist is so that the players direction variables remains constant while idling, 
        so that the idle states are facing the last direction the player moved in. */
        if(moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetFloat("xDirection", moveDirection.x);
            animator.SetFloat("yDirection", moveDirection.y);
        }

    }

    void SwordLogic()
    {
        // Assign mouse position to a Vector3
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Do calculations to find the sword direction it needs to be in
        Vector2 swordDirection = new Vector2(
            mousePosition.x - sword.transform.position.x,
            mousePosition.y - sword.transform.position.y
        );

        // If game is not paused, run rest of sword logic (movement and firing.)
        if(Time.timeScale != 0) 
        {
            sword.transform.up = swordDirection;
            
            if(Input.GetMouseButtonDown(0) && power != 0)
            {
                SoundManager.PlaySound("sword_swing");
                StartCoroutine(Attack());
            }
        }

    }

    public IEnumerator Attack() 
    {
        power -= 5;
        swordAnimator.SetBool("Attacking", true);

        // These precise timings sync up with the animation correctly.
        yield return new WaitForSeconds(0.25f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.875f);
        swordAnimator.SetBool("Attacking", false);
        swordCollider.enabled = false;
    }

}
