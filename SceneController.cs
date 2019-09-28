using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    //public string setting;
    public GameObject listener;
    public int numCardsDifficulty;
   
    public string randomColor;
    private int randomNumber;

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Sprite[] labels;

    Dictionary<string, Color32> string_to_color;

    public GameObject canvas;

    public enum State
    {
        Wait,
        Next,
        Complete
    }

    public State currentState;

    int[] numbers;
    int[] labels_num;
    string[] answer_choices;

    public int currentProb;

    private void Awake()
    {
        currentState = State.Wait;
        currentProb = 1;

        Instantiate(canvas);

        string_to_color = new Dictionary<string, Color32>();
        setUpDictionary();

        setUpCurrentCard();

        assignAnswerChoices();
        
    }

    private void Start()
    {
        listener.SetActive(true);
    }

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

    private void setUpCurrentCard()
    {
        numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        numbers = ShuffleArray(numbers);

        randomNumber = Random.Range(0, numbers.Length);
        randomColor = labels[randomNumber].name; //changed from images to labels    

        labels_num = ShuffleArray(numbers);
        int label = labels_num[Random.Range(0, labels_num.Length)];

        //originalCard.ChangeSprite(currentProb, label, images[0]);
        //originalCard.Card_Label.GetComponent<SpriteRenderer>().sprite = labels[randomNumber];
        Text colorText = GameObject.FindGameObjectWithTag("Color").GetComponent<Text>();
        colorText.text = labels[Random.Range(0, numbers.Length)].name.Split('_')[0];
        colorText.color = string_to_color[randomColor.Split('_')[0]];
        
        //originalCard.setUp();
    }

    private void assignAnswerChoices()
    {
        answer_choices = new string[4];
        answer_choices[0] = randomColor.Split('_')[0];
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

    private void setUpDictionary()
    {
        string_to_color.Add("red", Color.red);
        string_to_color.Add("orange", new Color32(255, 165, 0, 255));
        string_to_color.Add("yellow", Color.yellow);
        string_to_color.Add("green", Color.green);
        string_to_color.Add("blue", Color.blue);
        string_to_color.Add("purple", new Color32(178, 0, 254, 255));
        string_to_color.Add("brown", new Color32(165, 42, 42, 255));
        string_to_color.Add("black", Color.black);
    }

    private void EndGame()
    {
        
        TrackScore score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<TrackScore>();
        score.accuracy = score.correct / (score.correct + score.incorrect);
        GameObject.FindGameObjectWithTag("ScoreKeeper").SetActive(false);
        GameObject.FindGameObjectWithTag("StroopObstacle").SetActive(false);
        listener.SetActive(false);
    }

    
    /*
    private void Update()
    {        
        if (currentState == State.Shuffled)
        {
            GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
            foreach (GameObject card in cards)
            {
                MainCard script = card.GetComponent<MainCard>();
                if (script.selected && script._color == randomColor)
                {
                    Debug.Log("Correct Selection");
                    instruction.text = "Correct!";
                    currentState = State.Wait;
                }
            }
        }
    }*/
}
