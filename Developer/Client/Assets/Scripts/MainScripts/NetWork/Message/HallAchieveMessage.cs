// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/achievement/HallAchieveMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto3 {

  /// <summary>Holder for reflection information generated from proto/achievement/HallAchieveMessage.proto</summary>
  public static partial class HallAchieveMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/achievement/HallAchieveMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HallAchieveMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cipwcm90by9hY2hpZXZlbWVudC9IYWxsQWNoaWV2ZU1lc3NhZ2UucHJvdG8a",
            "IHByb3RvL2NvbW1vbi9Db21tb25NZXNzYWdlLnByb3RvIkYKC0FjaGlldmVJ",
            "bmZvEgoKAmlkGAEgASgFEgwKBHN0ZXAYAiABKAMSHQoFc3RhdGUYAyABKA4y",
            "Di5FQWNoaWV2ZVN0YXRlIoIBChNBY2hpZXZlSW5mb1Jlc3BvbnNlEg4KBnVz",
            "ZXJJRBgBIAEoAxIeCghhY2hpZXZlcxgCIAMoCzIMLkFjaGlldmVJbmZvEhsK",
            "BXRhc2tzGAMgAygLMgwuQWNoaWV2ZUluZm8SHgoIbWFpblRhc2sYBCABKAsy",
            "DC5BY2hpZXZlSW5mbyIyChRBY2hpZXZlUmV3YXJkUmVxdWVzdBIOCgZ1c2Vy",
            "SUQYASABKAMSCgoCaWQYAiABKAUiTwoVQWNoaWV2ZVJld2FyZFJlc3BvbnNl",
            "Eg4KBnVzZXJJRBgBIAEoAxIKCgJpZBgCIAEoBRIaCgdyZXdhcmRzGAMgAygL",
            "MgkuS2V5VmFsdWUigwEKFEFjaGlldmVEaXJ0eVJlc3BvbnNlEg4KBnVzZXJJ",
            "RBgBIAEoAxIeCghhY2hpZXZlcxgCIAMoCzIMLkFjaGlldmVJbmZvEhsKBXRh",
            "c2tzGAMgAygLMgwuQWNoaWV2ZUluZm8SHgoIbWFpblRhc2sYBCABKAsyDC5B",
            "Y2hpZXZlSW5mbyo5Cg1FQWNoaWV2ZVN0YXRlEg4KClVuQ29tcGxldGUQABIN",
            "CglDb21wbGV0ZWQQARIJCgVHb3RlZBACKvUDCgxFQWNoaWV2ZVR5cGUSFgoS",
            "Q3VtdWxhdGl2ZURpc3RhbmNlEAASGQoVQ3VtdWxhdGl2ZUtpbGxNb25zdGVy",
            "EAESFgoSQ3VtdWxhdGl2ZVBpY2tJdGVtEAISFwoTQ3VtdWxhdGl2ZUh1cnRD",
            "b3VudBADEhIKDlNpbmdsZURpc3RhbmNlEAQSFQoRU2luZ2xlS2lsbE1vbnN0",
            "ZXIQBRISCg5TaW5nbGVQaWNrSXRlbRAGEhMKD1NpbmdsZUh1cnRDb3VudBAH",
            "Eg4KCkxvZ2luQ291bnQQZRIMCghQbGF5ZXJMdhBmEg4KCk9ubGluZVRpbWUQ",
            "ZxIVChFIYW5nVXBSZXdhcmRDb3VudBBoEhYKElVwZ3JhZGVUYWxlbnRDb3Vu",
            "dBBpEhAKDFdhdGNoQURDb3VudBBqEhAKDE9wZW5QZXRDb3VudBBrEhAKDEdl",
            "dEdvbGRDb3VudBBsEiEKHUN1bXVsYXRpdmVLaWxsQW55TW9uc3RlckNvdW50",
            "EG0SDAoIRHJhd0NhcmQQbhITCg9DaHBhdGVyUHJvZ3Jlc3MQeBITCg9Ub3Rh",
            "bEVxdWlwTGV2ZWwQeRIMCghQZXRMZXZlbBB6EgsKB1BldFN0YXIQexINCghQ",
            "VkVDb3VudBDJARIVChBJbmZpbml0ZVJ1bkNvdW50EMoBQiMKGGNvbS50b3Bn",
            "YW1lLm1lc3NhZ2UuaGFsbKoCBlByb3RvM2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Proto3.CommonMessageReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Proto3.EAchieveState), typeof(global::Proto3.EAchieveType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.AchieveInfo), global::Proto3.AchieveInfo.Parser, new[]{ "Id", "Step", "State" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.AchieveInfoResponse), global::Proto3.AchieveInfoResponse.Parser, new[]{ "UserID", "Achieves", "Tasks", "MainTask" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.AchieveRewardRequest), global::Proto3.AchieveRewardRequest.Parser, new[]{ "UserID", "Id" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.AchieveRewardResponse), global::Proto3.AchieveRewardResponse.Parser, new[]{ "UserID", "Id", "Rewards" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.AchieveDirtyResponse), global::Proto3.AchieveDirtyResponse.Parser, new[]{ "UserID", "Achieves", "Tasks", "MainTask" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum EAchieveState {
    /// <summary>
    ///未完成
    /// </summary>
    [pbr::OriginalName("UnComplete")] UnComplete = 0,
    /// <summary>
    ///完成
    /// </summary>
    [pbr::OriginalName("Completed")] Completed = 1,
    /// <summary>
    ///已领取
    /// </summary>
    [pbr::OriginalName("Goted")] Goted = 2,
  }

  public enum EAchieveType {
    /// <summary>
    ///累计跑酷距离
    /// </summary>
    [pbr::OriginalName("CumulativeDistance")] CumulativeDistance = 0,
    /// <summary>
    ///累计击杀某类型怪物数量（参数1 = monsterType，参数2 = 数量）
    /// </summary>
    [pbr::OriginalName("CumulativeKillMonster")] CumulativeKillMonster = 1,
    /// <summary>
    ///累计拾取障碍物数量（参数1 = 障碍物id，参数2 = 数量）
    /// </summary>
    [pbr::OriginalName("CumulativePickItem")] CumulativePickItem = 2,
    /// <summary>
    /// 累计碰撞受伤次数
    /// </summary>
    [pbr::OriginalName("CumulativeHurtCount")] CumulativeHurtCount = 3,
    /// <summary>
    ///单局跑酷距离
    /// </summary>
    [pbr::OriginalName("SingleDistance")] SingleDistance = 4,
    /// <summary>
    ///单局击杀某类型怪物数量
    /// </summary>
    [pbr::OriginalName("SingleKillMonster")] SingleKillMonster = 5,
    /// <summary>
    ///单局拾取障碍物数量
    /// </summary>
    [pbr::OriginalName("SinglePickItem")] SinglePickItem = 6,
    /// <summary>
    ///单局碰撞受伤次数
    /// </summary>
    [pbr::OriginalName("SingleHurtCount")] SingleHurtCount = 7,
    /// <summary>
    ///登录游戏次数
    /// </summary>
    [pbr::OriginalName("LoginCount")] LoginCount = 101,
    /// <summary>
    /// 玩家等级达到X
    /// </summary>
    [pbr::OriginalName("PlayerLv")] PlayerLv = 102,
    /// <summary>
    ///在线分钟数
    /// </summary>
    [pbr::OriginalName("OnlineTime")] OnlineTime = 103,
    /// <summary>
    ///领取巡逻收益次数
    /// </summary>
    [pbr::OriginalName("HangUpRewardCount")] HangUpRewardCount = 104,
    /// <summary>
    ///升级天赋次数
    /// </summary>
    [pbr::OriginalName("UpgradeTalentCount")] UpgradeTalentCount = 105,
    /// <summary>
    ///观看视频次数
    /// </summary>
    [pbr::OriginalName("WatchADCount")] WatchAdcount = 106,
    /// <summary>
    ///孵化宠物次数
    /// </summary>
    [pbr::OriginalName("OpenPetCount")] OpenPetCount = 107,
    /// <summary>
    ///累计获得金币次数
    /// </summary>
    [pbr::OriginalName("GetGoldCount")] GetGoldCount = 108,
    /// <summary>
    ///累计击杀任意类型怪物数量
    /// </summary>
    [pbr::OriginalName("CumulativeKillAnyMonsterCount")] CumulativeKillAnyMonsterCount = 109,
    /// <summary>
    ///玩家抽卡次数
    /// </summary>
    [pbr::OriginalName("DrawCard")] DrawCard = 110,
    /// <summary>
    ///通关主线进度
    /// </summary>
    [pbr::OriginalName("ChpaterProgress")] ChpaterProgress = 120,
    /// <summary>
    /// 升级装备次数
    /// </summary>
    [pbr::OriginalName("TotalEquipLevel")] TotalEquipLevel = 121,
    /// <summary>
    ///宠物等级累计达到
    /// </summary>
    [pbr::OriginalName("PetLevel")] PetLevel = 122,
    /// <summary>
    ///宠物星级累计达到
    /// </summary>
    [pbr::OriginalName("PetStar")] PetStar = 123,
    /// <summary>
    ///参与主线关卡次数
    /// </summary>
    [pbr::OriginalName("PVECount")] Pvecount = 201,
    /// <summary>
    ///参与无限模式次数
    /// </summary>
    [pbr::OriginalName("InfiniteRunCount")] InfiniteRunCount = 202,
  }

  #endregion

  #region Messages
  public sealed partial class AchieveInfo : pb::IMessage<AchieveInfo> {
    private static readonly pb::MessageParser<AchieveInfo> _parser = new pb::MessageParser<AchieveInfo>(() => new AchieveInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AchieveInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.HallAchieveMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfo(AchieveInfo other) : this() {
      id_ = other.id_;
      step_ = other.step_;
      state_ = other.state_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfo Clone() {
      return new AchieveInfo(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    /// <summary>
    /// ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "step" field.</summary>
    public const int StepFieldNumber = 2;
    private long step_;
    /// <summary>
    /// 进度 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Step {
      get { return step_; }
      set {
        step_ = value;
      }
    }

    /// <summary>Field number for the "state" field.</summary>
    public const int StateFieldNumber = 3;
    private global::Proto3.EAchieveState state_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Proto3.EAchieveState State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AchieveInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AchieveInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Step != other.Step) return false;
      if (State != other.State) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Step != 0L) hash ^= Step.GetHashCode();
      if (State != 0) hash ^= State.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (Step != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(Step);
      }
      if (State != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) State);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (Step != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Step);
      }
      if (State != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) State);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AchieveInfo other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Step != 0L) {
        Step = other.Step;
      }
      if (other.State != 0) {
        State = other.State;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 16: {
            Step = input.ReadInt64();
            break;
          }
          case 24: {
            state_ = (global::Proto3.EAchieveState) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  public sealed partial class AchieveInfoResponse : pb::IMessage<AchieveInfoResponse> {
    private static readonly pb::MessageParser<AchieveInfoResponse> _parser = new pb::MessageParser<AchieveInfoResponse>(() => new AchieveInfoResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AchieveInfoResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.HallAchieveMessageReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfoResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfoResponse(AchieveInfoResponse other) : this() {
      userID_ = other.userID_;
      achieves_ = other.achieves_.Clone();
      tasks_ = other.tasks_.Clone();
      mainTask_ = other.mainTask_ != null ? other.mainTask_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveInfoResponse Clone() {
      return new AchieveInfoResponse(this);
    }

    /// <summary>Field number for the "userID" field.</summary>
    public const int UserIDFieldNumber = 1;
    private long userID_;
    /// <summary>
    ///用户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserID {
      get { return userID_; }
      set {
        userID_ = value;
      }
    }

    /// <summary>Field number for the "achieves" field.</summary>
    public const int AchievesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Proto3.AchieveInfo> _repeated_achieves_codec
        = pb::FieldCodec.ForMessage(18, global::Proto3.AchieveInfo.Parser);
    private readonly pbc::RepeatedField<global::Proto3.AchieveInfo> achieves_ = new pbc::RepeatedField<global::Proto3.AchieveInfo>();
    /// <summary>
    ///成就
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.AchieveInfo> Achieves {
      get { return achieves_; }
    }

    /// <summary>Field number for the "tasks" field.</summary>
    public const int TasksFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Proto3.AchieveInfo> _repeated_tasks_codec
        = pb::FieldCodec.ForMessage(26, global::Proto3.AchieveInfo.Parser);
    private readonly pbc::RepeatedField<global::Proto3.AchieveInfo> tasks_ = new pbc::RepeatedField<global::Proto3.AchieveInfo>();
    /// <summary>
    ///任务
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.AchieveInfo> Tasks {
      get { return tasks_; }
    }

    /// <summary>Field number for the "mainTask" field.</summary>
    public const int MainTaskFieldNumber = 4;
    private global::Proto3.AchieveInfo mainTask_;
    /// <summary>
    ///主线任务
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Proto3.AchieveInfo MainTask {
      get { return mainTask_; }
      set {
        mainTask_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AchieveInfoResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AchieveInfoResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserID != other.UserID) return false;
      if(!achieves_.Equals(other.achieves_)) return false;
      if(!tasks_.Equals(other.tasks_)) return false;
      if (!object.Equals(MainTask, other.MainTask)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserID != 0L) hash ^= UserID.GetHashCode();
      hash ^= achieves_.GetHashCode();
      hash ^= tasks_.GetHashCode();
      if (mainTask_ != null) hash ^= MainTask.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserID);
      }
      achieves_.WriteTo(output, _repeated_achieves_codec);
      tasks_.WriteTo(output, _repeated_tasks_codec);
      if (mainTask_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(MainTask);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserID);
      }
      size += achieves_.CalculateSize(_repeated_achieves_codec);
      size += tasks_.CalculateSize(_repeated_tasks_codec);
      if (mainTask_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(MainTask);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AchieveInfoResponse other) {
      if (other == null) {
        return;
      }
      if (other.UserID != 0L) {
        UserID = other.UserID;
      }
      achieves_.Add(other.achieves_);
      tasks_.Add(other.tasks_);
      if (other.mainTask_ != null) {
        if (mainTask_ == null) {
          mainTask_ = new global::Proto3.AchieveInfo();
        }
        MainTask.MergeFrom(other.MainTask);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            UserID = input.ReadInt64();
            break;
          }
          case 18: {
            achieves_.AddEntriesFrom(input, _repeated_achieves_codec);
            break;
          }
          case 26: {
            tasks_.AddEntriesFrom(input, _repeated_tasks_codec);
            break;
          }
          case 34: {
            if (mainTask_ == null) {
              mainTask_ = new global::Proto3.AchieveInfo();
            }
            input.ReadMessage(mainTask_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// 成就领奖
  /// </summary>
  public sealed partial class AchieveRewardRequest : pb::IMessage<AchieveRewardRequest> {
    private static readonly pb::MessageParser<AchieveRewardRequest> _parser = new pb::MessageParser<AchieveRewardRequest>(() => new AchieveRewardRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AchieveRewardRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.HallAchieveMessageReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardRequest(AchieveRewardRequest other) : this() {
      userID_ = other.userID_;
      id_ = other.id_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardRequest Clone() {
      return new AchieveRewardRequest(this);
    }

    /// <summary>Field number for the "userID" field.</summary>
    public const int UserIDFieldNumber = 1;
    private long userID_;
    /// <summary>
    ///用户id 
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserID {
      get { return userID_; }
      set {
        userID_ = value;
      }
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 2;
    private int id_;
    /// <summary>
    /// ID	
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AchieveRewardRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AchieveRewardRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserID != other.UserID) return false;
      if (Id != other.Id) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserID != 0L) hash ^= UserID.GetHashCode();
      if (Id != 0) hash ^= Id.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserID);
      }
      if (Id != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Id);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserID);
      }
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AchieveRewardRequest other) {
      if (other == null) {
        return;
      }
      if (other.UserID != 0L) {
        UserID = other.UserID;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            UserID = input.ReadInt64();
            break;
          }
          case 16: {
            Id = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class AchieveRewardResponse : pb::IMessage<AchieveRewardResponse> {
    private static readonly pb::MessageParser<AchieveRewardResponse> _parser = new pb::MessageParser<AchieveRewardResponse>(() => new AchieveRewardResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AchieveRewardResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.HallAchieveMessageReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardResponse(AchieveRewardResponse other) : this() {
      userID_ = other.userID_;
      id_ = other.id_;
      rewards_ = other.rewards_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveRewardResponse Clone() {
      return new AchieveRewardResponse(this);
    }

    /// <summary>Field number for the "userID" field.</summary>
    public const int UserIDFieldNumber = 1;
    private long userID_;
    /// <summary>
    ///用户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserID {
      get { return userID_; }
      set {
        userID_ = value;
      }
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 2;
    private int id_;
    /// <summary>
    /// ID	
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "rewards" field.</summary>
    public const int RewardsFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Proto3.KeyValue> _repeated_rewards_codec
        = pb::FieldCodec.ForMessage(26, global::Proto3.KeyValue.Parser);
    private readonly pbc::RepeatedField<global::Proto3.KeyValue> rewards_ = new pbc::RepeatedField<global::Proto3.KeyValue>();
    /// <summary>
    ///key-rewardList id, value-具体数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.KeyValue> Rewards {
      get { return rewards_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AchieveRewardResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AchieveRewardResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserID != other.UserID) return false;
      if (Id != other.Id) return false;
      if(!rewards_.Equals(other.rewards_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserID != 0L) hash ^= UserID.GetHashCode();
      if (Id != 0) hash ^= Id.GetHashCode();
      hash ^= rewards_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserID);
      }
      if (Id != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Id);
      }
      rewards_.WriteTo(output, _repeated_rewards_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserID);
      }
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      size += rewards_.CalculateSize(_repeated_rewards_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AchieveRewardResponse other) {
      if (other == null) {
        return;
      }
      if (other.UserID != 0L) {
        UserID = other.UserID;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      rewards_.Add(other.rewards_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            UserID = input.ReadInt64();
            break;
          }
          case 16: {
            Id = input.ReadInt32();
            break;
          }
          case 26: {
            rewards_.AddEntriesFrom(input, _repeated_rewards_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class AchieveDirtyResponse : pb::IMessage<AchieveDirtyResponse> {
    private static readonly pb::MessageParser<AchieveDirtyResponse> _parser = new pb::MessageParser<AchieveDirtyResponse>(() => new AchieveDirtyResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AchieveDirtyResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.HallAchieveMessageReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveDirtyResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveDirtyResponse(AchieveDirtyResponse other) : this() {
      userID_ = other.userID_;
      achieves_ = other.achieves_.Clone();
      tasks_ = other.tasks_.Clone();
      mainTask_ = other.mainTask_ != null ? other.mainTask_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AchieveDirtyResponse Clone() {
      return new AchieveDirtyResponse(this);
    }

    /// <summary>Field number for the "userID" field.</summary>
    public const int UserIDFieldNumber = 1;
    private long userID_;
    /// <summary>
    ///用户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UserID {
      get { return userID_; }
      set {
        userID_ = value;
      }
    }

    /// <summary>Field number for the "achieves" field.</summary>
    public const int AchievesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Proto3.AchieveInfo> _repeated_achieves_codec
        = pb::FieldCodec.ForMessage(18, global::Proto3.AchieveInfo.Parser);
    private readonly pbc::RepeatedField<global::Proto3.AchieveInfo> achieves_ = new pbc::RepeatedField<global::Proto3.AchieveInfo>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.AchieveInfo> Achieves {
      get { return achieves_; }
    }

    /// <summary>Field number for the "tasks" field.</summary>
    public const int TasksFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Proto3.AchieveInfo> _repeated_tasks_codec
        = pb::FieldCodec.ForMessage(26, global::Proto3.AchieveInfo.Parser);
    private readonly pbc::RepeatedField<global::Proto3.AchieveInfo> tasks_ = new pbc::RepeatedField<global::Proto3.AchieveInfo>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.AchieveInfo> Tasks {
      get { return tasks_; }
    }

    /// <summary>Field number for the "mainTask" field.</summary>
    public const int MainTaskFieldNumber = 4;
    private global::Proto3.AchieveInfo mainTask_;
    /// <summary>
    ///主线任务
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Proto3.AchieveInfo MainTask {
      get { return mainTask_; }
      set {
        mainTask_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AchieveDirtyResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AchieveDirtyResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserID != other.UserID) return false;
      if(!achieves_.Equals(other.achieves_)) return false;
      if(!tasks_.Equals(other.tasks_)) return false;
      if (!object.Equals(MainTask, other.MainTask)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserID != 0L) hash ^= UserID.GetHashCode();
      hash ^= achieves_.GetHashCode();
      hash ^= tasks_.GetHashCode();
      if (mainTask_ != null) hash ^= MainTask.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserID != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(UserID);
      }
      achieves_.WriteTo(output, _repeated_achieves_codec);
      tasks_.WriteTo(output, _repeated_tasks_codec);
      if (mainTask_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(MainTask);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserID != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UserID);
      }
      size += achieves_.CalculateSize(_repeated_achieves_codec);
      size += tasks_.CalculateSize(_repeated_tasks_codec);
      if (mainTask_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(MainTask);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AchieveDirtyResponse other) {
      if (other == null) {
        return;
      }
      if (other.UserID != 0L) {
        UserID = other.UserID;
      }
      achieves_.Add(other.achieves_);
      tasks_.Add(other.tasks_);
      if (other.mainTask_ != null) {
        if (mainTask_ == null) {
          mainTask_ = new global::Proto3.AchieveInfo();
        }
        MainTask.MergeFrom(other.MainTask);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            UserID = input.ReadInt64();
            break;
          }
          case 18: {
            achieves_.AddEntriesFrom(input, _repeated_achieves_codec);
            break;
          }
          case 26: {
            tasks_.AddEntriesFrom(input, _repeated_tasks_codec);
            break;
          }
          case 34: {
            if (mainTask_ == null) {
              mainTask_ = new global::Proto3.AchieveInfo();
            }
            input.ReadMessage(mainTask_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
