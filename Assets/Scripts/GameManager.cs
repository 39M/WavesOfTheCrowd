﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	// Speech[] speech; 存储整个关卡的演讲内容；每个成员是一句话；每句话是一个数组／List，存储音节成员
	// int nextSentenceIndex; 当前进行到的话的 Index
	//

	void Awake ()
	{
		gameManager = this;
	}

	void Start ()
	{
		
	}

	void Update ()
	{
	
	}

	private static GameManager gameManager;

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

	void OnSpeech ()
	{
		// 读取下一句话，调用 SpeechManager 显示出来
	}

	public void EndSpeech ()
	{
		// 演讲结束，延时，并进行下一步行动
	}

	void OnWave ()
	{
		// 人浪环节，调用 WaveManager
	}

	public void EndWave ()
	{
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
