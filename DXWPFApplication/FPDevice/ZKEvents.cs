using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
using System.Timers;
using zkemkeeper;

// Need to add zkemkeeper reference (Embed Interop Types must be False)
namespace FPDevice
{
    // [DllImport("kernel32.dll")] internal static extern uint SleepEx(uint dwMilliseconds, bool bAlertable); 

    #region Constants
    public enum ErrorCode
    {
        [Description("Operation failed or data not exist")]
        OperationFailed = -100, //Operation failed or data not exist
        [Description("Transmitted data length is incorrect")]
        IncorrectDataLength = -10, //Transmitted data length is incorrect
        [Description("Connection TimedOut")]
        TimedOut = -7, //Connection TimedOut
        [Description("Data already exists")]
        DataAlreadyExisted = -5, //Data already exists
        [Description("Space is not enough")]
        NotEnoughSpace = -4, //Space is not enough
        [Description("Error size")]
        InvalidSize = -3, //Error size
        [Description("Error in file read/write")]
        IOError = -2, //Error in file read/write
        [Description("SDK is not initialized and needs to be reconnected")]
        NotInitialized = -1, //SDK is not initialized and needs to be reconnected
        [Description("Data not found or data repeated")]
        DataNotFound = 0, //Data not found or data repeated
        [Description("Operation is correct")]
        NoError = 1, //Operation is correct
        [Description("Parameter is incorrect")]
        IncorrectParameter = 4, //Parameter is incorrect
        [Description("Error in allocating buffer")]
        BufferAllocationError = 101, //Error in allocating buffer
    }

    public enum InOutMode //AttState
    {
        CheckIn = 0,
        CheckOut = 1,
        BreakOut = 2,
        BreakIn = 3,
        OTIn = 4,
        OTOut = 5,
    }

    public enum VerifyMethod
    {
        UnSet = -1,
        Password = 0,
        FingerPrint = 1,
        Card = 2,
    }

    public enum MultiVerifyMode
    {
        FP_OR_PW_OR_RF = 0,
        FP = 1,
        PIN = 2,
        PW = 3,
        RF = 4,
        FP_OR_PW = 5,
        FP_OR_RF = 6,
        PW_OR_RF = 7,
        PIN_AND_FP = 8,
        FP_AND_PW = 9,
        FP_AND_RF = 10,
        PW_AND_RF = 11,
        FP_AND_PW_AND_RF = 12,
        PIN_AND_FP_AND_PW = 13,
        FP_AND_RF_OR_PIN = 14,
    }

    public enum BatchUpdateFlag
    {
        NotOverwrite = 0,
        Overwrite = 1, // force overwrite
    }

    public enum DataFlag
    {
        AttLog = 1, //Attendance record data file
        FPTemplate = 2, //Fingerprint template data file
        None = 3, //None
        OpLog = 4, //Operation record data file
        UserInfo = 5, //User information data file (ClearData only use 1-5, if it's 5 All User data including templates also deleted)
        SMS = 6, //SMS data file
        SMSUserRelation = 7, //SMS and user relationship data file
        ExtUserInfo = 8, //Extended user information data file
        WorkCode = 9, //Work code data file
    }

    public enum TemplateFlag // Valid Flag?
    {
        Normal = 1, //Ordinary?
        Forced = 3, //Threatened?
    }

    public enum EnrollBackupNumber
    {
        Finger0 = 0,
        Finger1 = 1,
        Finger2 = 2,
        Finger3 = 3,
        Finger4 = 4,
        Finger5 = 5,
        Finger6 = 6,
        Finger7 = 7,
        Finger8 = 8,
        Finger9 = 9,
        Password = 10,
        AllFingers = 11,
        AllUserData = 12,
        AllFingers_ = 13,
    }

    public enum UserPrivilege
    {
        User = 0,
        Enroller = 1, 
        Admin = 2,
        Supervisor = 3, //SuperAdmin?
    }

    public enum DeviceStatusType
    {
        AdminCount = 1,
        UserCount = 2,
        FPTemplateCount = 3,
        PasswordCount = 4,
        OpLogCount = 5,
        AttLogCount = 6,
        MaxFPTemplate = 7,
        MaxUser = 8,
        MaxAttLog = 9,
        FreeFPTemplate = 10, // Remaining
        FreeUser = 11,
        FreeAttLog = 12,

        FaceCount = 21,
        MaxFace = 22,
    }

