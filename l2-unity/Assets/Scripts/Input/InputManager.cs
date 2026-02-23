using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    #region InputActions
    // Mouse
    private InputAction _leftClickAction;
    private InputAction _rightClickAction;
    // Camera
    private InputAction _cameraAxisAction;
    private InputAction _zoomAxisAction;
    // Movements
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _nextTargetAction;
    private InputAction _sitAction;
    // UI
    private InputAction _inventoryAction;
    private InputAction _characterStatusAction;
    private InputAction _actionsAction;
    private InputAction _closeWindowAction;
    private InputAction _systemMenuAction;
    private InputAction _validateAction;

    #endregion

    #region InputValues
    // Mouse
    public bool LeftClickDown { get; private set; }
    public bool RightClickDown { get; private set; }
    public bool RightClickUp { get; private set; }
    public bool LeftClickHeld { get; private set; }
    public bool RightClickHeld { get; private set; }

    // Camera
    public Vector2 CameraAxis { get; private set; }
    public bool CameraMoving { get; private set; }
    public bool TurnCamera { get; private set; }
    public float ZoomAxis { get; private set; }

    // Movements
    public Vector2 MoveInput { get; private set; }
    public bool Move { get; private set; }
    public bool MoveForward { get; private set; }
    public bool Jump { get; private set; }
    public bool Attack { get; private set; }
    public bool NextTarget { get; private set; }
    public bool Sit { get; private set; }

    // UI
    public bool OpenInventory { get; private set; }
    public bool OpenCharacerStatus { get; private set; }
    public bool OpenSystemMenu { get; private set; }
    public bool OpenActions { get; private set; }
    public bool CloseWindow { get; private set; }
    public bool Validate { get; private set; }
    
    public InputAction[,] SkillbarActions { get; private set; } // TODO m0nster: broke encapsulation
    
    // TODO m0nster: do not produce public access to arrays: anyone can change value inside array
    public bool[,] SkillbarInputs { get; private set; } = new bool[5, 12]; 

    #endregion

    private PlayerInput _playerInput;

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        _leftClickAction = _playerInput.actions["LeftClick"];
        _rightClickAction = _playerInput.actions["RightClick"];

        _cameraAxisAction = _playerInput.actions["CameraAxis"];
        _zoomAxisAction = _playerInput.actions["ZoomAxis"];

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _nextTargetAction = _playerInput.actions["NextTarget"];
        _attackAction = _playerInput.actions["Attack"];
        _sitAction = _playerInput.actions["Sit"];

        _inventoryAction = _playerInput.actions["Inventory"];
        _characterStatusAction = _playerInput.actions["CharacterStatus"];
        _actionsAction = _playerInput.actions["Actions"];
        _closeWindowAction = _playerInput.actions["CloseWindow"];
        _systemMenuAction = _playerInput.actions["SystemMenu"];
        _validateAction = _playerInput.actions["Validate"];

        SkillbarActions = new InputAction[5, 12];

        for (int skillbar = 1; skillbar <= 5; skillbar++)
        {
            for (int i = 1; i <= 12; i++)
            {
                SkillbarActions[skillbar - 1, i - 1] = _playerInput.actions[$"Shortcut{skillbar}-{i}"];
            }
        }

        SkillbarInputs = new bool[5, 12];
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        CameraAxis = _cameraAxisAction.ReadValue<Vector2>();
        CameraMoving = CameraAxis.y != 0 || CameraAxis.x != 0;

        LeftClickDown = _leftClickAction.WasPerformedThisFrame();
        RightClickDown = _rightClickAction.WasPerformedThisFrame();
        RightClickUp = _rightClickAction.WasReleasedThisFrame();
        LeftClickHeld = _leftClickAction.IsPressed();
        RightClickHeld = _rightClickAction.IsPressed();

        CloseWindow = _closeWindowAction.WasPerformedThisFrame();
        Validate = _validateAction.WasPerformedThisFrame();

        if (!L2GameUI.Instance.MouseOverUI)
        {
            if (RightClickHeld && CameraMoving)
            {
                TurnCamera = true;
                L2GameUI.Instance.DisableMouse();
            }

            ZoomAxis = _zoomAxisAction.ReadValue<float>();
        }

        if (RightClickUp)
        {
            TurnCamera = false;
            L2GameUI.Instance.EnableMouse();
        }

        if (!ChatWindow.Instance.ChatOpened)
        {
            MoveInput = _moveAction.ReadValue<Vector2>();
            Jump = _jumpAction.WasPerformedThisFrame();
            Attack = _attackAction.WasPerformedThisFrame();
            NextTarget = _nextTargetAction.WasPerformedThisFrame();
            Sit = _sitAction.WasPerformedThisFrame();

            OpenCharacerStatus = _characterStatusAction.WasPerformedThisFrame();
            OpenInventory = _inventoryAction.WasPerformedThisFrame();
            OpenSystemMenu = _systemMenuAction.WasPerformedThisFrame();
            OpenActions = _actionsAction.WasPerformedThisFrame();
        }
        else
        {
            MoveInput = Vector2.zero;
        }

        MoveForward = LeftClickHeld && RightClickHeld;
        Move = MoveInput.y != 0 || MoveInput.x != 0 || MoveForward;

        for (int skillbar = 0; skillbar < 5; skillbar++)
        {
            for (int i = 0; i < 12; i++)
            {
                SkillbarInputs[skillbar, i] = SkillbarActions[skillbar, i].WasPerformedThisFrame();
            }
        }
    }

}
