namespace ChickenAPI.Game.Permissions.Ranks
{
    public class UserRank : RankBase
    {
        public override string Name => "User";

        public UserRank()
        {
            #region Inventory

            AddPermission(
                PermissionType.INVENTORY_ADD_ITEM,
                PermissionType.INVENTORY_DELETE_ITEM,
                PermissionType.INVENTORY_MERGE_ITEM,
                PermissionType.INVENTORY_MOVE_ITEM,
                PermissionType.INVENTORY_SWAP_ITEM);

            #endregion

            #region Friend / Blocked List

            AddPermission(
                PermissionType.RELATION_ADD_FRIEND,
                PermissionType.RELATION_REMOVE_FRIEND,
                PermissionType.RELATION_TELEPORT_FRIEND,
                PermissionType.RELATION_CHAT_FRIEND,
                PermissionType.RELATION_ADD_BLOCKED,
                PermissionType.RELATION_REMOVE_BLOCKED
            );

            #endregion

            #region Movement

            AddPermission(
                PermissionType.MOVEMENT_MOVE_SELF,
                PermissionType.MOVEMENT_MOVE_PETS);

            #endregion

            #region Quicklist

            AddPermission(
                PermissionType.QUICKLIST_ADD_ELEMENT,
                PermissionType.QUICKLIST_MOVE_ELEMENT,
                PermissionType.QUICKLIST_REMOVE_ELEMENT);

            #endregion
        }
    }
}