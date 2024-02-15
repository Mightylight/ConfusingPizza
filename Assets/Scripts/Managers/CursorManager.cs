using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    /// <summary>
    /// Sets the cursors locked and visible state.
    /// </summary>
    /// <param name="lockedState">If set to true, cursor is locked and not visible. If set to false, cursor is confined and visible.</param>
    public static void CursorState(bool lockedState)
    {
        if(lockedState == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if(lockedState == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
