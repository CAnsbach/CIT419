using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    public Button login;
    private string loginurl = "https://citcapstones.com/CIT419/php/gameuserlogin.php";
    public GameObject username, password;
    private string usernameString, passwordString;
    public Text usernameInfo;

    GameController gc;

    void Start()
    {
        //Get the GameController object
        gc = gc = GameObject.Find("GameController").GetComponent<GameController>();
        Button btn = login.GetComponent<Button>();
        
        btn.onClick.AddListener(Login);
    }

    /// <summary>
    /// Method used to start the Login coroutine
    /// </summary>
    void Login()
    {
        StartCoroutine("Upload");
    }

    /// <summary>
    /// Coroutine used to log the user in and get feedback from the website.
    /// </summary>
    /// <returns></returns>
    IEnumerator Upload()
    {
        //String to hold the user's username and password to send to the website
        usernameString = this.username.GetComponent<TMP_InputField>().text;
        passwordString = this.password.GetComponent<TMP_InputField>().text;

        //Form used to provide the website with data
        WWWForm form = new WWWForm();
        form.AddField("txtUserName", usernameString);
        form.AddField("txtPassword", passwordString);

        //Post the data to the website and wait for a response
        using (UnityWebRequest www = UnityWebRequest.Post(loginurl, form))
        {
            yield return www.SendWebRequest();

            //If the result of the connection was not successful, notify the user.
            if (www.result != UnityWebRequest.Result.Success)
            {
                usernameInfo.text = "Login Not Successful";
            }
            
            //Else, Determine if the user was successfully logged in.
            else
            {
                //Get the result
                string result = www.downloadHandler.text;
                
                //If the user was successfully logged in, keep the username and go to the main menu.
                if (result.Equals("login successful for user"))
                {
                    gc.setUsername(usernameString);

                    SceneManager.LoadScene("MainMenu");
                }

                //Else, notify the user of an unsuccessful login.
                else
                {
                    usernameInfo.text = result;
                }
            }
        }
    }
}
