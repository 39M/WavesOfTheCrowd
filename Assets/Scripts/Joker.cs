using UnityEngine;
using System.Collections.Generic;

public class Joker : MonoBehaviour 
{
	public static Vector2 center;
	public static Vector2 sensitive;
	public static List<Joker> jokers=new List<Joker>();
	public static Rect clamp;

	public static void Move()
	{
		var hori =Input.GetAxis( "Horizontal" );
 		var velt =Input.GetAxis( "Veltical" ); 
		if( hori!=0 )
		{
			center.x+=hori*sensitive.x;
		}
		if( velt!= 0)
		{
			center.y+=velt*sensitive.y;
		}
		center.x=Mathf.Clamp( center.x, clamp.xMin, clamp.xMax );
		center.y=Mathf.Clamp( center.y, clamp.yMin, clamp.yMax );
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


	public Vector2 location;
	public Vector2 size=new Vector2( 10, 10 );
	public float high=10;

	public void High()
	{
	
	}

	public void OnMouseDown()
	{
		var rect=new Rect();
		rect.position=center+location-size/2;
		rect.size=size;
		Person.High( rect, high );
	}

}
