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
        cnv = GameObject.Find("Canvas").GetComponent<Canvas>();
        userImagePrefab = Resources.Load<GameObject>("OtherPrefabs/imgUser");
        
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();
        SetLoginInfo(false);
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
        if (userImage != null)
        {
            Destroy(userImage);
        }

        if (Social.localUser.authenticated && !forceSignOut)
        {
            txtLogin.text = SIGN_OUT;
            if (Social.localUser.image != null)
            {

                userImage = Instantiate(userImagePrefab, cnv.transform);
                userImage.GetComponent<RawImage>().texture = Social.localUser.image;
            }
        }
        else
        {
            txtLogin.text = SIGN_OUT;
        }
    }
}
