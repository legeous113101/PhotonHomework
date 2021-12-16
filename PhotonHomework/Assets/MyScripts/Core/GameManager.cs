using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyScripts.Core
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;
        string gameVersion = "1";
        public GameObject localPlayer;

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogErrorFormat(gameObject,
                        "Multiple instances of {0} is not allow", GetType().Name);
                DestroyImmediate(gameObject);
                return;
            }
            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(gameObject);
            instance = this;

        }
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (!PhotonNetwork.InRoom)
            {
                return;
            }
            localPlayer = PhotonNetwork.Instantiate("Tankplayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
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
            if (PhotonNetwork.IsMasterClient)
            {
                print("Create room.");
                PhotonNetwork.LoadLevel("GameScene");
            }
            else
            {
                print("Join Room!");
            }
        }
    }
}