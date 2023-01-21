using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class MainMenuControll : MonoBehaviour
    {
        [SerializeField] private GameObject _mainScreen;
        [SerializeField] private GameObject _joinScreen;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;

        [SerializeField] private Button _submitCodeButton;
        [SerializeField] private TextMeshProUGUI _codeText;
        // Start is called before the first frame update
        void OnEnable()
        {
            _hostButton.onClick.AddListener(OnHostClick);
            _joinButton.onClick.AddListener(OnJoinClick);
            _submitCodeButton.onClick.AddListener(OnSubmitCodeClicked);
        }
        private void OnDisable()
        {
            _hostButton.onClick.RemoveListener(OnHostClick);
            _joinButton.onClick.RemoveListener(OnJoinClick);
            _submitCodeButton.onClick.RemoveListener(OnSubmitCodeClicked);
        }

        private async void OnHostClick()
        {
            bool succeeded = await GameLobbyManager.Instance.CreateLobby();
            if (succeeded)
            {
                SceneManager.LoadScene("Lobby");
            }
        }

        private void OnJoinClick()
        {
            Debug.Log(message: "joinClick!!!");
            _mainScreen.SetActive(false);
            _joinScreen.SetActive(true);
        }

        private async void OnSubmitCodeClicked()
        {
            string code = _codeText.text;
            code = code.Substring(0, code.Length - 1);

            bool succeeded = await GameLobbyManager.Instance.JoinLobby(code);
            Debug.Log(code);
            if (succeeded)
            {
                SceneManager.LoadScene("Lobby");
            }
        }
    }
}
