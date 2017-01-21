using UnityEngine;
using System.Collections.Generic;

public class Joker : MonoBehaviour
{
	#region static
	public static Vector2 center;
	public static Vector2 sensitive;
	public static List<Joker> jokers=new List<Joker>();
	public static Rect clamp;
	public static Vector2 preMouse;
	public static float moveRadius = 100;
	public static void Move(Vector2 mouse)
	{
		if( preMouse==Vector2.zero )
		{
			preMouse=mouse;
		}
		var delta=mouse-preMouse;

		foreach( var j in jokers )
		{
			if( Vector2.SqrMagnitude( mouse-center-j.location )<moveRadius )
			{
				j.Move( mouse, delta );
			}
		}
	}

	public static void High(Vector2 mouse, float radius)
	{
		foreach( var j in jokers )
		{
			if( Vector2.SqrMagnitude( mouse-center-j.location )<radius )
			{
				j.High();
			}
		}
	}
	#endregion

	#region object
	public Vector2 location;
	public Vector2 size=new Vector2( 10, 10 );
	public float high=10;
	public bool isRise =false;

	public void Move( Vector2 mouse, Vector2 delta )
	{
		var distance=Vector2.SqrMagnitude( mouse-location );
		location+=delta*( 1-distance/moveRadius );
		location.x=Mathf.Clamp( location.x, clamp.xMin, clamp.xMax );
		location.y=Mathf.Clamp( location.y, clamp.yMin, clamp.yMax );
	}

	public void High()
	{
		OnMouseDown();
	}

	public void OnMouseDown()
	{
		var rect=new Rect();
		rect.position=center+location-size/2;
		rect.size=size;
		Person.High( rect, high );
	}
	#endregion

}
