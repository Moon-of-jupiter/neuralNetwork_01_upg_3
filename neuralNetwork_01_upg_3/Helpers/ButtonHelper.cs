using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Helpers
{
    public class ButtonHelper
    {
        public bool isPressed;
        //public Action<bool> pressAction;

        public event Action<bool> OnPress;

        public void UpdatePressed(bool pressed)
        {
            if (isPressed == pressed)
                return;

            TogglePressed(pressed);
        }

        private void TogglePressed(bool newValue)
        {
            isPressed = newValue;
            OnPress?.Invoke(newValue);
        }

        
        public void UnsubscribeAll()
        {
            var invocationList = OnPress?.GetInvocationList();

            foreach (Action<bool> invocation in invocationList)
            {
                OnPress -= invocation;
            }
        }


    }
}
