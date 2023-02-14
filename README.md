# U.stick-battle-arena
### Game Description
Stick Battle Arena is a multiplayer hunger games type game where the last person standing wins. Players will fight as stick people and battle each other to victory. The game will include several themed stages, and even more unique weapons such as bows, guns, spears, swords, grenades, machine guns, rocket launchers, flame throwers and more. Learning good movement and how to line up shots will be a core element of the gameplay.
### Version Control
Version control is done through `JimmyC7834/U.stick-battle-arena` repo on github.
Adding to the project is done in a standardized [pull request workflow](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests) and follow these rules:
- Create a branch for the content you wish to add
  - be sure your branch is descriptive for your changes along with your name. For example, `Jimmy-dev-Weapon-logic`, `Todd-patch-documentation`, etc.
- Commit often and with relevant messages/descriptions to make bug tracking easier
- Write tests for all new content you are adding
- Since the GameCI takes a while to run, before submitting a pull request for your branch make sure all tests pass locally (refer below for info on running tests)
- Do not approve pull requests that have not/ do not pass all tests in the GameCI (check actions tab)
- Do not approve pull requests that you have not fully reviewed and built locally
- Lastly, make sure all code follows the [C# style guide for Unity ](https://github.com/JimmyC7834/U.stick-battle-arena/blob/main/C%23%20style%20guide.md)
### Bug Tracking And Reporting
Bugs are reported to `JimmyC7834/U.stick-battle-arena` repo under the issues tab.
If a bug is found in a branch you are attempting to PR, the PR will not be approved until a suitable fix is implemented.
### Testing And GameCI
The project utilizes gameCI, which integrates with github under github actions. Everytime a PR is submitted all build tests and Test Runner tests are ran and the result will appear on the pull request page. Additionally, all submitted PR's will have their test results listed in the actions tab of the repo.
### Running in Unity editor for development
In order to run the system within the editor follow these steps:
- Download and install Unity
- Clone `JimmyC7834/U.stick-battle-arena` via ssh into desired project location
- Under the projects tab select 'Open'
- Select `U.stick-battle-arena` folder
- Ensure editor version is `2021.3.16f1`
- Once project is opened select the 'Play' button at the top of the screen
### Running the tests
The tests are handled by Unity TestRunner and can be launched through github actions or in Unity itself by following steps:
- From Github:
  - Github actions runs with every PR of the project and you can view the tests results in the actions tab of github
  - To rerun tests on a commit you can go to repo `JimmyC7834/U.stick-battle-arena` > actions > GameCI and click any PR then select 'Re-Run all jobs'
  - The test will then take 10-20 minutes to run and will make sure the game scuccessfully builds for Windows and the Unity web plugin. It will also run all unit tests     controlled by the Unity TestRunner
- From Unity:
  - Note: The Unit tests that can be ran from Unity are the same that are ran automatically in github actions upon a PR
  - Download and install Unity and open the project repo folder as a project using editor version `2021.3.16f1`
  - Once in the editor, at the top click Window > General > Test Runner 
  - A small window will appear with a list of tests
  - To run a particular test you can double click it in the list
  - To run all tests select 'Run All' in the top left of the Test Runner window
### Building the project
  - make sure you open th project in Unity version `2021.3.16f1`
  - Checkout and pull the `main` branch 
  - In Unity editor, click File > Build and Run to build and run the project
  - Alternatively, click File > Build Settings open up a window allowing simply building the project
### Our goal
Our goal is to ship a finished game with refined details and fun gameplay by the end of the quarter. Also, to gain group project and teamwork experience along the development process.
### Repo layout
- `/Asset`
  - The resources for the whole project including art assets, C# scripts, game prefabs, external packages, etc.
  - `/Arts`
    - Contains all the audio, textures, any resources applied to the game.
  - `/Scripts`
    - Contains all the c# scripts.
  - `/Prefabs`
    - Contains all prefabs of unity gameObjects
  - `/SOs`
    - Contains all the ScriptableObjects created
  - `/Scenes`
    - Contains all the different scenes of the game (Main menu, different stages, etc)
  - `/Settings`
    - Contains config files of external packages
    - Probably no need to modify
  - `/Packages`
    - Contains folders of external packages
- `/Package`
  - The unity default and extra packages that are used for our game.
  - DO NOT MODIFY MANUALLY
- `/ProjectSetting`
  - The preference and editor setting of the Unity engine (physics engine, audio engine, rendering engine, etc)
  - Should be changed inside the Unity Editor.
  - DO NOT MODIFY MANUALLY
- `/UserSettings`
  - The preference of the Unity editor (layout of UIs, tools settings, etc)
  - DO NOT MODIFY MANUALLY
