using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
/// <summary>
/// Drag and Drop item.
/// </summary>
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static bool dragDisabled = false;										// Drag start global disable

	public static DragAndDropItem draggedItem;                                      // Item that is dragged now
	public static GameObject icon;                                                  // Icon of dragged item
	public static DragAndDropCell sourceCell;                                       // From this cell dragged item is

	public delegate void DragEvent(DragAndDropItem item);
	public static event DragEvent OnItemDragStartEvent;                             // Drag start event
	public static event DragEvent OnItemDragEndEvent;                               // Drag end event

	private static Canvas canvas;                                                   // Canvas for item drag operation
	private static string canvasName = "DragAndDropCanvas";                   		// Name of canvas
	private static int canvasSortOrder = 105;										// Sort order for canvas

    public int positionOnActionBar;

    public AudioClip dragSound;
    public AudioClip dropUiSound;


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
	{
		if (canvas == null)
		{
			GameObject canvasObj = new GameObject(canvasName);
			canvas = canvasObj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = canvasSortOrder;
		}
	}

	/// <summary>
	/// This item started to drag.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (dragDisabled == false && !GameLibOfMethods.doingSomething && GameLibOfMethods.canInteract && !GameLibOfMethods.cantMove)
		{
			sourceCell = GetCell();                       							// Remember source cell
			draggedItem = this;                                             		// Set as dragged item
			// Create item's icon
			icon = new GameObject();
			
			icon.name = "Icon";
			Image myImage = GetComponentsInChildren<Image>()[1];
            icon.transform.SetParent(canvas.transform);
            myImage.raycastTarget = false;                                        	// Disable icon's raycast for correct drop handling
			Image iconImage = icon.AddComponent<Image>();
			iconImage.raycastTarget = false;
			iconImage.sprite = myImage.sprite;
			RectTransform iconRect = icon.GetComponent<RectTransform>();
			// Set icon's dimensions
			RectTransform myRect = GetComponent<RectTransform>();
			iconRect.pivot = new Vector2(0.5f, 0.5f);
			iconRect.anchorMin = new Vector2(0.5f, 0.5f);
			iconRect.anchorMax = new Vector2(0.5f, 0.5f);
			iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);
            iconImage.preserveAspect = true;
            icon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

			if (OnItemDragStartEvent != null)
			{
				OnItemDragStartEvent(this);                                			// Notify all items about drag start for raycast disabling
			}
            AtommInventory.Instance.SpawnFX(dragSound);
        }
        else
        {
            GameLibOfMethods.CreateFloatingText("You are busy, can't rearrange inventory now!", 2);
        }
	}

	/// <summary>
	/// Every frame on this item drag.
	/// </summary>
	/// <param name="data"></param>
	public void OnDrag(PointerEventData data)
	{
       
            if (icon != null && !GameLibOfMethods.doingSomething && GameLibOfMethods.canInteract && !GameLibOfMethods.cantMove)
		{
			icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
        }
        else
        {
            ResetConditions();
        }
	}

	/// <summary>
	/// This item is dropped.
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
        if (dragDisabled == false)
        {
            positionOnActionBar = transform.parent.GetSiblingIndex();
            Debug.Log("ended item drag");
            if (eventData.hovered.Count == 0 && draggedItem.GetCell().cellType != DragAndDropCell.CellType.DropOnly)
            {
                AtommInventory.Slot tempSlot = AtommInventory.inventory.Where(obj => obj.ID == GetComponent<ItemSlot>().ID).FirstOrDefault();
                AtommInventory.DropItem(tempSlot);
            }
            else
            {
                AtommInventory.Instance.SpawnFX(dropUiSound);
            }
            ResetConditions();
        }
        else
        {
            GameLibOfMethods.CreateFloatingText("You are busy, can't rearrange inventory now!", 2);
            
            ResetConditions();
        }

    }

	/// <summary>
	/// Resets all temporary conditions.
	/// </summary>
	public void ResetConditions()
	{
		if (icon != null)
		{
			Destroy(icon);                                                          // Destroy icon on item drop
		}
		if (OnItemDragEndEvent != null)
		{
			OnItemDragEndEvent(this);                                       		// Notify all cells about item drag end
		}
		draggedItem = null;
		icon = null;
		sourceCell = null;
	}

	/// <summary>
	/// Enable item's raycast.
	/// </summary>
	/// <param name="condition"> true - enable, false - disable </param>
	public void MakeRaycast(bool condition)
	{
		Image image = GetComponent<Image>();
		if (image != null)
		{
			image.raycastTarget = condition;
		}
	}

	/// <summary>
	/// Gets DaD cell which contains this item.
	/// </summary>
	/// <returns>The cell.</returns>
	public DragAndDropCell GetCell()
	{
		return GetComponentInParent<DragAndDropCell>();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		ResetConditions();
	}
}
