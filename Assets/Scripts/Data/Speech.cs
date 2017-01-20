using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Speech : IEnumerable
{
	public List<Sentence> sentences;

	public Sentence this [int index] {
		get {
			return sentences [index];
		}

		set {
			sentences [index] = value;
		}
	}

	//private enumerator class
	private class MyEnumerator : IEnumerator
	{
		public List<Sentence> sentences;
		int position = -1;

		//constructor
		public MyEnumerator (List<Sentence> sentences)
		{
			this.sentences = sentences;
		}

		private IEnumerator getEnumerator ()
		{
			return (IEnumerator)this;
		}

		//IEnumerator
		public bool MoveNext ()
		{
			position++;
			return (position < sentences.Count);
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
					return sentences [position];
				} catch (IndexOutOfRangeException) {
					throw new InvalidOperationException ();
				}
			}
		}
	}

	public IEnumerator GetEnumerator ()
	{
		return new MyEnumerator (sentences);
	}
}
