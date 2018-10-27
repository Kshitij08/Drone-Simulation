using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class backButton : MonoBehaviour {

    // Defined by me Temproraly
    public void OnBackClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
