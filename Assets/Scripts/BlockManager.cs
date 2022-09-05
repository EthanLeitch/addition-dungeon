using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    
	public int solution;
	List<int> blockValues = new List<int>();  

    // Start is called before the first frame update
	void Start()
	{
		// Create an array called blocks for storing all the math blocks
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Math Block");

        // Add every block's value to the blockValues list
        foreach (GameObject block in blocks) 
        {
        	blockValues.Add(block.GetComponent<MathBlock>().randomNumber); 
            //Debug.Log(block.GetComponent<MathBlock>().randomNumber);        	
        }
        
        // Shuffle all items in the blockValues list
        for (int i = 0; i < blockValues.Count; i++) {
            int temp = blockValues[i];
            int randomIndex = Random.Range(i, blockValues.Count);
            blockValues[i] = blockValues[randomIndex];
            blockValues[randomIndex] = temp;
        }

        // Get the solution value with the combination of the first and second item in the shuffled list
        solution = blockValues[0] + blockValues[1];
        Debug.Log("Solution is: " + solution + " (" + blockValues[0] + ", " + blockValues[1] + ")");
	}

}