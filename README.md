# RhythmicVR

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
    - [X] dynamic structure
    - [ ] asset selection
    - [ ] mod management
    - [X] audio settings
    - [ ] UI style settings
    - [ ] playspace settings
    - [ ] controller offset (per gamemode?) settings
    
 
 
refactor everything because the way things are done right now are completely useless and not modular enough.
Will implement features as is in the current system, but the entire song playing, loading, etc logic will be handled by individual plugins. 
They will be able to create deriviatives of the Song class, wich can be loaded through the song list. 
The song list might need to be a plugin aswell... that is to be determined still.....

And the way settings are setup rn is really janky... 
I need to figure something better out. 
I will probably give the UI elements a path string similar to unity editor menu points and dropdowns work.

### Screenshots
![Early Screenshot](https://i.imgur.com/KWZKX2P.png)
