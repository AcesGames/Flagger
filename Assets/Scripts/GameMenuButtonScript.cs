using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuButtonScript : MonoBehaviour
{
    public int buttonValue;
    public Color defaultButtonColor;
    private bool regionValue, colorValue;

    public void QuickPlay()
    {
        GameMenuManager.INSTANCE.QuickPlay();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void CustomPlay()
    {
        GameMenuManager.INSTANCE.CustomPlay();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void SetUpCustomPlay()
    {
        GameMenuManager.INSTANCE.SetUpCustomPlay();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void Options()
    {
        GameMenuManager.INSTANCE.Options();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void QuitGame()
    {
        GameMenuManager.INSTANCE.QuitGame();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void FlagPoolDefault()
    {
        UIManager.INSTANCE.FlagPoolAddDefault();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void FlagPoolCustom()
    {
        GameMenuManager.INSTANCE.Regions();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void PlayerAmount()
    {
        GameMenuManager.INSTANCE.PlayerAmount(buttonValue);
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void ConfirmPlayers()
    {
        GameMenuManager.INSTANCE.ConfirmPlayerNames();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void SelectGameType()
    {
        UIManager.INSTANCE.SelectGameStyle(buttonValue);
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void BackToMain()
    {
        GameMenuManager.INSTANCE.BackToMain();
        SoundManager.INSTANCE.PlayButtonClick();
    }

    public void ChangeColor()
    {
        colorValue = !colorValue;

        if (colorValue)
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            GetComponent<Image>().color = defaultButtonColor;
        }
    }

    public void SetGreenColor()
    {
        GetComponent<Image>().color = Color.green;
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void SetDefaultColor()
    {
        GetComponent<Image>().color = defaultButtonColor;
    }

    public void Region()
    {
        regionValue = !regionValue; // Is it adding or removing region flags to/from the pool

        UIManager.INSTANCE.FlagPoolAddOrRemoveRegionSpecific(buttonValue, regionValue);

        if (regionValue)
        {
            GameMenuManager.INSTANCE.regionAmount++;
        }
        else
        {
            GameMenuManager.INSTANCE.regionAmount--;
        }

        GameMenuManager.INSTANCE.RegionAccept();
    }
}
