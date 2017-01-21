using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sentence : IEnumerable
{
	public enum Types
	{
		Normal,
		High,
		Wrong,
	};

	public Types type;
	public float totalTime;
	public int fontSize = 48;
	public List<Word> words;
//	[HideInInspector]
	public bool interrupted = false;

	public int Count {
		get {
			return words.Count;
		}
	}

	public Word LastWord {
		get {
			return words [words.Count - 1];
		}
	}

	public Word this [int index] {
		get {
			return words [index];
		}

		set {
			words [index] = value;
		}
	}

	//private enumerator class
	private class MyEnumerator : IEnumerator
	{
		public List<Word> words;
		int position = -1;

		//constructor
		public MyEnumerator (List<Word> words)
		{
			this.words = words;
		}

		private IEnumerator getEnumerator ()
		{
			return (IEnumerator)this;
		}

		//IEnumerator
		public bool MoveNext ()
		{
			position++;
			return (position < words.Count);
		}

		//IEnumerator
		public void Reset ()
		{
			position = -1;
		}

		//IEnumerator
		public object Current {
			get { 
				try {
					return words [position];
				} catch (IndexOutOfRangeException) {
					throw new InvalidOperationException ();
				}
			}
		}
	}

	public IEnumerator GetEnumerator ()
	{
		return new MyEnumerator (words);
	}
}
