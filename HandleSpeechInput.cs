using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSpeechInput : MonoBehaviour {

    //This is the Text that displays instructions.
    [SerializeField] private Text transfer;

    //Available colors that the player can choose from.
    private string[] colors;

    //If the user says "confirm," this boolean will be true.
    private bool confirmation;

    //The color the player said.
    private string answeredColor;

    //Boolean to see if the game is done. All questions answered.
    public bool gameComplete;

    //SceneController script that controls game flow.
    [SerializeField] private SceneController sceneController;

    //The score tracker for the game.
    [SerializeField] private TrackScore trackScore;

    /*
     * Initializes variables that will be used.
    */
	private void Awake () {
        gameComplete = false;
        confirmation = false;
        answeredColor = "";
        colors = new string[] { "red", "orange", "yellow", "green", "blue", "purple", "black", "brown", "cyan", "aqua", "maroon", "violet", "indigo", "lime", "crimson" };
	}

    /*
     * Finds the Text object used to display instructions. 
     * Find the TrackScore script.
    */
    private void Start()
    {
        transfer = GameObject.FindGameObjectWithTag("Instruction").GetComponent<Text>();
        trackScore = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
    }

    // Update is called once per frame
    private void Update () {
        Debug.Log("In Handle Speech Input Update Function");

        //Only do stuff when SceneController is waiting for a response.
        if (sceneController.currentState == SceneController.State.Wait)
        {
            Debug.Log("Listening");

            //Player confirmed an answer and said "continue"
            if (confirmation && gameObject.GetComponent<Text>().text.Contains("continue"))
            {
                //SceneController went through all the questions.
                if (sceneController.GetComponent<SceneController>().currentState == SceneController.State.Complete)
                {
                    //TrackScore score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
                    float accuracy = trackScore.getCorrect() / (trackScore.getCorrect() + trackScore.getIncorrect());
                    trackScore.setAccuracy(accuracy);
                    gameComplete = true;
                    GameObject.FindGameObjectWithTag("StroopObstacle").SetActive(false);
                }
                else //Set up question.
                {
                    sceneController.GetComponent<SceneController>().currentState = SceneController.State.Next;
                    transfer.text = "Please say the color of the word.";
                    answeredColor = "";
                    confirmation = false;
                }
            }
            //No answer yet. Listen for answer. Get confirmation if a color in the answer choices was mentioned.
            if (answeredColor == "")
            {
                foreach (string color in colors)
                {
                    //Check that answered color is an option.
                    if (gameObject.GetComponent<Text>().text.Contains(color))
                    {
                        answeredColor = color;
                        transfer.text = "Your answer: " + color + "." + " Please confirm by saying 'Yes' or 'No'.";
                    }
                }
            }
            else
            {
                //Confirm answer and check result.
                if (gameObject.GetComponent<Text>().text.Contains("yes"))
                {
                    confirmation = true;
                    transfer.text = "You confirmed the color: " + answeredColor;
                    string compare = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().randomColor.Split('_')[0];
                    
                    //Correct answer.
                    if (answeredColor == compare)
                    {
                        transfer.text = "Your confirmed answer of " + answeredColor + " is" + " CORRECT!" + " Please say 'continue.'";
                        if (confirmation)
                        {
                            trackScore.setCorrect(trackScore.getCorrect() + 1);
                            answeredColor = "";
                        }
                        
                        //sceneController.GetComponent<SceneController>().currentState = SceneController.State.Next;
                    }
                    else //Incorrect answer.
                    {
                        transfer.text = "Your confirmed answer of " + answeredColor + " is" + " incorrect. Please try again.";
                        if (confirmation)
                        {
                            trackScore.setIncorrect(trackScore.getIncorrect() + 1);
                            confirmation = false;
                        }
                        //GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>().incorrect += 1;
                        answeredColor = "";
                        confirmation = false;
                    }

                }
                else if (gameObject.GetComponent<Text>().text.Contains("no")) //User does not want to confirm answer. No penalty (not incorrect).
                {
                    confirmation = false;
                    transfer.text = "Please try again.";
                    answeredColor = "";
                }
            }
        }         
	}
}
