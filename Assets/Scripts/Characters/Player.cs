using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeShiftState
{
    RAT,
    HUMAN
}

public class Player : Character
{
        private ShapeShiftState currentState;

        // Start is called before the first frame update
        void Start()
        {
            currentState = ShapeShiftState.HUMAN;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentState == ShapeShiftState.RAT)
                    currentState = ShapeShiftState.HUMAN;
                else
                    currentState = ShapeShiftState.RAT;
            }

            if (currentState == ShapeShiftState.RAT)
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, .25f, 1), .01f);
            else
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), .01f);
        }
}