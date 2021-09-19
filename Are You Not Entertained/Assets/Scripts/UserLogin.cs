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

    public GameController gc;

    void Start()
    {
        Button btn = login.GetComponent<Button>();
        
        btn.onClick.AddListener(Login);
    }

    void Login()
    {
        StartCoroutine("Upload");
    }

    IEnumerator Upload()
    {
        usernameString = this.username.GetComponent<TMP_InputField>().text;
        passwordString = this.password.GetComponent<TMP_InputField>().text;

        WWWForm form = new WWWForm();
        form.AddField("txtUserName", usernameString);
        form.AddField("txtPassword", passwordString);

        using (UnityWebRequest www = UnityWebRequest.Post(loginurl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                usernameInfo.text = "Login Not Successful";
            }
            else
            {
                string result = www.downloadHandler.text;
                if (result.Equals("login successful for user"))
                {
                    gc.setUsername(usernameString);
                    gc.setPassword(passwordString);

                    SceneManager.LoadScene("MainMenu");
                    
                }

                else
                {
                    usernameInfo.text = result;
                }
            }
        }
    }
}
