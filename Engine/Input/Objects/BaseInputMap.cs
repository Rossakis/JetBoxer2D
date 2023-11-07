using System;
using System.Collections.Generic;
using JetBoxer2D.Engine.Input.Enums;

namespace JetBoxer2D.Engine.Input.Objects;

public abstract class BaseInputMap
{
    /// <summary>
    /// Assign input Keys/Buttons/MouseInputTypes to an Input Action Dictionary. For each <c>InputAction</c> (e.g. EnterGame) exists a dictionary, where for each <c>Key</c> <see cref="InputDevices"/>
    /// exists an <c>Enum</c> List containing all the inputs that can trigger the input action
    /// E.g. {Keys.A, Keys.Left} for InputDevices.Keyboard, or {Buttons.LeftTrigger} for InputDevices.Gamepad)
    /// </summary>
    protected void InitializeInputActionDictionary(Dictionary<InputDevices, Enum> inputActionDict, InputDevices inputDevice,
       Enum inputButtons)
    {
        inputActionDict.TryAdd(inputDevice, inputButtons);
    }

    public virtual void UpdateInput()
    {
        UpdateKeyboardInput();
        UpdateGamepadInput();
        UpdateMouseInput();
    }
    protected virtual void UpdateKeyboardInput() {}
    protected virtual void UpdateMouseInput() {}
    protected virtual void UpdateGamepadInput() {}
    public abstract void RemapInputAction(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputType, List<Enum> newInput);
}