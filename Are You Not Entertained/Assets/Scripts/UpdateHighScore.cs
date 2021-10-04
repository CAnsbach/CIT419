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

    void ScoreUpdate()
    {
        StartCoroutine("UpdateScore");
    }

    IEnumerator UpdateScore()
    {
        feedback.SetText("Updating Score....");
        Debug.Log("Updating Score....");
        usernameString = gc.getUsername();

        WWWForm form = new WWWForm();
        form.AddField("txtUserName", usernameString);
        form.AddField("intScore", gc.GetScore());

        using (UnityWebRequest www = UnityWebRequest.Post(loginurl, form))
        {
            yield return www.SendWebRequest();



            if (www.result != UnityWebRequest.Result.Success)
            {
                feedback.SetText("High score update not successful.");
            }
            else
            {
                string result = www.downloadHandler.text;
                
                feedback.SetText(result);
            }

            gc.updateScore = false;
        }
    }
}
