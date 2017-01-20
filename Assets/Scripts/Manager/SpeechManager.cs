using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpeechManager : MonoBehaviour
{
	public Speech speech;

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
	List<Transform> wordItemsHide = new List<Transform>();
	List<Transform> wordItems = new List<Transform>();

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
		float delay = 0.5f;
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
		nextWordID++;

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

	}

	void EndSpeech()
	{
		GameManager.instance.EndSpeech ();
	}
}
