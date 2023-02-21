Public Class Instrument_Setting

    Public SPU_Resource() As String                 'SPU resource name array
    Public USB_Resource() As String                 'USB resource name array

    Public TSL_Communicater(3) As String            'TSL Communicator name
    Public OSU_Communicater As String               'OSU Communicator name
    Public MPM_Communicater As String               'MPM Communicator name

    Public TSL_Count As Integer                     'TSL number of device
    Public TSL_Address(3) As String                 'TSL address
    Public TSL_Portnumber(3) As Integer             'TSL LAN port number
    Public TSL_OSUport() As Integer                 'Combination of TSL and OSU port

    Public OSU_Address As String                    'OSU address 
    Public OSU_Portnumber As Integer                'OSU LAN port number
    Public OSU_DeviveID As String                   'OSU Deviece resource
    Public Flag_OSU_100 As Boolean                  'OSU-100 or not  T: OSU-100 F: OSU-110

    Public MPM_Count As Integer                     'MPM number of control count
    Public MPM_Address(1) As String                 'MPM address 
    Public MPM_Portnumber(1) As Integer             'MPM LAN port number

    Public SPU_DeviveID As String                   'SPU Deviece resource

    Private Sub Instrument_Setting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '---------------------------------------------------------------
        '       Sub Form Load
        '---------------------------------------------------------------
        Dim loop1 As Integer

        '---Add in SPU resource to comboboxfrom main form
        For loop1 = 0 To UBound(SPU_Resource)
            Me.cmbspu_dev.Items.Add(SPU_Resource(loop1))
        Next

        '----Add in TSL USB resource to combobox from main form
        For loop1 = 0 To UBound(USB_Resource)
            Me.cmbtsl_usb1.Items.Add(USB_Resource(loop1))
            Me.cmbtsl_usb2.Items.Add(USB_Resource(loop1))
            Me.cmbtsl_usb3.Items.Add(USB_Resource(loop1))
            Me.cmbtsl_usb4.Items.Add(USB_Resource(loop1))
        Next

        '---Add in OSU resource to comboboxfrom main form
        For loop1 = 0 To UBound(SPU_Resource)
            Me.cmbosu_dev.Items.Add(SPU_Resource(loop1))
        Next

        '----Add in OSU USB resource to combobox from main form
        For loop1 = 0 To UBound(USB_Resource)
            Me.cmbosu_usb.Items.Add(USB_Resource(loop1))
        Next

    End Sub

    Private Sub chktsl_dev1_CheckedChanged(sender As Object, e As EventArgs) Handles chktsl_dev1.CheckedChanged
        '------------------------------------------------------------------
        '           Selecet TSL Multi-Device
        '-------------------------------------------------------------------

        If chktsl_dev1.Checked = False Then
            Me.grp_tsldev1.Enabled = False
        Else
            Me.grp_tsldev1.Enabled = True
        End If

    End Sub

    Private Sub chktsl_dev2_CheckedChanged(sender As Object, e As EventArgs) Handles chktsl_dev2.CheckedChanged
        '------------------------------------------------------------------
        '           Selecet TSL Multi-Device
        '-------------------------------------------------------------------

        If chktsl_dev2.Checked = False Then
            Me.grp_tsldev2.Enabled = False
        Else
            Me.grp_tsldev2.Enabled = True
        End If

    End Sub

    Private Sub chktsl_dev3_CheckedChanged(sender As Object, e As EventArgs) Handles chktsl_dev3.CheckedChanged, chktsl_dev1.CheckedChanged
        '------------------------------------------------------------------
        '           Selecet TSL Multi-Device
        '-------------------------------------------------------------------

        If chktsl_dev3.Checked = False Then
            Me.grp_tsldev3.Enabled = False
        Else
            Me.grp_tsldev3.Enabled = True
        End If

    End Sub

    Private Sub chktsl_dev4_CheckedChanged(sender As Object, e As EventArgs) Handles chktsl_dev4.CheckedChanged
        '------------------------------------------------------------------
        '           Selecet TSL Multi-Device
        '-------------------------------------------------------------------

        If chktsl_dev4.Checked = False Then
            Me.grp_tsldev4.Enabled = False
        Else
            Me.grp_tsldev4.Enabled = True
        End If

    End Sub

    Private Sub rdo570_1_CheckedChanged(sender As Object, e As EventArgs) Handles rdo570_1.CheckedChanged
        '-----------------------------------------------------
        '       570 Checked
        '       
        '-----------------------------------------------------

        If Me.rdo570_1.Checked = True Then
            'TSL-570
            Me.rdo_tsltcpip1.Enabled = True
            Me.rdo_tslusb1.Enabled = True
        Else
            'TSL-550/710
            ' There can control only GPIB
            Me.rdo_tslusb1.Enabled = False
            Me.rdo_tsltcpip1.Enabled = False
            Me.rdotsl_gpib1.Checked = True
        End If

    End Sub

    Private Sub rdo570_2_CheckedChanged(sender As Object, e As EventArgs) Handles rdo570_2.CheckedChanged
        '-----------------------------------------------------
        '       570 Checked
        '       
        '-----------------------------------------------------

        If Me.rdo570_2.Checked = True Then
            'TSL-570
            Me.rdo_tsltcpip2.Enabled = True
            Me.rdo_tslusb2.Enabled = True
        Else
            'TSL-550/710
            ' There can control only GPIB
            Me.rdo_tslusb2.Enabled = False
            Me.rdo_tsltcpip2.Enabled = False
            Me.rdotsl_gpib2.Checked = True
        End If

    End Sub

    Private Sub rdo570_3_CheckedChanged(sender As Object, e As EventArgs) Handles rdo570_3.CheckedChanged
        '-----------------------------------------------------
        '       570 Checked
        '       
        '-----------------------------------------------------

        If Me.rdo570_3.Checked = True Then
            'TSL-570
            Me.rdo_tsltcpip3.Enabled = True
            Me.rdo_tslusb3.Enabled = True
        Else
            'TSL-550/710
            ' There can control only GPIB
            Me.rdo_tslusb3.Enabled = False
            Me.rdo_tsltcpip3.Enabled = False
            Me.rdotsl_gpib3.Checked = True
        End If

    End Sub

    Private Sub rdo570_4_CheckedChanged(sender As Object, e As EventArgs) Handles rdo570_4.CheckedChanged
        '-----------------------------------------------------
        '       570 Checked
        '       
        '-----------------------------------------------------

        If Me.rdo570_4.Checked = True Then
            'TSL-570
            Me.rdo_tsltcpip4.Enabled = True
            Me.rdo_tslusb4.Enabled = True
        Else
            'TSL-550/710
            ' There can control only GPIB
            Me.rdo_tslusb4.Enabled = False
            Me.rdo_tsltcpip4.Enabled = False
            Me.rdotsl_gpib4.Checked = True
        End If

    End Sub

    Private Sub rdotsl_gpib1_CheckedChanged(sender As Object, e As EventArgs) Handles rdotsl_gpib1.CheckedChanged
        '--------------------------------------------------------------------
        '           TSL Control GPIB
        '--------------------------------------------------------------------

        If Me.rdotsl_gpib1.Checked = True Then
            Me.txttsl_gpibadd1.Enabled = True
            Me.txttsl_ipadd1.Enabled = False
            Me.txttsl_lanport1.Enabled = False
            Me.cmbtsl_usb1.Enabled = False
        Else
            Me.txttsl_gpibadd1.Enabled = False
            Me.txttsl_ipadd1.Enabled = True
            Me.txttsl_lanport1.Enabled = True
            Me.cmbtsl_usb1.Enabled = True

        End If

    End Sub

    Private Sub rdotsl_gpib2_CheckedChanged(sender As Object, e As EventArgs) Handles rdotsl_gpib2.CheckedChanged
        '--------------------------------------------------------------------
        '           TSL Control GPIB
        '--------------------------------------------------------------------

        If Me.rdotsl_gpib2.Checked = True Then
            Me.txttsl_gpibadd2.Enabled = True
            Me.txttsl_ipadd2.Enabled = False
            Me.txttsl_lanport2.Enabled = False
            Me.cmbtsl_usb2.Enabled = False
        Else
            Me.txttsl_gpibadd2.Enabled = False
            Me.txttsl_ipadd2.Enabled = True
            Me.txttsl_lanport2.Enabled = True
            Me.cmbtsl_usb2.Enabled = True

        End If

    End Sub

    Private Sub rdotsl_gpib3_CheckedChanged(sender As Object, e As EventArgs) Handles rdotsl_gpib3.CheckedChanged
        '--------------------------------------------------------------------
        '           TSL Control GPIB
        '--------------------------------------------------------------------

        If Me.rdotsl_gpib3.Checked = True Then
            Me.txttsl_gpibadd3.Enabled = True
            Me.txttsl_ipadd3.Enabled = False
            Me.txttsl_lanport3.Enabled = False
            Me.cmbtsl_usb3.Enabled = False
        Else
            Me.txttsl_gpibadd3.Enabled = False
            Me.txttsl_ipadd3.Enabled = True
            Me.txttsl_lanport3.Enabled = True
            Me.cmbtsl_usb3.Enabled = True

        End If

    End Sub

    Private Sub rdotsl_gpib4_CheckedChanged(sender As Object, e As EventArgs) Handles rdotsl_gpib4.CheckedChanged
        '--------------------------------------------------------------------
        '           TSL Control GPIB
        '--------------------------------------------------------------------

        If Me.rdotsl_gpib4.Checked = True Then
            Me.txttsl_gpibadd4.Enabled = True
            Me.txttsl_ipadd4.Enabled = False
            Me.txttsl_lanport4.Enabled = False
            Me.cmbtsl_usb4.Enabled = False
        Else
            Me.txttsl_gpibadd4.Enabled = False
            Me.txttsl_ipadd4.Enabled = True
            Me.txttsl_lanport4.Enabled = True
            Me.cmbtsl_usb4.Enabled = True

        End If

    End Sub

    Private Sub rdo_tsltcpip1_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tsltcpip1.CheckedChanged
        '------------------------------------------------------
        '       TSL Control TCP/IP
        '------------------------------------------------------

        If Me.rdo_tsltcpip1.Checked = True Then
            Me.txttsl_gpibadd1.Enabled = False
            Me.txttsl_ipadd1.Enabled = True
            Me.txttsl_lanport1.Enabled = True
            Me.cmbtsl_usb1.Enabled = False

        End If

    End Sub

    Private Sub rdo_tsltcpip2_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tsltcpip2.CheckedChanged
        '------------------------------------------------------
        '       TSL Control TCP/IP
        '------------------------------------------------------

        If Me.rdo_tsltcpip2.Checked = True Then
            Me.txttsl_gpibadd2.Enabled = False
            Me.txttsl_ipadd2.Enabled = True
            Me.txttsl_lanport2.Enabled = True
            Me.cmbtsl_usb2.Enabled = False

        End If

    End Sub

    Private Sub rdo_tsltcpip3_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tsltcpip3.CheckedChanged
        '------------------------------------------------------
        '       TSL Control TCP/IP
        '------------------------------------------------------

        If Me.rdo_tsltcpip3.Checked = True Then
            Me.txttsl_gpibadd3.Enabled = False
            Me.txttsl_ipadd3.Enabled = True
            Me.txttsl_lanport3.Enabled = True
            Me.cmbtsl_usb3.Enabled = False

        End If

    End Sub

    Private Sub rdo_tsltcpip4_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tsltcpip4.CheckedChanged
        '------------------------------------------------------
        '       TSL Control TCP/IP
        '------------------------------------------------------

        If Me.rdo_tsltcpip4.Checked = True Then
            Me.txttsl_gpibadd4.Enabled = False
            Me.txttsl_ipadd4.Enabled = True
            Me.txttsl_lanport4.Enabled = True
            Me.cmbtsl_usb4.Enabled = False

        End If

    End Sub

    Private Sub rdo_tslusb1_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tslusb1.CheckedChanged
        '------------------------------------------------------
        '       TSL Control USB
        '------------------------------------------------------

        If Me.rdo_tslusb1.Checked = True Then
            Me.txttsl_gpibadd1.Enabled = False
            Me.txttsl_ipadd1.Enabled = False
            Me.txttsl_lanport1.Enabled = False
            Me.cmbtsl_usb1.Enabled = True

        End If

    End Sub

    Private Sub rdo_tslusb2_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tslusb2.CheckedChanged
        '------------------------------------------------------
        '       TSL Control USB
        '------------------------------------------------------

        If Me.rdo_tslusb2.Checked = True Then
            Me.txttsl_gpibadd2.Enabled = False
            Me.txttsl_ipadd2.Enabled = False
            Me.txttsl_lanport2.Enabled = False
            Me.cmbtsl_usb2.Enabled = True

        End If

    End Sub

    Private Sub rdo_tslusb3_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tslusb3.CheckedChanged
        '------------------------------------------------------
        '       TSL Control USB
        '------------------------------------------------------

        If Me.rdo_tslusb3.Checked = True Then
            Me.txttsl_gpibadd3.Enabled = False
            Me.txttsl_ipadd3.Enabled = False
            Me.txttsl_lanport3.Enabled = False
            Me.cmbtsl_usb3.Enabled = True

        End If

    End Sub

    Private Sub rdo_tslusb4_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_tslusb4.CheckedChanged
        '------------------------------------------------------
        '       TSL Control USB
        '------------------------------------------------------

        If Me.rdo_tslusb4.Checked = True Then
            Me.txttsl_gpibadd4.Enabled = False
            Me.txttsl_ipadd4.Enabled = False
            Me.txttsl_lanport4.Enabled = False
            Me.cmbtsl_usb4.Enabled = True

        End If

    End Sub

    Private Sub rdo110_CheckedChanged(sender As Object, e As EventArgs) Handles rdo110.CheckedChanged
        '-----------------------------------------------------
        '       110 Checked
        '       
        '-----------------------------------------------------

        If Me.rdo110.Checked = True Then
            'OSU-110
            Me.rdo_osugpib.Enabled = True
            Me.rdo_osutcpip.Enabled = True
            Me.rdo_osuusb.Enabled = True
            Me.rdo_osugpib.Checked = True
            Me.txtosu_gpibadd.Enabled = True
            Me.cmbosu_dev.Enabled = False

            Me.cmbspu_dev.Enabled = True
            Me.cmbspu_dev.SelectedIndex = -1
            Me.cmbosu_dev.SelectedIndex = -1
        Else
            'OSU-100
            Me.rdo_osugpib.Enabled = False
            Me.rdo_osutcpip.Enabled = False
            Me.rdo_osuusb.Enabled = False
            Me.txtosu_gpibadd.Enabled = False
            Me.txtosu_ipadd.Enabled = False
            Me.txtosu_lanport.Enabled = False
            Me.cmbosu_usb.Enabled = False
            Me.cmbosu_dev.Enabled = True

            Me.cmbspu_dev.Enabled = False
            Me.cmbspu_dev.SelectedIndex = -1
        End If

    End Sub

    Private Sub rdo_osugpib_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_osugpib.CheckedChanged
        '--------------------------------------------------------------------
        '           OSU Control GPIB
        '--------------------------------------------------------------------

        If Me.rdo_osugpib.Checked = True Then
            Me.txtosu_gpibadd.Enabled = True
            Me.txtosu_ipadd.Enabled = False
            Me.txtosu_lanport.Enabled = False
            Me.cmbosu_usb.Enabled = False
        Else
            Me.txtosu_gpibadd.Enabled = False
            Me.txtosu_ipadd.Enabled = True
            Me.txtosu_lanport.Enabled = True
            Me.cmbosu_usb.Enabled = True

        End If

    End Sub

    Private Sub rdo_osutcpip_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_osutcpip.CheckedChanged
        '------------------------------------------------------
        '       OSU Control TCP/IP
        '------------------------------------------------------

        If Me.rdo_osutcpip.Checked = True Then
            Me.txtosu_gpibadd.Enabled = False
            Me.txtosu_ipadd.Enabled = True
            Me.txtosu_lanport.Enabled = True
            Me.cmbosu_usb.Enabled = False

        End If

    End Sub

    Private Sub rdo_osuusb_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_osuusb.CheckedChanged
        '------------------------------------------------------
        '       OSU Control USB
        '------------------------------------------------------

        If Me.rdo_osuusb.Checked = True Then
            Me.txtosu_gpibadd.Enabled = False
            Me.txtosu_ipadd.Enabled = False
            Me.txtosu_lanport.Enabled = False
            Me.cmbosu_usb.Enabled = True

        End If

    End Sub

    Private Sub cmbosu_dev_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbosu_dev.SelectedIndexChanged
        '------------------------------------------------------
        '       Deviece resource
        '------------------------------------------------------

        Me.cmbspu_dev.SelectedIndex = Me.cmbosu_dev.SelectedIndex

    End Sub

    Private Sub chkmulti_dev_CheckedChanged(sender As Object, e As EventArgs) Handles chkmulti_dev.CheckedChanged
        '------------------------------------------------------------------
        '           Selecet MPM Multi-Device
        '-------------------------------------------------------------------

        Me.grp_mpmdev2.Enabled = Me.chkmulti_dev.Checked

    End Sub

    Private Sub rdo_mpmgpib_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_mpmgpib.CheckedChanged
        '--------------------------------------------------------------------
        '           MPM Control GPIB
        '--------------------------------------------------------------------

        If Me.rdo_mpmgpib.Checked = True Then
            Me.txtmpm_gpibadd1.Enabled = True
            Me.txtmpm_ipadd1.Enabled = False
            Me.txtmpm_lanport1.Enabled = False

            Me.txtmpm_gpibadd2.Enabled = True
            Me.txtmpm_ipadd2.Enabled = False
            Me.txtmpm_lanport2.Enabled = False
        Else
            Me.txtmpm_gpibadd1.Enabled = False
            Me.txtmpm_ipadd1.Enabled = True
            Me.txtmpm_lanport1.Enabled = True

            Me.txtmpm_gpibadd2.Enabled = False
            Me.txtmpm_ipadd2.Enabled = True
            Me.txtmpm_lanport2.Enabled = True

        End If

    End Sub

    Private Sub btnconnect_Click(sender As Object, e As EventArgs) Handles btnconnect.Click

        '----------------------------------------------------------
        '           Connect       
        '----------------------------------------------------------


        ' ----TSL Communication diteal

        TSL_Count = 0

        If Me.chktsl_dev1.Checked = True Then

            '-----Device1

            'GPIB Communcation?
            If Me.rdotsl_gpib1.Checked = True Then

                TSL_Communicater(TSL_Count) = "GPIB"
                TSL_Address(TSL_Count) = Me.txttsl_gpibadd1.Text
                TSL_Portnumber(TSL_Count) = 0
            End If

            'TCP/IP Communciation?
            If Me.rdo_tsltcpip1.Checked = True Then
                TSL_Communicater(TSL_Count) = "LAN"
                TSL_Address(TSL_Count) = Me.txttsl_ipadd1.Text
                TSL_Portnumber(TSL_Count) = Me.txttsl_lanport1.Text
            End If

            'USB Communcation?
            If Me.rdo_tslusb1.Checked = True Then

                If Me.cmbtsl_usb1.Text = "" Then
                    MsgBox("Please enter to the TSL USB Resource", vbOKOnly)
                    Exit Sub
                End If

                TSL_Communicater(TSL_Count) = "USB"
                TSL_Address(TSL_Count) = Me.cmbtsl_usb1.SelectedIndex
                TSL_Portnumber(TSL_Count) = 0
            End If

            ReDim Preserve TSL_OSUport(TSL_Count + 1)
            TSL_OSUport(TSL_Count) = 1
            TSL_Count = TSL_Count + 1

        End If

        If Me.chktsl_dev2.Checked = True Then

            '-----Device2

            'GPIB Communcation?
            If Me.rdotsl_gpib2.Checked = True Then

                TSL_Communicater(TSL_Count) = "GPIB"
                TSL_Address(TSL_Count) = Me.txttsl_gpibadd2.Text
                TSL_Portnumber(TSL_Count) = 0
            End If

            'TCP/IP Communciation?
            If Me.rdo_tsltcpip2.Checked = True Then
                TSL_Communicater(TSL_Count) = "LAN"
                TSL_Address(TSL_Count) = Me.txttsl_ipadd2.Text
                TSL_Portnumber(TSL_Count) = Me.txttsl_lanport2.Text
            End If

            'USB Communcation?
            If Me.rdo_tslusb2.Checked = True Then

                If Me.cmbtsl_usb2.Text = "" Then
                    MsgBox("Please enter to the TSL USB Resource", vbOKOnly)
                    Exit Sub
                End If

                TSL_Communicater(TSL_Count) = "USB"
                TSL_Address(TSL_Count) = Me.cmbtsl_usb2.SelectedIndex
                TSL_Portnumber(TSL_Count) = 0
            End If

            ReDim Preserve TSL_OSUport(TSL_Count + 1)
            TSL_OSUport(TSL_Count) = 2
            TSL_Count = TSL_Count + 1
        End If

        If Me.chktsl_dev3.Checked = True Then

            '-----Device3

            'GPIB Communcation?
            If Me.rdotsl_gpib3.Checked = True Then

                TSL_Communicater(TSL_Count) = "GPIB"
                TSL_Address(TSL_Count) = Me.txttsl_gpibadd3.Text
                TSL_Portnumber(TSL_Count) = 0
            End If

            'TCP/IP Communciation?
            If Me.rdo_tsltcpip3.Checked = True Then
                TSL_Communicater(TSL_Count) = "LAN"
                TSL_Address(TSL_Count) = Me.txttsl_ipadd3.Text
                TSL_Portnumber(TSL_Count) = Me.txttsl_lanport3.Text
            End If

            'USB Communcation?
            If Me.rdo_tslusb3.Checked = True Then

                If Me.cmbtsl_usb3.Text = "" Then
                    MsgBox("Please enter to the TSL USB Resource", vbOKOnly)
                    Exit Sub
                End If

                TSL_Communicater(TSL_Count) = "USB"
                TSL_Address(TSL_Count) = Me.cmbtsl_usb3.SelectedIndex
                TSL_Portnumber(TSL_Count) = 0
            End If

            ReDim Preserve TSL_OSUport(TSL_Count + 1)
            TSL_OSUport(TSL_Count) = 3
            TSL_Count = TSL_Count + 1
        End If

        If Me.chktsl_dev4.Checked = True Then

            '-----Device4

            'GPIB Communcation?
            If Me.rdotsl_gpib4.Checked = True Then

                TSL_Communicater(TSL_Count) = "GPIB"
                TSL_Address(TSL_Count) = Me.txttsl_gpibadd4.Text
                TSL_Portnumber(TSL_Count) = 0
            End If

            'TCP/IP Communciation?
            If Me.rdo_tsltcpip4.Checked = True Then
                TSL_Communicater(TSL_Count) = "LAN"
                TSL_Address(TSL_Count) = Me.txttsl_ipadd4.Text
                TSL_Portnumber(TSL_Count) = Me.txttsl_lanport4.Text
            End If

            'USB Communcation?
            If Me.rdo_tslusb4.Checked = True Then

                If Me.cmbtsl_usb4.Text = "" Then
                    MsgBox("Please enter to the TSL USB Resource", vbOKOnly)
                    Exit Sub
                End If

                TSL_Communicater(TSL_Count) = "USB"
                TSL_Address(TSL_Count) = Me.cmbtsl_usb4.SelectedIndex
                TSL_Portnumber(TSL_Count) = 0
            End If

            ReDim Preserve TSL_OSUport(TSL_Count + 1)
            TSL_OSUport(TSL_Count) = 4
            TSL_Count = TSL_Count + 1
        End If

        If TSL_Count = 0 Then
            MsgBox("Please enter to the SPU device resouce", vbOKOnly)
            Exit Sub
        End If

        '-----OSU Communcation diteal

        If rdo110.Checked Then
            'OSU-110

            'GPIB Communcation?
            If Me.rdo_osugpib.Checked = True Then

                OSU_Communicater = "GPIB"
                OSU_Address = Me.txtosu_gpibadd.Text
                OSU_Portnumber = 0
            End If

            'TCP/IP Communciation?
            If Me.rdo_osutcpip.Checked = True Then
                OSU_Communicater = "LAN"
                OSU_Address = Me.txtosu_ipadd.Text
                OSU_Portnumber = Me.txtosu_lanport.Text
            End If

            'USB Communcation?
            If Me.rdo_osuusb.Checked = True Then

                If Me.cmbosu_usb.Text = "" Then
                    MsgBox("Please enter to the OSU USB Resource", vbOKOnly)
                    Exit Sub
                End If

                OSU_Communicater = "USB"
                OSU_Address = Me.cmbosu_usb.SelectedIndex
                OSU_Portnumber = 0
            End If

            OSU_DeviveID = ""
            Flag_OSU_100 = False

        Else
            'OSU-100

            If Me.cmbosu_dev.Text = "" Then
                MsgBox("Please enter to the OSU device Resource", vbOKOnly)
                Exit Sub
            End If

            OSU_DeviveID = Me.cmbosu_dev.Text
            OSU_Communicater = ""
            OSU_Address = ""
            OSU_Portnumber = 0
            Flag_OSU_100 = True

        End If

        '-----MPM Communcation diteal

        'Multi Device?
        If Me.chkmulti_dev.Checked = True Then
            MPM_Count = 2

            'GPIB communcation?
            If Me.rdo_mpmgpib.Checked = True Then
                MPM_Address(0) = Me.txtmpm_gpibadd1.Text
                MPM_Address(1) = Me.txtmpm_gpibadd2.Text
                MPM_Portnumber(0) = 0
                MPM_Portnumber(1) = 0

                MPM_Communicater = "GPIB"
            Else
                ' TCL/IP communcation?
                MPM_Address(0) = Me.txtmpm_ipadd1.Text
                MPM_Address(1) = Me.txtmpm_ipadd2.Text
                MPM_Portnumber(0) = Me.txtmpm_lanport1.Text
                MPM_Portnumber(1) = Me.txtmpm_lanport2.Text

                MPM_Communicater = "LAN"
            End If
        Else
            MPM_Count = 1

            'GPIB communcation?
            If Me.rdo_mpmgpib.Checked = True Then
                MPM_Address(0) = Me.txtmpm_gpibadd1.Text
                MPM_Address(1) = ""
                MPM_Portnumber(0) = 0
                MPM_Portnumber(1) = 0

                MPM_Communicater = "GPIB"
            Else
                ' TCL/IP communcation?
                MPM_Address(0) = Me.txtmpm_ipadd1.Text
                MPM_Address(1) = ""
                MPM_Portnumber(0) = Me.txtmpm_lanport1.Text
                MPM_Portnumber(1) = 0

                MPM_Communicater = "LAN"
            End If
        End If


        ' SPU Resouce
        If Me.cmbspu_dev.Text = "" Then
            MsgBox("Please enter to the SPU device resouce", vbOKOnly)
            Exit Sub
        End If

        SPU_DeviveID = Me.cmbspu_dev.Text

        Me.Dispose()

    End Sub

End Class
