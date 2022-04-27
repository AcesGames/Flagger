using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManagerBackup : MonoBehaviour
{
    public static UIManagerBackup INSTANCE;

    [Header("Flags")]
    private int countFlagsFromTheTop;
    public List<Flag> allFlags;

    public List<Flag> currentFlags;
    public GameObject flagObjectPrefab;
    public Transform flagSpacerPanel;

    private GameObject currentFlagObject;
    private Flag currentFlag;

    public GameObject panelAnswerSelectionButtons;
    public List<string> wrongCountries = new List<string>();
    public List<GameObject> buttonsAnswersAvailable;

    [Header("Stats")]
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textSession;
    public TextMeshProUGUI textDifficulty;
    public TextMeshProUGUI textTotalScore;

    public Difficulty difficulty;
    public enum Difficulty
    {
        EASY, NORMAL, HARD
    }

    private int currentsession = 0;
    private int totalSession = 15;
    public int totalScore;

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
        difficulty = Difficulty.HARD;
        SelectDifficulty();
    }

    private void Update()
    {
        //if (Input.GetKeyDown("k"))
        //{
        //    GenerateAnswer();
        //}
    }

    private void SelectDifficulty()
    {
        if (difficulty == Difficulty.HARD)
        {
            foreach (var flag in allFlags)
            {
                currentFlags.Add(flag);
            }
        }
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
        int randomIndex = Random.Range(0, currentFlags.Count);
        InstantiateTheFlagObject(randomIndex);
        GenerateAnswer(currentFlags);
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

        f.transform.position = flagSpacerPanel.position;
        f.transform.SetParent(flagSpacerPanel);

        currentFlags.Remove(flag);
    }


    public void DrawFourFlags()
    {

    }

    public void HardDifficulty()    // All flags are in the pool
    {

    }

    public void EasyDifficulty()  // The biggest countries
    {
    }

    public void RegionSpecific(int regionID)
    {
        Debug.Log(regionID);
        foreach (var flag in allFlags)
        {
            if ((int)flag.region == regionID)
            {
                currentFlags.Add(flag);
            }
        }
    }

    private void FindSpecificRegion(string region)
    {

    }

    private void GenerateAnswer(List<Flag> flags)
    {
        wrongCountries.Clear();

        foreach (var flag in flags)
        {
            if (flag.country == currentFlag.country)
            {
                currentFlag.country = flag.country;
            }
        }

        for (int i = 0; i < 4; i++)
        {

            if (currentFlag.country != allFlags[i].country)
            {
                var randomIndex = Random.Range(0, allFlags.Count);

                wrongCountries.Add(allFlags[randomIndex].country);
            }
        }

        var correctButtonIndex = Random.Range(0, 4);

        foreach (var button in buttonsAnswersAvailable)
        {


            UIButtonScript buttonScript = button.GetComponent<UIButtonScript>();

            if (correctButtonIndex != buttonScript.buttonId)
            {
                buttonScript.GetComponentInChildren<TextMeshProUGUI>().text = wrongCountries[buttonScript.buttonId];
            }
            else
            {
                buttonScript.GetComponentInChildren<TextMeshProUGUI>().text = currentFlag.country;

            }
        }
    }

    public void AnswerFlag(string buttonCountryName)
    {
        if (buttonCountryName == currentFlag.country)
        {
            Debug.Log("Correct");
            totalScore++;
        }
        else
        {
            Debug.Log("Incorrect");
        }

        currentsession++;
        textSession.text = "Session: " + currentsession + " / " + totalSession;
        textTotalScore.text = "Total Score: " + totalScore;

        DrawRandomFlag();
    }

    public void ClearCurrentHolder()
    {
        flagSpacerPanel.SetParent(null);
    }
}
