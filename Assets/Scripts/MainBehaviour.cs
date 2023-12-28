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
    private int score;
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
    public GameObject PanelMenu;
    public GameObject BlockPanel;

    [Header("Audio")]
    public AudioSource BackgroundSource;
    public AudioSource AskSource;
    public AudioSource AnswerClickSource;
    public AudioSource TrueAnswerSource;
    public AudioSource FalseAnswerSource;


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
    public Sprite defaultImage;
    public Sprite trueImage;
    public Sprite falseImage;
    public Sprite hoverImage;


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
        PanelMenu.SetActive(false);
        currentQuestionIndex = 1;
        SetQuestions();
        BackgroundSource.Play();
    }

    public void OnClickRestart()
    {
        GameOverPanel.SetActive(false);
        BackgroundSource.Play();
        ResetButtonColor();
        ResetTimer();
        SetQuestions();

        
    }

    private void ResetButtonColor()
    {
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            buttonBackgrounds[i].sprite = defaultImage;
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
        BackgroundSource.Stop();
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

        AskSource.Play();
    }

    public Quiz GetRandomQuestionIndex()
    {
        Quiz result = new Quiz();

        if (currentQuestionIndex <= 4)
        {
            int randomNumber = Random.Range(0, EasyQuestions.Length);
            score = 10;
            result = EasyQuestions[randomNumber];

        }
        else if (currentQuestionIndex > 4 && currentQuestionIndex <= 8)
        {
            int randomNumber = Random.Range(0, NormalQuestions.Length);
            score = 20;
            result = NormalQuestions[randomNumber];
        }
        else
        {
            int randomNumber = Random.Range(0, HardQuestions.Length);
            score = 30;
            result = HardQuestions[randomNumber];
        }

        return result;
    }

    private int currentOption;
    public void OnClickButtonA(int option)
    {
        timerActive = false;
        BlockPanel.SetActive(true);
        currentOption = option;
        buttonBackgrounds[option].sprite = hoverImage;
        StartCoroutine(AnswerCoroutine());
        AnswerClickSource.Play();
    }

    IEnumerator AnswerCoroutine()
    {
        yield return new WaitForSeconds(5);
        BlockPanel.SetActive(false);
        if (currentOption == trueAnswer)
        {
            TrueAnswerSource.Play();
            buttonBackgrounds[currentOption].sprite = trueImage;
            NextQuestionButton.SetActive(true);
        }
        else
        {
            FalseAnswerSource.Play();
            buttonBackgrounds[currentOption].sprite = falseImage;
            GamerOver();
        }
    }

    public void NextQuestionOnClick()
    {
        currentQuestionIndex++;
        SetQuestions();
        ResetTimer();
        ResetButtonColor();
        CurrentScore += score;
        CurretnScoreText.text = CurrentScore.ToString();
        NextQuestionButton.SetActive(false);
    }


    public void SettingsOnClick()
    {
        PanelMenu.SetActive(true);
    }
}
