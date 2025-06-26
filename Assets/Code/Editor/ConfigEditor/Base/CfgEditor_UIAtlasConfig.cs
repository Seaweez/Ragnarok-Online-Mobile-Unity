using System.Collections.Generic;

public class CfgEditor_UIAtlasConfig
{
    public static Dictionary<string, string[]> map = new Dictionary<string, string[]>()
    {
        // 道具
        {"Item", new string[]{
            "GUI/atlas/preferb/Item_1",
            "GUI/atlas/preferb/Item_2",
            "GUI/atlas/preferb/hairStyle_1"
        }},
	    // 技能，注意！skill必须排在第一位
        {"Skill", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_dz",
            "GUI/atlas/preferb/Skill_fs",
            "GUI/atlas/preferb/Skill_lr",
            "GUI/atlas/preferb/Skill_ms",
            "GUI/atlas/preferb/Skill_zs",
            "GUI/atlas/preferb/Skill_sr"
        }},
        // 初心者职业技能
        {"SkillProfess_0", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff"
        }},
        // 战士职业技能
        {"SkillProfess_1", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_zs",
        }},
        // 法师职业技能
        {"SkillProfess_2", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_fs",
        }},
        // 盗贼职业技能
        {"SkillProfess_3", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_dz",
        }},
        // 猎人职业技能
        {"SkillProfess_4", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_lr",
        }},
        // 牧师职业技能职业技能
        {"SkillProfess_5", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_ms",
        }},
        // 商人职业技能职业技能
        {"SkillProfess_6", new string[]{
            "GUI/atlas/preferb/Skill_1",
            "GUI/atlas/preferb/Skill_buff",
            "GUI/atlas/preferb/Skill_sr",
        }},
        // 动作
        {"Action", new string[]{
            "GUI/atlas/preferb/action",
        }},
        // 小地图图标
        {"Map", new string[]{
            "GUI/atlas/preferb/Map_1",
        }},
        // 职业
        {"career", new string[]{
            "GUI/atlas/preferb/career",
        }},
        // 头像
        {"face", new string[]{
            "GUI/atlas/preferb/face_1",	
            "GUI/atlas/preferb/face_2",
        }},
        // 关键字
        {"keyword", new string[]{
            "GUI/atlas/preferb/keyword",
        }},
        // 头发 发型
        {"hairStyle", new string[]{
            "GUI/atlas/preferb/hairStyle_1",
        }},
        // 公会
        {"guild", new string[]{
            "GUI/atlas/preferb/guild",
        }},
        // ui用 如背包图标等等
        {"uiicon", new string[]{
            "GUI/atlas/preferb/v2_icon",
        }},
        // 充值档位图案
        {"ZenyShopItem", new string[]{
            "GUI/atlas/preferb/ZenyShopItem",
        }},
        // AvatarBody字段使用到的头像贴图
        {"Head", new string[]{
            "Assets/Art/Public/Texture/GUI/New/Atlas/Head/Head",
        }},
		// AvatarBody字段使用到的头像贴图
		{"HeadAccessoryFront", new string[]{
				"GUI/atlas/preferb/Assesories_Front_1",
		}},
		// AvatarBody字段使用到的头像贴图
		{"HeadAccessoryBack", new string[]{
				"GUI/atlas/preferb/Assesories_Back_1",
		}},
    };
}