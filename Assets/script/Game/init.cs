using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class init : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await UnityServices.InitializeAsync();

            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();



                if (AuthenticationService.Instance.IsSignedIn)
                {
                    string username = PlayerPrefs.GetString(key: "Username");
                    if (username == "")
                    {
                        username = "Player";
                        PlayerPrefs.SetString("Username", username);
                    }
                }
            }

            SceneManager.LoadSceneAsync("MainMenu");
        }


        private void OnSignedIn()
        {
            Debug.Log($"Signed In, playerId: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Signed In, token: {AuthenticationService.Instance.AccessToken}");
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
