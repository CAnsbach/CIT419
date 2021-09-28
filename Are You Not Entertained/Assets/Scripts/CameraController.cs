using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset = new Vector3(0.0f, 20.0f, 0f);

    private void LateUpdate()
    {
        Vector3 chosenPosition = target.position + offset;

        transform.position = chosenPosition;

        transform.LookAt(target);
    }
}
