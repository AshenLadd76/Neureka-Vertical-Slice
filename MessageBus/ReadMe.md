\# MessageBus



A lightweight, generic message bus for broadcasting events in Unity.



---



\## Features



\- Centralized, static event hub.

\- Supports \*\*0 to 3 parameters\*\* per event.

\- Prevents duplicate subscriptions.

\- Optional \*\*tracing/logging\*\* for debugging.

\- Auto-clears all listeners on domain reload (editor/play mode).



---



\## Installation



Add this to your Unity project `Packages/manifest.json`:



```json

"com.ashenladd.messagebus": "https://gist.github.com/YOUR\_GIST\_ID.git"

```



\## Usage



Import the namespace:



```csharp

using ToolBox.Messaging;

```





\## Adding Listeners



\### Add a listener for an event by specifying the event name and a callback method. The callback can take 0–3 parameters of any type:



```csharp

// 0 parameters

MessageBus.AddListener("OnGameStart", OnGameStart);



// 1 parameter (generic)

MessageBus.AddListener<int>("OnScoreChanged", OnScoreChanged);



// 2 parameters (generic)

MessageBus.AddListener<string, int>("OnPlayerHit", OnPlayerHit);



// 3 parameters (generic)

MessageBus.AddListener<string, int, bool>("OnEnemyDefeated", OnEnemyDefeated);

```





\### Call Broadcast to trigger all listeners of an event, passing the appropriate number of parameters:



```csharp

// 0 parameters

MessageBus.Broadcast("OnGameStart");



// 1 parameter

MessageBus.Broadcast("OnScoreChanged", 42);



// 2 parameters

MessageBus.Broadcast("OnPlayerHit", "Enemy", 10);



// 3 parameters

MessageBus.Broadcast("OnEnemyDefeated", "Orc", 100, true);

```



\## Removing Listeners



\### Remove a previously registered listener to stop receiving events:



```csharp

// 0 parameters

MessageBus.RemoveListener("OnGameStart", OnGameStart);



// 1 parameter

MessageBus.RemoveListener<int>("OnScoreChanged", OnScoreChanged);



// 2 parameters

MessageBus.RemoveListener<string, int>("OnPlayerHit", OnPlayerHit);



// 3 parameters

MessageBus.RemoveListener<string, int, bool>("OnEnemyDefeated", OnEnemyDefeated);

```



\## Optional: Enable Tracing



\### For debugging, enable tracing to log all broadcasts:



```csharp

MessageBus.EnableTracing = true;

```









