# Prefab 批处理工具

一个简单而实用的 Unity Editor Prefab 批处理工具集。

主要用于快速向多个选中的 GameObject 添加 Prefab，或批量删除选中对象下的 Prefab Instance，减少重复的 Hierarchy 操作。

## ✨ 功能

### Prefab Adder

批量将指定 Prefab 添加到多个选中的 GameObject 下。

- 支持同时选择多个 GameObject
- 将 Prefab 自动作为子物体添加
- 自动设置 Local Position 为 `(0, 0, 0)`
- 自动设置 Local Rotation 为 `Identity`
- 自动设置 Local Scale 为 `(1, 1, 1)`
- 支持禁止重复添加相同 Prefab
- 支持 Unity Undo

### Prefab Deleter

批量删除选中 GameObject 下的 Prefab Instance。

支持：

- 删除某种 Prefab
- 删除所有 Prefab Instance
- 仅删除直接子物体
- 递归检查 Nested Children
- 当指定 Prefab 不存在时，可选择删除其他 Prefab
- 支持 Unity Undo
- 显示删除数量

## 📦 安装

将脚本放入 Unity 项目的 `Editor` 文件夹中，例如：

```text
Assets/
└── Editor/
    ├── PrefabAdder.cs
    └── PrefabDeleter.cs