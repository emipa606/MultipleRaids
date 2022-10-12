# MultipleRaids

![Image](https://i.imgur.com/buuPQel.png)

Update of MemeTurtles mod
https://steamcommunity.com/sharedfiles/filedetails/?id=1206031733

- Migrated from old HugsLib settings, opened the settings for translations

![Image](https://i.imgur.com/pufA0kM.png)

	
![Image](https://i.imgur.com/Z4GOv8H.png)


If you feel that fighting one raid at a time is too easy and want some real challenge then this mod is for you. Fight 2, 3 or as many as you like raids at the same time. Show those raiders who owns the land! Or die trying.


Does not affect the frequency of raids or other events.


The mod patches the class handler for RaidEnemy incident. 

Spawned raids will have varying tactics and spawn locations.
The mod should be compatible with existing games and any storyteller, provided this mod is loaded last.

Mod has several variables to adjust:
1) Extra Raids[0,inf) - defines maximum number of additional raids to spawn. The more raids spawn the fewer points each raid has.
2) Spawn Threshold[0,100] - chance for the current raid to be spawned. If current raid is not spawned then no more additional raids will spawn. For example, 50% threshold means that you can have 1 extra raid at 50% chance, 2 raids at 25%, 3 raids and 12.5%
3) Points offset[0, inf) - if more than one raid spawn, raid points are rescaled with this formula: points = points*(1/totalNum + pointsOffset).
4) Force Desperate[true, false] - removes safety check from faction selection. Allows to have regular raiders and tribesman on extreme dessert/ice maps.
5) Force Raid Type[true, false] - forces every 3d raid to drop pod in center, every 4th to siege. Faction restrictions apply(tribesmen will never drop or siege).
6) Random Factions[true, false] - Allows spawned raids to belong to different factions.

-----
I have added the source code to the download. Feel free to modify and reupload it as long as you reference the original work.


![Image](https://i.imgur.com/PwoNOj4.png)



-  See if the the error persists if you just have this mod and its requirements active.
-  If not, try adding your other mods until it happens again.
-  Post your error-log using https://steamcommunity.com/workshop/filedetails/?id=818773962]HugsLib and command Ctrl+F12
-  For best support, please use the Discord-channel for error-reporting.
-  Do not report errors by making a discussion-thread, I get no notification of that.
-  If you have the solution for a problem, please post it to the GitHub repository.




