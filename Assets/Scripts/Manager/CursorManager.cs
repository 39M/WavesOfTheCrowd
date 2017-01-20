using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CursorManager : MonoBehaviour
{
	public static float cursorSize;

	bool show;

	public Canvas canvas;

	RectTransform rectTransform;

	Image cursorImage;

	void Start ()
	{
		rectTransform = GetComponent<RectTransform> ();
		cursorImage = GetComponent<Image> ();
		cursorImage.enabled = false;
	}

	void Update ()
	{
		if (show) {
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
			transform.position = canvas.transform.TransformPoint (pos);

			rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, cursorSize);
			rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, cursorSize);
		}
	}

	public void ShowCursor ()
	{
		show = true;
		cursorImage.enabled = true;
	}

	public void HideCursor ()
	{
		show = false;
		cursorImage.enabled = false;
	}
}
