using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour
{
	public Text peopleCountLabel;
	public Text riseCountLabel;

	const string peopleCountString = "";
	const string riseCountString = "";

	void Start ()
	{
	
	}
	
	void Update ()
	{
		peopleCountLabel.text = peopleCountString + GameManager.peopleCount;
		riseCountLabel.text = riseCountString + GameManager.riseCount;
	}
}
