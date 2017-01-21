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
		peopleCountLabel.text = peopleCountString + GameManager.instance.peopleCount;
		riseCountLabel.text = riseCountString + GameManager.instance.riseCount;
	}
}
