# Prefab Batch Tools

A simple and practical set of Unity Editor tools for batch processing Prefabs.

These tools allow you to quickly add a Prefab to multiple selected GameObjects or remove Prefab Instances from selected objects, reducing repetitive Hierarchy operations.

## ✨ Features

### Prefab Adder

Batch-add a selected Prefab to multiple GameObjects.

- Select multiple GameObjects at once
- Automatically adds the Prefab as a child
- Sets Local Position to `(0, 0, 0)`
- Sets Local Rotation to `Identity`
- Sets Local Scale to `(1, 1, 1)`
- Optional duplicate prevention
- Supports Unity Undo

### Prefab Deleter

Batch-remove Prefab Instances from selected GameObjects.

Supports:

- Remove a specific Prefab
- Remove all Prefab Instances
- Direct children only
- Recursive search through nested children
- Option to remove other Prefabs when the target Prefab is not found
- Supports Unity Undo
- Displays the number of deleted Prefab Instances

## 📦 Installation

Place the scripts inside an `Editor` folder in your Unity project:

```text
Assets/
└── Editor/
    ├── PrefabAdder.cs
    └── PrefabDeleter.cs