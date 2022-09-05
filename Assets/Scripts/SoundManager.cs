using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    /* 
    DEVELOPER NOTE:
    Yes, this code is incrediably inflexible and stupid. If I had more time, I'd fix it so I don't have to expand this switch statement to
    accomodate new sound effects. However, I simply don't have enough time to do this.
    */

	public static AudioClip success, fail, door_open, sword_swing, player_hit, enemy_hit, enemy_killed, health_potion;
	static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        success = Resources.Load<AudioClip>("success");
        fail = Resources.Load<AudioClip>("fail");
        door_open = Resources.Load<AudioClip>("door_open");
        sword_swing = Resources.Load<AudioClip>("sword_swing");
        player_hit = Resources.Load<AudioClip>("player_hit");
        enemy_hit = Resources.Load<AudioClip>("enemy_hit");
        enemy_killed = Resources.Load<AudioClip>("enemy_killed");
        health_potion = Resources.Load<AudioClip>("health_potion");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
    	switch(clip){
    	case "success":
    		audioSrc.PlayOneShot(success);
    		break;
    	case "fail":
    		audioSrc.PlayOneShot(fail);
    		break;
    	case "door_open":
    		audioSrc.PlayOneShot(door_open);
            break;
        case "sword_swing":
            audioSrc.PlayOneShot(sword_swing);
            break;
        case "player_hit":
            audioSrc.PlayOneShot(player_hit);
            break;
        case "enemy_hit":
            audioSrc.PlayOneShot(enemy_hit);
            break;
        case "enemy_killed":
            audioSrc.PlayOneShot(enemy_killed);
            break;
        case "health_potion":
            audioSrc.PlayOneShot(health_potion);
            break;
    	}
    }

}
