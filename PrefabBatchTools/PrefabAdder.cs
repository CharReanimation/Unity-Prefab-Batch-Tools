#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class PrefabAdder : EditorWindow
{
    private GameObject prefab;
    private bool allowDuplicate = false;


    [MenuItem("Tools/Prefab Batch Tools/Prefab Adder")]
    private static void OpenWindow()
    {
        GetWindow<PrefabAdder>("Prefab Adder");
    }




    // ON GUI
    private void OnGUI()
    {
        // Space
        EditorGUILayout.Space(10);

        // GUI: Label
        EditorGUILayout.LabelField(
            "Add Prefab To Selected GameObjects",
            EditorStyles.boldLabel
        );

        // Space
        EditorGUILayout.Space(10);

        // GUI Select prefab
        prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab",
            prefab,
            typeof(GameObject),
            false
        );

        // Space
        EditorGUILayout.Space(5);

        // GUI: Allow Duplicate
        allowDuplicate = EditorGUILayout.Toggle(
            "Allow Duplicate",
            allowDuplicate
        );

        // Space
        EditorGUILayout.Space(15);

        // Selected objects
        GameObject[] selectedObjects = Selection.gameObjects;

        // GUI: Selected objects
        EditorGUILayout.LabelField(
            $"Selected Objects: {selectedObjects.Length}"
        );

        // Space
        EditorGUILayout.Space(10);

        GUI.enabled = prefab != null && selectedObjects.Length > 0;

        if (GUILayout.Button("Add Prefab", GUILayout.Height(35)))
        {
            AddPrefabToSelectedObjects();
        }

        GUI.enabled = true;

        // Space
        EditorGUILayout.Space(15);

        // GUI: Help box
        EditorGUILayout.HelpBox(
            "The prefab will be instantiated as a child of every selected GameObject.",
            MessageType.Info
        );
    }


    // Add Prefab to selected obejects
    private void AddPrefabToSelectedObjects()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab Adder: No prefab selected.");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("Prefab Adder: No GameObjects selected.");
            return;
        }


        // Add prefab to selected objects
        int addedCount = 0;
        foreach (GameObject parent in selectedObjects)
        {
            if (parent == null)
                continue;

            // Prevent added to prefab
            if (parent == prefab)
            {
                Debug.LogWarning(
                    $"Prefab Adder: Skipping '{parent.name}' because it is the prefab itself."
                );

                continue;
            }

            // Cannot Duplicate
            if (!allowDuplicate && HasPrefabChild(parent, prefab))
            {
                continue;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (instance == null)
            {
                Debug.LogError(
                    $"Prefab Adder: Failed to instantiate prefab '{prefab.name}'."
                );

                continue;
            }

            // Undo
            Undo.RegisterCreatedObjectUndo(
                instance,
                $"Add {prefab.name}"
            );

            // Set prefab parent to be selected objects
            Transform instanceTransform = instance.transform;
            instanceTransform.SetParent(
                parent.transform,
                false
            );

            // Set location, rotation
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localRotation = Quaternion.identity;
            instanceTransform.localScale = Vector3.one;

            addedCount++;
        }
        Debug.Log($"Prefab Adder: Added '{prefab.name}' to {addedCount} GameObject(s).");
    }


    // Has Prefab Child
    private bool HasPrefabChild(GameObject parent, GameObject targetPrefab)
    {
        Transform parentTransform = parent.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            GameObject childObject = child.gameObject;
            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(childObject);

            if (sourcePrefab == targetPrefab)
            {
                return true;
            }
        }
        return false;
    }
}

#endif