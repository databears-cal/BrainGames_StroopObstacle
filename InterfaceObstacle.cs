using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceObstacle : MonoBehaviour, Obstacle
{
    //-1 not started, 0 started, 1 won, 2 lost
    private int state;

    //obstacle to activate or deactivate
    [SerializeField] GameObject obstacle;

    //controls flow of obstacle
    [SerializeField] SceneController sceneControllerScript;

    //determines if obstacle state should still be tracked
    private enum trackObstacle
    {
        Track,
        StopTracking
    }

    //tracking state (Track or StopTracking)
    private trackObstacle trackingState;

    /*
     * Intialize variables
    */
    private void Start()
    {
        state = -1;
        trackingState = trackObstacle.Track;
    }

    /*
     * Activate the obstacle.
     * Get necessary script to track obstacle state.
    */
    public void startGame()
    {
        state = 0;
        obstacle.SetActive(true);
        sceneControllerScript = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    /*
     * Deactivate the obstacle. For the Stroop Obstacle, user will always win the game. 
     * We are testing the accuracy of their memory/reaction, so the game has to be completed to get a percentage.
    */
    public void endGame()
    {
        state = 2;
        obstacle.SetActive(false);
    }

    /*
     * Return state of game.
     * -1 not started
     * 0 started
     * 1 lost game
     * 2 won game
    */
    public int gameState()
    {
        return state;
    }

    /*
     * See when to stop tracking the obstacle and return the game state.
    */
    private void Update()
    {
        if (trackingState == trackObstacle.Track)
        {
            if (sceneControllerScript.currentState == SceneController.State.Complete)
            {
                trackingState = trackObstacle.Track;
                endGame();
            }
        }
    }
}
