using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE;


    [Header("Panels")]
    public GameObject panelNameTheFlag;
    public GameObject panelFlagTheName;
    public GameObject panelScoreboard;
    public Transform panelStatsSpacer;
    public GameObject panelPlayerStatsPrefab;
    public GameObject panelGetreadyPlayer;
    public TextMeshProUGUI textGetReadyPlayer;

    [Header("Flags")]
    private int countFlagsFromTheTop;
    public List<Flag> allFlags;
    public List<Flag> currentFlags;
    public GameObject flagObjectPrefab;
    public Transform flagSpacerPanel;

    private GameObject currentFlagObject;
    private Flag currentFlag;

    public GameObject panelAnswerSelectionButtons;
    public List<Flag> wrongFlags = new List<Flag>(3);
    public List<GameObject> buttonsAnswersAvailable;

    // Flag the name
    public Button[] flagButtons;
    public TextMeshProUGUI textCurrentCountry;
    private string currentCountry;

    [Header("Stats")]
    public TextMeshProUGUI textPlayer;
    public TextMeshProUGUI textSession;
    public TextMeshProUGUI textTotalScore;
    public TextMeshProUGUI textTimer;

    private float currentAnswerTime;
    private float totalAnswerTime;
    public float defaultAnswerTime = 10;

    public bool isSessionEnded;
    private bool startTimer;
    private int currentsession = 0;
    [SerializeField]
    private int totalSession = 15;
    private int currentScore;
    private int totalScore;
    private int achievementCounter;
    private bool isQuickPlay;
    private bool isFirstRound = true;

    private int playerAmount;
    public List<string> playerNames;
    public List<int> playerflagsGuessed;
    public List<int> playerScores;

    [Header("Misc")]
    private Color defaultButtonColor = Color.white;
    private bool currentFlagsHasBeenCleared;
    private int gameStyleIndex;
    public int currentPlayerIsUp;


    public void DrawRandomCountryName()
    {
        currentFlags = allFlags;
        int randomIndex = Random.Range(0, currentFlags.Count);
        currentCountry = currentFlags[randomIndex].country;
        textCurrentCountry.text = currentCountry;

        //var sprite = currentFlags[randomIndex].flagImage.;
        //GenerateButtons();
    }

    public void GenerateButtons(Texture correctFlag)
    {
        int randomIndex = Random.Range(0, flagButtons.Length);

     //   flagButtons[randomIndex].GetComponent<Image>().sprite. = correctFlag;
        flagButtons[randomIndex].GetComponent<RawImage>().SetNativeSize();
        flagButtons[randomIndex].GetComponent<RawImage>().rectTransform.sizeDelta /= 2;
    }

    private void InstantiateMultipleFlags(int index)
    {
        if (currentFlagObject != null)
        {
            Destroy(currentFlagObject);
        }

        Flag flag = currentFlags[index];
        currentFlag = flag;

        var f = Instantiate(flagObjectPrefab);

        currentFlagObject = f;

        f.GetComponent<UIFlagObject>().flagName.text = flag.country;
        f.GetComponent<UIFlagObject>().flagTexture.texture = flag.flagImage;

        f.GetComponentInChildren<RawImage>().SetNativeSize();
        f.GetComponentInChildren<RawImage>().rectTransform.sizeDelta /= 1.2f;
        var flagSize = f.GetComponentInChildren<RawImage>().rectTransform.sizeDelta;

        f.transform.position = flagSpacerPanel.position;
        f.transform.SetParent(flagSpacerPanel);

        flagSpacerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(flagSize.x + 10f, flagSize.y + 10f);
        currentFlags.Remove(flag);
    }

    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else if (INSTANCE != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentAnswerTime = defaultAnswerTime;
    }

    private void Update()
    {
        if (startTimer && !isSessionEnded)
        {
            Timer();
        }

        //if(Input.GetKey("i"))
        //{
        //    DrawRandomCountryName();
            
        //}
    }

    public void QuickPlay()
    {
        foreach (Transform child in panelStatsSpacer)
        {
            Destroy(child.gameObject);
        }

        NameTheFlag();
        isQuickPlay = true;
        playerNames.Add("Player 1");
        GetReadyPlayer();
    }

    public void CustomPlay()
    {
        foreach (Transform child in panelStatsSpacer)
        {
            Destroy(child.gameObject);
        }
        if (gameStyleIndex == 0)
        {
            NameTheFlag();
        }
        if (gameStyleIndex == 1)
        {
            FlagTheName();
        }

        DrawRandomFlag();
    }

    private void NameTheFlag()
    {
        panelNameTheFlag.SetActive(true);
        panelFlagTheName.SetActive(false);
        FlagPoolAddDefault();

    }
    private void FlagTheName()
    {
        panelFlagTheName.SetActive(true);
        panelNameTheFlag.SetActive(false);
        FlagPoolAddDefault();
    }

    public void FlagPoolAddDefault()
    {
        currentFlags.Clear();

        foreach (var flag in allFlags)
        {
            currentFlags.Add(flag);
        }
    }

    public void FlagPoolAddOrRemoveRegionSpecific(int regionID, bool value)
    {
        if (value)
        {
            foreach (var flag in allFlags)
            {
                if (regionID == (int)flag.region)
                {
                    currentFlags.Add(flag);
                }
            }
        }
        else
        {
            currentFlags.RemoveAll(f => (int)f.region == regionID);
        }
    }

    public void SelectGameStyle(int index)
    {
        gameStyleIndex = index;
    }

    public void DrawFromTheTop()
    {
        if (countFlagsFromTheTop < allFlags.Count)
        {
            InstantiateTheFlagObject(countFlagsFromTheTop);
            countFlagsFromTheTop++;
        }
        else
        {
            Debug.Log("We are at bottom");
        }
    }

    public void DrawRandomFlag()
    {
        if (totalSession == currentsession)
        {
            if (isQuickPlay)
            {
                Debug.Log("Ending quick game");
                EndGame();
            }
            else if (currentPlayerIsUp >= playerNames.Count)
            {
                EndGame();
                Debug.Log("Ending game");
            }
            else
            {
                EndSession();
                GetReadyPlayer();
                Debug.Log("Ending session...Starting a new session");
            }
        }
        else
        {
            StartCoroutine(RandomFlag());
            Debug.Log("Drawing new flag");
        }
    }
    IEnumerator RandomFlag()
    {
        yield return new WaitForSeconds(.5f);
        isSessionEnded = false;
        int randomIndex = Random.Range(0, currentFlags.Count);
        InstantiateTheFlagObject(randomIndex);
        GenerateAnswer(currentFlags);
        currentAnswerTime = defaultAnswerTime;
        startTimer = true;
        currentsession++;
        Debug.Log(currentsession);
        textSession.text = "Session: " + currentsession + " / " + totalSession;
        textTotalScore.text = "Total Score: " + currentScore;

        foreach (var button in buttonsAnswersAvailable)
        {
            button.SetActive(true);
        }
    }

    private void Timer()
    {
        currentAnswerTime -= Time.deltaTime;
        textTimer.text = "Time: " + currentAnswerTime.ToString("0");

        if (currentAnswerTime < 0)
        {
            startTimer = false;
            DrawRandomFlag();
            textSession.text = "Session: " + currentsession + " / " + totalSession;
        }
    }

    private void InstantiateTheFlagObject(int index)
    {
        if (currentFlagObject != null)
        {
            Destroy(currentFlagObject);
        }

        Flag flag = currentFlags[index];
        currentFlag = flag;

        var f = Instantiate(flagObjectPrefab);

        currentFlagObject = f;

        f.GetComponent<UIFlagObject>().flagName.text = flag.country;
        f.GetComponent<UIFlagObject>().flagTexture.texture = flag.flagImage;

        f.GetComponentInChildren<RawImage>().SetNativeSize();
        f.GetComponentInChildren<RawImage>().rectTransform.sizeDelta /= 1.2f;
        var flagSize = f.GetComponentInChildren<RawImage>().rectTransform.sizeDelta;

        f.transform.position = flagSpacerPanel.position;
        f.transform.SetParent(flagSpacerPanel);

        flagSpacerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(flagSize.x + 10f, flagSize.y + 10f);
        currentFlags.Remove(flag);
    }


    public void DrawFourFlags()
    {

    }

    private void EndGame()
    {
        Debug.Log("GAME HAS ENDED");
        EndSession();
        ShowStats();
        currentPlayerIsUp = 0;
        isFirstRound = true;
    }

    private void EndSession()
    {
        playerflagsGuessed.Add(currentScore);
        playerScores.Add(totalScore);
        isSessionEnded = true;

        ClearScore();

        foreach (var button in buttonsAnswersAvailable)
        {
            button.SetActive(false);
        }
    }

    private void ClearScore()
    {
        achievementCounter = 0;
        currentsession = 0;
        currentScore = 0;
        totalScore = 0;
    }

    //private void ShowStats(int flagGuessed,int totalScore)
    //{
    //    panelScoreboard.SetActive(true);

    //    for (int i = 0; i < playerNames.Count; i++)
    //    {
    //        var playerStats = Instantiate(panelPlayerStatsPrefab);

    //        playerStats.GetComponent<PlayerStats>().playername.text = playerNames[i];
    //        playerStats.GetComponent<PlayerStats>().flagSession.text = flagGuessed + " / " + totalSession.ToString();
    //        playerStats.GetComponent<PlayerStats>().totalScore.text = (totalAnswerTime * totalScore).ToString("0");

    //        playerStats.transform.SetParent(panelStatsSpacer);
    //    }
    //}
    private void ShowStats()
    {
        panelScoreboard.SetActive(true);

        for (int i = 0; i < playerNames.Count; i++)
        {
            var playerStats = Instantiate(panelPlayerStatsPrefab);

            playerStats.GetComponent<PlayerStats>().playername.text = playerNames[i];
            playerStats.GetComponent<PlayerStats>().flagSession.text = playerflagsGuessed[i] + " / " + totalSession.ToString();
            playerStats.GetComponent<PlayerStats>().totalScore.text = (totalAnswerTime * playerScores[i]).ToString("0");

            playerStats.transform.SetParent(panelStatsSpacer);
        }
    }

    public void WipeEverything()
    {
        foreach (Transform child in panelStatsSpacer)
        {
            Destroy(child.gameObject);
        }
        foreach (var btn in buttonsAnswersAvailable)
        {
            btn.GetComponent<Button>().enabled = false;
        }
        foreach (Transform child in flagSpacerPanel)
        {
            Destroy(child.gameObject);
        }

        currentFlags.Clear();
        

        playerNames.Clear();
        playerflagsGuessed.Clear();
        playerScores.Clear();
        currentPlayerIsUp = 0;
        isFirstRound = true;
        isQuickPlay = false;
        ClearScore();
    }

    private void GenerateAnswer(List<Flag> flags)
    {
        wrongFlags.Clear();

        while (wrongFlags.Count < 4)
        {
            var randomFlag = allFlags[Random.Range(0, allFlags.Count)];

            if (currentFlag.country != randomFlag.country)
            {
                if (wrongFlags.IndexOf(randomFlag) < 0)
                {

                    wrongFlags.Add(randomFlag);
                }
            }
        }

        foreach (var flag in flags)
        {
            if (flag.country == currentFlag.country)
            {
                currentFlag.country = flag.country;
            }
        }

        var correctButtonIndex = Random.Range(0, 4);

        foreach (var button in buttonsAnswersAvailable)
        {
            button.GetComponent<Button>().enabled = true;
            button.GetComponent<Button>().image.color = defaultButtonColor;
            UIButtonScript buttonScript = button.GetComponent<UIButtonScript>();


            buttonScript.GetComponentInChildren<TextMeshProUGUI>().text = wrongFlags[buttonScript.buttonId].country;

            if (correctButtonIndex == buttonScript.buttonId)
            {
                buttonScript.GetComponentInChildren<TextMeshProUGUI>().text = currentFlag.country;
            }
        }
    }

    public void AnswerFlag(GameObject buttonObj)
    {
        foreach (var btn in buttonsAnswersAvailable)
        {
            btn.GetComponent<Button>().enabled = false;
        }

        var button = buttonObj.GetComponent<Button>();

        if (buttonObj.GetComponentInChildren<TextMeshProUGUI>().text == currentFlag.country)
        {
            Debug.Log("Correct");
            currentScore++;
            totalScore += currentScore;
            totalAnswerTime += currentAnswerTime;
            button.image.color = Color.green;
            achievementCounter++;
            SoundManager.INSTANCE.sfxCorrect.Play();
        }
        else
        {
            button.image.color = Color.red;
            achievementCounter = 0;
            SoundManager.INSTANCE.sfxIncorrect.Play();
            Debug.Log("Incorrect");
        }

        Acheivements();
        DrawRandomFlag();
    }

    private void Acheivements()
    {
        if (achievementCounter == totalSession)
        {
            Debug.Log("No ERROR");
        }
        else if (achievementCounter == 10)
        {
            Debug.Log("10 in a row!");
        }
        else if (achievementCounter == 3)
        {
            Debug.Log("3 in a row!");
        }
    }

    public void ClearCurrentHolder()
    {
        flagSpacerPanel.SetParent(null);
    }

    public void GetReadyPlayer()
    {
        if(isFirstRound)
        {
            panelGetreadyPlayer.SetActive(true);
        }
        else
        {
            StartCoroutine(GetReadyPlayerDelay());
        }

        textGetReadyPlayer.text = playerNames[currentPlayerIsUp];
        currentPlayerIsUp++;
        isFirstRound = false;
    }

    IEnumerator GetReadyPlayerDelay()
    {
        yield return new WaitForSeconds(.5f);
        panelGetreadyPlayer.SetActive(true);
    }
}
