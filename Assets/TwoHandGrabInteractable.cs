 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class TwoHandGrabInteractable : XRGrabInteractable
{
    private XRBaseInteractor secondInteractor;
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private Quaternion attachIntitialRotation;
    public enum TwoHandRotationType { None, First, Second};
    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;
    private Quaternion intitialRotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEnter.AddListener(OnSecondHandGrab);
            item.onSelectExit.AddListener(OnSecondHandRelease);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        secondInteractor = interactor;
        intitialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
    }
    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        interactor = null;
    }
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        attachIntitialRotation = interactor.transform.localRotation;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.transform.localRotation = attachIntitialRotation;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {
            //compute transform
            if (snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * intitialRotationOffset;
        }
        base.ProcessInteractable(updatePhase);

    }
    private Quaternion GetTwoHandRotation()
    {
        Quaternion target;

        if (twoHandRotationType == TwoHandRotationType.None)
        {
            target = Quaternion.LookRotation(-selectingInteractor.transform.position + secondInteractor.transform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            target = Quaternion.LookRotation(-selectingInteractor.transform.position + secondInteractor.transform.position, selectingInteractor.attachTransform.up);

        }
        else
        {
          target =  Quaternion.LookRotation(-selectingInteractor.transform.position + secondInteractor.transform.position, secondInteractor.attachTransform.up);
        }

        return target;


    }


    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

}
