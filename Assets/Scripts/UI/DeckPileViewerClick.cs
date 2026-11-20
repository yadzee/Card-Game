using UnityEngine;

namespace UI
{
    public class DeckPileViewerClick : MonoBehaviour
    {
        private enum PileType
        {
            Draw,
            Discard
        }

        [SerializeField] private PileType pileType;

        private DeckViewer _deckViewer;
        private Collider _collider;
        private Camera _mainCamera;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _mainCamera = Camera.main;
            _deckViewer = FindFirstObjectByType<DeckViewer>(FindObjectsInactive.Include);

            if (_deckViewer == null || _collider == null || _mainCamera == null)
            {
                Debug.LogError("DeckPileViewerClick: DeckViewer, Collider, or Main Camera is missing.", this);
                enabled = false;
            }
        }

        private void Update()
        {
            if (_deckViewer == null || _deckViewer.IsOpen)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!_collider.Raycast(ray, out _, _mainCamera.farClipPlane))
                return;

            if (pileType == PileType.Draw)
                _deckViewer.ShowDrawPile();
            else
                _deckViewer.ShowDiscardPile();
        }
    }
}
