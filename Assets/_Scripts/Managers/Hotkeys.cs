using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hotkeys : MonoBehaviour
{
    [SerializeField] private Button[] jobsButtons;
    [SerializeField] private Button[] placeablesButtons;

    private Button activeButton;

    private void Update()
    {
        ActivateKey(jobsButtons); 
        ActivateKey(placeablesButtons);
    }

    private void ActivateKey(Button[] buttonList)
    {
        if (PressedHotkey() != 0)
        {
            if (buttonList.Length !>= PressedHotkey()) 
            {
                int num = PressedHotkey() - 1;

                if (buttonList[num] == null) return;
                if (buttonList[num].gameObject.activeInHierarchy == false) return;

                buttonList[num].onClick.Invoke();

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                buttonList[num].OnPointerDown(pointerEventData);

                activeButton = buttonList[num];
            }

            ResetButtonState(activeButton);
        }
    }

    private int PressedHotkey()
    {
        if (Input.anyKeyDown && Input.inputString.Length == 1 && char.IsDigit(Input.inputString[0]))
        {
            char keyPressed = Input.inputString[0];
            int numberPressed = int.Parse(keyPressed.ToString());

            if (numberPressed == 0)
            {
                numberPressed = 10;
            }

            return numberPressed;
        }
        
        return 0;
    }

    private void ResetButtonState(Button button)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        button.OnPointerUp(pointerEventData);
    }
}
