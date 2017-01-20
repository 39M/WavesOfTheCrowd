using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Speech : IEnumerator, IEnumerable
{
	public List<Sentence> sentences;
	int position = -1;

	public Sentence this [int index] {
		get {
			return sentences [index];
		}

		set {
			sentences [index] = value;
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
		return (position < sentences.Count);
	}

	//IEnumerable
	public void Reset ()
	{
		position = 0;
	}

	//IEnumerable
	public object Current {
		get { return sentences [position]; }
	}
}
