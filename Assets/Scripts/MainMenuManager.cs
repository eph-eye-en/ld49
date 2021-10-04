using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LevelSelect(string LevelName) {
        SceneManager.LoadScene(LevelName);
    }
}
