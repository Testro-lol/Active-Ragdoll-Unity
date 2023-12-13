using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    [SerializeField] BetterIKFootSolver leftIK;
    [SerializeField] BetterIKFootSolver rightIK;

    private void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
    }

    IEnumerator LegUpdateCoroutine()
    {
        // Run continuously
        while (true)
        {
            do
            {
                // This leg will try to move first
                leftIK.TryMove();
                yield return null;
            } 
            while (leftIK.isMoving);

            
            do
            {

                // This one will move second
                rightIK.TryMove();
                yield return null;
            } 
            while (rightIK.isMoving);
        }
    }
}
