using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KenneyJam2026.NPCs.Bubbles
{
    public class NpcBubblesCanvas : MonoBehaviour
    {
        [SerializeField] private Transform[] _bubblePlaceholders;
        [SerializeField] private Transform _bubblesContainer;
        [SerializeField] private Transform _orientationOrigin;

        private readonly Dictionary<NpcBubble, List<NpcBubble>> _bubbleInstancesPerPrefab = new();
        private readonly List<NpcBubble> _currentBubbles = new();

        private CancellationTokenSource _showMessageCancellationTokenSource;

        public void ShowBubble(NpcBubble bubblePrefab)
        {
            if (_currentBubbles.Count >= _bubblePlaceholders.Length)
            {
                Debug.LogWarning("Trying to push more bubbles than possible");
                return;
            }

            if (!_bubbleInstancesPerPrefab.TryGetValue(bubblePrefab, out var bubbleInstances))
            {
                bubbleInstances = new List<NpcBubble>();
                _bubbleInstancesPerPrefab.Add(bubblePrefab, bubbleInstances);
            }

            var instanceIndex = 0;

            while (instanceIndex < bubbleInstances.Count && bubbleInstances[instanceIndex].gameObject.activeSelf)
            {
                instanceIndex++;
            }

            if (instanceIndex == bubbleInstances.Count)
            {
                var newInstance = Instantiate(bubblePrefab, _bubblesContainer);
                newInstance.OrientationOrigin = _orientationOrigin;
                bubbleInstances.Add(newInstance);
            }

            var instance = bubbleInstances[instanceIndex];
            var positionIndex = _currentBubbles.Count;
            var placeholder = _bubblePlaceholders[positionIndex];
            placeholder.gameObject.SetActive(true);

            _currentBubbles.Add(instance);
            instance.Appear(placeholder);
        }

        public void ClearMessage()
        {
            foreach (var currentBubble in _currentBubbles)
            {
                currentBubble.Disappear();
            }

            foreach (var placeholder in _bubblePlaceholders)
            {
                placeholder.gameObject.SetActive(false);
            }

            _currentBubbles.Clear();
        }

        private void OnDestroy()
        {
            _showMessageCancellationTokenSource?.Cancel();
            _showMessageCancellationTokenSource?.Dispose();
            _showMessageCancellationTokenSource = null;
        }

        public void ShowMessage(NpcMessage message)
        {
            _showMessageCancellationTokenSource?.Cancel();
            _showMessageCancellationTokenSource?.Dispose();
            _showMessageCancellationTokenSource = new CancellationTokenSource();

            ClearMessage();

            ShowMessageAsync(message, _showMessageCancellationTokenSource.Token).Forget();
        }

        private async UniTask ShowMessageAsync(NpcMessage message, CancellationToken cancellationToken)
        {
            for (var i = 0; i < message.BubbleCount; ++i)
            {
                await UniTask.Delay((int)(message.GetDelay(i) * 1000), cancellationToken: cancellationToken);

                ShowBubble(message.GetBubble(i));
            }
        }
    }
}