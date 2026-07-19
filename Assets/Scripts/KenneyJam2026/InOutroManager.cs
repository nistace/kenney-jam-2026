using KenneyJam2026.Mechanisms;
using KenneyJam2026.NPCs.Bubbles;
using UnityEngine;

namespace KenneyJam2026
{
    public class InOutroManager : MonoBehaviour
    {
        private static readonly int Intro = Animator.StringToHash("Intro");
        private static readonly int Outro = Animator.StringToHash("Outro");

        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private Transform _playerCameraHolder;
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _cutsceneCameraHolder;
        [SerializeField] private Door _door;
        [SerializeField] private NpcBubble _heartBubble;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _playerObject.SetActive(false);
            _camera.SetParent(_cutsceneCameraHolder);
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.identity;
            _animator.SetBool(Intro, true);
        }

        public void EndIntro()
        {
            _playerObject.SetActive(true);
            _camera.SetParent(_playerCameraHolder);
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.identity;

            _door.OnExitTriggered += HandleExitTriggered;
        }

        private void HandleExitTriggered()
        {
            _door.OnExitTriggered -= HandleExitTriggered;

            _playerObject.SetActive(false);
            _camera.SetParent(_cutsceneCameraHolder);
            _camera.localPosition = Vector3.zero;
            _camera.localRotation = Quaternion.identity;
            _animator.SetBool(Outro, true);
        }

        public void ShowHeart() => _heartBubble.Appear(null);

        public void EndOutro()
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        private void OnDestroy()
        {
            _door.OnExitTriggered -= HandleExitTriggered;
        }
    }
}