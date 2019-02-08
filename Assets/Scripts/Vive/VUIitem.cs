using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class VUIitem : MonoBehaviour
{
	private BoxCollider boxCollider;
	private RectTransform rectTransform;

	private void OnEnable()
	{
		ValidateCollider();
	}

	private void OnValidate()
	{
		ValidateCollider();
	}

	private void ValidateCollider()
	{
		rectTransform = GetComponent<RectTransform>();

		boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider>();
		}

		boxCollider.size = rectTransform.sizeDelta;
	}

	//void OnTriggerEnter(Collider col) {
	//	if (col.gameObject.tag == "controller") {
	//		gameObject.GetComponent<Button> ().onClick.Invoke ();
	//	}
	//}
}
