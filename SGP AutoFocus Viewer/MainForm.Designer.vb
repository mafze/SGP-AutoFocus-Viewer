<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Button_OpenSGPFile = New System.Windows.Forms.Button()
        Me.OpenFD = New System.Windows.Forms.OpenFileDialog()
        Me.Button_AFGraphs = New System.Windows.Forms.Button()
        Me.Button_ReloadFile = New System.Windows.Forms.Button()
        Me.Button_PosTempGraph = New System.Windows.Forms.Button()
        Me.Button_ScanDetails = New System.Windows.Forms.Button()
        Me.CheckBox_ShowAllRuns = New System.Windows.Forms.CheckBox()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPage_AFRunList = New System.Windows.Forms.TabPage()
        Me.TextBox_FocusData = New System.Windows.Forms.TextBox()
        Me.TabPage_Analysis = New System.Windows.Forms.TabPage()
        Me.TextBox_Analysis = New System.Windows.Forms.TextBox()
        Me.Button_Advanced = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl.SuspendLayout()
        Me.TabPage_AFRunList.SuspendLayout()
        Me.TabPage_Analysis.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_OpenSGPFile
        '
        Me.Button_OpenSGPFile.Location = New System.Drawing.Point(16, 17)
        Me.Button_OpenSGPFile.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_OpenSGPFile.Name = "Button_OpenSGPFile"
        Me.Button_OpenSGPFile.Size = New System.Drawing.Size(114, 23)
        Me.Button_OpenSGPFile.TabIndex = 0
        Me.Button_OpenSGPFile.Text = "Open SGP Logfile"
        Me.ToolTip.SetToolTip(Me.Button_OpenSGPFile, "Open a standard SGP log file to analyze the recorded AF runs. Supports multiple f" &
        "ile selections.")
        Me.Button_OpenSGPFile.UseVisualStyleBackColor = True
        '
        'OpenFD
        '
        Me.OpenFD.RestoreDirectory = True
        '
        'Button_AFGraphs
        '
        Me.Button_AFGraphs.Location = New System.Drawing.Point(350, 17)
        Me.Button_AFGraphs.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_AFGraphs.Name = "Button_AFGraphs"
        Me.Button_AFGraphs.Size = New System.Drawing.Size(96, 23)
        Me.Button_AFGraphs.TabIndex = 2
        Me.Button_AFGraphs.Text = "AF Run Graphs"
        Me.Button_AFGraphs.UseVisualStyleBackColor = True
        '
        'Button_ReloadFile
        '
        Me.Button_ReloadFile.Location = New System.Drawing.Point(146, 17)
        Me.Button_ReloadFile.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_ReloadFile.Name = "Button_ReloadFile"
        Me.Button_ReloadFile.Size = New System.Drawing.Size(77, 23)
        Me.Button_ReloadFile.TabIndex = 3
        Me.Button_ReloadFile.Text = "Reload file"
        Me.Button_ReloadFile.UseVisualStyleBackColor = True
        '
        'Button_PosTempGraph
        '
        Me.Button_PosTempGraph.Location = New System.Drawing.Point(460, 17)
        Me.Button_PosTempGraph.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_PosTempGraph.Name = "Button_PosTempGraph"
        Me.Button_PosTempGraph.Size = New System.Drawing.Size(117, 23)
        Me.Button_PosTempGraph.TabIndex = 4
        Me.Button_PosTempGraph.Text = "Position-Temp Graph"
        Me.Button_PosTempGraph.UseVisualStyleBackColor = True
        '
        'Button_ScanDetails
        '
        Me.Button_ScanDetails.Location = New System.Drawing.Point(239, 17)
        Me.Button_ScanDetails.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_ScanDetails.Name = "Button_ScanDetails"
        Me.Button_ScanDetails.Size = New System.Drawing.Size(96, 23)
        Me.Button_ScanDetails.TabIndex = 6
        Me.Button_ScanDetails.Text = "Scan details"
        Me.Button_ScanDetails.UseVisualStyleBackColor = True
        '
        'CheckBox_ShowAllRuns
        '
        Me.CheckBox_ShowAllRuns.AutoSize = True
        Me.CheckBox_ShowAllRuns.Location = New System.Drawing.Point(596, 19)
        Me.CheckBox_ShowAllRuns.Margin = New System.Windows.Forms.Padding(1)
        Me.CheckBox_ShowAllRuns.Name = "CheckBox_ShowAllRuns"
        Me.CheckBox_ShowAllRuns.Size = New System.Drawing.Size(105, 17)
        Me.CheckBox_ShowAllRuns.TabIndex = 7
        Me.CheckBox_ShowAllRuns.Text = "Show all AF runs"
        Me.ToolTip.SetToolTip(Me.CheckBox_ShowAllRuns, "Show also incomplete AF runs (aborted or failed). Note that the logfile must be r" &
        "e-loaded.")
        Me.CheckBox_ShowAllRuns.UseVisualStyleBackColor = True
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPage_AFRunList)
        Me.TabControl.Controls.Add(Me.TabPage_Analysis)
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Location = New System.Drawing.Point(0, 83)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1062, 465)
        Me.TabControl.TabIndex = 10
        '
        'TabPage_AFRunList
        '
        Me.TabPage_AFRunList.Controls.Add(Me.TextBox_FocusData)
        Me.TabPage_AFRunList.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_AFRunList.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage_AFRunList.Name = "TabPage_AFRunList"
        Me.TabPage_AFRunList.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage_AFRunList.Size = New System.Drawing.Size(1054, 439)
        Me.TabPage_AFRunList.TabIndex = 0
        Me.TabPage_AFRunList.Text = "AF Run List"
        Me.TabPage_AFRunList.UseVisualStyleBackColor = True
        '
        'TextBox_FocusData
        '
        Me.TextBox_FocusData.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_FocusData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_FocusData.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_FocusData.Location = New System.Drawing.Point(2, 2)
        Me.TextBox_FocusData.Margin = New System.Windows.Forms.Padding(1)
        Me.TextBox_FocusData.Multiline = True
        Me.TextBox_FocusData.Name = "TextBox_FocusData"
        Me.TextBox_FocusData.ReadOnly = True
        Me.TextBox_FocusData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox_FocusData.Size = New System.Drawing.Size(1050, 435)
        Me.TextBox_FocusData.TabIndex = 1
        Me.TextBox_FocusData.WordWrap = False
        '
        'TabPage_Analysis
        '
        Me.TabPage_Analysis.Controls.Add(Me.TextBox_Analysis)
        Me.TabPage_Analysis.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Analysis.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage_Analysis.Name = "TabPage_Analysis"
        Me.TabPage_Analysis.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage_Analysis.Size = New System.Drawing.Size(1054, 439)
        Me.TabPage_Analysis.TabIndex = 1
        Me.TabPage_Analysis.Text = "Temp & Filter Analysis"
        Me.TabPage_Analysis.UseVisualStyleBackColor = True
        '
        'TextBox_Analysis
        '
        Me.TextBox_Analysis.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_Analysis.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_Analysis.Location = New System.Drawing.Point(2, 2)
        Me.TextBox_Analysis.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox_Analysis.Multiline = True
        Me.TextBox_Analysis.Name = "TextBox_Analysis"
        Me.TextBox_Analysis.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Analysis.Size = New System.Drawing.Size(1050, 435)
        Me.TextBox_Analysis.TabIndex = 0
        '
        'Button_Advanced
        '
        Me.Button_Advanced.Location = New System.Drawing.Point(596, 44)
        Me.Button_Advanced.Margin = New System.Windows.Forms.Padding(2)
        Me.Button_Advanced.Name = "Button_Advanced"
        Me.Button_Advanced.Size = New System.Drawing.Size(75, 23)
        Me.Button_Advanced.TabIndex = 14
        Me.Button_Advanced.Text = "Options"
        Me.Button_Advanced.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button_PosTempGraph)
        Me.Panel1.Controls.Add(Me.Button_Advanced)
        Me.Panel1.Controls.Add(Me.Button_OpenSGPFile)
        Me.Panel1.Controls.Add(Me.Button_AFGraphs)
        Me.Panel1.Controls.Add(Me.CheckBox_ShowAllRuns)
        Me.Panel1.Controls.Add(Me.Button_ReloadFile)
        Me.Panel1.Controls.Add(Me.Button_ScanDetails)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1062, 83)
        Me.Panel1.TabIndex = 15
        '
        'ToolTip
        '
        Me.ToolTip.AutoPopDelay = 20000
        Me.ToolTip.InitialDelay = 500
        Me.ToolTip.ReshowDelay = 100
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1062, 548)
        Me.Controls.Add(Me.TabControl)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "MainForm"
        Me.Text = "SGP Auto Focus LogViewer 1.26"
        Me.TabControl.ResumeLayout(False)
        Me.TabPage_AFRunList.ResumeLayout(False)
        Me.TabPage_AFRunList.PerformLayout()
        Me.TabPage_Analysis.ResumeLayout(False)
        Me.TabPage_Analysis.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_OpenSGPFile As Button
    Friend WithEvents OpenFD As OpenFileDialog
    Friend WithEvents Button_AFGraphs As Button
    Friend WithEvents Button_ReloadFile As Button
    Friend WithEvents Button_PosTempGraph As Button
    Friend WithEvents Button_ScanDetails As Button
    Friend WithEvents CheckBox_ShowAllRuns As CheckBox
    Friend WithEvents TabControl As TabControl
    Friend WithEvents TabPage_AFRunList As TabPage
    Friend WithEvents TextBox_FocusData As TextBox
    Friend WithEvents TabPage_Analysis As TabPage
    Friend WithEvents TextBox_Analysis As TextBox
    Friend WithEvents Button_Advanced As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolTip As ToolTip
End Class
