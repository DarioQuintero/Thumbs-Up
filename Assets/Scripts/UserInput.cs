using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    // public static UserInput instance;
    public bool highJustPressed { get; private set; }
    public bool highBeingHeld { get; private set; }
    public bool midJustPressed { get; private set; }
    public bool midBeingHeld { get; private set; }
    public bool blockBeingHeld { get; private set; }
    public bool leftBeingHeld { get; private set; }
    public bool rightBeingHeld { get; private set; }

    public bool upJustPressed { get; private set; }
    public bool downJustPressed { get; private set; }


    private PlayerInput _playerInput;
    public int controllerIndex;
    private InputAction _highAction;
    private InputAction _midAction;
    private InputAction _blockAction;
    private InputAction _leftAction;
    private InputAction _rightAction;

    private InputAction _upInput;
    private InputAction _downInput;
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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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

        _upInput = _playerInput.actions["Up"];
        _downInput = _playerInput.actions["Down"];

    }

    private void UpdateInputs()
    {
        highJustPressed = _highAction.WasPressedThisFrame();
        highBeingHeld = _highAction.IsPressed();
        midJustPressed = _midAction.WasPressedThisFrame();
        midBeingHeld = _midAction.IsPressed();
        blockBeingHeld = _blockAction.IsPressed();
        leftBeingHeld = _leftAction.IsPressed();
        rightBeingHeld = _rightAction.IsPressed();

        upJustPressed = _upInput.WasPressedThisFrame();
        downJustPressed = _downInput.WasPressedThisFrame();

        if (rightBeingHeld) {
            print("right");
        }
        if (upJustPressed) {
            print("up");
        }
        if (downJustPressed) {
            print("down");
        }
    }
}
