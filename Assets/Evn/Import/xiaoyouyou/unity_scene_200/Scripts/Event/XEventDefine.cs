using System;
public enum EEvent
{
	Begin,
	#region 常用事件
	MainPlayer_EnterGame,			// 主角进入游戏
	Notice_SystemTopFinish,
	Notice_SystemMidFinish,
	Notice_OperTip,
	#endregion

    #region Packet Message
	Msg_Server_Return,				// 服务器返回消息
	ResourceFileReady,
	DBFileLoadReady,
    #endregion

    #region Object属性更新
	Attr_Name,
	Attr_ModelId,
	Attr_Hp,
	Attr_Exp,
	Attr_GameMoney,
	Attr_RealMoney,
	Attr_Power,
	Attr_ArmourItemID,
	Attr_WeaponItemID,
	Attr_Dynamic,					// 所有保存在 DynamicAttrMgr 内的属性更新, 任何一个动态属性更新了, 都会发送这个消息	
	Attr_Changed,	
    #endregion
	
	#region updateMessage
	Update_Level,
	TopFuncBtn_Ready,
	BottomFuncBtn_Ready,
	Obj_AddAward,
	UI_AddAward,
	UI_ShowAwardInfo,
	UI_FreshAward,
	UI_ShowChallengeCount,
	UI_UpdateVIP,
	UI_EatFoodCount,
	UI_ShengWangValue,
	UI_TotalShengWangValue,
	UI_DayPeiYangCount,
	UI_UpdateBuyHealthCount,
	UI_UpdateBattleValue,
	#endregion
	
    #region UI公用消息
	UI_ShowMainUI,					// 显示主要UI
	UI_HideAllUI,					// 隐藏主要UI
	UI_Show,						// 显示界面
	UI_Hide,						// 关闭界面
	UI_Toggle,						// 显示/关闭界面
	UI_ReqOriginal,					// 请求原始UI资源
	
	UI_MuliShow,					//多对象UI显示
	UI_MuliHide,					//多对象关闭
	
	UI_OnOriginal,					// 原始UI资源准备好
	UI_OnCreated,                   // 界面创建完毕
	UI_OnShow,						// UI成功显示
	UI_OnHide,						// UI成功隐藏
    #endregion
	
	#region 登陆界面
	Login_CheckAccount,				// 验证账号密码
	#endregion
	
	#region 选择服务器界面
	ServerList_AddServerInfo,		// 增加显示服务器信息
	ServerList_SelectServer,		// 选择服务器
	#endregion
	
	#region 角色创建界面
	CharOper_CreatePlayer,			// 界面事件	- 创建角色
	CharOper_SelectClassSex,		// 界面事件 - 选择职业和性别
	CharOper_RandomName,			// 界面事件 - 随机一个名字
	#endregion
	
	#region 聊天界面
	Chat_OnChatMsg,					// 聊天信息到
	Chat_OrderChat,					// 命令聊天(开始聊天or发送聊天信息)
	Chat_SendChatMsg,				// 发送聊天信息
	Chat_ToggleChat,				// 切换聊天状态
	Chat_Notice,					// 发送 系统提示 到聊天对话框
	Chat_SetChatItemData,			// 设置聊天数据到聊天界面中的聊天输入框
	Chat_SetChatData,               // 设置聊天信息数据
	Chat_SetPrivateChatData,		// 设置私聊信息
	Chat_PrivateUserChange,			// 私聊界面用户更改
	Chat_OpenPrivateUI,				// 根据用户名到服务器获取玩家信息，当获取成功后打开私聊聊天窗口
	Chat_OpenPrivate,				// 根据服务器获取得到的用户信息，通知私聊界面打开对应的数据窗口
	Chat_ShowPlayerInfoReq,			// 发送获取玩家信息请求
	Chat_ShowPlayerInfo,			// 显示玩家信息
	Chat_HideBiaoQingSelUI,			// 显示玩家信息
	#endregion
	
	#region Npc对话框
	NpcDialog_BindNpc,		// 验证与NPC的距离
	NpcDialog_CheckSignal,	// 刷新信号
	NpcDialog_FarDistance,	// 隐藏NPC窗口 
	#endregion
	
