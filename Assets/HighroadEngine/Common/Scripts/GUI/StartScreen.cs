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



        public virtual void OnLocalGameClick()
        {
            RemoveBackgroundGame();
            //LoadingSceneManager.LoadScene(LocalGameSceneName);
        }

        public virtual void OnPunOnlineGameClick()
        {
                OnlineSdkBroker.SelectedOnlineSdk = OnlineSdkBroker.OnlineSdks.Pun;
			    RemoveBackgroundGame();
                //LoadingSceneManager.LoadScene(OnlineGameSceneName);
        }

        protected virtual void RemoveBackgroundGame()
        {
            // We need to remove LocalLobby since it's a persistent object
            Destroy(LocalLobbyManager.Instance.gameObject);
        }
    }
}
