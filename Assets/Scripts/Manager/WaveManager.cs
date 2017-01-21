using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
	[SerializeField]
	Sentence sentence = null;
	[SerializeField]
	Word currentWord = null;

	int nextWordID;

	float startTime;
	float timer;

	bool onWave;

	float cursorSizeMax = 250f;
	float cursorSizeMin = 75f;

	float clickTimeRange = 0.2f;


	void Start ()
	{
	
	}

	void OnGUI ()
	{
		var e = Event.current;
		if (e.isMouse) {
			Joker.High (e.mousePosition, 100);
		}
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

			if (timer - currentWord.time > clickTimeRange) {
				while (nextWordID < sentence.Count) {
					currentWord = sentence [nextWordID++];
					if (!currentWord.dirty && !currentWord.interrupted) {
						break;
					}
					currentWord = Word.Infinity;
				}
			}
		}
	}

	public void StartWave (Sentence sentence)
	{
		this.sentence = sentence;
		nextWordID = 0;

		foreach (Word word in sentence) {
			nextWordID++;
			if (!word.dirty && !word.interrupted) {
				currentWord = word;
				break;
			}
		}

		if (currentWord == null) {
			currentWord = new Word ();
			currentWord.time = float.PositiveInfinity;
		}

		startTime = Time.time;

		onWave = true;

		GameManager.cursorManager.ShowCircle ();
	}

	void EndWave ()
	{
		sentence = null;
		currentWord = null;
		onWave = false;

		GameManager.cursorManager.HideCircle ();
		GameManager.speechManager.EndWave ();
	}
}
