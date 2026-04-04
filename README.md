# Neureka Vertical Slice


The purpose of this vertical slice is to architect a core framework that enables fast, modular design of apps and games. It demonstrates how a data-driven, flexible architecture combined with UI Toolkit and my custom Fluent UI framework allows developers to quickly build and extend interactive systems while keeping content easy to update for non-technical users.
<br>
<br>

<p align="center">
  <img src="Documentation/neureka.gif" width="240" alt="Neureka Demo"/>
</p>

<br>
The original Neureka app gamifies cognitive assessments, collecting data through questionnaires and interactive tasks to support research in dementia and mental health. This vertical slice demonstrates the core functionality of the app, allowing users to complete tasks, view results, and interact with dynamic content in a realistic environment.
<br>
<br>
The app was built using UGUI, while this vertical slice is data driven, is built with UI Toolkit and my custom framework, Fluent UI, which sits on top of VisualElement and enables fast, dynamic UI content generation. 

<br>
<br>

The app is data-driven and modular, so developers can quickly extend it and non-technical users can update content or tasks without changing code. This architecture makes the app easy to maintain, extend, and iterate on, and could be applied to other apps or games requiring modular, data-driven design.

<br>
<br>


## Table of Contents
- [Overview](#neureka-vertical-slice)
- [Installation](#installation)
- [Features](#features)
- [Core Systems](#core-systems)
    - [FLuentUi](#fluentui)
    - [Document Service](#document-service)
    - [Questionnaire Service](#questionnaire-service)
    - [Audio Service](#audio-service)
    - [Web Service](#Web-service)
    - [Audio Service](#audio-service)
    - [File Importer](#file-importer)
    - [Message Bus](#message-bus)
    - [Haptics Service](#haptics-service)
 - [Conclusion](#conclusion)






<br>
<br>
  
## 📌 Installation

- Install Unity Hub
- Install Unity 6.3 LTS (6000.3.11f1) via Unity Hub
- Clone the repository:

```
bash git clone https://github.com/AshenLadd76/Neureka-Vertical-Slice.git

cd Neureka-Vertical-Slice
```

- Add the project in Unity Hub and open it with Unity 6.3 LTS
- Open the main scene: Assets/Scenes/NeurekaAppSlice.unity
- Hit Play ▶️ in the Editor to run the project


<br>
<br>


## Features

- UI Toolkit & Fluent UI framework for editor and runtime UI
- Dynamic content generation from JSON/CSV files
- Core services are automatically bootstrapped and persist across scene loads, with full support for single-scene and multi-scene projects.
- Modular architecture with self-contained systems
- Event-driven messaging between modules using a custom message bus, imaginatively named Message Bus 
- Rapid prototyping and experimentation with UI and content
- UI styling is handled via USS, allowing layouts and visual themes to be adjusted, extended, or replaced without modifying code


<br>
<br>

## Core Systems
<br>
<br>

## FluentUi
Fluent UI is a lightweight framework built on top of UI Toolkit that uses the Curiously Recurring Template Pattern (CRTP) to implement a fluent builder for low-level UI components like buttons, labels, and containers. It was created as an alternative to UXML, enabling data-driven UI to be built entirely in code, with each component self-contained and independent of the scene. Fluent UI supports both editor and runtime UI.

<br>
<br>

## Audio Service
A custom audio service for managing SFX and music. Handles one-shot sounds, positional audio, and looped tracks with optional cross-fading. Uses object pooling for audio sources to minimize runtime allocations, and coroutines to automatically clean up finished sounds. Audio can be triggered through a simple message bus, or accessed directly and injected via an interface for flexible integration into your project.

<br>
<br>

## Document Service 
In this context, a document is a self-contained portion of the app — for example, the navigation UI, a game, or an assessment. 

The Document Service manages a collection of documents that are **lazy-loaded**: each document is built and loaded only when needed. Documents can also be **optionally cached** for persistent use. 

All document requests go through the **Message Bus**, and each document is self contained and responsible for building its own UI and managing its own state.

<br>
<br>

## Questionnaire Service  
The Questionnaire Service builds and displays questionnaires for the user. It listens for requests via the **Message Bus**, loads the correct **questionnaire ScriptableObject**, and uses its data with **Fluent UI** to dynamically construct the questionnaire interface.

This design makes adding or updating questionnaires straightforward and keeps the UI fully decoupled from other services.
<br>
[Questionnaire Service Flow (PDF)](Documentation/Questionnaire_Service_Diagram.pdf)

<br>
<br>

## Web Service
The Web Service handles requests from other services to handle web requests such as upload data to a web server. 

<br>
<br>


## File Importer
The **File Importer** system includes:
- Drag and Drop file importer
- Dispatch Manager
- Parser Manager
- Individual file parsers
- Can easily be extended to support other file types

You can drag JSON or CSV files into the project, and the system automatically generates the corresponding **questionnaire ScriptableObjects**, ready to be used by the **Questionnaire Service**. It's easy to extend and support new file types and add new parsers
<br>
  [File Importer Flow (PDF)](Documentation/File_Import_Architecture.pdf)
  
<br>
<br>


## [Message Bus](https://github.com/AshenLadd76/MessageBus)
The message bus facilitates communication between all relevant services and coordinates interactions between scene-level systems and the UI, removing the need for direct dependencies.

<br>
<br>


## Haptics Service
A custom Java plugin that gives direct access to an Android device’s vibration functionality. Unlike Unity’s basic vibrate call, it enables control of the duration and intensity of the vibration, giving more flexibility for feedback in the app.

<br>
<br>


## Conclusion
This vertical slice demonstrates a modular, event-driven Unity architecture with dynamic content, bootstrapped services, and a flexible UI framework. 

<br>
<br>

<p align="center">
  <img src="ScreenShots/appslice.png"  alt="Neureka Demo"/>
</p>

<br>
<br>














