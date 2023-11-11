using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
#endif
#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleManager : SingletonMono<GoogleManager>
{
    [SerializeField]
    private string editorLoginId;

    [SerializeField]
    private string testId = "a_8846847867697156085";

    [SerializeField]
    private UiNickNameInputBoard nickNameInputBoard;

    private bool isSignIn = false;

    public ObscuredString loginId;

    public static string email { get; private set; } = "Editor";

    private static string savedId_key;
    private static string savedPass_Key;

    public enum IOS_LoginType
    {
        GameCenter,
        Custom
    }

    private string iOS_LoginType = string.Empty;


    private new void Awake()
    {
        base.Awake();
        Backend.Initialize(HandleBackendCallBack, true);

        GleyNotifications.Initialize();
    }

    private void HandleBackendCallBack()
    {
        if (Backend.IsInitialized)
        {
#if UNITY_EDITOR
            WhenVersionCheckSuccess();
            return;
#endif
            CheckCurrentVersion();
        }
        else
        {
            Debug.LogError("Failed to initialize the backend");
        }
    }

    private void CheckCurrentVersion()
    {
        var servercheckBro = Backend.Utils.GetServerStatus();

        if (servercheckBro.IsSuccess())
        {
            var json = servercheckBro.GetReturnValuetoJSON();
            var state = json["serverStatus"].ToString();

            //온라인
            if (state == "0")
            {
                int clientVersion = int.Parse(Application.version);

                var bro = Backend.Utils.GetLatestVersion();

                if (bro.IsSuccess())
                {
                    var jsonData = bro.GetReturnValuetoJSON();
                    string serverVersion = jsonData["version"].ToString();

                    //버전이 높거나 같음
                    if (clientVersion >= int.Parse(serverVersion))
                    {
                        Debug.LogError($"클라이언트 버전 {clientVersion} 서버 버전 {serverVersion} 같음");
                        WhenVersionCheckSuccess();
                    }
                    else
                    {
                        Debug.LogError($"클라이언트 버전 {clientVersion} 서버 버전 {serverVersion} 다름");

                        PopupManager.Instance.ShowVersionUpPopup(CommonString.Notice, "업데이트 버전이 있습니다. 스토어로 이동합니다.", () =>
                        {
#if UNITY_ANDROID
                            Application.OpenURL("https://play.google.com/store/apps/details?id=com.DragonGames.yoyo");
#endif
#if UNITY_IOS
                    Application.OpenURL("itms-apps://itunes.apple.com/app/id1587651736");
#endif
                        }, false);
                    }

                    Debug.LogError($"clientVersion = {clientVersion} serverVersion {serverVersion}");
                }
                else
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "클라이언트 버전 정보 로드에 실패했습니다.\n버전 정보를 다시 요청합니다.", CheckCurrentVersion);
                }
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "서버 점검중입니다. 자세한 사항은\n네이버 카페에서 확인 부탁드립니다!", () => { Application.OpenURL("https://cafe.naver.com/yokiki"); });
            }
        }
    }

    private void WhenVersionCheckSuccess()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 요청
            .RequestIdToken() // 토큰 요청
            .Build();

        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        iOS_LoginType = PlayerPrefs.GetString(CommonString.IOS_loginType, string.Empty);

        //GPGS 시작.
        PlayGamesPlatform.Activate();
#endif

#if UNITY_EDITOR
        LoginByCustumId();
#elif UNITY_ANDROID
            GoogleAuth();
#elif UNITY_IOS
            LoginBySavedData();
#endif
    }

    public void GameCenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    loginId = Social.localUser.id;

                    PlayerPrefs.SetString(CommonString.SavedLoginTypeKey, loginId);

                    PlayerPrefs.SetString(CommonString.IOS_loginType, IOS_LoginType.GameCenter.ToString());

                    LoginByCustumId();
                }
                else
                {
                    UiIosLoginBoard.Instance.loginProcess = false;
                }
            });
        }
    }

    private void LoginBySavedData()
    {
        string savedData = PlayerPrefs.GetString(CommonString.IOS_loginType, string.Empty);

        Debug.Log($"savedData {savedData}");
        if (savedData.Equals(string.Empty))
        {
            //가입 진행
            UiIosLoginBoard.Instance.ShowCustomGuestCreateBoard();
        }
        //커스텀 계정
        else if (savedData.Equals(IOS_LoginType.Custom.ToString()))
        {
            string id = PlayerPrefs.GetString(CommonString.SavedLoginTypeKey, string.Empty);
            string passWord = PlayerPrefs.GetString(CommonString.SavedLoginPassWordKey, string.Empty);

            LoginByIdPassWord(id, passWord);
        }
        else if (savedData.Equals(IOS_LoginType.GameCenter.ToString()))
        {
            GameCenterLogin();
        }
    }


    public void LoginByIdPassWord(string id = null, string password = null)
    {
        loginId = PlayerPrefs.GetString(CommonString.SavedLoginTypeKey, loginId);
        LoginByCustumId(id, password);
    }

