using UnityEngine;
using System.Collections;

[System.Serializable]
public class Word
{
	public string text;
	public float time;
	public bool dirty;
	public AudioClip clip;
	[HideInInspector]
	public bool interrupted;
}
