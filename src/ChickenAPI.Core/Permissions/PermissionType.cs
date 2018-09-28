using Newtonsoft.Json.Serialization;

namespace ChickenAPI.Game.Permissions
{
    public enum PermissionType : ulong
    {
        SESSION_CONNECT,

        INVENTORY_MOVE_ITEM,
        INVENTORY_DELETE_ITEM,
        INVENTORY_ADD_ITEM,
        INVENTORY_SWAP_ITEM,
        INVENTORY_MERGE_ITEM,


        MOVEMENT_MOVE_SELF,
        MOVEMENT_MOVE_PETS,
        MOVEMENT_SIT_SELF,
        MOVEMENT_SIT_PETS,

        #region Quicklist

        QUICKLIST_ADD_ELEMENT,
        QUICKLIST_MOVE_ELEMENT,
        QUICKLIST_REMOVE_ELEMENT,

        #endregion


        #region Relation

        RELATION_ADD_FRIEND,
        RELATION_REMOVE_FRIEND,
        RELATION_TELEPORT_FRIEND,
        RELATION_CHAT_FRIEND,
        RELATION_ADD_BLOCKED,
        RELATION_REMOVE_BLOCKED,

        #endregion

        #region TELEPORT

        NPC_DIALOG_TELEPORT,

        #endregion

        GURI_EMOTICON,
        GURI_IDENTIFY_SHELL
    }
}