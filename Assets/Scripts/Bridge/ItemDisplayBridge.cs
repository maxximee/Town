using System.Collections;
using UnityEngine;
using ArrayUtility;

public class ItemDisplayBridge : MonoBehaviour
{

	public string[] powerups;

	public void PlaySpinTickSound()
	{
		Debug.Log("play audio");
	}

	public void GetRandomIcon()
	{
		Debug.Log(powerups.RandomElement());
	}

	private int GetRandomPowerup() {
        var powerUps = powerups;
        // var seed = Runner.Simulation.Tick;
        // Random.InitState(seed);
        
        return Random.Range(0, powerUps.Length);
    }
}