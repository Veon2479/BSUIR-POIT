
const
  S_OK = 0;                             {$EXTERNALSYM S_OK}
  S_FALSE = $00000001;                  {$EXTERNALSYM S_FALSE}
  E_NOINTERFACE = HRESULT($80004002);   {$EXTERNALSYM E_NOINTERFACE}
  E_UNEXPECTED = HRESULT($8000FFFF);    {$EXTERNALSYM E_UNEXPECTED}
  E_NOTIMPL = HRESULT($80004001);       {$EXTERNALSYM E_NOTIMPL}

type
  IInterface = interface
    ['{00000000-0000-0000-C000-000000000046}']
    function QueryInterface(const IID: TGUID; out Obj): HResult; stdcall;
    function _AddRef: Integer; stdcall;
    function _Release: Integer; stdcall;
  end;

  IUnknown = IInterface;

  IDispatch = interface(IUnknown)
    ['{00020400-0000-0000-C000-000000000046}']
    function GetTypeInfoCount(out Count: Integer): HResult; stdcall;
    function GetTypeInfo(Index, LocaleID: Integer; out TypeInfo): HResult; stdcall;
    function GetIDsOfNames(const IID: TGUID; Names: Pointer;
      NameCount, LocaleID: Integer; DispIDs: Pointer): HResult; stdcall;
    function Invoke(DispID: Integer; const IID: TGUID; LocaleID: Integer;
      Flags: Word; var Params; VarResult, ExcepInfo, ArgErr: Pointer): HResult; stdcall;
  end;

  IClassFactory = interface(IUnknown)
    ['{00000001-0000-0000-C000-000000000046}']
    function CreateInstance(const unkOuter: IUnknown; const iid: TGUID;
      out obj): HResult; stdcall;
    function LockServer(fLock: LongBool): HResult; stdcall;
  end;

  IClassFactory2 = interface(IClassFactory)
    ['{B196B28F-BAB4-101A-B69C-00AA00341D07}']
    function GetLicInfo(var licInfo: TLicInfo): HResult; stdcall;
    function RequestLicKey(dwResrved: Longint; out bstrKey: WideString): HResult;
      stdcall;
    function CreateInstanceLic(const unkOuter: IUnknown; const unkReserved: IUnknown;
      const iid: TIID; const bstrKey: WideString; out vObject): HResult; stdcall;
  end;
