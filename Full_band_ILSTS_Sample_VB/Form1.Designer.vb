<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnget_reference = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtpower = New System.Windows.Forms.TextBox()
        Me.cmbspeed = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtstartwave = New System.Windows.Forms.TextBox()
        Me.txtstepwave = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtstopwave = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.btntslsetcheck = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtspecminwave4 = New System.Windows.Forms.TextBox()
        Me.txtspecmaxwave4 = New System.Windows.Forms.TextBox()
        Me.txtstartwave4 = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtstopwave4 = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtspecminwave3 = New System.Windows.Forms.TextBox()
        Me.txtspecmaxwave3 = New System.Windows.Forms.TextBox()
        Me.txtstartwave3 = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtstopwave3 = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtspecminwave2 = New System.Windows.Forms.TextBox()
        Me.txtspecmaxwave2 = New System.Windows.Forms.TextBox()
        Me.txtstartwave2 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtstopwave2 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtspecminwave1 = New System.Windows.Forms.TextBox()
        Me.txtspecmaxwave1 = New System.Windows.Forms.TextBox()
        Me.txtstartwave1 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtstopwave1 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.toolstatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.toolmessage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkeach_ch = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnsaveRawdata = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chklst_range = New System.Windows.Forms.CheckedListBox()
        Me.chklst_ch = New System.Windows.Forms.CheckedListBox()
        Me.btnsaveref_rawdata = New System.Windows.Forms.Button()
        Me.btnset = New System.Windows.Forms.Button()
        Me.btnmeas = New System.Windows.Forms.Button()
        Me.groupbox_tsl = New System.Windows.Forms.GroupBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.groupbox_tsl.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(494, 25)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(133, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Spec MaxWavelength(nm)"
        '
        'btnget_reference
        '
        Me.btnget_reference.Location = New System.Drawing.Point(465, 38)
        Me.btnget_reference.Name = "btnget_reference"
        Me.btnget_reference.Size = New System.Drawing.Size(102, 35)
        Me.btnget_reference.TabIndex = 32
        Me.btnget_reference.Text = "Reference"
        Me.btnget_reference.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "StartWavelength(nm)"
        '
        'txtpower
        '
        Me.txtpower.Location = New System.Drawing.Point(549, 54)
        Me.txtpower.Name = "txtpower"
        Me.txtpower.Size = New System.Drawing.Size(117, 20)
        Me.txtpower.TabIndex = 4
        Me.txtpower.Text = "0"
        '
        'cmbspeed
        '
        Me.cmbspeed.FormattingEnabled = True
        Me.cmbspeed.Location = New System.Drawing.Point(424, 54)
        Me.cmbspeed.Name = "cmbspeed"
        Me.cmbspeed.Size = New System.Drawing.Size(102, 21)
        Me.cmbspeed.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(548, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "TSL Power(dBm)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(164, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "StopWavelength(nm)"
        '
        'txtstartwave
        '
        Me.txtstartwave.Location = New System.Drawing.Point(14, 54)
        Me.txtstartwave.Name = "txtstartwave"
        Me.txtstartwave.Size = New System.Drawing.Size(117, 20)
        Me.txtstartwave.TabIndex = 0
        '
        'txtstepwave
        '
        Me.txtstepwave.Location = New System.Drawing.Point(302, 54)
        Me.txtstepwave.Name = "txtstepwave"
        Me.txtstepwave.Size = New System.Drawing.Size(100, 20)
        Me.txtstepwave.TabIndex = 2
        Me.txtstepwave.Text = "0.01"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(422, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(113, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "SweepSpeed(nm/sec)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(299, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "WavelengthStep(nm)"
        '
        'txtstopwave
        '
        Me.txtstopwave.Location = New System.Drawing.Point(165, 54)
        Me.txtstopwave.Name = "txtstopwave"
        Me.txtstopwave.Size = New System.Drawing.Size(117, 20)
        Me.txtstopwave.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(10, 121)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(717, 478)
        Me.TabControl1.TabIndex = 13
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.btntslsetcheck)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage1.Size = New System.Drawing.Size(709, 452)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TSL Parameter"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btntslsetcheck
        '
        Me.btntslsetcheck.Location = New System.Drawing.Point(561, 400)
        Me.btntslsetcheck.Name = "btntslsetcheck"
        Me.btntslsetcheck.Size = New System.Drawing.Size(132, 35)
        Me.btntslsetcheck.TabIndex = 36
        Me.btntslsetcheck.Text = "Parameter check"
        Me.btntslsetcheck.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.GroupBox7)
        Me.GroupBox1.Controls.Add(Me.GroupBox6)
        Me.GroupBox1.Controls.Add(Me.GroupBox5)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 16)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(692, 371)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Sweep Setting"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label20)
        Me.GroupBox7.Controls.Add(Me.Label21)
        Me.GroupBox7.Controls.Add(Me.txtspecminwave4)
        Me.GroupBox7.Controls.Add(Me.txtspecmaxwave4)
        Me.GroupBox7.Controls.Add(Me.txtstartwave4)
        Me.GroupBox7.Controls.Add(Me.Label22)
        Me.GroupBox7.Controls.Add(Me.txtstopwave4)
        Me.GroupBox7.Controls.Add(Me.Label23)
        Me.GroupBox7.Location = New System.Drawing.Point(12, 284)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox7.Size = New System.Drawing.Size(666, 69)
        Me.GroupBox7.TabIndex = 10
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "port4"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(494, 25)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(133, 13)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "Spec MaxWavelength(nm)"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(164, 25)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(107, 13)
        Me.Label21.TabIndex = 6
        Me.Label21.Text = "StopWavelength(nm)"
        '
        'txtspecminwave4
        '
        Me.txtspecminwave4.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecminwave4.Enabled = False
        Me.txtspecminwave4.Location = New System.Drawing.Point(345, 41)
        Me.txtspecminwave4.Name = "txtspecminwave4"
        Me.txtspecminwave4.Size = New System.Drawing.Size(117, 20)
        Me.txtspecminwave4.TabIndex = 0
        '
        'txtspecmaxwave4
        '
        Me.txtspecmaxwave4.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecmaxwave4.Enabled = False
        Me.txtspecmaxwave4.Location = New System.Drawing.Point(496, 41)
        Me.txtspecmaxwave4.Name = "txtspecmaxwave4"
        Me.txtspecmaxwave4.Size = New System.Drawing.Size(117, 20)
        Me.txtspecmaxwave4.TabIndex = 1
        '
        'txtstartwave4
        '
        Me.txtstartwave4.Enabled = False
        Me.txtstartwave4.Location = New System.Drawing.Point(15, 41)
        Me.txtstartwave4.Name = "txtstartwave4"
        Me.txtstartwave4.Size = New System.Drawing.Size(117, 20)
        Me.txtstartwave4.TabIndex = 0
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(343, 24)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(130, 13)
        Me.Label22.TabIndex = 5
        Me.Label22.Text = "Spec MinWavelength(nm)"
        '
        'txtstopwave4
        '
        Me.txtstopwave4.Enabled = False
        Me.txtstopwave4.Location = New System.Drawing.Point(166, 41)
        Me.txtstopwave4.Name = "txtstopwave4"
        Me.txtstopwave4.Size = New System.Drawing.Size(117, 20)
        Me.txtstopwave4.TabIndex = 1
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(13, 24)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(107, 13)
        Me.Label23.TabIndex = 5
        Me.Label23.Text = "StartWavelength(nm)"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label16)
        Me.GroupBox6.Controls.Add(Me.Label17)
        Me.GroupBox6.Controls.Add(Me.txtspecminwave3)
        Me.GroupBox6.Controls.Add(Me.txtspecmaxwave3)
        Me.GroupBox6.Controls.Add(Me.txtstartwave3)
        Me.GroupBox6.Controls.Add(Me.Label18)
        Me.GroupBox6.Controls.Add(Me.txtstopwave3)
        Me.GroupBox6.Controls.Add(Me.Label19)
        Me.GroupBox6.Location = New System.Drawing.Point(12, 195)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox6.Size = New System.Drawing.Size(666, 69)
        Me.GroupBox6.TabIndex = 10
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "port3"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(494, 25)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(133, 13)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Spec MaxWavelength(nm)"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(164, 25)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(107, 13)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "StopWavelength(nm)"
        '
        'txtspecminwave3
        '
        Me.txtspecminwave3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecminwave3.Enabled = False
        Me.txtspecminwave3.Location = New System.Drawing.Point(345, 41)
        Me.txtspecminwave3.Name = "txtspecminwave3"
        Me.txtspecminwave3.Size = New System.Drawing.Size(117, 20)
        Me.txtspecminwave3.TabIndex = 0
        '
        'txtspecmaxwave3
        '
        Me.txtspecmaxwave3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecmaxwave3.Enabled = False
        Me.txtspecmaxwave3.Location = New System.Drawing.Point(496, 41)
        Me.txtspecmaxwave3.Name = "txtspecmaxwave3"
        Me.txtspecmaxwave3.Size = New System.Drawing.Size(117, 20)
        Me.txtspecmaxwave3.TabIndex = 1
        '
        'txtstartwave3
        '
        Me.txtstartwave3.Enabled = False
        Me.txtstartwave3.Location = New System.Drawing.Point(15, 41)
        Me.txtstartwave3.Name = "txtstartwave3"
        Me.txtstartwave3.Size = New System.Drawing.Size(117, 20)
        Me.txtstartwave3.TabIndex = 0
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(343, 24)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(130, 13)
        Me.Label18.TabIndex = 5
        Me.Label18.Text = "Spec MinWavelength(nm)"
        '
        'txtstopwave3
        '
        Me.txtstopwave3.Enabled = False
        Me.txtstopwave3.Location = New System.Drawing.Point(166, 41)
        Me.txtstopwave3.Name = "txtstopwave3"
        Me.txtstopwave3.Size = New System.Drawing.Size(117, 20)
        Me.txtstopwave3.TabIndex = 1
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(13, 24)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(107, 13)
        Me.Label19.TabIndex = 5
        Me.Label19.Text = "StartWavelength(nm)"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.txtspecminwave2)
        Me.GroupBox5.Controls.Add(Me.txtspecmaxwave2)
        Me.GroupBox5.Controls.Add(Me.txtstartwave2)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.txtstopwave2)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Location = New System.Drawing.Point(12, 111)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox5.Size = New System.Drawing.Size(666, 69)
        Me.GroupBox5.TabIndex = 10
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "port2"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(494, 25)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(133, 13)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Spec MaxWavelength(nm)"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(164, 25)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(107, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "StopWavelength(nm)"
        '
        'txtspecminwave2
        '
        Me.txtspecminwave2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecminwave2.Enabled = False
        Me.txtspecminwave2.Location = New System.Drawing.Point(345, 41)
        Me.txtspecminwave2.Name = "txtspecminwave2"
        Me.txtspecminwave2.Size = New System.Drawing.Size(117, 20)
        Me.txtspecminwave2.TabIndex = 0
        '
        'txtspecmaxwave2
        '
        Me.txtspecmaxwave2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecmaxwave2.Enabled = False
        Me.txtspecmaxwave2.Location = New System.Drawing.Point(496, 41)
        Me.txtspecmaxwave2.Name = "txtspecmaxwave2"
        Me.txtspecmaxwave2.Size = New System.Drawing.Size(117, 20)
        Me.txtspecmaxwave2.TabIndex = 1
        '
        'txtstartwave2
        '
        Me.txtstartwave2.Enabled = False
        Me.txtstartwave2.Location = New System.Drawing.Point(15, 41)
        Me.txtstartwave2.Name = "txtstartwave2"
        Me.txtstartwave2.Size = New System.Drawing.Size(117, 20)
        Me.txtstartwave2.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(343, 24)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(130, 13)
        Me.Label14.TabIndex = 5
        Me.Label14.Text = "Spec MinWavelength(nm)"
        '
        'txtstopwave2
        '
        Me.txtstopwave2.Enabled = False
        Me.txtstopwave2.Location = New System.Drawing.Point(166, 41)
        Me.txtstopwave2.Name = "txtstopwave2"
        Me.txtstopwave2.Size = New System.Drawing.Size(117, 20)
        Me.txtstopwave2.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(13, 24)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(107, 13)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = "StartWavelength(nm)"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.txtspecminwave1)
        Me.GroupBox3.Controls.Add(Me.txtspecmaxwave1)
        Me.GroupBox3.Controls.Add(Me.txtstartwave1)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.txtstopwave1)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 26)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox3.Size = New System.Drawing.Size(666, 69)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "port1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(164, 25)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(107, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "StopWavelength(nm)"
        '
        'txtspecminwave1
        '
        Me.txtspecminwave1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecminwave1.Enabled = False
        Me.txtspecminwave1.Location = New System.Drawing.Point(345, 41)
        Me.txtspecminwave1.Name = "txtspecminwave1"
        Me.txtspecminwave1.Size = New System.Drawing.Size(117, 20)
        Me.txtspecminwave1.TabIndex = 0
        '
        'txtspecmaxwave1
        '
        Me.txtspecmaxwave1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtspecmaxwave1.Enabled = False
        Me.txtspecmaxwave1.Location = New System.Drawing.Point(496, 41)
        Me.txtspecmaxwave1.Name = "txtspecmaxwave1"
        Me.txtspecmaxwave1.Size = New System.Drawing.Size(117, 20)
        Me.txtspecmaxwave1.TabIndex = 1
        '
        'txtstartwave1
        '
        Me.txtstartwave1.Enabled = False
        Me.txtstartwave1.Location = New System.Drawing.Point(15, 41)
        Me.txtstartwave1.Name = "txtstartwave1"
        Me.txtstartwave1.Size = New System.Drawing.Size(117, 20)
        Me.txtstartwave1.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(343, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(130, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Spec MinWavelength(nm)"
        '
        'txtstopwave1
        '
        Me.txtstopwave1.Enabled = False
        Me.txtstopwave1.Location = New System.Drawing.Point(166, 41)
        Me.txtstopwave1.Name = "txtstopwave1"
        Me.txtstopwave1.Size = New System.Drawing.Size(117, 20)
        Me.txtstopwave1.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "StartWavelength(nm)"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.StatusStrip1)
        Me.TabPage2.Controls.Add(Me.chkeach_ch)
        Me.TabPage2.Controls.Add(Me.Button1)
        Me.TabPage2.Controls.Add(Me.btnsaveRawdata)
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.btnsaveref_rawdata)
        Me.TabPage2.Controls.Add(Me.btnset)
        Me.TabPage2.Controls.Add(Me.btnmeas)
        Me.TabPage2.Controls.Add(Me.btnget_reference)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage2.Size = New System.Drawing.Size(709, 452)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Measurement and Save"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolstatus, Me.toolmessage})
        Me.StatusStrip1.Location = New System.Drawing.Point(2, 428)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(705, 22)
        Me.StatusStrip1.TabIndex = 44
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'toolstatus
        '
        Me.toolstatus.Name = "toolstatus"
        Me.toolstatus.Size = New System.Drawing.Size(0, 17)
        '
        'toolmessage
        '
        Me.toolmessage.Name = "toolmessage"
        Me.toolmessage.Size = New System.Drawing.Size(0, 17)
        '
        'chkeach_ch
        '
        Me.chkeach_ch.AutoSize = True
        Me.chkeach_ch.Location = New System.Drawing.Point(364, 79)
        Me.chkeach_ch.Name = "chkeach_ch"
        Me.chkeach_ch.Size = New System.Drawing.Size(145, 17)
        Me.chkeach_ch.TabIndex = 42
        Me.chkeach_ch.Text = "each channel individually"
        Me.chkeach_ch.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(364, 150)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(132, 35)
        Me.Button1.TabIndex = 36
        Me.Button1.Text = "Read Reference Data"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnsaveRawdata
        '
        Me.btnsaveRawdata.Location = New System.Drawing.Point(168, 150)
        Me.btnsaveRawdata.Name = "btnsaveRawdata"
        Me.btnsaveRawdata.Size = New System.Drawing.Size(132, 35)
        Me.btnsaveRawdata.TabIndex = 35
        Me.btnsaveRawdata.Text = "Save Rawdata"
        Me.btnsaveRawdata.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.chklst_range)
        Me.GroupBox2.Controls.Add(Me.chklst_ch)
        Me.GroupBox2.Location = New System.Drawing.Point(5, 28)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(332, 96)
        Me.GroupBox2.TabIndex = 30
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Measurement ch And Range"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(176, 21)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Measurement Range"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Measurement ch"
        '
        'chklst_range
        '
        Me.chklst_range.FormattingEnabled = True
        Me.chklst_range.Location = New System.Drawing.Point(177, 37)
        Me.chklst_range.Name = "chklst_range"
        Me.chklst_range.Size = New System.Drawing.Size(118, 34)
        Me.chklst_range.TabIndex = 1
        '
        'chklst_ch
        '
        Me.chklst_ch.FormattingEnabled = True
        Me.chklst_ch.Location = New System.Drawing.Point(12, 37)
        Me.chklst_ch.Name = "chklst_ch"
        Me.chklst_ch.Size = New System.Drawing.Size(135, 34)
        Me.chklst_ch.TabIndex = 0
        '
        'btnsaveref_rawdata
        '
        Me.btnsaveref_rawdata.Location = New System.Drawing.Point(17, 150)
        Me.btnsaveref_rawdata.Name = "btnsaveref_rawdata"
        Me.btnsaveref_rawdata.Size = New System.Drawing.Size(132, 35)
        Me.btnsaveref_rawdata.TabIndex = 34
        Me.btnsaveref_rawdata.Text = "Save Reference Rawdata"
        Me.btnsaveref_rawdata.UseVisualStyleBackColor = True
        '
        'btnset
        '
        Me.btnset.Location = New System.Drawing.Point(364, 38)
        Me.btnset.Name = "btnset"
        Me.btnset.Size = New System.Drawing.Size(83, 35)
        Me.btnset.TabIndex = 31
        Me.btnset.Text = "SET"
        Me.btnset.UseVisualStyleBackColor = True
        '
        'btnmeas
        '
        Me.btnmeas.Location = New System.Drawing.Point(590, 38)
        Me.btnmeas.Name = "btnmeas"
        Me.btnmeas.Size = New System.Drawing.Size(106, 35)
        Me.btnmeas.TabIndex = 33
        Me.btnmeas.Text = "Measurement"
        Me.btnmeas.UseVisualStyleBackColor = True
        '
        'groupbox_tsl
        '
        Me.groupbox_tsl.Controls.Add(Me.Label1)
        Me.groupbox_tsl.Controls.Add(Me.txtpower)
        Me.groupbox_tsl.Controls.Add(Me.cmbspeed)
        Me.groupbox_tsl.Controls.Add(Me.Label5)
        Me.groupbox_tsl.Controls.Add(Me.Label2)
        Me.groupbox_tsl.Controls.Add(Me.txtstartwave)
        Me.groupbox_tsl.Controls.Add(Me.txtstepwave)
        Me.groupbox_tsl.Controls.Add(Me.Label4)
        Me.groupbox_tsl.Controls.Add(Me.Label3)
        Me.groupbox_tsl.Controls.Add(Me.txtstopwave)
        Me.groupbox_tsl.Location = New System.Drawing.Point(10, 11)
        Me.groupbox_tsl.Name = "groupbox_tsl"
        Me.groupbox_tsl.Size = New System.Drawing.Size(717, 93)
        Me.groupbox_tsl.TabIndex = 14
        Me.groupbox_tsl.TabStop = False
        Me.groupbox_tsl.Text = "Sweep Setting"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 609)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.groupbox_tsl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Full-Band STS IL sample software"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.groupbox_tsl.ResumeLayout(False)
        Me.groupbox_tsl.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label11 As Label
    Friend WithEvents btnget_reference As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtpower As TextBox
    Friend WithEvents cmbspeed As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtstartwave As TextBox
    Friend WithEvents txtstepwave As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtstopwave As TextBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents btntslsetcheck As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents txtspecminwave4 As TextBox
    Friend WithEvents txtspecmaxwave4 As TextBox
    Friend WithEvents txtstartwave4 As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents txtstopwave4 As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents txtspecminwave3 As TextBox
    Friend WithEvents txtspecmaxwave3 As TextBox
    Friend WithEvents txtstartwave3 As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtstopwave3 As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtspecminwave2 As TextBox
    Friend WithEvents txtspecmaxwave2 As TextBox
    Friend WithEvents txtstartwave2 As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtstopwave2 As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtspecminwave1 As TextBox
    Friend WithEvents txtspecmaxwave1 As TextBox
    Friend WithEvents txtstartwave1 As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtstopwave1 As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Button1 As Button
    Friend WithEvents btnsaveRawdata As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents chklst_range As CheckedListBox
    Friend WithEvents chklst_ch As CheckedListBox
    Friend WithEvents btnsaveref_rawdata As Button
    Friend WithEvents btnset As Button
    Friend WithEvents btnmeas As Button
    Friend WithEvents groupbox_tsl As GroupBox
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents chkeach_ch As CheckBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents toolstatus As ToolStripStatusLabel
    Friend WithEvents toolmessage As ToolStripStatusLabel
End Class
