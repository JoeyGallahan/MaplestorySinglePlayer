using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class CharacterCreationSave
{
    static string savePath = Path.Combine(Application.persistentDataPath, "characterCreation.ms"); //What our save file will be called
    static BinaryFormatter binFormatter = new BinaryFormatter(); //Serializes and deserializes the data for us <3 

    public static void SaveCharacter(TempCharacter character)
    {
        FileStream stream = new FileStream(savePath, FileMode.Create); //Creates a new file for us to throw our character details in

        CharacterData data = new CharacterData(character);

        binFormatter.Serialize(stream, data); //Serializes all our data

        stream.Close();//Close out our stream because we don't need it anymore
    }

    //Loads the necessary data to create the character
    public static CharacterData LoadCreatedCharacter()
    {
        if (File.Exists(savePath)) //If there's actually a save file that exists
        {
            FileStream stream = new FileStream(savePath, FileMode.Open); //Open it up

            CharacterData data = binFormatter.Deserialize(stream) as CharacterData; //Deserialize it as a TempCharacter object

            stream.Close();//Close out the stream

            return data; //return that good good character info
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath); //Ruh-Roh
            return null;
        }
    }

}
