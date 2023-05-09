//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: login.proto
// Note: requires additional types generated from: types.proto
namespace bs.login
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginReq")]
  public partial class LoginReq : global::ProtoBuf.IExtensible
  {
    public LoginReq() {}
    
    private uint _game_kind = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"game_kind", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint game_kind
    {
      get { return _game_kind; }
      set { _game_kind = value; }
    }
    private bs.login.LoginReq.LoginAction _action_type = bs.login.LoginReq.LoginAction.ByAccount;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"action_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.login.LoginReq.LoginAction.ByAccount)]
    public bs.login.LoginReq.LoginAction action_type
    {
      get { return _action_type; }
      set { _action_type = value; }
    }
    private string _account = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private string _password = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private string _version = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string version
    {
      get { return _version; }
      set { _version = value; }
    }
    private string _IP = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"IP", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string IP
    {
      get { return _IP; }
      set { _IP = value; }
    }
    private string _system_version = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"system_version", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string system_version
    {
      get { return _system_version; }
      set { _system_version = value; }
    }
    private uint _channel_id = default(uint);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"channel_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint channel_id
    {
      get { return _channel_id; }
      set { _channel_id = value; }
    }
    private uint _site_id = default(uint);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"site_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint site_id
    {
      get { return _site_id; }
      set { _site_id = value; }
    }
    private string _mobile_code = "";
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"mobile_code", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string mobile_code
    {
      get { return _mobile_code; }
      set { _mobile_code = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"LoginAction")]
    public enum LoginAction
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ByAccount", Value=0)]
      ByAccount = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Robot", Value=100)]
      Robot = 100
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginRsp")]
  public partial class LoginRsp : global::ProtoBuf.IExtensible
  {
    public LoginRsp() {}
    
    private bs.login.LoginRsp.Result _result = bs.login.LoginRsp.Result.SUCCESS;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.login.LoginRsp.Result.SUCCESS)]
    public bs.login.LoginRsp.Result result
    {
      get { return _result; }
      set { _result = value; }
    }
    private bs.types.BaseUserInfo _base_info = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"base_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public bs.types.BaseUserInfo base_info
    {
      get { return _base_info; }
      set { _base_info = value; }
    }
    private bs.types.ErrorInfo _err_info = null;
    [global::ProtoBuf.ProtoMember(99, IsRequired = false, Name=@"err_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public bs.types.ErrorInfo err_info
    {
      get { return _err_info; }
      set { _err_info = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Result")]
    public enum Result
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SUCCESS", Value=0)]
      SUCCESS = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NOTEXIST", Value=1)]
      NOTEXIST = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"FROZEN", Value=2)]
      FROZEN = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"FALSEPW", Value=3)]
      FALSEPW = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NETERROR", Value=4)]
      NETERROR = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"APPISBUSY", Value=5)]
      APPISBUSY = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GUESTFORBID", Value=6)]
      GUESTFORBID = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONNECTERROR", Value=7)]
      CONNECTERROR = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"VERSIONOLD", Value=8)]
      VERSIONOLD = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NOMOREGUEST", Value=9)]
      NOMOREGUEST = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"FREQUENTLY", Value=10)]
      FREQUENTLY = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"APPINITING", Value=11)]
      APPINITING = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SERVERERROR", Value=255)]
      SERVERERROR = 255,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UNKOWN", Value=1000)]
      UNKOWN = 1000,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TOKEN_FAILED", Value=1001)]
      TOKEN_FAILED = 1001,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TOKEN_EXPIRED", Value=1002)]
      TOKEN_EXPIRED = 1002,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TOKEN_NOTMATCH", Value=1003)]
      TOKEN_NOTMATCH = 1003
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LogoutReq")]
  public partial class LogoutReq : global::ProtoBuf.IExtensible
  {
    public LogoutReq() {}
    
    private ulong _user_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"user_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong user_id
    {
      get { return _user_id; }
      set { _user_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LogoutRsp")]
  public partial class LogoutRsp : global::ProtoBuf.IExtensible
  {
    public LogoutRsp() {}
    
    private bs.login.LogoutRsp.LogoutReason _reason = bs.login.LogoutRsp.LogoutReason.Normal;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(bs.login.LogoutRsp.LogoutReason.Normal)]
    public bs.login.LogoutRsp.LogoutReason reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    private bs.types.ErrorInfo _err_info = null;
    [global::ProtoBuf.ProtoMember(99, IsRequired = false, Name=@"err_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public bs.types.ErrorInfo err_info
    {
      get { return _err_info; }
      set { _err_info = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"LogoutReason")]
    public enum LogoutReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Normal", Value=0)]
      Normal = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"AnotherLogin", Value=1)]
      AnotherLogin = 1
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"CMDLogin")]
    public enum CMDLogin
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"IDLoginReq", Value=1)]
      IDLoginReq = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IDLoginRsp", Value=2)]
      IDLoginRsp = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IDLogoutReq", Value=3)]
      IDLogoutReq = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IDLogoutRsp", Value=4)]
      IDLogoutRsp = 4
    }
  
}