using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tanks
{
    public class HUD : MonoBehaviourPunCallbacks
    {
        static HUD instance;
        [SerializeField]
        Button startButton;
        [SerializeField]
        GameObject UI;
        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            startButton.interactable = false;
        }
        public override void OnConnectedToMaster()
        {
            startButton.interactable = true;
        }
        public override void OnEnable()
        {
            // Always call the base to add callbacks 
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        public override void OnDisable()
        {
            // Always call the base to remove callbacks 
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UI.SetActive(!PhotonNetwork.InRoom);
        }


    }
}
