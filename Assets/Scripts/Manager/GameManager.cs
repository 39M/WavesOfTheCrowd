using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public SpeechData speechData;
	public AudioClipData audioClipData;

	public enum Status
	{
		Idle,
		InSpeech,
		InTransform,
		InWave,
	};

	public Status status = Status.Idle;

	public int peopleCount = 0;
	public int riseCount = 0;


	void Awake ()
	{
		gameManager = this;
		speechManager.speech = speechData.speech;
	}

	void Start ()
	{
		Joker.Create ();
		Person.Create (20);
		Invoke ("OnStart", 2);
	}

	void OnStart ()
	{
		audio.PlayOneShot (audioClipData.openingSpeech);
		Invoke ("OnSpeech", audioClipData.openingSpeech.length + 1);
	}

	void Update ()
	{
		//do move expected inwave
		Joker.Move (CursorManager.mousePosition);

		switch (status) {
		case Status.Idle:
			break;
		case Status.InSpeech:
			break;
		case Status.InTransform:
			break;
		case Status.InWave:
			if (Input.GetMouseButtonDown (0)) {
				if (waveManager.highTime > 0) {
					waveManager.highTime--;
					audio.PlayOneShot (audioClipData.highs [Random.Range (0, audioClipData.highs.Count)]);
					Person.attractPeople = 0;
					Joker.High (CursorManager.mousePosition,
						CursorManager.cursorSize,
						speechManager.currentSentence.type == Sentence.Types.HandsUp
					);
				}
			}
			break;
		}
	}

	public void onPresidentClick ()
	{
		Joker.Interrupt (CursorManager.mousePosition, CursorManager.cursorSize);
		speechManager.Interrupt ();
		audio.PlayOneShot (audioClipData.interrupt);
	}

	private static GameManager gameManager = null;

	public static GameManager instance {
		get {
			if (gameManager == null) {
				var go = new GameObject ("GameManager");
				var gm = go.AddComponent<GameManager> ();
				return gm;
			}
			return gameManager;
		}
	}

	private static SpeechManager _speechMgr = null;

	public static SpeechManager speechManager {
		get {
			if (_speechMgr == null) {
				_speechMgr = instance.GetComponent<SpeechManager> ();
			}
			return _speechMgr;
		}
	}

	private static WaveManager _waveMgr = null;

	public static WaveManager waveManager {
		get {
			if (_waveMgr == null) {
				_waveMgr = instance.GetComponent<WaveManager> ();
			}
			return _waveMgr;
		}
	}

	private static CursorManager _cursorMgr = null;

	public static CursorManager cursorManager {
		get {
			if (_cursorMgr == null) {
				_cursorMgr = GameObject.Find ("UICanvas/CursorPanel/Cursor").GetComponent<CursorManager> ();
			}
			return _cursorMgr;
		}
	}


	private static AudioSource _audio = null;

	public static new AudioSource audio {
		get {
			if (_audio == null) {
				_audio = Camera.main.GetComponent<AudioSource> ();
			}
			return _audio;
		}
	}

	private static Canvas _canvas = null;

	public static Canvas canvas {
		get {
			if (_canvas == null) {
				_canvas = GameObject.Find ("UICanvas").GetComponent<Canvas> ();
			}
			return _canvas;
		}
	}


	void OnSpeech ()
	{
		status = Status.InSpeech;
		Person.inSpeech = true;

		Debug.Log ("Speech Start!");

		speechManager.ShowNextSentence ();
	}

	public void EndSpeech ()
	{
		status = Status.Idle;
		Person.inSpeech = false;

		Debug.Log ("Speech End!");

		OnTransformToNote ();
	}

	void OnTransformToNote ()
	{
		status = Status.InTransform;

		Debug.Log ("Transform Start!");
		
		speechManager.TransformToNote ();
	}

	public void EndTransformToNote ()
	{
		status = Status.Idle;

		Debug.Log ("Transform End!");

		OnWave ();
	}

	void OnWave ()
	{
		status = Status.InWave;

		Debug.Log ("Wave Start!");

		speechManager.StartWave ();
	}

	public void EndWave ()
	{
		status = Status.Idle;

		Debug.Log ("Wave End!");

		OnSpeech ();
	}

	public void EndLevel ()
	{
		// 关卡结束时的操作
		Debug.Log ("Level End!");
	}
}
