using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackScore : MonoBehaviour {
    public int incorrect;
    public int correct;
    public float accuracy;
    
	// Use this for initialization
	void Start () {
        correct = 0;
        incorrect = 0;
        accuracy = 0;
	}
}
