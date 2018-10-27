using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

        Debug.Log(Helper.Serialize<SaveState>(state));
    }

    // Save the whole state of this saveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    // Load the previous saved state from the player prefs
    public void Load()
    {
        // Do we already have a save?
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("No save file found, creating a new one!");
        }
    }

    // Check if the color is owned
    public bool IsColorOwned(int index)
    {
        // Check if the bit is set, if so the color is owned
        return (state.colorOwned & (1 << index)) != 0;
    }

    // Check if the trail is owned
    public bool IsTrailOwned(int index)
    {
        // Check if the bit is set, if so the color is owned
        return (state.trailOwned & (1 << index)) != 0;
    }

    // Attempt buy a color, return true/false
    public bool BuyColor(int index, int cost)
    {
        if(state.gold >= cost)
        {
            // Enough money, remove from the current gold stack
            state.gold -= cost;
            UnlockColor(index);

            // Save progress
            Save();

            return true;
        }
        else
        {
            // Not enough money, return false
            return false;
        }
    }

    // Attempt buy a trail, return true/false
    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            // Enough money, remove from the current gold stack
            state.gold -= cost;
            UnlockTrail(index);

            // Save progress
            Save();

            return true;
        }
        else
        {
            // Not enough money, return false
            return false;
        }
    }

    // Unlock a color in the "colorOwned" int
    public void UnlockColor(int index)
    {
        // Toggle on the bit at index
        state.colorOwned |= 1 << index;
    }

    // Unlock a trail in the "trailOwned" int
    public void UnlockTrail(int index)
    {
        // Toggle on the bit at index
        state.trailOwned |= 1 << index;
    }

    // Reset the whole save file
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
