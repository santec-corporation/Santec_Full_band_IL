Imports Santec                                                               'import Instrument DLL namespace
Imports Santec.STSProcess                                                    'import STSProcess DLL namespace

Public Class Form1
    Private TSL() As TSL                                                     'TSL control class
    Private OSU As New OSU                                                   'OSU control class
    Private MPM() As MPM                                                     'MPM control class
    Private SPU As New SPU                                                   'SPU control class
    Private Cal_STS() As ILSTS                                               'STS calucrate class
    Private Data_struct As New List(Of STSDataStruct)                        'STS data Struct 
    Private Refdata_struct As New List(Of STSDataStruct)                     'Reference data Struct
    Private Meas_monitor_struct As New List(Of STSMonitorStruct)             'Measurement monitor data struct
    Private Ref_monitordata_struct As New List(Of STSDataStruct)             'STS Monitor data Struct for Reference
    Private Mergedata_struct As New List(Of STSDataStructForMerge)           'Data struct for merge  
    Private Meas_rang As New List(Of Integer)                                'Measurement Range
    Private TSL_OSUport() As Integer                                         'Combination of TSL and OSU port
    Private Flag_213 As Boolean                                              'Exist 213 flag      T: Exist F: nothing
    Private Flag_215 As Boolean                                              'Exist 215 flag      T: Exist F: nothing
    Private Flag_100 As Boolean                                              'Using OSU-100
    Private TSL_minspeed As Double = -9999                                   'TSL Min Sweep Speed(nm/sec)
    Private TSL_maxspeed As Double = 9999　　　　　　　　　　　　　　     　 'TSL Max Sweep Speed(nm/sec)
    Private TSL_minpower As Double = -9999                                   'TSL Min APC Power(dBm)
    Private TSL_maxpower As Double = 9999　　　　　　　　　　　　　　     　 'TSL Max APC Power(dBm)
    Private TSL_sweepstartwave As Double                                     'TSL Sweep Start Wavelength(nm)
    Private TSL_sweepstopwave As Double                                      'TSL Sweep Stop Wavelength(nm)
    Private Counter_570 As Integer                                           'TSL-570 Counter

    Structure TSLSweepItem                                                   'Sweep items for each TSL
        Public specminwave As Double                                         'TSL Spec Min Wavelenght(nm)
        Public specmaxwave As Double                                         'TSL Spec Max Wavelenght(nm)
        Public startwave As Double                                           'TSL Sweep Start Wavelength(nm)
        Public stopwave As Double                                            'TSL Sweep Stop Wavelength(nm)
        Public speed As Double                                               'TSL Sweep Speed(nm/sec)
        Public power As Single                                               'TSL APC Power(dBm)
        Public acctualstep As Double                                         'TSL output trigger step(nm)
        Public wavestep As Double                                            'STS wavelenthg step(nm)
    End Structure


    Private SPU_Sampling_timeDictionary As New Dictionary(Of TSL, Double)    'SPU sampling time Dictionary
    Private TSL_ProductNameDictionary As New Dictionary(Of TSL, String)      'TSL ProductName Dictionary
    Private TSL_SweepItemDictionary As New Dictionary(Of TSL, TSLSweepItem)  'TSL_SweepItem Dictionary


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '-------------------------------------------------------------------------
        '       Form Load    (MainForm)
        '-------------------------------------------------------------------------
        Dim spudev() As String = Nothing                        'SPU device name
        Dim errorcode As Integer                                'errorcode 
        Dim usb_resource() As String = Nothing                  'usb communication resource


        '----Check Connction of spu deviece
        errorcode = SPU.Get_Device_ID(spudev)

        If errorcode <> 0 Then
            Show_Inst_Error(errorcode)
            End
        End If


        '----Check usb resource
        usb_resource = Communication.MainCommunication.Get_USB_Resouce()


        '----show Setting Form
        Dim set_form As New Instrument_Setting

        set_form.Owner = Me
        set_form.SPU_Resource = spudev
        set_form.USB_Resource = usb_resource
        set_form.ShowDialog()

        '----Apply to communication parametere from Instrument setting form
        Dim tsl_communcation_method() As Communication.CommunicationMethod
        Dim osu_communcation_method As Communication.CommunicationMethod
        Dim mpm_communcation_method As Communication.CommunicationMethod


        '----TSL　Communication method

        TSL_OSUport = set_form.TSL_OSUport

        Dim loop1 As Integer
        Dim tslcount As Integer                     'TSL count

        tslcount = set_form.TSL_Count
        ReDim TSL(tslcount - 1)
        ReDim tsl_communcation_method(tslcount - 1)

        For loop1 = 0 To tslcount - 1

            TSL(loop1) = New TSL

            If set_form.TSL_Communicater(loop1) = "GPIB" Then

                tsl_communcation_method(loop1) = Communication.CommunicationMethod.GPIB
                TSL(loop1).Terminator = CommunicationTerminator.CrLf
                TSL(loop1).GPIBAddress = set_form.TSL_Address(loop1)
                TSL(loop1).GPIBBoard = 0
                TSL(loop1).GPIBConnectType = Communication.GPIBConnectType.NI4882

            ElseIf set_form.TSL_Communicater(loop1) = "LAN" Then

                tsl_communcation_method(loop1) = Communication.CommunicationMethod.TCPIP
                TSL(loop1).Terminator = CommunicationTerminator.Cr
                TSL(loop1).IPAddress = set_form.TSL_Address(loop1)
                TSL(loop1).Port = set_form.TSL_Portnumber(loop1)
            Else
                'USB 
                tsl_communcation_method(loop1) = Communication.CommunicationMethod.USB
                TSL(loop1).DeviceID = CInt(set_form.TSL_Address(loop1))
                TSL(loop1).Terminator = CommunicationTerminator.Cr
            End If

        Next

        '----OSU Communicatipon method

        '----Using OSU-100
        If set_form.Flag_OSU_100 = True Then
            Flag_100 = True
        End If

        If Flag_100 = False Then
            'OSU-110
            If set_form.OSU_Communicater = "GPIB" Then

                osu_communcation_method = Communication.CommunicationMethod.GPIB
                OSU.Terminator = CommunicationTerminator.Cr
                OSU.GPIBAddress = set_form.OSU_Address

            ElseIf set_form.OSU_Communicater = "LAN" Then

                osu_communcation_method = Communication.CommunicationMethod.TCPIP
                OSU.Terminator = CommunicationTerminator.Cr
                OSU.IPAddress = set_form.OSU_Address
                OSU.Port = set_form.OSU_Portnumber
            Else
                'USB 
                osu_communcation_method = Communication.CommunicationMethod.USB
                OSU.DeviceID = CInt(set_form.OSU_Address)
                OSU.Terminator = CommunicationTerminator.Cr
            End If
        Else
            'OSU-100
            OSU.DeviceName = set_form.OSU_DeviveID
        End If


        '----MPM Communicatipon method

        Dim mpmcount As Integer                     'MPM count

        If set_form.MPM_Communicater = "GPIB" Then
            mpm_communcation_method = Communication.CommunicationMethod.GPIB
        Else
            mpm_communcation_method = Communication.CommunicationMethod.TCPIP
        End If

        mpmcount = set_form.MPM_Count
        ReDim MPM(mpmcount - 1)

        For loop1 = 0 To mpmcount - 1

            MPM(loop1) = New MPM

            If set_form.MPM_Communicater = "GPIB" Then
                MPM(loop1).GPIBAddress = set_form.MPM_Address(loop1)
            Else
                MPM(loop1).IPAddress = set_form.MPM_Address(loop1)
                MPM(loop1).Port = set_form.MPM_Portnumber(loop1)
            End If

            '-------------------------------------------------------------------------
            '  MPM muximum logging data read time is 11s
            '  communication time out must to set > mpm logging data read time.
            '--------------------------------------------------------------------------
            MPM(loop1).TimeOut = 11000

        Next


        '----SPU Communcation Setting 
        SPU.DeviceName = set_form.SPU_DeviveID


        '----Connect
        'TSL
        For loop1 = 0 To UBound(TSL)
            errorcode = TSL(loop1).Connect(tsl_communcation_method(loop1))

            If errorcode <> 0 Then
                MsgBox("TSL can't connect.Please check connection.", vbOKOnly)
                End
            End If
        Next


        'OSU
        If Flag_100 = False Then
            'OSU-110
            errorcode = OSU.Connect(osu_communcation_method)
        Else
            'OSU-100
            errorcode = OSU.Connect()
        End If

        If errorcode <> 0 Then
            MsgBox("OSU can't connect.Please check connection.", vbOKOnly)
            End
        End If


        'MPM
        For loop1 = 0 To UBound(MPM)
            errorcode = MPM(loop1).Connect(mpm_communcation_method)

            If errorcode <> 0 Then
                MsgBox("MPM can't connect.Please check connection.", vbOKOnly)
                End
            End If
        Next

        If errorcode <> 0 Then
            MsgBox("MPM can't connect.Please check connection.", vbOKOnly)
            End
        End If

        'SPU(DAQ)
        Dim ans As String = String.Empty
        errorcode = SPU.Connect(ans)

        If errorcode <> 0 Then
            MsgBox("SPU Can't connect. Please check connection.", vbOKOnly)
            End
        End If


        '----Check MPM Module information
        errorcode = Check_Module_Information()

        If errorcode <> 0 Then
            MsgBox("System can't use MPM-215 togeter other module", vbOKOnly)
            End
        End If

        '----Reflect instrument parameter to Form
        Referect_EnableCh_for_form()                           'MPM Eanble ch
        Referect_EnableRange_for_form()                        'MPM selectable range

        errorcode = Add_TSL_SpecWavelength()                   'TSL Spec Wavelength

        If errorcode <> 0 Then
            Show_Inst_Error(errorcode)
            Exit Sub
        End If

        errorcode = Add_TSL_Sweep_Speed()                      'TSL Sweep speed(only TSL-570)

        If errorcode <> 0 Then
            Show_Inst_Error(errorcode)
            Exit Sub
        End If

        Get_TSL_Sweep_MinSpeed()                               'TSL Min Sweep speed

        errorcode = Get_TSL_APC_MaxPower(TSL_maxpower)         'TSL APC Max Power

        If errorcode <> 0 Then
            Show_Inst_Error(errorcode)
            Exit Sub
        End If

        Get_TSL_APC_MinPower()                                 'TSL APC Min Power

        '----Check fiber connection between TSL and OSU
        errorcode = Check_Fiber_Connection()

        If errorcode <> 0 Then
            MsgBox("Check the fiber connection between TSL and OSU", vbOKOnly)
            End
        End If

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        '-------------------------------------------------------------------------
        '           TabControl SelectedIndexChanged
        '--------------------------------------------------------------------------
        Dim selectedIndex As Integer = TabControl1.SelectedIndex

        If selectedIndex = 0 Then
            groupbox_tsl.Enabled = True
        Else
            groupbox_tsl.Enabled = False
        End If

    End Sub

    Private Function Add_TSL_SpecWavelength() As Integer
        '---------------------------------------------------------
        '       Add the spec wavelength to Dictionary
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim inst_error As Integer                       'instullment error
        Dim specminwave As Double                       'Spec WavelengthMin(nm)
        Dim specmaxwave As Double                       'Spec WavelengthMax(nm)
        Dim minwave As Double = 9999                    'WavelengthMin(nm)
        Dim maxwave As Double = 0                       'WavelengthMax(nm)
        Dim productname As String                       'ProductName

        'Dictionary clear 
        TSL_ProductNameDictionary.Clear()               'TSL ProductName
        TSL_SweepItemDictionary.Clear()                 'TSL Sweepitem

        For loop1 = 0 To UBound(TSL)

            Dim item As New TSLSweepItem

            'Spec Wavelength
            inst_error = TSL(loop1).Get_Spec_Wavelength(specminwave, specmaxwave)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Return inst_error
            End If

            'TSL ProductName
            productname = TSL(loop1).Information.ProductName

            item.specminwave = specminwave
            item.specmaxwave = specmaxwave

            TSL_ProductNameDictionary.Add(TSL(loop1), productname)
            TSL_SweepItemDictionary.Add(TSL(loop1), item)

            If minwave > specminwave Then
                minwave = specminwave
            End If

            If maxwave < specmaxwave Then
                maxwave = specmaxwave
            End If

            Select Case TSL_OSUport(loop1)
                Case 1
                    'port1
                    Me.txtstartwave1.Enabled = True
                    Me.txtstopwave1.Enabled = True
                    Me.txtstartwave1.Text = specminwave
                    Me.txtstopwave1.Text = specmaxwave
                    Me.txtspecminwave1.Text = specminwave
                    Me.txtspecmaxwave1.Text = specmaxwave
                Case 2
                    'port2
                    Me.txtstartwave2.Enabled = True
                    Me.txtstopwave2.Enabled = True
                    Me.txtstartwave2.Text = specminwave
                    Me.txtstopwave2.Text = specmaxwave
                    Me.txtspecminwave2.Text = specminwave
                    Me.txtspecmaxwave2.Text = specmaxwave
                Case 3
                    'port3
                    Me.txtstartwave3.Enabled = True
                    Me.txtstopwave3.Enabled = True
                    Me.txtstartwave3.Text = specminwave
                    Me.txtstopwave3.Text = specmaxwave
                    Me.txtspecminwave3.Text = specminwave
                    Me.txtspecmaxwave3.Text = specmaxwave
                Case 4
                    'port4
                    Me.txtstartwave4.Enabled = True
                    Me.txtstopwave4.Enabled = True
                    Me.txtstartwave4.Text = specminwave
                    Me.txtstopwave4.Text = specmaxwave
                    Me.txtspecminwave4.Text = specminwave
                    Me.txtspecmaxwave4.Text = specmaxwave
            End Select
        Next

        Me.txtstartwave.Text = minwave
        Me.txtstopwave.Text = maxwave

        TSL_sweepstartwave = minwave
        TSL_sweepstopwave = maxwave

        Return 0

    End Function

    Private Function Add_TSL_Sweep_Speed() As Integer
        '---------------------------------------------------------
        '       Add in selectable sweep speed to speed combbox
        '       this function can use only TSL-570
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim inst_error As Integer                       'instullment error
        Dim sweep_table() As Double = Nothing           'table
        Dim before_sweep_table() As Double = Nothing    'table
        Dim common_sweep_table() As Double = Nothing    'table
        Dim productname As String                       'ProductName

        'get max sweep speed for spec wavelength
        inst_error = Get_TSL_Sweep_MaxSpeed(TSL_maxspeed)
        If inst_error <> 0 Then
            Return inst_error
        End If

        'Get Sweep speed tabele
        'Except for TSL-570 "Device Error" occurre when call this function.

        For loop1 = 0 To UBound(TSL)

            productname = TSL_ProductNameDictionary(TSL(loop1))

            If productname.IndexOf("570") > -1 Then
                Counter_570 = Counter_570 + 1

                inst_error = TSL(loop1).Get_Sweep_Speed_table(sweep_table)
                If inst_error <> 0 Then
                    Return inst_error
                End If

                If Counter_570 > 1 Then
                    'merging of table data
                    common_sweep_table = sweep_table.Intersect(before_sweep_table).ToArray()
                    before_sweep_table = common_sweep_table
                Else
                    before_sweep_table = sweep_table
                End If

            Else
                Continue For
            End If

        Next

        If Counter_570 = 1 Then
            common_sweep_table = before_sweep_table
        End If

        '----Add in combbox when TSL-570
        If common_sweep_table Is Nothing = False Then

            For loop1 = 0 To UBound(common_sweep_table)

                If common_sweep_table(loop1) > TSL_maxspeed Then
                    ReDim Preserve common_sweep_table(loop1 - 1)
                    Exit For
                End If
            Next

            For loop1 = 0 To UBound(common_sweep_table)
                Me.cmbspeed.Items.Add(common_sweep_table(loop1))
            Next
        End If

        Return 0

    End Function

    Private Function Get_TSL_Sweep_MaxSpeed(ByRef maxspeed As Double) As Integer
        '---------------------------------------------------------
        '       get the max sweep speed for TSL
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim inst_error As Integer           'instullment error
        Dim speed As Double                 'Sweep Speed(nm/sec)
        Dim specminwave As Double           'Spec WavelengthMin(nm)
        Dim specmaxwave As Double           'Spec WavelengthMax(nm)

        For loop1 = 0 To UBound(TSL)

            specminwave = TSL(loop1).Information.MinimunWavelength
            specmaxwave = TSL(loop1).Information.MaximumWavelength

            'get the max sweep speed for Wavelength
            inst_error = TSL(loop1).Get_Sweep_Speed_for_Wavelength(specminwave, specmaxwave, speed)
            If inst_error <> 0 Then
                Return inst_error
            End If

            If maxspeed > speed Then
                maxspeed = speed
            End If

        Next

        Return 0

    End Function

    Private Sub Get_TSL_Sweep_MinSpeed()
        '---------------------------------------------------------
        '       get the min sweep speed for TSL
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim speed As Double                 'Sweep Speed(nm/sec)

        For loop1 = 0 To UBound(TSL)

            speed = TSL(loop1).Information.MinimumSpeed

            If TSL_minspeed < speed Then
                TSL_minspeed = speed
            End If
        Next

    End Sub

    Private Function Get_TSL_APC_MaxPower(ByRef maxpower As Double) As Integer
        '---------------------------------------------------------
        '       get the APC max power for TSL
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim inst_error As Integer           'instullment error
        Dim productname As String           'ProductName
        Dim power As Double                 'APC Power(dBm)
        Dim specminwave As Double           'Spec WavelengthMin(nm)
        Dim specmaxwave As Double           'Spec WavelengthMax(nm)

        For loop1 = 0 To UBound(TSL)

            productname = TSL_ProductNameDictionary(TSL(loop1))

            If productname.IndexOf("570") > -1 Then
                specminwave = TSL(loop1).Information.MinimunWavelength
                specmaxwave = TSL(loop1).Information.MaximumWavelength

                ' get the APC max power for sweep
                inst_error = TSL(loop1).Get_APC_Limit_for_Sweep(specminwave, specmaxwave, power)
                If inst_error <> 0 Then
                    Return inst_error
                Else
                    If maxpower > power Then
                        maxpower = power
                    End If
                End If
            End If
        Next

        If Counter_570 = 0 Then
            maxpower = 10
        End If

        txtpower.Text = Math.Floor(maxpower)

        Return 0

    End Function

    Private Sub Get_TSL_APC_MinPower()
        '---------------------------------------------------------
        '       get the APC min power for TSL
        '----------------------------------------------------------
        Dim loop1 As Integer
        Dim power As Double                 'APC Power(dBm)

        For loop1 = 0 To UBound(TSL)

            power = TSL(loop1).Information.MinimumAPCPower_dBm

            If TSL_minpower < power Then
                TSL_minpower = power
            End If
        Next

    End Sub

    Private Function Check_Fiber_Connection()
        '---------------------------------------------------------
        '       check fiber connection between TSL and OSU
        '----------------------------------------------------------
        Dim inst_error As Integer                'instullment error
        Dim port As Integer                      'active port
        Dim ldstatus As Santec.TSL.LD_Status     'LD status

        Dim loop1 As Integer

        For loop1 = 0 To UBound(TSL)
            inst_error = TSL(loop1).Get_LD_Status(ldstatus)
            If inst_error <> 0 Then
                Return inst_error
            End If

            If ldstatus = Santec.TSL.LD_Status.LD_OFF Then
                MsgBox("Exit the application because the LD status is OFF", vbOKOnly)
                End
            End If

            'close the shutters of all TSL
            inst_error = TSL(loop1).Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Close)
            If inst_error <> 0 Then
                Return inst_error
            End If
        Next

        For loop1 = 0 To UBound(TSL)

            'open the shutter of the target TSL
            inst_error = TSL(loop1).Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Open)
            If inst_error <> 0 Then
                Return inst_error
            End If

            'check the optical input port
            inst_error = OSU.Get_Active_Port(port)
            If inst_error <> 0 Then
                Return inst_error
            End If

            If port <> TSL_OSUport(loop1) Then
                Return -1
            End If

            'close the shutter of the target TSL
            inst_error = TSL(loop1).Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Close)
            If inst_error <> 0 Then
                Return inst_error
            End If

        Next

        'open the shutters of all TSL
        For loop1 = 0 To UBound(TSL)

            inst_error = TSL(loop1).Set_Shutter_Status(Santec.TSL.Shutter_Status.Shutter_Open)
            If inst_error <> 0 Then
                Return inst_error
            End If
        Next

        Return 0

    End Function

    Private Function Check_Module_Information() As Integer
        '------------------------------------------------------------
        '       check Module information inside system
        '------------------------------------------------------------
        Dim loop1 As Integer
        Dim loop2 As Integer
        Dim counter_215 As Integer                           '215 counter 

        'MPM device loop
        For loop1 = 0 To UBound(MPM)
            'Slot loop
            For loop2 = 0 To 4
                'Enable Slot
                If MPM(loop1).Information.ModuleEnable(loop2) = True Then

                    'Check MPM-215 insert
                    If MPM(loop1).Information.ModuleType(loop2) = "MPM-215" Then
                        Flag_215 = True
                        counter_215 = counter_215 + 1
                    End If

                    'Check MPM-213 insert
                    If MPM(loop1).Information.ModuleType(loop2) = "MPM-213" Then
                        Flag_213 = True
                    End If

                End If
            Next

        Next


        'Check MPM-215 count & Module total count
        'STS system can't use 215 together other module.
        Dim enable_module_count As Integer                      'enable module count

        For loop1 = 0 To UBound(MPM)
            enable_module_count = MPM(loop1).Information.NumberofModule + enable_module_count       'total module count
        Next

        If Flag_215 = True Then
            If enable_module_count <> counter_215 Then
                Return -1
            End If
        End If
        Return 0

    End Function
    Private Sub Referect_EnableCh_for_form()
        '------------------------------------------------
        '       Reflect mpm ch 
        '------------------------------------------------
        Dim loop1 As Integer
        Dim loop2 As Integer
        Dim enable_slot() As Boolean
        Dim slot_type As String

        For loop1 = 0 To UBound(MPM)
            'get insert module count with "MPM Information" class 
            enable_slot = MPM(loop1).Information.ModuleEnable

            'modeule loop(Maximum 5 slot)
            For loop2 = 0 To 4
                If enable_slot(loop2) = True Then
                    'get module type with "MPM Information" class 
                    slot_type = MPM(loop1).Information.ModuleType(loop2)

                    If slot_type <> "MPM-212" Then
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch1")
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch2")
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch3")
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch4")
                    Else
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch1")
                        Me.chklst_ch.Items.Add("MPM" & CStr(loop1 + 1) & " Slot" & CStr(loop2) & " Ch2")
                    End If

                End If
            Next
        Next
    End Sub
    Private Sub Referect_EnableRange_for_form()
        '-----------------------------------------------------
        '       Refelect MPM Range
        '-------------------------------------------------------


        ' MPM-213 can use just 1 to 4 range
        If Flag_213 = True Then
            Me.chklst_range.Items.Add("Range1")
            Me.chklst_range.Items.Add("Range2")
            Me.chklst_range.Items.Add("Range3")
            Me.chklst_range.Items.Add("Range4")
        Else
            Me.chklst_range.Items.Add("Range1")
            Me.chklst_range.Items.Add("Range2")
            Me.chklst_range.Items.Add("Range3")
            Me.chklst_range.Items.Add("Range4")
            Me.chklst_range.Items.Add("Range5")
        End If

        ' MPM-215 can't select range
        If Flag_215 = True Then
            Me.chklst_range.Enabled = False
        End If

    End Sub

    Private Sub btntslset_Click(sender As Object, e As EventArgs) Handles btntslsetcheck.Click
        '-------------------------------------------------------------------------
        '           Check and set TSL Parameter
        '--------------------------------------------------------------------------
        Dim item As TSLSweepItem                     'TSL SweepItem
        Dim specminwave As Double                    'Spec min wavelength(nm)
        Dim specmaxwave As Double                    'Spec max wavelength(nm)
        Dim startwave As Double                      'Startwavelength(nm)
        Dim stopwave As Double                       'Stopwavelength(nm)
        Dim set_pow As Single                        'Power(dBm)
        Dim speed As Double                          'Sweep Speed (nm/sec)
        Dim wavestep As Double                       'wavelenthg step(nm)
        Dim eachtsl_startwave As Double              'Startwavelength each TSL(nm)
        Dim eachtsl_stopwave As Double               'Stopwavelength each TSL(nm)

        If Me.cmbspeed.Text = "" Then
            MsgBox("Please enter to the SweepSpeed", vbOKOnly)
            Exit Sub
        End If

        '----Sweep Setting Check
        startwave = Me.txtstartwave.Text
        stopwave = Me.txtstopwave.Text
        set_pow = Me.txtpower.Text
        speed = Me.cmbspeed.Text
        wavestep = Me.txtstepwave.Text

        'wavelength
        If (startwave < TSL_sweepstartwave Or stopwave > TSL_sweepstopwave) Then
            Show_Inst_Error(ExceptionCode.ParameterError)
            Exit Sub
        End If

        'set power
        If (set_pow < TSL_minpower Or set_pow > TSL_maxpower) Then
            Show_Inst_Error(ExceptionCode.ParameterError)
            Exit Sub
        End If

        'Sweep Speed
        If (speed < TSL_minspeed Or speed > TSL_maxspeed) Then
            Show_Inst_Error(ExceptionCode.ParameterError)
            Exit Sub
        End If

        'wavelenthg step
        If (wavestep < 0.001) Then
            Show_Inst_Error(ExceptionCode.ParameterError)
            Exit Sub
        End If

        Dim loop1 As Integer

        For loop1 = 0 To UBound(TSL)

            item = TSL_SweepItemDictionary(TSL(loop1))

            specminwave = item.specminwave
            specmaxwave = item.specmaxwave

            Select Case TSL_OSUport(loop1)
                Case 1
                    'port1
                    eachtsl_startwave = Me.txtstartwave1.Text
                    eachtsl_stopwave = Me.txtstopwave1.Text
                Case 2
                    'port2
                    eachtsl_startwave = Me.txtstartwave2.Text
                    eachtsl_stopwave = Me.txtstopwave2.Text
                Case 3
                    'port3
                    eachtsl_startwave = Me.txtstartwave3.Text
                    eachtsl_stopwave = Me.txtstopwave3.Text
                Case 4
                    'port4
                    eachtsl_startwave = Me.txtstartwave4.Text
                    eachtsl_stopwave = Me.txtstopwave4.Text
            End Select

            'wavelength
            If (eachtsl_startwave < specminwave Or eachtsl_startwave > specmaxwave) Then
                Show_Inst_Error(ExceptionCode.ParameterError)
                Exit Sub
            End If

            If (eachtsl_stopwave < specminwave Or eachtsl_stopwave > specmaxwave) Then
                Show_Inst_Error(ExceptionCode.ParameterError)
                Exit Sub
            End If

            If (eachtsl_startwave < startwave Or eachtsl_startwave > stopwave) Then
                Show_Inst_Error(ExceptionCode.ParameterError)
                Exit Sub
            End If

            If (eachtsl_stopwave < startwave Or eachtsl_stopwave > stopwave) Then
                Show_Inst_Error(ExceptionCode.ParameterError)
                Exit Sub
            End If

            'wavelenthg step
            If (wavestep > Math.Abs(eachtsl_startwave - eachtsl_stopwave)) Then
                Show_Inst_Error(ExceptionCode.ParameterError)
                Exit Sub
            End If

        Next

        MsgBox("Completed.", vbOKOnly)

    End Sub

    Private Sub btnset_Click(sender As Object, e As EventArgs) Handles btnset.Click
        '-------------------------------------------------------------------------
        '           Set Parameter
        '--------------------------------------------------------------------------
        Dim startwave As Double             'Startwavelength(nm)
        Dim stopwave As Double              'Stopwavelength(nm)
        Dim set_pow As Double               'Power(dBm)
        Dim wavestep As Double              'wavelenthg step(nm)
        Dim speed As Double                 'Sweep Speed (nm/sec)
        Dim inst_error As Integer           'instullment error
        Dim tslcount As Integer             'TSL count
        Dim item As TSLSweepItem            'TSL SweepItem

        '----TSL Setting
        Dim loop1 As Integer

        tslcount = TSL.Count
        ReDim Cal_STS(tslcount - 1)

        For loop1 = 0 To tslcount - 1

            Cal_STS(loop1) = New ILSTS
        Next

        For loop1 = 0 To UBound(TSL)

            Select Case TSL_OSUport(loop1)
                Case 1
                    'port1
                    startwave = Me.txtstartwave1.Text
                    stopwave = Me.txtstopwave1.Text
                Case 2
                    'port2
                    startwave = Me.txtstartwave2.Text
                    stopwave = Me.txtstopwave2.Text
                Case 3
                    'port3
                    startwave = Me.txtstartwave3.Text
                    stopwave = Me.txtstopwave3.Text
                Case 4
                    'port4
                    startwave = Me.txtstartwave4.Text
                    stopwave = Me.txtstopwave4.Text
            End Select

            wavestep = Me.txtstepwave.Text
            speed = Me.cmbspeed.Text
            set_pow = Me.txtpower.Text

            'set Power
            inst_error = TSL(loop1).Set_APC_Power_dBm(set_pow)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            'busy check
            inst_error = TSL(loop1).TSL_Busy_Check(3000)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            'OSU port select and range adjust must set to be after TSL Power set.
            'port select
            inst_error = OSU.Set_Switch(TSL_OSUport(loop1))
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            'range adjust
            inst_error = OSU.Range_Adjust()
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If


            Dim tsl_acctualstep As Double          'TSL output trigger step(nm)

            'set Sweep parameter
            inst_error = TSL(loop1).Set_Sweep_Parameter_for_STS(startwave, stopwave, speed, wavestep, tsl_acctualstep)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            'wavelength -> start wavelength
            inst_error = TSL(loop1).Set_Wavelength(startwave)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If


            'Add item to Dictionary
            item = TSL_SweepItemDictionary(TSL(loop1))

            item.startwave = startwave
            item.stopwave = stopwave
            item.speed = speed
            item.power = set_pow
            item.wavestep = wavestep
            item.acctualstep = tsl_acctualstep
            TSL_SweepItemDictionary(TSL(loop1)) = item

            'busy check
            inst_error = TSL(loop1).TSL_Busy_Check(3000)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            '----MPM setting 
            Dim loop2 As Integer

            For loop2 = 0 To UBound(MPM)
                'Sweep parameter setting 
                inst_error = MPM(loop2).Set_Logging_Paremeter_for_STS(startwave, stopwave, wavestep, speed, Santec.MPM.Measurement_Mode.Freerun)
                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                    Exit Sub
                End If
            Next

            Dim averaging_time As Double

            inst_error = MPM(0).Get_Averaging_Time(averaging_time)

            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If


            '----set STS Process parameter

            Dim sts_error As Integer            'sts process class error

            'data Clear 
            sts_error = Cal_STS(loop1).Clear_Measdata()

            If sts_error <> 0 Then
                Show_STS_Error(sts_error)
                Exit Sub
            End If

            sts_error = Cal_STS(loop1).Clear_Refdata

            If sts_error <> 0 Then
                Show_STS_Error(sts_error)
                Exit Sub
            End If

            'Setting for STS rescaling mode
            sts_error = Cal_STS(loop1).Set_Rescaling_Setting(RescalingMode.Freerun_SPU, averaging_time, True)

            If sts_error <> 0 Then
                Show_STS_Error(sts_error)
                Exit Sub
            End If

            'Make acctual wavelength step table
            sts_error = Cal_STS(loop1).Make_Sweep_Wavelength_Table(startwave, stopwave, tsl_acctualstep)

            If sts_error <> 0 Then
                Show_STS_Error(sts_error)
                Exit Sub
            End If

            'Make rescaling wavelength step table
            sts_error = Cal_STS(loop1).Make_Target_Wavelength_Table(startwave, stopwave, wavestep)

            If sts_error <> 0 Then
                Show_STS_Error(sts_error)
                Exit Sub
            End If
        Next

        '----prepare data struct & hold instrument condition
        If Me.chkeach_ch.Checked Then
            Prepare_dataST_Each()
        Else
            Prepare_dataST()
        End If

        MsgBox("Completed.", vbOKOnly)
    End Sub

    Private Sub Prepare_dataST()
        '-----------------------------------------------
        '       Prepar STS data struct
        '-----------------------------------------------
        Dim rangecout As Integer                        'number of range
        Dim chcount As Integer                          'number of ch 
        Dim loop1 As Integer                            'loop count1
        Dim loop2 As Integer                            'loop count2
        Dim text_st As String = String.Empty            'text String 
        Dim split_st() As String = Nothing              'split string array

        'List clear 
        Meas_rang.Clear()                               'Measurement Range
        Data_struct.Clear()                             'DataSturct of STS
        Refdata_struct.Clear()                          'Reference data struct
        Ref_monitordata_struct.Clear()                  'Reference monitor data struct
        Meas_monitor_struct.Clear()                     'Measurement monitor data struct



        Mergedata_struct.Clear()                        'DataStruct of Merge 

        rangecout = Me.chklst_range.CheckedItems.Count
        chcount = Me.chklst_ch.CheckedItems.Count

        If Flag_215 = True Then
            'If mom215 range must be set 1
            Meas_rang.Add(1)
        Else
            If rangecout = 0 Or chcount = 0 Then
                MsgBox("Please check measurement parameters.", vbOKOnly)
                Exit Sub
            End If


            'hold range data 
            For loop1 = 0 To Me.chklst_range.Items.Count - 1
                If Me.chklst_range.GetItemChecked(loop1) = True Then
                    Meas_rang.Add(loop1 + 1)
                End If
            Next
        End If
        Dim device_number As Integer
        Dim slot_number As Integer
        Dim ch_number As Integer
        Dim set_struct As STSDataStruct
        Dim set_struct_merge As STSDataStructForMerge
        Dim sweep_count As Integer                          'Sweep count 

        '--for measurement MPM data
        For loop2 = 0 To Meas_rang.Count - 1
            For loop1 = 0 To Me.chklst_ch.Items.Count - 1

                If Me.chklst_ch.GetItemChecked(loop1) = True Then
                    text_st = Me.chklst_ch.Items(loop1)
                    split_st = Split(text_st, " ")
                    'ch parameter
                    device_number = CInt(split_st(0).Substring(3)) - 1
                    slot_number = CInt(split_st(1).Substring(4))
                    ch_number = CInt(split_st(2).Substring(2))
                    'for data
                    set_struct.MPMNumber = device_number
                    set_struct.SlotNumber = slot_number
                    set_struct.ChannelNumber = ch_number
                    set_struct.RangeNumber = Meas_rang(loop2)
                    set_struct.SweepCount = loop2 + 1
                    set_struct.SOP = 0
                    Data_struct.Add(set_struct)
                End If

            Next
        Next

        '---for merasurement Monitor data
        ' monitor data need each sweep times data
        Dim set_monitor_struct As STSMonitorStruct              'set struct for monitor
        For loop2 = 0 To Meas_rang.Count - 1
            For loop1 = 0 To Me.chklst_ch.Items.Count - 1

                If Me.chklst_ch.GetItemChecked(loop1) = True Then
                    text_st = Me.chklst_ch.Items(loop1)
                    split_st = Split(text_st, " ")
                    'ch parameter
                    device_number = CInt(split_st(0).Substring(3)) - 1

                    set_monitor_struct.MPMNumber = device_number
                    set_monitor_struct.SOP = 0
                    set_monitor_struct.SweepCount = loop2 + 1

                    Meas_monitor_struct.Add(set_monitor_struct)
                    If Meas_monitor_struct.Count = loop2 + 1 Then
                        Exit For
                    End If
                End If
            Next
        Next

        '---for　reference MPM data & merge data
        '   reference data need only 1 range data
        For loop1 = 0 To Me.chklst_ch.Items.Count - 1

            If Me.chklst_ch.GetItemChecked(loop1) = True Then
                text_st = Me.chklst_ch.Items(loop1)
                split_st = Split(text_st, " ")
                'ch parameter
                device_number = CInt(split_st(0).Substring(3)) - 1
                slot_number = CInt(split_st(1).Substring(4))
                ch_number = CInt(split_st(2).Substring(2))

                'for reference data
                set_struct.MPMNumber = device_number
                set_struct.SlotNumber = slot_number
                set_struct.ChannelNumber = ch_number
                set_struct.RangeNumber = 1
                set_struct.SweepCount = 1
                set_struct.SOP = 0

                Refdata_struct.Add(set_struct)

                'for mege
                set_struct_merge.MPMNumber = device_number
                set_struct_merge.SlotNumber = slot_number
                set_struct_merge.ChannelNumber = ch_number
                set_struct_merge.SOP = 0
                Mergedata_struct.Add(set_struct_merge)

            End If
        Next

        '----for referece Monitor data 
        Dim set_ref_monitor_struct = New STSDataStruct
        For loop1 = 0 To Me.chklst_ch.Items.Count - 1

            If Me.chklst_ch.GetItemChecked(loop1) = True Then
                text_st = Me.chklst_ch.Items(loop1)
                split_st = Split(text_st, " ")
                'Mainframe parameter
                device_number = CInt(split_st(0).Substring(3)) - 1
                slot_number = CInt(split_st(1).Substring(4))
                ch_number = CInt(split_st(2).Substring(2))

                ' for reference monitor data
                set_ref_monitor_struct.MPMNumber = device_number
                set_ref_monitor_struct.SlotNumber = slot_number
                set_ref_monitor_struct.ChannelNumber = ch_number
                set_ref_monitor_struct.RangeNumber = 1
                set_ref_monitor_struct.SOP = 0
                set_ref_monitor_struct.SweepCount = 1

                Ref_monitordata_struct.Add(set_ref_monitor_struct)
                Exit For
            End If
        Next


    End Sub

    Private Sub Prepare_dataST_Each()
        '-----------------------------------------------
        '       Prepar STS data struct
        '-----------------------------------------------
        Dim rangecout As Integer                        'number of range
        Dim chcount As Integer                          'number of ch 
        Dim loop1 As Integer                            'loop count1
        Dim loop2 As Integer                            'loop count2
        Dim text_st As String = String.Empty            'text String 
        Dim split_st() As String = Nothing              'split string array

        'List clear 
        Meas_rang.Clear()                               'Measurement Range
        Data_struct.Clear()                             'DataSturct of STS
        Refdata_struct.Clear()                          'Reference data struct
        Ref_monitordata_struct.Clear()                  'Reference monitor data struct
        Meas_monitor_struct.Clear()                     'Measurement monitor data struct



        Mergedata_struct.Clear()                        'DataStruct of Merge 

        rangecout = Me.chklst_range.CheckedItems.Count
        chcount = Me.chklst_ch.CheckedItems.Count

        If Flag_215 = True Then
            'If mom215 range must be set 1
            Meas_rang.Add(1)
        Else
            If rangecout = 0 Or chcount = 0 Then
                MsgBox("Please check measurement parameters.", vbOKOnly)
                Exit Sub
            End If


            'hold range data 
            For loop1 = 0 To Me.chklst_range.Items.Count - 1
                If Me.chklst_range.GetItemChecked(loop1) = True Then
                    Meas_rang.Add(loop1 + 1)
                End If
            Next
        End If
        Dim device_number As Integer
        Dim slot_number As Integer
        Dim ch_number As Integer
        Dim set_struct As STSDataStruct
        Dim set_struct_merge As STSDataStructForMerge
        Dim sweep_count As Integer                          'Sweep count 

        '--for measurement MPM data
        For loop2 = 0 To Meas_rang.Count - 1
            For loop1 = 0 To Me.chklst_ch.Items.Count - 1

                If Me.chklst_ch.GetItemChecked(loop1) = True Then
                    text_st = Me.chklst_ch.Items(loop1)
                    split_st = Split(text_st, " ")
                    'ch parameter
                    device_number = CInt(split_st(0).Substring(3)) - 1
                    slot_number = CInt(split_st(1).Substring(4))
                    ch_number = CInt(split_st(2).Substring(2))
                    'for data
                    set_struct.MPMNumber = device_number
                    set_struct.SlotNumber = slot_number
                    set_struct.ChannelNumber = ch_number
                    set_struct.RangeNumber = Meas_rang(loop2)
                    set_struct.SweepCount = loop2 + 1
                    set_struct.SOP = 0
                    Data_struct.Add(set_struct)
                End If

            Next
        Next

        '---for merasurement Monitor data
        ' monitor data need each sweep times data
        Dim set_monitor_struct As STSMonitorStruct              'set struct for monitor
        For loop2 = 0 To Meas_rang.Count - 1
            For loop1 = 0 To Me.chklst_ch.Items.Count - 1

                If Me.chklst_ch.GetItemChecked(loop1) = True Then
                    text_st = Me.chklst_ch.Items(loop1)
                    split_st = Split(text_st, " ")
                    'ch parameter
                    device_number = CInt(split_st(0).Substring(3)) - 1

                    set_monitor_struct.MPMNumber = device_number
                    set_monitor_struct.SOP = 0
                    set_monitor_struct.SweepCount = loop2 + 1

                    Meas_monitor_struct.Add(set_monitor_struct)
                    If Meas_monitor_struct.Count = loop2 + 1 Then
                        Exit For
                    End If
                End If
            Next
        Next

        '---for　reference MPM data & reference monitor data & merge data
        '   reference data need only 1 range data
        set_struct = New STSDataStruct
        Dim set_ref_monitor_struct As STSDataStruct
        For loop1 = 0 To Me.chklst_ch.Items.Count - 1

            If Me.chklst_ch.GetItemChecked(loop1) = True Then
                text_st = Me.chklst_ch.Items(loop1)
                split_st = Split(text_st, " ")
                'ch parameter
                device_number = CInt(split_st(0).Substring(3)) - 1
                slot_number = CInt(split_st(1).Substring(4))
                ch_number = CInt(split_st(2).Substring(2))

                'for reference data
                set_struct.MPMNumber = device_number
                set_struct.SlotNumber = slot_number
                set_struct.ChannelNumber = ch_number
                set_struct.RangeNumber = 1
                set_struct.SweepCount = 1
                set_struct.SOP = 0

                Refdata_struct.Add(set_struct)


                'for reference monitor data
                set_ref_monitor_struct.MPMNumber = device_number
                set_ref_monitor_struct.SlotNumber = slot_number
                set_ref_monitor_struct.ChannelNumber = ch_number
                set_ref_monitor_struct.RangeNumber = 1
                set_ref_monitor_struct.SweepCount = 1
                set_ref_monitor_struct.SOP = 0

                Ref_monitordata_struct.Add(set_ref_monitor_struct)

                'for mege
                set_struct_merge.MPMNumber = device_number
                set_struct_merge.SlotNumber = slot_number
                set_struct_merge.ChannelNumber = ch_number
                set_struct_merge.SOP = 0
                Mergedata_struct.Add(set_struct_merge)

            End If
        Next


    End Sub

    Private Sub btnget_reference_Click(sender As Object, e As EventArgs) Handles btnget_reference.Click

        '------------------------------------------------------------------------------
        '           Get Reference
        '------------------------------------------------------------------------------
        Dim inst_error As Integer                       'Instullment error
        Dim inst_flag As Boolean
        Dim loop1 As Integer

        SPU_Sampling_timeDictionary.Clear()

        '----MPM setting for selected 1st range

        'set Range for MPM
        For loop1 = 0 To UBound(MPM)
            inst_error = MPM(loop1).Set_Range(Meas_rang(0))
            If (inst_error <> 0) Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If
        Next

        If Me.chkeach_ch.Checked Then
            'Reference measurement one channel at a time
            inst_error = Each_channel_reference(inst_flag)
        Else
            inst_error = All_channel_reference(inst_flag)
        End If

        If inst_error <> 0 Then
            If inst_error = -9999 Then
                MsgBox("MPM Trigger receive error! Please check trigger cable connection.", vbOKOnly)
            ElseIf inst_flag = True Then
                Show_Inst_Error(inst_error)             'Instullment error
            Else
                Show_STS_Error(inst_error)              'Processing error
            End If
            Exit Sub
        End If

        MsgBox("Completed.", vbOKOnly)

    End Sub

    Private Function All_channel_reference(ByRef inst_flag As Boolean) As Integer

        '------------------------------------------------------------------------------
        '           Get Reference
        '------------------------------------------------------------------------------
        Dim inst_error As Integer                       'Instullment error
        Dim loop1 As Integer

        Me.toolstatus.Text = ""
        Me.toolmessage.Text = ""

        For loop1 = 0 To UBound(TSL)

            inst_error = Set_Sweep_Parameter(False, loop1)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
            End If

            '----Move to start wavelength with Sweep Start method.
            inst_error = TSL(loop1).Sweep_Start()
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
            End If

            Me.toolstatus.Text = "Reference..."
            Me.toolmessage.Text = "OSU Port" & TSL_OSUport(loop1)
            Me.Refresh()

            'Sweep 
            inst_error = Sweep_Process(loop1)


            If inst_error <> 0 Then
                inst_flag = True
                Return inst_error
            End If

            'Move to start wavelength  with Sweep Start method.
            inst_error = TSL(loop1).Sweep_Start

            If inst_error <> 0 Then
                inst_flag = True
                Return inst_error
            End If

            'get logging data & add in STSProcess class
            inst_error = Get_reference_samplingdata(inst_flag, loop1)

            If inst_error <> 0 Then
                Return inst_error

            End If

            '------Reference data rescaling 
            Dim process_error As Integer
            process_error = Cal_STS(loop1).Cal_RefData_Rescaling()

            If process_error <> 0 Then
                inst_flag = False
                Return process_error
            End If

            'TSL Sweep Stop
            inst_error = TSL(loop1).Sweep_Stop()

            If inst_error <> 0 Then
                Return inst_error
            End If

        Next

        Me.toolstatus.Text = ""
        Me.toolmessage.Text = ""
        Return 0

    End Function

    Private Function Each_channel_reference(ByRef inst_flag As Boolean) As Integer

        '------------------------------------------------------------------------------
        '           Get Reference
        '------------------------------------------------------------------------------
        Dim inst_error As Integer                       'Instullment error
        Dim loop1 As Integer

        For Each item As STSDataStruct In Refdata_struct

            Me.toolstatus.Text = ""
            Me.toolmessage.Text = ""

            MsgBox("Connect fiber to MPM" & item.MPMNumber + 1 & "_Slot" & item.SlotNumber & "_Ch" & item.ChannelNumber & ".", vbOKOnly)

            For loop1 = 0 To UBound(TSL)

                inst_error = Set_Sweep_Parameter(False, loop1)
                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                End If

                '----Move to start wavelength with Sweep Start method.
                inst_error = TSL(loop1).Sweep_Start()
                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                End If

                Me.toolstatus.Text = "Reference..."
                Me.toolmessage.Text = "OSU Port" & TSL_OSUport(loop1) & "   MPM" & item.MPMNumber + 1 & " Slot" & item.SlotNumber & " Ch" & item.ChannelNumber
                Me.Refresh()

                'Sweep 
                inst_error = Sweep_Process(loop1)

                If inst_error <> 0 Then
                    inst_flag = True
                    Return inst_error
                End If

                'Move to start wavelength  with Sweep Start method.
                inst_error = TSL(loop1).Sweep_Start
                If inst_error <> 0 Then
                    inst_flag = True
                    Return inst_error
                End If


                'get logging data & add in STSProcess class
                inst_error = Get_Each_channel_reference_samplingdata(inst_flag, loop1, item.MPMNumber, item.SlotNumber, item.ChannelNumber, item.SweepCount)

                If inst_error <> 0 Then
                    Return inst_error
                End If

                'TSL Sweep Stop
                inst_error = TSL(loop1).Sweep_Stop()

                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                    Return inst_error
                End If

                '------Reference data rescaling 
                Dim process_error As Integer
                process_error = Cal_STS(loop1).Cal_RefData_Rescaling()

                If process_error <> 0 Then
                    inst_flag = False
                    Return process_error
                End If

            Next
        Next

        Me.toolstatus.Text = ""
        Me.toolmessage.Text = ""
        Return 0

    End Function

    Private Function Set_Sweep_Parameter(ByVal Flag_meas As Boolean, ByVal deveice As Integer) As Integer

        Dim inst_error As Integer           'Instullment error
        Dim startwave As Double             'Startwavelength(nm)
        Dim stopwave As Double              'Stopwavelength(nm)
        Dim wavestep As Double              'wavelenthg step(nm)
        Dim tsl_acctualstep As Double       'TSL output trigger step(nm)
        Dim speed As Double                 'Sweep Speed (nm/sec)
        Dim item As TSLSweepItem            'TSL SweepItem


        item = TSL_SweepItemDictionary(TSL(deveice))

        startwave = item.startwave
        stopwave = item.stopwave
        speed = item.speed
        wavestep = item.wavestep
        tsl_acctualstep = item.acctualstep

        '----OSU setting
        'optical switch setting
        inst_error = OSU.Set_Switch(TSL_OSUport(deveice))
        If inst_error <> 0 Then
            Return inst_error
        End If

        '----TSL setting
        'set Sweep parameter
        inst_error = TSL(deveice).Set_Sweep_Parameter_for_STS(startwave, stopwave, speed, wavestep, tsl_acctualstep)
        If inst_error <> 0 Then
            Return inst_error
        End If

        '----MPM setting 
        'Sweep parameter setting 
        For loop1 = 0 To UBound(MPM)
            'Sweep parameter setting 
            inst_error = MPM(loop1).Set_Logging_Paremeter_for_STS(startwave, stopwave, wavestep, speed, Santec.MPM.Measurement_Mode.Freerun)
            If inst_error <> 0 Then
                Return inst_error
            End If
        Next

        '----SPU setting
        Dim averaging_time As Double

        inst_error = MPM(0).Get_Averaging_Time(averaging_time)

        If inst_error <> 0 Then
            Return inst_error
        End If

        'parameter setting
        If Flag_meas Then
            'measurement
            If SPU_Sampling_timeDictionary.ContainsKey(TSL(deveice)) = True Then
                SPU.Meas_Sampling_time = SPU_Sampling_timeDictionary(TSL(deveice))
                inst_error = SPU.Set_Sampling_Parameter_for_Measure(startwave, stopwave, speed, tsl_acctualstep)
            Else
                'Initial measurement when using Read Reference Data function
                inst_error = SPU.Set_Sampling_Parameter(startwave, stopwave, speed, tsl_acctualstep)
            End If
        Else
            'reference
            inst_error = SPU.Set_Sampling_Parameter(startwave, stopwave, speed, tsl_acctualstep)
        End If


        If inst_error <> 0 Then
            Return inst_error
        End If

        'mpm averageing time-> spu
        SPU.AveragingTime = averaging_time

        Return 0

    End Function

    Private Function Sweep_Process(ByVal deveice As Integer) As Integer
        '------------------------------------------------------------
        '       Sweep Process
        '------------------------------------------------------------
        Dim inst_error As Integer               'Instullment error

        'MPM sampling start 
        For loop1 = 0 To UBound(MPM)
            inst_error = MPM(loop1).Logging_Start
            If inst_error <> 0 Then
                Return inst_error
            End If
        Next

        'TSL waiting for start status 
        inst_error = TSL(deveice).Waiting_For_Sweep_Status(3000, Santec.TSL.Sweep_Status.WaitingforTrigger)

        '----error handling -> MPM Logging Stop
        If inst_error <> 0 Then
            For loop1 = 0 To UBound(MPM)
                MPM(loop1).Logging_Stop()
            Next

            Return inst_error
        End If

        'SPU sampling start
        inst_error = SPU.Sampling_Start()
        If inst_error <> 0 Then
            Return inst_error
        End If

        'TSL issue software trigger
        inst_error = TSL(deveice).Set_Software_Trigger()

        '----error handling -> MPM Logging Stop
        If inst_error <> 0 Then
            For loop1 = 0 To UBound(MPM)
                MPM(loop1).Logging_Stop()
            Next

            Return inst_error
        End If

        'SPU waiting for sampling 
        inst_error = SPU.Waiting_for_sampling()

        '----error handling -> MPM Logging Stop
        If inst_error <> 0 Then
            For loop1 = 0 To UBound(MPM)
                MPM(loop1).Logging_Stop()
            Next

            Return inst_error
        End If

        Dim mpm_stauts As Integer                   'mpm logging status 0:douring measurement 1:Compleated -1:Stopped
        Dim mpm_count As Integer                    'mpm number of logging completed point
        Dim timeout As Double = 2000                'MPM Logging Status timeout(2000msec) after the SPU sampling completed.
        Dim st As New Stopwatch                     'stopwatch           
        Dim mpm_completed_falg As Boolean = True    'mpm logging completed flag  F:not completed T:completed
        Dim mpm_complet_flag As Boolean = True

        'MPM waiting for sampling 
        st.Start()                                  'stopwathc start 

        Do
            For loop1 = 0 To UBound(MPM)
                inst_error = MPM(loop1).Get_Logging_Status(mpm_stauts, mpm_count)
                If inst_error <> 0 Then
                    Return inst_error
                End If

                If mpm_stauts = 1 Then
                    Exit Do
                End If
            Next

            If st.ElapsedMilliseconds >= timeout Then
                mpm_complet_flag = False
                Exit Do
            End If
        Loop

        st.Stop()

        'MPM sampling stop
        For loop1 = 0 To UBound(MPM)
            inst_error = MPM(loop1).Logging_Stop()
            If inst_error <> 0 Then
                Return inst_error
            End If
        Next


        'TSL Waiting for standby
        inst_error = TSL(deveice).Waiting_For_Sweep_Status(5000, Santec.TSL.Sweep_Status.Standby)

        If inst_error <> 0 Then
            Return inst_error
        End If

        If mpm_completed_falg = False Then
            'mpm logging timeout occurred.
            Return -9999
        End If

        Return 0

    End Function

    Private Function Get_reference_samplingdata(ByRef inst_flag As Boolean, ByVal deveice As Integer) As Integer
        '---------------------------------------------------------------
        '       Get logging reference data & add in 
        '---------------------------------------------------------------
        Dim inst_error As Integer                        'Instullment error
        Dim logg_data() As Single = Nothing              'MPM Logging data
        Dim cal_error As Integer                         'process error
        Dim sampling_time As Double

        '----Load　Reference MPM data & add in data for STS Process Class for each channel
        For Each item As STSDataStruct In Refdata_struct

            'Read corresponded MPM data
            inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber, logg_data)

            If inst_error <> 0 Then
                inst_flag = True
                Return inst_error
            End If

            'Add in to MPM reference data to STS Process Class
            cal_error = Cal_STS(deveice).Add_Ref_MPMData_CH(logg_data, item)

            If cal_error <> 0 Then
                inst_flag = False
                Return cal_error
            End If
        Next

        '----Load Monitor data & add in data for STS Proccsess class with "STS Data Struct"
        Dim triggerdata() As Single = Nothing     'tigger data 
        Dim monitordata() As Single = Nothing     'monitor data

        inst_error = SPU.Get_Sampling_Rawdata(triggerdata, monitordata)

        If inst_error <> 0 Then
            inst_flag = True
            Return inst_error
        End If

        If SPU_Sampling_timeDictionary.ContainsKey(TSL(deveice)) = False Then
            sampling_time = SPU.Meas_Sampling_time
            SPU_Sampling_timeDictionary.Add(TSL(deveice), sampling_time)
        End If

        For Each monitor_item As STSDataStruct In Ref_monitordata_struct

            cal_error = Cal_STS(deveice).Add_Ref_MonitorData(triggerdata, monitordata, monitor_item)

            If cal_error <> 0 Then
                inst_flag = False
                Return cal_error
            End If
        Next

        Return 0


    End Function

    Private Function Get_Each_channel_reference_samplingdata(ByRef inst_flag As Boolean, ByVal deveice As Integer, ByVal currentMPMNumber As Integer, ByVal currentSlotNumber As Integer, ByVal currentChannelNumber As Integer, ByVal currentSweepCount As Integer) As Integer
        '---------------------------------------------------------------
        '       Get logging reference data & add in 
        '---------------------------------------------------------------
        Dim inst_error As Integer                        ' Instullment error
        Dim logg_data() As Single = Nothing              'MPM Logging data
        Dim cal_error As Integer                         'process error


        '----Load　Reference MPM data & add in data for STS Process Class for each channel
        For Each item As STSDataStruct In Refdata_struct

            If (item.MPMNumber <> currentMPMNumber Or item.SlotNumber <> currentSlotNumber Or item.ChannelNumber <> currentChannelNumber) Then
                Continue For
            End If

            ' Read corresponded MPM data
            inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber, logg_data)

            If inst_error <> 0 Then
                inst_flag = True
                Return inst_error
            End If

            'Add in to MPM reference data to STS Process Class
            cal_error = Cal_STS(deveice).Add_Ref_MPMData_CH(logg_data, item)

            If cal_error <> 0 Then
                inst_flag = False
                Return cal_error
            End If
        Next

        '------Load Monitor data & add in data for STS Proccsess class with "STS Data Struct"
        Dim triggerdata() As Single = Nothing     'tigger data 
        Dim monitordata() As Single = Nothing     'monitor data

        inst_error = SPU.Get_Sampling_Rawdata(triggerdata, monitordata)

        If inst_error <> 0 Then
            inst_flag = True
            Return inst_error
        End If

        For Each monitor_item As STSDataStruct In Ref_monitordata_struct

            cal_error = Cal_STS(deveice).Add_Ref_MonitorData(triggerdata, monitordata, monitor_item)
            If cal_error <> 0 Then
                inst_flag = False
                Return cal_error
            End If
        Next

        'cal_error = Cal_STS.Add_Ref_MonitorData(triggerdata, monitordata, Ref_monitor_struct)

        'If cal_error <> 0 Then
        '    inst_flag = False
        '    Return cal_error
        'End If

        Return 0


    End Function

    Private Function Get_MPM_Loggdata(ByVal deveice As Integer, ByVal slot As Integer, ByVal ch As Integer, ByRef data() As Single)
        '---------------------------------------------------------------
        '       Get MPM Logg data
        '--------------------------------------------------------------
        Dim inst_error As Integer

        inst_error = MPM(deveice).Get_Each_Channel_Logdata(slot, ch, data)
        Return inst_error

    End Function

    Private Sub btnmeas_Click(sender As Object, e As EventArgs) Handles btnmeas.Click
        '-------------------------------------------------------------------------
        '           Mesurement Process
        '-------------------------------------------------------------------------
        Dim loop1 As Integer                        'loop Count 1
        Dim loop2 As Integer                        'loop count 2
        Dim loop3 As Integer                        'loop count 3
        Dim inst_error As Integer                   'instllment error   
        Dim inst_flag As Boolean                    'instrment error flag
        Dim process_error As Integer                'STS　Process error


        Me.toolstatus.Text = ""
        Me.toolmessage.Text = ""

        '-------Measurement-----------------------------------------------

        For loop1 = 0 To UBound(TSL)

            inst_error = Set_Sweep_Parameter(True, loop1)
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
            End If

            'Move to start wavelength  with Sweep Start method.
            inst_error = TSL(loop1).Sweep_Start
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If

            '----Rang Loop
            For loop2 = 0 To Meas_rang.Count - 1

                'MPM range Setting 
                For loop3 = 0 To UBound(MPM)

                    inst_error = MPM(loop3).Set_Range(Meas_rang(loop2))

                    If inst_error <> 0 Then
                        Show_Inst_Error(inst_error)
                        Exit Sub
                    End If
                Next

                Me.toolstatus.Text = "Measurement..."
                Me.toolmessage.Text = "OSU Port" & TSL_OSUport(loop1) & "  Range" & Meas_rang(loop2)
                Me.Refresh()

                'Sweep process
                inst_error = Sweep_Process(loop1)
                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                    Exit Sub
                End If

                'Move to start wavelength  with Sweep Start method for next sweep.
                inst_error = TSL(loop1).Sweep_Start
                If inst_error <> 0 Then
                    Show_Inst_Error(inst_error)
                    Exit Sub
                End If

                'get loggging data & Add in STS Process class
                inst_error = Get_measurement_samplingdata(loop1, loop2 + 1, inst_flag)

                If inst_error <> 0 Then
                    If inst_flag = True Then
                        Show_Inst_Error(inst_error)
                    Else
                        Show_STS_Error(inst_error)
                    End If

                    Exit Sub
                End If

            Next

            '----STS Process

            'Rescaling
            process_error = Cal_STS(loop1).Cal_MeasData_Rescaling()

            If process_error <> 0 Then
                Show_STS_Error(process_error)
                Exit Sub
            End If

            'merge or IL calculate
            Dim merge_type As Module_Type

            If Flag_215 = False Then
                If Flag_213 = True Then
                    merge_type = Module_Type.MPM_213
                Else
                    merge_type = Module_Type.MPM_211
                End If

                'Process ranges merge
                process_error = Cal_STS(loop1).Cal_IL_Merge(merge_type)
            Else
                'just IL process
                process_error = Cal_STS(loop1).Cal_IL()
            End If

            inst_error = TSL(loop1).Sweep_Stop()
            If inst_error <> 0 Then
                Show_Inst_Error(inst_error)
                Exit Sub
            End If
        Next

        For loop1 = 0 To UBound(TSL)
            'data save
            process_error = Save_Measurement_data(loop1)
            If process_error <> 0 Then
                Show_STS_Error(process_error)
            End If
        Next

        Me.toolstatus.Text = ""
        Me.toolmessage.Text = ""
        MsgBox("Completed.")

    End Sub

    Private Function Get_measurement_samplingdata(ByVal deveice As Integer, ByVal sweepcount As Integer, ByRef inst_flag As Boolean) As Integer
        '---------------------------------------------------------------
        '       Get logging measurement data & add in 
        '---------------------------------------------------------------
        Dim inst_error As Integer                        ' Instullment error
        Dim logg_data() As Single = Nothing              'MPM Logging data
        Dim cal_error As Integer                         'process error
        Dim sampling_time As Double


        '----Load MPM Logging data & Add in STS Process class with measurment sts data struct 
        For Each item As STSDataStruct In Data_struct

            If item.SweepCount <> sweepcount Then
                Continue For
            End If

            'Read corresponded MPM data
            inst_error = Get_MPM_Loggdata(item.MPMNumber, item.SlotNumber, item.ChannelNumber, logg_data)

            If inst_error <> 0 Then
                inst_flag = True
                Return inst_error
            End If

            'Add in to MPM reference data to STS Process Class
            cal_error = Cal_STS(deveice).Add_Meas_MPMData_CH(logg_data, item)

            If cal_error <> 0 Then
                inst_flag = False
                Return cal_error
            End If
        Next

        '----Lado SPU monitor data & Add in STS Process class with measurement monitor data struct
        Dim triggerdata() As Single = Nothing
        Dim monitordata() As Single = Nothing

        inst_error = SPU.Get_Sampling_Rawdata(triggerdata, monitordata)

        If inst_error <> 0 Then
            inst_flag = True
            Return inst_error
        End If

        'Initial measurement when using Read Reference Data function
        If SPU_Sampling_timeDictionary.ContainsKey(TSL(deveice)) = False Then
            sampling_time = SPU.Meas_Sampling_time
            SPU_Sampling_timeDictionary.Add(TSL(deveice), sampling_time)
        End If


        '----Search item from measurement monitor data structure according to sweep count.
        For Each item As STSMonitorStruct In Meas_monitor_struct

            If item.SweepCount = sweepcount Then
                cal_error = Cal_STS(deveice).Add_Meas_MonitorData(triggerdata, monitordata, item)

                If cal_error <> 0 Then
                    inst_flag = False
                    Return cal_error
                End If
                Exit For

            End If
        Next
        Return 0


    End Function

    Private Function Save_Measurement_data(ByVal deveice As Integer) As Integer
        '-------------------------------------------------------
        '       Save Measurement data
        '-------------------------------------------------------
        Dim wavelength_table() As Double = Nothing            'Rescaled wavelength table  
        Dim lstILdata As New List(Of Single())                'IL data list 
        Dim process_error As Integer                          'process class error  
        Dim loop1 As Integer                                  'loop count
        Dim ildata() As Single = Nothing                      'il data arrray

        'Get Rescaled wavelength tabel 
        process_error = Cal_STS(deveice).Get_Target_Wavelength_Table(wavelength_table)


        'Get IL data 
        If Flag_215 = True Then
            For Each items As STSDataStruct In Data_struct

                process_error = Cal_STS(deveice).Get_IL_Data(ildata, items)
                If process_error <> 0 Then
                    Return process_error
                End If

                lstILdata.Add(ildata.ToArray)
            Next

        Else

            For Each items As STSDataStructForMerge In Mergedata_struct

                process_error = Cal_STS(deveice).Get_IL_Merge_Data(ildata, items)
                If process_error <> 0 Then
                    Return process_error
                End If

                lstILdata.Add(ildata.ToArray)
            Next
        End If


        '----Data Save 
        Dim file_path As String = String.Empty

        Me.SaveFileDialog1.Title = "port:" & TSL_OSUport(deveice) & " IL data save"
        Me.SaveFileDialog1.Filter = "csv file(*.csv)|*.csv"
        Me.SaveFileDialog1.ShowDialog()

        file_path = Me.SaveFileDialog1.FileName

        Dim writer As New System.IO.StreamWriter(file_path, False, System.Text.Encoding.GetEncoding("UTF-8"))
        Dim write_string As String = Nothing

        Dim hedder As String = String.Empty                 'file hedder 

        hedder = "Wavelength(nm)"

        For Each item As STSDataStruct In Data_struct
            If item.SweepCount <> 1 Then
                Continue For
            End If

            hedder = hedder & ",MPM" & CStr(item.MPMNumber + 1) & "Slot" & CStr(item.SlotNumber) & "Ch" & CStr(item.ChannelNumber)
        Next

        'write hedder
        writer.WriteLine(hedder)

        Dim loop2 As Integer

        For loop1 = 0 To UBound(wavelength_table)

            write_string = wavelength_table(loop1)

            For loop2 = 0 To lstILdata.Count - 1
                write_string = write_string & "," & lstILdata(loop2)(loop1)
            Next

            writer.WriteLine(write_string)

        Next

        writer.Close()

        Return 0

    End Function

    Private Sub btnsaveref_rawdata_Click(sender As Object, e As EventArgs) Handles btnsaveref_rawdata.Click
        '---------------------------------------------------------------------------
        '           Save reference Raw data
        '---------------------------------------------------------------------------
        Dim loop1 As Integer                        'loop count1   
        Dim process_error As Integer                'process class error
        Dim wavetable() As Double = Nothing         'wavelength table
        Dim powdata() As Single = Nothing           'powerdata  rescaled    
        Dim monitordata() As Single = Nothing       'monitordata rescaled 
        Dim lstpowdata As New List(Of Single())     'Power data list
        Dim lstmonitordata As New List(Of Single()) 'monitor data list 

        For loop1 = 0 To UBound(TSL)

            lstpowdata = New List(Of Single())
            lstmonitordata = New List(Of Single())

            ' Get reference Raw power data (after the rescaling)
            For Each item As STSDataStruct In Refdata_struct
                process_error = Cal_STS(loop1).Get_Ref_Power_Rawdata(item, powdata)
                If process_error <> 0 Then
                    Show_STS_Error(process_error)
                    Exit Sub
                End If

                lstpowdata.Add(powdata.ToArray)
            Next

            'Get reference Raw monitor data
            Dim get_struct As STSDataStruct                 'struct of get
            Dim befor_struct As New STSDataStruct           'befor struct

            For Each item As STSDataStruct In Ref_monitordata_struct

                If Me.chkeach_ch.Checked Then
                    'Reference measurement one channel at a time
                    If (item.MPMNumber = befor_struct.MPMNumber) _
                    And (item.SlotNumber = befor_struct.SlotNumber) _
                    And (item.ChannelNumber = befor_struct.ChannelNumber) Then
                        Continue For
                    End If
                End If

                process_error = Cal_STS(loop1).Get_Ref_Monitor_Rawdata(item, monitordata)

                If process_error <> 0 Then
                    Show_STS_Error(process_error)
                    Exit Sub
                End If

                get_struct.MPMNumber = item.MPMNumber
                get_struct.SlotNumber = item.SlotNumber
                get_struct.ChannelNumber = item.ChannelNumber

                lstmonitordata.Add(monitordata.ToArray)
                befor_struct = get_struct
            Next


            'Get Target wavelengt table
            process_error = Cal_STS(loop1).Get_Target_Wavelength_Table(wavetable)

            If process_error <> 0 Then
                Show_STS_Error(process_error)
                Exit Sub
            End If


            '----File save 

            Dim fpath As String = String.Empty                  'file path 

            Me.SaveFileDialog1.Title = "port:" & TSL_OSUport(loop1) & "Reference Raw data"
            Me.SaveFileDialog1.Filter = "csv file(*.csv)|*.csv"
            Me.SaveFileDialog1.ShowDialog()
            fpath = Me.SaveFileDialog1.FileName

            Dim writer As New System.IO.StreamWriter(fpath, False, System.Text.Encoding.GetEncoding("UTF-8"))

            Dim hedder As String = String.Empty                 'file hedder 

            hedder = "Wavelength(nm)"

            For Each item As STSDataStruct In Data_struct
                If item.SweepCount <> 1 Then
                    Continue For
                End If

                hedder = hedder & ",MPM" & CStr(item.MPMNumber + 1) & "Slot" & CStr(item.SlotNumber) & "Ch" & CStr(item.ChannelNumber)
            Next

            If Me.chkeach_ch.Checked Then
                For Each item As STSDataStruct In Refdata_struct
                    hedder = hedder & ",Monitor_MPM" & CStr(item.MPMNumber + 1) & "Slot" & CStr(item.SlotNumber) & "Ch" & CStr(item.ChannelNumber)
                Next
            Else
                hedder = hedder & ",Monitor"
            End If

            writer.WriteLine(hedder)


            'Write data 
            Dim write_str As String = String.Empty                  'write string
            Dim loop2 As Integer                                    'loop count 2
            Dim loop3 As Integer                                    'loop count 3
            Dim loop4 As Integer                                    'loop count 4


            For loop2 = 0 To UBound(wavetable)

                'wavelength data
                write_str = CStr(wavetable(loop2))

                'Power data
                For loop3 = 0 To lstpowdata.Count - 1
                    write_str = write_str & "," & lstpowdata(loop3)(loop2)
                Next


                'monitordata
                For loop4 = 0 To lstmonitordata.Count - 1
                    write_str = write_str & "," & lstmonitordata(loop4)(loop2)
                Next

                writer.WriteLine(write_str)
            Next

            lstpowdata.Clear()
            writer.Close()

        Next

        MsgBox("Completed.", vbOKOnly)

    End Sub

    Private Sub btnsaveRawdata_Click(sender As Object, e As EventArgs) Handles btnsaveRawdata.Click
        '-------------------------------------------------------------------------
        '       Save mesurement raw data
        '-------------------------------------------------------------------------
        Dim loop1 As Integer                                        'loop1
        Dim loop2 As Integer                                        'loop2
        Dim loop3 As Integer                                        'loop3
        Dim loop4 As Integer                                        'loop4
        Dim wavelength_table() As Double = Nothing                  'Wavelength table
        Dim monitordata() As Single = Nothing
        Dim powerdata() As Single = Nothing
        Dim errorcode As Integer                                    'Errorcode
        Dim lstpower As New List(Of Single())

        Dim fpath As String = String.Empty              'File　path
        Dim writer As System.IO.StreamWriter            'Writer 
        Dim write_string As String = String.Empty
        Dim hedder As String = String.Empty

        For loop1 = 0 To UBound(TSL)

            '-- Get Wavelength table
            errorcode = Cal_STS(loop1).Get_Target_Wavelength_Table(wavelength_table)

            If errorcode <> 0 Then
                Show_STS_Error(errorcode)
                Exit Sub
            End If

            For loop2 = 0 To Meas_rang.Count - 1

                '----get raw power data same range 
                For Each item As STSDataStruct In Data_struct

                    If item.RangeNumber <> Meas_rang(loop2) Then
                        Continue For
                    End If

                    errorcode = Cal_STS(loop1).Get_Meas_Power_Rawdata(item, powerdata)

                    If errorcode <> 0 Then
                        Show_STS_Error(errorcode)
                        Exit Sub
                    End If

                    lstpower.Add(powerdata.ToArray)
                Next

                '----get raw monitor data same range
                For Each monitoritem As STSMonitorStruct In Meas_monitor_struct

                    If monitoritem.SweepCount = loop2 + 1 Then
                        errorcode = Cal_STS(loop1).Get_Meas_Monitor_Rawdata(monitoritem, monitordata)
                    Else
                        Continue For
                    End If

                Next


                '----File save at same range data 
                Select Case loop2
                    Case 0
                        Me.SaveFileDialog1.Title = "port:" & TSL_OSUport(loop1) & " 1st Range data"
                    Case 1
                        Me.SaveFileDialog1.Title = "port:" & TSL_OSUport(loop1) & " 2nd Range data"
                    Case Else
                        Me.SaveFileDialog1.Title = "port:" & TSL_OSUport(loop1) & " " & CStr(loop2 + 1) & "rd Range data"
                End Select


                Me.SaveFileDialog1.ShowDialog()
                Me.SaveFileDialog1.Filter = "csv file(*.csv)|*.csv"
                fpath = Me.SaveFileDialog1.FileName

                writer = New System.IO.StreamWriter(fpath, False, System.Text.Encoding.GetEncoding("UTF-8"))

                hedder = "wavelength"

                For Each item As STSDataStruct In Data_struct
                    If item.RangeNumber <> Meas_rang(loop2) Then
                        Continue For
                    End If

                    hedder = hedder & "," & "MPM" & CStr(item.MPMNumber + 1) & "Slot" & CStr(item.SlotNumber) & "Ch" & CStr(item.ChannelNumber)
                Next

                hedder = hedder & "," & "Monitordata"

                writer.WriteLine(hedder)

                For loop3 = 0 To UBound(wavelength_table)
                    write_string = CStr(wavelength_table(loop3))

                    For loop4 = 0 To lstpower.Count - 1
                        write_string = write_string & "," & lstpower(loop4)(loop3)
                    Next

                    write_string = write_string & "," & monitordata(loop3)


                    writer.WriteLine(write_string)

                Next

                writer.Close()
                lstpower = New List(Of Single())
                monitordata = Nothing

            Next

        Next


        MsgBox("Completed.", vbOKOnly)

    End Sub

    Private Sub Show_Inst_Error(ByVal errordata As Integer)
        '------------------------------------
        '       Show error code
        '------------------------------------


        Dim errorstring() As String = [Enum].GetNames(GetType(Santec.ExceptionCode))
        Dim errorvale() As Integer = [Enum].GetValues(GetType(Santec.ExceptionCode))
        Dim loop1 As Integer

        For loop1 = 0 To UBound(errorvale)

            If errorvale(loop1) = errordata Then
                MsgBox(errorstring(loop1))
                Exit For
            End If

        Next

    End Sub


    Private Sub Show_STS_Error(ByVal errordata As Integer)
        '------------------------------------
        '       Show error code for STS
        '------------------------------------


        Dim errorstring() As String = [Enum].GetNames(GetType(Santec.STSProcess.ErrorCode))
        Dim errorvale() As Integer = [Enum].GetValues(GetType(Santec.STSProcess.ErrorCode))
        Dim loop1 As Integer

        For loop1 = 0 To UBound(errorvale)

            If errorvale(loop1) = errordata Then
                MsgBox(errorstring(loop1))
                Exit For
            End If

        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '-----------------------------------------------------------------------------
        '       Reference Data Read
        '       This function must use after "SET" 
        '-----------------------------------------------------------------------------
        Dim fpath As String = String.Empty
        Dim reader As System.IO.StreamReader
        Dim loop1 As Integer                                            'Loop count1
        Dim tslitem As TSLSweepItem

        For loop1 = 0 To UBound(TSL)

            tslitem = TSL_SweepItemDictionary(TSL(loop1))

            '----Reference file　Read 

            Me.OpenFileDialog1.Title = "port:" & TSL_OSUport(loop1) & "_Reference Data"
            Me.OpenFileDialog1.ShowDialog()
            fpath = Me.OpenFileDialog1.FileName

            reader = New System.IO.StreamReader(fpath)

            Dim read_st As String = String.Empty                            'Read String 
            Dim split_st() As String = Nothing                              'split strin array

            'hedder Read 
            read_st = reader.ReadLine
            read_st = read_st.Trim()

            split_st = Split(read_st, ",")


            'Check data cout 
            Dim ch_count As Integer                                         'file data ch count 
            Dim loop2 As Integer                                            'Loop count2
            Dim chk_str As String = String.Empty                            'check string
            Dim mpm_number As Integer                                       'MPM number
            Dim slot_number As Integer                                      'Slot number
            Dim ch_number As Integer                                        'ch number 


            'Check data cout 
            If Me.chkeach_ch.Checked Then
                ch_count = (split_st.Count - 1) / 2
            Else
                ch_count = split_st.Count - 2
            End If

            If ch_count <> Me.chklst_ch.CheckedItems.Count Then

                MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                reader.Close()
                Exit Sub
            End If

            '----Check parameter & make reference data struct 
            Dim refdata_strunct As STSDataStruct                        'Data struct for reference
            Dim lst_refdata_struct As New List(Of STSDataStruct)        'Data struct for reference List    
            Dim match_flag As Boolean = False                           'match flag


            For loop2 = 1 To ch_count
                'MPM device number 
                chk_str = split_st(loop2).Substring(3, 1)
                mpm_number = CInt(chk_str) - 1

                'MPM Slot number 
                chk_str = split_st(loop2).Substring(8, 1)
                slot_number = CInt(chk_str)

                'MPM Ch number 
                chk_str = split_st(loop2).Substring(11, 1)
                ch_number = CInt(chk_str)


                ' Check exsist data in data struct 
                For Each item As STSDataStruct In Data_struct

                    If item.MPMNumber = mpm_number And item.SlotNumber = slot_number And item.ChannelNumber = ch_number Then
                        match_flag = True
                        Exit For
                    End If
                Next

                If match_flag = False Then
                    MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                    reader.Close()
                    Exit Sub
                End If

                'Add reference data struct 
                refdata_strunct.MPMNumber = mpm_number
                refdata_strunct.SlotNumber = slot_number
                refdata_strunct.ChannelNumber = ch_number
                refdata_strunct.RangeNumber = 1
                refdata_strunct.SweepCount = 1

                lst_refdata_struct.Add(refdata_strunct)

            Next

            '----Read Reference data

            If Me.chkeach_ch.Checked Then
                Dim power() As List(Of Single)                 'Power data list 
                Dim monitor() As List(Of Single)               'Monitordata
                Dim counter As Integer                         'Counter
                Dim wavelength As Double                       'Read Wavelength 


                ReDim power(ch_count - 1)
                ReDim monitor(ch_count - 1)

                For loop2 = 0 To ch_count - 1
                    power(loop2) = New List(Of Single)
                    monitor(loop2) = New List(Of Single)
                Next


                Do
                    read_st = reader.ReadLine()

                    If read_st = "" Then
                        Exit Do
                    End If

                    read_st = read_st.Trim()
                    split_st = Split(read_st, ",")

                    'Check Start Wavelength 
                    If counter = 0 Then
                        If CDbl(split_st(0)) <> tslitem.startwave Then
                            MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                            reader.Close()
                            Exit Sub
                        End If
                    End If

                    'hold wavelength data
                    wavelength = CDbl(split_st(0))

                    For loop2 = 0 To ch_count - 1
                        power(loop2).Add(CDbl(split_st(loop2 + 1)))
                    Next

                    For loop2 = 0 To ch_count - 1
                        monitor(loop2).Add(CDbl(split_st(ch_count + loop2 + 1)))
                    Next

                    counter = counter + 1

                Loop

                reader.Close()

                'Check Stop wavelength 
                If wavelength <> tslitem.stopwave Then
                    MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                    Exit Sub
                End If

                'check number of point 

                Dim datapoint As Integer                            'number of data point 

                datapoint = (Math.Abs(tslitem.stopwave - tslitem.startwave) / tslitem.wavestep) + 1

                If datapoint <> monitor(0).Count Then
                    MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                    Exit Sub
                End If



                '----Add in  data to STS Process class
                Dim errorcode As Integer                            'Errorcode
                counter = 0

                For Each item In lst_refdata_struct
                    'Add in reference data of rescaled.
                    errorcode = Cal_STS(loop1).Add_Ref_Rawdata(power(counter).ToArray, monitor(counter).ToArray, item)

                    If errorcode <> 0 Then
                        Show_Inst_Error(errorcode)
                        Exit Sub
                    End If
                    counter = counter + 1
                Next

            Else

                Dim power() As List(Of Single)                 'Power data list 
                Dim monitor As New List(Of Single)             'Monitordata
                Dim counter As Integer                         'Counter
                Dim wavelength As Double                       'Read Wavelength 


                ReDim power(ch_count - 1)

                For loop2 = 0 To ch_count - 1
                    power(loop2) = New List(Of Single)
                Next


                Do
                    read_st = reader.ReadLine()

                    If read_st = "" Then
                        Exit Do
                    End If

                    read_st = read_st.Trim()
                    split_st = Split(read_st, ",")

                    'Check Start Wavelength 
                    If counter = 0 Then
                        If CDbl(split_st(0)) <> tslitem.startwave Then
                            MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                            reader.Close()
                            Exit Sub
                        End If
                    End If

                    'hold wavelength data
                    wavelength = CDbl(split_st(0))

                    For loop2 = 0 To ch_count - 1
                        power(loop2).Add(CDbl(split_st(loop2 + 1)))
                    Next

                    monitor.Add(CDbl(split_st(ch_count + 1)))

                    counter = counter + 1

                Loop

                reader.Close()

                'Check Stop wavelength 
                If wavelength <> tslitem.stopwave Then
                    MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                    Exit Sub
                End If

                'check number of point 

                Dim datapoint As Integer                            'number of data point 

                datapoint = (Math.Abs(tslitem.stopwave - tslitem.startwave) / tslitem.wavestep) + 1

                If datapoint <> monitor.Count Then
                    MsgBox("Reference data mismatch.Please selecet right data.", vbOKOnly)
                    Exit Sub
                End If



                '----Add in  data to STS Process class
                Dim errorcode As Integer                            'Errorcode
                counter = 0

                For Each item In lst_refdata_struct
                    'Add in reference data of rescaled.
                    errorcode = Cal_STS(loop1).Add_Ref_Rawdata(power(counter).ToArray, monitor.ToArray, item)

                    If errorcode <> 0 Then
                        Show_Inst_Error(errorcode)
                        Exit Sub
                    End If
                    counter = counter + 1
                Next
            End If
        Next


        MsgBox("Completed.", vbOKOnly)

    End Sub

End Class