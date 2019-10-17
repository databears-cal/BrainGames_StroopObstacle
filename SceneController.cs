using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SceneController : MonoBehaviour
{
    //Listener with IBM Watson scripts. DO NOT edit these scripts (from Watson Unity SDK)
    [SerializeField] private GameObject listener;

    //Parameter to change number of cards (questions). 
    public int numCardsDifficulty;
    
    //Random color generated for current question. This variable is reused for every question.
    public string randomColor;

    //Randomly generated number corresponding to randomColor in shuffled array.
    private int randomNumber;

    //Color of the card. Now only has white (previously had 9 colors)
    [SerializeField] private Sprite[] images;

    //Unused in this version. Previously used sprites instead of text for card colors.
    [SerializeField] private Sprite[] labels;

    //Dictionary matching 8 colors (strings) to their corresponding Color32.
    private Dictionary<string, Color32> string_to_color;

    //Colors closely related to red.
    private Dictionary<string, Color32> redColors;

    //Colors closely related to orange (unused as of now).
    private Dictionary<string, Color32> orangeColors;

    //Colors closely related to yellow (unused as of now).
    private Dictionary<string, Color32> yellowColors;

    //Colors closely related to green.
    private Dictionary<string, Color32> greenColors;

    //Colors closely related to blue.
    private Dictionary<string, Color32> blueColors;

    //Colors closely related to purple.
    private Dictionary<string, Color32> purpleColors;

    

    //Used to control game flow.
    public enum State
    {
        Wait,
        Next,
        Complete
    }

    //Which state game is currently in.
    public State currentState;

    //Array of numbers 0 to 7. This array will be shuffled every question.
    int[] numbers;

    //Array of numbers 0 to 7. This array will be shuffled every question. One number will be picked to choose a color.
    int[] labels_num;

    //Will store 4 random colors every question. These are the answer choices the player can choose from.
    string[] answer_choices;

    //Current question number.
    public int currentProb;

    //Score keeping script.
    [SerializeField] TrackScore trackScore;

    /*
     * Occurs before the first update frame. Sets up the first question.
    */
    private void Awake()
    {
        currentState = State.Wait;
        currentProb = 1;

        //Instantiate(canvas);

        string_to_color = new Dictionary<string, Color32>();
        setUpDictionary();

        setUpCurrentCard();

        assignAnswerChoices();
        
    }

    /*
     * Sets IBM Watson GameObject active.  
    */
    private void Start()
    {
        listener.SetActive(true);
    }

    /*
     * Controls gameflow.
     * When the state is State.Wait: do nothing (wait for HandleSpeechInput script).
     * When the state is State.Next: HandleSpeechInput detected correct answer and shuffles cards for next question.
     * When the state is State.Complete: game has been completed.
    */
    private void Update()
    {
        switch (currentState)
        {
            case State.Wait:
                break;
            case State.Next:
                Shuffle();
                currentState = State.Wait;
                break;
            case State.Complete:
                Debug.Log("Game complete.");
                EndGame();
                break;                
        }
    }

    /*
     * Shuffle cards and setup new problem. End game if number if questions reached.
     */
    public void Shuffle()
    {

        if (currentProb < numCardsDifficulty)
        {
            setUpCurrentCard();
            assignAnswerChoices();
            currentProb++;
        }
        else
        {
            currentState = State.Complete;
        }            
    }

    /*
     * Shuffles an int array randomly. Used to shuffle numbers array and select a randomColor.
     * Input: int array
     * Output: shuffled int array
     */
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for(int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = temp;
        }
        return newArray;
    }

    /*
     * Shuffles a string array randomly. Used to shuffle answer choices.
     * Input: string array
     * Output: shuffled string array
    */
    private string[] ShuffleAnswers(string[] str)
    {
        string[] newArray = str.Clone() as string[];
        for(int i = 0; i < newArray.Length; i++)
        {
            string temp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = temp;
        }
        return newArray;
    }

    /*
     * Set up current question.
     * Gets randomColor (answer) for question.
     * Randomly assigns string on canvas and gives it a color corresponding to randomColor.
    */
    private void setUpCurrentCard()
    {
        numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        numbers = ShuffleArray(numbers);

        randomNumber = Random.Range(0, numbers.Length);
        randomColor = labels[randomNumber].name; //changed from images to labels    

        randomColor = subsetColor(randomColor);

        labels_num = ShuffleArray(numbers);
        int label = labels_num[Random.Range(0, labels_num.Length)];

        Text colorText = GameObject.FindGameObjectWithTag("Color").GetComponent<Text>();
        colorText.text = labels[Random.Range(0, numbers.Length)].name.Split('_')[0];
        colorText.color = string_to_color[randomColor];
        
    }

    /*
     * Takes a color and if there is a related color, randomly select one from the dictionary (i.e. maroon is a subset of red).
     * Input: randomColor string
     * Output: new color string
    */
    private string subsetColor(string color)
    {
        string str = color.Split('_')[0];
        if (str == "red")
        {
            int i = Random.Range(0, redColors.Count);
            string[] keys = redColors.Keys.ToArray();
            return keys[i];
        }
        else if (str == "green")
        {
            int i = Random.Range(0, greenColors.Count);
            string[] keys = greenColors.Keys.ToArray();
            return keys[i];

        }
        else if (str == "blue")
        {
            int i = Random.Range(0, blueColors.Count);
            string[] keys = blueColors.Keys.ToArray();
            return keys[i];

        }
        else if (str == "purple")
        {
            int i = Random.Range(0, purpleColors.Count);
            string[] keys = purpleColors.Keys.ToArray();
            return keys[i];

        }
        else
        {
            return str;
        }
    }

    /*
     * Places answer choices (randomly chosen already) on canvas. 
    */
    private void assignAnswerChoices()
    {
        answer_choices = new string[4];
        answer_choices[0] = randomColor;
        for (int i = 1; i < 4; i++)
        {
            int rand = Random.Range(0, numbers.Length);
            while (rand == randomNumber)
            {
                rand = Random.Range(0, numbers.Length);
            }
            answer_choices[i] = labels[rand].name.Split('_')[0];
        }
        answer_choices = ShuffleAnswers(answer_choices);
        GameObject.FindGameObjectWithTag("Option1").GetComponent<Text>().text = answer_choices[0];
        GameObject.FindGameObjectWithTag("Option2").GetComponent<Text>().text = answer_choices[1];
        GameObject.FindGameObjectWithTag("Option3").GetComponent<Text>().text = answer_choices[2];
        GameObject.FindGameObjectWithTag("Option4").GetComponent<Text>().text = answer_choices[3];
    }

    /*
     * Assigns corresponding color to its Color32.
    */
    private void setUpDictionary()
    {
        //most primary colors except indigo and violet
        string_to_color.Add("red", Color.red);
        string_to_color.Add("orange", new Color32(255, 165, 0, 255));
        string_to_color.Add("yellow", Color.yellow);
        string_to_color.Add("green", Color.green);
        string_to_color.Add("blue", Color.blue);
        string_to_color.Add("purple", new Color32(178, 0, 254, 255));
        string_to_color.Add("brown", new Color32(165, 42, 42, 255));
        string_to_color.Add("black", Color.black);

        //extra colors to add
        blueColors.Add("blue", Color.blue);
        blueColors.Add("cyan", Color.cyan);
        blueColors.Add("aqua", new Color32(0, 201, 254, 1));

        redColors.Add("red", Color.red);
        redColors.Add("maroon", new Color32(128, 0, 0, 255));
        redColors.Add("crimson", new Color32(220, 20, 60, 255));

        greenColors.Add("green", Color.green);
        greenColors.Add("lime", new Color32(0, 255, 0, 255));

        purpleColors.Add("purple", new Color32(178, 0, 254, 255));
        purpleColors.Add("violet", new Color32(238, 130, 238, 255));
        purpleColors.Add("indigo", new Color32(75, 0, 130, 255));

    }

    /*
     * Game is complete. Finalize score and stop updating score. Set speech listener inactive.
    */
    private void EndGame()
    {        
        //TrackScore score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
        float accuracy = trackScore.getCorrect() / (trackScore.getCorrect() + trackScore.getIncorrect());
        trackScore.setAccuracy(accuracy);

        trackScore.gameObject.SetActive(false);
        listener.SetActive(false);

        // GameObject.FindGameObjectWithTag("ScoreKeeper").SetActive(false);
        GameObject.FindGameObjectWithTag("StroopObstacle").SetActive(false);
    }
}
