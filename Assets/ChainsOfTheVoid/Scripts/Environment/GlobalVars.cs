using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*
--------------------------------
Author: Tung Nguyen

Purpose: Like the LevelTracker script in which it stores data for the gameobjects to use except
storing it here will save the data to be used on the next loaded scene also. Many scripts will
rely on this script.

Script communicate with: LevelTracker(For spawning the object this script is attached to, not
directly)

Used by: Almost everything

Last edited: Tung Nguyen
--------------------------------
*/

public class GlobalVars : MonoBehaviour
{
    //Misc
    public bool Pause = false;
    public bool Save = false;
    public bool Load = false;

    /*Place graphics settings here*/

    /*Place gameplay settings here*/

    /*Test*/
    public bool DebugMode = false;

    public string MainMenu;

    //void Awake()
    //{
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Confined;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {
        PauseGame();
        SaveGame();
        LoadGame();

        //Reload the scene
        if (Input.GetButtonDown("Reload")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Back to main menu
        if (Input.GetButtonDown("Pause"))
        {
            SceneManager.LoadScene(MainMenu);
        }
    }

    //Pause the game
    void PauseGame()
    {
        if (Pause == true) Time.timeScale = 0;
        else if (Time.timeScale == 0 && Pause == false) Time.timeScale = 1;
    }

    //Saves the game
    void SaveGame()
    {
        if(Save == true)
        {
            Save = false;
        }
    }

    //Loads the game
    void LoadGame()
    {
        if (Load == true)
        {
            Load = false;
        }
    }
}
