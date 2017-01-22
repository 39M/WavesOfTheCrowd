using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[RequireComponent (typeof(Animator))]
public class Joker : MonoBehaviour
{
	#region static

	public static float sensitive = 0.0001f;
	public static List<Joker> jokers = new List<Joker> ();
	public static Rect clamp = new Rect (-900, -600, 1800, 1200);
	public static Vector2 preMouse;
	public static float moveRadius = 0.1f;

	public static void Create ()
	{
		jokers = GameObject.FindObjectsOfType<Joker> ().ToList ();
		//Resources.Load<Joker>( "Entity/shuijun" );
	}

	public static void Move (Vector2 mouse)
	{
		if (GameManager.instance.status == GameManager.Status.InWave) {
			preMouse = mouse;
			return;
		}
		
		if (preMouse == Vector2.zero) {
			preMouse = mouse;
		}
		var delta = mouse - preMouse;
		if (delta.magnitude > 0.1)
			delta = Vector2.zero;

		foreach (var j in jokers) {
			var vect = new Vector2 (j.transform.position.x, j.transform.position.y);
			if (Vector2.SqrMagnitude (mouse - vect) < moveRadius) {
				j.Move (mouse, delta);
			} else {
				j.isMove = false;
			}
		}
		preMouse = mouse;
	}

	public static void High (Vector2 mouse, float radius, bool handsUp = false)
	{
		foreach (var j in jokers) {
			var vect = new Vector2 (j.transform.position.x, j.transform.position.y);
			if (Vector2.SqrMagnitude (mouse - vect) < moveRadius) {
				j.radius = radius;
				j.High (handsUp);
			} else {
				j.isRise = false;
			}
		}
	}

	public static bool Interrupt (Vector2 mouse, float radius)
	{
		bool success = false;
		foreach (var j in jokers) {
			var vect = new Vector2 (j.transform.position.x, j.transform.position.y);
			if (Vector2.SqrMagnitude (mouse - vect) < moveRadius) {
				success = true;
				j.Interrupt ();
			} else {
				j.isBreak = false;
			}
		}
		return success;
	}

	#endregion

	#region object

	public Vector2 location {
		get {
			var pos = transform.localPosition;
			return new Vector2 (pos.x, pos.y * 2);
		}
		set { 
			transform.localPosition = new Vector3 (value.x, value.y * 0.5f, value.y);
		}
	}

	public float radius = 350;
	public float high = 40;
	public bool isBreak = false;
	public bool isMove = false;
	public bool isRise = false;
	public Transform wave;

	Animator _animator;

	Animator animator {
		get {
			if (!_animator)
				_animator = GetComponent<Animator> ();
			return _animator;
		}
	}

	public void Move (Vector2 mouse, Vector2 delta)
	{
		if (!isMove) {
			isMove = true;
		}
		var distance = Vector2.SqrMagnitude (mouse - location);
		location -= delta * (1 - distance / moveRadius) * sensitive;
//		location = new Vector2 (Mathf.Clamp (location.x, clamp.xMin, clamp.xMax), Mathf.Clamp (location.y, clamp.yMin, clamp.yMax));
	}

	public void High (bool handsUp)
	{
		if (!isRise) {
			isRise = true;
			var image = GetComponent<Image> ();
			image.color = new Color (0.5f, 1f, 1f, 1);
			image.DOColor (Color.white, 1).SetAutoKill ();
			animator.Play ("rise");
			Person.High (location, radius, high, handsUp);
			if (handsUp) {
				HandsUp ();
			}
		}
	}

	public void Interrupt ()
	{
		if (!isBreak) {
			animator.Play ("break");
			isBreak = true;
		}
	}

	public void HandsUp ()
	{
		if (wave) {
			wave.GetComponent<Image> ().color = bannerColor ();
			wave.localScale = new Vector3 (1, 0, 1);
			wave.DOScaleY (1, 2f).SetAutoKill (true).OnComplete<Tween> (delegate {
				wave.DOScaleY (0, 2f).SetAutoKill (true);
			});
		}
	}

	public Color bannerColor ()
	{
		var pos = transform.position;
		var x = pos.x + Screen.width / 2;
		var y = pos.y + Screen.height / 2;
		var t2 = GameManager.speechManager.banner.texture;
		return t2.GetPixel ((int)x / Screen.width * t2.width, (int)y / Screen.height * t2.height);
	}

	#endregion

}
