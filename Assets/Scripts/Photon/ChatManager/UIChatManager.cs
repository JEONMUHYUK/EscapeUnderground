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

    // ����, ä�� �� �޽��� ó���� ���� Photon Chat API�� �߾� Ŭ����.
    // ���������� Service�� ȣ���Ͽ� ���� ������ ����. 
    // Photon Chat ���ø����̼����� ������ AppId�� Connect�� ȣ��. 
    // ����: Connect�� ���� Ŭ���̾�Ʈ�� ���� ���� �޽���. ª�� ��ũ�÷θ� ���� ä�� ������ ����.

    ChatClient chatClient = null;

    private void Awake()
    {
        ConnectedMyChat = ConnectedChat;
        ClearText = delegate
        {
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

            // 
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
        // �޼������� 50���� �Ѿ�� ù��° �޼������� ����.
        if (content.transform.childCount > 15)
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
        obj.transform.localScale = Vector3.one;
    }

    #region Chat_CallBacks

    //
    // ä�� �ý����� ��� ������ �α׸� ���� �� �ִ�.
    // DebugLevel �� ���� �� enum Ÿ�Կ� ���� �޼����� ����Ѵ�
    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
        //Debug.Log(message);
    }

    //
    // ���� Ŭ���̾�Ʈ�� ���¸� ���
    // OnConnected �� OnDisconnected�� �����ϴ� �ݹ��Լ�.
    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
        addChatLine("[System]", "OnChatStateChange : " + state);
    }

    //
    // ������ ����� ȣ��Ǵ� �ݹ� �Լ�.
    // Ŭ���̾�Ʈ�� ���¸� ������ ä���� �����ϰ� �޽����� ������ ���� ����Ǿ� �־� �Ѵ�.
    public void OnConnected()
    {
        addChatLine("[System]", "OnConnected");

        //
        // ���� ä���� �����ϰ� ���������� ä���� �����Ǵ� ��� �� �˷��� ä�� �Ӽ��� ����.
        // 
        // <param name="channel">������ ä�� �̸�</param>
        // <param name="lastMsgId">������ �޽����� �����ϵ��� �籸���� �� �� ä�ο��� ���������� ���ŵ� �޽����� ID, �⺻���� 0.</param>
        // <param name="messagesFromHistory">��Ͽ��� ������ ������ �޽��� ��, �⺻���� -1(��� ������ ���).
        // 0�� �ƹ��͵� ��ȯ���� �ʴ´�. ��� ���� ���� �� �������� ���ѵ˴ϴ�.</param>
        // <param name="creationOptions">������ ä���� ������ ��� ����� �ɼ�.</param>
        // 
        chatClient.Subscribe("public", 0);
    }

    // ���� ���� ������ ȣ��Ǵ� �ݹ� �Լ�.
    public void OnDisconnected()
    {
        addChatLine("[System]", "DisConnected");
    }

    //
    // ������ ���� ä�� �޼����� �޾ƿ��� �Լ�
    //Ŭ���̾�Ʈ�� �������� �� �޽����� �޾����� �ۿ� �˷��ش�.
    // ���� ����� ���� 'messages'�� �޽��� ���� ����.
    //
    // <param name="channelName">�޽����� �� ä��</param>
    // <param name="senders">�޽����� ���� ����� ���</param>
    // <param name="messages">��ü �޽��� ���</param>
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //throw new System.NotImplementedException();
        for (int i = 0; i < messages.Length; i++)
            addChatLine(senders[i], messages[i].ToString());
    }

    // 
    // Ŭ���̾�Ʈ���� ���� �޽����� �˷��ش�.
    // 
    // <param name="sender">�� �޽����� ���� �����</param>
    // <param name="message">�ڽſ��� �޽��� ������</param>
    // <param name="channelName">����� �޽����� channelName(�ڽſ��� ���� �޽����� ��� ����� �̸����� ä�ο� �߰���)</param>
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    //
    // �ٸ� ������� �� ����(ģ�� ��Ͽ� ������ ����ڿ� ���� ������Ʈ�� �޴´�).
    // 
    // <param name="user">������� �̸�.</param>
    // <param name="status">�ش� ������� �� ����.</param>
    // <param name="gotMessage">���¿� ���÷� ĳ���ؾ� �ϴ� �޽����� ���ԵǾ� ������ ��. False: �� ���� ������Ʈ���� �޽����� ���Ե��� �ʴ´�(������ �ִ� �޽����� ����).</param>
    // <param name="message">����ڰ� ������ �޽���.</param>
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    //
    // ����ڰ� ���� ä�� ä���� ���������� ȣ��Ǵ� �ݹ� �Լ�.
    // 
    // <param name="channel">ä�� ä�� �̸�</param>
    // <param name="user">������ ������� UserId</param>
    public void OnSubscribed(string[] channels, bool[] results)
    {
        //throw new System.NotImplementedException();
        addChatLine("[system]", string.Format("OnSubscribed({0})<{1}>", string.Join(",", channels), string.Join(",", results)));
    }

    // 
    // ���� ��Ұ� �Ǿ����� ȣ��Ǵ� �ݹ� �Լ�.
    // ä���� ���� ���� ��ҵ� ��� ä�� �̸��� ��ȯ.
    //
    // Unsubscribe �۾����� ���� ä���� ���۵� ��� OnUnsubscribed�� ���� �� ȣ��ȴ�.
    // �� ȣ���� ���۵� �迭�� �Ϻ� �Ǵ� "channels" �Ű������� ���� ä���� ���.
    // "channels" �Ű������� ȣ�� ���� �� ä�� ������ Unsubscribe �۾��� "channel" �Ű������� ä�� ������ �ٸ� �� �ִ�.
    // <param name="channels">�� �̻� �������� �ʴ� ä�� �̸��� �迭�̴�.</param>

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
        addChatLine("[system]", string.Format("OnSubscribed({0})", string.Join(",", channels)));
    }


    //
    // ����ڰ� ���� ä�� ä���� �������� �� ȣ��Ǵ� �Լ�.
    // </���>
    // <param name="channel">ä�� ä�� �̸�</param>
    // <param name="user">������ ������� UserId</param>
    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }


    //
    // ����ڰ� ���� ä�� ä���� ���� ������� �� ȣ��Ǵ� �ݹ� �Լ�.
    // </���>
    // <param name="channel">ä�� ä�� �̸�</param>
    // <param name="user">������ ����� ������� UserId</param>
    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    #endregion
}
