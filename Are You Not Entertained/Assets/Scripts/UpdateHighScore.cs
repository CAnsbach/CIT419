using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UpdateHighScore : MonoBehaviour
{
    private string loginurl = "https://citcapstones.com/CIT419/php/gameupdatehighscores.php";
    public string usernameString;
    TMP_Text feedback;

    public GameController gc;

    void Start()
    {
        gc = gc = GameObject.Find("GameController").GetComponent<GameController>();
        feedback = gameObject.GetComponent<TMP_Text>();
        ScoreUpdate();
    }

    /// <summary>
    /// Starts the coroutine used to update the user's high score on the website.
    /// </summary>

    void ScoreUpdate()
    {
        StartCoroutine("UpdateScore");
    }

    /// <summary>
    /// Coroutine used to update the user's high score
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateScore()
    {
        //Notify the user that their high score is being updated
        feedback.SetText("Updating Score....");
        
        //User to have thier high score updated.
        usernameString = gc.getUsername();

        //Form to provide the website with the necessary data.
        WWWForm form = new WWWForm();
        form.AddField("txtUserName", usernameString);
        form.AddField("intScore", gc.GetScore());

        using (UnityWebRequest www = UnityWebRequest.Post(loginurl, form))
        {
            yield return www.SendWebRequest();

            //If the connection was not successful, notify the user.
            if (www.result != UnityWebRequest.Result.Success)
            {
                feedback.SetText("High score update not successful.");
            }

            //Else, get the feedback from the website and display it to the user.
            else
            {
                string result = www.downloadHandler.text;
                
                feedback.SetText(result);
            }

            //Score does not need to be updated anymore.
            gc.updateScore = false;
        }
    }
}
