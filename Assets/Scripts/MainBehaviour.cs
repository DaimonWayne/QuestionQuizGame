using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quiz
{
    public string Question;
    [Space(10)]
    public string OptionA;
    public string OptionB;
    public string OptionC;
    public string OptionD;
    [Space(10)]
    public int Answer;
}

public class MainBehaviour : MonoBehaviour
{
    [Range(0f, 45f)] 
    public float Timer;
    public float CurrentTime;
    private int currentQuestionIndex;
    public int trueAnswer;
    public int CurrentScore;
    public TextMeshProUGUI CurretnScoreText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI FinalButton;
    public TMP_InputField QuestionsText;
    bool timerActive = false;
    public GameObject StartButton;
    public GameObject GameOverPanel;
    public GameObject RestartButton;
    public GameObject NextQuestionButton;
    public GameObject FinishPanel;


    [Header("Answer Buttons")]
    public GameObject ButtonA;
    public GameObject ButtonB;
    public GameObject ButtonC;
    public GameObject ButtonD;
    [Header("Answer Text")]
    public TextMeshProUGUI AnswerButtonA;
    public TextMeshProUGUI AnswerButtonB;
    public TextMeshProUGUI AnswerButtonC;
    public TextMeshProUGUI AnswerButtonD;


    [Header("Colors")]
    public Color defaultColor;
    public Color trueColor;
    public Color falseColor;


    public Image[] buttonBackgrounds;

    [Header("Sorular")]
    public Quiz[] EasyQuestions;
    public Quiz[] NormalQuestions;
    public Quiz[] HardQuestions;


    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = Timer;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive) UpdateTime();
    }

    public void OnClickStart()
    {
        timerActive = true;
        StartButton.SetActive(false);

        currentQuestionIndex = 1;
        SetQuestions();
    }

    public void OnClickRestart()
    {
        GameOverPanel.SetActive(false);

        ResetButtonColor();
        ResetTimer();
        SetQuestions();

        
    }

    private void ResetButtonColor()
    {
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            buttonBackgrounds[i].color = defaultColor;
        }
    }

    void UpdateTime()
    {
        if (CurrentTime > 0f)
        {
            CurrentTime -= Time.deltaTime;
            TimeText.text = CurrentTime.ToString("F0");
        }

        if (CurrentTime <= 0f)
        {
            GamerOver();
        }
    }

    private void ResetTimer()
    {
        CurrentTime = Timer;
        timerActive = true;

    }

    private void GamerOver()
    {
        GameOverPanel.SetActive(true);
        timerActive = false;
        currentQuestionIndex = 1;
    }


    private void SetQuestions()
    {

        Quiz quiz = GetRandomQuestionIndex();

        QuestionsText.text = quiz.Question;
        AnswerButtonA.text = quiz.OptionA;
        AnswerButtonB.text = quiz.OptionB;
        AnswerButtonC.text = quiz.OptionC;
        AnswerButtonD.text = quiz.OptionD;
        trueAnswer = quiz.Answer;
    }

    public Quiz GetRandomQuestionIndex()
    {
        Quiz result = new Quiz();

        if (currentQuestionIndex <= 3)
        {
            int randomNumber = Random.Range(0, EasyQuestions.Length);

            result = EasyQuestions[randomNumber];

        }
        else if (currentQuestionIndex >3 && currentQuestionIndex <= 8)
        {
            int randomNumber = Random.Range(0, NormalQuestions.Length);

            result = NormalQuestions[randomNumber];
        }
        else
        {
            int randomNumber = Random.Range(0, HardQuestions.Length);

            result = HardQuestions[randomNumber];
        }

        return result;
    }

    public void OnClickButtonA(int option)
    {
        if (!timerActive)
            return;

        timerActive = false;

        if (option == trueAnswer)
        {

            buttonBackgrounds[option].color = trueColor;
            NextQuestionButton.SetActive(true);
        }
        else
        {
            buttonBackgrounds[option].color = falseColor;
            GamerOver();
        }
    }

    public void NextQuestionOnClick()
    {
        currentQuestionIndex++;
        SetQuestions();
        ResetTimer();
        ResetButtonColor();
        CurrentScore += 20;
        CurretnScoreText.text = CurrentScore.ToString();
        NextQuestionButton.SetActive(false);
    }
}
