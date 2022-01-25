using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        base.OnSelectEntered(args);
        if(interactor is XRDirectInteractor)
            Climber.climbingHand.GetComponent<XRBaseController>();


    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        XRBaseInteractor interactor = args.interactor;
        base.OnSelectExited(args);
        if (Climber.climbingHand && Climber.climbingHand.name == interactor.name)
            Climber.climbingHand = null;
    }
}
