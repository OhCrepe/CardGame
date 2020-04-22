# Lords
Repository for my final year uni project, an online card game created using the Unity Game Engine called Lords. Gameplay in a style similar to most popular Trading Card Games, both physical (*Magic: The Gathering*, *Yu-Gi-Oh*) and virtual (*Hearthstone*).

## Running the Game
To run the game executable, simply download the Executable/Lords folder from the repository. From here you can run the Lords.exe to run the most recent build of the game (as of 10/12/2019-21:32).

### Requirements for running the game
**System Requirements**

The following system requirements are according to the recommendations on the Unity Website [here](https://unity3d.com/unity/system-requirements)
*  Windows 7 SP1+
*  Graphics card with DX10 (shader model 4.0) capabilities.
*  CPU: SSE2 instruction set support

## Opening the Game in the Unity Editor
To open the game in the Unity Editor, you must have Unity installed. Download this repository, then when you open Unity, select "open project", and select the "ce301\_thorn\_t" folder.

### Requirements for opening the game in the Unity Editor
The Unity version used to create this game is 2018.4.10f1. It is therefore recommended that you use the same version to open the game yourself. You can download this version of the Unity Engine [here](https://unity3d.com/get-unity/download?thank-you=update&download_nid=63055&os=Win).

**System Requirements**

The system requirements for opening the game in the Unity Editor are identical to those for running the game executable.

## Running the Server Locally

By default, Lords will attempt to connect to a server hosted on my home pc. In the future this will be a permanently on PC, however, for now, this is the case. To host a server locally on your machine follow these steps:

1) Download the Server_Executable folder in the Git repository.
2) In this folder, open Config.settings
3) Change the value of test_local_server to true
4) Run the server by double-clicking on Server.exe

Note that you may need to enable Server.exe on your anti-virus/firewall. Also, this has only been tested to work on Windows.


To then connect to this server, open your instance of Lords and follow these steps:

1) Click "Enter Game"
2) Press the 'L' key to toggle between local-server on and off (when it is on some text will appear to indicate as such)
3) Choose a deck and click "Enter Game"

Note that this only supports instances of Lords running on the same machine as the server. To start an actual game, you'll need to start a second instance of Lords and repeat these steps.

### Running the Server Unit Tests

To run the server Unit Tests follow these steps.

1) Import the server project, found in the /Server directory, to Visual Studio.
2) On the toolbar at the top of the page, click Test -> Run -> Run All Tests
3) The "Test Explorer" window should appear on the screen, presenting the results of the tests including which tests passed and which tests failed.

# Credits/References

Card artwork provided by Charlie Adams