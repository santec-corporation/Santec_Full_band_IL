using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Full_band_ILSTS_Sample_CS
{
    public partial class Instrument_Setting : Form
    {
        public Instrument_Setting()
        {
            InitializeComponent();
        }
        public string[] SPU_Resource;                 // SPU resource name array
        public string[] USB_Resource;                 // USB resource name array

        public string[] TSL_Communicater = new string[4];            // TSL Communicator name
        public string OSU_Communicater;               // OSU Communicator name
        public string MPM_Communicater;               // MPM Communicator name

        public int TSL_Count;                     // TSL number of device
        public string[] TSL_Address = new string[4];                 // TSL address
        public int[] TSL_Portnumber = new int[4];             // TSL LAN port number
        public int[] TSL_OSUport = new int[4];                 // Combination of TSL and OSU port

        public string OSU_Address;                    // OSU address 
        public int OSU_Portnumber;                // OSU LAN port number
        public string OSU_DeviveID;                   // OSU Deviece resource
        public bool Flag_OSU_100;                  // OSU-100 or not  T: OSU-100 F: OSU-110

        public int MPM_Count;                     // MPM number of control count
        public string[] MPM_Address = new string[2];                 // MPM address 
        public int[] MPM_Portnumber = new int[2];             // MPM LAN port number

        public string SPU_DeviveID;                   // SPU Deviece resource
        private void btnconnect_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------------
            // Connect       
            // ----------------------------------------------------------


            // ----TSL Communication diteal

            TSL_Count = 0;

            if (chktsl_dev1.Checked == true)
            {

                // -----Device1

                // GPIB Communcation?
                if (rdotsl_gpib1.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "GPIB";
                    TSL_Address[TSL_Count] = txttsl_gpibadd1.Text;
                    TSL_Portnumber[TSL_Count] = 0;
                }

                // TCP/IP Communciation?
                if (rdo_tsltcpip1.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "LAN";
                    TSL_Address[TSL_Count] = txttsl_ipadd1.Text;
                    TSL_Portnumber[TSL_Count] = int.Parse(txttsl_lanport1.Text);
                }

                // USB Communcation?
                if (rdo_tslusb1.Checked == true)
                {
                    if (cmbtsl_usb1.Text == "")
                    {
                        MessageBox.Show("Please enter to the TSL USB Resource");
                        return;
                    }

                    TSL_Communicater[TSL_Count] = "USB";
                    TSL_Address[TSL_Count] = cmbtsl_usb1.SelectedIndex.ToString();
                    TSL_Portnumber[TSL_Count] = 0;
                }
                TSL_OSUport[TSL_Count] = 1;
                TSL_Count = TSL_Count + 1;
            }

            if (chktsl_dev2.Checked == true)
            {

                // -----Device2

                // GPIB Communcation?
                if (rdotsl_gpib2.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "GPIB";
                    TSL_Address[TSL_Count] = txttsl_gpibadd2.Text;
                    TSL_Portnumber[TSL_Count] = 0;
                }

                // TCP/IP Communciation?
                if (rdo_tsltcpip2.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "LAN";
                    TSL_Address[TSL_Count] = txttsl_ipadd2.Text;
                    TSL_Portnumber[TSL_Count] = int.Parse(txttsl_lanport2.Text);
                }

                // USB Communcation?
                if (rdo_tslusb2.Checked == true)
                {
                    if (cmbtsl_usb2.Text == "")
                    {
                        MessageBox.Show("Please enter to the TSL USB Resource");
                        return;
                    }

                    TSL_Communicater[TSL_Count] = "USB";
                    TSL_Address[TSL_Count] = cmbtsl_usb2.SelectedIndex.ToString();
                    TSL_Portnumber[TSL_Count] = 0;
                }

                TSL_OSUport[TSL_Count] = 2;
                TSL_Count = TSL_Count + 1;
            }

            if (chktsl_dev3.Checked == true)
            {

                // -----Device3

                // GPIB Communcation?
                if (rdotsl_gpib3.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "GPIB";
                    TSL_Address[TSL_Count] = txttsl_gpibadd3.Text;
                    TSL_Portnumber[TSL_Count] = 0;
                }

                // TCP/IP Communciation?
                if (rdo_tsltcpip3.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "LAN";
                    TSL_Address[TSL_Count] = txttsl_ipadd3.Text;
                    TSL_Portnumber[TSL_Count] = int.Parse(txttsl_lanport3.Text);
                }

                // USB Communcation?
                if (rdo_tslusb3.Checked == true)
                {
                    if (cmbtsl_usb3.Text == "")
                    {
                        MessageBox.Show("Please enter to the TSL USB Resource");
                        return;
                    }

                    TSL_Communicater[TSL_Count] = "USB";
                    TSL_Address[TSL_Count] = cmbtsl_usb3.SelectedIndex.ToString();
                    TSL_Portnumber[TSL_Count] = 0;
                }

                TSL_OSUport[TSL_Count] = 3;
                TSL_Count = TSL_Count + 1;
            }

            if (chktsl_dev4.Checked == true)
            {

                // -----Device4

                // GPIB Communcation?
                if (rdotsl_gpib4.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "GPIB";
                    TSL_Address[TSL_Count] = txttsl_gpibadd4.Text;
                    TSL_Portnumber[TSL_Count] = 0;
                }

                // TCP/IP Communciation?
                if (rdo_tsltcpip4.Checked == true)
                {
                    TSL_Communicater[TSL_Count] = "LAN";
                    TSL_Address[TSL_Count] = txttsl_ipadd4.Text;
                    TSL_Portnumber[TSL_Count] = int.Parse(txttsl_lanport4.Text);
                }

                // USB Communcation?
                if (rdo_tslusb4.Checked == true)
                {
                    if (cmbtsl_usb4.Text == "")
                    {
                        MessageBox.Show("Please enter to the TSL USB Resource");
                        return;
                    }

                    TSL_Communicater[TSL_Count] = "USB";
                    TSL_Address[TSL_Count] = cmbtsl_usb4.SelectedIndex.ToString();
                    TSL_Portnumber[TSL_Count] = 0;
                }

                TSL_OSUport[TSL_Count] = 4;
                TSL_Count = TSL_Count + 1;
            }

            //Determine the number of channels
            Array.Resize(ref TSL_OSUport, TSL_Count);

            if (TSL_Count == 0)
            {
                MessageBox.Show("Please enter to the SPU device resouce");
                return;
            }

            // -----OSU Communcation diteal

            if (rdo110.Checked)
            {
                // OSU-110

                // GPIB Communcation?
                if (rdo_osugpib.Checked == true)
                {
                    OSU_Communicater = "GPIB";
                    OSU_Address = txtosu_gpibadd.Text;
                    OSU_Portnumber = 0;
                }

                // TCP/IP Communciation?
                if (rdo_osutcpip.Checked == true)
                {
                    OSU_Communicater = "LAN";
                    OSU_Address = txtosu_ipadd.Text;
                    OSU_Portnumber = int.Parse(txtosu_lanport.Text);
                }

                // USB Communcation?
                if (rdo_osuusb.Checked == true)
                {
                    if (cmbosu_usb.Text == "")
                    {
                        MessageBox.Show("Please enter to the OSU USB Resource");
                        return;
                    }

                    OSU_Communicater = "USB";
                    OSU_Address = cmbosu_usb.SelectedIndex.ToString();
                    OSU_Portnumber = 0;
                }

                OSU_DeviveID = "";
                Flag_OSU_100 = false;
            }
            else
            {
                // OSU-100

                if (cmbosu_dev.Text == "")
                {
                    MessageBox.Show("Please enter to the OSU device Resource");
                    return;
                }

                OSU_DeviveID = cmbosu_dev.Text;
                OSU_Communicater = "";
                OSU_Address = "";
                OSU_Portnumber = 0;
                Flag_OSU_100 = true;
            }

            // -----MPM Communcation diteal

            // Multi Device?
            if (chkmulti_dev.Checked == true)
            {
                MPM_Count = 2;

                // GPIB communcation?
                if (rdo_mpmgpib.Checked == true)
                {
                    MPM_Address[0] = txtmpm_gpibadd1.Text;
                    MPM_Address[1] = txtmpm_gpibadd2.Text;
                    MPM_Portnumber[0] = 0;
                    MPM_Portnumber[1] = 0;

                    MPM_Communicater = "GPIB";
                }
                else
                {
                    // TCL/IP communcation?
                    MPM_Address[0] = txtmpm_ipadd1.Text;
                    MPM_Address[1] = txtmpm_ipadd2.Text;
                    MPM_Portnumber[0] = int.Parse(txtmpm_lanport1.Text);
                    MPM_Portnumber[1] = int.Parse(txtmpm_lanport2.Text);

                    MPM_Communicater = "LAN";
                }
            }
            else
            {
                MPM_Count = 1;

                // GPIB communcation?
                if (rdo_mpmgpib.Checked == true)
                {
                    MPM_Address[0] = txtmpm_gpibadd1.Text;
                    MPM_Address[1] = "";
                    MPM_Portnumber[0] = 0;
                    MPM_Portnumber[1] = 0;

                    MPM_Communicater = "GPIB";
                }
                else
                {
                    // TCL/IP communcation?
                    MPM_Address[0] = txtmpm_ipadd1.Text;
                    MPM_Address[1] = "";
                    MPM_Portnumber[0] = int.Parse(txtmpm_lanport1.Text);
                    MPM_Portnumber[1] = 0;

                    MPM_Communicater = "LAN";
                }
            }


            // SPU Resouce
            if (cmbspu_dev.Text == "")
            {
                MessageBox.Show("Please enter to the SPU device resouce");
                return;
            }

            SPU_DeviveID = cmbspu_dev.Text;

            Dispose();
        }

        private void Instrument_Setting_Load(object sender, EventArgs e)
        {

            // ---------------------------------------------------------------
            // Sub Form Load
            // ---------------------------------------------------------------
            int loop1;

            // ---Add in SPU resource to comboboxfrom main form
            for (loop1 = 0; loop1 <= SPU_Resource.GetUpperBound(0); loop1++)
                cmbspu_dev.Items.Add(SPU_Resource[loop1]);

            // ----Add in TSL USB resource to combobox from main form
            for (loop1 = 0; loop1 <= USB_Resource.GetUpperBound(0); loop1++)
            {
                cmbtsl_usb1.Items.Add(USB_Resource[loop1]);
                cmbtsl_usb2.Items.Add(USB_Resource[loop1]);
                cmbtsl_usb3.Items.Add(USB_Resource[loop1]);
                cmbtsl_usb4.Items.Add(USB_Resource[loop1]);
            }

            // ---Add in OSU resource to comboboxfrom main form
            for (loop1 = 0; loop1 <= SPU_Resource.GetUpperBound(0); loop1++)
                cmbosu_dev.Items.Add(SPU_Resource[loop1]);

            // ----Add in OSU USB resource to combobox from main form
            for (loop1 = 0; loop1 <= USB_Resource.GetUpperBound(0); loop1++)
                cmbosu_usb.Items.Add(USB_Resource[loop1]);
        }

        private void chktsl_dev1_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------
            // Selecet TSL Multi-Device
            // -------------------------------------------------------------------

            if (chktsl_dev1.Checked == false)
                grp_tsldev1.Enabled = false;
            else
                grp_tsldev1.Enabled = true;
        }

        private void chktsl_dev2_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------
            // Selecet TSL Multi-Device
            // -------------------------------------------------------------------

            if (chktsl_dev2.Checked == false)
                grp_tsldev2.Enabled = false;
            else
                grp_tsldev2.Enabled = true;
        }

        private void chktsl_dev3_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------
            // Selecet TSL Multi-Device
            // -------------------------------------------------------------------

            if (chktsl_dev3.Checked == false)
                grp_tsldev3.Enabled = false;
            else
                grp_tsldev3.Enabled = true;
        }

        private void chktsl_dev4_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------
            // Selecet TSL Multi-Device
            // -------------------------------------------------------------------

            if (chktsl_dev4.Checked == false)
                grp_tsldev4.Enabled = false;
            else
                grp_tsldev4.Enabled = true;
        }

        private void rdo570_1_CheckedChanged(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // 570 Checked
            // 
            // -----------------------------------------------------

            if (rdo570_1.Checked == true)
            {
                // TSL-570
                rdo_tsltcpip1.Enabled = true;
                rdo_tslusb1.Enabled = true;
            }
            else
            {
                // TSL-550/710
                // There can control only GPIB
                rdo_tslusb1.Enabled = false;
                rdo_tsltcpip1.Enabled = false;
                rdotsl_gpib1.Checked = true;
            }
        }

        private void rdo570_2_CheckedChanged(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // 570 Checked
            // 
            // -----------------------------------------------------

            if (rdo570_2.Checked == true)
            {
                // TSL-570
                rdo_tsltcpip2.Enabled = true;
                rdo_tslusb2.Enabled = true;
            }
            else
            {
                // TSL-550/710
                // There can control only GPIB
                rdo_tslusb2.Enabled = false;
                rdo_tsltcpip2.Enabled = false;
                rdotsl_gpib2.Checked = true;
            }
        }

        private void rdo570_3_CheckedChanged(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // 570 Checked
            // 
            // -----------------------------------------------------

            if (rdo570_3.Checked == true)
            {
                // TSL-570
                rdo_tsltcpip3.Enabled = true;
                rdo_tslusb3.Enabled = true;
            }
            else
            {
                // TSL-550/710
                // There can control only GPIB
                rdo_tslusb3.Enabled = false;
                rdo_tsltcpip3.Enabled = false;
                rdotsl_gpib3.Checked = true;
            }
        }

        private void rdo570_4_CheckedChanged(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // 570 Checked
            // 
            // -----------------------------------------------------

            if (rdo570_4.Checked == true)
            {
                // TSL-570
                rdo_tsltcpip4.Enabled = true;
                rdo_tslusb4.Enabled = true;
            }
            else
            {
                // TSL-550/710
                // There can control only GPIB
                rdo_tslusb4.Enabled = false;
                rdo_tsltcpip4.Enabled = false;
                rdotsl_gpib4.Checked = true;
            }
        }

        private void rdotsl_gpib1_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // TSL Control GPIB
            // --------------------------------------------------------------------

            if (rdotsl_gpib1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = true;
                txttsl_ipadd1.Enabled = false;
                txttsl_lanport1.Enabled = false;
                cmbtsl_usb1.Enabled = false;
            }
            else
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = true;
            }
        }

        private void rdotsl_gpib2_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // TSL Control GPIB
            // --------------------------------------------------------------------

            if (rdotsl_gpib2.Checked == true)
            {
                txttsl_gpibadd2.Enabled = true;
                txttsl_ipadd2.Enabled = false;
                txttsl_lanport2.Enabled = false;
                cmbtsl_usb2.Enabled = false;
            }
            else
            {
                txttsl_gpibadd2.Enabled = false;
                txttsl_ipadd2.Enabled = true;
                txttsl_lanport2.Enabled = true;
                cmbtsl_usb2.Enabled = true;
            }
        }

        private void rdotsl_gpib3_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // TSL Control GPIB
            // --------------------------------------------------------------------

            if (rdotsl_gpib3.Checked == true)
            {
                txttsl_gpibadd3.Enabled = true;
                txttsl_ipadd3.Enabled = false;
                txttsl_lanport3.Enabled = false;
                cmbtsl_usb3.Enabled = false;
            }
            else
            {
                txttsl_gpibadd3.Enabled = false;
                txttsl_ipadd3.Enabled = true;
                txttsl_lanport3.Enabled = true;
                cmbtsl_usb3.Enabled = true;
            }
        }

        private void rdotsl_gpib4_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // TSL Control GPIB
            // --------------------------------------------------------------------

            if (rdotsl_gpib4.Checked == true)
            {
                txttsl_gpibadd4.Enabled = true;
                txttsl_ipadd4.Enabled = false;
                txttsl_lanport4.Enabled = false;
                cmbtsl_usb4.Enabled = false;
            }
            else
            {
                txttsl_gpibadd4.Enabled = false;
                txttsl_ipadd4.Enabled = true;
                txttsl_lanport4.Enabled = true;
                cmbtsl_usb4.Enabled = true;
            }
        }

        private void rdo_tsltcpip1_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tsltcpip2_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tsltcpip3_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tsltcpip4_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tslusb1_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tslusb2_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control TCP/IP
            // ------------------------------------------------------

            if (rdo_tsltcpip1.Checked == true)
            {
                txttsl_gpibadd1.Enabled = false;
                txttsl_ipadd1.Enabled = true;
                txttsl_lanport1.Enabled = true;
                cmbtsl_usb1.Enabled = false;
            }
        }

        private void rdo_tslusb3_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control USB
            // ------------------------------------------------------

            if (rdo_tslusb3.Checked == true)
            {
                txttsl_gpibadd3.Enabled = false;
                txttsl_ipadd3.Enabled = false;
                txttsl_lanport3.Enabled = false;
                cmbtsl_usb3.Enabled = true;
            }
        }

        private void rdo_tslusb4_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // TSL Control USB
            // ------------------------------------------------------

            if (rdo_tslusb4.Checked == true)
            {
                txttsl_gpibadd4.Enabled = false;
                txttsl_ipadd4.Enabled = false;
                txttsl_lanport4.Enabled = false;
                cmbtsl_usb4.Enabled = true;
            }
        }

        private void rdo110_CheckedChanged(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // 110 Checked
            // 
            // -----------------------------------------------------

            if (rdo110.Checked == true)
            {
                // OSU-110
                rdo_osugpib.Enabled = true;
                rdo_osutcpip.Enabled = true;
                rdo_osuusb.Enabled = true;
                rdo_osugpib.Checked = true;
                txtosu_gpibadd.Enabled = true;
                cmbosu_dev.Enabled = false;

                cmbspu_dev.Enabled = true;
                cmbspu_dev.SelectedIndex = -1;
                cmbosu_dev.SelectedIndex = -1;
            }
            else
            {
                // OSU-100
                rdo_osugpib.Enabled = false;
                rdo_osutcpip.Enabled = false;
                rdo_osuusb.Enabled = false;
                txtosu_gpibadd.Enabled = false;
                txtosu_ipadd.Enabled = false;
                txtosu_lanport.Enabled = false;
                cmbosu_usb.Enabled = false;
                cmbosu_dev.Enabled = true;

                cmbspu_dev.Enabled = false;
                cmbspu_dev.SelectedIndex = -1;
            }
        }

        private void rdo_osugpib_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // OSU Control GPIB
            // --------------------------------------------------------------------

            if (rdo_osugpib.Checked == true)
            {
                txtosu_gpibadd.Enabled = true;
                txtosu_ipadd.Enabled = false;
                txtosu_lanport.Enabled = false;
                cmbosu_usb.Enabled = false;
            }
            else
            {
                txtosu_gpibadd.Enabled = false;
                txtosu_ipadd.Enabled = true;
                txtosu_lanport.Enabled = true;
                cmbosu_usb.Enabled = true;
            }
        }

        private void rdo_osutcpip_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // OSU Control TCP/IP
            // ------------------------------------------------------

            if (rdo_osutcpip.Checked == true)
            {
                txtosu_gpibadd.Enabled = false;
                txtosu_ipadd.Enabled = true;
                txtosu_lanport.Enabled = true;
                cmbosu_usb.Enabled = false;
            }
        }

        private void rdo_osuusb_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // OSU Control USB
            // ------------------------------------------------------

            if (rdo_osuusb.Checked == true)
            {
                txtosu_gpibadd.Enabled = false;
                txtosu_ipadd.Enabled = false;
                txtosu_lanport.Enabled = false;
                cmbosu_usb.Enabled = true;
            }
        }

        private void cmbosu_dev_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------
            // Deviece resource
            // ------------------------------------------------------

            cmbspu_dev.SelectedIndex = cmbosu_dev.SelectedIndex;
        }

        private void chkmulti_dev_CheckedChanged(object sender, EventArgs e)
        {
            // ------------------------------------------------------------------
            // Selecet MPM Multi-Device
            // -------------------------------------------------------------------

            grp_mpmdev2.Enabled = chkmulti_dev.Checked;
        }

        private void rdo_mpmgpib_CheckedChanged(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------
            // MPM Control GPIB
            // --------------------------------------------------------------------

            if (rdo_mpmgpib.Checked == true)
            {
                txtmpm_gpibadd1.Enabled = true;
                txtmpm_ipadd1.Enabled = false;
                txtmpm_lanport1.Enabled = false;

                txtmpm_gpibadd2.Enabled = true;
                txtmpm_ipadd2.Enabled = false;
                txtmpm_lanport2.Enabled = false;
            }
            else
            {
                txtmpm_gpibadd1.Enabled = false;
                txtmpm_ipadd1.Enabled = true;
                txtmpm_lanport1.Enabled = true;

                txtmpm_gpibadd2.Enabled = false;
                txtmpm_ipadd2.Enabled = true;
                txtmpm_lanport2.Enabled = true;
            }
        }
    }
}
