using System;
using KenneyJam2026.Interactables;
using KenneyJam2026.Milk;
using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;

namespace KenneyJam2026.NPCs
{
    public class Npc : MonoBehaviour
    {
        public enum ECurrentState
        {
            Spawned = 0,
            InLine = 1,
            FirstInLine = 2,
            WaitToOpen = 3,
            Knocking = 4,
            AskMilk = 5,
            AskQuantity = 7,
            WaitMilk = 8,
            RefuseDelivery = 12,
            AcceptDelivery = 13,
            CancelRequest = 10,
            WantToLeave = 11,
            Leaving = 9
        }

        [SerializeField] private float _smoothSpeed = .1f;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private Vector3 _targetForward = Vector3.forward;
        [SerializeField] private bool _atTargetPosition;

        private Vector3 _currentVelocity;
        public ECurrentState CurrentState { get; private set; } = ECurrentState.Spawned;
        private float _currentStateStartTime;
        private float CurrentStateDuration => Time.time - _currentStateStartTime;
        private bool _windowOpen;

        public Vector3 TargetForward
        {
            get => _targetForward;
            set => _targetForward = value;
        }

        public bool IsMoving => _targetPosition != transform.position;
        public NpcType Type { get; set; }
        public NpcExpectedQuantity ExpectedQuantity { get; set; }
        public bool IsKnocking => CurrentState == ECurrentState.Knocking;

        public event Action<NpcMessage> OnMessageArticulated;
        public event Action OnMessageCancelled;
        public event Action<Npc> OnWantToLeave;
        public event Action<Npc> OnDeliveryFulfilled;
        public event Action OnLeft;

        public void SetTargetPositionInLine(Vector3 targetPosition, bool firstInLine)
        {
            _targetPosition = targetPosition;
            _atTargetPosition = false;
            ChangeState(firstInLine ? ECurrentState.FirstInLine : ECurrentState.InLine);
        }

        public void SetTargetPositionToLeave(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _atTargetPosition = false;
            ChangeState(ECurrentState.Leaving);
        }

        private bool ChangeState(ECurrentState newState)
        {
            if (newState == CurrentState)
            {
                return false;
            }

            CurrentState = newState;
            _currentStateStartTime = Time.time;

            OnMessageCancelled?.Invoke();

            return true;
        }

        private void Update()
        {
            UpdatePosition();
            UpdateState();
        }

        private void UpdateState()
        {
            if (CurrentState == ECurrentState.InLine && CurrentStateDuration > Type.MaxDurationInLine)
            {
                StartLeaving();
            }
            else if (CurrentState == ECurrentState.WaitToOpen && CurrentStateDuration > Type.DelayBeforeKnocking)
            {
                Knock();
            }
            else if (CurrentState == ECurrentState.Knocking && CurrentStateDuration > Type.MaxDurationKnocking)
            {
                CancelRequest();
            }
            else if (CurrentState == ECurrentState.CancelRequest && CurrentStateDuration > Type.CancelRequestDuration)
            {
                StartLeaving();
            }
            else if (CurrentState == ECurrentState.RefuseDelivery && CurrentStateDuration > Type.TimeAfterHandlingDelivery)
            {
                AskQuantity();
            }
            else if (CurrentState == ECurrentState.AcceptDelivery && CurrentStateDuration > Type.TimeAfterHandlingDelivery)
            {
                StartLeaving();
            }
        }

        private void StartLeaving()
        {
            if (CurrentState != ECurrentState.Leaving && ChangeState(ECurrentState.WantToLeave))
            {
                OnWantToLeave?.Invoke(this);
            }
        }

        private void UpdatePosition()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _smoothSpeed, Type.MovementSpeed);

            if ((transform.position - _targetPosition).sqrMagnitude > 0.01f)
            {
                transform.forward = _targetPosition - transform.position;
            }
            else
            {
                if (!_atTargetPosition)
                {
                    _atTargetPosition = true;

                    if (CurrentState == ECurrentState.Leaving)
                    {
                        OnLeft?.Invoke();
                        Destroy(gameObject);
                    }

                    if (CurrentState == ECurrentState.FirstInLine)
                    {
                        if (_windowOpen)
                        {
                            AskMilk();
                        }
                        else
                        {
                            ChangeState(ECurrentState.WaitToOpen);
                        }
                    }
                }

                transform.forward = TargetForward;
            }
        }

        private void AskMilk()
        {
            if (ChangeState(ECurrentState.AskMilk))
            {
                OnMessageArticulated?.Invoke(Type.AskMilkMessage);
            }
        }

        private void CancelRequest()
        {
            if (ChangeState(ECurrentState.CancelRequest))
            {
                OnMessageArticulated?.Invoke(Type.MessageWhileCancelling);
            }
        }

        private void Knock()
        {
            if (ChangeState(ECurrentState.Knocking))
            {
                OnMessageArticulated?.Invoke(Type.MessageWhileKnocking);
            }
        }

        public void AskQuantity()
        {
            if (ChangeState(ECurrentState.AskQuantity))
            {
                OnMessageArticulated?.Invoke(ExpectedQuantity.Message);
            }
        }

        public void SetWindowOpen(bool windowOpen)
        {
            _windowOpen = windowOpen;

            if (_windowOpen && CurrentState is ECurrentState.WaitToOpen or ECurrentState.Knocking)
            {
                AskMilk();
            }

            if (!_windowOpen && CurrentState is ECurrentState.AskMilk or ECurrentState.AskQuantity or ECurrentState.WaitMilk)
            {
                CancelRequest();
            }
        }

        public void RefuseDelivery()
        {
            if (ChangeState(ECurrentState.RefuseDelivery))
            {
                OnMessageArticulated?.Invoke(Type.RefuseDeliveryMessage);
            }
        }

        public void Deliver(MilkContainer milkContainer)
        {
            var validDelivery = Mathf.Abs(milkContainer.CurrentCharge - ExpectedQuantity.ExpectedQuantity) < ExpectedQuantity.ErrorMargin;

            if (validDelivery && ChangeState(ECurrentState.AcceptDelivery))
            {
                OnDeliveryFulfilled?.Invoke(this);
                OnMessageArticulated?.Invoke(Type.AcceptDeliveryMessage);
            }
            else if (!validDelivery && ChangeState(ECurrentState.RefuseDelivery))
            {
                OnMessageArticulated?.Invoke(Type.RefuseDeliveryMessage);
            }

            var draggable = milkContainer.GetComponentInParent<IDraggable>();

            if (draggable != null && draggable.gameObject)
            {
                Destroy(draggable.gameObject);
            }
        }
    }
}