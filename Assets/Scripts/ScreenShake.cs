using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

	public static ScreenShake Instance { get; private set; }

	private CinemachineVirtualCamera cinemachineVirtualCamera;
	private float shakeTimer; 
    private float shakeTimerTotal;
    private float startingIntensity;

    // Start is called before the first frame update
    public void Start()
    {
        // Making this script an instance makes it easily callable from other scripts
    	Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
    	CinemachineBasicMultiChannelPerlin cinemachineNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    	cinemachineNoise.m_AmplitudeGain = intensity;

    	shakeTimer = time;
        shakeTimerTotal = time;
        startingIntensity = intensity;
    }

    void Update()
    {
    	if(shakeTimer > 0) 
    	{
    		shakeTimer -= Time.deltaTime;

            // Mathf.Lerp decreases intensity over time
			CinemachineBasicMultiChannelPerlin cinemachineNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			cinemachineNoise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
    	}
    }

}
