namespace ChickenAPI.Game.Permissions.Ranks
{
    public class UserPermissibleRank : PermissibleRankBase
    {
        public override string Name => "User";

        public UserPermissibleRank()
        {
            #region Inventory

            GrantPermission(
                PermissionType.INVENTORY_ADD_ITEM,
                PermissionType.INVENTORY_DELETE_ITEM,
                PermissionType.INVENTORY_MERGE_ITEM,
                PermissionType.INVENTORY_MOVE_ITEM,
                PermissionType.INVENTORY_SWAP_ITEM);

            #endregion

            #region Friend / Blocked List

            GrantPermission(
                PermissionType.RELATION_ADD_FRIEND,
                PermissionType.RELATION_REMOVE_FRIEND,
                PermissionType.RELATION_TELEPORT_FRIEND,
                PermissionType.RELATION_CHAT_FRIEND,
                PermissionType.RELATION_ADD_BLOCKED,
                PermissionType.RELATION_REMOVE_BLOCKED
            );

            #endregion

            #region Movement

            GrantPermission(
                PermissionType.MOVEMENT_MOVE_SELF,
                PermissionType.MOVEMENT_MOVE_PETS);

            #endregion

            #region Quicklist

            GrantPermission(
                PermissionType.QUICKLIST_ADD_ELEMENT,
                PermissionType.QUICKLIST_MOVE_ELEMENT,
                PermissionType.QUICKLIST_REMOVE_ELEMENT);

            #endregion
        }
    }
}