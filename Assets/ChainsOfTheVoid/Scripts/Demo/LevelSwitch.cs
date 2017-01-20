using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public string NextLevel1 = "";
    public string NextLevel2 = "";
    public string NextLevel3 = "";
    void Update ()
    {
        if(Input.GetKey(KeyCode.Alpha1)) SceneManager.LoadScene(NextLevel1);
        if (Input.GetKey(KeyCode.Alpha2)) SceneManager.LoadScene(NextLevel2);
        if (Input.GetKey(KeyCode.Alpha3)) SceneManager.LoadScene(NextLevel3);
    }
}
