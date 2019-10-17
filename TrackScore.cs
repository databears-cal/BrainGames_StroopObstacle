using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackScore : MonoBehaviour {

    //number of incorrect responses
    private int incorrect;

    //number of correct responses
    private int correct;

    //percent of questions answered correctly
    private float accuracy;
    
	//Initializes value for use.
	void Start () {
        correct = 0;
        incorrect = 0;
        accuracy = 0f;
	}

    //Returns incorrect.
    public int getIncorrect()
    {
        return incorrect;
    }

    //Returns correct.
    public int getCorrect()
    {
        return correct;
    }

    //Returns accuracy.
    public float getAccuracy()
    {
        return accuracy;
    }

    //Set incorrect.
    public void setIncorrect(int x)
    {
        incorrect = x;
    }

    //Set correct.
    public void setCorrect(int x)
    {
        correct = x;
    }

    //Set accuracy
    public void setAccuracy(float x)
    {
        accuracy = x;
    }
}
