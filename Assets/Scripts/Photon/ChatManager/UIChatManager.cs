using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIChatManager : MonoBehaviour, IChatClientListener
{
    // �κ� ���ӽ� Ŀ��Ʈ ����
    public Action ConnectedMyChat;
    public Action ClearText;

    [Header("[ RectView ]")]
    [SerializeField] GridLayoutGroup content = null;
    [Header("[ InputFields ]")]
    [SerializeField] TMP_InputField inputChat= null;

    ChatClient chatClient = null;


    List<GameObject> chatsList = new List<GameObject>();

    private void Awake()
    {
        ConnectedMyChat = ConnectedChat;
        ClearText = delegate
        {
            Debug.Log("ClearText");
            for (int i = 0; i < content.transform.childCount; i++)
                MyChatPool.Instance.Release(content.transform.GetChild(i).gameObject);
        };
    }

    void ConnectedChat()
    {
        // �κ� ���ӽ� ��ȭ���� ��ü ȸ��
        Image[] texts =  content.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < texts.Length; i++)
            MyChatPool.Instance.Release(texts[i].gameObject);

        if (chatClient == null)
        {
            //ChatClient�� IChatClientListener�� �Ѱ��־�� �Ѵ�.
            chatClient = new ChatClient(this);

            // ȣȯ �÷����� ��� PhotonServerSettings���� "��׶��忡�� ����"�� ����ϵ��� �����ؾ� �Ѵ�. 
            //�̷��� �� ���� ������� ���ø����̼��� ��׶���� ��ȯ�Ǹ� ������ ��������.
            chatClient.UseBackgroundWorkerForSending = true;

            // ä�ÿ� �ʿ��� ��ü�� �˻����ش�.
            chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);

            chatClient.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings());
        }

    }
    void Update()
    {
        if (chatClient != null)
        {
            // ���´��� Ȯ���Ѵ�.
            chatClient.Service();
        }
        if (content.transform.childCount > 50)
        {
            MyChatPool.Instance.Release(content.transform.GetChild(0).gameObject);
        }
    }

    // ä��â �Է�
    public void OnEndEdit(string inStr)
    {
        // ä�� �Է��� �ƹ��͵� ������ ����
        if (inStr.Length <= 0) return;

        // ���� ä������ ���� �Է��� ������ ������
        chatClient.PublishMessage("public", inStr);
        inputChat.text = "";
    }

    void addChatLine(string userName, string ChatLine)
    {
        GameObject obj = MyChatPool.Instance.Get(transform.position);
        obj.transform.SetParent(content.transform);
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = userName;
        obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ChatLine;
    }

    //void ClearText()
    //{
    //    for (int i = 0; i < content.transform.childCount; i++)
    //        MyChatPool.Instance.Release(content.transform.GetChild(i).gameObject);
    //}

    #region Chat_CallBacks
    // ä�� �ý����� ��� ������ �α׸� ���� �� �ִ�.
    // DebugLevel �� ���� �� enum Ÿ�Կ� ���� �޼����� ����Ѵ�
    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
        //Debug.Log(message);
    }

    // ���� Ŭ���̾�Ʈ�� ���¸� ���
    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
        addChatLine("[System]", "OnChatStateChange : " + state);
    }

    public void OnConnected()
    {
        addChatLine("[System]", "OnConnected");
        //throw new System.NotImplementedException();
        chatClient.Subscribe("public", 0);
    }

    public void OnDisconnected()
    {
        addChatLine("[System]", "DisConnected");
        //throw new System.NotImplementedException();
    }
    // ������ ���� ä�� �޼����� �޾ƿ��� �Լ�
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //throw new System.NotImplementedException();
        for (int i = 0; i < messages.Length; i++)
            addChatLine(senders[i], messages[i].ToString());
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //throw new System.NotImplementedException();
        addChatLine("[system]", string.Format("OnSubscribed({0})<{1}>", string.Join(",", channels), string.Join(",", results)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
        addChatLine("[system]", string.Format("OnSubscribed({0})", string.Join(",", channels)));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    #endregion
}
