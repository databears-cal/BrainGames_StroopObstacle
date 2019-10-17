using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    //Controls flow of game.
    [SerializeField] private GameObject scene_controller;

    //Text that gives player instructions. Changes depending on their responses.
    [SerializeField] private Text instruction;

    //Wait for a response or stop when a color is received.
    private enum State
    {
        Wait,
        ColorReceived
    }
    
    //Current status of this gameobject.
    private State currentState = State.Wait;

    /*
     *Awake is called before the first frame update. Identifies SceneController
    */
    void Awake()
    {
        scene_controller = GameObject.FindGameObjectWithTag("SceneController");
    }

    /*
     * Identifies random color SceneController selected and updates instruction. 
     * This only happens once to intialize the first question. 
     * Future questions handled by HandleSpeechInput script.
    */
    private void Update()
    {
        if (currentState == State.Wait)
        {
            if (scene_controller.GetComponent<SceneController>().randomColor != null)
            {
                string color = scene_controller.GetComponent<SceneController>().randomColor;
                color = color.Split('_')[0];
                string s = "Please say the color of the card.";
                instruction.text = s;
                currentState = State.ColorReceived;
            }
        }
    }
}
