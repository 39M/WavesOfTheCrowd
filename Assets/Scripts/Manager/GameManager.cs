using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public SpeechData speechData;

	public enum Status
	{
		Idle,
		InSpeech,
		InTransform,
		InWave,
	};

	public static Status status = Status.Idle;

	public static int peopleCount = 0;
	public static int riseCount = 0;


	void Awake ()
	{
		gameManager = this;
		speechManager.speech = speechData.speech;
	}

	void Start ()
	{
		OnSpeech ();
	}

	void Update ()
	{
		switch (status) {
		case Status.Idle:
			break;
		case Status.InSpeech:
			Joker.Move (CursorManager.mousePosition);
			break;
		case Status.InTransform:
			break;
		case Status.InWave:
			Joker.High (CursorManager.mousePosition, CursorManager.cursorSize);
			break;
		}
	
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

	void OnSpeech ()
	{
		Person.inSpeech = true;

		Debug.Log ("Speech Start!");

		speechManager.ShowNextSentence ();
	}

	public void EndSpeech ()
	{
		Person.inSpeech = false;

		Debug.Log ("Speech End!");

		switch (speechManager.currentSentence.type) {
		case Sentence.Types.Normal:
			OnTransformToNote ();
			break;
		case Sentence.Types.High:
			break;
		case Sentence.Types.Wrong:
			break;
		}
	}

	void OnTransformToNote ()
	{
		Debug.Log ("Transform Start!");
		
		speechManager.TransformToNote ();
	}

	public void EndTransformToNote ()
	{
		Debug.Log ("Transform End!");

		switch (speechManager.currentSentence.type) {
		case Sentence.Types.Normal:
			OnWave ();
			break;
		case Sentence.Types.High:
			break;
		case Sentence.Types.Wrong:
			break;
		}
	}

	void OnWave ()
	{
		Debug.Log ("Wave Start!");

		speechManager.StartWave ();
	}

	public void EndWave ()
	{
		Debug.Log ("Wave End!");

		switch (speechManager.currentSentence.type) {
		case Sentence.Types.Normal:
			OnSpeech ();
			break;
		case Sentence.Types.High:
			break;
		case Sentence.Types.Wrong:
			break;
		}
	}

	public void EndLevel ()
	{
		// 关卡结束时的操作
		Debug.Log ("Level End!");
	}
}
