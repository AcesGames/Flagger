using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager INSTANCE;

    public GameObject panelMainBackground;
    public GameObject panelGameMenu;
    public GameObject panelOptions;
    public GameObject panelSelectPlayerAmount;
    public GameObject panelEnterPlayerInfo;
    public GameObject panelGamePlay;
    public GameObject panelRegions;
    public GameObject buttonBackToMain;
    public GameObject buttonRegionAccept;
    public GameObject textTitle;
    public GameObject textVersion;

    public Transform panelPlayerAmountSpacer;
    public TMP_InputField inputFieldPlayerNamePrefab;

    public List<TMP_InputField> playerInputFields;

    public int regionAmount;
    private int playerAmount;
    private bool isGameMenuOpen = true;
    private bool isGameStarted = false;

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

    private void Update()
    {
        if (isGameStarted)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                isGameMenuOpen = !isGameMenuOpen;

                GameMenu(isGameMenuOpen);
            }
        }
    }

    public void GameMenu(bool isOpen)
    {
        panelMainBackground.SetActive(isOpen);

        if(isOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void QuickPlay()
    {
        CloseAll();
        panelMainBackground.SetActive(false);
        isGameStarted = true;
        UIManager.INSTANCE.QuickPlay();
    }

    public void CustomPlay()
    {
        CloseAll();
        panelMainBackground.SetActive(false);
        isGameStarted = true;
        UIManager.INSTANCE.GetReadyPlayer();
    }

    public void SetUpCustomPlay()
    {
        CloseAll();
        panelSelectPlayerAmount.SetActive(true);
        buttonBackToMain.SetActive(true);
    }

    public void Options()
    {
        CloseAll();
        panelOptions.SetActive(true);
        buttonBackToMain.SetActive(true);
    }
    public void QuitGame()
    {
        
        Application.OpenURL("https://acesgames.itch.io/flagger");
    }

    public void PlayerAmount(int _playerAmount)
    {
        CloseAll();
        panelGamePlay.SetActive(true);
        buttonBackToMain.SetActive(true);
        playerAmount = _playerAmount;
        ShowPlayerInfo();
    }
    private void ShowPlayerInfo()
    {
        CloseAll();
        panelEnterPlayerInfo.SetActive(true);

        for (int i = 0; i < playerAmount; i++)
        {
            var inputField = Instantiate(inputFieldPlayerNamePrefab);

            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Player " + (1+i);  
            inputField.transform.SetParent(panelPlayerAmountSpacer);

            playerInputFields.Add(inputField);
        }
    }

    public void ConfirmPlayerNames()
    {
        foreach (var inputField in playerInputFields)
        {
            if(inputField.text == "")
            {
                UIManager.INSTANCE.playerNames.Add(inputField.placeholder.GetComponent<TextMeshProUGUI>().text);
            }
            else
            {
                UIManager.INSTANCE.playerNames.Add(inputField.text);
            }
        }

        ClearPlayers();

        CloseAll();
        panelGamePlay.SetActive(true);
    }

    private void ClearPlayers()
    {
        foreach (Transform child in panelPlayerAmountSpacer)
        {
            playerInputFields.Remove(child.gameObject.GetComponent<TMP_InputField>());
            Destroy(child.gameObject);
        }
    }

    public void Regions()
    {
        panelRegions.SetActive(true);
    }

    public void BackToMain()
    {
        CloseAll();
        panelMainBackground.SetActive(true);
        panelGameMenu.SetActive(true);
        buttonBackToMain.SetActive(false);
        UIManager.INSTANCE.playerNames.Clear();
        ClearPlayers();
        textTitle.gameObject.SetActive(true);
        textVersion.gameObject.SetActive(true);
    }

    public void CloseAll()
    {
        panelGameMenu.SetActive(false);
        panelOptions.SetActive(false);
        panelSelectPlayerAmount.SetActive(false);
        panelGamePlay.SetActive(false);
        panelEnterPlayerInfo.SetActive(false);
        panelRegions.SetActive(false);
        textTitle.SetActive(false);
        textVersion.SetActive(false);
    }

    public void RegionAccept()
    {
        if(regionAmount>1)
        {
            buttonRegionAccept.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonRegionAccept.GetComponent<Button>().interactable = false;
        }
    }
}
