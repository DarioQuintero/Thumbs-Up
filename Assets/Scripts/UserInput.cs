using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    // public static UserInput instance;
    public bool highJustPressed { get; private set; }
    public bool midJustPressed { get; private set; }
    public bool blockBeingHeld { get; private set; }
    public bool leftBeingHeld { get; private set; }
    public bool rightBeingHeld { get; private set; }

    private PlayerInput _playerInput;
    public int controllerIndex;
    private InputAction _highAction;
    private InputAction _midAction;
    private InputAction _blockAction;
    private InputAction _leftAction;
    private InputAction _rightAction;
    private void Awake()
    {
        /*
        if (instance == null) {
            instance = this;
        }
        */
        _playerInput = GetComponent<PlayerInput>();
        controllerIndex = _playerInput.playerIndex;
        SetupInputActions();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateInputs();
    }

    private void SetupInputActions() 
    {
        _highAction = _playerInput.actions["High"];
        _midAction = _playerInput.actions["Mid"];
        _blockAction = _playerInput.actions["Block"];
        _leftAction = _playerInput.actions["Left"];
        _rightAction = _playerInput.actions["Right"];
    }

    private void UpdateInputs()
    {
        highJustPressed = _highAction.WasPressedThisFrame();
        midJustPressed = _midAction.WasPressedThisFrame();
        blockBeingHeld = _blockAction.IsPressed();
        leftBeingHeld = _leftAction.IsPressed();
        rightBeingHeld = _rightAction.IsPressed();
    }
}
