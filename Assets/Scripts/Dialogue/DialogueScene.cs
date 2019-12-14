using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScene : MonoBehaviour
{
    [SerializeField] TextAsset textFile;
    [SerializeField] List<DialogueLine> dialogueLines = new List<DialogueLine>();

    ReadTextFile textReader;

    private void Awake()
    {
        textReader = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadTextFile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textReader.SplitText(textFile.text, dialogueLines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
