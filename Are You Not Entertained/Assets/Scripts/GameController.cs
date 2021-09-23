using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance 
    { 
        get 
        { 
            return _instance; 
        } 
    }

    private string username;

    public string getUsername()
    {
        return username;
    }

    public void setUsername(string username)
    {
        this.username = username;
    }

    private string password;

    public string getPassword()
    {
        return password;
    }

    public void setPassword(string password)
    {
        this.password = password;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);
    }
}
