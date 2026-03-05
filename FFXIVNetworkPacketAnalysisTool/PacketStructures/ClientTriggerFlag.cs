namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 客户端触发器标志枚举，用于 ClientTrigger 包中的标志字段。
/// </summary>
public enum ClientTriggerFlag
{
    // 拔出/收回武器
    // param1: 1 - 拔出, 0 - 收回
    // param2: 未知, 固定为 1
    DrawOrSheatheWeapon = 1,

    // 自动攻击
    // param1: 是否开启自动攻击 (0 - 否, 1 - 是)
    // param2: 目标对象ID
    // param3: 是否为手动操作 (0 - 否, 1 - 是)
    AutoAttack = 2,

    // 选中目标
    // param1: 目标 Entity ID (无目标为: 0xE0000000)
    Target = 3,
    
    // PVP 快捷发言
    // param1: QuickChat Row ID
    // param2: 参数 1
    // param3: 参数 2
    PVPQuickChat = 5,

    // 下坐骑
    // param1: 0 - 不进入队列; 1 - 进入队列
    Dismount = 101,

    // 召唤宠物
    // param1: 宠物 ID
    SummonPet = 102,

    // 收回宠物
    WithdrawPet = 103,
    
    // 取消身上指定的状态效果
    // param1: Status ID
    // param3: Status 来源的 GameObjectID，亦可用 0xE0000000 指定清除任意来源的首个该状态 
    StatusOff = 104,

    // 中断咏唱
    CancelCast = 105,

    // 共同骑乘
    // param1: 目标 ID
    // param2: 位置索引
    RidePillion = 106,

    // 收起时尚配饰
    WithdrawParasol109 = 109,

    // 收起时尚配饰
    WithdrawParasol110 = 110,

    // 复活
    // param1: 操作 (5 - 接受复活; 8 - 确认返回返回点)
    Revive = 200,

    // 区域变更
    // param1: 变更方式
    // param2: 区域内位置变更方式
    TerritoryTransport = 201,

    // 传送至指定的以太之光
    // param1: 以太之光 ID
    // param2: 是否使用传送券 (0 - 否, 1 - 是)
    // param3: 以太之光 Sub ID
    Teleport = 202,

    // 接受传送邀请
    AcceptTeleportOffer = 203,

    // 请求好友房屋传送信息
    // param1: 未知
    // param2: 未知
    RequestFriendHouseTeleport = 210,

    // 传送至好友房屋
    // param1: 未知
    // param2: 未知
    // param3: 以太之光 ID (个人房屋 - 61, 部队房屋 - 57)
    // param4: 以太之光 Sub ID (疑似恒定为 1)
    TeleportToFriendHouse = 211,

    // 若当前种族不是拉拉菲尔族, 则返回至当前地图的最近安全点
    ReturnIfNotLalafell = 213,

    // 立即返回至返回点, 若在副本内则返回至副本内重生点
    InstantReturn = 214,

    // 检查指定玩家
    // param1: 待对象 Object ID
    Inspect = 300,

    // 更改佩戴的称号
    // param1: 称号 ID
    ChangeTitle = 302,

    // 请求过场剧情数据
    // param1: 过场剧情在 Cutscene.csv 中的对应索引
    RequestCutscene307 = 307,

    // 请求挑战笔记具体类别下数据
    // param1: 类别索引 (从 1 开始)
    RequestContentsNoteCategory = 310,

    // 清除场地标点
    ClearFieldMarkers = 313,

    // 将青魔法师技能交换或应用于有效技能
    // param1: 类型 (0 为应用有效技能, 1 为交换有效技能)
    // param2: 格子序号 (从 0 开始, 小于 24)
    // param3: 技能 ID / 格子序号 (从 0 开始, 小于 24)
    AssignBLUActionToSlot = 315,

    // 请求跨界传送数据
    RequestWorldTravel = 316,

    // 放置场地标点
    // param1: 标点索引
    // param2: 坐标 X * 1000
    // param3: 坐标 Y * 1000
    // param4: 坐标 Z * 1000
    PlaceFieldMarker = 317,

    // 移除场地标点
    // param1: 标点索引
    RemoveFieldMarker = 318,