    public enum EventMask
    {
        OnAttTransaction = 0x01, // also OnAttTransactionEx
        OnFinger = 0x02,
        OnNewUser = 0x04,
        OnEnrollFinger = 0x08,
        OnKeyPress = 0x10,
        OnVerify = 0x100,
        OnFingerFeature = 0x200,
        OnDoorAlarm = 0x400, // OnDoor & OnAlarm
        OnHIDNum = 0x800,
        OnWriteCard = 0x1000,
        OnEmptyCard = 0x2000,
        OnDeleteTemplate = 0x4000,
    }

    public enum DeviceInfoType
    {
        MaxAdmin = 1,
        DeviceNumber = 2,
        Language = 3,
        IdleDuration = 4,
        LockDuration = 5,
        AttLogAlarmThreshold = 6,
        OpLogAlarmThreshold = 7,
        RepeatedRecordTime = 8,
        BaudRate = 9,
        ParityCheck = 10, 
        StopBit = 11,
        DateSeparator = 12,
        NetworkEnabled = 13,
        RS232Enabled = 14,
        RS485Enabled = 15,
        VoiceFunctionSupported = 16,
        HiSpeedComparison = 17,
        IdleMode = 18,
        AutoPowerOffTimePoint = 19,
        AutoPowerOnTimePoint = 20,
        AutoHibernateTimePoint = 21,
        AutoRingTimePoint = 22,
        MatchThreshold1toN = 23,
        MatchThresholdRegistration = 24,
        MatchThreshold1to1 = 25,
        ScoreDisplayed = 26,
        ConcurrentUnlockUserCount = 27,
        VerifyCardNumberOnly = 28,
        NetworkSpeed = 29,
        CardMustBeRegistered = 30,
        NoOpWaitingTime = 31,
        PINNoResponseWaitingTime = 32,
        MenuNoResponseWaitingTime = 33,
        TimeFormat = 34,
        MustUse1to1Match = 35,
        AutoRingTimePoint2 = 36,
        AutoRingTimePoint3 = 37,
        AutoRingTimePoint4 = 38,
        AutoRingTimePoint5 = 39,
        AutoRingTimePoint6 = 40,
        AutoStateChangeTimePoint1 = 41,
        AutoStateChangeTimePoint2 = 42,
        AutoStateChangeTimePoint3 = 43,
        AutoStateChangeTimePoint4 = 44,
        AutoStateChangeTimePoint5 = 45,
        AutoStateChangeTimePoint6 = 46,
        AutoStateChangeTimePoint7 = 47,
        AutoStateChangeTimePoint8 = 48,
        AutoStateChangeTimePoint9 = 49,
        AutoStateChangeTimePoint10 = 50,
        AutoStateChangeTimePoint11 = 51,
        AutoStateChangeTimePoint12 = 52,
        AutoStateChangeTimePoint13 = 53,
        AutoStateChangeTimePoint14 = 54,
        AutoStateChangeTimePoint15 = 55,
        AutoStateChangeTimePoint16 = 56,
        WiegandFailureID = 57,
        WiegandThreatenID = 58,
        WiegandPositionCode = 59,
        WiegandOutputPulseWidth = 60,
        WiegandOutputPulseInterval = 61,
        MifareCardStartSector = 62,
        MifareCardTotalSector = 63,
        MifareCardFPCount = 64,

        AttStateDisplayed = 66,
        TemporarilyUnavailable = 67,
        NotSupported = 68,

        IOValue = 8999, //dwValue is similar to GetSysOption
    }

    public static class Constants
    {
        public const int MAXTEMPLATESIZE = 602;
        public const int MAX_SMS_CONTENT_SIZE = 60; // + 1 for null terminated?
        public const int FACE_TMP_MAXSIZE = 1024 * 2 + 512;
    }
    #endregion

    #region Data Structures
    // Fingerprint template data might be encoded using Base64
    //Data structure of 9.0 fingerprint template:
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct _Template9_
    { 
        public UInt16 Size; //fingerprint template length
        public UInt16 PIN; //internal user ID, which corresponds to PIN2 in user table
        public byte FingerID; // fingerprint backup index
        public byte Valid;
        public fixed byte Template[Constants.MAXTEMPLATESIZE]; //maximize template length
    } //MAXTEMPLATESIZE 602 Bytes

    //Data structure of template.fp10:
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct _Template10_
    {
        public UInt16 Size; //entire structure data size
        public UInt16 PIN; //user ID
        public byte FingerID; //fingerprint index
        public byte Valid; //flag
        public byte* Template; //template(s)
    } 
    #endregion