	#region 商店对话框
	ShopDialog_BindNpc,		 	 //验证与NPC的距离
	ShopDialog_UpdateBuyBack,	 //update
	ShopDialog_ChangeVisiable,	 //隐藏消息
	ShopDialog_NpcLimiteList,	 //某个NPC有关的 限数物品列表更新
	#endregion
	
	#region 任务列表
	Mission_ActionState,
	Mission_CanReceiveListUpdate,
	Mission_ReferMission,
	Mission_GiveUpMission,
	Mission_ReceiveMission,
	Mission_NpcMissionUpdate,
	#endregion
	
	#region 山海经
	HillSeaBook_Message,
	#endregion
	
	#region 场景动画
	CutScene_LeftSay,
	CutScene_RightSay,
	CutScene_TopSay,
	CutScene_NextTalk,
	CutScene_BackWord,
	CutScene_BackAlpha,
	CutScene_BattleAnimationEnd,
	#endregion
	
	#region 选择副本界面
	SelectScene_Clear,				// 清空副本界面
	SelectScene_AddScene,			// 增加副本信息
	SelectScene_SetName,			// 设置副本名称
	SelectScene_ChooseScene,		// 选择副本
	SelectScene_SendData,			// 发送当前选择的副本场景数据
	SelectScene_SelectSceneType,	// 选择副本的类型
	#endregion

    #region Battle Fight Result
	CopySceneResult_Confirm,		// 副本结算界面确定
	CopySceneResult_Close,			// 副本结算界面关闭
	CopySceneResult_FailLevel,
	CopySceneResult_WinLevel,
	CopySceneResult_Win_SetItem,
	CopySceneResult_Win_Reset,
	
    #endregion

    #region 技能相关
	Skill_AddSkillOperConfig,		// 加载了一条技能学习配置
	Skill_SkillPoint,				// 当前技能点
	Skill_OnLearnSkill,
	Skill_OnUpgradeSkill,
	Skill_OnForgetSkill,
	Skill_OnEquipSkill,
	Skill_ImproveSkill,
	Skill_EquipSkill,
	Skill_ResetSkill,
    #endregion
	
	#region 各种提示
	ToolTip_A,						// tooltip样式A
	ToolTip_B,						// tooltip样式B
	ToolTip_C,						// tooltip样式C，用于显示聊天对话框中的物品信息
	ToolTip_CenterTip,				// 中心提示
	MessageBox,						// MessageBox
	InputMessageBox,
	InputMessageBoxSetMaxValue,
	MessageBoxWithNoCancel,
	ToolTip_ReadTip_Discription,	// ReadTip的描述
	ToolTip_ReadTip_Progress,		// ReadTip的进度
	#endregion

	#region CharInfomation
	CharInfo_FirstOpen,				// 第一次打开角色界面，向服务器请求装备数据
	charInfo_Update,				// 更新角色界面数据	
	charInfo_SetSprite,				// 更新图标
	CharInfo_TurnLeft,				// 模型左转
	CharInfo_TurnRight,				// 模型右转
	CharInfo_UIModel,				// 更新UI模型
	CharInfo_BottomBtn1,
	CharInfo_BottomBtn2,
	CharInfo_BottomBtn3,
	CharInfo_DelPet,
	CharInfo_AddPet,
	CharInfo_ChangeName,
	CharInfo_WuLi,
	CharInfo_LingQiao,
	CharInfo_TiZhi,
	CharInfo_FaShu,
	CharInfo_UpdateLingDan,
	CharInfo_LingDan,
	CharInfo_HL,
	CharInfo_PY,
	CharInfo_Reflash_PeiYang,
	CharInfo_Reflash_JJ,
	CharInfo_Reflash_RoleInfo,
	CharInfo_NickNameSelect,
	CharInfo_RandomAddLD,
	CharInfo_FashionChange,
	CharInfo_LeftPageBtn,
	CharInfo_RightPageBtn,
	CharInfo_OpenHuaLing,
	#endregion