    // 清除来自木人的仇恨
    // param1: 木人的 Object ID
    ResetStrikingDummy = 319,
    
    // 设置当前雇员市场出售物品价格
    // param1: 物品 Slot
    // param2: 物品价格
    SetRetainerMarketPrice = 400,

    // 请求指定物品栏数据
    // param1: (int)InventoryType
    RequestInventory = 405,

    // 进入镶嵌魔晶石状态
    // param1: 物品 ID
    EnterMateriaAttachState = 408,

    // 退出镶嵌魔晶石状态
    LeaveMateriaAttachState = 410,

    // 取消魔晶石镶嵌委托
    CancelMateriaMeldRequest = 419,

    // 请求收藏柜的数据
    RequestCabinet = 424,

    // 存入物品至收藏柜
    // param1: 物品在 Cabinet.csv 中的对应索引
    StoreToCabinet = 425,

    // 从收藏柜中取回物品
    // param1: 物品在 Cabinet.csv 中的对应索引
    RestoreFromCabinet = 426,

    // 维修装备
    // param1: Inventory Type
    // param2: Inventory Slot
    // param3: Item ID
    RepairItem = 434,

    // 批量维修装备中装备
    // param1: Inventory Type (固定为 1000)
    RepairEquippedItems = 435,
    
    // 批量维修装备
    // param1: 分类 (0 - 主手/副手; 1 - 头部/身体/手臂; 2 - 腿部/脚部; 3 - 耳部;颈部; 4 - 腕部;戒指; 5 - 物品)
    RepairAllItems = 436,

    // 精制魔晶石
    // param1: Inventory Type
    // param1: Inventory Slot
    ExtractMateria = 437,

    // 更换套装
    GearsetChange = 441,

    // 请求陆行鸟鞍囊的数据
    RequestSaddleBag = 444,
    
    // 请求支援物资退还箱物资数据
    RequestReconstrcutionBuyBack445 = 445,
    
    // 请求支援物资退还箱物资数据
    RequestReconstrcutionBuyBack446 = 446,
    
    // 发送修理委托
    /// /// <remarks>
    /// <para><c>param1</c>: 目标 Entity ID</para>
    /// </remarks>
    SendRepairRequest = 450,
    
    // 取消修理委托
    /// /// <remarks>
    /// <para><c>param1</c>: 目标 Entity ID</para>
    /// </remarks>
    CancelRepairRequest = 453,

    // 打断当前正在进行的情感动作
    InterruptEmote = 502,
    
    // 打断当前正在进行的特殊情感动作
    InterruptEmoteSpecial = 503,
    
    // 更改闲置状态姿势
    // param2: 姿势索引
    IdlePostureChange = 505,

    // 进入闲置状态姿势
    // param2: 姿势索引
    IdlePostureEnter = 506,

    // 退出闲置状态姿势
    IdlePostureExit = 507,

    // 进入游泳状态 (也会强制下坐骑)
    EnterSwim = 608,

    // 退出游泳状态
    LeaveSwim = 609,

    // 赋予/取消禁止骑乘坐骑状态
    // param1: 0 - 取消; 1 - 赋予
    DisableMounting = 612,

    // 进入飞行状态
    EnterFlight = 616,

    // 生产
    // param1: 类型 (0 - 普通制作, 1 - 简易制作; 2 - 制作练习)
    // param2: 配方 ID (在 Recipe.csv 中)
    // param3: 额外参数 (简易制作 - 数量, 最多 255 个)
    Craft = 700,

    // 钓鱼
    // param2: 额外参数 (若为换饵, 则为物品 ID; 若为游动饵, 则为饵索引)
    Fish = 701,

    // 加载制作笔记数据
    // param1: 职业索引 (从左至右, 从 0 开始, 至 7 结束)
    LoadCraftLog = 710,

    // 结束制作
    ExitCraft = 711,

    // 放弃任务
    // param1: 任务 ID (非 RowID)
    AbandonQuest = 800,

    // 刷新理符任务状态
    RefreshLeveQuest = 801,
    
    // 放弃理符任务
    // param1: 理符任务 ID
    AbandonLeveQuest = 802,

