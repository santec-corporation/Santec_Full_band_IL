using Santec;
using Santec.STSProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Full_band_ILSTS_Sample_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private TSL[] TSL;                                                     // TSL control class
        private OSU OSU = new OSU();                                                   // OSU control class
        private MPM[] MPM;                                                     // MPM control class
        private SPU SPU = new SPU();                                                   // SPU control class
        private ILSTS[] Cal_STS;                                               // STS calucrate class
        private List<STSDataStruct> Data_struct = new List<STSDataStruct>();                        // STS data Struct 
        private List<STSDataStruct> Refdata_struct = new List<STSDataStruct>();                     // Reference data Struct
        private List<STSMonitorStruct> Meas_monitor_struct = new List<STSMonitorStruct>();             // Measurement monitor data struct
        private List<STSDataStruct> Ref_monitordata_struct = new List<STSDataStruct>();             // STS Monitor data Struct for Reference
        private List<STSDataStructForMerge> Mergedata_struct = new List<STSDataStructForMerge>();           // Data struct for merge  
        private List<int> Meas_rang = new List<int>();                                // Measurement Range
        private int[] TSL_OSUport;                                         // Combination of TSL and OSU port
        private bool Flag_213;                                              // Exist 213 flag      T: Exist F: nothing
        private bool Flag_215;                                              // Exist 215 flag      T: Exist F: nothing
        private bool Flag_100;                                              // Using OSU-100
        private double TSL_minspeed = -9999;                                   // TSL Min Sweep Speed(nm/sec)
        private double TSL_maxspeed = 9999;                     // TSL Max Sweep Speed(nm/sec)
        private double TSL_minpower = -9999;                                   // TSL Min APC Power(dBm)
        private double TSL_maxpower = 9999;                     // TSL Max APC Power(dBm)
        private double TSL_sweepstartwave;                                     // TSL Sweep Start Wavelength(nm)
        private double TSL_sweepstopwave;                                      // TSL Sweep Stop Wavelength(nm)
        private int Counter_570;                                           // TSL-570 Counter

        struct TSLSweepItem                                                   // Sweep items for each TSL
        {
            public double specminwave;                                         // TSL Spec Min Wavelenght(nm)
            public double specmaxwave;                                         // TSL Spec Max Wavelenght(nm)
            public double startwave;                                           // TSL Sweep Start Wavelength(nm)
            public double stopwave;                                            // TSL Sweep Stop Wavelength(nm)
            public double speed;                                               // TSL Sweep Speed(nm/sec)
            public float power;                                               // TSL APC Power(dBm)
            public double acctualstep;                                         // TSL output trigger step(nm)
            public double wavestep;                                            // STS wavelenthg step(nm)
        }


        private Dictionary<TSL, double> SPU_Sampling_timeDictionary = new Dictionary<TSL, double>();    // SPU sampling time Dictionary
        private Dictionary<TSL, string> TSL_ProductNameDictionary = new Dictionary<TSL, string>();      // TSL ProductName Dictionary
        private Dictionary<TSL, TSLSweepItem> TSL_SweepItemDictionary = new Dictionary<TSL, TSLSweepItem>();  // TSL_SweepItem Dictionary
        private void Show_Inst_Error(int errordata)
        {
            // ------------------------------------
            // Show error code
            // ------------------------------------
            string[] errorstring = Enum.GetNames(typeof(ExceptionCode));
            int[] errorvale = (int[])Enum.GetValues(typeof(ExceptionCode));
            int loop1;

            for (loop1 = 0; loop1 <= errorvale.GetUpperBound(0); loop1++)
            {
                if (errorvale[loop1] == errordata)
                {
                    MessageBox.Show(errorstring[loop1]);
                    break;
                }
            }
        }

        private void Show_STS_Error(int errordata)
        {
            // ------------------------------------
            // Show error code for STS
            // ------------------------------------
            string[] errorstring = Enum.GetNames(typeof(ErrorCode));
            int[] errorvale = (int[])Enum.GetValues(typeof(ErrorCode));
            int loop1;

            for (loop1 = 0; loop1 <= errorvale.GetUpperBound(0); loop1++)
            {
                if (errorvale[loop1] == errordata)
                {
                    MessageBox.Show(errorstring[loop1]);
                    break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // Form Load    (MainForm)
            // -------------------------------------------------------------------------
            string[] spudev = null;                        // SPU device name
            int errorcode;                                // errorcode 
            string[] usb_resource = null;                  // usb communication resource


            // ----Check Connction of spu deviece
            errorcode = SPU.Get_Device_ID(ref spudev);

            if (errorcode != 0)
            {
                Show_Inst_Error(errorcode);
                System.Environment.Exit(0);
            }


            // ----Check usb resource
            usb_resource = Santec.Communication.MainCommunication.Get_USB_Resouce();


            // ----show Setting Form
            Instrument_Setting set_form = new Instrument_Setting();

            set_form.Owner = this;
            set_form.SPU_Resource = spudev;
            set_form.USB_Resource = usb_resource;
            set_form.ShowDialog();

            // ----Apply to communication parametere from Instrument setting form
            Santec.Communication.CommunicationMethod[] tsl_communcation_method;
            Santec.Communication.CommunicationMethod osu_communcation_method=new Santec.Communication.CommunicationMethod();
            Santec.Communication.CommunicationMethod mpm_communcation_method;


            // ----TSL　Communication method

            TSL_OSUport = set_form.TSL_OSUport;

            int loop1;
            int tslcount;                     // TSL count

            tslcount = set_form.TSL_Count;
            TSL = new Santec.TSL[tslcount];
            tsl_communcation_method = new Santec.Communication.CommunicationMethod[tslcount];

            for (loop1 = 0; loop1 <= tslcount - 1; loop1++)
            {
                TSL[loop1] = new TSL();

                if (set_form.TSL_Communicater[loop1] == "GPIB")
                {
                    tsl_communcation_method[loop1] = Santec.Communication.CommunicationMethod.GPIB;
                    TSL[loop1].Terminator = CommunicationTerminator.CrLf;
                    TSL[loop1].GPIBAddress = Convert.ToInt32(set_form.TSL_Address[loop1]);
                    TSL[loop1].GPIBBoard = 0;
                    TSL[loop1].GPIBConnectType = Santec.Communication.GPIBConnectType.NI4882;
                }
                else if (set_form.TSL_Communicater[loop1] == "LAN")
                {
                    tsl_communcation_method[loop1] = Santec.Communication.CommunicationMethod.TCPIP;
                    TSL[loop1].Terminator = CommunicationTerminator.Cr;
                    TSL[loop1].IPAddress = set_form.TSL_Address[loop1];
                    TSL[loop1].Port = set_form.TSL_Portnumber[loop1];
                }
                else
                {
                    // USB 
                    tsl_communcation_method[loop1] = Santec.Communication.CommunicationMethod.USB;
                    TSL[loop1].DeviceID = System.Convert.ToUInt32(set_form.TSL_Address[loop1]);
                    TSL[loop1].Terminator = CommunicationTerminator.Cr;
                }
            }

            // ----OSU Communicatipon method

            // ----Using OSU-100
            if (set_form.Flag_OSU_100 == true)
                Flag_100 = true;

            if (Flag_100 == false)
            {
                // OSU-110
                if (set_form.OSU_Communicater == "GPIB")
                {
                    osu_communcation_method = Santec.Communication.CommunicationMethod.GPIB;
                    OSU.Terminator = CommunicationTerminator.Cr;
                    OSU.GPIBAddress = System.Convert.ToInt32(set_form.OSU_Address);
                }
                else if (set_form.OSU_Communicater == "LAN")
                {
                    osu_communcation_method = Santec.Communication.CommunicationMethod.TCPIP;
                    OSU.Terminator = CommunicationTerminator.Cr;
                    OSU.IPAddress = set_form.OSU_Address;
                    OSU.Port = set_form.OSU_Portnumber;
                }
                else
                {
                    // USB 
                    osu_communcation_method = Santec.Communication.CommunicationMethod.USB;
                    OSU.DeviceID = System.Convert.ToUInt32(set_form.OSU_Address);
                    OSU.Terminator = CommunicationTerminator.Cr;
                }
            }
            else
                // OSU-100
                OSU.DeviceName = set_form.OSU_DeviveID;


            // ----MPM Communicatipon method

            int mpmcount;                     // MPM count

            if (set_form.MPM_Communicater == "GPIB")
                mpm_communcation_method = Santec.Communication.CommunicationMethod.GPIB;
            else
                mpm_communcation_method = Santec.Communication.CommunicationMethod.TCPIP;

            mpmcount = set_form.MPM_Count;
            MPM = new MPM[mpmcount];
            for (loop1 = 0; loop1 <= mpmcount - 1; loop1++)
            {
                MPM[loop1] = new MPM();

                if (set_form.MPM_Communicater == "GPIB")
                    MPM[loop1].GPIBAddress = int.Parse(set_form.MPM_Address[loop1]);
                else
                {
                    MPM[loop1].IPAddress = set_form.MPM_Address[loop1];
                    MPM[loop1].Port = set_form.MPM_Portnumber[loop1];
                }

                // -------------------------------------------------------------------------
                // MPM muximum logging data read time is 11s
                // communication time out must to set > mpm logging data read time.
                // --------------------------------------------------------------------------
                MPM[loop1].TimeOut = 11000;
            }


            // ----SPU Communcation Setting 
            SPU.DeviceName = set_form.SPU_DeviveID;


            // ----Connect
            // TSL
            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                errorcode = TSL[loop1].Connect(tsl_communcation_method[loop1]);

                if (errorcode != 0)
                {
                    MessageBox.Show("TSL can't connect.Please check connection.");
                    System.Environment.Exit(0);
                }
            }


            // OSU
            if (Flag_100 == false)
                // OSU-110
                errorcode = OSU.Connect(osu_communcation_method);
            else
                // OSU-100
                errorcode = OSU.Connect();

            if (errorcode != 0)
            {
                MessageBox.Show("OSU can't connect.Please check connection.");
                System.Environment.Exit(0);
            }


            // MPM
            for (loop1 = 0; loop1 <=MPM.GetUpperBound(0); loop1++)
            {
                errorcode = MPM[loop1].Connect(mpm_communcation_method);

                if (errorcode != 0)
                {
                    MessageBox.Show("MPM can't connect.Please check connection.");
                    System.Environment.Exit(0);
                }
            }

            if (errorcode != 0)
            {
                MessageBox.Show("MPM can't connect.Please check connection.");
                System.Environment.Exit(0);
            }

            // SPU(DAQ)
            string ans = string.Empty;
            errorcode = SPU.Connect(ref ans);

            if (errorcode != 0)
            {
                MessageBox.Show("SPU Can't connect. Please check connection.");
                System.Environment.Exit(0);
            }


            // ----Check MPM Module information
            errorcode = Check_Module_Information();

            if (errorcode != 0)
            {
                MessageBox.Show("System can't use MPM-215 togeter other module");
                System.Environment.Exit(0);
            }

            // ----Reflect instrument parameter to Form
            Referect_EnableCh_for_form();                           // MPM Eanble ch
            Referect_EnableRange_for_form();                        // MPM selectable range

            errorcode = Add_TSL_SpecWavelength();                   // TSL Spec Wavelength

            if (errorcode != 0)
            {
                Show_Inst_Error(errorcode);
                return;
            }

            errorcode = Add_TSL_Sweep_Speed();                      // TSL Sweep speed(only TSL-570)

            if (errorcode != 0)
            {
                Show_Inst_Error(errorcode);
                return;
            }

            Get_TSL_Sweep_MinSpeed();                               // TSL Min Sweep speed

            errorcode = Get_TSL_APC_MaxPower(ref TSL_maxpower);         // TSL APC Max Power

            if (errorcode != 0)
            {
                Show_Inst_Error(errorcode);
                return;
            }

            Get_TSL_APC_MinPower();                                 // TSL APC Min Power

            // ----Check fiber connection between TSL and OSU
            //errorcode = Check_Fiber_Connection();

            //if (errorcode != 0)
            //{
            //    MessageBox.Show("Check the fiber connection between TSL and OSU");
            //    System.Environment.Exit(0);
            //}
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // TabControl SelectedIndexChanged
            // --------------------------------------------------------------------------
            int selectedIndex = TabControl1.SelectedIndex;

            if (selectedIndex == 0)
                groupbox_tsl.Enabled = true;
            else
                groupbox_tsl.Enabled = false;
        }
        private int Add_TSL_SpecWavelength()
        {
            // ---------------------------------------------------------
            // Add the spec wavelength to Dictionary
            // ----------------------------------------------------------
            int loop1;
            int inst_error;                       // instullment error
            double specminwave=0;                       // Spec WavelengthMin(nm)
            double specmaxwave=0;                       // Spec WavelengthMax(nm)
            double minwave = 9999;                    // WavelengthMin(nm)
            double maxwave = 0;                       // WavelengthMax(nm)
            string productname;                       // ProductName

            // Dictionary clear 
            TSL_ProductNameDictionary.Clear();               // TSL ProductName
            TSL_SweepItemDictionary.Clear();                 // TSL Sweepitem

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {             
                TSLSweepItem item = new TSLSweepItem();

                // Spec Wavelength
                inst_error = TSL[loop1].Get_Spec_Wavelength(ref specminwave, ref specmaxwave);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return inst_error;
                }

                // TSL ProductName
                productname = TSL[loop1].Information.ProductName;

                item.specminwave = specminwave;
                item.specmaxwave = specmaxwave;

                TSL_ProductNameDictionary.Add(TSL[loop1], productname);
                TSL_SweepItemDictionary.Add(TSL[loop1], item);

                if (minwave > specminwave)
                    minwave = specminwave;

                if (maxwave < specmaxwave)
                    maxwave = specmaxwave;

                switch (TSL_OSUport[loop1])
                {
                    case 1:
                        {
                            // port1
                             txtstartwave1.Enabled = true;
                             txtstopwave1.Enabled = true;
                             txtstartwave1.Text = specminwave.ToString();
                             txtstopwave1.Text = specmaxwave.ToString();
                             txtspecminwave1.Text = specminwave.ToString();
                             txtspecmaxwave1.Text = specmaxwave.ToString();
                            break;
                        }

                    case 2:
                        {
                            // port2
                             txtstartwave2.Enabled = true;
                             txtstopwave2.Enabled = true;
                             txtstartwave2.Text = specminwave.ToString();
                             txtstopwave2.Text = specmaxwave.ToString();
                             txtspecminwave2.Text = specminwave.ToString();
                             txtspecmaxwave2.Text = specmaxwave.ToString();
                            break;
                        }

                    case 3:
                        {
                            // port3
                             txtstartwave3.Enabled = true;
                             txtstopwave3.Enabled = true;
                             txtstartwave3.Text = specminwave.ToString();
                             txtstopwave3.Text = specmaxwave.ToString();
                             txtspecminwave3.Text = specminwave.ToString();
                             txtspecmaxwave3.Text = specmaxwave.ToString();
                            break;
                        }

                    case 4:
                        {
                            // port4
                             txtstartwave4.Enabled = true;
                             txtstopwave4.Enabled = true;
                             txtstartwave4.Text = specminwave.ToString();
                             txtstopwave4.Text = specmaxwave.ToString();
                             txtspecminwave4.Text = specminwave.ToString();
                             txtspecmaxwave4.Text = specmaxwave.ToString();
                            break;
                        }
                }
            }

             txtstartwave.Text = minwave.ToString();
             txtstopwave.Text = maxwave.ToString();

            TSL_sweepstartwave = minwave;
            TSL_sweepstopwave = maxwave;

            return 0;
        }

        private int Add_TSL_Sweep_Speed()
        {
            // ---------------------------------------------------------
            // Add in selectable sweep speed to speed combbox
            // this function can use only TSL-570
            // ----------------------------------------------------------
            int loop1;
            int inst_error;                       // instullment error
            double[] sweep_table = null;           // table
            double[] before_sweep_table = null;    // table
            double[] common_sweep_table = null;    // table
            string productname;                       // ProductName

            // get max sweep speed for spec wavelength
            inst_error = Get_TSL_Sweep_MaxSpeed(ref TSL_maxspeed);
            if (inst_error != 0)
                return inst_error;

            // Get Sweep speed tabele
            // Except for TSL-570 "Device Error" occurre when call this function.

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                productname = TSL_ProductNameDictionary[TSL[loop1]];

                if (productname.IndexOf("570") > -1)
                {
                    Counter_570 = Counter_570 + 1;

                    inst_error = TSL[loop1].Get_Sweep_Speed_table(ref sweep_table);
                    if (inst_error != 0)
                        return inst_error;

                    if (Counter_570 > 1)
                    {
                        // merging of table data
                        common_sweep_table = sweep_table.Intersect(before_sweep_table).ToArray();
                        before_sweep_table = common_sweep_table;
                    }
                    else
                        before_sweep_table = sweep_table;
                }
                else
                    continue;
            }

            if (Counter_570 == 1)
                common_sweep_table = before_sweep_table;

            // ----Add in combbox when TSL-570
            if (common_sweep_table == null == false)
            {
                for (loop1 = 0; loop1 <= common_sweep_table.GetUpperBound(0); loop1++)
                {
                    if (common_sweep_table[loop1] > TSL_maxspeed)
                    {
                        var oldCommon_sweep_table = common_sweep_table;
                        common_sweep_table = new double[loop1 - 1 + 1];
                        if (oldCommon_sweep_table != null)
                            Array.Copy(oldCommon_sweep_table, common_sweep_table, Math.Min(loop1 - 1 + 1, oldCommon_sweep_table.Length));
                        break;
                    }
                }

                for (loop1 = 0; loop1 <= common_sweep_table.GetUpperBound(0); loop1++)
                     cmbspeed.Items.Add(common_sweep_table[loop1]);
            }

            return 0;
        }

        private int Get_TSL_Sweep_MaxSpeed(ref double maxspeed)
        {
            // ---------------------------------------------------------
            // get the max sweep speed for TSL
            // ----------------------------------------------------------
            int loop1;
            int inst_error;           // instullment error
            double speed=0;                 // Sweep Speed(nm/sec)
            double specminwave;           // Spec WavelengthMin(nm)
            double specmaxwave;           // Spec WavelengthMax(nm)

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                specminwave = TSL[loop1].Information.MinimunWavelength;
                specmaxwave = TSL[loop1].Information.MaximumWavelength;

                // get the max sweep speed for Wavelength
                inst_error = TSL[loop1].Get_Sweep_Speed_for_Wavelength(specminwave, specmaxwave,ref speed);
                if (inst_error != 0)
                    return inst_error;

                if (maxspeed > speed)
                    maxspeed = speed;
            }

            return 0;
        }

        private void Get_TSL_Sweep_MinSpeed()
        {
            // ---------------------------------------------------------
            // get the min sweep speed for TSL
            // ----------------------------------------------------------
            int loop1;
            double speed;                 // Sweep Speed(nm/sec)

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                speed = TSL[loop1].Information.MinimumSpeed;

                if (TSL_minspeed < speed)
                    TSL_minspeed = speed;
            }
        }

        private int Get_TSL_APC_MaxPower(ref double maxpower)
        {
            // ---------------------------------------------------------
            // get the APC max power for TSL
            // ----------------------------------------------------------
            int loop1;
            int inst_error;           // instullment error
            string productname;           // ProductName
            float power=0;                 // APC Power(dBm)
            double specminwave;           // Spec WavelengthMin(nm)
            double specmaxwave;           // Spec WavelengthMax(nm)

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                productname = TSL_ProductNameDictionary[TSL[loop1]];

                if (productname.IndexOf("570") > -1)
                {
                    specminwave = TSL[loop1].Information.MinimunWavelength;
                    specmaxwave = TSL[loop1].Information.MaximumWavelength;

                    // get the APC max power for sweep
                    inst_error = TSL[loop1].Get_APC_Limit_for_Sweep(specminwave, specmaxwave,ref power);
                    if (inst_error != 0)
                        return inst_error;
                    else if (maxpower > power)
                        maxpower = power;
                }
            }

            if (Counter_570 == 0)
                maxpower = 10;

            txtpower.Text = Math.Floor(maxpower).ToString();

            return 0;
        }

        private void Get_TSL_APC_MinPower()
        {
            // ---------------------------------------------------------
            // get the APC min power for TSL
            // ----------------------------------------------------------
            int loop1;
            double power;                 // APC Power(dBm)

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                power = TSL[loop1].Information.MinimumAPCPower_dBm;

                if (TSL_minpower < power)
                    TSL_minpower = power;
            }
        }

        private int  Check_Fiber_Connection()
        {
            // ---------------------------------------------------------
            // check fiber connection between TSL and OSU
            // ----------------------------------------------------------
            int inst_error;                // instullment error
            int port=0;                      // active port
            Santec.TSL.LD_Status ldstatus=new Santec.TSL.LD_Status();     // LD status

            int loop1;

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                inst_error = TSL[loop1].Get_LD_Status(ref ldstatus);
                if (inst_error != 0)
                    return inst_error;

                if (ldstatus == Santec.TSL.LD_Status.LD_OFF)
                {
                    MessageBox.Show("Exit the application because the LD status is OFF");
                    System.Environment.Exit(0);
                }

                // close the shutters of all TSL
                inst_error = TSL[loop1].Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Close);
                if (inst_error != 0)
                    return inst_error;
            }

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {

                // open the shutter of the target TSL
                inst_error = TSL[loop1].Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Open);
                if (inst_error != 0)
                    return inst_error;

                // check the optical input port
                inst_error = OSU.Get_Active_Port(ref port);
                if (inst_error != 0)
                    return inst_error;

                if (port != TSL_OSUport[loop1])
                    return -1;

                // close the shutter of the target TSL
                inst_error = TSL[loop1].Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Close);
                if (inst_error != 0)
                    return inst_error;
            }

            // open the shutters of all TSL
            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                inst_error = TSL[loop1].Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Open);
                if (inst_error != 0)
                    return inst_error;
            }

            return 0;
        }

        private int Check_Module_Information()
        {
            // ------------------------------------------------------------
            // check Module information inside system
            // ------------------------------------------------------------
            int loop1;
            int loop2;
            int counter_215=0;                           // 215 counter 

            // MPM device loop
            for (loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                // Slot loop
                for (loop2 = 0; loop2 <= 4; loop2++)
                {
                    // Enable Slot
                    if (MPM[loop1].Information.ModuleEnable[loop2] == true)
                    {

                        // Check MPM-215 insert
                        if (MPM[loop1].Information.ModuleType[loop2] == "MPM-215")
                        {
                            Flag_215 = true;
                            counter_215 = counter_215 + 1;
                        }

                        // Check MPM-213 insert
                        if (MPM[loop1].Information.ModuleType[loop2] == "MPM-213")
                            Flag_213 = true;
                    }
                }
            }


            // Check MPM-215 count & Module total count
            // STS system can't use 215 together other module.
            int enable_module_count=0;                      // enable module count

            for (loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
                enable_module_count = MPM[loop1].Information.NumberofModule + enable_module_count;// total module count

            if (Flag_215 == true)
            {
                if (enable_module_count != counter_215)
                    return -1;
            }
            return 0;
        }
        private void Referect_EnableCh_for_form()
        {
            // ------------------------------------------------
            // Reflect mpm ch 
            // ------------------------------------------------
            int loop1;
            int loop2;
            bool[] enable_slot;
            string slot_type;

            for (loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                // get insert module count with "MPM Information" class 
                enable_slot = MPM[loop1].Information.ModuleEnable;

                // modeule loop(Maximum 5 slot)
                for (loop2 = 0; loop2 <= 4; loop2++)
                {
                    if (enable_slot[loop2] == true)
                    {
                        // get module type with "MPM Information" class 
                        slot_type = MPM[loop1].Information.ModuleType[loop2];

                        if (slot_type != "MPM-212")
                        {
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch1");
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch2");
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch3");
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch4");
                        }
                        else
                        {
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch1");
                             chklst_ch.Items.Add("MPM" + System.Convert.ToString(loop1 + 1) + " Slot" + System.Convert.ToString(loop2) + " Ch2");
                        }
                    }
                }
            }
        }
        private void Referect_EnableRange_for_form()
        {
            // -----------------------------------------------------
            // Refelect MPM Range
            // -------------------------------------------------------


            // MPM-213 can use just 1 to 4 range
            if (Flag_213 == true)
            {
                 chklst_range.Items.Add("Range1");
                 chklst_range.Items.Add("Range2");
                 chklst_range.Items.Add("Range3");
                 chklst_range.Items.Add("Range4");
            }
            else
            {
                 chklst_range.Items.Add("Range1");
                 chklst_range.Items.Add("Range2");
                 chklst_range.Items.Add("Range3");
                 chklst_range.Items.Add("Range4");
                 chklst_range.Items.Add("Range5");
            }

            // MPM-215 can't select range
            if (Flag_215 == true)
                 chklst_range.Enabled = false;
        }

        private void btntslsetcheck_Click(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // Check and set TSL Parameter
            // --------------------------------------------------------------------------
            TSLSweepItem item;                     // TSL SweepItem
            double specminwave;                    // Spec min wavelength(nm)
            double specmaxwave;                    // Spec max wavelength(nm)
            double startwave;                      // Startwavelength(nm)
            double stopwave;                       // Stopwavelength(nm)
            float set_pow;                        // Power(dBm)
            double speed;                          // Sweep Speed (nm/sec)
            double wavestep;                       // wavelenthg step(nm)
            double eachtsl_startwave=0;              // Startwavelength each TSL(nm)
            double eachtsl_stopwave=0;               // Stopwavelength each TSL(nm)

            if ( cmbspeed.Text == "")
            {
                MessageBox.Show("Please enter to the SweepSpeed");
                return;
            }

            // ----Sweep Setting Check
            startwave = double.Parse( txtstartwave.Text);
            stopwave = double.Parse( txtstopwave.Text);
            set_pow = float.Parse( txtpower.Text);
            speed = double.Parse( cmbspeed.Text);
            wavestep = double.Parse( txtstepwave.Text);

            // wavelength
            if ((startwave < TSL_sweepstartwave | stopwave > TSL_sweepstopwave))
            {
                Show_Inst_Error((int)ExceptionCode.ParameterError);
                return;
            }

            // set power
            if ((set_pow < TSL_minpower | set_pow > TSL_maxpower))
            {
                Show_Inst_Error((int)ExceptionCode.ParameterError);
                return;
            }

            // Sweep Speed
            if ((speed < TSL_minspeed | speed > TSL_maxspeed))
            {
                Show_Inst_Error((int)ExceptionCode.ParameterError);
                return;
            }

            // wavelenthg step
            if ((wavestep < 0.001))
            {
                Show_Inst_Error((int)ExceptionCode.ParameterError);
                return;
            }

            int loop1;

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                item = TSL_SweepItemDictionary[TSL[loop1]];

                specminwave = item.specminwave;
                specmaxwave = item.specmaxwave;

                switch (TSL_OSUport[loop1])
                {
                    case 1:
                        {
                            // port1
                            eachtsl_startwave = double.Parse( txtstartwave1.Text);
                            eachtsl_stopwave = double.Parse( txtstopwave1.Text);
                            break;
                        }

                    case 2:
                        {
                            // port2
                            eachtsl_startwave = double.Parse( txtstartwave2.Text);
                            eachtsl_stopwave = double.Parse( txtstopwave2.Text);
                            break;
                        }

                    case 3:
                        {
                            // port3
                            eachtsl_startwave = double.Parse( txtstartwave3.Text);
                            eachtsl_stopwave = double.Parse( txtstopwave3.Text);
                            break;
                        }

                    case 4:
                        {
                            // port4
                            eachtsl_startwave = double.Parse( txtstartwave4.Text);
                            eachtsl_stopwave = double.Parse( txtstopwave4.Text);
                            break;
                        }
                }

                // wavelength
                if ((eachtsl_startwave < specminwave | eachtsl_startwave > specmaxwave))
                {
                    Show_Inst_Error((int)ExceptionCode.ParameterError);
                    return;
                }

                if ((eachtsl_stopwave < specminwave | eachtsl_stopwave > specmaxwave))
                {
                    Show_Inst_Error((int)ExceptionCode.ParameterError);
                    return;
                }

                if ((eachtsl_startwave < startwave | eachtsl_startwave > stopwave))
                {
                    Show_Inst_Error((int)ExceptionCode.ParameterError);
                    return;
                }

                if ((eachtsl_stopwave < startwave | eachtsl_stopwave > stopwave))
                {
                    Show_Inst_Error((int)ExceptionCode.ParameterError);
                    return;
                }

                // wavelenthg step
                if ((wavestep > Math.Abs(eachtsl_startwave - eachtsl_stopwave)))
                {
                    Show_Inst_Error((int)ExceptionCode.ParameterError);
                    return;
                }
            }

            MessageBox.Show("Completed.");
        }

        private void btnset_Click(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // Set Parameter
            // --------------------------------------------------------------------------
            double startwave=0;             // Startwavelength(nm)
            double stopwave=0;              // Stopwavelength(nm)
            float set_pow;               // Power(dBm)
            double wavestep;              // wavelenthg step(nm)
            double speed;                 // Sweep Speed (nm/sec)
            int inst_error;           // instullment error
            int tslcount;             // TSL count
            TSLSweepItem item;            // TSL SweepItem

            // ----TSL Setting
            int loop1;

            tslcount = TSL.Count();
            Cal_STS = new ILSTS[tslcount];

            for (loop1 = 0; loop1 <= tslcount - 1; loop1++)

                Cal_STS[loop1] = new ILSTS();

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                switch (TSL_OSUport[loop1])
                {
                    case 1:
                        {
                            // port1
                            startwave = double.Parse( txtstartwave1.Text);
                            stopwave = double.Parse( txtstopwave1.Text);
                            break;
                        }

                    case 2:
                        {
                            // port2
                            startwave = double.Parse( txtstartwave2.Text);
                            stopwave = double.Parse( txtstopwave2.Text);
                            break;
                        }

                    case 3:
                        {
                            // port3
                            startwave = double.Parse( txtstartwave3.Text);
                            stopwave = double.Parse( txtstopwave3.Text);
                            break;
                        }

                    case 4:
                        {
                            // port4
                            startwave = double.Parse( txtstartwave4.Text);
                            stopwave = double.Parse( txtstopwave4.Text);
                            break;
                        }
                }

                wavestep = double.Parse( txtstepwave.Text);
                speed = double.Parse( cmbspeed.Text);
                set_pow = float.Parse( txtpower.Text);

                // set Power
                inst_error = TSL[loop1].Set_APC_Power_dBm(set_pow);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // busy check
                inst_error = TSL[loop1].TSL_Busy_Check(3000);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // OSU port select and range adjust must set to be after TSL Power set.
                // port select
                inst_error = OSU.Set_Switch(TSL_OSUport[loop1]);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // range adjust
                inst_error = OSU.Range_Adjust();
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }


                double tsl_acctualstep=0;          // TSL output trigger step(nm)

                // set Sweep parameter
                inst_error = TSL[loop1].Set_Sweep_Parameter_for_STS(startwave, stopwave, speed, wavestep,ref tsl_acctualstep);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // wavelength -> start wavelength
                inst_error = TSL[loop1].Set_Wavelength(startwave);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }


                // Add item to Dictionary
                item = TSL_SweepItemDictionary[TSL[loop1]];

                item.startwave = startwave;
                item.stopwave = stopwave;
                item.speed = speed;
                item.power = set_pow;
                item.wavestep = wavestep;
                item.acctualstep = tsl_acctualstep;
                TSL_SweepItemDictionary[TSL[loop1]] = item;

                // busy check
                inst_error = TSL[loop1].TSL_Busy_Check(3000);
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // ----MPM setting 
                int loop2;

                for (loop2 = 0; loop2 <= MPM.GetUpperBound(0); loop2++)
                {
                    // Sweep parameter setting 
                    inst_error = MPM[loop2].Set_Logging_Paremeter_for_STS(startwave, stopwave, wavestep, speed, Santec.MPM.Measurement_Mode.Freerun);
                    if (inst_error != 0)
                    {
                        Show_Inst_Error(inst_error);
                        return;
                    }
                }

                double averaging_time=0;

                inst_error = MPM[0].Get_Averaging_Time(ref averaging_time);

                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }


                // ----set STS Process parameter

                int sts_error;            // sts process class error

                // data Clear 
                sts_error = Cal_STS[loop1].Clear_Measdata();

                if (sts_error != 0)
                {
                    Show_STS_Error(sts_error);
                    return;
                }

                sts_error = Cal_STS[loop1].Clear_Refdata();

                if (sts_error != 0)
                {
                    Show_STS_Error(sts_error);
                    return;
                }

                // Setting for STS rescaling mode
                sts_error = Cal_STS[loop1].Set_Rescaling_Setting(RescalingMode.Freerun_SPU, averaging_time, true);

                if (sts_error != 0)
                {
                    Show_STS_Error(sts_error);
                    return;
                }

                // Make acctual wavelength step table
                sts_error = Cal_STS[loop1].Make_Sweep_Wavelength_Table(startwave, stopwave, tsl_acctualstep);

                if (sts_error != 0)
                {
                    Show_STS_Error(sts_error);
                    return;
                }

                // Make rescaling wavelength step table
                sts_error = Cal_STS[loop1].Make_Target_Wavelength_Table(startwave, stopwave, wavestep);

                if (sts_error != 0)
                {
                    Show_STS_Error(sts_error);
                    return;
                }
            }

            // ----prepare data struct & hold instrument condition
            if ( chkeach_ch.Checked)
                Prepare_dataST_Each();
            else
                Prepare_dataST();

            MessageBox.Show("Completed.");
        }
        private void Prepare_dataST()
        {
            // -----------------------------------------------
            // Prepar STS data struct
            // -----------------------------------------------
            int rangecout;                        // number of range
            int chcount;                          // number of ch 
            int loop1;                            // loop count1
            int loop2;                            // loop count2
            string text_st = string.Empty;            // text String 
            string[] split_st = null;              // split string array

            // List clear 
            Meas_rang.Clear();                               // Measurement Range
            Data_struct.Clear();                             // DataSturct of STS
            Refdata_struct.Clear();                          // Reference data struct
            Ref_monitordata_struct.Clear();                  // Reference monitor data struct
            Meas_monitor_struct.Clear();                     // Measurement monitor data struct



            Mergedata_struct.Clear();                        // DataStruct of Merge 

            rangecout =  chklst_range.CheckedItems.Count;
            chcount =  chklst_ch.CheckedItems.Count;

            if (Flag_215 == true)
                // If mom215 range must be set 1
                Meas_rang.Add(1);
            else
            {
                if (rangecout == 0 | chcount == 0)
                {
                    MessageBox.Show("Please check measurement parameters.");
                    return;
                }


                // hold range data 
                for (loop1 = 0; loop1 <=  chklst_range.Items.Count - 1; loop1++)
                {
                    if ( chklst_range.GetItemChecked(loop1) == true)
                        Meas_rang.Add(loop1 + 1);
                }
            }
            int device_number;
            int slot_number;
            int ch_number;
            STSDataStruct set_struct=new STSDataStruct();
            STSDataStructForMerge set_struct_merge=new STSDataStructForMerge();

            // --for measurement MPM data
            for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
            {
                for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
                {
                    if ( chklst_ch.GetItemChecked(loop1) == true)
                    {
                        text_st =  chklst_ch.Items[loop1].ToString();
                        split_st = text_st.Split(' ');
                        // ch parameter
                        device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;
                        slot_number = System.Convert.ToInt32(split_st[1].Substring(4));
                        ch_number = System.Convert.ToInt32(split_st[2].Substring(2));
                        // for data
                        set_struct.MPMNumber = device_number;
                        set_struct.SlotNumber = slot_number;
                        set_struct.ChannelNumber = ch_number;
                        set_struct.RangeNumber = Meas_rang[loop2];
                        set_struct.SweepCount = loop2 + 1;
                        set_struct.SOP = 0;
                        Data_struct.Add(set_struct);
                    }
                }
            }

            // ---for merasurement Monitor data
            // monitor data need each sweep times data
            STSMonitorStruct set_monitor_struct=new STSMonitorStruct();              // set struct for monitor
            for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
            {
                for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
                {
                    if ( chklst_ch.GetItemChecked(loop1) == true)
                    {
                        text_st =  chklst_ch.Items[loop1].ToString();
                        split_st = text_st.Split(' ');
                        // ch parameter
                        device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;

                        set_monitor_struct.MPMNumber = device_number;
                        set_monitor_struct.SOP = 0;
                        set_monitor_struct.SweepCount = loop2 + 1;

                        Meas_monitor_struct.Add(set_monitor_struct);
                        if (Meas_monitor_struct.Count == loop2 + 1)
                            break;
                    }
                }
            }

            // ---for　reference MPM data & merge data
            // reference data need only 1 range data
            for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
            {
                if ( chklst_ch.GetItemChecked(loop1) == true)
                {
                    text_st =  chklst_ch.Items[loop1].ToString();
                    split_st = text_st.Split(' ');
                    // ch parameter
                    device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;
                    slot_number = System.Convert.ToInt32(split_st[1].Substring(4));
                    ch_number = System.Convert.ToInt32(split_st[2].Substring(2));

                    // for reference data
                    set_struct.MPMNumber = device_number;
                    set_struct.SlotNumber = slot_number;
                    set_struct.ChannelNumber = ch_number;
                    set_struct.RangeNumber = 1;
                    set_struct.SweepCount = 1;
                    set_struct.SOP = 0;

                    Refdata_struct.Add(set_struct);

                    // for mege
                    set_struct_merge.MPMNumber = device_number;
                    set_struct_merge.SlotNumber = slot_number;
                    set_struct_merge.ChannelNumber = ch_number;
                    set_struct_merge.SOP = 0;
                    Mergedata_struct.Add(set_struct_merge);
                }
            }

            // ----for referece Monitor data 
            var set_ref_monitor_struct = new STSDataStruct();
            for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
            {
                if ( chklst_ch.GetItemChecked(loop1) == true)
                {
                    text_st =  chklst_ch.Items[loop1].ToString();
                    split_st = text_st.Split(' ');
                    // Mainframe parameter
                    device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;
                    slot_number = System.Convert.ToInt32(split_st[1].Substring(4));
                    ch_number = System.Convert.ToInt32(split_st[2].Substring(2));

                    // for reference monitor data
                    set_ref_monitor_struct.MPMNumber = device_number;
                    set_ref_monitor_struct.SlotNumber = slot_number;
                    set_ref_monitor_struct.ChannelNumber = ch_number;
                    set_ref_monitor_struct.RangeNumber = 1;
                    set_ref_monitor_struct.SOP = 0;
                    set_ref_monitor_struct.SweepCount = 1;

                    Ref_monitordata_struct.Add(set_ref_monitor_struct);
                    break;
                }
            }
        }

        private void Prepare_dataST_Each()
        {
            // -----------------------------------------------
            // Prepar STS data struct
            // -----------------------------------------------
            int rangecout;                        // number of range
            int chcount;                          // number of ch 
            int loop1;                            // loop count1
            int loop2;                            // loop count2
            string text_st = string.Empty;            // text String 
            string[] split_st = null;              // split string array

            // List clear 
            Meas_rang.Clear();                               // Measurement Range
            Data_struct.Clear();                             // DataSturct of STS
            Refdata_struct.Clear();                          // Reference data struct
            Ref_monitordata_struct.Clear();                  // Reference monitor data struct
            Meas_monitor_struct.Clear();                     // Measurement monitor data struct



            Mergedata_struct.Clear();                        // DataStruct of Merge 

            rangecout =  chklst_range.CheckedItems.Count;
            chcount =  chklst_ch.CheckedItems.Count;

            if (Flag_215 == true)
                // If mom215 range must be set 1
                Meas_rang.Add(1);
            else
            {
                if (rangecout == 0 | chcount == 0)
                {
                    MessageBox.Show("Please check measurement parameters.");
                    return;
                }


                // hold range data 
                for (loop1 = 0; loop1 <=  chklst_range.Items.Count - 1; loop1++)
                {
                    if ( chklst_range.GetItemChecked(loop1) == true)
                        Meas_rang.Add(loop1 + 1);
                }
            }
            int device_number;
            int slot_number;
            int ch_number;
            STSDataStruct set_struct=new STSDataStruct();
            STSDataStructForMerge set_struct_merge=new STSDataStructForMerge();

            // --for measurement MPM data
            for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
            {
                for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
                {
                    if ( chklst_ch.GetItemChecked(loop1) == true)
                    {
                        text_st =  chklst_ch.Items[loop1].ToString();
                        split_st = text_st.Split(' ');
                        // ch parameter
                        device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;
                        slot_number = System.Convert.ToInt32(split_st[1].Substring(4));
                        ch_number = System.Convert.ToInt32(split_st[2].Substring(2));
                        // for data
                        set_struct.MPMNumber = device_number;
                        set_struct.SlotNumber = slot_number;
                        set_struct.ChannelNumber = ch_number;
                        set_struct.RangeNumber = Meas_rang[loop2];
                        set_struct.SweepCount = loop2 + 1;
                        set_struct.SOP = 0;
                        Data_struct.Add(set_struct);
                    }
                }
            }

            // ---for merasurement Monitor data
            // monitor data need each sweep times data
            STSMonitorStruct set_monitor_struct=new STSMonitorStruct();              // set struct for monitor
            for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
            {
                for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
                {
                    if ( chklst_ch.GetItemChecked(loop1) == true)
                    {
                        text_st =  chklst_ch.Items[loop1].ToString();
                        split_st = text_st.Split(' ');
                        // ch parameter
                        device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;

                        set_monitor_struct.MPMNumber = device_number;
                        set_monitor_struct.SOP = 0;
                        set_monitor_struct.SweepCount = loop2 + 1;

                        Meas_monitor_struct.Add(set_monitor_struct);
                        if (Meas_monitor_struct.Count == loop2 + 1)
                            break;
                    }
                }
            }

            // ---for　reference MPM data & reference monitor data & merge data
            // reference data need only 1 range data
            set_struct = new STSDataStruct();
            STSDataStruct set_ref_monitor_struct=new STSDataStruct();
            for (loop1 = 0; loop1 <=  chklst_ch.Items.Count - 1; loop1++)
            {
                if ( chklst_ch.GetItemChecked(loop1) == true)
                {
                    text_st =  chklst_ch.Items[loop1].ToString();
                    split_st = text_st.Split(' ');
                    // ch parameter
                    device_number = System.Convert.ToInt32(split_st[0].Substring(3)) - 1;
                    slot_number = System.Convert.ToInt32(split_st[1].Substring(4));
                    ch_number = System.Convert.ToInt32(split_st[2].Substring(2));

                    // for reference data
                    set_struct.MPMNumber = device_number;
                    set_struct.SlotNumber = slot_number;
                    set_struct.ChannelNumber = ch_number;
                    set_struct.RangeNumber = 1;
                    set_struct.SweepCount = 1;
                    set_struct.SOP = 0;

                    Refdata_struct.Add(set_struct);


                    // for reference monitor data
                    set_ref_monitor_struct.MPMNumber = device_number;
                    set_ref_monitor_struct.SlotNumber = slot_number;
                    set_ref_monitor_struct.ChannelNumber = ch_number;
                    set_ref_monitor_struct.RangeNumber = 1;
                    set_ref_monitor_struct.SweepCount = 1;
                    set_ref_monitor_struct.SOP = 0;

                    Ref_monitordata_struct.Add(set_ref_monitor_struct);

                    // for mege
                    set_struct_merge.MPMNumber = device_number;
                    set_struct_merge.SlotNumber = slot_number;
                    set_struct_merge.ChannelNumber = ch_number;
                    set_struct_merge.SOP = 0;
                    Mergedata_struct.Add(set_struct_merge);
                }
            }
        }

        private void btnget_reference_Click(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------------------
            // Get Reference
            // ------------------------------------------------------------------------------
            int inst_error;                       // Instullment error
            bool inst_flag=new bool();
            int loop1;

            SPU_Sampling_timeDictionary.Clear();

            // ----MPM setting for selected 1st range

            // set Range for MPM
            for (loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                inst_error = MPM[loop1].Set_Range(Meas_rang[0]);
                if ((inst_error != 0))
                {
                    Show_Inst_Error(inst_error);
                    return;
                }
            }

            if ( chkeach_ch.Checked)
                // Reference measurement one channel at a time
                inst_error = Each_channel_reference(ref inst_flag);
            else
                inst_error = All_channel_reference(ref inst_flag);

            if (inst_error != 0)
            {
                if (inst_error == -9999)
                    MessageBox.Show("MPM Trigger receive error! Please check trigger cable connection.");
                else if (inst_flag == true)
                    Show_Inst_Error(inst_error);             // Instullment error
                else
                    Show_STS_Error(inst_error);// Processing error
                return;
            }

            MessageBox.Show("Completed.");
        }
        private int All_channel_reference(ref bool inst_flag)
        {

            // ------------------------------------------------------------------------------
            // Get Reference
            // ------------------------------------------------------------------------------
            int inst_error;                       // Instullment error
            int loop1;

             toolstatus.Text = "";
             toolmessage.Text = "";

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                inst_error = Set_Sweep_Parameter(false, loop1);
                if (inst_error != 0)
                    Show_Inst_Error(inst_error);

                // ----Move to start wavelength with Sweep Start method.
                inst_error = TSL[loop1].Sweep_Start();
                if (inst_error != 0)
                    Show_Inst_Error(inst_error);

                 toolstatus.Text = "Reference...";
                 toolmessage.Text = "OSU Port" + TSL_OSUport[loop1];
                 Refresh();

                // Sweep 
                inst_error = Sweep_Process(loop1);


                if (inst_error != 0)
                {
                    inst_flag = true;
                    return inst_error;
                }

                // Move to start wavelength  with Sweep Start method.
                inst_error = TSL[loop1].Sweep_Start();

                if (inst_error != 0)
                {
                    inst_flag = true;
                    return inst_error;
                }

                // get logging data & add in STSProcess class
                inst_error = Get_reference_samplingdata(ref inst_flag, loop1);

                if (inst_error != 0)
                    return inst_error;

                // ------Reference data rescaling 
                int process_error;
                process_error = Cal_STS[loop1].Cal_RefData_Rescaling();

                if (process_error != 0)
                {
                    inst_flag = false;
                    return process_error;
                }

                // TSL Sweep Stop
                inst_error = TSL[loop1].Sweep_Stop();

                if (inst_error != 0)
                    return inst_error;
            }

             toolstatus.Text = "";
             toolmessage.Text = "";
            return 0;
        }

        private int Each_channel_reference(ref bool inst_flag)
        {

            // ------------------------------------------------------------------------------
            // Get Reference
            // ------------------------------------------------------------------------------
            int inst_error;                       // Instullment error
            int loop1;

            foreach (STSDataStruct item in Refdata_struct)
            {
                 toolstatus.Text = "";
                 toolmessage.Text = "";

                MessageBox.Show("Connect fiber to MPM" + item.MPMNumber + "_Slot" + item.SlotNumber + "_Ch" + item.ChannelNumber + ".");

                for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
                {
                    inst_error = Set_Sweep_Parameter(false, loop1);
                    if (inst_error != 0)
                        Show_Inst_Error(inst_error);

                    // ----Move to start wavelength with Sweep Start method.
                    inst_error = TSL[loop1].Sweep_Start();
                    if (inst_error != 0)
                        Show_Inst_Error(inst_error);

                     toolstatus.Text = "Reference...";
                     toolmessage.Text = "OSU Port" + TSL_OSUport[loop1] + "   MPM" + item.MPMNumber + 1 + " Slot" + item.SlotNumber + " Ch" + item.ChannelNumber;
                     Refresh();

                    // Sweep 
                    inst_error = Sweep_Process(loop1);

                    if (inst_error != 0)
                    {
                        inst_flag = true;
                        return inst_error;
                    }

                    // Move to start wavelength  with Sweep Start method.
                    inst_error = TSL[loop1].Sweep_Start();
                    if (inst_error != 0)
                    {
                        inst_flag = true;
                        return inst_error;
                    }


                    // get logging data & add in STSProcess class
                    inst_error = Get_Each_channel_reference_samplingdata(ref inst_flag, loop1, item.MPMNumber, item.SlotNumber, item.ChannelNumber, item.SweepCount);

                    if (inst_error != 0)
                        return inst_error;

                    // TSL Sweep Stop
                    inst_error = TSL[loop1].Sweep_Stop();

                    if (inst_error != 0)
                    {
                        Show_Inst_Error(inst_error);
                        return inst_error;
                    }

                    // ------Reference data rescaling 
                    int process_error;
                    process_error = Cal_STS[loop1].Cal_RefData_Rescaling();

                    if (process_error != 0)
                    {
                        inst_flag = false;
                        return process_error;
                    }
                }
            }

             toolstatus.Text = "";
             toolmessage.Text = "";
            return 0;
        }
        private int Set_Sweep_Parameter(bool Flag_meas, int deveice)
        {
            int inst_error;           // Instullment error
            double startwave;             // Startwavelength(nm)
            double stopwave;              // Stopwavelength(nm)
            double wavestep;              // wavelenthg step(nm)
            double tsl_acctualstep;       // TSL output trigger step(nm)
            double speed;                 // Sweep Speed (nm/sec)
            TSLSweepItem item;            // TSL SweepItem


            item = TSL_SweepItemDictionary[TSL[deveice]];

            startwave = item.startwave;
            stopwave = item.stopwave;
            speed = item.speed;
            wavestep = item.wavestep;
            tsl_acctualstep = item.acctualstep;

            // ----OSU setting
            // optical switch setting
            inst_error = OSU.Set_Switch(TSL_OSUport[deveice]);
            if (inst_error != 0)
                return inst_error;

            // ----TSL setting
            // set Sweep parameter
            inst_error = TSL[deveice].Set_Sweep_Parameter_for_STS(startwave, stopwave, speed, wavestep,ref tsl_acctualstep);
            if (inst_error != 0)
                return inst_error;

            // ----MPM setting 
            // Sweep parameter setting 
            for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                // Sweep parameter setting 
                inst_error = MPM[loop1].Set_Logging_Paremeter_for_STS(startwave, stopwave, wavestep, speed, Santec.MPM.Measurement_Mode.Freerun);
                if (inst_error != 0)
                    return inst_error;
            }

            // ----SPU setting
            double averaging_time=0;

            inst_error = MPM[0].Get_Averaging_Time(ref averaging_time);

            if (inst_error != 0)
                return inst_error;

            // parameter setting
            if (Flag_meas)
            {
                // measurement
                if (SPU_Sampling_timeDictionary.ContainsKey(TSL[deveice]) == true)
                {
                    SPU.Meas_Sampling_time = SPU_Sampling_timeDictionary[TSL[deveice]];
                    inst_error = SPU.Set_Sampling_Parameter_for_Measure(startwave, stopwave, speed, tsl_acctualstep);
                }
                else
                    // Initial measurement when using Read Reference Data function
                    inst_error = SPU.Set_Sampling_Parameter(startwave, stopwave, speed, tsl_acctualstep);
            }
            else
                // reference
                inst_error = SPU.Set_Sampling_Parameter(startwave, stopwave, speed, tsl_acctualstep);


            if (inst_error != 0)
                return inst_error;

            // mpm averageing time-> spu
            SPU.AveragingTime = averaging_time;

            return 0;
        }

        private int Sweep_Process(int deveice)
        {
            // ------------------------------------------------------------
            // Sweep Process
            // ------------------------------------------------------------
            int inst_error;               // Instullment error

            // MPM sampling start 
            for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                inst_error = MPM[loop1].Logging_Start();
                if (inst_error != 0)
                    return inst_error;
            }

            // TSL waiting for start status 
            inst_error = TSL[deveice].Waiting_For_Sweep_Status(3000, Santec.TSL.Sweep_Status.WaitingforTrigger);

            // ----error handling -> MPM Logging Stop
            if (inst_error != 0)
            {
                for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
                    MPM[loop1].Logging_Stop();

                return inst_error;
            }

            // SPU sampling start
            inst_error = SPU.Sampling_Start();
            if (inst_error != 0)
                return inst_error;

            // TSL issue software trigger
            inst_error = TSL[deveice].Set_Software_Trigger();

            // ----error handling -> MPM Logging Stop
            if (inst_error != 0)
            {
                for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
                    MPM[loop1].Logging_Stop();

                return inst_error;
            }

            // SPU waiting for sampling 
            inst_error = SPU.Waiting_for_sampling();

            // ----error handling -> MPM Logging Stop
            if (inst_error != 0)
            {
                for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
                    MPM[loop1].Logging_Stop();

                return inst_error;
            }

            int mpm_stauts=0;                   // mpm logging status 0:douring measurement 1:Compleated -1:Stopped
            int mpm_count=0;                    // mpm number of logging completed point
            double timeout = 2000;                // MPM Logging Status timeout(2000msec) after the SPU sampling completed.
            Stopwatch st = new Stopwatch();                     // stopwatch           
            bool mpm_completed_falg = true;    // mpm logging completed flag  F:not completed T:completed
            bool mpm_complet_flag = true;

            // MPM waiting for sampling 
            st.Start();                                  // stopwathc start 

            do
            {
                for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
                {
                    inst_error = MPM[loop1].Get_Logging_Status(ref mpm_stauts, ref mpm_count);
                    if (inst_error != 0)
                        return inst_error;
                    if (mpm_stauts == 1)
                        break;
                }

                if (st.ElapsedMilliseconds >= timeout)
                {
                    mpm_complet_flag = false;
                    break;
                }
            }
            while (true);

            st.Stop();

            // MPM sampling stop
            for (var loop1 = 0; loop1 <= MPM.GetUpperBound(0); loop1++)
            {
                inst_error = MPM[loop1].Logging_Stop();
                if (inst_error != 0)
                    return inst_error;
            }


            // TSL Waiting for standby
            inst_error = TSL[deveice].Waiting_For_Sweep_Status(5000, Santec.TSL.Sweep_Status.Standby);

            if (inst_error != 0)
                return inst_error;

            if (mpm_completed_falg == false)
                // mpm logging timeout occurred.
                return -9999;

            return 0;
        }

        private int Get_reference_samplingdata(ref bool inst_flag, int deveice)
        {
            // ---------------------------------------------------------------
            // Get logging reference data & add in 
            // ---------------------------------------------------------------
            int inst_error;                        // Instullment error
            float[] logg_data = null;              // MPM Logging data
            int cal_error;                         // process error
            double sampling_time;

            // ----Load　Reference MPM data & add in data for STS Process Class for each channel
            foreach (STSDataStruct item in Refdata_struct)
            {

                // Read corresponded MPM data
                inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber, ref logg_data);

                if (inst_error != 0)
                {
                    inst_flag = true;
                    return inst_error;
                }

                // Add in to MPM reference data to STS Process Class
                cal_error = Cal_STS[deveice].Add_Ref_MPMData_CH(logg_data, item);

                if (cal_error != 0)
                {
                    inst_flag = false;
                    return cal_error;
                }
            }

            // ----Load Monitor data & add in data for STS Proccsess class with "STS Data Struct"
            float[] triggerdata = null;     // tigger data 
            float[] monitordata = null;     // monitor data

            inst_error = SPU.Get_Sampling_Rawdata(ref triggerdata, ref monitordata);

            if (inst_error != 0)
            {
                inst_flag = true;
                return inst_error;
            }

            if (SPU_Sampling_timeDictionary.ContainsKey(TSL[deveice]) == false)
            {
                sampling_time = SPU.Meas_Sampling_time;
                SPU_Sampling_timeDictionary.Add(TSL[deveice], sampling_time);
            }

            foreach (STSDataStruct monitor_item in Ref_monitordata_struct)
            {
                cal_error = Cal_STS[deveice].Add_Ref_MonitorData(triggerdata, monitordata, monitor_item);

                if (cal_error != 0)
                {
                    inst_flag = false;
                    return cal_error;
                }
            }

            return 0;
        }

        private int Get_Each_channel_reference_samplingdata(ref bool inst_flag, int deveice, int currentMPMNumber, int currentSlotNumber, int currentChannelNumber, int currentSweepCount)
        {
            // ---------------------------------------------------------------
            // Get logging reference data & add in 
            // ---------------------------------------------------------------
            int inst_error;                        // Instullment error
            float[] logg_data = null;              // MPM Logging data
            int cal_error;                         // process error


            // ----Load　Reference MPM data & add in data for STS Process Class for each channel
            foreach (STSDataStruct item in Refdata_struct)
            {
                if ((item.MPMNumber != currentMPMNumber | item.SlotNumber != currentSlotNumber | item.ChannelNumber != currentChannelNumber))
                    continue;

                // Read corresponded MPM data
                inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber, ref logg_data);

                if (inst_error != 0)
                {
                    inst_flag = true;
                    return inst_error;
                }

                // Add in to MPM reference data to STS Process Class
                cal_error = Cal_STS[deveice].Add_Ref_MPMData_CH(logg_data, item);

                if (cal_error != 0)
                {
                    inst_flag = false;
                    return cal_error;
                }
            }

            // ------Load Monitor data & add in data for STS Proccsess class with "STS Data Struct"
            float[] triggerdata = null;     // tigger data 
            float[] monitordata = null;     // monitor data

            inst_error = SPU.Get_Sampling_Rawdata(ref triggerdata,ref monitordata);

            if (inst_error != 0)
            {
                inst_flag = true;
                return inst_error;
            }

            foreach (STSDataStruct monitor_item in Ref_monitordata_struct)
            {
                cal_error = Cal_STS[deveice].Add_Ref_MonitorData(triggerdata, monitordata, monitor_item);
                if (cal_error != 0)
                {
                    inst_flag = false;
                    return cal_error;
                }
            }

            return 0;
        }

        private int Get_MPM_Loggdata(int deveice, int slot, int ch, ref float[] data)
        {
            // ---------------------------------------------------------------
            // Get MPM Logg data
            // --------------------------------------------------------------
            int inst_error;

            inst_error = MPM[deveice].Get_Each_Channel_Logdata(slot, ch, ref data);
            return inst_error;
        }

        private void btnmeas_Click(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // Mesurement Process
            // -------------------------------------------------------------------------
            int loop1;                        // loop Count 1
            int loop2;                        // loop count 2
            int loop3;                        // loop count 3
            int inst_error;                   // instllment error   
            bool inst_flag=new bool();                    // instrment error flag
            int process_error;                // STS　Process error


             toolstatus.Text = "";
             toolmessage.Text = "";

            // -------Measurement-----------------------------------------------

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                inst_error = Set_Sweep_Parameter(true, loop1);
                if (inst_error != 0)
                    Show_Inst_Error(inst_error);

                // Move to start wavelength  with Sweep Start method.
                inst_error = TSL[loop1].Sweep_Start();
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }

                // ----Rang Loop
                for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
                {

                    // MPM range Setting 
                    for (loop3 = 0; loop3 <= MPM.GetUpperBound(0); loop3++)
                    {
                        inst_error = MPM[loop3].Set_Range(Meas_rang[loop2]);

                        if (inst_error != 0)
                        {
                            Show_Inst_Error(inst_error);
                            return;
                        }
                    }

                     toolstatus.Text = "Measurement...";
                     toolmessage.Text = "OSU Port" + TSL_OSUport[loop1] + "  Range" + Meas_rang[loop2];
                     Refresh();

                    // Sweep process
                    inst_error = Sweep_Process(loop1);
                    if (inst_error != 0)
                    {
                        Show_Inst_Error(inst_error);
                        return;
                    }

                    // Move to start wavelength  with Sweep Start method for next sweep.
                    inst_error = TSL[loop1].Sweep_Start();
                    if (inst_error != 0)
                    {
                        Show_Inst_Error(inst_error);
                        return;
                    }

                    // get loggging data & Add in STS Process class
                    inst_error = Get_measurement_samplingdata(loop1, loop2 + 1, ref inst_flag);

                    if (inst_error != 0)
                    {
                        if (inst_flag == true)
                            Show_Inst_Error(inst_error);
                        else
                            Show_STS_Error(inst_error);

                        return;
                    }
                }

                // ----STS Process

                // Rescaling
                process_error = Cal_STS[loop1].Cal_MeasData_Rescaling();

                if (process_error != 0)
                {
                    Show_STS_Error(process_error);
                    return;
                }

                // merge or IL calculate
                Module_Type merge_type;

                if (Flag_215 == false)
                {
                    if (Flag_213 == true)
                        merge_type = Module_Type.MPM_213;
                    else
                        merge_type = Module_Type.MPM_211;

                    // Process ranges merge
                    process_error = Cal_STS[loop1].Cal_IL_Merge(merge_type);
                }
                else
                    // just IL process
                    process_error = Cal_STS[loop1].Cal_IL();

                inst_error = TSL[loop1].Sweep_Stop();
                if (inst_error != 0)
                {
                    Show_Inst_Error(inst_error);
                    return;
                }
            }

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                // data save
                process_error = Save_Measurement_data(loop1);
                if (process_error != 0)
                    Show_STS_Error(process_error);
            }

             toolstatus.Text = "";
             toolmessage.Text = "";
            MessageBox.Show("Completed.");
        }
        private int Get_measurement_samplingdata(int deveice, int sweepcount, ref bool inst_flag)
        {
            // ---------------------------------------------------------------
            // Get logging measurement data & add in 
            // ---------------------------------------------------------------
            int inst_error;                        // Instullment error
            float[] logg_data = null;              // MPM Logging data
            int cal_error;                         // process error
            double sampling_time;


            // ----Load MPM Logging data & Add in STS Process class with measurment sts data struct 
            foreach (STSDataStruct item in Data_struct)
            {
                if (item.SweepCount != sweepcount)
                    continue;

                // Read corresponded MPM data
                inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber,ref logg_data);

                if (inst_error != 0)
                {
                    inst_flag = true;
                    return inst_error;
                }

                // Add in to MPM reference data to STS Process Class
                cal_error = Cal_STS[deveice].Add_Meas_MPMData_CH(logg_data, item);

                if (cal_error != 0)
                {
                    inst_flag = false;
                    return cal_error;
                }
            }

            // ----Lado SPU monitor data & Add in STS Process class with measurement monitor data struct
            float[] triggerdata = null;
            float[] monitordata = null;

            inst_error = SPU.Get_Sampling_Rawdata(ref triggerdata, ref monitordata);

            if (inst_error != 0)
            {
                inst_flag = true;
                return inst_error;
            }

            // Initial measurement when using Read Reference Data function
            if (SPU_Sampling_timeDictionary.ContainsKey(TSL[deveice]) == false)
            {
                sampling_time = SPU.Meas_Sampling_time;
                SPU_Sampling_timeDictionary.Add(TSL[deveice], sampling_time);
            }


            // ----Search item from measurement monitor data structure according to sweep count.
            foreach (STSMonitorStruct item in Meas_monitor_struct)
            {
                if (item.SweepCount == sweepcount)
                {
                    cal_error = Cal_STS[deveice].Add_Meas_MonitorData(triggerdata, monitordata, item);

                    if (cal_error != 0)
                    {
                        inst_flag = false;
                        return cal_error;
                    }
                    break;
                }
            }
            return 0;
        }

        private int Save_Measurement_data(int deveice)
        {
            // -------------------------------------------------------
            // Save Measurement data
            // -------------------------------------------------------
            double[] wavelength_table = null;            // Rescaled wavelength table  
            List<float[]> lstILdata = new List<float[]>();                // IL data list 
            int process_error;                          // process class error  
            int loop1;                                  // loop count
            float[] ildata = null;                      // il data arrray

            // Get Rescaled wavelength tabel 
            process_error = Cal_STS[deveice].Get_Target_Wavelength_Table(ref wavelength_table);


            // Get IL data 
            if (Flag_215 == true)
            {
                foreach (STSDataStruct items in Data_struct)
                {
                    process_error = Cal_STS[deveice].Get_IL_Data(ref ildata, items);
                    if (process_error != 0)
                        return process_error;

                    lstILdata.Add(ildata.ToArray());
                }
            }
            else
                foreach (STSDataStructForMerge items in Mergedata_struct)
                {
                    process_error = Cal_STS[deveice].Get_IL_Merge_Data(ref ildata, items);
                    if (process_error != 0)
                        return process_error;

                    lstILdata.Add(ildata.ToArray());
                }


            // ----Data Save 
            string file_path = string.Empty;

             SaveFileDialog1.Title = "port:" + TSL_OSUport[deveice] + " IL data save";
             SaveFileDialog1.Filter = "csv file(*.csv)|*.csv";
             SaveFileDialog1.ShowDialog();

            file_path =  SaveFileDialog1.FileName;

            System.IO.StreamWriter writer = new System.IO.StreamWriter(file_path, false, System.Text.Encoding.GetEncoding("UTF-8"));
            string write_string = null;

            string hedder = string.Empty;                 // file hedder 

            hedder = "Wavelength(nm)";

            foreach (STSDataStruct item in Data_struct)
            {
                if (item.SweepCount != 1)
                    continue;

                hedder = hedder + ",MPM" + System.Convert.ToString(item.MPMNumber + 1) + "Slot" + System.Convert.ToString(item.SlotNumber) + "Ch" + System.Convert.ToString(item.ChannelNumber);
            }

            // write hedder
            writer.WriteLine(hedder);

            int loop2;

            for (loop1 = 0; loop1 <= wavelength_table.GetUpperBound(0); loop1++)
            {
                write_string = wavelength_table[loop1].ToString();

                for (loop2 = 0; loop2 <= lstILdata.Count - 1; loop2++)
                    write_string = write_string + "," + lstILdata[loop2][loop1];

                writer.WriteLine(write_string);
            }

            writer.Close();

            return 0;
        }

        private void btnsaveref_rawdata_Click(object sender, EventArgs e)
        {
            // ---------------------------------------------------------------------------
            // Save reference Raw data
            // ---------------------------------------------------------------------------
            int loop1;                        // loop count1   
            int process_error;                // process class error
            double[] wavetable = null;         // wavelength table
            float[] powdata = null;           // powerdata  rescaled    
            float[] monitordata = null;       // monitordata rescaled 
            List<float[]> lstpowdata = new List<float[]>();     // Power data list
            List<float[]> lstmonitordata = new List<float[]>(); // monitor data list 

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                lstpowdata = new List<float[]>();
                lstmonitordata = new List<float[]>();

                // Get reference Raw power data (after the rescaling)
                foreach (STSDataStruct item in Refdata_struct)
                {
                    process_error = Cal_STS[loop1].Get_Ref_Power_Rawdata(item, ref powdata);
                    if (process_error != 0)
                    {
                        Show_STS_Error(process_error);
                        return;
                    }

                    lstpowdata.Add(powdata.ToArray());
                }

                // Get reference Raw monitor data
                STSDataStruct get_struct=new STSDataStruct();                 // struct of get
                STSDataStruct befor_struct = new STSDataStruct();           // befor struct

                foreach (STSDataStruct item in Ref_monitordata_struct)
                {
                    if ( chkeach_ch.Checked)
                    {
                        // Reference measurement one channel at a time
                        if ((item.MPMNumber == befor_struct.MPMNumber) & (item.SlotNumber == befor_struct.SlotNumber) & (item.ChannelNumber == befor_struct.ChannelNumber))
                            continue;
                    }

                    process_error = Cal_STS[loop1].Get_Ref_Monitor_Rawdata(item,ref monitordata);

                    if (process_error != 0)
                    {
                        Show_STS_Error(process_error);
                        return;
                    }

                    get_struct.MPMNumber = item.MPMNumber;
                    get_struct.SlotNumber = item.SlotNumber;
                    get_struct.ChannelNumber = item.ChannelNumber;

                    lstmonitordata.Add(monitordata.ToArray());
                    befor_struct = get_struct;
                }


                // Get Target wavelengt table
                process_error = Cal_STS[loop1].Get_Target_Wavelength_Table(ref wavetable);

                if (process_error != 0)
                {
                    Show_STS_Error(process_error);
                    return;
                }


                // ----File save 

                string fpath = string.Empty;                  // file path 

                 SaveFileDialog1.Title = "port:" + TSL_OSUport[loop1] + "Reference Raw data";
                 SaveFileDialog1.Filter = "csv file(*.csv)|*.csv";
                 SaveFileDialog1.ShowDialog();
                fpath =  SaveFileDialog1.FileName;

                System.IO.StreamWriter writer = new System.IO.StreamWriter(fpath, false, System.Text.Encoding.GetEncoding("UTF-8"));

                string hedder = string.Empty;                 // file hedder 

                hedder = "Wavelength(nm)";

                foreach (STSDataStruct item in Data_struct)
                {
                    if (item.SweepCount != 1)
                        continue;

                    hedder = hedder + ",MPM" + System.Convert.ToString(item.MPMNumber + 1) + "Slot" + System.Convert.ToString(item.SlotNumber) + "Ch" + System.Convert.ToString(item.ChannelNumber);
                }

                if ( chkeach_ch.Checked)
                {
                    foreach (STSDataStruct item in Refdata_struct)
                        hedder = hedder + ",Monitor_MPM" + System.Convert.ToString(item.MPMNumber + 1) + "Slot" + System.Convert.ToString(item.SlotNumber) + "Ch" + System.Convert.ToString(item.ChannelNumber);
                }
                else
                    hedder = hedder + ",Monitor";

                writer.WriteLine(hedder);


                // Write data 
                string write_str = string.Empty;                  // write string
                int loop2;                                    // loop count 2
                int loop3;                                    // loop count 3
                int loop4;                                    // loop count 4


                for (loop2 = 0; loop2 <= wavetable.GetUpperBound(0); loop2++)
                {

                    // wavelength data
                    write_str = System.Convert.ToString(wavetable[loop2]);

                    // Power data
                    for (loop3 = 0; loop3 <= lstpowdata.Count - 1; loop3++)
                        write_str = write_str + "," + lstpowdata[loop3][loop2];


                    // monitordata
                    for (loop4 = 0; loop4 <= lstmonitordata.Count - 1; loop4++)
                        write_str = write_str + "," + lstmonitordata[loop4][loop2];

                    writer.WriteLine(write_str);
                }

                lstpowdata.Clear();
                writer.Close();
            }

            MessageBox.Show("Completed.");
        }

        private void btnsaveRawdata_Click(object sender, EventArgs e)
        {
            // -------------------------------------------------------------------------
            // Save mesurement raw data
            // -------------------------------------------------------------------------
            int loop1;                                        // loop1
            int loop2;                                        // loop2
            int loop3;                                        // loop3
            int loop4;                                        // loop4
            double[] wavelength_table = null;                  // Wavelength table
            float[] monitordata = null;
            float[] powerdata = null;
            int errorcode;                                    // Errorcode
            List<float[]> lstpower = new List<float[]>();

            string fpath = string.Empty;              // File　path
            System.IO.StreamWriter writer;            // Writer 
            string write_string = string.Empty;
            string hedder = string.Empty;

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {

                // -- Get Wavelength table
                errorcode = Cal_STS[loop1].Get_Target_Wavelength_Table(ref wavelength_table);

                if (errorcode != 0)
                {
                    Show_STS_Error(errorcode);
                    return;
                }

                for (loop2 = 0; loop2 <= Meas_rang.Count - 1; loop2++)
                {

                    // ----get raw power data same range 
                    foreach (STSDataStruct item in Data_struct)
                    {
                        if (item.RangeNumber != Meas_rang[loop2])
                            continue;

                        errorcode = Cal_STS[loop1].Get_Meas_Power_Rawdata(item,ref powerdata);

                        if (errorcode != 0)
                        {
                            Show_STS_Error(errorcode);
                            return;
                        }

                        lstpower.Add(powerdata.ToArray());
                    }

                    // ----get raw monitor data same range
                    foreach (STSMonitorStruct monitoritem in Meas_monitor_struct)
                    {
                        if (monitoritem.SweepCount == loop2 + 1)
                            errorcode = Cal_STS[loop1].Get_Meas_Monitor_Rawdata(monitoritem, ref monitordata);
                        else
                            continue;
                    }


                    // ----File save at same range data 
                    switch (loop2)
                    {
                        case 0:
                            {
                                 SaveFileDialog1.Title = "port:" + TSL_OSUport[loop1] + " 1st Range data";
                                break;
                            }

                        case 1:
                            {
                                 SaveFileDialog1.Title = "port:" + TSL_OSUport[loop1] + " 2nd Range data";
                                break;
                            }

                        default:
                            {
                                 SaveFileDialog1.Title = "port:" + TSL_OSUport[loop1] + " " + System.Convert.ToString(loop2 + 1) + "rd Range data";
                                break;
                            }
                    }


                     SaveFileDialog1.ShowDialog();
                     SaveFileDialog1.Filter = "csv file(*.csv)|*.csv";
                    fpath =  SaveFileDialog1.FileName;

                    writer = new System.IO.StreamWriter(fpath, false, System.Text.Encoding.GetEncoding("UTF-8"));

                    hedder = "wavelength";

                    foreach (STSDataStruct item in Data_struct)
                    {
                        if (item.RangeNumber != Meas_rang[loop2])
                            continue;

                        hedder = hedder + "," + "MPM" + System.Convert.ToString(item.MPMNumber + 1) + "Slot" + System.Convert.ToString(item.SlotNumber) + "Ch" + System.Convert.ToString(item.ChannelNumber);
                    }

                    hedder = hedder + "," + "Monitordata";

                    writer.WriteLine(hedder);

                    for (loop3 = 0; loop3 <= wavelength_table.GetUpperBound(0); loop3++)
                    {
                        write_string = System.Convert.ToString(wavelength_table[loop3]);

                        for (loop4 = 0; loop4 <= lstpower.Count - 1; loop4++)
                            write_string = write_string + "," + lstpower[loop4][loop3];

                        write_string = write_string + "," + monitordata[loop3];


                        writer.WriteLine(write_string);
                    }

                    writer.Close();
                    lstpower = new List<float[]>();
                    monitordata = null;
                }
            }


            MessageBox.Show("Completed.");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------------
            // Reference Data Read
            // This function must use after "SET" 
            // -----------------------------------------------------------------------------
            string fpath = string.Empty;
            System.IO.StreamReader reader;
            int loop1;                                            // Loop count1
            TSLSweepItem tslitem;

            for (loop1 = 0; loop1 <= TSL.GetUpperBound(0); loop1++)
            {
                tslitem = TSL_SweepItemDictionary[TSL[loop1]];

                // ----Reference file　Read 

                 OpenFileDialog1.Title = "port:" + TSL_OSUport[loop1] + "_Reference Data";
                 OpenFileDialog1.ShowDialog();
                fpath =  OpenFileDialog1.FileName;

                reader = new System.IO.StreamReader(fpath);

                string read_st = string.Empty;                            // Read String 
                string[] split_st = null;                              // split strin array

                // hedder Read 
                read_st = reader.ReadLine();
                read_st = read_st.Trim();

                split_st = read_st.Split(',');


                // Check data cout 
                int ch_count;                                         // file data ch count 
                int loop2;                                            // Loop count2
                string chk_str = string.Empty;                            // check string
                int mpm_number;                                       // MPM number
                int slot_number;                                      // Slot number
                int ch_number;                                        // ch number 


                // Check data cout 
                if ( chkeach_ch.Checked)
                    ch_count = (split_st.Count() - 1) / 2;
                else
                    ch_count = split_st.Count() - 2;

                if (ch_count !=  chklst_ch.CheckedItems.Count)
                {
                    MessageBox.Show("Reference data mismatch.Please selecet right data.");
                    reader.Close();
                    return;
                }

                // ----Check parameter & make reference data struct 
                STSDataStruct refdata_strunct=new STSDataStruct();                        // Data struct for reference
                List<STSDataStruct> lst_refdata_struct = new List<STSDataStruct>();        // Data struct for reference List    
                bool match_flag = false;                           // match flag


                for (loop2 = 1; loop2 <= ch_count; loop2++)
                {
                    // MPM device number 
                    chk_str = split_st[loop2].Substring(3, 1);
                    mpm_number = System.Convert.ToInt32(chk_str) - 1;

                    // MPM Slot number 
                    chk_str = split_st[loop2].Substring(8, 1);
                    slot_number = System.Convert.ToInt32(chk_str);

                    // MPM Ch number 
                    chk_str = split_st[loop2].Substring(11, 1);
                    ch_number = System.Convert.ToInt32(chk_str);


                    // Check exsist data in data struct 
                    foreach (STSDataStruct item in Data_struct)
                    {
                        if (item.MPMNumber == mpm_number & item.SlotNumber == slot_number & item.ChannelNumber == ch_number)
                        {
                            match_flag = true;
                            break;
                        }
                    }

                    if (match_flag == false)
                    {
                        MessageBox.Show("Reference data mismatch.Please selecet right data.");
                        reader.Close();
                        return;
                    }

                    // Add reference data struct 
                    refdata_strunct.MPMNumber = mpm_number;
                    refdata_strunct.SlotNumber = slot_number;
                    refdata_strunct.ChannelNumber = ch_number;
                    refdata_strunct.RangeNumber = 1;
                    refdata_strunct.SweepCount = 1;

                    lst_refdata_struct.Add(refdata_strunct);
                }

                // ----Read Reference data

                if ( chkeach_ch.Checked)
                {
                    List<float>[] power;                 // Power data list 
                    List<float>[] monitor;               // Monitordata
                    int counter = new int();                         // Counter
                    double wavelength=0;                       // Read Wavelength 


                    power = new List<float>[ch_count];
                    monitor = new List<float>[ch_count];

                    for (loop2 = 0; loop2 <= ch_count - 1; loop2++)
                    {
                        power[loop2] = new List<float>();
                        monitor[loop2] = new List<float>();
                    }


                    do
                    {
                        read_st = reader.ReadLine();
                        if (read_st == "")
                            break;
                        read_st = read_st.Trim();
                        split_st = read_st.Split(',');

                        // Check Start Wavelength 
                        if (counter == 0)
                        {
                            if (System.Convert.ToDouble(split_st[0]) != tslitem.startwave)
                            {
                                MessageBox.Show("Reference data mismatch.Please selecet right data.");
                                reader.Close();
                                return;
                            }
                        }

                        // hold wavelength data
                        wavelength = System.Convert.ToDouble(split_st[0]);
                        for (loop2 = 0; loop2 <= ch_count - 1; loop2++)
                            power[loop2].Add(System.Convert.ToSingle(split_st[loop2 + 1]));
                        for (loop2 = 0; loop2 <= ch_count - 1; loop2++)
                            monitor[loop2].Add(System.Convert.ToSingle(split_st[ch_count + loop2 + 1]));
                        counter = counter + 1;
                    }
                    while (true);

                    reader.Close();

                    // Check Stop wavelength 
                    if (wavelength != tslitem.stopwave)
                    {
                        MessageBox.Show("Reference data mismatch.Please selecet right data.");
                        return;
                    }

                    // check number of point 

                    int datapoint;                            // number of data point 

                    datapoint = (int)(Math.Abs(tslitem.stopwave - tslitem.startwave) / (double)tslitem.wavestep) + 1;

                    if (datapoint != monitor[0].Count)
                    {
                        MessageBox.Show("Reference data mismatch.Please selecet right data.");
                        return;
                    }



                    // ----Add in  data to STS Process class
                    int errorcode;                            // Errorcode
                    counter = 0;

                    foreach (var item in lst_refdata_struct)
                    {
                        // Add in reference data of rescaled.
                        errorcode = Cal_STS[loop1].Add_Ref_Rawdata(power[counter].ToArray(), monitor[counter].ToArray(), item);

                        if (errorcode != 0)
                        {
                            Show_Inst_Error(errorcode);
                            return;
                        }
                        counter = counter + 1;
                    }
                }
                else
                {
                    List<float>[] power;                 // Power data list 
                    List<float> monitor = new List<float>();             // Monitordata
                    int counter=0;                         // Counter
                    double wavelength=0;                       // Read Wavelength 


                    power = new List<float>[ch_count];

                    for (loop2 = 0; loop2 <= ch_count - 1; loop2++)
                        power[loop2] = new List<float>();


                    do
                    {
                        read_st = reader.ReadLine();
                        if (read_st == "")
                            break;
                        read_st = read_st.Trim();
                        split_st = read_st.Split(',');

                        // Check Start Wavelength 
                        if (counter == 0)
                        {
                            if (System.Convert.ToDouble(split_st[0]) != tslitem.startwave)
                            {
                                MessageBox.Show("Reference data mismatch.Please selecet right data.");
                                reader.Close();
                                return;
                            }
                        }

                        // hold wavelength data
                        wavelength = System.Convert.ToDouble(split_st[0]);
                        for (loop2 = 0; loop2 <= ch_count - 1; loop2++)
                            power[loop2].Add(System.Convert.ToSingle(split_st[loop2 + 1]));
                        monitor.Add(System.Convert.ToSingle(split_st[ch_count + 1]));
                        counter = counter + 1;
                    }
                    while (true);

                    reader.Close();

                    // Check Stop wavelength 
                    if (wavelength != tslitem.stopwave)
                    {
                        MessageBox.Show("Reference data mismatch.Please selecet right data.");
                        return;
                    }

                    // check number of point 

                    int datapoint;                            // number of data point 

                    datapoint = (int)(Math.Abs(tslitem.stopwave - tslitem.startwave) / (double)tslitem.wavestep) + 1;

                    if (datapoint != monitor.Count)
                    {
                        MessageBox.Show("Reference data mismatch.Please selecet right data.");
                        return;
                    }

                    // ----Add in  data to STS Process class
                    int errorcode;                            // Errorcode
                    counter = 0;

                    foreach (var item in lst_refdata_struct)
                    {
                        // Add in reference data of rescaled.
                        errorcode = Cal_STS[loop1].Add_Ref_Rawdata(power[counter].ToArray(), monitor.ToArray(), item);

                        if (errorcode != 0)
                        {
                            Show_Inst_Error(errorcode);
                            return;
                        }
                        counter = counter + 1;
                    }
                }
            }
            MessageBox.Show("Completed.");
        }
    }
}