	#region Bag
	Bag_FirstOpen,					// 第一次打开背包界面，向服务器请求装备数据
	Bag_Update,						// 更新背包界面数据
	Bag_SetSprite,					// 更新图标
	Bag_ChangePage,					// 更改背包页面
	Bag_ItemSpace,
	Bag_UpdateItemSpace,			// 更新物品槽
	Bag_ItemUpdate,					// 更新特定物品栏位所有数据
	Bag_ItemNumChanged,				// 物品数量发生变化(物品的原始ID)
	Bag_ItemSeal,
	Bag_Arrange,
	Bag_Bank,
	Bag_Guide,						// 背包物品使用引导
	Bag_UpdateNum,
	Bag_UpdateItemContainer,
	Bag_ShowEffect,					// 对特定类型物品高亮显示
	#endregion

	#region Bank
	Bank_FirstOpen,					// 第一次打开仓库界面，向服务器请求装备数据
	Bank_Update,					// 更新仓库界面数据
	Bank_SetSprite,					// 更新图标
	Bank_ItemUpdate,				// 更新特定物品栏位所有数据
	#endregion
	
	#region 世界地图
	WorldMap_RequireLoadScene,		// 请求进入场景
	#endregion
	
	#region PopMenu
	PopMenu_Data,					// 显示左键菜单
	PopMenu_Equip,					// 装备
	PopMenu_Use,					// 使用
	PopMenu_Drop,					// 丢弃
	PopMenu_Split,					// 拆分
	PopMenu_Decompose,				// 分解
	PopMenu_Compose,				// 合成
	PopMenu_Upload,					// 上传
	PopMenu_NameData,				// 人名菜单
	PopMenu_SellItem,				// 卖物品
	PopMenu_SendMsg,				// 发送消息
	PopMenu_SendEmail,				// 发送邮件
	PopMenu_LookDetail,				// 查看资料
	PopMenu_AddFriend,				// 添加好友
	PopMenu_Auction,				// 添加好友
	
	PopMenu_BuyItem,				//购买
	PopMenu_BuyBackItem,			//回购
	PopMenu_GuildKickMem,			// 帮会成员
	#endregion
	
	#region CurSor
	//Cursor_UpdateSprite,			// 设置图片
	//Cursor_ClearSprite,			// 清空图片
	Cursor_UpdateModel,
	Cursor_ClearModel,	
	Cursor_UpdateIcon,
	Cursor_UpdateIconData,
	Cursor_ClearIcon,
	#endregion
	
	#region Strengthen
	Strengthen_BtnUp,				// 按钮向上
	Strengthen_BtnDown,				// 按钮向下
	Strengthen_ShowStrengthen,		// 显示强化部分UI
	Strengthen_ShowInlay,			// 显示注入部分UI
	Strengthen_ShowInXiLian,		// 显示洗练部分UI
	Strengthen_StartShtrengthen,	// 开始强化
	Strengthen_StartDirectStr,		// 开始直接强化
	Strengthen_DirectStrTip,		// 直接强化TIP
	Strengthen_UpdateData,			// 更新强化数据
	Strengthen_Item_Update,			// 更新背包物品
	Strengthen_UpdateIndex,			// 更新选中物品索引
	Strengthen_ItemClick,			// 条目被点击
	Strengthen_UpdateUI,			// 更新UI界面
	Strengthen_ShowBag,				// 显示背包中物品强化
	Strengthen_ShowEquip,			// 显示装备栏中物品强化
	Strengthen_InlayBtn,			// 镶嵌
	Strengthen_InlayRemoveAll,		// 除去所有宝石
	Strengthen_LeftPageBtn,
	Strengthen_RightPageBtn,
	#endregion
	
	#region 场景加载界面
	LoadScene_Discription,			// 加载场景时的描述文字
	LoadScene_Progress,				// 加载进度
	#endregion
	
	
	
	#region
	PassInfo_RetScene,
	#endregion
	
