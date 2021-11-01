using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(OpenOptions);
    }

    /// <summary>
    /// Method used to load the Options Menu Scene
    /// </summary>
    void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }
}
