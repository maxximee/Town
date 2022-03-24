using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Types;

namespace MoreMountains.HighroadEngine 
{
	/// <summary>
	/// Online lobby manager. Manages UNET network integration with server creation / joining and lobby players.
	/// It's also in charge of communication to the network race manager (players data & scene selection)
	/// </summary>
	public class OnlineLobbyManager : IGenericLobbyManager 
	{
		/// reference to static instance.
		static public OnlineLobbyManager Instance;

		[Header("GUI")]
		/// reference to OnlineLobbyGUI child GameObject
		public OnlineLobbyUI _onlineLobbyUI;

		[Header("Vehicles configuration")]
		/// the list of vehicles prefabs the player can choose from.
		public GameObject[] AvailableVehiclesPrefab; 

		[Header("Tracks configuration")]
		/// the list of Track Scenes names. Used to load scene & show scene name in UI
		public string[] AvailableTracksSceneName; 
		/// the list of tracks sprites. Used to show image of chosen track in UI
		public Sprite[] AvailableTracksSprite; 

		[Header("Matchmaking")]
		[Information("Set a unique id (number). This will be used to separate online rooms between each type of game.\n", InformationAttribute.InformationType.Info, false)]
		/// the unique online game identifier.
		public int GameId = 0;

		protected ulong _matchId; // The match identifier in matchmaking
		protected ulong _nodeId;
		protected string _matchName;
		protected bool _matchServer = false;
		protected bool _disconnectServer = false;
		protected int _playersReadyToPlayCount;
		protected int _currentStartPosition = 0;
		protected bool _destroyInstance = false;


		/// <summary>
		/// Gets or sets a value indicating whether players are ready to play.
		/// We use this value to delay the start race countdown
		/// </summary>
		/// <value><c>true</c> if players ready to play; otherwise, <c>false</c>.</value>
		public virtual bool PlayersReadyToPlay {get; protected set;}

		/// <summary>
		/// Initializes the manager
		/// </summary>
		public virtual void Start() 
		{
			Instance = this;

            //_onlineLobbyUI = GetComponentInChildren<OnlineLobbyUI>(); 

			// Init UI
			_onlineLobbyUI.ShowLobby();

			OnReturnToMain();

			// Register call on scene loaded to destroy this object
			SceneManager.sceneLoaded += SceneManager_sceneLoaded; 
		}
			
		


		/// <summary>
		/// Updates the wait for players text in the lobby screen.
		/// </summary>
		public virtual void Update()
		{
			
		}

		/// <summary>
		/// Describes what happens when the server player clicks on the start game button
		/// </summary>
		public virtual void OnStartGame()
		{
			
		}

		#region main actions

		/// <summary>
		/// Describes what happens when the player clicks on the Match Making button
		/// </summary>
		public virtual void OnMatchmaking()
		{
			// Update UI
			_onlineLobbyUI.ShowMatchmaking();
		
			InitMatchmaking();
		}

		/// <summary>
		/// Describes what happens when the player clicks on the Direct Connection button
		/// </summary>
		public virtual void OnDirectConnection()
		{
			// Update UI
			_onlineLobbyUI.ShowDirectConnection();
		}

		/// <summary>
		/// Describes what happens when the player clicks on the main button
		/// </summary>
		public virtual void OnReturnToMain()
		{
	
			_onlineLobbyUI.ShowLobby();
			_onlineLobbyUI.ShowMainMenu();
		}

		#endregion

		#region matchmaking

		/// <summary>
		/// Initializes the matchmaking connection.
		/// </summary>
		public virtual void InitMatchmaking()
		{
			
		}

		/// <summary>
		/// Describes what happens when the player creates a new matchmaking game
		/// </summary>
		public virtual void OnClickCreateMatchmakingGame()
		{
			
		}

		/// <summary>
		/// Describes what happens when the server list gets refreshed
		/// </summary>
		public virtual void OnClickRefreshServerList()
		{
			// we remove the currently shown matches
			_onlineLobbyUI.RemoveMatchesFromMatchmakingList();

			// we display a popup to inform the user
			_onlineLobbyUI.ShowPopup("Refresh online matches...");

		}


		#endregion

		#region direct connection

		/// <summary>
		/// Start host action
		/// </summary>
		public virtual void OnClickStartHost()
		{
			
		}



		#endregion

		#region connected 

		/// <summary>
		/// Back button from connected canvas
		/// </summary>
		public virtual void OnConnectedReturnToMain()
		{
			
	
		}

			
		#endregion

		#region GenericLobbyManager implementation

		/// <summary>
		/// Returns Players
		/// </summary>
		public virtual Dictionary<int, ILobbyPlayerInfo> Players()
		{
			Dictionary<int, ILobbyPlayerInfo> players = new Dictionary<int, ILobbyPlayerInfo>();

			return players;
		}

		/// <summary>
		/// Gets the player.
		/// </summary>
		/// <returns>The player.</returns>
		/// <param name="position">Position.</param>
		public virtual ILobbyPlayerInfo GetPlayer (int position)
		{

			return null;
		}

		/// <summary>
		/// Returns the maximum number of players allowed
		/// </summary>
		/// <value>The max players.</value>
		public virtual int MaxPlayers 
		{
			get 
			{
				return 4;
			}
		}

		/// <summary>
		/// Changes the current scene to lobby scene as server
		/// </summary>
		public virtual void ReturnToLobby()
		{
			
		}

		/// <summary>
		/// Changes the current scene to the start screen.
		/// </summary>
		public virtual void ReturnToStartScreen()
		{
			_destroyInstance = true;
			LoadingSceneManager.LoadScene("StartScreen");
		}

		#endregion

		/// <summary>
		/// We use this event to destroy this object when the scene has changed and the instance must be destroyed
		/// </summary>
		/// <param name="scene">Scene.</param>
		/// <param name="mode">Mode.</param>
		protected virtual void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (_destroyInstance)
			{
				SceneManager.sceneLoaded -= SceneManager_sceneLoaded; 
				
			}
		}
	}
}
