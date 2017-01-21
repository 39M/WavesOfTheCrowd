using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
		if (preMouse == Vector2.zero) {
			preMouse = mouse;
		}
		var delta = mouse - preMouse;

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

	public static void High (Vector2 mouse, float radius)
	{
		foreach (var j in jokers) {
			var vect = new Vector2 (j.transform.position.x, j.transform.position.y);
			if (Vector2.SqrMagnitude (mouse - vect) < moveRadius) {
				j.High ();
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
		get{
			var pos = transform.localPosition;
			return new Vector2 (pos.x, pos.y * 2); }
		set { 
			transform.localPosition = new Vector3 (value.x, value.y * 0.5f, 0);
		}
	}

	public Vector2 size = new Vector2 (100, 100);
	public float high = 10;
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
		location = new Vector2 (Mathf.Clamp (location.x, clamp.xMin, clamp.xMax), Mathf.Clamp (location.y, clamp.yMin, clamp.yMax));
	}

	public void High ()
	{
		if (!isRise) {
			isRise = true;
			OnMouseDown ();
		}
	}

	public void Interrupt ()
	{
		if (!isBreak) {
			animator.Play ("break");
			isBreak = true;
		}
	}

	public void OnMouseDown ()
	{
		animator.Play ("rise");
		wave.DOScale (4, 1).SetAutoKill (true).OnComplete<Tween> (delegate {
			wave.localScale = Vector3.zero;
		});
		var rect = new Rect ();
		rect.position = location - size / 2;
		rect.size = size;
		Person.High (rect, high);
	}

	#endregion

}