    /// <summary>
    /// 开始理符任务
    /// <remarks>
    /// <para><c>param1</c>: 理符任务 ID</para>
    /// <para><c>param2</c>: 要提高的等级数</para>
    /// </remarks>
    /// </summary>
    StartLeveQuest = 804,
    
    // 副本相关
    Content = 808,

    // 开始指定的临危受命任务
    // param1: FATE ID
    // param2: 目标 Object ID
    FateStart = 809,

    /// <summary>
    /// 加载临危受命信息
    /// (在切换地图时会一次性加载完地图内所有 FATE 信息)
    /// </summary>
    /// <remarks>
    /// <para><c>param1</c>: FATE ID</para>
    /// </remarks>
    FateLoad = 810,

    // 进入 临危受命 范围 (若 FATE 在脚底下生成则不会发送该命令)
    // param1: FATE ID
    FateEnter = 812,

    // 为 临危受命 等级同步
    // param1: FATE ID
    // param2: 是否等级同步 (0 - 否, 1 - 是)
    FateLevelSync = 813,

    // 临危受命 野怪生成
    // param1: Object ID
    FateMobSpawn = 814,
    
    // 区域变更完成
    TerritoryTransportFinish = 816,

    // 离开副本
    // param1: 类型 (0 - 正常退本, 1 - 一段时间未操作)
    LeaveDuty = 819,
    
    // 发送单人任务战斗请求
    // param1: 难度 (0 - 通常, 1 - 简单, 2 - 非常简单)
    StartSoloQuestBattle = 823,

    // 昔日重现模式
    // param1: QuestRedo.csv 中对应的昔日重现章节序号 (0 - 退出昔日重现)
    QuestRedo = 824,

    // 刷新物品栏
    InventoryRefresh = 830,

    // 请求过场剧情数据
    // param1: 过场剧情在 Cutscene.csv 中的对应索引
    RequestCutscene831 = 831,

    // 请求成就进度数据
    // param1: 成就在 Achievement.csv 中的对应索引
    RequestAchievement = 1000,

    // 请求所有成就概览 (不含具体成就内容)
    RequestAllAchievement = 1001,

    // 请求接近达成成就概览 (不含具体成就内容)
    // param1: 未知, 固定为 1
    RequestNearCompletionAchievement = 1002,

    // 请求抽选数据
    // param1: Territory Type
    // param2: 地皮对应索引
    RequestLotteryData = 1105,

    // 请求门牌数据
    // param1: Territory Type
    // param2: 地皮对应索引
    RequestPlacardData = 1106,

    // 请求住宅区数据
    // param1: Territory Type
    // param2: 分区索引
    RequestHousingAreaData = 1107,

    // 向房屋仓库存入指定的物品
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    // param3: InventoryType
    // param4: InventorySlot
    StoreFurniture = 1112,

    // 从房屋中取回指定的家具
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    // param3: InventoryType (25000 至 25010 / 27000 至 27008)
    // param4: InventorySlot (若 >65535 则将 slot 为 (i - 65536) 的家具收入仓库)
    RestoreFurniture = 1113,

    // 请求房屋名称设置数据
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    RequestHousingName = 1114,
    
    // 请求房屋问候语设置数据
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    RequestHousingGreeting = 1115,
    
    // 请求房屋访客权限设置数据
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    RequestHousingGuestAccess = 1117,
    
    // 保存房屋访客权限设置
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    // param3: 设置枚举值组合 (已知: 1 - 传送权限; 65536 - 进入权限)
    SaveHousingGuestAccess = 1118,
    
    // 请求房屋宣传设置数据
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    RequestHousingEstateTag = 1119,
    
    // 保存房屋宣传设置
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    // param3: 设置枚举值组合 (注: 即使是相同名称的 Tag 在不同位置上对应的枚举值也不同)
    SaveHousingEstateTag = 1120,
    
    // 移动到庭院门前
    // param1: 地块索引
    MoveToHouseFrontGate = 1122,

    // 进入到"布置家具/庭具"状态
    // param2: 房屋地块索引 (公寓为 0)
    FurnishState = 1123,
    
