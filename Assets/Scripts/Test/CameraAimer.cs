using System;
using System.Collections.Generic;
using Test.WatchListClasses;
using UnityEngine;
namespace Test
{
    [RequireComponent(typeof(Camera))]
    public class CameraAimer : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _watchList;
        [SerializeField]
        private float _damping = 100f;
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private  float _padding = 100f; 
        
        private void Awake()
        {
            _watchList = WatchList.watchList;
        }
        private void Start()
        {
            _camera=Camera.main;
        }
        private void LateUpdate()
        {
            if (_watchList.Count == 0)
            {
                return;
            }
            RotateView();
            Zoom();
        }
        private void Zoom()
        {
            var greatestDistance =  _camera.fieldOfView/GetScaleCoefficient();
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView,greatestDistance,Time.deltaTime/100f);
        }
        private float GetScaleCoefficient()
    {
        var bounds=GetBounds();
        var screenBounds = BoundsToScreenRect(bounds);
        var heightCoefficient = (Screen.height - 2*_padding) / screenBounds.size.y;
        var widthCoefficient = (Screen.width - 2*_padding) / screenBounds.size.x;
        return Mathf.Min(heightCoefficient,widthCoefficient);
    }
        private void RotateView()
        {
            Vector3 lookPosition = GetCenterPoint() - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _damping);
        }
        private Vector3 GetCenterPoint()
        {
            if (_watchList.Count == 1)
            {
                return _watchList[0].position;
            }

            Bounds bounds = GetBounds();
            return bounds.center;
        }
        private Bounds GetBounds()
        {
            Bounds bounds = new Bounds(_watchList[0].position, Vector3.zero);
            foreach (var t in _watchList)
            {
                bounds.Encapsulate(t.position);
            }
            return bounds;
        }
        private Rect BoundsToScreenRect(Bounds bounds)
        {
            // Get mesh origin and farthest extent
            Vector3 origin = _camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
            Vector3 extent = _camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
     
            // Create rect in screen space and return
            return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
        }
        

    }
}
