// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: proto/command/CommandMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Proto3 {

  /// <summary>Holder for reflection information generated from proto/command/CommandMessage.proto</summary>
  public static partial class CommandMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for proto/command/CommandMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommandMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiJwcm90by9jb21tYW5kL0NvbW1hbmRNZXNzYWdlLnByb3RvIjYKC0NvbW1h",
            "bmRJdGVtEgsKA2tleRgBIAEoBRINCgV2YWx1ZRgCIAEoBRILCgNjbWQYAyAB",
            "KAkiSwoOQ29tbWFuZFJlcXVlc3QSDwoHYWNjb3VudBgBIAEoAxIMCgR0eXBl",
            "GAIgASgFEhoKBGNtZHMYAyADKAsyDC5Db21tYW5kSXRlbUIlChpjb20udG9w",
            "Z2FtZS5tZXNzYWdlLmNvbW1vbqoCBlByb3RvM2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.CommandItem), global::Proto3.CommandItem.Parser, new[]{ "Key", "Value", "Cmd" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Proto3.CommandRequest), global::Proto3.CommandRequest.Parser, new[]{ "Account", "Type", "Cmds" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CommandItem : pb::IMessage<CommandItem> {
    private static readonly pb::MessageParser<CommandItem> _parser = new pb::MessageParser<CommandItem>(() => new CommandItem());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CommandItem> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.CommandMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandItem() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandItem(CommandItem other) : this() {
      key_ = other.key_;
      value_ = other.value_;
      cmd_ = other.cmd_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandItem Clone() {
      return new CommandItem(this);
    }

    /// <summary>Field number for the "key" field.</summary>
    public const int KeyFieldNumber = 1;
    private int key_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Key {
      get { return key_; }
      set {
        key_ = value;
      }
    }

    /// <summary>Field number for the "value" field.</summary>
    public const int ValueFieldNumber = 2;
    private int value_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    /// <summary>Field number for the "cmd" field.</summary>
    public const int CmdFieldNumber = 3;
    private string cmd_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Cmd {
      get { return cmd_; }
      set {
        cmd_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CommandItem);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CommandItem other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Key != other.Key) return false;
      if (Value != other.Value) return false;
      if (Cmd != other.Cmd) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key != 0) hash ^= Key.GetHashCode();
      if (Value != 0) hash ^= Value.GetHashCode();
      if (Cmd.Length != 0) hash ^= Cmd.GetHashCode();
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
      if (Key != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Key);
      }
      if (Value != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Value);
      }
      if (Cmd.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Cmd);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Key);
      }
      if (Value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Value);
      }
      if (Cmd.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Cmd);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CommandItem other) {
      if (other == null) {
        return;
      }
      if (other.Key != 0) {
        Key = other.Key;
      }
      if (other.Value != 0) {
        Value = other.Value;
      }
      if (other.Cmd.Length != 0) {
        Cmd = other.Cmd;
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
            Key = input.ReadInt32();
            break;
          }
          case 16: {
            Value = input.ReadInt32();
            break;
          }
          case 26: {
            Cmd = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CommandRequest : pb::IMessage<CommandRequest> {
    private static readonly pb::MessageParser<CommandRequest> _parser = new pb::MessageParser<CommandRequest>(() => new CommandRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CommandRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Proto3.CommandMessageReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandRequest(CommandRequest other) : this() {
      account_ = other.account_;
      type_ = other.type_;
      cmds_ = other.cmds_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CommandRequest Clone() {
      return new CommandRequest(this);
    }

    /// <summary>Field number for the "account" field.</summary>
    public const int AccountFieldNumber = 1;
    private long account_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Account {
      get { return account_; }
      set {
        account_ = value;
      }
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 2;
    private int type_;
    /// <summary>
    ///0-道具, 1-英雄 2-xxx 根据需要商议定义
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    /// <summary>Field number for the "cmds" field.</summary>
    public const int CmdsFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Proto3.CommandItem> _repeated_cmds_codec
        = pb::FieldCodec.ForMessage(26, global::Proto3.CommandItem.Parser);
    private readonly pbc::RepeatedField<global::Proto3.CommandItem> cmds_ = new pbc::RepeatedField<global::Proto3.CommandItem>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Proto3.CommandItem> Cmds {
      get { return cmds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CommandRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CommandRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Account != other.Account) return false;
      if (Type != other.Type) return false;
      if(!cmds_.Equals(other.cmds_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Account != 0L) hash ^= Account.GetHashCode();
      if (Type != 0) hash ^= Type.GetHashCode();
      hash ^= cmds_.GetHashCode();
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
      if (Account != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(Account);
      }
      if (Type != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Type);
      }
      cmds_.WriteTo(output, _repeated_cmds_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Account != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Account);
      }
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Type);
      }
      size += cmds_.CalculateSize(_repeated_cmds_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CommandRequest other) {
      if (other == null) {
        return;
      }
      if (other.Account != 0L) {
        Account = other.Account;
      }
      if (other.Type != 0) {
        Type = other.Type;
      }
      cmds_.Add(other.cmds_);
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
            Account = input.ReadInt64();
            break;
          }
          case 16: {
            Type = input.ReadInt32();
            break;
          }
          case 26: {
            cmds_.AddEntriesFrom(input, _repeated_cmds_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
