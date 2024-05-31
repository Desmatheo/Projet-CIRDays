using UnityEngine;

public class Test : MonoBehaviour
{
    public Selection selection = new Selection();

    void Start(){
        
    }

    public void SauvegardeJson(){
        string data = JsonUtility.ToJson(selection);
        string filepath = Application.persistentDataPath + "/SelectionSave.json";
        System.IO.File.WriteAllText(filepath, data);
    }

    public void SetPlayer1(string name){
        selection.player1 = name;
    }

    public void SetPlayer2(string name){
        selection.player2 = name;
    }
}

[System.Serializable]
public class Selection{
    public string player1;
    public string player2;
}