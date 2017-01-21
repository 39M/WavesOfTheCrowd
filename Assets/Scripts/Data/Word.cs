﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Word
{
	public string text;
	public float time;
	public AudioClip clip;
	public bool dirty;
	[HideInInspector]
	public bool interrupted;
}
