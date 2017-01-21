using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CursorManager : MonoBehaviour
{
	public static Canvas canvas;

	public static Vector2 mousePosition {
		get {
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
			return canvas.transform.TransformPoint (pos);
		}
	}

	public static float cursorSize;

	bool showCircle;

	public Texture2D hand;

	RectTransform rectTransform;

	Image circle;

	void Start ()
	{
		canvas = GameManager.canvas;
		rectTransform = GetComponent<RectTransform> ();
		circle = GetComponent<Image> ();
		circle.enabled = false;
		Cursor.SetCursor (hand, Vector2.zero, CursorMode.Auto);
	}

	void Update ()
	{
		if (showCircle) {
			transform.position = mousePosition;

			rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, cursorSize);
			rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, cursorSize);
		}
	}

	public void ShowCircle ()
	{
		showCircle = true;
		Cursor.visible = false;
		circle.enabled = true;
	}

	public void HideCircle ()
	{
		showCircle = false;
		Cursor.visible = true;
		circle.enabled = false;
		transform.position = Vector2.up * 10000;
	}
}
