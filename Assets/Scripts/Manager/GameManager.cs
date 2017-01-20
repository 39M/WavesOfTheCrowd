using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Joker joker;
	public Person person;
	// Speech[] speech; 存储整个关卡的演讲内容；每个成员是一句话；每句话是一个数组／List，存储音节成员
	// int nextSentenceIndex; 当前进行到的话的 Index


	void Awake ()
	{
		gameManager = this;
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
				_cursorMgr = GameObject.Find("UICanvas/CursorPanel/Cursor").GetComponent<CursorManager> ();
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
		// 读取下一句话，调用 SpeechManager 显示出来
		Person.inSpeech = true;
		speechManager.ShowNextSentence ();

		Debug.Log ("Speech Start!");
	}

	public void EndSpeech ()
	{
		// 演讲结束，延时，并进行下一步行动
		Person.inSpeech = false;

		Debug.Log ("Speech End!");

		OnTransformToNote ();
	}

	void OnTransformToNote ()
	{
		speechManager.TransformToNote ();

		Debug.Log ("Transform Start!");
	}

	public void EndTransformToNote ()
	{
		Debug.Log ("Transform End!");

		OnWave ();
	}

	void OnWave ()
	{
		speechManager.StartWave ();

		Debug.Log ("Wave Start!");
	}

	public void EndWave ()
	{
		Debug.Log ("Wave End!");
		// 结束人浪环节，延时，并进行下一步行动
	}

	void NextMove ()
	{
		// 进行下一步行动
	}

	void OnEndLevel ()
	{
		// 关卡结束时的操作
	}
}
