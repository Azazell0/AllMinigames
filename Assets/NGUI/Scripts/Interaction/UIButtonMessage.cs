//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
	}

	public GameObject target;
	public string functionName;
	public Trigger trigger = Trigger.OnClick;
	public bool includeChildren = false;

	bool mStarted = false;

	void Start () { mStarted = true; }

    void Update()
    {
        //#if (UNITY_EDITOR || UNITY_STANDALONE)

        //if (Input.GetMouseButtonUp(0) && trigger == Trigger.OnRelease)
        //    if (_isOver && !_isPressed)
        //    {
        //        Send();
        //    }

        //#endif

        //if (Input.GetMouseButtonUp(0) && trigger == Trigger.OnRelease)
        //{
        //    RaycastHit hit;
        //    Debug.Log(UICamera.currentCamera.ScreenPointToRay(Input.mousePosition));
        //    if (Physics.Raycast(UICamera.currentCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        //    {
        //        UIButtonMessage[] ui = hit.transform.GetComponents<UIButtonMessage>();
        //        foreach (UIButtonMessage u in ui)
        //            u.Send();
        //    }
        //}

        //#if (UNITY_IPHONE || UNITY_ANDROID)
        
        //if (trigger == Trigger.OnRelease && Input.touchCount > 0)
        //{
        //    Touch t = Input.touches[0];
        //    if (t.phase == TouchPhase.Ended)
        //    {
        //        RaycastHit hit;
        //        if (Physics.Raycast(UICamera.currentCamera.ScreenPointToRay(t.position), out hit, Mathf.Infinity))
        //        {
        //            UIButtonMessage[] ui = hit.transform.GetComponents<UIButtonMessage>();
        //            foreach (UIButtonMessage u in ui)
        //                u.Send();
        //        }
        //    }
        //}

        //#endif
    }

	void OnEnable () { if (mStarted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnHover (bool isOver)
	{
		if (enabled)
		{
            if (((isOver && trigger == Trigger.OnMouseOver) ||
                (!isOver && trigger == Trigger.OnMouseOut))) Send();
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (((isPressed && trigger == Trigger.OnPress)/* ||
                (!isPressed && trigger == Trigger.OnRelease)*/)) Send();

		}
	}

	void OnSelect (bool isSelected)
	{
		if (enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover(isSelected);
	}

	void OnClick () { if (enabled && trigger == Trigger.OnClick) Send(); }

	void OnDoubleClick () { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }

	public void Send ()
	{
		if (string.IsNullOrEmpty(functionName)) return;
		if (target == null) target = gameObject;

		if (includeChildren)
		{
			Transform[] transforms = target.GetComponentsInChildren<Transform>();

			for (int i = 0, imax = transforms.Length; i < imax; ++i)
			{
				Transform t = transforms[i];
				t.gameObject.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