#if UNITY_ANDROID
    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    PopupManager.Instance.ShowYesNoPopup("알림", "구글 로그인 실패 재시도 합니다", GoogleAuth, () => { Application.Quit(); });
                    return;
                }

                Debug.Log($"Hash {Backend.Utils.GetGoogleHash()}");
                // 로그인이 성공되었습니다.
                loginId = Social.localUser.id;
                email = ((PlayGamesLocalUser)Social.localUser).Email;
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleId - " + Social.localUser.id);
                Debug.Log("UserName - " + Social.localUser.userName);

                LoginByCustumId();
            });
        }
    }
#endif

    private IEnumerator SceneChangeRoutine()
    {
        ServerData.LoadTables();

#if UNITY_IOS
        SendQueue.ResumeSendQueue();
#endif

        while (SendQueue.UnprocessedFuncCount != 0)
        {
            yield return null;
        }

        //
        ServerData.SecondLoadTable();

        while (SendQueue.UnprocessedFuncCount != 0)
        {
            yield return null;
        }
        
        ServerData.ThirdLoadTable();
        
        while (SendQueue.UnprocessedFuncCount != 0)
        {
            yield return null;
        }

        if (isSignIn == false)
        {
            PlayerData.Instance.LoadUserNickName();
        }
        else
        {
            while (true)
            {
                yield return null;

                if (SendQueue.UnprocessedFuncCount <= 0 && isSignIn)
                {
                    nickNameInputBoard.gameObject.SetActive(true);

                    break;
                }
            }
        }
    }

    public void LoginSuccess()
    {
        UiIosLoginBoard.Instance.CloseCustomGuestCreateBoard();
        StartCoroutine(SceneChangeRoutine());
    }

    private void LoginByCustumId(string id = null, string password = null)
    {
        BackendReturnObject bro = null;

        if (id == null)
        {
            bro = Backend.BMember.CustomLogin(GetSocialLoginKey(), GetSocialLoginKey());
        }
        else
        {
            bro = Backend.BMember.CustomLogin(id, password);
            email = password;
        }

        //회원가입 안됨
        if (bro.IsSuccess())
        {
            Debug.Log("Login success");

            LoginSuccess();
        }
        else
        {
            Debug.Log($"LoginFail bro.GetStatusCode() {bro.GetStatusCode()}");
            string message = bro.GetMessage();
            if (message.Equals("Forbidden blocked user, 금지된 blocked user"))
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "차단된 사용자 입니다. 차단 사유 : " + bro.GetErrorCode(), () => { Application.Quit(); });
            }
            else if (message.Equals("Forbidden blocked device, 금지된 blocked device"))
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "차단된 기기 입니다. 차단 사유 : " + bro.GetErrorCode(), () => { Application.Quit(); });
            }
            else if (bro.GetStatusCode() == "403")
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "서버에 문제가 있습니다. 앱을 종료합니다. \n 잠시후 다시 시도해주세요", () => { Application.Quit(); });
            }

            //구글인경우 id=null
            if (bro.GetStatusCode() == "401" && id == null)
            {
                SignIn();
            }
            else if (bro.GetStatusCode() == "401")
            {
                //암호 틀림
                if (bro.GetMessage().Contains("Password"))
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "잘못된 패스워드 입니다.", null);
                }
                else
                {
                    SignIn(id, password);
                }
            }
        }
    }

    public void SignIn(string id = null, string password = null)
    {
        BackendReturnObject BRO = null;

        if (id == null)
        {
            BRO = Backend.BMember.CustomSignUp(GetSocialLoginKey(), GetSocialLoginKey());
        }
        else
        {
            BRO = Backend.BMember.CustomSignUp(id, password);
        }

        if (BRO.IsSuccess())
        {
            isSignIn = true;
            Debug.Log("Sign in success");
            LoginByCustumId(id, password);
        }
        else
        {
            Debug.Log($"SignIn error {BRO.GetStatusCode()}");
            switch (BRO.GetStatusCode())
            {
                case "401":
                    Debug.Log("이미 회원가입된 회원");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아이디나 패스워드가 잘못됐습니다.", null);
                    break;
                case "409":
                    Debug.Log("이미 회원가입된 회원");
                    if (id == null)
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이미 등록된 계정 입니다.", null);
                    }
                    else
                    {
                        LoginByCustumId(id, password);
                    }

                    break;

                case "403":
                    Debug.Log("차단된 사용자 입니다. 차단 사유 : " + BRO.GetErrorCode());
                    break;
            }

#if UNITY_ANDROID
            if (id == null)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "로그인 실패 재시도 합니까?", () => { SignIn(); });
            }
#endif
        }
    }

    private string GetSocialLoginKey()
    {
        //테스트용 a_8846847867697156085
        //로꼬 a_3961873472804492579
#if UNITY_EDITOR
        return editorLoginId;
#endif

        Debug.LogError($"GetGoogleLoginKey {loginId}");
        return loginId;
    }
}