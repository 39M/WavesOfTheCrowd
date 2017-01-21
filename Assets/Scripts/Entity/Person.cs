using UnityEngine;
using System.Collections.Generic;

public class Person : MonoBehaviour
{
	#region Static
	public static List<Person> people=new List<Person>();
	public static List<Person> risePeople=new List<Person>();

	public static bool inSpeech=false;
	public static Vector2 mousePos;
	public static Vector2 center;
	public static Vector2 sensitive;
	public static Rect clamp;

	public static int attractLevel{get{return 5;}}
	public static int attractPeople;

	public static void RisePerson(Person person)
	{
		risePeople.Add( person );
		if( risePeople.Count/attractLevel-attractPeople>1 )
		{
			var go=new GameObject( "person "+people.Count );
			var p=go.AddComponent<Person>();
			people.Add( p );
			//TODO create anim
			attractPeople+=1;
		}
	}

	public static void High( Rect area, float level )
	{
		foreach( var p in people )
		{
			if( area.Contains( p.location ) )
			{
				p.High( level );
			}
		}
	}

	#endregion


	#region object
	[SerializeField]
	Vector2 location;
	[SerializeField]
	float calm=5f;
	[SerializeField]
	int minSenti=0;
	[SerializeField]
	int maxSenti=100;


	[HideInInspector]
	public float sentiment = 50;
	public bool isRise
	{
		get
		{
			return sentiment>80;
		}
	}

	public void SetLocation(Vector2 loc)
	{
		location=loc;
	}

	public void High(float level)
	{
		//影响观众
		sentiment+=level;
		//add in high list
		if( isRise&&!risePeople.Contains( this ) )
		{
			RisePerson(this);
		}
	}

	void Update()
	{
		if( !inSpeech )
		{
			sentiment-=Time.deltaTime*calm;
			sentiment=Mathf.Clamp( sentiment, minSenti, maxSenti);
		}
	}
	#endregion

}
