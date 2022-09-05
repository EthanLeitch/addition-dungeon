using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

	new public Rigidbody2D rigidbody;
    public Animator animator;
    private NavMeshAgent navMeshAgent;

    public GameObject player;
	private Transform target;
	NavMeshAgent agent;
    
    public float health;
    public float attack;

    private PlayerController playerScript;
    private bool engaged;

    public CircleCollider2D circleCollider;

    private GameObject exitDoor;
    private Animator exitDoorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // Setup AI agent
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		
        player = GameObject.FindGameObjectWithTag("Player");
		target = player.transform;

		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

		// Freeze enemy so it doesn't move until the player enters its activation range
        navMeshAgent.enabled = false;

        playerScript = player.GetComponent<PlayerController>();

        exitDoor = GameObject.Find("Exit Door");
        exitDoorAnimator = exitDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.enabled) 
        {
            agent.SetDestination(target.position);
            
            if(health <= 0 || exitDoorAnimator.GetBool("doorOpen") == true)
            {
                StartCoroutine(Death());
            }
        }
        
    }

    // This will run when something enters the enemies activation range
    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "Player")
    	{
            animator.SetTrigger("Awake");

            // This repurposes the Circle Collider from an activator to a hitbox for the sword to detect. 
            // The reason I didn't do this with the polygon 2D collider was because it was too small and difficult for the player to hit.
            circleCollider.radius = 0.5f;
    		
            // Unfreeze enemy so it can move and attack the player
            navMeshAgent.enabled = true;
    	}

        if(col.gameObject.tag == "Sword" && engaged != true && navMeshAgent.enabled == true)
        {
            StartCoroutine(EngageFrames("taking_damage"));
        }
    }

    // This will run when something collides with the enemies hitbox
    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" && engaged != true)
        {
            StartCoroutine(EngageFrames("attacking"));
        }
    }


    // Flashing frames coroutine. This will trigger when the enemy is attacking or being attacked.
    public IEnumerator EngageFrames(string state) 
    {
        // This "engaged" switch is to make sure that this coroutine can only be running once at any given time.
        if(engaged != true) 
        {
            engaged = true;
            navMeshAgent.enabled = false;
            animator.SetBool("Flashing", true);
            
            if(state == "attacking")
            {
                SoundManager.PlaySound("player_hit");
                playerScript.health -= attack;
            }

            else if(state == "taking_damage")
            {
                SoundManager.PlaySound("enemy_hit");
                health -= 5;
            }
            
            yield return new WaitForSeconds(3f);
            navMeshAgent.enabled = true;
            animator.SetBool("Flashing", false);
            engaged = false;
        }
    }

    // Death coroutine. Switches off Nav Mesh Agent, plays death animation, and disables itself.
    public IEnumerator Death()
    {
        navMeshAgent.enabled = false;
        animator.SetTrigger("Dead");
        SoundManager.PlaySound("enemy_killed");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
