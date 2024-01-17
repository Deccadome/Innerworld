using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Dome {
    public class InputMapper : MonoBehaviour
    {
        const int MOVE_UP = 0;
        const int MOVE_RIGHT = 1;
        const int MOVE_DOWN = 2;
        const int MOVE_LEFT = 3;
        const int INTERACT = 4;
        const int ATTACK = 5;
        const int SHOOT = 6;
        const int AIM = 7;

        public Dictionary<string, string> controls;
        public InputReader inputReader;
        GameInput gameInput;
        private Dictionary<string, InputAction> commands;

    void Start()
        {
            gameInput = inputReader._gameInput;

            controls = new Dictionary<string, string>
            {
                { "A", "<Keyboard>/a" },
                { "D", "<Keyboard>/d" },
                { "E", "<Keyboard>/e" },
                { "W", "<Keyboard>/w" },
                { "F", "<Keyboard>/f" },
                { "S", "<Keyboard>/s" },
                { "LeftClick", "<Mouse>/leftButton" },
                { "RightClick", "<Mouse>/rightButton" },
            };

            commands = new Dictionary<string, InputAction>
            {
                { "MoveUp", gameInput.OW.MoveUp },
                { "MoveRight", gameInput.OW.MoveRight },
                { "MoveLeft", gameInput.OW.MoveLeft },
                { "MoveDown", gameInput.OW.MoveDown },
                { "Interact", gameInput.OW.Interact },
                { "Attack", gameInput.OW.Attack },
                { "Shoot", gameInput.OW.Shoot },
                { "Aim", gameInput.OW.Aim },
            };
        }

        public void SetBinding(string label, string control)
        {
            Debug.Log("Label: " + label + " Control: " + control);
            Debug.Log(commands[label].AddBinding().WithPath(controls[control]));
        }

        public void UnsetBinding(string label)
        {
            commands[label].ChangeBinding(0).Erase();
        }

        private string GetInputCode(string control)
        {
            return controls[control];
        }
    }
}