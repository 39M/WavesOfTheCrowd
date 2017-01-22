using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		var canvas = GameObject.Find ("UICanvas");
		for (int i = 0; i < num; i++) {
			var go = Resources.Load<GameObject> ("Entity/person");
			var p = Instantiate (go).GetComponent<Person> ();
			p.sentiment = 50;
			p.manKind = (ManKind)Random.Range (0, 4);
			p.animator.Play (p.manKind.ToString () + "_stand");
			p.transform.SetParent (canvas.transform, false);
			p.location = new Vector2 (Random.Range (-800, 800), Random.Range (-500, 500));

			var image = p.GetComponent<Image> ();
			image.color = new Color (1, 1, 1, 0);
			image.DOFade (1, 0.5f).SetAutoKill (true);
			people.Add (p);
			GameManager.instance.peopleCount = people.Count;
			GameManager.instance.riseCount = risePeople.Count;
		}
	}

	static void RisePerson (Person person)
	{
		risePeople.Add (person);
		var plus = (int)(risePeople.Count / attractLevel - attractPeople);
		Create (plus);
		attractPeople += plus;
	}

	static void CalmPerson (Person person)
	{
		person.ConfusePerson();
		risePeople.Remove (person);
		attractPeople--;
		GameManager.instance.riseCount = risePeople.Count;
	}
	
	static void LeavePerson (Person person)
	{
		person.GetComponent<Image> ().DOFade (0, 0.5f).SetAutoKill ();
		person.emotion.sprite = person.leave;
		person.emotion.color = Color.white;
		person.emotion.DOFade (0, 0.6f).SetDelay (0.4f);
		people.Remove (person);
		GameManager.instance.peopleCount = people.Count;
	}

	public static void High (Vector2 navy, float radius, float level, bool handsUp = false)
	{
		var sqrt = radius * radius;
		var clone = new List<Person> ();
		var count = 0;
		clone.AddRange (people);
		foreach (var p in clone) {
			var delta = navy - p.location;
			if (delta.x * delta.x + delta.y * delta.y < sqrt) {
				p.High (level, handsUp);
				count++;
			}
			if (count > 20) {
				break;
			}
		}
		clone.Clear ();
	}

	public static void Buster (Vector2 center, float radius, float level)
	{
		var sqrt = radius * radius;
		var clone = new List<Person> ();
		var count = 0;
		clone.AddRange (people);
		foreach (var p in clone) {
			var delta = center - p.location;
			if (delta.x * delta.x + delta.y * delta.y < sqrt) {
				p.Buster (level);
				count++;
			}
			if (count > 20) {
				break;
			}
		}
		clone.Clear ();
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
	Vector2 location {
		get {
			var pos = transform.localPosition;
			return new Vector2 (pos.x, pos.y * 2);
		}
		set { 
			transform.localPosition = new Vector3 (value.x, value.y * 0.5f, value.y);
		}
	}

	[SerializeField]
	Image emotion;
	[SerializeField]
	Sprite up;
	[SerializeField]
	Sprite down;
	[SerializeField]
	Sprite leave;
	[SerializeField]
	Transform wave;
	[SerializeField]
	float calm = 0.015f;
	[SerializeField]
	int minSenti = 0;
	[SerializeField]
	int maxSenti = 100;

	public float sentiment = 50;

	public bool isRise {
		get {
			return sentiment > 60;
		}
	}

	public bool isNormal {
		get {
			return sentiment > 30;
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

	public void High (float level, bool handsUp)
	{
		//影响观众
		sentiment += level;
		if (handsUp) {
			HandsUp ();
		} else {
			emotion.sprite = up;
			emotion.color = Color.white;
			var fadeTime = isRise ? 2 : 0.6f;
			emotion.DOFade (0, fadeTime).SetAutoKill (true);
		}
		//add in high list
		if (isRise && !risePeople.Contains (this)) {
			animator.Play ("rise");
			RisePerson (this);
		}
	}

	void Buster (float level)
	{
		var rised = isRise;
		var normal = isNormal;

		sentiment -= level;
		//already do it in update
		//sentiment = Mathf.Clamp (sentiment, minSenti, maxSenti);
		emotion.sprite = down;
		emotion.color = Color.white;
		var fadeTime = 0.6f;
		emotion.DOFade (0, fadeTime).SetAutoKill (true);

		LowerSentiment (rised && !isRise, normal && !isNormal);
	}

	void ConfusePerson()
	{
		emotion.sprite = down;
		emotion.color = Color.white;
		emotion.DOFade (0, 0.6f).SetAutoKill (true);
		animator.Play (manKind.ToString () + "_stand");
	}
	
	public void HandsUp ()
	{
		if (!wave) {
			wave = transform.GetChild (0);
		}
		if (wave) {
			wave.GetComponent<Image> ().color = bannerColor ();
			wave.localScale = new Vector3 (1, 0, 1);
			wave.DOScaleY (1, 2f).SetAutoKill (true).OnComplete<Tween> (delegate {
				wave.DOScaleY (0, 2f).SetAutoKill (true);
			});
		}
	}

	void LowerSentiment (bool fromRise, bool fromNormal)
	{
		if (fromRise) {
			CalmPerson (this);
		}
		if (fromNormal) {
			ConfusePerson ();
		}
		if (people.Contains (this)) {
			if (sentiment == 0) {
				LeavePerson (this);
			}
		}
	}

	public Color bannerColor ()
	{
		var t2 = GameManager.speechManager.banner.texture;
		var pos = transform.localPosition;
		float locX = (pos.x / Screen.width + 0.5f);
		float locY = (pos.y / Screen.height + 0.5f);
		int x = (int)(locX * t2.width);
		int y = (int)(locY * t2.height);
		return t2.GetPixel (x, y);
	}

	void Update ()
	{
		GetComponent<Image> ().SetNativeSize ();
		if (!inSpeech) {
			var rised = isRise;
			var normal = isNormal;
			sentiment -= Time.deltaTime * calm;
			sentiment = Mathf.Clamp (sentiment, minSenti, maxSenti);
			LowerSentiment (rised && !isRise, normal && !isNormal);
		}
	}

	#endregion

}
