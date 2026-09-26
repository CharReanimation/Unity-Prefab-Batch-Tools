#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class PrefabDeleter : EditorWindow
{
    [Header("Settings")]
    private GameObject prefab;
    private bool removeTargetPrefabOnly = true;
    private bool includeNestedChildren = false;


    [MenuItem("Tools/Prefab Deleter")]
    private static void OpenWindow()
    {
        GetWindow<PrefabDeleter>("Prefab Deleter");
    }




    // ON GUI
    private void OnGUI()
    {
        // Space
        EditorGUILayout.Space(10);

        // GUI: Label
        EditorGUILayout.LabelField(
            "Delete Prefabs From Selected GameObjects",
            EditorStyles.boldLabel
        );

        // Space
        EditorGUILayout.Space(10);

        // GUI: Prefab
        prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab",
            prefab,
            typeof(GameObject),
            false
        );

        // Space
        EditorGUILayout.Space(5);

        // GUI: Remove Target Prefab Only
        removeTargetPrefabOnly = EditorGUILayout.Toggle(
            "Remove Target Prefab Only",
            removeTargetPrefabOnly
        );

        // Space
        EditorGUILayout.Space(5);

        // GUI: Include Nested Children
        includeNestedChildren = EditorGUILayout.Toggle(
            "Include Nested Children",
            includeNestedChildren
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

        GUI.enabled = selectedObjects.Length > 0;

        if (GUILayout.Button("Delete Prefabs", GUILayout.Height(35)))
        {
            DeletePrefabsFromSelectedObjects();
        }

        GUI.enabled = true;

        // Space
        EditorGUILayout.Space(15);

        // GUI: Help box
        if (prefab == null)
        {
            EditorGUILayout.HelpBox(
                includeNestedChildren
                    ? "No Prefab selected. Deletes all Prefab Instances inside the selected GameObjects, including nested children."
                    : "No Prefab selected. Deletes all Prefab Instances that are direct children of the selected GameObjects.",
                MessageType.Info
            );
        }
        else if (removeTargetPrefabOnly)
        {
            EditorGUILayout.HelpBox(
                includeNestedChildren
                    ? $"Only '{prefab.name}' will be deleted. If it is not found, nothing will be deleted."
                    : $"Only '{prefab.name}' will be deleted from direct children. If it is not found, nothing will be deleted.",
                MessageType.Info
            );
        }
        else
        {
            EditorGUILayout.HelpBox(
                includeNestedChildren
                    ? $"If '{prefab.name}' exists, only it will be deleted. If it does not exist, all other Prefab Instances will be deleted."
                    : $"If '{prefab.name}' exists, only it will be deleted. If it does not exist, all other direct Prefab children will be deleted.",
                MessageType.Info
            );
        }
    }


    // Delete Prefabs from selected objects
    private void DeletePrefabsFromSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning(
                "Prefab Deleter: No GameObjects selected."
            );

            return;
        }


        int deletedCount = 0;

        foreach (GameObject parent in selectedObjects)
        {
            if (parent == null)
                continue;

            // =====================================================
            // No specific prefab selected
            // =====================================================

            if (prefab == null)
            {
                if (includeNestedChildren)
                {
                    deletedCount += DeletePrefabInstancesRecursive(
                        parent.transform
                    );
                }
                else
                {
                    deletedCount += DeleteDirectPrefabChildren(
                        parent.transform
                    );
                }

                continue;
            }


            // =====================================================
            // Specific prefab selected
            // =====================================================

            // Target prefab exists
            bool hasTargetPrefab = HasPrefab(parent.transform);
            if (hasTargetPrefab)
            {
                if (includeNestedChildren)
                {
                    deletedCount += DeleteSpecificPrefabRecursive(
                        parent.transform
                    );
                }
                else
                {
                    deletedCount += DeleteSpecificPrefabDirect(
                        parent.transform
                    );
                }

                continue;
            }


            // =====================================================
            // Target prefab NOT found
            // =====================================================

            if (removeTargetPrefabOnly)
            {
                // Do nothing
                Debug.Log(
                    $"Prefab Deleter: Prefab '{prefab.name}' not found from GameObject '{parent.name}'."
                );
            }
            else
            {
                // Original behavior:
                // Delete all OTHER prefab instances.
                if (includeNestedChildren)
                {
                    deletedCount += DeleteOtherPrefabInstancesRecursive(
                        parent.transform
                    );
                }
                else
                {
                    deletedCount += DeleteOtherPrefabInstancesDirect(
                        parent.transform
                    );
                }
            }
        }

        Debug.Log(
            $"Prefab Deleter: Deleted {deletedCount} Prefab Instance(s) from {selectedObjects.Length} GameObject(s)."
        );
    }


    // =========================================================
    // Specific Prefab
    // =========================================================

    // Check if selected prefab exists
    private bool HasPrefab(Transform parent)
    {
        if (includeNestedChildren)
        {
            return HasPrefabRecursive(parent);
        }

        return HasPrefabDirect(parent);
    }


    // Check specific prefab - Direct
    private bool HasPrefabDirect(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (IsTargetPrefab(child.gameObject))
            {
                return true;
            }
        }

        return false;
    }


    // Check specific prefab - Recursive
    private bool HasPrefabRecursive(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (IsTargetPrefab(child.gameObject))
            {
                return true;
            }

            if (HasPrefabRecursive(child))
            {
                return true;
            }
        }

        return false;
    }


    // Delete specific prefab - Direct
    private int DeleteSpecificPrefabDirect(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsTargetPrefab(child.gameObject))
            {
                // Undo
                Undo.DestroyObjectImmediate(child.gameObject);

                deletedCount++;
            }
        }
        return deletedCount;
    }


    // Delete specific prefab - Recursive
    private int DeleteSpecificPrefabRecursive(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsTargetPrefab(child.gameObject))
            {
                Undo.DestroyObjectImmediate(
                    child.gameObject
                );

                deletedCount++;
                continue;
            }

            deletedCount += DeleteSpecificPrefabRecursive(child);
        }
        return deletedCount;
    }


    // =========================================================
    // Other Prefabs
    // =========================================================

    // Delete all other prefab instances - Direct
    private int DeleteOtherPrefabInstancesDirect(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsPrefabInstance(child.gameObject)
                && !IsTargetPrefab(child.gameObject))
            {
                Undo.DestroyObjectImmediate(
                    child.gameObject
                );

                deletedCount++;
            }
        }

        return deletedCount;
    }


    // Delete all other prefab instances - Recursive
    private int DeleteOtherPrefabInstancesRecursive(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsPrefabInstance(child.gameObject)
                && !IsTargetPrefab(child.gameObject))
            {
                Undo.DestroyObjectImmediate(
                    child.gameObject
                );

                deletedCount++;
                continue;
            }

            deletedCount += DeleteOtherPrefabInstancesRecursive(
                child
            );
        }

        return deletedCount;
    }


    // =========================================================
    // All Prefabs
    // =========================================================

    // Delete all direct prefab children
    private int DeleteDirectPrefabChildren(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsPrefabInstance(child.gameObject))
            {
                Undo.DestroyObjectImmediate(
                    child.gameObject
                );

                deletedCount++;
            }
        }

        return deletedCount;
    }


    // Delete all prefab instances recursively
    private int DeletePrefabInstancesRecursive(Transform parent)
    {
        int deletedCount = 0;

        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);

            if (IsPrefabInstance(child.gameObject))
            {
                Undo.DestroyObjectImmediate(
                    child.gameObject
                );

                deletedCount++;
                continue;
            }

            deletedCount += DeletePrefabInstancesRecursive(
                child
            );
        }

        return deletedCount;
    }


    // =========================================================
    // Prefab Check
    // =========================================================

    // Check if GameObject is Prefab Instance
    private bool IsPrefabInstance(GameObject target)
    {
        if (target == null) return false;

        PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(target);

        return status == PrefabInstanceStatus.Connected
            || status == PrefabInstanceStatus.MissingAsset;
    }


    // Check if GameObject is the selected Prefab
    private bool IsTargetPrefab(GameObject target)
    {
        if (target == null || prefab == null) return false;

        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(target);

        return sourcePrefab == prefab;
    }
}

#endif