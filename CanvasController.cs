using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    GameObject scene_controller;
    public Text instruction;

    private enum State
    {
        Wait,
        ColorReceived
    }
    private State currentState = State.Wait;

    // Start is called before the first frame update
    void Awake()
    {
        scene_controller = GameObject.FindGameObjectWithTag("SceneController");
    }

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
