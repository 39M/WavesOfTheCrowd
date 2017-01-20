using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeechManager : MonoBehaviour
{
	Sentence currentSentence;
	Word currentWord;

	int nextSentenceID;
	int nextWordID;

	float startTime;
	float timer;

	public Transform wordStartMark;
	public Transform wordGroupHide;
	public Transform wordGroup;
	public GameObject wordItemPrefab;
	List<Transform> wordItemsHide = new List<Transform> ();
	List<Transform> wordItems = new List<Transform> ();

	public Transform noteBar;
	public Transform noteGroup;
	public float noteBarLength;
	public Transform scanLine;



	public Speech speech;

	void Start ()
	{
		nextSentenceID = 0;
	}

	void Update ()
	{
		// 赋值 timer
		// 判断下一个单词的出现时机和 timer 
		// Y: ShowWord(nextWord)
		// N: pass

		// 如果已经显示了最后一个单词，则告知 GameManager 结束

	}

	public void ShowNextSentence ()
	{
		currentSentence = speech [nextSentenceID++];
		nextWordID = 0;

//		startTime = Time.time;
//		timer = 0;

		foreach (Word word in currentSentence) {
			var item = Instantiate (wordItemPrefab, wordGroupHide) as GameObject;
			wordItemsHide.Add (item.transform);

			item.GetComponentInChildren<Text> ().text = word.text;

			Invoke ("ShowWord", word.time);
		}

		var lastWord = currentSentence.LastWord;
		float delay = 1f;
		Invoke ("EndSpeech", lastWord.time + delay);
	}

	void ShowWord ()
	{
		// 显示单词 播放动画
		currentWord = currentSentence [nextWordID];
		if (currentWord.clip) {
			GameManager.audio.PlayOneShot (currentWord.clip);
		}

		var itemHide = wordItemsHide [nextWordID];

		var item = Instantiate (wordItemPrefab, wordGroup) as GameObject;
		wordItems.Add (item.transform);

		item.GetComponentInChildren<Text> ().text = currentWord.text;
		var posTween = item.GetComponent<PositionTween> ();
		posTween.from = wordStartMark.localPosition;
		posTween.to = itemHide.localPosition;

		foreach (BaseTween tween in item.GetComponents<BaseTween>()) {
			tween.ResetTween ();
			tween.Play ();
		}

		nextWordID++;
	}

	void EndSpeech ()
	{
		GameManager.instance.EndSpeech ();
	}

	public void TransformToNote ()
	{
		nextWordID = 0;

		foreach (Word word in currentSentence) {
			Invoke ("TransformNext", word.time / 2f);
		}

		Invoke ("EndTransform", currentSentence.LastWord.time / 2 + 1);
	}

	void TransformNext ()
	{
		var item = wordItems [nextWordID];
		item.SetParent (noteGroup);
		item.Find ("Text").gameObject.SetActive (false);
		item.Find ("Note").gameObject.SetActive (true);

		var posTween = item.GetComponent<PositionTween> ();
		posTween.from = item.localPosition;

		var posTo = Vector3.zero;
		var word = currentSentence [nextWordID];
		posTo.x = (word.time / currentSentence.totalTime - 0.5f) * noteBarLength;
		posTween.to = posTo;

		posTween.Play ();

		nextWordID++;
	}

	void EndTransform ()
	{
		GameManager.instance.EndTransformToNote ();
	}

	public void StartWave ()
	{
		var posTween = scanLine.GetComponent<PositionTween> ();

		var posFrom = Vector3.zero;
		posFrom.x = -noteBarLength / 2f;
		posTween.from = posFrom;

		var posTo = Vector3.zero;
		posTo.x = noteBarLength / 2f;
		posTween.to = posTo;

		posTween._duration = currentSentence.totalTime;

		posTween.ResetTween ();
		posTween.Play ();

		GameManager.waveManager.StartWave (currentSentence);
	}

	public void EndWave()
	{
		GameManager.instance.EndWave ();
	}
}
