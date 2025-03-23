using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SelectVisual : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabObject;
    [SerializeField] private GameObject selectVisual;
    [SerializeField] private GameObject activateVisual;
    [SerializeField] private Transform fixedNozzle;
    private void Start()
    {
        grabObject.hoverEntered.AddListener(OnHoverEnter);
        grabObject.hoverExited.AddListener(OnHoverExit);
        grabObject.selectEntered.AddListener(OnHoverExit);
        grabObject.activated.AddListener(OnActivate);
        grabObject.deactivated.AddListener(OnDeactivate);
        grabObject.selectExited.AddListener(OnDeactivate);
    }

    private void OnDeactivate(SelectExitEventArgs arg0)
    {
        if(arg0.isCanceled)
            Deactivate();
    }

    private void OnDeactivate(DeactivateEventArgs arg0)
    {
        Deactivate();
    }

    private void OnActivate(ActivateEventArgs arg0)
    {
        Activate();
    }

    private void OnHoverExit(SelectEnterEventArgs arg0)
    {
        Hide();
    }

    private void OnHoverExit(HoverExitEventArgs arg0)
    {
        Hide();
    }

    private void OnHoverEnter(HoverEnterEventArgs arg0)
    {
        if (!grabObject.isSelected)
        {
            Show();
        }
    }

    private void Show()
    {
        selectVisual.SetActive(true);
    }

    private void Hide()
    {
        selectVisual.SetActive(false);
    }

    private void Activate()
    {
        activateVisual.transform.position -= transform.up * 0.01f;
    }

    private void Deactivate()
    {
        activateVisual.transform.position = fixedNozzle.position;
    }
}
