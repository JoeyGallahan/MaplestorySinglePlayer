using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    GameObject parentObj;

    private void Awake()
    {
        parentObj = GameObject.FindGameObjectWithTag("SkillCanvas");
    }
    // Start is called before the first frame update
    void Start()
    {
        Show(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool maybe)
    {
        parentObj.SetActive(maybe);
    }

    public bool Showing()
    {
        return parentObj.activeInHierarchy;
    }
}
