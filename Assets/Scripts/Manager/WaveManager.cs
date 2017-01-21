using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
	Sentence sentence;
	Word currentWord;

	int nextWordID;

	float startTime;
	float timer;

	bool onWave;

	float cursorSizeMax = 250f;
	float cursorSizeMin = 75f;

	float clickTimeRange = 0.25f;


	void Start ()
	{
	
	}

	void OnGUI()
	{
		var e=Event.current;
		if( e.isMouse )
		{
			Joker.High( e.mousePosition, 100 );
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
//				GameManager.audio.PlayOneShot (currentWord.clip);
				if (nextWordID < sentence.Count) {
					currentWord = sentence [nextWordID++];
				}
			}

			//		var cursor = Texture2D.whiteTexture;
			//		cursor.Resize ((int)Time.time, (int)Time.time);
			//		Cursor.SetCursor (cursor, Vector2.zero, CursorMode.Auto);
			
			if (Input.anyKeyDown) {
				// Input.mousePosition, cursorSize
				// 将鼠标位置 圆圈范围 发给 Joker 进行判断

			}
		}
	}

	public void StartWave (Sentence sentence)
	{
		this.sentence = sentence;
		nextWordID = 0;
		currentWord = sentence [0];

		startTime = Time.time;

		onWave = true;

		GameManager.cursorManager.ShowCursor ();
	}

	void EndWave ()
	{
		onWave = false;

		GameManager.cursorManager.HideCursor ();
		GameManager.speechManager.EndWave ();
	}
}
