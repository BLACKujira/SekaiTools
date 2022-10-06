// Sekai.UserResource

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class UserResource
    {
        public const string RESOURCE_TYPE_COIN = "coin";
        public const string RESOURCE_TYPE_VIRTUAL_COIN = "virtual_coin";
        public const string RESOURCE_TYPE_MATERIAL = "material";
        public const string RESOURCE_TYPE_AREAITEM = "area_item";
        public const string RESOURCE_TYPE_MUSIC = "music";
        public const string RESOURCE_TYPE_MUSIC_DIFFICULTY = "music_difficulty";
        public const string RESOURCE_TYPE_MUSIC_VOCAL = "music_vocalitem";
        public const string RESOURCE_TYPE_MUSIC_VOCAL_SUPPLY = "music_vocal";
        public const string RESOURCE_TYPE_JEWEL = "jewel";
        public const string RESOURCE_TYPE_PAID_JEWEL = "paid_jewel";
        public const string RESOURCE_TYPE_CARD = "card";
        public const string RESOURCE_TYPE_COSTUME = "costume_3d";
        public const string RESOURCE_TYPE_PRACTICE_TICKET = "practice_ticket";
        public const string RESOURCE_TYPE_SKILL_PRACTICE_TICKET = "skill_practice_ticket";
        public const string RESOURCE_TYPE_MASTER_LESSON = "master_lesson";
        public const string RESOURCE_TYPE_STAMP = "stamp";
        public const string RESOURCE_TYPE_BOOST_ITEM = "boost_item";
        public const string RESOURCE_TYPE_GACHA_CEIL_ITEM = "gacha_ceil_item";
        public const string RESOURCE_TYPE_GACHA_TICKET = "gacha_ticket";
        public const string RESOURCE_TYPE_HONOR = "honor";
        public const string RESOURCE_TYPE_AVATAR_SKIN_COLOR = "avatar_skin_color";
        public const string RESOURCE_TYPE_AVATAR_COSTUME = "avatar_costume";
        public const string RESOURCE_TYPE_AVATAR_ACCESSORY = "avatar_accessory";
        public const string RESOURCE_TYPE_AVATAR_MOTION = "avatar_motion";
        public const string RESOURCE_TYPE_COLORFUL_PASS = "colorful_pass";
        public const string RESOURCE_TYPE_AVATAR_COORDINATE = "avatar_coordinate";
        public const string RESOURCE_TYPE_PENLIGHT = "penlight";
        public const string RESOURCE_TYPE_LIVE_POINT = "live_point";
        public const string RESOURCE_TYPE_EVENT_ITEM = "event_item";
        public const string RESOURCE_TYPE_BOOST_PRESENT = "boost_present";
        public const string RESOURCE_TYPE_STREAMING_TICKET = "virtual_live_ticket";
        public const string RESOURCE_TYPE_BONDS_HONOR = "bonds_honor";
        public const string RESOURCE_TYPE_BONDS_HONOR_WORD = "bonds_honor_word";
        public const string RESOURCE_TYPE_COLLECTION = "custom_profile_collection_resource";
        public const string RESOURCE_TYPE_CUT_IN_VOICE = "cut_in_voice";
        public const string RESOURCE_TYPE_TOTAL_POWER = "total_power";
        public const string RESOURCE_TYPE_CHARACTER_RANK_EXP = "character_rank_exp";
        public const string RESOURCE_TYPE_CHALLENGE_SLOT = "challenge_slot";
        public int resourceId;
        public string resourceType;
        public int resourceLevel;
        public int quantity;

        public UserResource(int id, string type, int level, int amount)
        {
        }

        public UserResource(ResourceBoxDetail boxDetail)
        {
        }
    }
}