    #region Conversion Functions
    public static class Convertion
    {
        public static T ByteArrayToStruct<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }

        public static byte[] StructToByteArray<T>(T structobj) where T : struct
        {
            int size = Marshal.SizeOf(structobj);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(structobj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        // This will return the Windows zone that matches the IANA zone, if one exists.
        public static string IanaToWindows(string ianaZoneId)
        {
            var utcZones = new[] { "Etc/UTC", "Etc/UCT" };
            if (utcZones.Contains(ianaZoneId, StringComparer.OrdinalIgnoreCase))
                return "UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // resolve any link, since the CLDR doesn't necessarily use canonical IDs
            var links = tzdbSource.CanonicalIdMap
              .Where(x => x.Value.Equals(ianaZoneId, StringComparison.OrdinalIgnoreCase))
              .Select(x => x.Key);

            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(links.Contains));
            if (item == null) return ""; //return null;
            return item.WindowsId;
        }

        // This will return the "primary" IANA zone that matches the given windows zone.
        // If the primary zone is a link, it then resolves it to the canonical ID.
        public static string WindowsToIana(string windowsZoneId)
        {
            if (windowsZoneId.Equals("UTC", StringComparison.OrdinalIgnoreCase))
                return "Etc/UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(windowsZoneId);
            var tzid = tzdbSource.MapTimeZoneId(tzi);// ?? windowsZoneId;
            // Unmappable Windows Time Zone (Mid-Atlantic Standard Time, E. Europe Standard Time)
            if (tzid == null || tzid.Trim() == "") tzid = "Etc/GMT+2";
            return tzdbSource.CanonicalIdMap[tzid] ?? "";
        }
    }
    #endregion

    #region Static Class
    // --- Prevent Exception when accessed with non-existing key ---
    public interface INullValueDictionary<T, U>
    where U : class
    {
        U this[T key] { get; set; }
        void Add(T key, U value);
        bool Remove(T key);
        int Count();
        Dictionary<T, U>.Enumerator GetEnumerator();
    }

    public class NullValueDictionary<T, U> : Dictionary<T, U>, INullValueDictionary<T, U>
        where U : class
    {
        private Dictionary<T, U> dict;

        public NullValueDictionary()
        {
            dict = new Dictionary<T, U>();
        }

        U INullValueDictionary<T, U>.this[T key]
        {
            get
            {
                U val;
                dict.TryGetValue(key, out val);
                return val;
            }

            set { dict[key] = value; }
        }

        public new void Add(T key, U value)
        {
            dict.Add(key, value);
        }

        public new bool Remove(T key)
        {
            return dict.Remove(key);
        }

        public new int Count()
        {
            return dict.Count();
        }

        public new Dictionary<T, U>.Enumerator GetEnumerator()
        {
            return dict.GetEnumerator();
        }

    }
    // ---------------------------------------------------

    public static class FPMachines // Persistent objects
    {
        public static INullValueDictionary<int, FPDevice.ZKEvents> fpDevices { get; set; }

        static FPMachines()
        {
            fpDevices = new NullValueDictionary<int, FPDevice.ZKEvents>();
        }
    }
    #endregion

    public class ZKEvents
    {
        //Create Standalone SDK class dynamicly.
        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        System.Timers.Timer zkTimer1 = new System.Timers.Timer(100);
        public object _locker = new object(); //private static object // Mutex _locker
        //public bool bIsBusy = false;

        public Action<int, int> OnConnected;
        public Action<int, int> OnDisConnected;
        public Action<int, int> OnFinger;
        public Action<int> OnVerify; //(int iUserID);
        public Action<string, int, int, int, int, int, int, int, int, int, int, int, int> OnAttTransactionEx; //(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode);
        public Action<int, int, int, int, int, int, int, int, int, int> OnAttTransaction; //(int iEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond);
        public Action<int> OnFingerFeature; //(int iScore);
        public Action<int, int, int, int, int, int> OnEnrollFinger; //(int iEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength);
        public Action<int> OnKeyPress; //(int iKey);
        public Action<int, int> OnDeleteTemplate; //(int iEnrollNumber, int iFingerIndex);
        public Action<int> OnNewUser; //(int iEnrollNumber);
        public Action<int> OnHIDNum; //(int iCardNumber);
        public Action<int, int, int> OnAlarm; //(int iAlarmType, int iEnrollNumber, int iVerified);
        public Action<int> OnDoor; //(int iEventType);
        public Action<int> OnEmptyCard; //(int iActionResult);
        public Action<int, int, int> OnWriteCard; //(int iEnrollNumber, int iActionResult, int iLength);
        public delegate void OnEMDataType(int DataType, int DataLen, ref sbyte DataBuffer);
        public OnEMDataType OnEMData; //(LONG DataType, LONG DataLen, CHAR* DataBuffer);

