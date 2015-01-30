using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Timers;
using zkemkeeper;

namespace FPDevice
{
    // [DllImport("kernel32.dll")] internal static extern uint SleepEx(uint dwMilliseconds, bool bAlertable); 

    // --- Prevent Exception when accessed with non-existing key ---
    public interface INullValueDictionary<T, U>
    where U : class
    {
        U this[T key] { get; set; }
        void Add(T key, U value);
        bool Remove(T key);
        int Count();
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

    public class ZKEvents
    {
        //Create Standalone SDK class dynamicly.
        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        System.Timers.Timer zkTimer1 = new System.Timers.Timer(100);

        //Destructor
        [STAThread]
        ~ZKEvents()
        {
            Disconnect();
            zkTimer1.Dispose();
            zkTimer1 = null;
        }

        [STAThread]
        public void Dispose()
        {
            if (axCZKEM1 != null)
            {
                axCZKEM1.Disconnect();
                //axCZKEM1.Dispose();
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
        public bool bIsBusy = false;
        public bool bIsDisconnecting = false;

        public void RegisterEvents()
        {
            this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
            this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            this.axCZKEM1.OnAttTransaction += new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
            this.axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
            this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
            this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);
            this.axCZKEM1.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
            this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
            this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
            this.axCZKEM1.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
            this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
            this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);

            //if (zkTimer1 == null) zkTimer1 = new System.Timers.Timer(100);
            this.zkTimer1.Elapsed += new ElapsedEventHandler(this.zkTimer1_Tick);
            this.zkTimer1.Interval = 100;
            //this.zkTimer1.Enabled = true;
            this.zkTimer1.Start();
        }

        public void UnregisterEvents()
        {
            this.zkTimer1.Stop();
            //this.zkTimer1.Enabled = false;
            this.zkTimer1.Elapsed -= new ElapsedEventHandler(this.zkTimer1_Tick);
            //zkTimer1.Dispose();
            //zkTimer1 = null;

            this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
            this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
            this.axCZKEM1.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
            this.axCZKEM1.OnKeyPress -= new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
            this.axCZKEM1.OnEnrollFinger -= new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);
            this.axCZKEM1.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
            this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
            this.axCZKEM1.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
            this.axCZKEM1.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
            this.axCZKEM1.OnWriteCard -= new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
            this.axCZKEM1.OnEmptyCard -= new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
        }

        // Disconnect device
        [STAThread]
        public void Disconnect()
        {
            bIsDisconnecting = true;
            UnregisterEvents();
            
            while (bIsConnected && bIsBusy) Thread.Sleep(0);
            axCZKEM1.Disconnect();

            bIsConnected = false;
            bIsDisconnecting = false;
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
            int idwErrorCode = 0;

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
                axCZKEM1.GetLastError(ref idwErrorCode);
                throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
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
            int idwErrorCode = 0;
            //accept serialport number from string like "COMi"
            int iPort;
            string sPort = ComPort.Trim();
            for (iPort = 1; iPort < 10; iPort++)
            {
                if (sPort.IndexOf(iPort.ToString()) > -1)
                {
                    break;
                }
            }

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
                axCZKEM1.GetLastError(ref idwErrorCode);
                throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
            }
            return bIsConnected;
        }

        //If your device supports the USBCLient, you can refer to this.
        //Not all series devices can support this kind of connection.Please make sure your device supports USBClient.
        [STAThread]
        public bool ConnectUSB(bool VirtualUSB, int VirtualUSBMachineSN = 1)
        {
            int idwErrorCode = 0;

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

                int iBaudRate = 115200;//115200 is one possible baudrate value(its value cannot be 0)
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
                axCZKEM1.GetLastError(ref idwErrorCode);
                throw new Exception("Unable to connect the device! ErrorCode=" + idwErrorCode.ToString());
            }
            return bIsConnected;
        }

        #endregion


        /*************************************************************************************************
        * Before you refer to this demo,we strongly suggest you read the development manual deeply first.*
        * This part is for demonstrating the RealTime Events that triggered  by your operations          *
        **************************************************************************************************/
        #region RealTime Events

        //When you place your finger on sensor of the device,this event will be triggered
        public virtual void axCZKEM1_OnFinger()
        {
            //lbRTShow.Items.Add("RTEvent OnFinger Has been Triggered");
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

        //After function GetRTLog() is called ,RealTime Events will be triggered. 
        //When you are using these two functions, it will request data from the device forwardly.
        [STAThread]
        protected virtual void zkTimer1_Tick(object sender, ElapsedEventArgs e) // 100ms interval
        {
            try
            {
                zkTimer1.Enabled = false; //(sender as System.Timers.Timer).Enabled = false;
                while (bIsConnected == true)
                {
                    bIsBusy = true;
                    if (axCZKEM1.ReadRTLog(iMachineNumber))
                    {
                        while (bIsConnected && axCZKEM1.GetRTLog(iMachineNumber))
                        {
                            ;
                        }
                    }
                    bIsBusy = false;
                    if (bIsDisconnecting) break;
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
