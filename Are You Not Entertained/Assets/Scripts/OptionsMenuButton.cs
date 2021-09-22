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

    void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }
}
