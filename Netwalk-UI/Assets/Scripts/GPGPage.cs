using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GPGPage : MonoBehaviour
{
    private const string SIGN_IN = "Sign In";
    private const string SIGN_OUT = "Sign Out";
    private const string USER_IMAGE_NAME = "UserImage";
    
    private Text txtLogin;
    private Canvas cnv;
    private GameObject userImagePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        txtLogin = GameObject.Find("txtLogin").GetComponent<Text>();
        Debug.Log("1");
        cnv = GameObject.Find("Canvas").GetComponent<Canvas>();
        Debug.Log("2");
        userImagePrefab = Resources.Load<GameObject>("OtherPrefabs/imgUser");
        Debug.Log("3");
        
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = true;
        Debug.Log("4");
        GooglePlayGames.PlayGamesPlatform.Activate();
        Debug.Log("5");
        SetLoginInfo(false);
        Debug.Log("6");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayClick()
    {
        SceneManager.LoadSceneAsync("Scenes/Main");
    }

    public void OnSignInClick()
    {
        if (Social.localUser.authenticated)
        {
            GooglePlayGames.PlayGamesPlatform.Instance.SignOut();
            SetLoginInfo(true);
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                SetLoginInfo(false);
            });

        }
    }

    private void SetLoginInfo(bool forceSignOut)
    {
        var userImage = GameObject.Find(USER_IMAGE_NAME);
        Debug.Log("20");
        if (userImage != null)
        {
            Debug.Log("21");
            Destroy(userImage);
            Debug.Log("22");
        }

        if (Social.localUser.authenticated && !forceSignOut)
        {
            txtLogin.text = SIGN_OUT;
            Debug.Log("10");
            if (Social.localUser.image != null)
            {
                Debug.Log("11");
                userImage = Instantiate(userImagePrefab, cnv.transform);
                Debug.Log("11a");
                userImage.name = USER_IMAGE_NAME;
                Debug.Log("12");
                userImage.GetComponent<RawImage>().texture = Social.localUser.image;
                Debug.Log("13");
            }
        }
        else
        {
            Debug.Log("14");
            txtLogin.text = SIGN_IN;
            Debug.Log("15");
        }
    }
}