	#region FightHead
	FightHead_Show_LBlood_Value,
	FightHead_Show_RBlood_Value,
	FightHead_Show_Battle_Value,
	FightHead_Show_Soul_Value,
	FightHead_Show_Soul_Fire,
	FightHead_Show_Soul_Explosion,
	FightHead_Show_Soul_Empty,
	
	FightHead_Show_Sub_Blood,
	FightHead_Show_ReSet_Max_Blood,
	Show_Fight_Result,
	Show_Fight_Result_Direct,
	Show_Fight_Replay,
	Show_Fight_Sub_Scene,	
	Fight_Anim_End,
	Fight_Anim_Start,
	#endregion
	
	#region subScene	
	SubScene_SetResult,
	SubScene_AwardSel,
	#endregion

	#region Buff
	buff_OnAddBuff,			// 增加buff
	buff_OnRemoveBuff,		// 删除buff
	buff_OnSetLayer,		// buff层级改变
	buff_DisplayBuff,		// 展现buff(查看buff_tooltip)
	buff_StopDisplayBuff,	// 停止展现buff
	buff_RemoveBuff,		// 请求删除buff
	#endregion
	
	// 生产系统
	#region Product
	product_LearnCareer,	// 学习(选择)专业
	product_UpgradeCareer,	// 升级专业
	product_ForgetCareer,	// 遗忘专业
	product_Strength,		// 体力值变更
	product_Exp,			// 熟练度变更
	product_AddGatherRec,	// 新增采集记录
	product_AddFormula,		// 新增配方
	#endregion
	
	// 拍卖系统
	#region Auction
	auction_UpdateAuction,
	auction_UpdateMyAuction,
	auction_DragPublish,			// 拖拽物品上架
	auction_UpdateHistroyInfo,		// 更新历史拍卖信息
	auction_UserSelectAuction,
	auction_RealMoney_Change,		// 玩家金钱数量更改
	auction_Update_Price,			// 价格变更
	auction_SetSoldNoDataVisible,	
	auction_SetMyNoDataVisible,
	auction_DeleteOnSoldItem,
	#endregion
	
	#region Formation
	Formation_BeginDrag,
	Formation_BeginDrop,
	Formation_SetFormationPos,
	Formation_BeginFormationDrag,
	Formation_ShowModelTip,
	Formation_DoubleClickModel,
	Formation_LeftView,
	Formation_RightView,
	#endregion
	
	#region UnLock
	FuncUnLock_Data,
	#endregion

    	#region Friend
	Friend_AddFriendBtn,
	Friend_DelFriendBtn,
	Friend_PresentFlowerBtn,
	Friend_GetMarryBtn,
	Friend_DivorceBtn,
	Friend_DelBlackBtn,
	Friend_MoveToBlackListBtn,
	
	Freind_ErrCode,
	Friend_AddFriend,
	Freind_ChatAddFriend,
	Friend_AddFriendTip,
	Friend_DelFriend,
	Friend_MoveToBlackList,
	Friend_RemoveBlack,
	Friend_SetSignature,
	Friend_GetMarryAgree,
	Friend_DivorceRecivedTip,
	Friend_DivorceResualt,
	Friend_UpdateLevel,
	Friend_UpdateSignature,
	Friend_UpdateOnlineStatus,
	Friend_UpdateRankInfo,
    	#endregion
	
	#region Mail
	Mail_Reply,
	Mail_Delete,
	Mail_SelectAll,
	Mail_DeleteSelect,
	Mail_DelectRead,
	Mail_WriteMail,
	Mail_UpdateMailDetail,
	Mail_AllDelete,
	Mail_AllGet,
	Mail_UpdateMailBox,
	Mail_Oper,
	Mail_Tip,
	#endregion
	
	#region Select
	ObjSel_SetData,
	#endregion
	
	#region PKDATA
	PKData_Other,
	PKData_Record,
	PKData_SingleObject,
	PKData_PKPlayer,
	PKData_Tip,
	PKData_Notice,
	PKData_SortRecord,
	PKData_BuyChallenge,
	PKData_AddSpeed,
	PKData_Failed,
	PKData_UI_BuyChallenge,
	PKData_UI_AddSpeed,
	PKData_ClearSortRecord,
	PKData_UI_AddSpeed_Tip,
	PKData_PKDataGetFinish,
	#endregion

