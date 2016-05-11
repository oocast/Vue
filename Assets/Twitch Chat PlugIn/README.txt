Structure:
/Twitch Chat PlugIn				Twitch integration scripts and prefabs, to hand over to client
/Twitch Chat PlugIn/Prefabs		Prefabs of chat bot and message ticker
/Twitch Chat PlugIn/Scripts		Scripts of chat bot and message ticker
/Twitch Chat PlugIn/Vote		Prefabs and scripts for votes

Usage:
1. Chat Bot
(1) Drag "Twitch Chat Bot" (under /Twitch Chat PlugIn/Prefabs) to the game hierarchy

2. Vote System
(1) Drag "Twitch Vote" prefab (under /Twitch Chat PlugIn/Vote/Prefabs) to the game hierarchy

3. Message Tickers
(1) Set Canvas Scaler component of canvas object, "Ui Scale Mode" to "Scale With Screen Size", "Reference Resolution" to 400 x 250.
(2) Drag "Ticker Region" prefab (under /Twitch Chat PlugIn/Prefabs) to canvas as a child
(3) Drag "Ticker Prefab" prefab (under /Twitch Chat PlugIn/Prefabs) to "Ticker Prefab" field of Ticker Region under canvas
(4) Make sure "Twitch Chat Bot" and "Twitch Vote" prefabs are in the scene and names not changed

4. Setup DOTween
(1) Goto http://dotween.demigiant.com/getstarted.php and follow the instruction
(2) The zip file is included in the package

For more information, load the "Integration" scene from /Twitch Chat PlugIn/Scenes, and play with it. 