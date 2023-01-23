using System;
using System.Collections.Generic;
using System.Numerics;
using _Project.Scripts.Behaviours;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.Models
{
    public class Portal : MonoBehaviour, ITeleport
    {
       [SerializeField] private Portal _linkedPortal;
       [SerializeField] private MeshRenderer _screen;
       private Camera _playerCam;
       private Camera _portalCam;
       private RenderTexture _viewTexture;
       public List<PortalTeleport> _portalTeleporters;
       public Material a;
       
       private void Awake()
       {
           _playerCam = Camera.main;
           _portalCam = this.GetComponentInChildren<Camera>();
           _portalCam.enabled = false;
           _screen.material.SetInt ("displayMask", 1);
           _portalTeleporters = new List<PortalTeleport>();
       }

       private void LateUpdate()
       {
           Render();
           TrackedTravellerLoop();
       }

       // Dot product calculation for position from portal of loop over all tracked travellers to figure out if they should be 
       private void TrackedTravellerLoop()
       {
           for (int i = 0; i < _portalTeleporters.Count; i++)
           {
               
               PortalTeleport teleporter = _portalTeleporters[i];
               Transform teleporterTransform = teleporter.transform;
               var m = _linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * teleporterTransform.localToWorldMatrix;

               Vector3 offsetFromPortal = teleporterTransform.position - transform.position;
               int portalSide = System.Math.Sign (Vector3.Dot (offsetFromPortal, transform.forward));
               int portalSideOld = System.Math.Sign (Vector3.Dot (teleporter.previousOffsetFromPortal, transform.forward));
               
               // Where traveller should end up and call teleport function
               if (portalSide != portalSideOld)
               {
                   ProtectScreenFromClipping(_playerCam.transform.position);
                   teleporter.Teleport(transform,_linkedPortal.transform,m.GetColumn(3),m.rotation);
                   _linkedPortal.DuringTeleport(teleporter.gameObject);
                   _portalTeleporters.RemoveAt(i);
                   i--;
               }
               else
               {
                   teleporter.previousOffsetFromPortal = offsetFromPortal;
               }

           }
       }
       
       void CreateViewTexture()
       {
           if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
           {
               if (_viewTexture != null)
               {
                   _viewTexture.Release();
               }
               _viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
               _portalCam.targetTexture = _viewTexture;
               _linkedPortal._screen.material.SetTexture("_MainTex",_viewTexture);
           }
       }

       bool VisibleFromCamera(Renderer renderer, Camera camera)
       {
           // 6 planes that make cameras view frustum.
           Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
           return GeometryUtility.TestPlanesAABB(frustumPlanes,renderer.bounds);
       }
       // Called before player camera is rendered.
       public void Render()
       {
           if (!VisibleFromCamera(_linkedPortal._screen, _playerCam))
           {
               // Create texture and add it to screen if it is not visible from player camera
               var testTexture = new Texture2D(1, 1);
               testTexture.SetPixel(0,0,Color.black);
               testTexture.Apply();
               _linkedPortal._screen.material.SetTexture("_MainTex",testTexture); 
               return;
           }
           _linkedPortal._screen.material.SetTexture("_MainTex",_viewTexture);
           _screen.enabled = false;
           var m = transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix *
                   _playerCam.transform.localToWorldMatrix;
           _portalCam.transform.SetPositionAndRotation(m.GetColumn(3),m.rotation);
           _portalCam.Render();
           _screen.enabled = true;
           CreateViewTexture();
       }
       

       void ProtectScreenFromClipping(Vector3 viewPoint) {
           float halfHeight = _playerCam.nearClipPlane * Mathf.Tan (_playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
           float halfWidth = halfHeight * _playerCam.aspect;
           float dstToNearClipPlaneCorner = new Vector3 (halfWidth, halfHeight, _playerCam.nearClipPlane).magnitude;
           float screenThickness = dstToNearClipPlaneCorner;

           Transform screenT = _screen.transform;
           bool camFacingSameDirAsPortal = Vector3.Dot (transform.forward, transform.position - viewPoint) > 0;
          // screenT.localScale = new Vector3 (screenT.localScale.x, screenT.localScale.y, screenThickness);
           // screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
       }
       public void DuringTeleport(GameObject go)
       {
           var teleporter = go.GetComponent<PortalTeleport>();
           if (!_portalTeleporters.Contains(teleporter))
           {
               teleporter.EnterPortalThreshold();
               teleporter.previousOffsetFromPortal = teleporter.transform.position - transform.position;
               _portalTeleporters.Add(teleporter); 
           }       
       }

       public void OnTeleportEnd(GameObject go)
       {
           var teleporter = go.GetComponent<PortalTeleport>();
           teleporter.ExitPortalThreshold(); 
           _portalTeleporters.Remove(teleporter);
       }

       private void OnTriggerExit(Collider other)
       {
           var teleporter = other.GetComponent<PortalTeleport>();
           if (teleporter && _portalTeleporters.Contains(teleporter))
           {
                OnTeleportEnd(other.gameObject);
           }
       }

       private void OnTriggerEnter(Collider other)
       {
           var teleporter = other.GetComponent<PortalTeleport>();
           if (teleporter)
           {
               DuringTeleport(other.gameObject);
           }
       }

    }
}
