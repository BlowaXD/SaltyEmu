# SaltyEmu - Features

## Core
- [x] Plugin System
- [x] Injection of Dependencies
- [x] Easily add new implementation
- [x] Permission System

## Database

 - [x] MSSQL support
 - [x] Bulk Insert/Save support
 - [x] Fast cache on static data
 - [x] Lazy data Loading


## Toolkit
- [x] .dat parsing
- [x] packet text file parsing
- [ ] Bulk generation `TODO`
- [ ] GUI (Web Interface or Desktop Client) `TODO`
	- [ ] Shop Customiser `TODO`
	- [ ] Item Customiser `TODO`
	- [ ] Recipe Customiser `TODO`
	- [ ] Monster Customiser `TODO`
	- [ ] Skill Customiser `TODO`


## Events
- [x] Map
	- [x] MapJoinEvent
	- [x] MapLeaveEvent
- [x] Entities
	- [x] ChangeVisibilityEvent `TODO REFACTOR`
	- [x] EntityDeathEvent
		- [x] PlayerDeathEvent
		- [x] MonsterDeathEvent
		- [x] NpcDeathEvent
	- [ ] EntityTransformEvent `TODO`
- [x] Battle
	- [x] HitRequestEvent
	- [x] FillHitRequestEvent 
	- [x] ProcessHitRequestEvent 
- [x] Family
	- [x] FamilyJoinEvent
	- [x] FamilyLeaveEvent
	- [x] FamilyCreationEvent
	- [ ] FamilyManagementEvent `TODO`
- [x] Player
	- [x] PlayerExperienceGainEvent

## World
- [x] Players
	- [ ] Specialists `UNDER DEVELOPMENT`
	- [x] Walking
	- [x] Combat
	- [x] Leveling
	- [x] Shop
	- [x] Inventory
	- [ ] Fairy
	- [ ] Miniland
- [x] Families
	- [ ] Logs `TODO`
	- [ ] Permissions `TODO`
	- [x] Join
	- [x] Leave
	- [x] Creation
- [ ] Relations `TODO`
	- [ ] Blocked List `TODO`
	- [ ] Friend List `TODO`
	- [ ] Wedding `TODO`
- [ ] Groups `TODO`
	- [ ] Exp Malus `TODO`
	- [ ] Exp Bonus `TODO`
- [ ] Scripting `TODO`
- [ ] Quests `TODO`
