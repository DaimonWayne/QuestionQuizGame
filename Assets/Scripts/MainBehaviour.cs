using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    public TextMeshProUGUI CurrentScoreText;
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
    public GameObject SettingsPanel;
    private int randomNumber;
    public Image TimerBar;
    public Button DoubleAnswer;
    public Button Fifty_Fifty;
    private bool doubleAnswer = false;
    private bool fiftyFifty = false;
    

    [Header("Audio")]
    public AudioSource BackgroundSource;
    public AudioSource AskSource;
    public AudioSource AnswerClickSource;
    public AudioSource TrueAnswerSource;
    public AudioSource FalseAnswerSource;


    [Header("Answer Buttons")]
    public Button ButtonA;
    public Button ButtonB;
    public Button ButtonC;
    public Button ButtonD;
    public GameObject[] AnswerButtons;
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

    [Header("Jokers")]
    public Sprite DoubleAnswerhover;
    public Sprite DoubleAnswerdisapled;
    public Sprite DoubleAnswerSprite;
    public Sprite Fifty_Fiftyhover;
    public Sprite Fifty_Fiftydisapled;
    public Sprite Fifty_FitySprite;


    public Image[] buttonBackgrounds;

    [Header("Sorular")]
    public Quiz[] EasyQuestions;
    public Quiz[] NormalQuestions;
    public Quiz[] HardQuestions;

    [Header("Sorular Listesi")]
    public List<int> EasyQuestionsIndex;
    public List<int> NormalQuestionsIndex;
    public List<int> HardQuestionsIndex;


    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = Timer;
        EasyQuestionsIndex = new List<int>();
        NormalQuestionsIndex = new List<int>();
        HardQuestionsIndex = new List<int>();
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
        currentQuestionIndex = 0;
        SetQuestions();
        BackgroundSource.Play();
        ResetInteractable();
        DoubleAnswer.image.sprite = DoubleAnswerSprite;
        Fifty_Fifty.image.sprite = Fifty_FitySprite;
    }

    public void OnClickRestart()
    {
        GameOverPanel.SetActive(false);
        BackgroundSource.Play();
        ResetButtonColor();
        ResetTimer();
        SetQuestions();
        EasyQuestionsIndex.Clear();
        NormalQuestionsIndex.Clear();
        HardQuestionsIndex.Clear();
        EasyQuestionsIndex.Add(randomNumber);
        CurrentScore = 0;
        CurrentScoreText.text = CurrentScore.ToString();
        DoubleAnswer.image.sprite = DoubleAnswerSprite;
        Fifty_Fifty.image.sprite = Fifty_FitySprite;
        ResetInteractable();
    }

    private void ResetButtonColor()
    {
        for (int i = 0; i < buttonBackgrounds.Length; i++)
        {
            buttonBackgrounds[i].sprite = defaultImage;
            buttonBackgrounds[i].color = Color.white;
        }
    }

    void UpdateTime()
    {
        if (CurrentTime > 0f)
        {
            CurrentTime -= Time.deltaTime;
            TimerBar.fillAmount = CurrentTime / Timer;
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
            randomNumber = Random.Range(0, EasyQuestions.Length);
            score = 10;    
            
            if (!EasyQuestionsIndex.Contains(randomNumber))
            {
                EasyQuestionsIndex.Add(randomNumber);
                result = EasyQuestions[randomNumber];
            }
            else
            {
                while (EasyQuestionsIndex.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, EasyQuestions.Length);
                }
            }
        }
        else if (currentQuestionIndex > 4 && currentQuestionIndex <= 8)
        {
            int randomNumber = Random.Range(0, NormalQuestions.Length);
            score = 20;

            if (!NormalQuestionsIndex.Contains(randomNumber))
            {
                NormalQuestionsIndex.Add(randomNumber);
                result = NormalQuestions[randomNumber];
            }
            else
            {
                while (NormalQuestionsIndex.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, NormalQuestions.Length);
                }
            }
        }
        else
        {
            int randomNumber = Random.Range(0, HardQuestions.Length);
            score = 30;  

            if (!HardQuestionsIndex.Contains(randomNumber))
            {
                HardQuestionsIndex.Add(randomNumber);
                result = HardQuestions[randomNumber];
            }
            else
            {
                while (HardQuestionsIndex.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, HardQuestions.Length);
                }
            }
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
            if (doubleAnswer)
            {
                DoubleAnswer.image.sprite = DoubleAnswerdisapled;
            }
            else if (fiftyFifty)
            {
                Fifty_Fifty.image.sprite = Fifty_Fiftydisapled;
            }
            
        }
        else
        {
            FalseAnswerSource.Play();
            buttonBackgrounds[currentOption].sprite = falseImage;
            GamerOver();
        }
    }

    public void DoubleAnswerOnClick()
    {
        DoubleAnswer.image.sprite = DoubleAnswerhover;
        doubleAnswer = true;
    }

    public void FiftyFityOnClick()
    {
        Fifty_Fifty.image.sprite = Fifty_Fiftyhover;
        fiftyFifty = true;

        ButtonA.interactable = false;
        buttonBackgrounds[0].color = Color.gray;
    }

    void ResetInteractable()
    {
        ButtonA.interactable = true;
        ButtonB.interactable = true;
        ButtonC.interactable = true;
        ButtonD.interactable = true;
    }

    public void NextQuestionOnClick()
    {
        currentQuestionIndex++;
        SetQuestions();
        ResetTimer();
        ResetButtonColor();
        CurrentScore += score;
        CurrentScoreText.text = CurrentScore.ToString();
        NextQuestionButton.SetActive(false);
        ResetInteractable();
    }


    public void SettingsOnClick()
    {
        SettingsPanel.SetActive(true);
    }

    public void SettingsPanelQuitOnClick()
    {
        SettingsPanel.SetActive(false);
    }

    public void MenuOpenOnClick()
    {
        SettingsPanel.SetActive(false);
        PanelMenu.SetActive(true);
        CurrentTime = Timer;
        timerActive = false;
        BackgroundSource.Stop();
        EasyQuestionsIndex.Clear();
        NormalQuestionsIndex.Clear();
        HardQuestionsIndex.Clear();
        ResetButtonColor();
        CurrentScore = 0;
        CurrentScoreText.text = CurrentScore.ToString();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
