using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.InputSystem.Base;
using Super_Duper_Shooter.Classes.InputSystem.InputTypes;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.InputSystem.InputMaps
{
    //Input Actions
    public class  MoveHorizontal : BaseInputAction
    {
        public AxisInput InputAction { get; private set; }
        public MoveHorizontal(AxisInput inputAction)
        {
            InputAction = inputAction;
        }
        public readonly Dictionary<InputDevices, List<Enum>> MoveHorLeftDict = new();
        public readonly Dictionary<InputDevices, List<Enum>> MoveHorRightDict = new();
    }

    public class MoveVertical : BaseInputAction
    {
        public AxisInput InputAction { get; private set; }
        public MoveVertical(AxisInput inputAction)
        {
            InputAction = inputAction;
        }
        public readonly Dictionary<InputDevices, List<Enum>> MoveVertUpDict = new(); 
        public readonly Dictionary<InputDevices, List<Enum>> MoveVertDownDict = new();
    }

    public class ShootLeft : BaseInputAction
    {
        public ButtonInput InputAction { get; private set; }

        public ShootLeft(ButtonInput inputAction)
        {
            InputAction = inputAction;
        }
        public readonly Dictionary<InputDevices, List<Enum>> ShootLeftDict = new(); 
    }

    public class ShootRight : BaseInputAction
    {
        public ButtonInput InputAction { get; private set; }

        public ShootRight(ButtonInput inputAction)
        {
            InputAction = inputAction;
        }
        public readonly Dictionary<InputDevices, List<Enum>> ShootRightDict = new();
    }
    
    public class GameplayInputMap : BaseInputMap
    {
        public static MoveHorizontal MoveHorizontal;
        public static MoveVertical MoveVertical;
        public static ShootLeft ShootLeft;
        public static ShootRight ShootRight;

        public GameplayInputMap()
        {
            InitializeInputActions();
            InitializeInputActionDictionaries();
        }

        //Assign the default Input keys/buttons
        private void InitializeInputActions()
        {
            MoveHorizontal = new MoveHorizontal(new AxisInput());
            MoveVertical = new MoveVertical(new AxisInput());
            ShootLeft = new ShootLeft(new ButtonInput());
            ShootRight = new ShootRight(new ButtonInput());
        }
        
        private void InitializeInputActionDictionaries()//Each Input Action Dictionary contains the InputActions table as values for each type of input
        {
            //Move Horizontal
            InitializeInputActionDictionary(MoveHorizontal.MoveHorLeftDict, InputDevices.Keyboard, new List<Enum>{Keys.A, Keys.Left});
            InitializeInputActionDictionary(MoveHorizontal.MoveHorLeftDict, InputDevices.Gamepad, new List<Enum>{Buttons.LeftThumbstickLeft});
            InitializeInputActionDictionary(MoveHorizontal.MoveHorRightDict, InputDevices.Keyboard, new List<Enum>{Keys.D, Keys.Right});
            InitializeInputActionDictionary(MoveHorizontal.MoveHorRightDict, InputDevices.Gamepad, new List<Enum>{Buttons.LeftThumbstickRight});
            
            //Move Vertical
            InitializeInputActionDictionary(MoveVertical.MoveVertUpDict, InputDevices.Keyboard, new List<Enum>{Keys.W, Keys.Up});
            InitializeInputActionDictionary(MoveVertical.MoveVertUpDict, InputDevices.Gamepad, new List<Enum>{Buttons.LeftThumbstickUp});
            InitializeInputActionDictionary(MoveVertical.MoveVertDownDict, InputDevices.Keyboard, new List<Enum>{Keys.S, Keys.Down});
            InitializeInputActionDictionary(MoveVertical.MoveVertDownDict, InputDevices.Gamepad, new List<Enum>{Buttons.LeftThumbstickDown});
            
            //Shoot Left
            InitializeInputActionDictionary(ShootLeft.ShootLeftDict, InputDevices.Mouse, new List<Enum>{MouseInputTypes.LeftButton});
            InitializeInputActionDictionary(ShootLeft.ShootLeftDict, InputDevices.Gamepad, new List<Enum>{Buttons.LeftTrigger});
            
            //Shoot Right
            InitializeInputActionDictionary(ShootRight.ShootRightDict, InputDevices.Mouse, new List<Enum>{MouseInputTypes.RightButton});
            InitializeInputActionDictionary(ShootRight.ShootRightDict, InputDevices.Gamepad, new List<Enum>{Buttons.RightTrigger});
        }

        #region Input Dictionary Finders

        private List<Enum> GetMoveLeftInput(InputDevices inputType)
        {
            // Check if the key exists in the dictionary
            if (MoveHorizontal.MoveHorLeftDict.TryGetValue(inputType, out var value))
                return value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{inputType} not found in {MoveHorizontal} dictionary");
        }

        private List<Enum> GetMoveRightInput(InputDevices inputType)
        {
            if (MoveHorizontal.MoveHorRightDict.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"{inputType} not found in {MoveHorizontal} dictionary");
        }
        
        private List<Enum> GetMoveUpInput(InputDevices inputType)
        {
            // Check if the key exists in the dictionary
            if (MoveVertical.MoveVertUpDict.TryGetValue(inputType, out var value))
                return value; // Return the value associated with the key
            else
                throw new KeyNotFoundException($"{inputType} not found in {MoveVertical} dictionary");
        }

        private List<Enum> GetMoveDownInput(InputDevices inputType)
        {
            if (MoveVertical.MoveVertDownDict.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"{inputType} not found in {MoveVertical} dictionary");
        }

        //The reason we make this generic, is because the ButtonState return that we need for the mouse is not 
        private List<Enum> GetShootLeftInput(InputDevices inputType)
        {
            if (ShootLeft.ShootLeftDict.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in {ShootLeft} dictionary");
        }
        
        private List<Enum> GetShootRightInput(InputDevices inputType)
        {
            if (ShootRight.ShootRightDict.TryGetValue(inputType, out var value))
                return value;
            else
                throw new KeyNotFoundException($"InputTypes.{inputType} not found in {ShootRight} dictionary");
        }
        #endregion

        #region Input Updates
        //Update all input states
        public override void UpdateInput()
        {
            UpdateKeyboardInput();
            UpdateGamepadInput();
            UpdateMouseInput();
        }

        protected override void UpdateKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            
            //MoveHorizontal - Left
            foreach (Keys leftInput in GetMoveLeftInput(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(leftInput))
                {
                    MoveHorizontal.InputAction.UpdateKeyboardValueX(true, MoveHorizontal.InputAction.CurrentY); 
                    break;
                }
                MoveHorizontal.InputAction.UpdateKeyboardValueX(false, MoveHorizontal.InputAction.CurrentY);
            }
            //MoveHorizontal - Right
            foreach (Keys rightInput in GetMoveRightInput(InputDevices.Keyboard))
            {
                if (keyboardState.IsKeyDown(rightInput))
                {
                    MoveHorizontal.InputAction.UpdateKeyboardValueX(MoveHorizontal.InputAction.CurrentX, true); 
                    break;
                }
                MoveHorizontal.InputAction.UpdateKeyboardValueX(MoveHorizontal.InputAction.CurrentX, false); 
            }
            
        }

        protected override void UpdateMouseInput()
        {
            //Shoot Right
            foreach (MouseInputTypes rightClick in GetShootRightInput(InputDevices.Mouse))
            {
                if (MouseInput.IsButtonPressed(rightClick))
                {
                    ShootRight.InputAction.UpdateMouseValue(true);
                    break;
                }
                ShootRight.InputAction.UpdateMouseValue(false);
            }
            
            //Shoot Left
            foreach (MouseInputTypes leftClick in GetShootLeftInput(InputDevices.Mouse))
            {
                if (MouseInput.IsButtonPressed(leftClick))
                {
                    ShootLeft.InputAction.UpdateMouseValue(true);
                    break;
                }
                ShootLeft.InputAction.UpdateMouseValue(false);
            }
        }

        protected override void UpdateGamepadInput()
        {
            var gamepadState = GamePad.GetState(0);

            //MoveHorizontal - Left
            foreach (Buttons leftInput in GetMoveLeftInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(leftInput))
                {
                    MoveHorizontal.InputAction.UpdateGamepadValueX(true, MoveHorizontal.InputAction.CurrentY); 
                    break;
                }
                MoveHorizontal.InputAction.UpdateGamepadValueX(false, MoveHorizontal.InputAction.CurrentY); 
            }
            
            //MoveHorizontal - Right
            foreach (Buttons rightInput in GetMoveRightInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(rightInput))
                {
                    MoveHorizontal.InputAction.UpdateGamepadValueX(MoveHorizontal.InputAction.CurrentX, true); 
                    break;
                }
                MoveHorizontal.InputAction.UpdateGamepadValueX(MoveHorizontal.InputAction.CurrentX, false); 
            }
            
            //Shoot Right
            foreach (Buttons rightTrigger in GetShootRightInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(rightTrigger))
                {
                    ShootRight.InputAction.UpdateGamepadValue(true);
                    break;
                }
                ShootRight.InputAction.UpdateGamepadValue(false);
            }
            
            //Shoot Left
            foreach (Buttons leftTrigger in GetShootLeftInput(InputDevices.Gamepad))
            {
                if (gamepadState.IsButtonDown(leftTrigger))
                {
                    ShootLeft.InputAction.UpdateGamepadValue(true);
                    break;
                }
                ShootLeft.InputAction.UpdateGamepadValue(false);
            }
            
        }

        #endregion

        #region Input Remapping

        /// <summary>
        /// Remap the an Input Action Dictionary key's value
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="newInput">Keys, Buttons, MouseButtons</param>
        public override void RemapInputAction(Dictionary<InputDevices, List<Enum>> inputActionDict, InputDevices inputType, List<Enum> newInput)
        {
            //E.g. RemapInputAction(MoveHorizontal.MoveHorLeftDict, InputTypes.Keyboard, Keys.Left)
            inputActionDict.Remove(inputType);
            inputActionDict.TryAdd(inputType, newInput);
        }

        #endregion
    }
}