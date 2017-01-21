using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(Animator))]
public class Person : MonoBehaviour
{
	#region Static

	public static List<Person> people = new List<Person> ();
	public static List<Person> risePeople = new List<Person> ();

	public static bool inSpeech = false;
	public static Vector2 mousePos;
	public static Vector2 center;
	public static Vector2 sensitive;
	public static Rect clamp;

	public static int attractLevel{ get { return 4; } }

	public static int attractPeople;

	public static void Create (int num = 1)
	{
		var canvas = GameObject.Find("UICanvas");
		for (int i = 0; i < num; i++) {
			var go = Resources.Load<GameObject> ("Entity/person");
			var p = Instantiate(go).GetComponent<Person>();
			p.sentiment = 50;
			p.manKind = (ManKind)Random.Range (0, 4);
			p.animator.Play (p.manKind.ToString () + "_stand");
			p.transform.SetParent (canvas.transform,false);
			p.location = new Vector2 (Random.Range (-500, 500), Random.Range (-500, 500));
			people.Add (p);
			GameManager.instance.peopleCount = people.Count;
			GameManager.instance.riseCount = risePeople.Count;
		}
	}

	public static void RisePerson (Person person)
	{
		Debug.Log (attractPeople);
		risePeople.Add (person);
		var plus = (int)(risePeople.Count / attractLevel - attractPeople);
		Create (plus);
			//TODO create anim
		attractPeople += plus;
	}

	public static void High (Rect area, float level)
	{
		foreach (var p in people) {
			if (area.Contains (p.location)) {
				Debug.Log (p.transform.localPosition);
				p.High (level);
			}
		}
	}

	#endregion


	#region object
	public enum ManKind
	{
		man,
		monk,
		musical,
		women,
	}
	public ManKind manKind;

	[SerializeField]
	Vector2 location
	{
		get{
			var pos = transform.localPosition;
			return new Vector2 (pos.x, pos.y * 2); }
		set{ 
			transform.localPosition = new Vector3 (value.x, value.y * 0.5f, 0);
		}
	}
	[SerializeField]
	float calm = 5f;
	[SerializeField]
	int minSenti = 0;
	[SerializeField]
	int maxSenti = 100;

	public float sentiment = 50;

	public bool isRise {
		get {
			return sentiment > 0;
		}
	}

	Animator _animator;

	Animator animator {
		get {
			if (!_animator)
				_animator = GetComponent<Animator> ();
			return _animator;
		}
	}

	public void SetLocation (Vector2 loc)
	{
		location = loc;
	}

	public void High (float level)
	{
		//影响观众
		sentiment += level;
		//add in high list
		if (isRise && !risePeople.Contains (this)) {
			animator.Play ("rise");
			GetComponent<UnityEngine.UI.Image> ().color = Color.red;
			RisePerson (this);
		}
	}

	void Update ()
	{
		if (!inSpeech) {
			var rised = isRise;
			sentiment -= Time.deltaTime * calm;
			sentiment = Mathf.Clamp (sentiment, minSenti, maxSenti);
			if (rised && !isRise) {
				animator.Play (manKind.ToString () + "_stand");
			}
		}
	}

	#endregion

}
