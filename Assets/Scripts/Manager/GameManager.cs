using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Joker joker;
	public Person person;
	// Speech[] speech; 存储整个关卡的演讲内容；每个成员是一句话；每句话是一个数组／List，存储音节成员
	// int nextSentenceIndex; 当前进行到的话的 Index
	public SpeechData speechData;

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
