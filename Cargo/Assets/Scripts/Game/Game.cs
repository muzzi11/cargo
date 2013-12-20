using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour, BattleListener
{
	public float battleDistanceInterval;
	public float battleChance;
	public Rect cameraBounds;
	public GUISkin guiSkin;
	public AudioClip[] audioClips;
	public AudioClip battleHorn, battleMusic;

	private float distSinceLastBattle = 0.0f;
	private bool gameOver = false;

	private Ship ship;
	private Cargo cargo;
	private Space space;
	private Balance balance;
	private State currentState;
	private GameObject playerNode;

	// Use this for initialization
	void Start()
	{
		SaveGame.LoadData();

		ship = new Ship();
		cargo = new Cargo(200);
		space = GameObject.Find("Space").GetComponent<Space>();
		space.GeneratePlanets(cameraBounds);
		balance = new Balance();
		currentState = new NavigationState(space, ship, balance, cargo);
		//currentState = new BattleState(currentState, ship);
		playerNode = GameObject.Find("Player Node");

		StartCoroutine("Save");
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Application.LoadLevel(0);

		distSinceLastBattle += ship.Update();

		playerNode.transform.position = ship.Position;
		UpdateCameraPosition();
		
		if(!audio.isPlaying && !(currentState is BattleState)) PlayMusic();
		if(!audio.isPlaying && (currentState is BattleState)) PlayBattleMusic();

		if(Input.GetKeyDown(KeyCode.Escape)) Application.LoadLevel(0);

		// Random battle event
		if(distSinceLastBattle >= battleDistanceInterval)
		{
			distSinceLastBattle -= battleDistanceInterval;
			if(Random.value <= battleChance)
			{
				PlayBattleHorn();
				ship.Stop();
				currentState = new BattleState(this, currentState, ship);
			}
		}
	}

	void OnGUI()
	{
		GUI.skin = guiSkin;
		currentState = currentState.UpdateState();
		if(gameOver)
		{
			gameOver = false;
			currentState = new GameOverState();
		}
	}

	public void ShipDestroyed()
	{
		gameOver = true;
		SaveGame.RemoveSaveGame();
	}

	private void UpdateCameraPosition()
	{
		Vector3 cameraPos = ship.Position;

		cameraPos.x = Mathf.Clamp(cameraPos.x, cameraBounds.xMin, cameraBounds.xMax);
		cameraPos.y = Mathf.Clamp(cameraPos.y, cameraBounds.yMin, cameraBounds.yMax);
		cameraPos.z = Camera.main.transform.position.z;
		
		Camera.main.transform.position = cameraPos;
	}

	private void PlayMusic()
	{
		if (audioClips.Length == 0) return;
		audio.clip = audioClips[Random.Range(0, audioClips.Length)];
		audio.Play();
	}

	private void PlayBattleHorn()
	{
		StopMusic();
		audio.clip = battleHorn;
		audio.Play();
	}

	private void PlayBattleMusic()
	{
		audio.clip = battleMusic;
		audio.Play();
	}

	private void StopMusic()
	{
		if (audio.isPlaying)
			audio.Stop();
	}

	IEnumerator Save()
	{
		while(!gameOver)
		{
			SaveGame.SavePosition(ship.Position);
			yield return new WaitForSeconds(2.0f);
		}
	}
}
