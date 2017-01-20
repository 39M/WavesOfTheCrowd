using UnityEngine;
using System.Collections.Generic;

public class Person : MonoBehaviour 
{
	public static bool inSpeech=false;
	public static List<Person> people=new List<Person>();

	public static void AddPerson(Vector2 location)
	{
		var go=new GameObject( "person" );
		var p=go.AddComponent<Person>();
		p.SetLocation( location );
		people.Add( p );
	}

	public static void High(Rect area,float level)
	{
		foreach(var p in people)
		{
			if( area.Contains( p.location ) )
			{
				p.High( level );
			}
		}
	}

	[SerializeField]
	Vector2 location;
	[HideInInspector]
	public float sentiment = 0;
	[HideInInspector]
	public float attraction=0;
	public float maxAttraction=30;
	public float calm = 1f;

	public void SetLocation(Vector2 loc)
	{
		location=loc;
	}

	public void High(float level)
	{
		//影响观众
		sentiment+=level;
		attraction+=level;
		// 吸引观众
		if( attraction>=maxAttraction )
		{
			var loc=location
				+Vector2.up*Random.Range( 1, 10 )
				+Vector2.right*Random.Range( 1, 10 );
			AddPerson(loc);
			attraction-=maxAttraction;
		}
		
	}

	void Update()
	{
		if( !inSpeech )
		{
			attraction-=Time.deltaTime*calm;
		}
	}

}
