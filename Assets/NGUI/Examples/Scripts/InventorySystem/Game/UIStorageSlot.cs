//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// A UI script that keeps an eye on the slot in a storage container.
/// </summary>

[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	public UIItemStorage storage;
	public int slot = 0;

	override protected InvGameItem observedItem
	{
		get
		{
			return (storage != null) ? storage.GetItem(slot) : null;
		}
	}

	/// <summary>
	/// Replace the observed item with the specified value. Should return the item that was replaced.
	/// </summary>

	override protected InvGameItem Replace (bool b)
	{
        //return (storage != null) ? storage.Replace(slot, item) : item;
        Debug.Log(b);
        if (b)
        {
            //if (UICursor.mInstance.mAtlas != null)
            //{
             //   Debug.Log("AA");
                icon.atlas = UICursor.mInstance.mSprite.atlas;
                //if (UICursor.mInstance.mSprite != null)
                    icon.spriteName = UICursor.mInstance.mSprite.spriteName;
            //}
        }
        else
        {
            icon.atlas = null;
            icon.spriteName = null;
        }
        return null;
	}
}