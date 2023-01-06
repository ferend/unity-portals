using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Behaviours
{
    public class PortalTeleport : MonoBehaviour, ITeleport
    {
        public Vector3 previousOffsetFromPortal { get; set; }
        
        float _smoothYaw = 5;
        Vector3 _velocity;
        
        public Material[] originalMaterials { get; set; }
        public Material[] cloneMaterials { get; set; }


        public virtual void Teleport(Transform from, Transform to, Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            Vector3 eulerRot = rot.eulerAngles;
            float delta = Mathf.DeltaAngle (_smoothYaw, eulerRot.y);
            _smoothYaw += delta;
            transform.eulerAngles = Vector3.up * _smoothYaw;
            _velocity = to.TransformVector (from.InverseTransformVector (_velocity));
            Physics.SyncTransforms ();
        }

        public void DuringTeleport(GameObject go)
        {

        }
        

        public void OnTeleportEnd(GameObject go){}

        public virtual void EnterPortalThreshold () {
       
        }
        

        // Called once no longer touching portal (excluding when teleporting)
        public virtual void ExitPortalThreshold () {
       
        }

    }
}
