using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sentence : IEnumerator, IEnumerable
{
	public List<Word> words;
	int position = -1;

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

	public IEnumerator GetEnumerator ()
	{
		return (IEnumerator)this;
	}

	//IEnumerator
	public bool MoveNext ()
	{
		position++;
		return (position < words.Count);
	}

	//IEnumerable
	public void Reset ()
	{
		position = 0;
	}

	//IEnumerable
	public object Current {
		get { return words [position]; }
	}
}
