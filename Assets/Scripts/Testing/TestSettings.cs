using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSettings : MonoBehaviour
{
    [SerializeField] int fpsLimit = -1;
    [SerializeField] int oldFPSLimit = -1;
    /*
    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }*/
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fpsLimit;
        Screen.SetResolution(1920, 1080, true);
    }

    private void Update()
    {
        if (fpsLimit != oldFPSLimit)
        {
            oldFPSLimit = fpsLimit;
            Application.targetFrameRate = fpsLimit;
        }
    }

}
