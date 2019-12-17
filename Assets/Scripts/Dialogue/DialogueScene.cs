using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueScene
{
    [SerializeField] string sceneTitle;
    [SerializeField] int requiredLevel;
    [SerializeField] TextAsset textFile;
    [SerializeField] List<DialogueLine> dialogueLines = new List<DialogueLine>();


    public string Title
    {
        get => sceneTitle;
        set
        {
            sceneTitle = value;
        }
    }
    public int RequiredLevel
    {
        get => requiredLevel;
    }
    public TextAsset TextFile
    {
        get => textFile;
    }
    public List<DialogueLine> DialogueLines
    {
        get => dialogueLines;
    }
    /*
    private void Awake()
    {
        textReader = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadTextFile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textReader.SplitText(textFile.text, dialogueLines, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