    // 查看房屋详情
    // param1: Territory Type
    // param2: 地皮对应索引
    // param3: (若有)公寓房间索引
    ViewHouseDetail = 1126,

    // 调整房间亮度
    // param1: 亮度等级 (最亮为 0, 最暗为 5)
    AdjustHouseLight = 1137,

    // 刷新部队合建物品交纳信息
    RefreshFCMaterialDelivery = 1143,

    // 刷新潜水艇完成情况信息
    RefreshSubmarineInfo = 1144,
    
    // 设置房屋背景音乐
    // param1: 管弦乐曲在 Orchestrion.csv 中的对应索引
    SetHouseBackgroundMusic = 1145,

    // 从房屋仓库中取出布置指定物品
    // param1: HouseManager 相关区域的 HouseID 地址的高 32 位
    // param2: HouseManager 相关区域的 HouseID
    // param3: InventoryType (25000 至 25010 / 27000 至 27008)
    // param4: InventorySlot
    Furnish = 1150,

    // 修理潜水艇部件
    // param1: 潜水艇索引
    // param2: 潜水艇部件索引
    RepairSubmarinePart = 1153,
    
    // 请求房屋内部改建信息
    // param1: 房屋索引 (从 0 开始, 59 结束)
    HouseInteriorDesignRequest = 1169,
    
    // 更改房屋内部装修风格
    // param1: 房屋索引 (从 0 开始, 59 结束)
    // param1: 内部装修风格 (3 - 海雾村风格; 6 - 薰衣草苗圃风格; 9 - 高脚孤丘风格; 12 - 白银乡风格; 15 - 穹顶皓天风格; 18 - 简装风格)
    HouseInteriorDesignChange = 1170,

    // 领取战利水晶
    // param1: 赛季 (0 - 本赛季; 1 - 上赛季)
    CollectTrophyCrystal = 1200,
    
    // 选择 PVP 职能技能
    // param1: 职能技能索引
    SelectPVPRoleAction = 1201,

    // 请求挑战笔记数据
    RequestContentsNote = 1301,
    
    // 请求雇员探险时间信息
    RequestRetainerVentureTime = 1400,

    // 在 NPC 处维修装备
    // param1: Inventory Type
    // param2: Inventory Slot
    // param3: Item ID
    RepairItemNPC = 1600,
    
    // 在 NPC 处批量维修装备
    // param1: 分类 (0 - 主手/副手; 1 - 头部/身体/手臂; 2 - 腿部/脚部; 3 - 耳部;颈部; 4 - 腕部;戒指; 5 - 物品)
    RepairAllItemsNPC = 1601,

    // 在 NPC 处批量维修装备中装备
    // param1: Inventory Type (固定为 1000)
    RepairEquippedItemsNPC = 1602,

    // 切换陆行鸟作战风格
    // param1: BuddyAction.csv 中的对应索引
    BuddyAction = 1700,
    
    // 陆行鸟装甲
    // param1: 部位 (0 - 头部, 1 - 身体, 2 - 腿部)
    // param2: 在 BuddyEquip.csv 中对应的装备索引 (0 - 卸下装备)
    BuddyEquip = 1701,
    
    // 陆行鸟学习技能
    // param1: Skill 索引
    BuddyLearnSkill = 1702,

    // 请求金碟游乐场面板 整体 信息
    RequestGSGeneral = 1850,

    // 请求金碟游乐场面板 陆行鸟 信息
    RequestGSChocobo = 1900,
    
    // 开始任务回顾
    StartDutyRecord = 1980,
    
    // 结束任务回顾
    EndDutyRecord = 1981,

    // 请求金碟游乐场面板 萌宠之王 信息
    RequestGSLordofVerminion = 2010,

    // 启用/解除自动加入新人频道设置
    EnableAutoJoinNoviceNetwork = 2102,

    // 发起决斗
    // param1: 被决斗对象的 GameObject ID
    SendDuel = 2200,

    // 确认决斗申请
    // param1: 0 - 确认; 1 - 取消; 4 - 强制取消
    RequestDuel = 2201,

    // 同意决斗
    ConfirmDuel = 2202,

