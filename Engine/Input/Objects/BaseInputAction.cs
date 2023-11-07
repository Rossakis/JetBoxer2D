using System;
using System.Collections.Generic;
using JetBoxer2D.Engine.Input.Enums;
using JetBoxer2D.Engine.Input.InputTypes;

namespace JetBoxer2D.Engine.Input.Objects;

public class BaseInputAction
{
    //For each BaseInputAction instance (e.g. MoveHorizontal) exists a list containing Input Dictionaries, that act as an Input
    //Map table, as in for every type of input device (keyboard, mouse, etc...) exists an Enum list containing
    //all the input Enums (Keys, Buttons, MouseInputTypes) that can trigger the BaseInputAction
    
    //A Dictionary for each InputType is created here, so that all subclasses of BaseInputAction can immediately use one of them without creating new one
    public List<(Dictionary<InputDevices, Enum>, BaseInput)> BaseInputDictionaries { get; }
    public List<(Dictionary<InputDevices, Enum>, ButtonInput)> ButtonInputDictionaries { get;}
    public List<(Dictionary<InputDevices, Enum>, AxisInput)> AxisInputDictionaries{ get; }
    public List<(Dictionary<InputDevices, Enum>, ValueInput)> ValueInputDictionaries { get; }

    protected BaseInputAction()
    {
        BaseInputDictionaries = new List<(Dictionary<InputDevices, Enum>, BaseInput)>();
        ButtonInputDictionaries = new List<(Dictionary<InputDevices, Enum>, ButtonInput)>();
        AxisInputDictionaries = new List<(Dictionary<InputDevices, Enum>, AxisInput)>();
        ValueInputDictionaries = new List<(Dictionary<InputDevices, Enum>, ValueInput)>();
    }
    public virtual void AddBaseInputToInputDictionary(Dictionary<InputDevices, Enum> inputDict, BaseInput input)
    {
        BaseInputDictionaries.Add((inputDict, input));
    }
    
    public virtual void AddButtonToInputDictionary(Dictionary<InputDevices, Enum> inputDict, ButtonInput input)
    {
        ButtonInputDictionaries.Add((inputDict, input));
    }
    
    public virtual void AddAxisToInputDictionary(Dictionary<InputDevices, Enum> inputDict, AxisInput input)
    {
        AxisInputDictionaries.Add((inputDict, input));
    }
    
    public virtual void AddValueToInputDictionary(Dictionary<InputDevices, Enum> inputDict, ValueInput input)
    {
        ValueInputDictionaries.Add((inputDict, input));
    }
}