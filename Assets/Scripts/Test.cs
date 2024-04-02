using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private float totalTime = 0f;
    [SerializeField] private float swipeTimer = 0f;
    [SerializeField] private bool allowIncrease = false;
    public GameObject TrailRendererPrefab;
    public GameObject objectPrefab;
    private GameObject currentTRobject;
    InputAction.CallbackContext currentContext;
    private float maxTimeDT = 0.2f;
    private float minTimePress = 0.15f;
    private float swipeLoopTime = 0.2f;
    private float swipeMinDistance = 3.0f;
    Vector3 currentPosition;
    Vector2 startPosition;
    bool isDoubleTap = false;
    bool isTapped = true;
    bool isPress = false;
    private void Update()
    {
        if (allowIncrease)
            totalTime += Time.deltaTime;

        if (totalTime >= maxTimeDT)
        {
            allowIncrease = false;
            totalTime = 0f;
            swipeTimer = 0f;
            isDoubleTap = false;
            if (isTapped)
                MyTap();
            isTapped = true;
            isPress = false;
            Destroy(currentTRobject);
        }
        else if (currentContext.phase == InputActionPhase.Performed && totalTime >= minTimePress)
        {
            swipeTimer += Time.deltaTime;
            isPress = true;
            totalTime = minTimePress;
            MyPress();
            if (currentTRobject == null)
            {
                currentTRobject = Instantiate(TrailRendererPrefab, new Vector3(currentPosition.x, currentPosition.y, 0), Quaternion.identity);
            }
        }

        if (swipeTimer >= swipeLoopTime)
        {
            swipeTimer = 0f;
            float distance = Vector2.Distance(startPosition, currentPosition);
            if (distance >= swipeMinDistance)
            {
                MySwipe();
            }
            startPosition = currentPosition;
        }
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        currentContext = context;
        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Started:
                if (isDoubleTap)
                {
                    isTapped = false;
                    MyDoubleTap();
                    totalTime = maxTimeDT;
                }
                startPosition = currentPosition;
                break;
            case InputActionPhase.Performed:
                allowIncrease = true;
                isDoubleTap = true;
                break;
            case InputActionPhase.Canceled:
                if (isPress)
                    isTapped = false;
                break;
        }
    }

    public void OnPosition(InputAction.CallbackContext context)
    {
        currentPosition = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(currentPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        currentPosition = ray.GetPoint(distance);
        Debug.Log(currentPosition);
    }

    //Inputs
    private void MyTap()
    {
        Debug.Log("Tap");
    }

    private void MyPress()
    {
        Debug.Log("Press");
        //Logica ser, agarrar objeto con un raycast
    }

    private void MyDoubleTap()
    {
        Debug.Log("DoubleTap");
    }
    private void MySwipe()
    {
        Debug.Log("Swipe");
        //Cuando se cumpla, se lanzara la pokebal
    }
}
