using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;

        string gameVersion = "1";

        public static GameObject localPlayer;

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
                DestroyImmediate(gameObject);
                return;
            }

            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            SceneManager.sceneLoaded += OnSceneLoaded;
            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!PhotonNetwork.InRoom) return;
            localPlayer = PhotonNetwork.Instantiate("Tankplayer", Vector3.zero, Quaternion.identity, 0);
        }

        public override void OnConnected()
        {
            Debug.Log("PUN Connected");
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Connected to Master");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Disconnected was called by PUN with reason {0}", cause);
        }

        public void JoinGameRoom()
        {
            var options = new RoomOptions
            {
                MaxPlayers = 6
            };

            PhotonNetwork.JoinOrCreateRoom("Kingdom", options, null);
        }

        public override void OnJoinedRoom()
        {
            var ismaster = PhotonNetwork.IsMasterClient;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Created room!!");
                PhotonNetwork.LoadLevel("GameScene");
            }
            else
            {
                Debug.Log("Joined room!!");
            }
        }
    }
}
