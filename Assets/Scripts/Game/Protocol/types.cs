//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: types.proto
namespace bs.types
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ErrorInfo")]
  public partial class ErrorInfo : global::ProtoBuf.IExtensible
  {
    public ErrorInfo() {}
    
    private int _code = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int code
    {
      get { return _code; }
      set { _code = value; }
    }
    private string _info = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string info
    {
      get { return _info; }
      set { _info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PropItem")]
  public partial class PropItem : global::ProtoBuf.IExtensible
  {
    public PropItem() {}
    
    private bs.types.PropType _prop_id = bs.types.PropType.Score;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"prop_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.types.PropType.Score)]
    public bs.types.PropType prop_id
    {
      get { return _prop_id; }
      set { _prop_id = value; }
    }
    private long _prop_count = default(long);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"prop_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long prop_count
    {
      get { return _prop_count; }
      set { _prop_count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BaseAppInfo")]
  public partial class BaseAppInfo : global::ProtoBuf.IExtensible
  {
    public BaseAppInfo() {}
    
    private string _name = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private uint _type = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private uint _id = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _version = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint version
    {
      get { return _version; }
      set { _version = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BaseUserInfo")]
  public partial class BaseUserInfo : global::ProtoBuf.IExtensible
  {
    public BaseUserInfo() {}
    
    private string _account = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private ulong _user_id = default(ulong);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"user_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong user_id
    {
      get { return _user_id; }
      set { _user_id = value; }
    }
    private ulong _game_id = default(ulong);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"game_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong game_id
    {
      get { return _game_id; }
      set { _game_id = value; }
    }
    private uint _gender = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"gender", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint gender
    {
      get { return _gender; }
      set { _gender = value; }
    }
    private uint _face_id = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"face_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint face_id
    {
      get { return _face_id; }
      set { _face_id = value; }
    }
    private string _custom_face = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"custom_face", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string custom_face
    {
      get { return _custom_face; }
      set { _custom_face = value; }
    }
    private string _nick_name = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"nick_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string nick_name
    {
      get { return _nick_name; }
      set { _nick_name = value; }
    }
    private bs.types.BaseUserInfo.UserType _user_type = bs.types.BaseUserInfo.UserType.UNKNOW;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"user_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.types.BaseUserInfo.UserType.UNKNOW)]
    public bs.types.BaseUserInfo.UserType user_type
    {
      get { return _user_type; }
      set { _user_type = value; }
    }
    private readonly global::System.Collections.Generic.List<bs.types.PropItem> _user_props = new global::System.Collections.Generic.List<bs.types.PropItem>();
    [global::ProtoBuf.ProtoMember(9, Name=@"user_props", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<bs.types.PropItem> user_props
    {
      get { return _user_props; }
    }
  
    private uint _market_id = default(uint);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"market_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint market_id
    {
      get { return _market_id; }
      set { _market_id = value; }
    }
    private uint _site_id = default(uint);
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"site_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint site_id
    {
      get { return _site_id; }
      set { _site_id = value; }
    }
    private uint _reg_market_id = default(uint);
    [global::ProtoBuf.ProtoMember(12, IsRequired = false, Name=@"reg_market_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint reg_market_id
    {
      get { return _reg_market_id; }
      set { _reg_market_id = value; }
    }
    private uint _reg_site_id = default(uint);
    [global::ProtoBuf.ProtoMember(13, IsRequired = false, Name=@"reg_site_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint reg_site_id
    {
      get { return _reg_site_id; }
      set { _reg_site_id = value; }
    }
    private string _register_data = "";
    [global::ProtoBuf.ProtoMember(14, IsRequired = false, Name=@"register_data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string register_data
    {
      get { return _register_data; }
      set { _register_data = value; }
    }
    private ulong _gate_connid = default(ulong);
    [global::ProtoBuf.ProtoMember(15, IsRequired = false, Name=@"gate_connid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong gate_connid
    {
      get { return _gate_connid; }
      set { _gate_connid = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"UserType")]
    public enum UserType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"UNKNOW", Value=0)]
      UNKNOW = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Normal", Value=1)]
      Normal = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Robot", Value=10)]
      Robot = 10
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserRoomInfo")]
  public partial class UserRoomInfo : global::ProtoBuf.IExtensible
  {
    public UserRoomInfo() {}
    
    private bs.types.BaseUserInfo _base_info = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"base_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public bs.types.BaseUserInfo base_info
    {
      get { return _base_info; }
      set { _base_info = value; }
    }
    private ulong _table_id = default(ulong);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"table_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong table_id
    {
      get { return _table_id; }
      set { _table_id = value; }
    }
    private uint _seat_id = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"seat_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint seat_id
    {
      get { return _seat_id; }
      set { _seat_id = value; }
    }
    private uint _user_state = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"user_state", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint user_state
    {
      get { return _user_state; }
      set { _user_state = value; }
    }
    private uint _lost_count = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"lost_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint lost_count
    {
      get { return _lost_count; }
      set { _lost_count = value; }
    }
    private uint _draw_count = default(uint);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"draw_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint draw_count
    {
      get { return _draw_count; }
      set { _draw_count = value; }
    }
    private uint _win_count = default(uint);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"win_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint win_count
    {
      get { return _win_count; }
      set { _win_count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RoomInfo")]
  public partial class RoomInfo : global::ProtoBuf.IExtensible
  {
    public RoomInfo() {}
    
    private bs.types.BaseAppInfo _app_info = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"app_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public bs.types.BaseAppInfo app_info
    {
      get { return _app_info; }
      set { _app_info = value; }
    }
    private uint _kind = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"kind", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint kind
    {
      get { return _kind; }
      set { _kind = value; }
    }
    private bs.types.RoomInfo.RoomType _type = bs.types.RoomInfo.RoomType.Gold;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.types.RoomInfo.RoomType.Gold)]
    public bs.types.RoomInfo.RoomType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private uint _level = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private string _name = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private long _base_score = default(long);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"base_score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long base_score
    {
      get { return _base_score; }
      set { _base_score = value; }
    }
    private long _join_min = default(long);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"join_min", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long join_min
    {
      get { return _join_min; }
      set { _join_min = value; }
    }
    private long _join_max = default(long);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"join_max", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long join_max
    {
      get { return _join_max; }
      set { _join_max = value; }
    }
    private long _out_score = default(long);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"out_score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long out_score
    {
      get { return _out_score; }
      set { _out_score = value; }
    }
    private long _win_limit = default(long);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"win_limit", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long win_limit
    {
      get { return _win_limit; }
      set { _win_limit = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"RoomType")]
    public enum RoomType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Gold", Value=1)]
      Gold = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Private", Value=16)]
      Private = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RedPack", Value=32)]
      RedPack = 32
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"PropType")]
    public enum PropType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Score", Value=1)]
      Score = 1
    }
  
}