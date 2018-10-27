using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInSpeed = 0.33f;

    public RectTransform menuContainer;
    public Transform levelPanel;

    public Transform colorPanel;
    public Transform trailPanel;

    public Text colorBuySetText;
    public Text trailBuySetText;
    public Text goldText;

    private int[] colorCost = new int[] { 0, 5, 5, 5, 10, 10, 10, 15, 15, 20 };
    private int[] trailCost = new int[] { 0, 20, 20, 40, 40, 60, 60, 80, 80, 100 };
    private int selectedColorIndex;
    private int selectedTrailIndex;
    private int activeColorIndex;
    private int activeTrailIndex;


    private Vector3 desiredMenuPosition;

    private void Start()
    {
        // $$ TEMPORARY
        SaveManager.Instance.state.gold = 999;

        // Tell our gold text how much he should displaying
        UpdateGoldText();

        //Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();

        //Start with a white screen
        fadeGroup.alpha = 1;

        //Add button on-click events to the shop buttons
        InitShop();

        // Add buttons on-click events to levels
        InitLevel();

        // Set player's prefrences ( color & trail )
        OnColorSelect(SaveManager.Instance.state.activeColor);
        SetColor(SaveManager.Instance.state.activeColor);

        OnTrailSelect(SaveManager.Instance.state.activeTrail);
        SetTrail(SaveManager.Instance.state.activeTrail);

        // Make the buttons bigger for the selected items
        colorPanel.GetChild(SaveManager.Instance.state.activeColor).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;
        trailPanel.GetChild(SaveManager.Instance.state.activeTrail).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

    }

    private void Update()
    {
        //Fade in 
        fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;

        //Menu navigation (smooth)
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);
    }

    private void InitShop()
    {
        //Just make sure we've assigned the refrences
        if (colorPanel == null || trailPanel == null)
            Debug.Log("You did not assign the colr/trail panel in the inspector");

        // For every children transform under our color panel, find the button and add onclick
        int i = 0;
        foreach(Transform t in colorPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnColorSelect(currentIndex));

            //Set color of the image, based on if owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsColorOwned(i) ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }

        //Reset index;
        i = 0;
        //Do the same for the trail panel
        foreach (Transform t in trailPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrailSelect(currentIndex));

            //Set color of the trail, based on if owned or not
            Image img = t.GetComponent<Image>();
            img.color = SaveManager.Instance.IsTrailOwned(i) ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            i++;
        }
    }

    private void InitLevel()
    {
        //Just make sure we've assigned the refrences
        if (levelPanel == null)
            Debug.Log("You did not assign the level panel in the inspector");

        // For every children transform under our level panel, find the button and add onclick
        int i = 0;
        foreach (Transform t in levelPanel)
        {
            int currentIndex = i;

            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnLevelSelect(currentIndex));

            i++;
        }
    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            // 0 && default case = Main Menu
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                break;
            // 1 = Play Menu
            case 1:
                desiredMenuPosition = Vector3.right * 1280;
                break;
            // 2 = Shop Menu
            case 2:
                desiredMenuPosition = Vector3.left * 1280;
                break;
        }
    }

    private void SetColor(int index)
    {
        // Set the active index
        activeColorIndex = index;
        SaveManager.Instance.state.activeColor = index;

        // Change the color on the player model

        // Change buy/set button text
        colorBuySetText.text = "Current";

        // Remember preferences
        SaveManager.Instance.Save();
    }

    private void SetTrail(int index)
    {
        // Set the active index
        activeTrailIndex = index;
        SaveManager.Instance.state.activeTrail = index;

        // Change the trail on the player model

        // Change buy/set button text
        trailBuySetText.text = "Current";

        // Remember preferences
        SaveManager.Instance.Save();
    }

    private void UpdateGoldText()
    {
        goldText.text = SaveManager.Instance.state.gold.ToString();
    }

    //Buttons
    public void OnPlayClick()
    {
        NavigateTo(1);
        Debug.Log("Play button has been clicked!");
    }

    public void OnShopClick()
    {
        NavigateTo(2);
        Debug.Log("Shop button has been clicked!");
    }

    public void OnBackClick()
    {
        NavigateTo(0);
        Debug.Log("Back button has been clicked!");
    }

    // Defined by me Temproraly
    public void OnLevelClickMulti()
    {
        SceneManager.LoadScene("Multi");
    }

    // Defined by me Temproraly
    public void OnLevelClickMain()
    {
        SceneManager.LoadScene("Main");
    }

    // Defined by me Temproraly
    public void OnLevelClickMars()
    {
        SceneManager.LoadScene("Mars");
    }

    // Defined by me Temproraly
    public void OnLevelClickMoon()
    {
        SceneManager.LoadScene("Moon");
    }

    // Defined by me Temproraly
    public void OnLevelClickSpace()
    {
        SceneManager.LoadScene("Space station");
    }

    // Defined by me Temproraly
    public void OnLevelClickMilitary()
    {
        SceneManager.LoadScene("Military");
    }

    // Defined by me Temproraly
    public void OnLevelClickAirport()
    {
        SceneManager.LoadScene("Airport");
    }

    // Defined by me Temproraly
    public void OnLevelClickApocalypse()
    {
        SceneManager.LoadScene("Apocalypse");
    }

    // Defined by me Temproraly
    public void OnLevelClickFantasy()
    {
        SceneManager.LoadScene("Fantasy");
    }

    // Defined by me Temproraly
    public void OnLevelClickFarm()
    {
        SceneManager.LoadScene("Farm");
    }

    // Defined by me Temproraly
    public void OnLevelClickPort()
    {
        SceneManager.LoadScene("Port");
    }

    // Defined by me Temproraly
    public void OnLevelClickTemple()
    {
        SceneManager.LoadScene("Temple");
    }

    // Defined by me Temproraly
    public void OnLevelClickTown()
    {
        SceneManager.LoadScene("Town");
    }

    // Defined by me Temproraly
    public void OnLevelClickTrain()
    {
        SceneManager.LoadScene("Train");
    }

    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selecting Color button: " + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedColorIndex == currentIndex)
            return;

        // Make the icon slightly bigget
        colorPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

        // Put the previous one on the normal scale
        colorPanel.GetChild(selectedColorIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //Set the selected Color
        selectedColorIndex = currentIndex;

        // Change the content of the buy/set button, depending on the state of the color
        if(SaveManager.Instance.IsColorOwned(currentIndex))
        {
            // Color is owned
            // Is it already our current color?
            if(activeColorIndex == currentIndex)
            {
                colorBuySetText.text = "Current";
            }
            else
            {
                colorBuySetText.text = "Select";
            } 
        }
        else
        {
            // Color isn't owned
            colorBuySetText.text = "Buy " + colorCost[currentIndex].ToString();
        }
    }

    private void OnTrailSelect(int currentIndex)
    {
        Debug.Log("Selecting Trail button: " + currentIndex);

        // if the button clicked is already selected, exit
        if (selectedTrailIndex == currentIndex)
            return;

        // Make the icon slightly bigget
        trailPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.125f;

        // Put the previous one on the normal scale
        trailPanel.GetChild(selectedTrailIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        //Set the selected Trail
        selectedTrailIndex = currentIndex;

        // Change the content of the buy/set button, depending on the state of the trail
        if (SaveManager.Instance.IsTrailOwned(currentIndex))
        {
            // Trail is owned
            // Is it already our current trail?
            if (activeTrailIndex == currentIndex)
            {
                trailBuySetText.text = "Current";
            }
            else
            {
                trailBuySetText.text = "Select";
            }
        }
        else
        {
            // Trail isn't owned
            trailBuySetText.text = "Buy " + trailCost[currentIndex].ToString();
        }
    }

    private void OnLevelSelect(int currentIndex)
    {
        Debug.Log("Selecting level : " + currentIndex);
    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy/Set color");

        // Is the selected color owned
        if (SaveManager.Instance.IsColorOwned(selectedColorIndex))
        {
            // Set the color!
            SetColor(selectedColorIndex);
        }
        else
        {
            // Attempt to buy the color
            if (SaveManager.Instance.BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                // Success!
                SetColor(selectedColorIndex);

                // Change the color of the button
                colorPanel.GetChild(selectedColorIndex).GetComponent<Image>().color = Color.white;

                // Update the gold text
                UpdateGoldText();
            }
            else
            {
                // Do not have enough gold!
                // Play sound feedback
                Debug.Log("Not enough gold");
            }
        }
    }

    public void OnTrailBuySet()
    {
        Debug.Log("Buy/Set trail");

        // Is the selected trail owned
        if (SaveManager.Instance.IsTrailOwned(selectedTrailIndex))
        {
            // Set the trail!
            SetTrail(selectedTrailIndex);
        }
        else
        {
            // Attempt to buy the trail
            if (SaveManager.Instance.BuyTrail(selectedTrailIndex, trailCost[selectedTrailIndex]))
            {
                // Success!
                SetTrail(selectedTrailIndex);

                // Change the color of the button
                trailPanel.GetChild(selectedTrailIndex).GetComponent<Image>().color = Color.white;

                // Update the gold text
                UpdateGoldText();
            }
            else
            {
                // Do not have enough gold!
                // Play sound feedback
                Debug.Log("Not enough gold");
            }
        }
    }
}
