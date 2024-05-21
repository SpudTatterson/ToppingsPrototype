using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hotkeys : MonoBehaviour
{
    [SerializeField] private GameObject jobsButtonsParent;
    [SerializeField] private GameObject placeablesButtonsParent;

    [SerializeField] private Button[] jobsButtons;
    [SerializeField] private Button[] placeablesButtons;

    private Button activeButton;

    private void Update()
    {
        ActivateHotkey();
    }

    private void ActivateHotkey()
    {
        if (ActiveButtonList() == null || ActiveUI() == null) return;

        if (PressedHotkey() == 11)
        {
            ActiveUI().GetComponent<Button>().onClick.Invoke();
        }
        else if (PressedHotkey() != 0)
        {
            if (ActiveButtonList().Length! >= PressedHotkey())
            {
                int objectOrder = PressedHotkey() - 1;

                if (ActiveButtonList()[objectOrder] == null) return;

                ActiveButtonList()[objectOrder].onClick.Invoke();

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                ActiveButtonList()[objectOrder].OnPointerDown(pointerEventData);

                activeButton = ActiveButtonList()[objectOrder];
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
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            return 11;
        }

        return 0;
    }

    private void ResetButtonState(Button button)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        button.OnPointerUp(pointerEventData);
    }

    private Button[] ActiveButtonList()
    {
        if (jobsButtons[0].gameObject.activeInHierarchy)
        {
            return jobsButtons;
        }
        else if (placeablesButtons[0].gameObject.activeInHierarchy)
        {
            return placeablesButtons;
        }

        return null;
    }

    private GameObject ActiveUI()
    {
        if (jobsButtonsParent.activeInHierarchy)
        {
            return jobsButtonsParent;
        }
        else if (placeablesButtonsParent.activeInHierarchy)
        {
            return placeablesButtonsParent;
        }

        return null;
    }
}