    // 确认天书奇谈副本结果
    // param1: 索引 (从左到右从上到下, 从 0 开始)
    WondrousTailsConfirm = 2253,

    // 天书奇谈其他操作
    // param1: 操作 (0 - 再想想)
    // param2: 索引 (从左到右从上到下, 从 0 开始)
    WondrousTailsOperate = 2253,

    // 请求投影台数据
    RequestPrismBox = 2350,

    // 取出投影台物品
    // param1: 投影台内部物品 ID (MirageManager.Instance().PrismBoxItemIds)
    RestorePrsimBoxItem = 2352,

    // 请求投影模板数据
    RequestGlamourPlates = 2355,

    // 进入/退出投影模板选择状态
    // param1: 0 - 退出, 1 - 进入
    // param2: 未知, 可能为 0 或 1
    EnterGlamourPlateState = 2356,

    // 应用投影模板 (需要先进入投影模板选择状态)
    // param1: 投影模板索引
    ApplyGlamourPlate = 2357,
    
    // 获取时尚品鉴每周参与奖励
    FashionCheckEntryReward = 2450,
    
    // 获取时尚品鉴每周额外奖励
    FashionCheckBonusReward = 2451,
    
    // 买回支援物资
    // param1: 物品索引
    BuybackReconstrcutionItem = 2501,

    // 请求金碟游乐场面板 多玛方城战 信息
    RequestGSMahjong = 2550,

    // 请求青魔法书数据
    RequstAOZNotebook = 2601,

    // 请求亲信战友数据
    RequestTrustedFriend = 2651,

    // 请求剧情辅助器数据
    RequestDutySupport = 2653,
    
    // 发送剧情辅助器申请请求
    // param1: DawnStroy 序号
    // param2: 前四位 DawnStroyMemberUIParam 序号的幂次方 (a1 * 256^0 + a2 * 256^1 + a3 * 256^2 + a4 * 256^3)
    // param2: 后三位 DawnStroyMemberUIParam 序号的幂次方 (a1 * 256^0 + a2 * 256^1 + a3 * 256^2)
    SendDutySupport = 2654,

    // 分解指定的物品 / 回收指定物品的魔晶石 / 精选指定物品
    // param1: 分解: 3735552; 回收魔晶石: 3735553; 精选: 3735554; 修理: 3735555
    // param2: Inventory Type
    // param3: Inventory Slot
    // param4: 物品 ID
    Desynthesize = 2800,
    
    // 博兹雅分配失传技能库到技能槽
    // param1: 失传技能库索引
    // param2: 要分配的槽位
    BozjaUseFromHolster = 2950,
    
    // 请求肖像列表数据
    RequestPortraits = 3200,
    
    // 切换无人岛模式
    // param1: 模式 (0 - 自由; 1 - 收获; 2 - 播种; 3 - 浇水; 4 - 铲除; 6 - 喂食; 7 - 宠爱; 8 - 招呼; 9 - 捕兽)
    MJISetMode = 3250,
    
    // 设置无人岛模式参数, 切换时会被设置为 0, 如播种、喂食、捕兽时会为对应的物品 ID
    // param1: 参数
    MJISetModeParam = 3251,
    
    // 无人岛设置面板开关
    // param1: 状态 (1 - 开启; 0 - 关闭)
    MJISettingPanelToggle = 3252,

    // 请求无人岛工房排班数据
    // param1: 具体天数 (0 为本周期第一天, 7 为下周期第一天)
    MJIWorkshopRequest = 3254,
    
    // 请求无人岛工房排班物品数据
    MJIWorkshopRequestItem = 3258,

    // 无人岛工房排班
    // param1: 物品和排班时间段: (8 * (startingHour | (32 * craftObjectId)))
    // param2: 具体天数 (0 - 本周期第一天, 7 - 下周期第一天)
    // param4: 添加/删除 (0 - 添加, 1 - 删除)
    MJIWorkshopAssign = 3259,

    // 取消无人岛工坊排班
    // param1: 物品和排班时间段: (8 * (startingHour | (32 * craftObjectId)))
    // param2: 具体天数 (0 - 本周期第一天, 7 - 下周期第一天)
    MJIWorkshopCancel = 3260,
    
