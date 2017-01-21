using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
	[SerializeField]
	Sentence sentence = null;
	[SerializeField]
	Word currentWord = null;
	[SerializeField]
	Word nextWord = null;

	int nextWordID;

	float startTime;
	float timer;

	bool onWave;

	float cursorSizeMax = 250f;
	float cursorSizeMin = 75f;

	float clickTimeRange = 0.2f;
	float refreshTimeRange = 0.4f;

	public int highTime = 1;

	void Start ()
	{
	
	}

	void Update ()
	{
		if (onWave) {
			timer = Time.time - startTime;

			if (timer >= sentence.totalTime) {
//				onWave = false;
//				Invoke ("EndWave", 1);
				EndWave ();
				return;
			}

			float cursorSize = Mathf.Clamp (
				                   cursorSizeMax - Mathf.Abs (currentWord.time - timer) / clickTimeRange * (cursorSizeMax - cursorSizeMin),
				                   cursorSizeMin,
				                   cursorSizeMax
			                   );
			CursorManager.cursorSize = cursorSize;

			if (timer - currentWord.time > nextWord.time - timer) {
				GetNextWord ();
			}
		}
	}

	void AddHighTime ()
	{
		highTime = Mathf.Clamp (highTime + 1, 0, 2);
	}

	void GetNextWord ()
	{
		currentWord = sentence [nextWordID++];
		if (nextWordID < sentence.Count) {
			nextWord = sentence [nextWordID];

			float refreshTime = Mathf.Min (currentWord.time + refreshTimeRange, nextWord.time - refreshTimeRange);
			if (refreshTime > timer) {
				Invoke ("AddHighTime", refreshTime - timer);
			}
		} else {
			nextWord = Word.Infinity;
		}
	}

	public void StartWave (Sentence sentence)
	{
		this.sentence = sentence;
		nextWordID = 0;
		highTime = 1;
		GetNextWord ();

		startTime = Time.time;

		onWave = true;

		GameManager.cursorManager.ShowCircle ();
	}

	void EndWave ()
	{
		sentence = null;
		currentWord = null;
		nextWord = null;
		onWave = false;

		GameManager.cursorManager.HideCircle ();
		GameManager.speechManager.EndWave ();
	}
}
