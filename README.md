# 🧩 EasyTangibleTable for Unity

![EasyUR3-Preview](Docs/EasyUR3-Preview.png)

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-green.svg)
![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20Editor-lightgrey.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)

---

## 🚀 Overview

**EasyTangibleTable** is a lightweight Unity package for building tangible table applications using TUIO tracking systems.

It allows Unity apps to detect, track, and interact with fiducial markers (tags) on interactive surfaces.

The package provides a clean event-driven API and a component-based tag controller system, making it easy to create interactive installations, museum exhibits, and multi-object experiences.

### ⚙️ Features

- ✅ Detect tangible tags using TUIO protocol
- ✅ Track tag position, rotation, and movement
- ✅ Event-driven architecture
- ✅ Create custom tag behaviors
- ✅ Support for multiple tags simultaneously
- ✅ Built-in target alignment detection
- ✅ Clean EasyTT API
- ✅ Designed for interactive installations and R&D prototypes
  
Perfect for: 

**🧩 Tangible user interfaces**
**🎮 Interactive installations**
**🧪 R&D prototypes**
**🏛️ Museum installations**
**🎥 Multi-object interactive demos**

---

## 📦 Installation

### Install via Unity Package Manager (Git URL)

1. Open **Unity → Window → Package Manager**
2. Click **+** → **Add package from Git URL**
3. Paste the following:
[https://github.com/IreshSampath/unity-assets-easy-tangible-table.git](https://github.com/IreshSampath/unity-assets-easy-tangible-table.git)
5. Click **Install**

---

## 🧰 Quick Start

### ✅ Step 1 — Setup the Tangible Table Receiver

1. Go to **Package Manager → EasyTangibleTable → Samples**
3. Click **Import  → EasyTangibleTable Sample**
4. Drag the **EasyTangibleTable Demo** prefab into your scene
   
![EasyUR3-Prefab](Docs/EasyUR3-Prefab.png)

### ✅ Step 2 — Setup Tag Manager
Add EasyTangibleTagManager to your scene.

Assign tag prefabs or existing tag objects in the inspector.

The manager automatically:

creates tag controllers

updates tag positions

activates/deactivates tags

### ✅ Step 3 — Listen to Tag Events
EasyTangibleTable provides a simple API via EasyTT.

Example
```csharp
using GAG.EasyTangibleTable;

void OnEnable()
{
    EasyTT.TagAligned += OnTagAligned;
    EasyTT.TagRemoved += OnTagRemoved;
}

void OnDisable()
{
    EasyTT.TagAligned -= OnTagAligned;
    EasyTT.TagRemoved -= OnTagRemoved;
}

void OnTagAligned(int tagID)
{
    Debug.Log($"Tag aligned: {tagID}");
}

void OnTagRemoved(int tagID)
{
    Debug.Log($"Tag removed: {tagID}");
}
```

#### 🎮 Create Custom Tag Behaviours
Each tangible tag can have its own controller script.

```csharp
EasyTangibleTagControllerBase
```

Example
```csharp
using GAG.EasyTangibleTable;
using UnityEngine;

public class MyCustomTagController : EasyTangibleTagControllerBase
{
    protected override void OnTargetReached()
    {
        Debug.Log("Tag reached target!");
    }

    protected override void OnTargetDeparted()
    {
        Debug.Log("Tag left target!");
    }
}
```
Attach this script to your tag prefab.

#### 📡 Available Events
EasyTangibleTable exposes multiple events:
```csharp
EasyTT.TagPlaced
EasyTT.TagUpdated
EasyTT.TagMoved
EasyTT.TagRotated
EasyTT.ActiveTagsUpdated
EasyTT.TagAligned
EasyTT.TagAlignmentLost
EasyTT.TagRemoved
```
These allow you to build complex interactions.
Example uses:
- trigger UI actions
- control animations
- synchronize multiple tags
- start/stop experiences

---

## 🎨 Sample Scene
The package includes a sample scene demonstrating:
- multiple tag controllers
- UI interaction
- rotation-based states
- alignment detection
Example tag controllers included:
```csharp
TagAController
TagBController
TagCController
TagDController
```
---

## 📜 License
IT License — Free for commercial and personal use.

---

## 🙏 Thank You
Thanks for using EasyTangibleTable!
- Feel free to contribute
⭐ Star the repo
🐞 Report issues
🚀 Suggest improvements

---

## 👤 Author
Iresh Sampath 🔗 [LinkedIn Profile](https://www.linkedin.com/in/ireshsampath/)
