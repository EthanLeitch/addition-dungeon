using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public GameObject settingsPanel;

    // Crossfade variables
    public GameObject crossfade;
    public Animator transition;
    private float transitionTime = 1f;

	public void Start()
    {
		settingsPanel.SetActive(false);
        crossfade.SetActive(false);
	}

    public void StartGame()
    {
        crossfade.SetActive(true);
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadLevel(1));
    }

    public void OpenSettings()
    {
    	settingsPanel.SetActive(true);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void CloseSettings()
    {
    	settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
    	Debug.Log("Quitting...");
        Application.Quit();
    }

    // Simple fade-to-black level transition
    public IEnumerator LoadLevel(int levelIndex) 
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
