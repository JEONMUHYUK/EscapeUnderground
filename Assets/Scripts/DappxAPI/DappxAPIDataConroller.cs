using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DappxAPIDataConroller : MonoBehaviour
{
    public static DappxAPIDataConroller Instance;

    [Header("[��ϵ� ������Ʈ���� ȹ�氡���� API Ű]")]
    [Tooltip("�̰��� http://odin-registration-sat.browseosiris.com/# �� ��ϵ� ������Ʈ�� ���ؼ� ȹ���� �� �ִ� API Key �̴�.\nhttps://odin-registration.browseosiris.com/ �� Production URL")]
    [SerializeField] string API_KEY = "3eg6tzd40ytTvxZJrDXSu9";


    [Header("[Betting Backend Base URL")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    #region Don't_Destroy_Awake
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
    #endregion
    #region BaseURL
    /// <summary>
    /// BaseURL
    /// </summary>
    /// <remarks>
    /// ������¡ �ܰ��̱⶧���� "https://odin-api-sat.browseosiris.com" ���.
    /// ���δ��� �ܰ��̸� "https://odin-api.browseosiris.com" ���.
    /// </remarks>
    string GetBaseURL()
    {
        // ���δ��� �ܰ��� ProductionURL�� ���.
        // return FullAppsProductionURL;
        // ���δ��� �ܰ谡 �ƴϱ� ������ Staging URL�� ���.
        return FullAppsStagingURL;
    }
    #endregion

    #region Variables
    // Json ������ ���� ������ ���� Class.
    GetUserProfile getUserProfile = null;
    #endregion

    public void OnClick_StartUserSetting()
    {
        StartCoroutine(ProcessRequestGetUserInfo());
    }

    IEnumerator ProcessRequestGetUserInfo()
    {
        yield return RequestGetUserInfo((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                getUserProfile = response;
                Debug.Log("StatusCode" + getUserProfile.StatusCode);
            }

        });
    }


    IEnumerator RequestGetUserInfo(Action<GetUserProfile> callback)
    {
        // ���� �������� �����´�.
        // UnityWebRequest : �� ������ ����� �����Ѵ�.
        UnityWebRequest www = UnityWebRequest.Get(" http://localhost:8546/api/getuserprofile");
        // SendWebRequest : ���ݼ����� ����� �����ϴ� �Լ��̴�.
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        //FromJson : Json�� Array������ ���� GetUserProfile���� ��ȯ���ش�.
        // downloadHandler ������ API JSON ������ �ٿ�ε� �Ѵ�.
        GetUserProfile getUserProfile = JsonUtility.FromJson<GetUserProfile>(www.downloadHandler.text);
        callback(getUserProfile);
    }
}
