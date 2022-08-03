using System;
using System.Collections.Generic;
using Script.Animation;
using Script.Enums;
using Script.Inventory;
using Script.Item;
using Script.Misc;
using Script.SceneMng;
using Script.TimeSystem;
using UnityEngine;
using EventHandler = Script.Events.EventHandler;

namespace Script.Player
{
    public class Player : SingletoneMb<Player>
    {
        //Movement Param
        private float _inputX;
        private float _inputY;
        private bool _isWalking;
        private bool _isRunning;
        private bool _isIdle;
        private bool _isCarrying = false;
        private ToolEffect _toolEffect;
        private bool _isUsingToolRight;
        private bool _isUsingToolLeft;
        private bool _isUsingToolUp;
        private bool _isUsingToolDown;
        private bool _isLiftingToolRight;
        private bool _isLiftingToolLeft;
        private bool _isLiftingToolUp;
        private bool _isLiftingToolDown;
        private bool _isPickingRight;
        private bool _isPickingLeft;
        private bool _isPickingUp;
        private bool _isPickingDown;
        private bool _isSwingingToolRight;
        private bool _isSwingingToolLeft;
        private bool _isSwingingToolUp;
        private bool _isSwingingToolDown;
        private bool _idleUp;
        private bool _idleDown;
        private bool _idleLeft;
        private bool _idleRight;

        //Animation Param
        private AnimationOverrides _animationOverrides;
        private List<CharacterAttribute> _characterAttributeList;
        [SerializeField] private SpriteRenderer _equipeditemSpriteRender = null;
         
        //Player Attribute
        private CharacterAttribute _armsAttribute;
        private CharacterAttribute _toolsAttribute;
        
        
        //Physics
        private Rigidbody2D _rigidbody2D;
        private Direction _playerDirection;
        private float _movementSpeed;

        private bool _playerInputDisable = false;

        public bool PlayerInputDisable
        {
            get => _playerInputDisable;
            set => _playerInputDisable = value;
        } 

        protected override void Awake()
        {
            base.Awake();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animationOverrides = GetComponentInChildren<AnimationOverrides>();
            _armsAttribute = new CharacterAttribute(CharacterPartAnimator.arms,PartVariantColor.none,PartVariantType.none);
            _characterAttributeList = new List<CharacterAttribute>();

        }

        private void Update()
        {
            #region Player Input

            if (!PlayerInputDisable)
            {
                ResetAnimationTrigger();
                PlayerMovementInput();
                PlayerWalkInput();
                
                EventHandler.CallMovementEvent(_inputX, _inputY, _isWalking, _isRunning, _isIdle, _isCarrying, _toolEffect,
                    _isUsingToolRight, _isUsingToolLeft, _isUsingToolUp, _isUsingToolDown,
                    _isLiftingToolRight, _isLiftingToolLeft, _isLiftingToolUp, _isLiftingToolDown,
                    _isPickingRight, _isPickingLeft, _isPickingUp, _isPickingDown,
                    _isSwingingToolRight, _isSwingingToolLeft, _isSwingingToolUp, _isSwingingToolDown,
                    _idleUp, _idleDown, _idleLeft, _idleRight);
            }
            #endregion
        }
 
        public void ClearCarriedItem()
        {
            _equipeditemSpriteRender.sprite = null;
            _equipeditemSpriteRender.color = new Color(0f, 0f, 0f, 0f);
            
            _armsAttribute.PartVariantType = PartVariantType.none;
            _characterAttributeList.Clear();
            _characterAttributeList.Add(_armsAttribute);
            _animationOverrides.ApplyCharacterCustomisationParameters(_characterAttributeList);

            _isCarrying = true;
        }

