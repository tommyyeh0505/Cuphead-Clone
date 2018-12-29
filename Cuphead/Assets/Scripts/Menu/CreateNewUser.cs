using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewUser : MonoBehaviour
{
    [SerializeField] Text usernameInput;

    public void CreateUser()
    {
        int userID = gameObject.GetComponent<User>().CreateNewUserID(usernameInput.text);
        PlayerPrefs.SetInt("USERID", userID);
    }
}
