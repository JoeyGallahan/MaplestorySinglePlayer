using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionUI : MonoBehaviour
{
    Button btn;
    TempCharacter character;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        character = GameObject.FindGameObjectWithTag("GameController").GetComponent<TempCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        character.classType = btn.GetComponentInChildren<Text>().text;
    }
}
