using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

// �÷��̾��� ������ ���� Ŭ����
[System.Serializable]
public class PlayerInfo
{
    public string playerName;
}
public class JsonDataController : MonoBehaviour
{

    public static JsonDataController Instance;

    [SerializeField] TMP_InputField nickNameInputField = null;
    [SerializeField] string playerName = null;

    public string PlayerName { get { return playerName; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EndEditSetPlayerName(string text)
    { 
        // �÷��̾� �̸��� ������ �޼���
        playerName = text;
        SavePlayerInfo();  
    }

    // --------------------------------------------------JSON----------------------------------------


    public void SavePlayerInfo()
    {
        PlayerInfo playerData = new PlayerInfo();
        playerData.playerName = playerName;

        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/savenamefile.json", json);
    }

    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savenamefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerInfo playerData = JsonUtility.FromJson<PlayerInfo>(json);

            playerName = playerData.playerName;
        }
    }

}
