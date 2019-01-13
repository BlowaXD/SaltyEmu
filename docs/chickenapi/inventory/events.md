# Inventory - Events

## Description
All events that are related to events

## InventoryLoadEvent
### Usage
You are supposed to use it only when a player tries to load its inventory from `ICharacterItemService`
### Expected behavior
Once the even is emitted, the handlers are supposed to load from `ICharacterItemService` and to fill the inventory with character's items


## InventoryAddItemEvent
### Usage
You are supposed to use it only when you want to add an item to an Inventory
### Arguments
* ItemInstance : `ItemInstanceDto`

### Expected behavior
Once the even is emitted, the handlers are supposed to add the given ItemInstance into the inventory and to actualize player's UI.

