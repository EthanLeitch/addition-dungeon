using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{

    public static GameUI Instance { get; private set; }

    // Reset timer variables
    public string key = "r";
    private float startTime = 0f;
    private float timer = 0f;
    private float holdTime = 2f;

    // UI managing variables
    public GameObject hud;
    public GameObject pauseMenu;
    public GameObject confirmMenu;
    public GameObject numberDisplay;
    private bool gameIsPaused = false;

    // Crossfade variables
    public GameObject crossfade;
    public Animator transition;
    
    public GameObject resetBar;
    private Slider resetBarSlider;

    public Slider healthBarSlider;
    public Slider powerBarSlider;

    public GameObject player;
    private PlayerController playerScript;

	public void Start()
    {
        // Making this script an instance makes it easily callable from other scripts
        Instance = this;

        hud.SetActive(true);
        pauseMenu.SetActive(false);
        confirmMenu.SetActive(false);
        crossfade.SetActive(true);

        // Get the solution variable and display it on the numberDisplay panel
        GameObject bm = GameObject.Find("BlockManager");
        numberDisplay.GetComponent<TextMeshProUGUI>().text = "" + bm.GetComponent<BlockManager>().solution;

        resetBarSlider = resetBar.GetComponent<Slider>();
        resetBar.SetActive(false);

        playerScript = player.GetComponent<PlayerController>();

        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            playerScript.health = PlayerPrefs.GetFloat("playerHealth");
            playerScript.power = PlayerPrefs.GetFloat("playerPower");
        }
	}

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }

        if(!gameIsPaused) 
        {
            HandleRoomResetting();
            healthBarSlider.value = playerScript.health;
            powerBarSlider.value = playerScript.power;
        }
    }

    public void Pause()
    {
        gameIsPaused = true;
        Time.timeScale = 0;

        hud.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        gameIsPaused = false;
        Time.timeScale = 1;

        hud.SetActive(true);
        pauseMenu.SetActive(false);
        confirmMenu.SetActive(false);
    }

    public void ConfirmExit() 
    {
        pauseMenu.SetActive(false);
        confirmMenu.SetActive(true);
    }

    public void ExitToMenu()
    {
    	Debug.Log("Exiting to main menu...");
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(0, 1f));
    }

    // Allows the player to reset the room by holding down the 'r' key for 3 seconds (holdTime)
    void HandleRoomResetting()
    {
        // Starts the timer from when the key is pressed
        if (Input.GetKeyDown(key))
        {
            startTime = Time.time;
            timer = startTime;
            resetBar.SetActive(true);
        }
 
        // Adds time onto the timer variable so long as the key is pressed
        if (Input.GetKey(key))
        {
            timer += Time.deltaTime;
            resetBarSlider.value = timer - startTime;

            // Once the timer float has added on the required holdTime, changes the bool (for a single trigger), and calls the function
            if (timer > (startTime + holdTime))
            {
                // Reload current scene
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, 1f));
            }
        } else {
            resetBar.SetActive(false);
        }
    }

    // Simple fade-to-black level transition
    public IEnumerator LoadLevel(int levelIndex, float transitionTime) 
    {
        PlayerPrefs.SetFloat("playerHealth", playerScript.health);
        PlayerPrefs.SetFloat("playerPower", playerScript.power);
        PlayerPrefs.Save();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

        // Simple fade-to-black level transition
    public IEnumerator LoadLevelOnDeath(int levelIndex, float transitionTime) 
    {
        PlayerPrefs.SetFloat("playerHealth", 20);
        PlayerPrefs.SetFloat("playerPower", 20);
        PlayerPrefs.Save();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }


}