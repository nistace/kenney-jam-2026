using System;
using KenneyJam2026.Spawners;
using UnityEditor;
using UnityEngine;

namespace KenneyJam2026.Interactables
{
    public class DraggableObject : MonoBehaviour, IDraggable
    {
        [SerializeField] private InteractableType _type;
        [SerializeField] private InteractableType _draggingInteractionType;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject[] _interactableLayerObjects;
        [SerializeField] private float _rotationStrength = 5;
        [SerializeField] private float _maxAngularVelocity = 50;

        public InteractableType InteractableType => _type;
        public InteractableType DraggingInteractionType => _draggingInteractionType;
        public Vector3 Position => transform.position;

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        public event Action<DraggableObject> OnDestroying;

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public bool CanInteractWith(IDraggable heldObject) => heldObject == null;

        public void Interact(Vector3 atPosition, IDraggable heldObject)
        {
            if (heldObject != null) return;

            _rigidbody.useGravity = false;
        }

        public void StopInteraction() => _rigidbody.useGravity = true;

        public void SetOnDraggedLayer(bool value)
        {
            var newLayer = value ? IDraggable.DraggedInteractableLayer : IDraggable.InteractableLayer;

            foreach (var layerObject in _interactableLayerObjects)
            {
                layerObject.layer = newLayer;
            }
        }

        public LayerMask GetDraggedInteractionLayerMask(LayerMask currentLayerMask) => currentLayerMask | IDraggable.DragAimCatcherLayerMask;

        public void RotateToPosition(Vector3 forward, Vector3 up)
        {
            var flatForward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

            var delta = Quaternion.LookRotation(flatForward, up) * Quaternion.Inverse(_rigidbody.rotation);
            delta.ToAngleAxis(out var angle, out var axis);

            if (angle > 180f)
            {
                angle -= 360f;
            }

            if (Mathf.Abs(angle) < 0.01f)
            {
                return;
            }

            var angularVelocity = axis.normalized * (angle * Mathf.Deg2Rad * _rotationStrength);
            _rigidbody.angularVelocity = Vector3.ClampMagnitude(angularVelocity, _maxAngularVelocity);
        }

        public void Release(Vector3 intendedForce)
        {
            _rigidbody.AddForce(intendedForce, ForceMode.Impulse);
        }

        private void OnDestroy() => OnDestroying?.Invoke(this);

#if UNITY_EDITOR
        [ContextMenu("To Spawner")]
        private void ToSpawner()
        {
            var spawner = new GameObject(name + " Spawner").AddComponent<DraggableSpawner>();
            spawner.transform.SetParent(transform.parent);
            spawner.transform.localPosition = transform.localPosition;
            spawner.transform.localRotation = transform.localRotation;

            var o = new SerializedObject(spawner);
            o.Update();
            o.FindProperty("_prefab").objectReferenceValue = PrefabUtility.GetCorrespondingObjectFromSource(gameObject).GetComponent<DraggableObject>();
            o.ApplyModifiedProperties();
        }
#endif
    }
}