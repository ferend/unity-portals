using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class VectorMethods 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

    public abstract class Mechanic : MonoBehaviour
    {
        

        public virtual void OnDown()
        {
        }

        public virtual void OnDrag()
        {
        }

        public virtual void OnUp()
        {
        }

        public virtual void KeyboardControls()
        {
            
        }
        

    }
