# RythmicVR

The following list contains my plans for this Project:
- [ ] Modularity / support for multiple gamemodes (addable afterwards -> via assetbundles?)
	- [X] Beat Saber inspired gamemode (Hands, feet, hips / Swords)
	- [ ] DDR inspired gamemode (arrow pad on floor)
	- [ ] Audica inspired gamemode (with Guns)
	- [ ] kind of Guitar hero on the floor (for feet)

### Mechanics
- [X] Song loading via json file (json specs in code -> Song.cs & Beatmap.cs)
- [X] dynamic (expandable) systems for anything you could imagine (gamemodes, songs, mods for gamemodes)
- [ ] on the fly loaded assets
    - [ ] tracked object models (swords, guns, gloves, shoes)
    - [ ] avatars / player models
    - [ ] environments
    - [ ] targets
    - [ ] obstacles
- [ ] loading of songs / beatmaps from other games
    - [X] beat saber
    - [ ] audica
    - [ ] osu!
    - [ ] guitar hero (and clones)
    - [ ] etc...

### ToDo
- [X] start songs from UI
    - [X] play the beatmap
    - [X] play the song
    - [X] disable menu
    - [X] enable pause button listener
- [X] list songs in UI
- [ ] filter songs in songlist
    - [ ] by gamemode
    - [ ] by string (search)
    - [ ] by song & beatmap properties
        - [ ] length
        - [ ] difficulty
        - [ ] etc...
- [X] main menu with button to
    - [X] songlist
    - [X] multiplayer (possibly, future)
    - [X] settings
    - [X] quit
- [X] settings menu with
    - [ ] asset selection
    - [ ] mod management
    - [ ] audio settings
    - [ ] UI style settings
    - [ ] playspace settings
    - [ ] controller offset (per gamemode?) settings

### Screenshots
![Early Screenshot](https://i.imgur.com/KWZKX2P.png)
