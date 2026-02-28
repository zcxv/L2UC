# L2UC

Famous korean game client made with Unity3D.

[![License](https://img.shields.io/github/license/zcxv/l2uc)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/zcxv/l2uc/main.yml)](actions)

---

## :bar_chart: Compatibility

### Protocol
More information on protocol versions can be found [here](PROTOCOL.md).

Important: client does not support connecting to game official servers!

#### Auth
| Protocol      | Compatibility      |
|---------------|--------------------|
| 0x0c621 (Old) | :white_check_mark: |
| ??? (New)     | :x:                |


#### Game
| 737                | 740       | 744                | 746                |
|--------------------|-----------|--------------------|--------------------|
| :white_check_mark: | :warning: | :white_check_mark: | :white_check_mark: |


### OS & Arch
| OS      | x86             | x64                | ARM64           |
|---------|-----------------|--------------------|-----------------|
| Windows | :yellow_circle: | :white_check_mark: | :x:             |
| Linux   | :x:             | :white_check_mark: | :x:             |
| macOS   | :x:             | :yellow_circle:    | :yellow_circle: |

 - :white_check_mark: - compatibility
 - :yellow_circle: - not tested
 - :x: - not compatibility


## :rocket: Quick Start

### Prerequisites
 * Unity 6.0.68f1

### Play Mode
1. Load scenes (`Assets/Resources/Scenes`):
   * Menu
   * l2_lobby
2. Play! :tada:

### :hammer_and_wrench: Build
1. Ensure all required scenes are included in the **Scene List** (File → Build Profiles).
![Scene List Example](.img/scene-list.png)
2. Set the scene order:
   * Menu - 0
   * l2_lobby - 1
   * All other scenes
3. Select the target platform in the **Platforms** list.
4. Click the **Build** button.


## :information_source: About
This project is a fork of [L2-Unity-L2J Acis](https://github.com/gawric/Unity-Client-for-L2J) by gawric, which in turn is a fork of [L2-Unity](https://github.com/shnok/l2-unity) by shnok.

### Differences from L2-Unity
Support for the game's original netcode.

### Differences from L2-Unity-L2J Acis
More active development of the codebase.


## :handshake: Contributing
Feel free to fork the repository and open any pull request.


## :speech_balloon: Community
The project does not have dedicated groups on these platforms, but you can always join the L2-Unity & L2-Unity-L2J Acis community:
 - [![L2-Unity Discord](https://img.shields.io/badge/Discord--blue?logo=discord&style=social)](https://discord.gg/ra3BmraPKp) L2-Unity 
 - [![L2-Unity-L2J Acis Telegram](https://img.shields.io/badge/Telegram--blue?logo=telegram&style=social)](https://t.me/l2unityForL2j) L2-Unity-L2J Acis 

[l2-unity has private server, its not avaliable to discord shield widget: https://discord.com/api/guilds/1246654807360405564/widget.png?style=shield]: #

## :heart: Thanks
- [shnok](https://github.com/shnok) (L2-Unity)
- [gawric](https://github.com/gawric) (L2-Unity and L2-Unity-L2J Acis)