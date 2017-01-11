Victor Kersey, Nick Steiner
11/22/2016

TODO's
---------------------------------------------

don't redraw all food everytime -- writeableBitmap

Post-Project Partner Documents

Wanted Additional Features
---------------------------------------------

- Scoreboard overlay using Tab

Done
---------------------------------------------
Snake class
Food class
World class
GUI (Design)
GUI (controller)
Network Controller
Scoreboard
- Reconnect after death (Extra Feature)
- Color wheel (Extra Feature)
- Aesthetics (Extra Feature)
- Spectate Mode (Extra Feature)

Notes for TA
---------------------------------------------
We weren't able to figure out the bitmap to prevent all items from being redrawn every frame.


Log
---------------------------------------------

	Nov. 11th -------------------------------
	Initial set up of Solution.
	Set up Network Controller class to handle all network communication.
	Rough sketch of how to interact with the Network Controller.

	Nov 13th --------------------------------
	Added Food.cs, Snake.cs, and World.cs to Model Project with some TODOs
	Added View Project with some TODOs

	Nov 14th --------------------------------
	Added Form and buttons. The form now works with a server
	Added DrawingPanel class and World Class.
	Added DrawingPanel to Form1

	Nov 15th --------------------------------
	Fixed up to Form1.cs[Design] so that it is not displaying an error.
	DrawingPanel is now working but needs to be worked on a lot.

	Nov 16th --------------------------------
	Moved all of the JSON code to the SnakeClient.
	If you run the server and hit connect, food is drawn at (35, 35).
	Need to work the provided points into the DrawingPanel Still.
	---
	Food is displaying correctly (on a fresh server...)
	nearly have snakes displaying
	---
	Pair Programming to draw snakes

	Nov 17th --------------------------------
	Fixed Networking issue caused by having too much food

	Nov 18th --------------------------------
	Graceful socket shutdown

	Nov 19th --------------------------------
	Scoreboard, random colors for everything, Welcome panel
	---
	Pair Programming -- Figured out and implemented Zoom

	Nov 20th --------------------------------
	Added Color picker for your snake and change color button after death.
	Made it so that the screen size and zoom goes back to default size when snake length equals screen width.

	Nov 22nd --------------------------------
	Added the background grass, along with grey boxes around the "lot"
	Code Scrubbed

	Nov 29th --------------------------------
	Server can now accept multiple clients.

	Nov 30th --------------------------------
	The Server now reads the settings.xml file and stores the values. 
	Still need to figure out how to directly link to the settings.xml in the resources folder.
	
	Dec 5th  --------------------------------
	Shrink mode added

Basic Network traffic from the Server
---------------------------------------------

2
150
150


{"ID":0,"name":"n00b AI","vertices":[{"x":48,"y":132},{"x":45,"y":132},{"x":45,"y":118}]}
{"ID":2,"name":"Victor","vertices":[{"x":85,"y":116},{"x":100,"y":116}]}
{"ID":0,"loc":{"x":98,"y":19}}
{"ID":2,"loc":{"x":107,"y":101}}
{"ID":3,"loc":{"x":81,"y":14}}
{"ID":4,"loc":{"x":139,"y":127}}
{"ID":5,"loc":{"x":3,"y":86}}
{"ID":6,"loc":{"x":57,"y":60}}
{"ID":7,"loc":{"x":11,"y":90}}
{"ID":8,"loc":{"x":45,"y":108}}
{"ID":10,"loc":{"x":43,"y":13}}
{"ID":11,"loc":{"x":43,"y":12}}
{"ID":12,"loc":{"x":43,"y":10}}
{"ID":13,"loc":{"x":43,"y":8}}
{"ID":14,"loc":{"x":43,"y":7}}
{"ID":15,"loc":{"x":43,"y":5}}
{"ID":16,"loc":{"x":43,"y":4}}
{"ID":17,"loc":{"x":43,"y":3}}


(1) //N
(2) //E
(3) //S
(4) //W