
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Deck;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeckViewer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Deck deck;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardsParent;
        [SerializeField] private GameObject viewerPanel;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject endTurnButton;

        [Header("Backdrop")]
        [SerializeField] private Color backdropColor;
        [SerializeField] private int backdropSortingOrder;

        [Header("Layout")]
        [SerializeField] private int columns;
        [SerializeField] private Vector2 spacing;
        [SerializeField, Min(1f)] private float visibleRows = 2.5f;
        [SerializeField] private Scrollbar pileScrollbar;
        [SerializeField, Min(0.01f)] private float cardsReferenceCanvasScale = 1f;

        private Camera _mainCamera;
        private Canvas _cardsCanvas;
        private RectTransform _scrollbarArtwork;
        private Texture2D _solidTexture;
        private Sprite _solidSprite;
        private SpriteRenderer _backdropRenderer;
        private bool _endTurnButtonWasActive;
        private Vector3 _cardsParentInitialPosition;
        private Vector3 _cardsParentInitialScale;
        private bool _cardsParentInitialPositionCaptured;
        private float _maxScrollOffset;
        private float _scrollProgress;
        public bool IsOpen => viewerPanel != null && viewerPanel.activeSelf;

        private void LateUpdate()
        {
            if (!IsOpen)
                return;

            UpdateCardsParentTransform();
            ResetScrollbarArtworkOffset();

            if (_backdropRenderer != null)
            {
                UpdateBackdropTransform();

                _backdropRenderer.color = backdropColor;
                _backdropRenderer.sortingOrder = backdropSortingOrder;
            }
        }

        public void ShowDrawPile()
        {
            if (deck == null)
            {
                Debug.LogError("DeckViewer: Deck reference is missing.");
                return;
            }

            ShowCards(deck.DrawPile);
        }

        public void ShowDiscardPile()
        {
            if (deck == null)
            {
                Debug.LogError("DeckViewer: Deck reference is missing.");
                return;
            }

            ShowCards(deck.DiscardPile);
        }

        private void ShowCards(IReadOnlyList<Card> cards)
        {
            Clear();

            if (cards == null || cardPrefab == null || cardsParent == null || viewerPanel == null)
            {
                Debug.LogError("DeckViewer: Missing references.");
                return;
            }

            EnsureViewerVisuals();
            CacheCardsParentTransform();
            if (!IsOpen && endTurnButton != null)
            {
                _endTurnButtonWasActive = endTurnButton.activeSelf;
                endTurnButton.SetActive(false);
            }

            viewerPanel.SetActive(true);
            if (backButton != null)
                backButton.SetActive(true);
            UpdateBackdropTransform();
            if (_backdropRenderer != null)
                _backdropRenderer.gameObject.SetActive(true);

            int safeColumns = Mathf.Max(1, columns);
            float cardVisualHeight = 0f;

            for (int i = 0; i < cards.Count; i++)
            {
                int row = i / safeColumns;
                int column = i % safeColumns;

                int cardsInRow = Mathf.Min(
                    safeColumns,
                    cards.Count - row * safeColumns
                );

                float x = (column - (cardsInRow - 1) * 0.5f) * spacing.x;
                float y = -row * spacing.y;

                GameObject cardObject = Instantiate(
                    cardPrefab,
                    cardsParent,
                    false
                );

                cardObject.transform.localPosition = new Vector3(x, y, 0f);

                UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();

                if (cardLink == null)
                {
                    Debug.LogError("DeckViewer: UiCardLink is missing.");
                    Destroy(cardObject);
                    continue;
                }

                cardLink.Initialize(cards[i]);

                if (i == 0)
                    cardVisualHeight = GetCardVisualHeightInParentSpace(cardObject);
            }

            ConfigureScrollbar(cards.Count, safeColumns, cardVisualHeight);
        }

        private float GetCardVisualHeightInParentSpace(GameObject cardObject)
        {
            Renderer[] renderers = cardObject.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0 || cardsParent == null)
                return 0f;

            Bounds visualBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                visualBounds.Encapsulate(renderers[i].bounds);

            return visualBounds.size.y / Mathf.Max(Mathf.Abs(cardsParent.lossyScale.y), 0.0001f);
        }

        private void ConfigureScrollbar(int cardCount, int safeColumns, float cardVisualHeight)
        {
            CacheCardsParentTransform();

            int rowCount = Mathf.CeilToInt((float)cardCount / safeColumns);
            float visibleRowsOnScreen = Mathf.Max(1f, visibleRows);
            float visibleHeight = visibleRowsOnScreen * spacing.y;
            float contentHeight = rowCount > 0
                ? (rowCount - 1) * spacing.y + Mathf.Max(cardVisualHeight, spacing.y)
                : 0f;

            _maxScrollOffset = Mathf.Max(0f, contentHeight - visibleHeight);

            if (pileScrollbar != null)
            {
                pileScrollbar.size = contentHeight > 0f
                    ? GetScrollbarHandleSize()
                    : 1f;
                pileScrollbar.SetValueWithoutNotify(1f);
                ResetScrollbarArtworkOffset();
                pileScrollbar.gameObject.SetActive(_maxScrollOffset > Mathf.Epsilon);
            }

            _scrollProgress = 0f;
            UpdateCardsParentTransform();
        }

        private float GetScrollbarHandleSize()
        {
            RectTransform handleRect = pileScrollbar.handleRect;
            RectTransform slidingArea = handleRect != null
                ? handleRect.parent as RectTransform
                : null;
            Graphic handleArtwork = handleRect != null
                ? handleRect.GetComponentInChildren<Graphic>(true)
                : null;

            if (slidingArea == null || handleArtwork == null)
                return 0.1f;

            float trackHeight = Mathf.Max(slidingArea.rect.height, 0.0001f);
            return Mathf.Clamp01(handleArtwork.rectTransform.rect.height / trackHeight);
        }

        private void ResetScrollbarArtworkOffset()
        {
            if (_scrollbarArtwork == null && pileScrollbar != null && pileScrollbar.handleRect != null)
            {
                Graphic handleArtwork = pileScrollbar.handleRect.GetComponentInChildren<Graphic>(true);
                if (handleArtwork != null)
                    _scrollbarArtwork = handleArtwork.rectTransform;
            }

            if (_scrollbarArtwork == null)
                return;

            Vector2 anchoredPosition = _scrollbarArtwork.anchoredPosition;
            if (Mathf.Approximately(anchoredPosition.y, 0f))
                return;

            anchoredPosition.y = 0f;
            _scrollbarArtwork.anchoredPosition = anchoredPosition;
        }

        public void OnScrollbarValueChanged(float value)
        {
            if (cardsParent == null || !_cardsParentInitialPositionCaptured)
                return;

            _scrollProgress = 1f - Mathf.Clamp01(value);
            UpdateCardsParentTransform();
            ResetScrollbarArtworkOffset();
        }

        private void CacheCardsParentTransform()
        {
            if (_cardsParentInitialPositionCaptured || cardsParent == null)
                return;

            _cardsCanvas = cardsParent.GetComponentInParent<Canvas>();
            _cardsParentInitialPosition = cardsParent.localPosition;
            _cardsParentInitialScale = cardsParent.localScale;
            _cardsParentInitialPositionCaptured = true;
        }

        private void UpdateCardsParentTransform()
        {
            if (cardsParent == null || !_cardsParentInitialPositionCaptured)
                return;

            float currentCanvasScale = _cardsCanvas != null ? _cardsCanvas.scaleFactor : 1f;
            float scaleRatio = cardsReferenceCanvasScale / Mathf.Max(currentCanvasScale, 0.0001f);

            cardsParent.localScale = _cardsParentInitialScale * scaleRatio;
            cardsParent.localPosition = new Vector3(
                _cardsParentInitialPosition.x,
                (_cardsParentInitialPosition.y + _maxScrollOffset * _scrollProgress) * scaleRatio,
                _cardsParentInitialPosition.z * scaleRatio
            );
        }
        public void Close()
        {
            Clear();
            if (_backdropRenderer != null)
                _backdropRenderer.gameObject.SetActive(false);

            if (backButton != null)
                backButton.SetActive(false);

            if (viewerPanel != null)
                viewerPanel.SetActive(false);

            if (endTurnButton != null)
                endTurnButton.SetActive(_endTurnButtonWasActive);
        }

        private void Clear()
        {
            if (cardsParent == null)
                return;

            foreach (Transform child in cardsParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void EnsureViewerVisuals()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            if (_mainCamera == null)
                Debug.LogWarning("DeckViewer: Main Camera not found; the dimmed backdrop will be skipped.", this);

            if (_solidSprite == null)
            {
                _solidTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
                {
                    name = "DeckViewerSolidPixel",
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp
                };
                _solidTexture.SetPixel(0, 0, Color.white);
                _solidTexture.Apply();
                _solidSprite = Sprite.Create(
                    _solidTexture,
                    new Rect(0f, 0f, 1f, 1f),
                    new Vector2(0.5f, 0.5f),
                    1f
                );
                _solidSprite.name = "DeckViewerSolidPixel";
            }

            if (_backdropRenderer == null && _mainCamera != null)
            {
                GameObject backdrop = new GameObject("DeckViewerBackdrop");
                backdrop.transform.SetParent(_mainCamera.transform, false);
                _backdropRenderer = backdrop.AddComponent<SpriteRenderer>();
                _backdropRenderer.sprite = _solidSprite;
                _backdropRenderer.color = backdropColor;
                _backdropRenderer.sortingOrder = backdropSortingOrder;
                backdrop.SetActive(false);
            }
        }

        private void UpdateBackdropTransform()
        {
            if (_backdropRenderer == null || _mainCamera == null || cardsParent == null)
                return;

            float depth = _mainCamera.WorldToViewportPoint(cardsParent.position).z;
            depth = Mathf.Max(depth, _mainCamera.nearClipPlane + 0.1f);

            Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
            Vector3 bottomRight = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
            Vector3 topLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
            Vector3 center = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, depth));

            Transform backdropTransform = _backdropRenderer.transform;
            backdropTransform.SetPositionAndRotation(center, _mainCamera.transform.rotation);
            backdropTransform.localScale = new Vector3(
                Vector3.Distance(bottomLeft, bottomRight),
                Vector3.Distance(bottomLeft, topLeft),
                1f
            );
        }

        private void OnDestroy()
        {
            ReleaseRuntimeObject(_backdropRenderer != null ? _backdropRenderer.gameObject : null);
            ReleaseRuntimeObject(_solidSprite);
            ReleaseRuntimeObject(_solidTexture);
        }

        private static void ReleaseRuntimeObject(Object runtimeObject)
        {
            if (runtimeObject == null)
                return;

            if (Application.isPlaying)
                Destroy(runtimeObject);
            else
                DestroyImmediate(runtimeObject);
        }
        
        
    }
}
