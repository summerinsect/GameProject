using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RuleManager : MonoBehaviour {
    public string[] titles = new string[8];
    public string[] details = new string[8];
    public Sprite[] images = new Sprite[8];

    private void Start() {
        titles[0] = "概述";
        details[0] = "本游戏是一款rougelike自走棋游戏。\r\n在游戏中，玩家需要深入丛林，招募不同角色，闯过重重关卡，击败最终敌人。";
        titles[1] = "地图";
        details[1] = "地图中有三种关卡：商店，战斗和事件。关卡之间由路径连接。\r\n地图共有12层。在第8层和第12层，会出现含有强力敌人的战斗关卡。";
        titles[2] = "战斗：准备";
        details[2] = "游戏的战斗在一个7行8列的正六边形网格上进行。\r\n在准备阶段，点击敌方角色可以看到它们的信息。\r\n点击背包或场地中的我方角色，可以选中该角色，并查看其信息。\r\n选中角色后，点击背包中的空格或场地中的空格，可以移动角色。\r\n准备好后，点击屏幕下方的“开始战斗”。";
        titles[3] = "战斗：过程";
        details[3] = "战斗过程是全自动进行的。\r\n游戏的战斗基于伪回合制（行动条模式）展开。所有角色拥有自己的速度属性，该属性决定了其两次行动之间的行动间隔。在屏幕的左上方可以看到当前的行动条。\r\n当一方角色全部死亡后，战斗结束。\r\n暂时无法查看战斗中的buff和debuff情况，还请谅解。";
        titles[4] = "战斗：结算";
        details[4] = "如果敌方角色全部死亡而我方角色还未全部死亡，则战斗胜利。\r\n在战斗的结算界面，可以选择包含金币和属性提升在内的各种奖励。\r\n共有9项奖励排成3行3列，玩家只能在每一行或每一列选择1项奖励。\r\n在奖励界面，也可以看到当前的金币数目和各个角色的属性。注意，如果选择了属性提升的奖励，需要重新查看属性才会显示提升。";
        titles[5] = "商店与角色";
        details[5] = "在商店中，玩家可以消耗金币招募角色。\r\n攻击距离：如果角色距离最近的敌人不超过攻击距离，则角色会对范围内的随机敌人进行攻击，否则角色会移动到相邻的空格。\r\n攻击范围：角色每次攻击的作用范围，是以被攻击的敌人为中心，边长等于攻击范围的正六边形。\r\n速度：速度决定了角色两次行动的间隔，速度值越大，间隔越小。\r\n星级：角色的星级显示在头像下方。重复招募相同的角色时可以为其升级，上限三级。";
        titles[6] = "事件";
        details[6] = "在事件关卡中，玩家需要与大语言模型交互。\r\n大语言模型会随机生成一个事件，显示在第一个文本框内。\r\n玩家需要在第二个文本框中输入自己的行动。\r\n大语言模型会将事件的结果和效果返回到第三个文本框内。\r\n事件的效果包括金币和属性变化等。\r\n\r\n提示：事件的期望当然是正面的。";
        titles[7] = "结算";
        details[7] = "如果玩家战胜了第12层的敌人，则游戏胜利。\r\n如果玩家在某一次战斗中失败，则游戏结束。\r\n在游戏的结算界面，将会展示数据统计和伤害统计信息。\r\n挑战获得更高的分数吧！";
    }

    public TextMeshProUGUI title;
    public TextMeshProUGUI detail;
    public Image image;

    public void SetRule(int index) {
        title.text = titles[index];
        detail.text = details[index];
        image.sprite = images[index];
    }
}
