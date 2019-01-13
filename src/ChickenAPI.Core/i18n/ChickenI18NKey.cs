namespace ChickenAPI.Core.i18n
{
    public enum ChickenI18NKey
    {
        #region Character Creation

        CHARACTER_NAME_INVALID,
        CHARACTER_NAME_ALREADY_TAKEN,

        #endregion

        #region CHANGEMENTS

        CHARACTER_YOUR_CLASS_CHANGED_TO_X,
        CHARACTER_X_CLASS_CHANGED_TO_Y,
        CHARACTER_YOUR_FACTION_CHANGED_TO_X,
        CHARACTER_X_FACTION_CHANGED_TO_Y,

        #endregion

        #region Family

        FAMILY_PLAYERX_HAS_ALREADY_FAMILY,
        FAMILY_ALREADY_LEADER,
        FAMILY_PLAYERX_JOINED_FAMILYX,

        #endregion

        #region You don't have requirements

        YOU_DONT_HAVE_ENOUGH_GOLD,
        YOU_DONT_HAVE_ENOUGH_REPUTATION,
        YOU_DONT_HAVE_ENOUGH_SPACE_IN_INVENTORY,

        #endregion Gold

        #region RELATIONS

        FRIEND_X_INVITED_YOU_TO_JOIN_HIS_FRIENDLIST,
        FRIEND_X_IS_NOW_IN_YOUR_FRIENDLIST,

        #endregion

        #region PETS

        PETS_YOU_GET_X_AS_A_NEW_PET,
        PETS_YOU_GET_X_AS_A_NEW_PARTNER,

        #endregion

        #region GROUP

        PLAYER_X_INVITED_YOU_TO_JOIN_HIS_GROUP,
        PLAYER_X_INVITED_TO_YOUR_GROUP

        #endregion
    }
}