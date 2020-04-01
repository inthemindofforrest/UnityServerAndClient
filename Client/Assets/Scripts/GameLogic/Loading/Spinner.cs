using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float SpinningSpeed = 3;

    private void FixedUpdate()
    {
        Vector3 AxisToSpin = new Vector3(0, 0, SpinningSpeed * Time.deltaTime);

        transform.Rotate(AxisToSpin);
    }
}