        //Constructor
        public ZKEvents()
        {
            //bIsDestroying = false;
            OnAlarm = axCZKEM1_OnAlarm;
            OnAttTransaction = axCZKEM1_OnAttTransaction;
            //OnAttTransactionEx = axCZKEM1_OnAttTransactionEx;
            //OnConnected = axCZKEM1_OnConnected;
            OnDeleteTemplate = axCZKEM1_OnDeleteTemplate;
            //OnDisConnected = axCZKEM1_OnDisConnected;
            OnDoor = axCZKEM1_OnDoor;
            OnEmptyCard = axCZKEM1_OnEmptyCard;
            //OnEnrollFinger = axCZKEM1_OnEnrollFinger;
            //OnFinger = axCZKEM1_OnFinger;
            OnFingerFeature = axCZKEM1_OnFingerFeature;
            OnHIDNum = axCZKEM1_OnHIDNum;
            OnKeyPress = axCZKEM1_OnKeyPress;
            OnNewUser = axCZKEM1_OnNewUser;
            OnVerify = axCZKEM1_OnVerify;
            OnWriteCard = axCZKEM1_OnWriteCard;
            OnEMData = axCZKEM1_OnEMData;
        }

        //Destructor
        [STAThread]
        ~ZKEvents()
        {
            bIsDestroying = true;
            zkTimer1.Dispose();
            zkTimer1 = null;
            _locker = null;
        }

        [STAThread]
        public void Dispose()
        {
            bIsDestroying = true;
            if (axCZKEM1 != null)
            {
                Disconnect();
                axCZKEM1 = null;
            }
            // Dispose of unmanaged resources.
            //Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /********************************************************************************************************************************************
        * Before you refer to this demo,we strongly suggest you read the development manual deeply first.                                           *
        * This part is for demonstrating the communication with your device.There are 3 communication ways: "TCP/IP","Serial Port" and "USB Client".*
        * The communication way which you can use duing to the model of the device.                                                                 *
        * *******************************************************************************************************************************************/
        #region Communication
        public bool bIsConnected = false; //the boolean value identifies whether the device is connected
        public int iMachineNumber = 1; //the serial number of the device.After connecting the device ,this value will be changed.
        public bool bIsDisconnecting = false;
        public bool bIsDestroying = false;
        public bool bIsDisabled = false;
        public int iLastError = 1; // 1 = OK/No Error
        public int TimerInterval = 100; //100ms
        public int DisabledTime = 7200; //7200 sec
        public int Tag = 0;

        public void RegisterEvents()
        {
            this.axCZKEM1.OnConnected += new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(axCZKEM1_OnConnected);
            this.axCZKEM1.OnDisConnected += new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(axCZKEM1_OnDisConnected);
            this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger); //axCZKEM1_
            this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(OnVerify);
            this.axCZKEM1.OnAttTransaction += new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(OnAttTransaction);
            this.axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(OnFingerFeature);
            this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(OnKeyPress);
            this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);
            this.axCZKEM1.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(OnDeleteTemplate);
            this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(OnNewUser);
            this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(OnHIDNum);
            this.axCZKEM1.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(OnAlarm);
            this.axCZKEM1.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(OnDoor);
            this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(OnWriteCard);
            this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(OnEmptyCard);
            this.axCZKEM1.OnEMData += new zkemkeeper._IZKEMEvents_OnEMDataEventHandler(OnEMData);

            //if (zkTimer1 == null) zkTimer1 = new System.Timers.Timer(TimerInterval);
            this.zkTimer1.Elapsed += new ElapsedEventHandler(this.zkTimer1_Tick);
            this.zkTimer1.Interval = TimerInterval;
            this.zkTimer1.Enabled = true;
            //this.zkTimer1.Start();
        }

