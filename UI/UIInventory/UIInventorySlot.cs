using System;
using Script.Enums;
using Script.Inventory;
using Script.Item;
using Script.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Script.Player;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using EventHandler = Script.Events.EventHandler;


namespace Script.UI.UIInventory
{
    public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        private Camera _mainCamera;
        private Transform _parentItem;
        private GameObject _draggedItem;
        private Canvas _parentCanvas;


        public Image inventorySlotHighLight;
        public Image inventorySlotImage;
        public TextMeshProUGUI slotText;

        [SerializeField] private UIInventoryBar inventoryBar;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int slotNumber = 0;

        [HideInInspector] public ItemDetails itemDetails;
        [HideInInspector] public int itemQuantity;
        [HideInInspector] public bool isSelected = false; 

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += SceneLoaded;
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= SceneLoaded;

        }
        

        private void Start()
        {
            _mainCamera = Camera.main;
        }
        
        private void SceneLoaded()
        {
            _parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemDetails != null)
            {
                Player.Player.Instance.DisablePlayerInput();

                _draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);
                _draggedItem.GetComponentInChildren<Image>().sprite= inventorySlotImage.sprite;
                
                SetSelectedItem();

            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
                _draggedItem.transform.position = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
               Destroy(_draggedItem);

               if (eventData.pointerCurrentRaycast.gameObject != null &&
                   eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
               {
                   int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>()
                       .slotNumber;
                   InventoryManager.Instance.SwapInventoryItem(InventoryLocation.Player, slotNumber, toSlotNumber);
                   
                   ClearSelectedItem();
               }
               else
               {
                   if (itemDetails.canBeDropped)
                   {
                       DropSelectedItem();
                   }
               }
               Player.Player.Instance.EnablePlayerInput();
            }
        }

        private void DropSelectedItem()
        {
            if (itemDetails != null && isSelected)
            {
                Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -_mainCamera.transform.position.z));
                GameObject itemGameObject = Instantiate(itemPrefab,worldPosition , Quaternion.identity, _parentItem);
                Item.Item item = itemGameObject.GetComponent<Item.Item>();
                item.itemCode = itemDetails.itemCode;

                InventoryManager.Instance.RemoveFromList(InventoryLocation.Player,item.itemCode);

                if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.Player, item.itemCode) == -1)
                {
                    ClearSelectedItem();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemQuantity != 0)
            {
                inventoryBar.inventoryTextBoxGameObject = Instantiate(inventoryBar.inventoryTextBoxPrefab,
                    transform.position, Quaternion.identity);

                inventoryBar.inventoryTextBoxGameObject.transform.SetParent(inventoryBar.transform,false);
                UIInventoryTextBox uiInventoryTextBox =
                    inventoryBar.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();

                string itemDes = InventoryManager.Instance.GetItemDescription(itemDetails.itemType);
                uiInventoryTextBox.SetTextboxText(itemDetails.itemDescription,itemDes,"",itemDetails.itemLongDescription,"","");
                
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 65f, transform.position.z);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DestroyInventoryTextBox();
        }

        private void DestroyInventoryTextBox()
        {
            if (inventoryBar.inventoryTextBoxGameObject != null)
            {
                Destroy(inventoryBar.inventoryTextBoxGameObject);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (isSelected == true)
                {
                    ClearSelectedItem();
                }
                else
                {
                    if (itemQuantity > 0)
                    {
                        SetSelectedItem();
                    }
                }
            }

            
        }

        private void ClearSelectedItem()
        {
            inventoryBar.ClearHighlightInventorySlot();
            isSelected = false;
            
            InventoryManager.Instance.ClearInventoryLocation(InventoryLocation.Player);
            Player.Player.Instance.ClearCarriedItem();
        }
        private void SetSelectedItem()
        {
            inventoryBar.ClearHighlightInventorySlot();
            isSelected = true;
            inventoryBar.SetHighlightedInventorySlot();
            // inventorySlotHighLight.color
            InventoryManager.Instance.SelectedInventoryItem(InventoryLocation.Player,itemDetails.itemCode);
            CarriedItemOption();
        }

        private void CarriedItemOption()
        {
            if (itemDetails.canBeCarried == true)
            {
                Player.Player.Instance.ShowedCarriedItem(itemDetails.itemCode);
            }
            else
            {
                Player.Player.Instance.ClearCarriedItem();
            }
        }
    }
}