using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    // loadar nyja senu
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