        public void UnregisterEvents()
        {
            //this.zkTimer1.Stop();
            this.zkTimer1.Enabled = false;
            this.zkTimer1.Elapsed -= new ElapsedEventHandler(this.zkTimer1_Tick);
            //zkTimer1.Dispose();
            //zkTimer1 = null;

            this.axCZKEM1.OnConnected -= new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(axCZKEM1_OnConnected);
            this.axCZKEM1.OnDisConnected -= new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(axCZKEM1_OnDisConnected);
            this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger); //axCZKEM1_
            this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(OnVerify);
            this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(OnAttTransaction);
            this.axCZKEM1.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(OnFingerFeature);
            this.axCZKEM1.OnKeyPress -= new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(OnKeyPress);
            this.axCZKEM1.OnEnrollFinger -= new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);
            this.axCZKEM1.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(OnDeleteTemplate);
            this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(OnNewUser);
            this.axCZKEM1.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(OnHIDNum);
            this.axCZKEM1.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(OnAlarm);
            this.axCZKEM1.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(OnDoor);
            this.axCZKEM1.OnWriteCard -= new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(OnWriteCard);
            this.axCZKEM1.OnEmptyCard -= new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(OnEmptyCard);
            this.axCZKEM1.OnEMData -= new zkemkeeper._IZKEMEvents_OnEMDataEventHandler(OnEMData);
        }

        //[STAThread]
        //public void WaitForReady(int mstime = 5000) // 0 = infinite
        //{
        //    int time = mstime;
        //    while (bIsConnected && (ilock > 0) && time >= 0)
        //    {
        //        Thread.Sleep(10);
        //        if (mstime == 0) continue;
        //        time -= 10;
        //    }
        //}

        public static string GetEnumDesc(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public string GetErrorMsg(int Code)
        {
            if (Enum.IsDefined(typeof(ErrorCode), Code))
            {
                return GetEnumDesc((ErrorCode)Code);
            }
            return Code.ToString();
        }

        // Get device's Last Error code
        [STAThread]
        public int GetLastError()
        {
            lock (_locker)
            {
                axCZKEM1.GetLastError(ref iLastError);
            }
            return iLastError;
        }

        [STAThread]
        public string GetLastErrorMsg()
        {
            string ret = iLastError.ToString();
            lock (_locker)
            {
                axCZKEM1.GetLastError(ref iLastError);
                if (Enum.IsDefined(typeof(ErrorCode), iLastError))
                {
                    ret = GetEnumDesc((ErrorCode)iLastError);
                }
            }
            return ret;
        }

        // Disable device
        [STAThread]
        public bool Disable(int TimeOutSec = 0)
        {
            bool disabled = false;
            lock (_locker)
            {
                //WaitForReady(); //while (bIsConnected && bIsBusy) Thread.Sleep(0);
                if (TimeOutSec < 0)
                {

                    disabled = axCZKEM1.EnableDevice(iMachineNumber, false);
                }
                else
                {
                    int sec = TimeOutSec;
                    if (sec == 0) sec = DisabledTime;
                    disabled = axCZKEM1.DisableDeviceWithTimeOut(iMachineNumber, sec);
                }
                if (disabled) bIsDisabled = true;
            }
            return disabled;
        }

        // Enable device
        [STAThread]
        public bool Enable()
        {
            bool enabled = false;
            lock (_locker)
            {
                //WaitForReady(); //while (bIsConnected && bIsBusy) Thread.Sleep(0);
                enabled = axCZKEM1.EnableDevice(iMachineNumber, true);
                if (enabled) bIsDisabled = false;
            }
            return enabled;
        }

        // Refresh device data
        [STAThread]
        public bool Refresh()
        {
            lock (_locker)
            {
                //WaitForReady(); //while (bIsConnected && bIsBusy) Thread.Sleep(0);
                return axCZKEM1.RefreshData(iMachineNumber);
            }
            //return false;
        }

        // Disconnect device
        [STAThread]
        public void Disconnect()
        {
            lock (_locker)
            {
                bIsDisconnecting = true;
                UnregisterEvents();

                //WaitForReady(); //while (bIsConnected && bIsBusy) Thread.Sleep(0);
                axCZKEM1.Disconnect();

                bIsConnected = false;
                bIsDisconnecting = false;
                //bIsDisabled = false;
            }
            return;
        }

        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
        [STAThread]
        public bool ConnectIP(string ip, int port)
        {
            if (ip.Trim() == "" || port <= 0)
            {
                throw new ArgumentNullException("IP and Port cannot be empty");
                //return false;
            }
            //int idwErrorCode = 0;
            if (bIsDestroying) return false;

            lock (_locker)
            {
                //ilock++; //bIsBusy = true;
                bIsConnected = axCZKEM1.Connect_Net(ip, port);
                if (bIsConnected)
                {
                    iMachineNumber = 1; //In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.

                    if (axCZKEM1.RegEvent(iMachineNumber, 65535)) //Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                    {
                        RegisterEvents();
                    }
                }
                else
                {
                    axCZKEM1.GetLastError(ref iLastError);
                    //throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
                }
                //ilock--; //bIsBusy = false;
            }
            return bIsConnected;
        }

        //If your device supports the SerialPort communications, you can refer to this.
        [STAThread]
        public bool ConnectRS232(string ComPort, int iBaudRate, int MachineSN)
        {
            if (ComPort.Trim() == "" || iBaudRate <= 0 || MachineSN <= 0)
            {
                throw new ArgumentNullException("Port, BaudRate, and MachineSN cannot be empty");
                //return false;
            }
            //int idwErrorCode = 0;
            //accept serialport number from string like "COMi"
            if (bIsDestroying) return false;

            int iPort;
            string sPort = ComPort.Trim();
            for (iPort = 1; iPort < 10; iPort++)
            {
                if (sPort.IndexOf(iPort.ToString()) > -1)
                {
                    break;
                }
            }

            lock (_locker)
            {
                //ilock++; //bIsBusy = true;
                iMachineNumber = MachineSN; //when you are using the serial port communication,you can distinguish different devices by their serial port number.
                bIsConnected = axCZKEM1.Connect_Com(iPort, iMachineNumber, iBaudRate);

                if (bIsConnected == true)
                {
                    if (axCZKEM1.RegEvent(iMachineNumber, 65535)) //Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                    {
                        RegisterEvents();
                    }
                }
                else
                {
                    axCZKEM1.GetLastError(ref iLastError);
                    //throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
                }
                //ilock--; //bIsBusy = false;
            }
            return bIsConnected;
        }

        //If your device supports the USBCLient, you can refer to this.
        //Not all series devices can support this kind of connection.Please make sure your device supports USBClient.
        [STAThread]
        public bool ConnectUSB(bool VirtualUSB, int VirtualUSBMachineSN = 1)
        {
            if (bIsDestroying) return false;

            lock (_locker)
            {
                //int idwErrorCode = 0;
                //ilock++; //bIsBusy = true;
                if (!VirtualUSB) //the common USBClient
                {
                    iMachineNumber = 1; //In fact,when you are using common USBClient communication,parameter Machinenumber will be ignored,that is any integer will all right.Here we use 1.
                    bIsConnected = axCZKEM1.Connect_USB(iMachineNumber);
                }
                else //connect the device via the virtual serial port created by USB
                {
                    SearchForUSBCom usbcom = new SearchForUSBCom();
                    string sCom = "";
                    bool bSearch = usbcom.SearchforCom(ref sCom);//modify by Darcy on Nov.26 2009
                    if (bSearch == false)//modify by Darcy on Nov.26 2009
                    {
                        throw new ArgumentNullException("Can not find the virtual serial port that can be used");
                        //return false;
                    }

                    int iPort;
                    for (iPort = 1; iPort < 10; iPort++)
                    {
                        if (sCom.IndexOf(iPort.ToString()) > -1)
                        {
                            break;
                        }
                    }

                    iMachineNumber = VirtualUSBMachineSN;
                    if (iMachineNumber == 0 || iMachineNumber > 255)
                    {
                        throw new ArgumentNullException("The Machine Number is invalid!");
                        //return false;
                    }

                    int iBaudRate = 115200; //115200 is one possible baudrate value(its value cannot be 0)
                    bIsConnected = axCZKEM1.Connect_Com(iPort, iMachineNumber, iBaudRate);
                }

                if (bIsConnected == true)
                {
                    if (axCZKEM1.RegEvent(iMachineNumber, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                    {
                        RegisterEvents();
                    }
                }
                else
                {
                    axCZKEM1.GetLastError(ref iLastError);
                    //throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
                }
                //ilock--; //bIsBusy = false;
            }
            return bIsConnected;
        }

        #endregion


        /*************************************************************************************************
        * Before you refer to this demo,we strongly suggest you read the development manual deeply first.*
        * This part is for demonstrating the RealTime Events that triggered  by your operations          *
        **************************************************************************************************/
        #region RealTime Events

        //This event is triggered when the PC connects to the device successfully
        public virtual void axCZKEM1_OnConnected()
        {
            //lbRTShow.Items.Add("RTEvent OnConnected Has been Triggered");
            if (OnConnected != null) OnConnected(iMachineNumber, Tag);
        }

        //This event is triggered when the PC disconnects from the device successfully
        public virtual void axCZKEM1_OnDisConnected()
        {
            //lbRTShow.Items.Add("RTEvent OnDisConnected Has been Triggered");
            if (OnDisConnected != null) OnDisConnected(iMachineNumber, Tag);
        }

        //When you place your finger on sensor of the device,this event will be triggered
        public virtual void axCZKEM1_OnFinger()
        {
            //lbRTShow.Items.Add("RTEvent OnFinger Has been Triggered");
            if (OnFinger != null) OnFinger(iMachineNumber, Tag);
        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        public virtual void axCZKEM1_OnVerify(int iUserID)
        {
            //lbRTShow.Items.Add("RTEvent OnVerify Has been Triggered,Verifying...");
            if (iUserID != -1)
            {
                //lbRTShow.Items.Add("Verified OK,the UserID is " + iUserID.ToString());
            }
            else
            {
                //lbRTShow.Items.Add("Verified Failed... ");
            }
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered
        public virtual void axCZKEM1_OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            if (OnAttTransactionEx != null) OnAttTransactionEx(sEnrollNumber, iIsInValid, iAttState, iVerifyMethod, iYear, iMonth, iDay, iHour, iMinute, iSecond, iWorkCode, iMachineNumber, Tag);
            //lbRTShow.Items.Add("RTEvent OnAttTrasactionEx Has been Triggered,Verified OK");
            //lbRTShow.Items.Add("...UserID:" + sEnrollNumber);
            //lbRTShow.Items.Add("...isInvalid:" + iIsInValid.ToString());
            //lbRTShow.Items.Add("...attState:" + iAttState.ToString());
            //lbRTShow.Items.Add("...VerifyMethod:" + iVerifyMethod.ToString());
            //lbRTShow.Items.Add("...Workcode:" + iWorkCode.ToString());//the difference between the event OnAttTransaction and OnAttTransactionEx
            //lbRTShow.Items.Add("...Time:" + iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + iHour.ToString() + ":" + iMinute.ToString() + ":" + iSecond.ToString());
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered
        public virtual void axCZKEM1_OnAttTransaction(int iEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond)
        {
            //lbRTShow.Items.Add("RTEvent OnAttTrasaction Has been Triggered,Verified OK");
            //lbRTShow.Items.Add("...UserID:" + iEnrollNumber.ToString());
            //lbRTShow.Items.Add("...isInvalid:" + iIsInValid.ToString());
            //lbRTShow.Items.Add("...attState:" + iAttState.ToString());
            //lbRTShow.Items.Add("...VerifyMethod:" + iVerifyMethod.ToString());
            //lbRTShow.Items.Add("...Time:" + iYear.ToString() + "-" + iMonth.ToString() + "-" + iDay.ToString() + " " + " " + iHour.ToString() + ":" + iMinute.ToString() + ":" + iSecond.ToString());
        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        public virtual void axCZKEM1_OnFingerFeature(int iScore)
        {
            if (iScore < 0)
            {
                //lbRTShow.Items.Add("The quality of your fingerprint is poor");
            }
            else
            {
                //lbRTShow.Items.Add("RTEvent OnFingerFeature Has been Triggered...Score:　" + iScore.ToString());
            }
        }

        //When you are enrolling your finger,this event will be triggered.
        public virtual void axCZKEM1_OnEnrollFinger(int iEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength)
        {
            if (OnEnrollFinger != null) OnEnrollFinger(iEnrollNumber, iFingerIndex, iActionResult, iTemplateLength, iMachineNumber, Tag);
            if (iActionResult == 0)
            {
                //lbRTShow.Items.Add("RTEvent OnEnrollFiger Has been Triggered....");
                //lbRTShow.Items.Add(".....UserID: " + iEnrollNumber + " Index: " + iFingerIndex.ToString() + " tmpLen: " + iTemplateLength.ToString());
            }
            else
            {
                //lbRTShow.Items.Add("RTEvent OnEnrollFiger was Triggered by Error");
            }
        }

        //When you press the keypad,this event will be triggered.
        public virtual void axCZKEM1_OnKeyPress(int iKey)
        {
            //lbRTShow.Items.Add("RTEvent OnKeyPress Has been Triggered, Key: " + iKey.ToString());
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        public virtual void axCZKEM1_OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {
            //lbRTShow.Items.Add("RTEvent OnDeleteTemplate Has been Triggered...");
            //lbRTShow.Items.Add("...UserID=" + iEnrollNumber.ToString() + " FingerIndex=" + iFingerIndex.ToString());
        }

        //When you have enrolled a new user,this event will be triggered.
        public virtual void axCZKEM1_OnNewUser(int iEnrollNumber)
        {
            //lbRTShow.Items.Add("RTEvent OnNewUser Has been Triggered...");
            //lbRTShow.Items.Add("...NewUserID=" + iEnrollNumber.ToString());
        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        public virtual void axCZKEM1_OnHIDNum(int iCardNumber)
        {
            //lbRTShow.Items.Add("RTEvent OnHIDNum Has been Triggered...");
            //lbRTShow.Items.Add("...Cardnumber=" + iCardNumber.ToString());
        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        public virtual void axCZKEM1_OnAlarm(int iAlarmType, int iEnrollNumber, int iVerified)
        {
            //lbRTShow.Items.Add("RTEvnet OnAlarm Has been Triggered...");
            //lbRTShow.Items.Add("...AlarmType=" + iAlarmType.ToString());
            //lbRTShow.Items.Add("...EnrollNumber=" + iEnrollNumber.ToString());
            //lbRTShow.Items.Add("...Verified=" + iVerified.ToString());
        }

        //Door sensor event
        public virtual void axCZKEM1_OnDoor(int iEventType)
        {
            //lbRTShow.Items.Add("RTEvent Ondoor Has been Triggered...");
            //lbRTShow.Items.Add("...EventType=" + iEventType.ToString());
        }

        //When you have emptyed the Mifare card,this event will be triggered.
        public virtual void axCZKEM1_OnEmptyCard(int iActionResult)
        {
            //lbRTShow.Items.Add("RTEvent OnEmptyCard Has been Triggered...");
            if (iActionResult == 0)
            {
                //lbRTShow.Items.Add("...Empty Mifare Card OK");
            }
            else
            {
                //lbRTShow.Items.Add("...Empty Failed");
            }
        }

        //When you have written into the Mifare card ,this event will be triggered.
        public virtual void axCZKEM1_OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {
            //lbRTShow.Items.Add("RTEvent OnWriteCard Has been Triggered...");
            if (iActionResult == 0)
            {
                //lbRTShow.Items.Add("...Write Mifare Card OK");
                //lbRTShow.Items.Add("...EnrollNumber=" + iEnrollNumber.ToString());
                //lbRTShow.Items.Add("...TmpLength=" + iLength.ToString());
            }
            else
            {
                //lbRTShow.Items.Add("...Write Failed");
            }
        }

        //This event is triggered when the device sends an unknown event to SDK
        public virtual unsafe void axCZKEM1_OnEMData(int DataType, int DataLen, ref sbyte DataBuffer)
        {
            //lbRTShow.Items.Add("RTEvent OnEMData Has been Triggered...");
            //lbRTShow.Items.Add("...DataType=" + DataType.ToString());
        }

        //After function GetRTLog() is called ,RealTime Events will be triggered. 
        //When you are using these two functions, it will request data from the device forwardly.
        [STAThread]
        protected virtual void zkTimer1_Tick(object sender, ElapsedEventArgs e) // 100ms interval
        {
            try
            {
                zkTimer1.Enabled = false; //(sender as System.Timers.Timer).Enabled = false;
                if (bIsConnected && !bIsDestroying && !bIsDisabled)
                {
                    //if (ilock > 0) break;
                    lock (_locker)
                    {
                        //ilock++; //bIsBusy = true;
                        if (axCZKEM1.ReadRTLog(iMachineNumber))
                        {
                            while (bIsConnected && !bIsDestroying && !bIsDisabled && axCZKEM1.GetRTLog(iMachineNumber))
                            {
                                ;
                            }
                        }
                        else
                        {
                            axCZKEM1.GetLastError(ref iLastError);
                            if (iLastError == (int)ErrorCode.TimedOut || iLastError == (int)ErrorCode.NotInitialized) Disconnect();
                        }
                        //ilock--; //bIsBusy = false;
                    }
                    //if (bIsDisconnecting) break;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                zkTimer1.Enabled = bIsConnected;
            }
        }

        #endregion

    }
}
