using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{
    public Button register;

    void Start()
    {
        Button btn = register.GetComponent<Button>();
        btn.onClick.AddListener(OpenRegistration);
    }

    //Method used to take the user to the registration page of the website
    void OpenRegistration()
    {
        Application.OpenURL("https://citcapstones.com/CIT419/php/register.php");
    }
}
