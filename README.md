# Neureka Vertical Slice

The original Neureka App was designed to gamify interactive cognitive assessments and collect data using questionnaires and tasks. Its purpose is to collect data that supports research into dementia and mental health.

This vertical slice demonstrates how modular architecture and dynamic content generation can make an app easy to extend for developers and simple to update for non-technical users.

## Key Features

- UI Toolkit & Fluent UI framework for editor and runtime UI
- Dynamic content generation from JSON/CSV files
- Modular architecture with self-contained systems
- Event-driven messaging between modules using a custom message bus, imaginatively named Message Bus 
- Rapid prototyping and experimentation with UI and content
- UI styling is handled via USS, allowing layouts and visual themes to be adjusted, extended, or replaced without modifying code



### Core Systems.

#### File Importer
File Importer, which encompasses the Drag and Drop file importer, the Dispatch Manager, the Parser Manager, and the parsers themselves. This system lets you drag and drop JSON or CSV files into the project, automatically generating the corresponding questionnaire ScriptableObjects which are available for loading by the questionnaire service. 

### Questionnaire Service  
The Questionnaire Service is a bootstrapped service responsible for building and displaying questionnaires. It listens for incoming requests via the Message Bus, loads the correct questionnaire ScriptableObject, and uses its data along with Fluent UI to dynamically construct the questionnaire UI for the user. This system makes adding or updating questionnaires straightforward and keeps the UI fully decoupled from other services.

### Document Service. 
In this context, a document represents a self-contained portion of the app, for example, the navigation UI, a game, or an assessment. The Document Service maintains a dictionary of lazy-loaded documents (only loaded when needed) that are dynamically built and can be optionally cached for persistent use. Document requests are handled via the Message Bus, and each document builds its own UI and manages its own state. 

### Message Bus
The message bus facilitates communication between all relevant services and coordinates interactions between scene-level systems and the UI, removing the need for direct dependencies.















