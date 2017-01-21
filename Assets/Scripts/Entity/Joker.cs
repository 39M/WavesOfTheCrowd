using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent( typeof( Animator ) )]
public class Joker : MonoBehaviour
{
	#region static
	public static Vector2 sensitive;
	public static List<Joker> jokers=new List<Joker>();
	public static Rect clamp;
	public static Vector2 preMouse;
	public static float moveRadius = 100;

	public static void Create()
	{
		Resources.Load<Joker>( "Entity/shuijun.prefab" );
	}

	public static void Move(Vector2 mouse)
	{
		if( preMouse==Vector2.zero )
		{
			preMouse=mouse;
		}
		var delta=mouse-preMouse;

		foreach( var j in jokers )
		{
			if( Vector2.SqrMagnitude( mouse-j.location )<moveRadius )
			{
				j.Move( mouse, delta );
			}
			else
			{
				j.isMove=false;
			}
		}
	}

	public static void High(Vector2 mouse, float radius)
	{
		foreach( var j in jokers )
		{
			if( Vector2.SqrMagnitude( mouse-j.location )<radius )
			{
				j.High();
			}
			else
			{
				j.isRise=false;	
			}
		}
	}

	public static bool Interrupt( Vector2 mouse, float radius )
	{
		bool success=false;
		foreach( var j in jokers )
		{
			if( Vector2.SqrMagnitude( mouse-j.location )<radius )
			{
				success=true;
				j.Interrupt();
			}
			else
			{
				j.isBreak=false;
			}
		}
		return success;
	}
	#endregion

	#region object
	public Vector2 location;
	public Vector2 size=new Vector2( 10, 10 );
	public float high=10;
	public bool isBreak=false;
	public bool isMove=false;
	public bool isRise=false;
	public Transform wave;

	Animator _animator;
	Animator animator
	{
		get
		{
			if( !_animator )
				_animator=GetComponent<Animator>();
			return _animator;
		}
	}

	public void Move( Vector2 mouse, Vector2 delta )
	{
		if( !isMove )
		{
			isMove=true;
			animator.Play( "walk" );
		}
		var distance=Vector2.SqrMagnitude( mouse-location );
		location+=delta*( 1-distance/moveRadius );
		location.x=Mathf.Clamp( location.x, clamp.xMin, clamp.xMax );
		location.y=Mathf.Clamp( location.y, clamp.yMin, clamp.yMax );
	}

	public void High()
	{
		if( !isRise )
		{
			isRise=true;
			OnMouseDown();
		}
	}

	public void Interrupt()
	{
		if( !isBreak )
		{
			animator.Play( "break" );
			isBreak=true;
		}
	}

	public void OnMouseDown()
	{
		animator.Play( "rise" );
		wave.DOScale( 4, 1 ).SetAutoKill( true ).OnComplete<Tween>( delegate
		{
			wave.localScale=Vector3.zero;
		} );
		var rect=new Rect();
		rect.position=location-size/2;
		rect.size=size;
		Person.High( rect, high );
	}
	#endregion

}
