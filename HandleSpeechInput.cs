using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSpeechInput : MonoBehaviour {

    Text transfer;
    private string[] colors;
    public bool confirmation;
    public string answeredColor;
    public bool gameComplete;
    public SceneController sceneController;

    // Use this for initialization
	private void Awake () {
        gameComplete = false;
        confirmation = false;
        answeredColor = "";
        colors = new string[] { "red", "orange", "yellow", "green", "blue", "purple", "black", "brown" };
	}

    private void Start()
    {
        transfer = GameObject.FindGameObjectWithTag("Instruction").GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update () {
        Debug.Log("In Handle Speech Input Update Function");
        if (sceneController.currentState == SceneController.State.Wait)
        {
            Debug.Log("Listening");
            if (confirmation && gameObject.GetComponent<Text>().text.Contains("continue"))
            {
                if (sceneController.GetComponent<SceneController>().currentState == SceneController.State.Complete)
                {
                    TrackScore score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
                    score.accuracy = score.correct / (score.correct + score.incorrect);
                    GameObject.FindGameObjectWithTag("StroopObstacle").SetActive(false);
                }
                else
                {
                    sceneController.GetComponent<SceneController>().currentState = SceneController.State.Next;
                    transfer.text = "Please say the color of the word.";
                    answeredColor = "";
                    confirmation = false;
                }
            }
            if (answeredColor == "")
            {
                foreach (string color in colors)
                {
                    if (gameObject.GetComponent<Text>().text.Contains(color))
                    {
                        answeredColor = color;
                        transfer.text = "Your answer: " + color + "." + " Please confirm by saying 'Yes' or 'No'.";
                    }
                }
            }
            else
            {
                if (gameObject.GetComponent<Text>().text.Contains("yes"))
                {
                    confirmation = true;
                    transfer.text = "You confirmed the color: " + answeredColor;
                    string compare = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().randomColor.Split('_')[0];
                    if (answeredColor == compare)
                    {
                        transfer.text = "Your confirmed answer of " + answeredColor + " is" + " CORRECT!" + " Please say 'continue.'";
                        if (confirmation)
                        {
                            GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>().correct += 1;
                            answeredColor = "";
                        }
                        
                        //sceneController.GetComponent<SceneController>().currentState = SceneController.State.Next;
                    }
                    else
                    {
                        transfer.text = "Your confirmed answer of " + answeredColor + " is" + " incorrect. Please try again.";
                        if (confirmation)
                        {
                            GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>().incorrect += 1;
                            confirmation = false;
                        }
                        //GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>().incorrect += 1;
                        answeredColor = "";
                        confirmation = false;
                    }

                }
                else if (gameObject.GetComponent<Text>().text.Contains("no"))
                {
                    confirmation = false;
                    transfer.text = "Please try again.";
                    answeredColor = "";
                }
            }
        } /*else if (sceneController.GetComponent<SceneController>().currentState == SceneController.State.Complete)
        {
            transfer.text = "Game complete. Please say 'exit' to leave the game.";
            if (gameObject.GetComponent<Text>().text.Contains("exit"))
            {                
                TrackScore score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
                score.accuracy = score.correct / (score.correct + score.incorrect);
                GameObject.FindGameObjectWithTag("StroopObstacle").SetActive(false);
            }
        }*/

        
	}
}
