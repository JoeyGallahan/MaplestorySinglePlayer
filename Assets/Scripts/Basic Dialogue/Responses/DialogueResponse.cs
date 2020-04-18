using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public enum ResponseType
    {
        CONTINUE,
        END,
        ACCEPT
    }

    [SerializeField] ResponseType responseType;
    [SerializeField] string responseText;

    public DialogueResponse(ResponseType type)
    {
        responseType = type;
        switch(responseType)
        {
            case ResponseType.CONTINUE: responseText = "Next";
                break;
            case ResponseType.END: responseText = "OK";
                break;
            case ResponseType.ACCEPT: responseText = "ACCEPT";
                break;
        }
    }

    public ResponseType TypeOfResponse
    {
        get => responseType;
    }

    public string ResponseText
    {
        get => responseText;
    }
}
