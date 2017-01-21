using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

public class SpeechManager : MonoBehaviour
{
	public Speech speech;
	public Sentence currentSentence;
	[SerializeField]
	Word currentWord;

	[SerializeField]
	int nextSentenceID = 0;
	[SerializeField]
	int nextWordID = 0;

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
	[SerializeField]
	float noteBarLength;
	public Transform scanLine;
	public AudioClip interrupt;

	List<string> badStrings = new List<string> () {
		@"$%^",
		@"^(#&$",
		@")#",
		@"#$%^&$^&",
		@"(*&^%(*^@#$",
		@"&#^",
		@"!(@#",
		@"!@(&{#",
		@"*#&@()%",
		@"?!@$*(#@",
		@"(&#&$)@",
	};

	void Start ()
	{
		noteBarLength = noteBar.GetComponent<RectTransform> ().rect.width;
	}

	void Update ()
	{
//		if (Input.anyKeyDown)
//			Interrupt ();
	}

	public void ShowNextSentence ()
	{
		if (nextSentenceID >= speech.Count) {
			GameManager.instance.EndLevel ();
			return;
		}

		ClearLastSentence ();

		switch (currentSentence.type) {
		case Sentence.Types.Normal:
			break;
		case Sentence.Types.High:
			// TODO 显示提示文字 演讲高潮
			break;
		case Sentence.Types.Wrong:
			break;
		}

		currentSentence = speech [nextSentenceID];
		nextWordID = 0;

//		startTime = Time.time;
//		timer = 0;

		foreach (Word word in currentSentence) {
			var item = Instantiate (wordItemPrefab, wordGroupHide) as GameObject;
			wordItemsHide.Add (item.transform);

			item.GetComponentInChildren<Text> ().text = word.text;

			Invoke ("ShowWord", word.time);
		}

		Invoke ("EndSpeech", currentSentence.totalTime);

		nextSentenceID++;
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

		var text = item.GetComponentInChildren<Text> ();
		text.text = currentWord.text;
		if (currentSentence.interrupted) {
			currentWord.interrupted = true;

			var badString = badStrings [Random.Range (0, badStrings.Count - 1)];
			itemHide.GetComponentInChildren<Text> ().text = badString;
			LayoutRebuilder.ForceRebuildLayoutImmediate (wordGroupHide.GetComponent<RectTransform> ());

			text.DOText (badString, 1f, true, ScrambleMode.All);
		}

		switch (currentSentence.type) {
		case Sentence.Types.High:
			text.color = Color.cyan;
			break;
		case Sentence.Types.Wrong:
			break;
		}

		var posTween = item.GetComponent<PositionTween> ();
		posTween.from = wordStartMark.localPosition;
		posTween.to = itemHide.localPosition;

		foreach (BaseTween tween in item.GetComponents<BaseTween>()) {
			tween.ResetTween ();
			tween.Play ();
		}

		nextWordID++;
	}

	void ClearLastSentence ()
	{
		foreach (var item in wordItemsHide) {
			Destroy (item.gameObject);
		}
		wordItemsHide.Clear ();

		foreach (var item in wordItems) {
			Destroy (item.gameObject);
		}
		wordItems.Clear ();
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

	Vector3 RandomPositionInPeople ()
	{
		return new Vector3 (Random.Range (-1200, 350), Random.Range (125f, 600f));
	}

	void TransformNext ()
	{
		var item = wordItems [nextWordID];
		var word = currentSentence [nextWordID];

		var posTween = item.GetComponent<PositionTween> ();
		posTween.from = item.localPosition;

		if (word.interrupted || word.dirty) {
			var newItem = (Instantiate (item.gameObject, wordGroup) as GameObject).transform;
			var newPosTween = newItem.GetComponent<PositionTween> ();

			if (word.dirty) {
				var text = newItem.Find ("Text").GetComponent<Text> ();
				text.color = Color.red;
				// TODO 飞到人群中爆炸(大)
				newPosTween.onComplete = () => {
					Debug.Log ("Big Bang");
					Destroy(newItem.gameObject);
				};
			} else {
				// TODO 飞到人群中爆炸（小）
				newPosTween.onComplete = () => {
					Debug.Log("Small Bang");
					Destroy(newItem.gameObject, 1f);
				};
			}

			newPosTween.from = newItem.localPosition;
			newPosTween.to = RandomPositionInPeople ();
			newPosTween.Play ();
		}

		item.SetParent (noteGroup);
		item.Find ("Text").gameObject.SetActive (false);
		item.Find ("Note").gameObject.SetActive (true);
			
		var posTo = Vector3.zero;
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

	public void EndWave ()
	{
		GameManager.instance.EndWave ();
	}

	public void Interrupt ()
	{
		if (GameManager.instance.status == GameManager.Status.InSpeech) {
			currentSentence.interrupted = true;
		}
	}
}
