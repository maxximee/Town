using UnityEngine;
using System.Collections;

namespace MoreMountains.HighroadEngine
{
    /// <summary>
    /// Simple class to allow the player to select a scene on the start screen
    /// </summary>
    public class StartScreen : MonoBehaviour
    {
        [Header("Racing Game")]
        /// the name of the basic racing game
        public string LocalGameSceneName;
        /// the name of the basic racing game / online version
        public string OnlineGameSceneName;

        [Header("Aphex")]
        /// the name of the aphex scene
        public string LocalGameAphexSceneName;
        /// the name of the aphex scene / online version
        public string OnlineGameAphexSceneName;


        public virtual void Start()
        {
        }

        public virtual void OnLocalGameClick()
        {
            RemoveBackgroundGame();
            LoadingSceneManager.LoadScene(LocalGameSceneName);
        }

        public virtual void OnOnlineGameClick()
        {
            OnlineSdkBroker.SelectedOnlineSdk = OnlineSdkBroker.OnlineSdks.Unity;
            RemoveBackgroundGame();
            LoadingSceneManager.LoadScene(OnlineGameSceneName);
        }

        public virtual void OnPunOnlineGameClick()
        {
#if PUN_2_OR_NEWER
                OnlineSdkBroker.SelectedOnlineSdk = OnlineSdkBroker.OnlineSdks.Pun;
			    RemoveBackgroundGame();
                LoadingSceneManager.LoadScene(OnlineGameSceneName);
#endif
        }

        public virtual void OnLocalGameAphexClick()
        {
            RemoveBackgroundGame();
            LoadingSceneManager.LoadScene(LocalGameAphexSceneName);
        }

        public virtual void OnOnlineGameAphexClick()
        {
            OnlineSdkBroker.SelectedOnlineSdk = OnlineSdkBroker.OnlineSdks.Unity;
            RemoveBackgroundGame();
            LoadingSceneManager.LoadScene(OnlineGameAphexSceneName);
        }

        public virtual void OnPunOnlineGameAphexClick()
        {
#if PUN_2_OR_NEWER
                OnlineSdkBroker.SelectedOnlineSdk = OnlineSdkBroker.OnlineSdks.Pun;
                RemoveBackgroundGame();
                LoadingSceneManager.LoadScene(OnlineGameAphexSceneName);
#endif
        }


        protected virtual void RemoveBackgroundGame()
        {
            // We need to remove LocalLobby since it's a persistent object
            Destroy(LocalLobbyManager.Instance.gameObject);
        }
    }
}
