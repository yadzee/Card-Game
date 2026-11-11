using Gameplay.Cards;
using UI;
using UnityEditor;
using UnityEngine;

namespace GameEditor.CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        private const string PrefabPath =
            "Assets/Prefabs/CardPrefabPure.prefab";

        private CardData _selectedCard;
        private SerializedObject _serializedCard;

        private PreviewRenderUtility _previewUtility;
        private GameObject _previewInstance;

        [MenuItem("Tools/Card Editor")]
        private static void OpenWindow()
        {
            GetWindow<CardEditorWindow>("Card Editor");
        }

        private void OnEnable()
        {
            _previewUtility = new PreviewRenderUtility();
            _previewUtility.cameraFieldOfView = 30f;

            minSize = new Vector2(700, 450);
        }

        private void OnDisable()
        {
            if (_previewUtility != null)
            {
                _previewUtility.Cleanup();
                _previewUtility = null;
                _previewInstance = null;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(
                "Card Editor",
                EditorStyles.boldLabel
            );

            if (GUILayout.Button("Create New Card", GUILayout.Width(140)))
            {
                CreateNewCard();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();

            CardData selectedCard = (CardData)EditorGUILayout.ObjectField(
                "Card Data",
                _selectedCard,
                typeof(CardData),
                false
            );

            bool selectionChanged = EditorGUI.EndChangeCheck();

            if (selectionChanged)
            {
                _selectedCard = selectedCard;

                _serializedCard = _selectedCard != null
                    ? new SerializedObject(_selectedCard)
                    : null;

                UpdatePreview();
            }

            EditorGUILayout.Space();

            if (_selectedCard == null)
            {
                EditorGUILayout.HelpBox(
                    "Select a CardData asset to edit.",
                    MessageType.Info
                );

                return;
            }

            EditorGUILayout.BeginHorizontal();

            // Card data fields
            EditorGUILayout.BeginVertical(
                GUILayout.Width(360)
            );

            _serializedCard.Update();

            SerializedProperty property = _serializedCard.GetIterator();
            bool enterChildren = true;

            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (property.propertyPath == "m_Script")
                    continue;

                EditorGUILayout.PropertyField(property, true);
            }

            bool dataChanged = _serializedCard.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();

            // Card preview
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(
                "Preview",
                EditorStyles.boldLabel
            );

            Rect previewRect = GUILayoutUtility.GetRect(
                220,
                320,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );

            DrawPreview(previewRect);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (dataChanged)
            {
                UpdatePreview();
            }
        }

        private void UpdatePreview()
        {
            if (_selectedCard == null || _previewUtility == null)
                return;

            if (_previewInstance == null)
            {
                GameObject prefab =
                    AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);

                if (prefab == null)
                {
                    Debug.LogError(
                        $"Card prefab not found at: {PrefabPath}"
                    );

                    return;
                }

                _previewInstance = Instantiate(prefab);
                _previewUtility.AddSingleGO(_previewInstance);
            }

            UiCardLink cardLink =
                _previewInstance.GetComponent<UiCardLink>();

            if (cardLink == null)
            {
                Debug.LogError(
                    "CardPrefabPure does not contain UiCardLink."
                );

                return;
            }

            Card card = new Card(_selectedCard);
            cardLink.Initialize(card);

            Repaint();
        }
        
        private void CreateNewCard()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create New Card",
                "NewCard",
                "asset",
                "Choose where to save the new card.",
                "Assets/GameData/CardData"
            );

            if (string.IsNullOrEmpty(path))
                return;

            CardData newCard = CreateInstance<CardData>();

            AssetDatabase.CreateAsset(newCard, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _selectedCard = newCard;
            _serializedCard = new SerializedObject(newCard);

            UpdatePreview();
            Repaint();
        }

        private void DrawPreview(Rect previewRect)
        {
            if (_previewInstance == null || _previewUtility == null)
                return;

            Camera previewCamera = _previewUtility.camera;

            Bounds bounds = CalculateBounds(_previewInstance);

            float aspect = previewRect.width / previewRect.height;

            previewCamera.orthographic = true;
            previewCamera.aspect = aspect;

            previewCamera.orthographicSize = Mathf.Max(
                bounds.extents.y,
                bounds.extents.x / aspect
            ) * 1.15f;

            float distance = Mathf.Max(
                bounds.extents.z + 10f,
                10f
            );

            previewCamera.transform.position =
                bounds.center + Vector3.back * distance;

            previewCamera.transform.rotation = Quaternion.identity;

            previewCamera.nearClipPlane = 0.01f;
            previewCamera.farClipPlane = 1000f;

            previewCamera.clearFlags = CameraClearFlags.Color;
            previewCamera.backgroundColor =
                new Color(0.18f, 0.18f, 0.18f, 1f);

            _previewUtility.BeginPreview(previewRect, GUIStyle.none);

            previewCamera.Render();

            Texture previewTexture = _previewUtility.EndPreview();

            GUI.DrawTexture(
                previewRect,
                previewTexture,
                ScaleMode.ScaleToFit,
                false
            );
        }

        private Bounds CalculateBounds(GameObject target)
        {
            Renderer[] renderers =
                target.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
                return new Bounds(target.transform.position, Vector3.one);

            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }
    }
}
