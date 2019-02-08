using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LaserPointer))]
public class LaserInput: MonoBehaviour
{
	private LaserPointer laserPointer;
	private SteamVR_TrackedController trackedController;

    private Transform lastTarget;

    private float padYval = 0;

    GameObject currentTarget;

	private void OnEnable()
	{
		laserPointer = GetComponent<LaserPointer>();
		laserPointer.PointerIn -= HandlePointerIn;
		laserPointer.PointerIn += HandlePointerIn;
		laserPointer.PointerOut -= HandlePointerOut;
		laserPointer.PointerOut += HandlePointerOut;

		trackedController = GetComponent<SteamVR_TrackedController>();
		if (trackedController == null)
		{
			trackedController = GetComponentInParent<SteamVR_TrackedController>();
		}
		trackedController.TriggerClicked -= HandleTriggerClicked;
		trackedController.TriggerClicked += HandleTriggerClicked;

        trackedController.PadClicked -= HandlePadClicked;
        trackedController.PadClicked += HandlePadClicked;

        trackedController.PadTouched -= HandlePadTouched;
        trackedController.PadTouched += HandlePadTouched;

    }


    void HandlePadTouched(object sender, ClickedEventArgs e)
    {
        padYval = e.padY;
        Debug.Log("Pad touched, Y value is: " + e.padY);
    }

	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
        if (lastTarget == null) return;

		if (EventSystem.current.currentSelectedGameObject != null)
		{
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            return;
		}

        var flatViewScript = lastTarget.GetComponent<FlatViewLaserDetection>();
        if (flatViewScript != null)
        {
            flatViewScript.Go();
            return;
        }

        var farStarScript = lastTarget.GetComponent<FarStarLaserDetection>();
        if (farStarScript != null) {
            farStarScript.Go();
            return;
        }

        var sunScript = lastTarget.GetComponent<SunLaserDetection>();
        if (sunScript != null) {
            sunScript.SpawnUiInfo();
            return;
        }

    }

    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        if (lastTarget == null) return;
        Debug.Log("Pad Clicked.");
        try
        {
            var sunScript = lastTarget.GetComponent<SunLaserDetection>();
            if (sunScript != null)
            {
                sunScript.RemoveSystem();
                return;
            }
        }
        catch (System.Exception)
        {

            throw;
        }


        try
        {
            var flatViewScript = lastTarget.GetComponent<FlatViewLaserDetection>();
            if (flatViewScript != null)
            {
                flatViewScript.Delete();
                return;
            } else
            {
                Debug.LogWarning("flatViewSCript was null!");
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning("flatViewSCript was null!");
            throw;
        }
    }

	private void HandlePointerIn(object sender, PointerEventArgs e)
	{
        var scrl = e.target.GetComponent<ScrollRect>();
        if (scrl != null)
        {
            Debug.Log("ScrollRect hit by laser.");
            scrl.verticalNormalizedPosition += padYval;
        }

        lastTarget = e.target;
		var button = e.target.GetComponent<Button>();
		if (button != null)
		{
			button.Select();
            return;
			//Debug.Log("HandlePointerIn", e.target.gameObject);
		}
	}

	private void HandlePointerOut(object sender, PointerEventArgs e)
	{

        lastTarget = null;

		var button = e.target.GetComponent<Button>();
		if (button != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
			//Debug.Log("HandlePointerOut", e.target.gameObject);
		}

    }
}