        public void ShowedCarriedItem(int itemCode)
        {
            ItemDetails details = InventoryManager.Instance.GetItemDetails(itemCode);
            if (details != null)
            {
                _equipeditemSpriteRender.sprite = details.itemSprite;
                _equipeditemSpriteRender.color = new Color(1f, 1f, 1f, 1f);

                _armsAttribute.PartVariantType = PartVariantType.carry;
                _characterAttributeList.Clear();
                _characterAttributeList.Add(_armsAttribute);
                _animationOverrides.ApplyCharacterCustomisationParameters(_characterAttributeList);

                _isCarrying = true;
            }
        }
        
        private void FixedUpdate()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            Vector2 move = new Vector2(_inputX * _movementSpeed * Time.deltaTime, _inputY * _movementSpeed * Time.deltaTime);

            _rigidbody2D.MovePosition(_rigidbody2D.position + move);
        }

        private void ResetAnimationTrigger()
        {
            _isPickingRight = false;
            _isPickingLeft = false;
            _isPickingUp = false;
            _isPickingDown = false;
            _isUsingToolRight = false;
            _isUsingToolLeft = false;
            _isUsingToolUp = false;
            _isUsingToolDown = false;
            _isLiftingToolRight = false;
            _isLiftingToolLeft = false;
            _isLiftingToolUp = false;
            _isLiftingToolDown = false;
            _isSwingingToolRight = false;
            _isSwingingToolLeft = false;
            _isSwingingToolUp = false;
            _isSwingingToolDown = false;
            _toolEffect = ToolEffect.None;
        }
        
        private void PlayerMovementInput()
        {
            _inputY = Input.GetAxisRaw("Vertical");
            _inputX = Input.GetAxisRaw("Horizontal");

            if (_inputY != 0 && _inputX != 0)
            {
                _inputX = _inputX * 0.71f;
                _inputY = _inputY * 0.71f;
            }

            if (_inputX != 0 || _inputY != 0)
            {
                _isRunning = true;
                _isWalking = false;
                _isIdle = false;
                _movementSpeed = Settings.RunningSpeed;

                // Capture player direction for save game
                if (_inputX < 0)
                {
                    _playerDirection = Direction.Left;
                }
                else if (_inputX > 0)
                {
                    _playerDirection = Direction.Right;
                }
                else if (_inputY < 0)
                {
                    _playerDirection = Direction.Down;
                }
                else
                {
                    _playerDirection = Direction.Up;
                }
            }
            else if (_inputX == 0 && _inputY == 0)
            {
                _isRunning = false;
                _isWalking = false;
                _isIdle = true;
            }
        }
        
        private void PlayerWalkInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _isRunning = false;
                _isWalking = true;
                _isIdle = false;
                _movementSpeed = Settings.WalkingSpeed;
            }
            else
            {
                _isRunning = true;
                _isWalking = false;
                _isIdle = false;
                _movementSpeed = Settings.RunningSpeed;
            }
        }

        public void DisablePlayerInputAndRestMovement()
        {
            DisablePlayerInput();
            ResetMovement();
            
            EventHandler.CallMovementEvent(_inputX, _inputY, _isWalking, _isRunning, _isIdle, _isCarrying, _toolEffect,
                _isUsingToolRight, _isUsingToolLeft, _isUsingToolUp, _isUsingToolDown,
                _isLiftingToolRight, _isLiftingToolLeft, _isLiftingToolUp, _isLiftingToolDown,
                _isPickingRight, _isPickingLeft, _isPickingUp, _isPickingDown,
                _isSwingingToolRight, _isSwingingToolLeft, _isSwingingToolUp, _isSwingingToolDown,
                _idleUp, _idleDown, _idleLeft, _idleRight);

        }

        private void ResetMovement()
        {
            _inputX = 0f;
            _inputY = 0f;
            _isRunning = false;
            _isWalking = false;
            _isIdle = true;
        }

        public void EnablePlayerInput()
        {
            PlayerInputDisable = false;
        }

        public void DisablePlayerInput()
        {
            PlayerInputDisable = true;
        }
    }

}
