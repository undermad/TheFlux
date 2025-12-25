using UnityEditor;

namespace TheFlux.Game.GameStates.Gameplay.Scripts.CombatSystem.Editor
{

    [CustomEditor(typeof(GameplayEffectDef))]
    public class GameplayEffectDefEditor : UnityEditor.Editor
    {
        private SerializedProperty displayNameProp;
        private SerializedProperty grantedTagsProp;
        private SerializedProperty requiredTargetTagsProp;
        private SerializedProperty blockedTargetTagsProp;

        private SerializedProperty policyProp;
        private SerializedProperty durationSecondsProp;
        private SerializedProperty isPeriodicProp;
        private SerializedProperty periodProp;

        private SerializedProperty canStackProp;
        private SerializedProperty maxStacksProp;
        private SerializedProperty refreshDurationOnStackProp;

        private SerializedProperty modifiersProp;

        private void OnEnable()
        {
            // Meta
            displayNameProp = serializedObject.FindProperty("DisplayName");
            grantedTagsProp = serializedObject.FindProperty("GrantedTags");
            requiredTargetTagsProp = serializedObject.FindProperty("RequiredTargetTags");
            blockedTargetTagsProp = serializedObject.FindProperty("BlockedTargetTags");

            // Duration & Periodic
            policyProp = serializedObject.FindProperty("Policy");
            durationSecondsProp = serializedObject.FindProperty("DurationSeconds");
            isPeriodicProp = serializedObject.FindProperty("IsPeriodic");
            periodProp = serializedObject.FindProperty("Period");

            // Stacking
            canStackProp = serializedObject.FindProperty("CanStack");
            maxStacksProp = serializedObject.FindProperty("MaxStacks");
            refreshDurationOnStackProp = serializedObject.FindProperty("RefreshDurationOnStack");

            // Modifiers
            modifiersProp = serializedObject.FindProperty("Modifiers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // --- Meta ---
            EditorGUILayout.LabelField("Meta", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(displayNameProp);
            
            EditorGUILayout.Space();

            // --- Duration & Periodic ---
            EditorGUILayout.LabelField("Duration & Periodic", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(policyProp);
            
            var policy = (DurationPolicy) policyProp.enumValueIndex;
            if (policy is DurationPolicy.Duration or DurationPolicy.Infinite)
            {
                if (policy is not DurationPolicy.Infinite)
                {
                    EditorGUILayout.PropertyField(durationSecondsProp);
                }
                
                EditorGUILayout.PropertyField(isPeriodicProp);

                if (isPeriodicProp.boolValue)
                {
                    EditorGUILayout.PropertyField(periodProp);
                }
                
                EditorGUILayout.Space();
                
                // --- Stacking ---
                EditorGUILayout.LabelField("Stacking", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(canStackProp);
                if (canStackProp.boolValue)
                {
                    EditorGUILayout.PropertyField(maxStacksProp);
                    EditorGUILayout.PropertyField(refreshDurationOnStackProp);
                }
            }
            else
            {
                if (HasIgnoredDurationOrPeriodicData())
                {
                    EditorGUILayout.HelpBox(
                        "You have duration/periodic data set, but it will be ignored because the policy is Instant.",
                        MessageType.Warning
                    );
                }
            }
            
            EditorGUILayout.Space();
            
            // --- Tags ---
            EditorGUILayout.LabelField("Tags", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (policy is DurationPolicy.Duration or DurationPolicy.Infinite)
            {
                EditorGUILayout.PropertyField(grantedTagsProp);
            }
            else
            {
                if (HasIgnoredGrantedTags())
                {
                    EditorGUILayout.HelpBox(
                        "You have selected Instant policy, you should clear granted tags.",
                        MessageType.Warning
                    );
                }
            }
            EditorGUILayout.PropertyField(requiredTargetTagsProp);
            EditorGUILayout.PropertyField(blockedTargetTagsProp);

            EditorGUILayout.Space();

            // --- Modifiers ---
            EditorGUILayout.LabelField("Modifiers", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(modifiersProp, true);

            serializedObject.ApplyModifiedProperties();
        }
        
        private bool HasIgnoredDurationOrPeriodicData()
        {
            var hasDurationData = false;
            var hasPeriodicData = false;

            if (durationSecondsProp != null)
            {
                var valueProp = durationSecondsProp.FindPropertyRelative("Value");
                hasDurationData = valueProp != null && valueProp.floatValue != 0f;
            }
            
            if (periodProp != null)
            {
                var valueProp = periodProp.FindPropertyRelative("Value");
                hasPeriodicData = isPeriodicProp.boolValue || (valueProp != null && valueProp.floatValue != 0f);
            }

            return hasDurationData || hasPeriodicData;
        }
        
        private bool HasIgnoredGrantedTags()
        {
            if (grantedTagsProp != null)
            {
                var tagListProp = grantedTagsProp.FindPropertyRelative("tags");
                if (tagListProp != null && tagListProp.arraySize > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}