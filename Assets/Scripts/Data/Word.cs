using UnityEngine;
using System.Collections;

[System.Serializable]
public class Word
{
	public Word (string text = "", float time = 0) {
		this.text = text;
		this.time = time;
	}

	public string text;
	public float time;
	public AudioClip clip;
	public bool dirty = false;
//	[HideInInspector]
	public bool interrupted = false;

	public static Word Infinity {
		get {
			return new Word ("", float.PositiveInfinity);
		}
	}
}
