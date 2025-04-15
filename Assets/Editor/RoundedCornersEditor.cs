using UnityEngine;
using UnityEditor;

namespace Emi.Unity
{
    [CustomEditor(typeof(RoundedCorners))]
    [CanEditMultipleObjects]
    public sealed class RoundedCornersEditor : Editor
    {
        private SerializedProperty sides = null;
        private SerializedProperty isElliptical = null;
        private SerializedProperty radius1 = null;
        private SerializedProperty radius2 = null;
        private SerializedProperty radius3 = null;
        private SerializedProperty radius4 = null;

        private RadiusLabels topLeftLabels;
        private RadiusLabels topLabels;
        private RadiusLabels topRightLabels;
        private RadiusLabels rightLabels;
        private RadiusLabels bottomRightLabels;
        private RadiusLabels bottomLabels;
        private RadiusLabels bottomLeftLabels;
        private RadiusLabels leftLabels;
        private RadiusLabels allLabels;
        private GUIContent absoluteValueLabel;
        private GUIContent relativeValueLabel;

        private void OnEnable()
        {
            sides = serializedObject.FindProperty("sides");
            isElliptical = serializedObject.FindProperty("isElliptical");
            radius1 = serializedObject.FindProperty("radius1");
            radius2 = serializedObject.FindProperty("radius2");
            radius3 = serializedObject.FindProperty("radius3");
            radius4 = serializedObject.FindProperty("radius4");

            topLeftLabels = new RadiusLabels("Top-left");
            topLabels = new RadiusLabels("Top");
            topRightLabels = new RadiusLabels("Top-right");
            rightLabels = new RadiusLabels("Right");
            bottomRightLabels = new RadiusLabels("Bottom-right");
            bottomLabels = new RadiusLabels("Bottom");
            bottomLeftLabels = new RadiusLabels("Bottom-left");
            leftLabels = new RadiusLabels("Left");
            allLabels = new RadiusLabels("All");
            absoluteValueLabel = new GUIContent("Abs");
            relativeValueLabel = new GUIContent("Rel");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(sides);
            EditorGUILayout.PropertyField(isElliptical);

            switch ((RoundedCorners.Sides)sides.intValue)
            {
                case RoundedCorners.Sides.None:
                    DoRadiusGUI(radius1, topLeftLabels);
                    DoRadiusGUI(radius2, topRightLabels);
                    DoRadiusGUI(radius3, bottomRightLabels);
                    DoRadiusGUI(radius4, bottomLeftLabels);
                    break;
                case RoundedCorners.Sides.All:
                    DoRadiusGUI(radius1, allLabels);
                    break;
                case RoundedCorners.Sides.Horizontal:
                    DoRadiusGUI(radius1, leftLabels);
                    DoRadiusGUI(radius2, rightLabels);
                    break;
                case RoundedCorners.Sides.Vertical:
                    DoRadiusGUI(radius1, topLabels);
                    DoRadiusGUI(radius2, bottomLabels);
                    break;
                case RoundedCorners.Sides.LeftSide:
                    DoRadiusGUI(radius1, leftLabels);
                    DoRadiusGUI(radius2, topRightLabels);
                    DoRadiusGUI(radius3, bottomRightLabels);
                    break;
                case RoundedCorners.Sides.TopSide:
                    DoRadiusGUI(radius1, topLabels);
                    DoRadiusGUI(radius2, bottomRightLabels);
                    DoRadiusGUI(radius3, bottomLeftLabels);
                    break;
                case RoundedCorners.Sides.RightSide:
                    DoRadiusGUI(radius1, topLeftLabels);
                    DoRadiusGUI(radius2, rightLabels);
                    DoRadiusGUI(radius3, bottomLeftLabels);
                    break;
                case RoundedCorners.Sides.BottomSide:
                    DoRadiusGUI(radius1, topLeftLabels);
                    DoRadiusGUI(radius2, topRightLabels);
                    DoRadiusGUI(radius3, bottomLabels);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DoRadiusGUI(SerializedProperty property, RadiusLabels labels)
        {
            if (isElliptical.boolValue)
            {
                DoRadiusComponentGUI(property.FindPropertyRelative("x"), labels.X);
                DoRadiusComponentGUI(property.FindPropertyRelative("y"), labels.Y);
            }
            else
            {
                DoRadiusComponentGUI(property.FindPropertyRelative("x"), labels.Default);
            }
        }

        private void DoRadiusComponentGUI(SerializedProperty property, GUIContent label)
        {
            var previousLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(label);

                EditorGUIUtility.labelWidth = Mathf.Floor(EditorStyles.label.CalcSize(absoluteValueLabel).x * 1.5f);
                EditorGUILayout.PropertyField(property.FindPropertyRelative("absoluteValue"), absoluteValueLabel);

                EditorGUILayout.Space(12.0f, false);

                EditorGUIUtility.labelWidth = Mathf.Floor(EditorStyles.label.CalcSize(relativeValueLabel).x * 1.5f);
                EditorGUILayout.PropertyField(property.FindPropertyRelative("relativeValue"), relativeValueLabel);
            }
            finally
            {
                EditorGUIUtility.labelWidth = previousLabelWidth;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
        }

        private struct RadiusLabels
        {
            public readonly GUIContent Default;
            public readonly GUIContent X;
            public readonly GUIContent Y;

            public RadiusLabels(string label)
            {
                this.Default = new GUIContent(label);
                this.X = new GUIContent($"{label} X");
                this.Y = new GUIContent($"{label} Y");
            }
        }
    }
}