    // 设置无人岛休息周期
    // param1: 休息日 1
    // param2: 休息日 2
    // param3: 休息日 3
    // param4: 休息日 4
    MJISetRestCycles = 3261,

    // 收取无人岛屯货仓库探索结果
    // param1: 仓库索引
    MJIGranaryCollect = 3262,

    // 查看无人岛屯货仓库探索目的地
    // param1: 仓库索引
    MJIGranaryViewDestinations = 3263,

    // 无人岛屯货仓库派遣探险
    // param1: 仓库索引
    // param2: 目的地索引
    // param3: 探索天数
    MJIGranaryAssign = 3264,
    
    // 在无人岛放养宠物
    // param1: 宠物 ID
    // param2: 放生区域索引
    MJIReleaseMinion = 3265,
    
    // 放生无人岛牧场动物
    // param1: 动物索引
    MJIReleaseAnimal = 3268,
    
    // 收集无人岛牧场动物产物
    // param1: 动物索引
    // param2: 收集标志
    MJICollectAnimalLeavings = 3269,
    
    // 收取无人岛牧场全部动物产物
    // param1: 预期收集的产物数量 (MJIManager.Instance()->PastureHandler->AvailableMammetLeavings)
    MJICollectAllAnimalLeavings = 3271,
    
    // 托管无人岛牧场动物
    // param1: 动物索引
    // param2: 喂食物品 ID
    MJIEntrustAnimal = 3272,
    
    // 召回无人岛放生的宠物
    // param1: 宠物索引
    MJIRecallMinion = 3277,

    // 托管单块无人岛耕地
    // param1: 耕地索引
    // param2: 种子物品 ID
    MJIFarmEntrustSingle = 3279,

    // 取消托管单块无人岛耕地
    // param1: 耕地索引
    MJIFarmDismiss = 3280,

    // 收取单块无人岛耕地
    // param1: 耕地索引
    // param2: 收取后是否取消托管 (0 - 否, 1 - 是)
    MJIFarmCollectSingle = 3281,

    // 收取全部无人岛耕地
    // param1: *(int*)MJIManager.Instance()->GranariesState
    MJIFarmCollectAll = 3282,

    // 请求无人岛工房需求数据
    MJIFavorStateRequest = 3292,
    
    // 变更宇宙探索模式
    // param1: 模式索引
    WKSChangeMode = 3400,
    
    // 宇宙探索结束交互1
    WKSEndInteraction1 = 3401,
    
    // 宇宙探索结束交互2
    WKSEndInteraction2 = 3402,
    
    // 宇宙探索接取任务
    // param1: Mission Unit ID
    WKSStartMission = 3440,
    
    // 宇宙探索完成任务
    WKSCompleteMission = 3441,
    
    // 宇宙探索放弃任务
    WKSAbandonMission = 3442,
    
    // 宇宙好运道开始抽奖
    // param1: 类型: 0 - 月球信用点; 1 - 法恩娜信用点
    WKSStartLottery = 3450,
    
    // 宇宙好运道选择转盘
    // param1: 类型: 0 - 月球信用点; 1 - 法恩娜信用点
    // param2: 转盘类型 (左边 - 0, 右边 - 1)
    WKSChooseLottery = 3451,
    
    // 宇宙好运道结束抽奖
    // param1: 类型: 0 - 月球信用点; 1 - 法恩娜信用点
    WKSEndLottery = 3452,
    
    // 宇宙探索请求探索成果数据
    WKSRequestSuccesses = 3460,
    
    // 宇宙探索请求机甲数据
    // param1: WKSMechaEventData Row ID (0 - 当前未开始)
    WKSRequestMecha = 3478,

    // 掷骰子
    // param1: 类型 (固定为 0)
    // param2: 最大值
    RollDice = 9000,

    // 雇员
    Retainer = 9003,
    
    // 设置角色显示范围
    /// /// <remarks>
    /// <para><c>param1</c>: 类型 (0 - 标准; 1 - 较大; 2 - 最大)</para>
    /// </remarks>
    AroundRangeSetMode = 9005,
}
