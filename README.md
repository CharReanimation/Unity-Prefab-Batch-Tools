# Prefab Batch Tools

[English](README.md) | [简体中文](README_CN.md)

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

## 📸 Preview

*Tools Menu*  
![Tools Menu](Preview/ToolsMenu.png)

*Prefab Adder*  
![Prefab Adder](Preview/PrefabAdder.png)

*Prefab Deleter*  
![Prefab Deleter](Preview/PrefabDeleter.png)

## 📦 Installation

Place the scripts inside an `Editor` folder in your Unity project:

```text
Assets/
└── Editor/
    └── PrefabBatchAdder/
        ├── PrefabAdder.cs
        └── PrefabDeleter.cs
```

No additional packages or dependencies are required.

After importing the scripts, the tools will appear in the Unity Editor menu:

```text
Tools
├── Prefab Adder
└── Prefab Deleter
```

## ↩️ Undo Support

Both tools support Unity's Undo system.

### Prefab Adder

Created Prefab Instances can be undone using `Ctrl + Z` or **Edit > Undo**.

### Prefab Deleter

Deleted Prefab Instances can also be restored through Unity's Undo system.

## 🎯 Use Cases

Prefab Batch Tools can be useful for:

- Level Design
- Environment Setup
- Scene Organization
- Batch scene editing
- Adding repeated environment elements
- Adding decorations or interaction objects
- Quickly cleaning up Prefab Instances
- Reducing repetitive Hierarchy operations

## 📋 Requirements

- Unity 2022.3 or later
- Unity Editor
- Compatible with URP
- Compatible with Built-in Render Pipeline
- Editor-only tools

## ⚠️ Notes

- These tools are intended for Unity Editor workflows and should be placed inside an `Editor` folder.
- The tools operate on the currently selected GameObjects in the Unity Hierarchy.
- No runtime components are required.

## 📄 License

This project is provided as-is.

You are free to use and modify these tools according to the license included with this repository.
