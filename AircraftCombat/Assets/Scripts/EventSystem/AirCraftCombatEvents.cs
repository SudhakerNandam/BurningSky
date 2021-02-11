/// <summary>
/// All events to be used need to be added to this script.
/// </summary>
#region Event Types enum
namespace AirCraftCombat
{
    public enum EventID
    {
        // INPUT EVENTS
        EVENT_ON_PLAYERINPUT_DETECTED,
        EVENT_ON_SPREAD_POWERUP_BUTTON_CLICK,
        EVENT_ON_SHIELD_BUTTON_CLICK,
        EVENT_ON_UPDATE_PLAYER_HEALTH,

        EVENT_ON_ENEMY_KILL,
        EVENT_ON_UPDATE_ENEMY_KILL,

        EVENT_ON_GAME_COMPLETE,

        //------------------------------//
        EVENT_COUNT,//PLEASE NOTE:Please keep this as last value in ENUM, Count of enum
    }
    #endregion

}