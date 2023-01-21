using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Game
{
    public class lobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        // Start is called before the first frame update
        void Start()
        {
            _lobbyCodeText.text = $"Lobby code: {GameLobbyManager.Instance.GetLobbyCode()}";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
