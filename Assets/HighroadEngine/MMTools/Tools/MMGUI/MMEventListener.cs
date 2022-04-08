namespace MoreMountains.Tools
{
	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface MMEventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface MMEventListener<T> : MMEventListenerBase
	{
		void OnMMEvent(T eventType);
	}


	/// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
		public delegate void Delegate<T>(T eventType);

		public static void MMEventStartListening<EventType>(this MMEventListener<EventType> caller) where EventType : struct
		{
			MMEventManager.AddListener<EventType>(caller);
		}

		public static void MMEventStopListening<EventType>(this MMEventListener<EventType> caller) where EventType : struct
		{
			MMEventManager.RemoveListener<EventType>(caller);
		}
	}

}