    #region  MoneyTree
	MoneyTree_GoShake,
	MoneyTree_Resualt,
	MoneyTree_ShakeTimes,
	MoneyTree_MaxShake,
	MoneyTree_Update,
    #endregion

    #region OnlineReward
	OnlineReward_NewEvent,
	OnlineReward_GetItem,
	OnlineReward_TimeToGet,
	OnlineReward_Close,
    #endregion
	
	#region PVP
	PVPResult_Reset,
	PVPResult_Set,
	PVPResult_Award,	
	#endregion
	
	#region Growth
	GROWTH_STATUS,
	GROWTH_STATUSALL,
	GROWTH_AWARDFAILED,
	GROWTH_GETAWARDITEMS,
	GROWH_SHOWTARGETS,
	#endregion
	
	#region FunctionButton
	FUNCTION_BUTTON_STARTEFFECT,
	FUNCTION_BUTTON_STOPEFFECT,
	#endregion
	
	#region NewPlayerGuide
	PlayerGuide_Start,
	PlayerGuide_StepStart,
	PlayerGuide_Finish,
	PlayerGuide_Hide,
	PlayerGuide_StepFinish,
	PlayerGuide_ReposPanel,
	PlayerNpc_Disapper,
	PlayerMove_DisapperDelta,
	#endregion
	
	#region ShanHE
	Click_SH_Btn,
	ShanHe_Update_BattleInfo,
	ShanHe_Init_Info,
	ShanHe_Rank,
	#endregion
	
	#region TargetInfo
	TargetInfo_AddFriend,
	TargetInfo_ChatPrivate,
	TargetInfo_RequestGroup,
	TargetInfo_LookInfo,
	#endregion
	
	#region Audio
	
	#endregion 
	
	Nav_TargetChange,
	Nav_MoveEnd,
	
	FlyItem_NewItem,
	
	#region fabao
	Mount_SetLevel,
	Mount_SetPeiYangCount,
	Mount_SetExp,
	Mount_SetBaoJiValue,
	Mount_SetNormalValue,
	Mount_ShowJiaChengInfo,
	Mount_ShowZuoQiInfo,
	Mount_ShowZuoQiSelect,
	#endregion
	
	#region ZhanYaoLu
	ZYL_SetMonsterInfo,
	ZYL_BattleEnd,
	ZYL_SetLeftCDTime,
	ZYL_SetComboCnt,
	ZYL_UpdateInfo,
	ZYL_UpdateTime,
	#endregion
	
	#region SevenTarget
	SevenTarget_AllItemStatus_Update,
	SevenTarget_ItemStatus_Update,
	#endregion
	
	#region Meditaion
	Meditation_UpdateExp,
	Meditation_EndEvent,
	Meditation_StartEvent,
	#endregion
	
	#region DailySign
	DailySign_Status,
	DailySign_Result,
	DailySign_Hide,
	#endregion
	
	#region Guild
	GuildList_Init,
	GuildList_UpdateGuildSynInfo,
	GuildList_UpdateSelfApplyState,
	GuildCreate_Init,
	GuildMain_Init,
	GuildMain_UpdateInfo,
	GuildMaintain_UpdateAnno,
	GuildInfo_Init,
	GuildInfo_UpdateMemInfo,
	GuildInfo_UpdateApplyInfo,
	#endregion
	
	#region 
	DayActivity_UpdateItem,
	DayActivity_UpdateAward,
	DayActivity_UpdateActivityValue,
	DayActivity_ResetInfo,
	#endregion

	#region VIP
	Vip_UpdateInfo,
	#endregion
	
	#region Fight
	Fight_LeaveSce,
	#endregion
	
	#region daily play sign
	DailyPlaySign_RefreshUI,
	DailyPlaySign_UpdateText,
	#endregion
	
	SingleTalk_Content,
	
	#region ZhanYaoLu
	SaoDang_Battle_Result,
	#endregion
	
	End,
}
