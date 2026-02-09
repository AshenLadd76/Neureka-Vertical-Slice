# Neureka Vertical Slice

The original Neureka App was designed to gamify interactive cognitive assessments and collect data using questionnaires and tasks. Its purpose is to collect data that supports research into dementia and mental health.

This vertical slice demonstrates how modular architecture and dynamic content generation can make an app easy to extend for developers and simple to update for non-technical users.

## Key Features

- UI Toolkit & Fluent UI framework for editor and runtime UI
- Dynamic content generation from JSON/CSV files
- Modular architecture with self-contained systems
- Event-driven messaging between modules using a custom message bus, imaginatively named Message Bus 
- Rapid prototyping and experimentation with UI and content


### Core Systems.

#### File Importer

''' File Importer, which encompasses the Drag and Drop file importer, the Dispatch Manager, the Parser Manager, and the parsers themselves. This system lets you drag and drop JSON or CSV files into the project, automatically generating the corresponding questionnaire ScriptableObjects. '''












