<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FocusRunGraph
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FocusRunGraph))
        Me.Chart_AFRun = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DropDownList_AFruns = New System.Windows.Forms.ComboBox()
        Me.Button_SaveToBMP = New System.Windows.Forms.Button()
        Me.CheckBox_ExInclPoint = New System.Windows.Forms.CheckBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.Chart_AFRun, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart_AFRun
        '
        ChartArea1.AxisX.Title = "XXX"
        ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.Title = "XXX"
        ChartArea1.AxisY.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.InnerPlotPosition.Auto = False
        ChartArea1.InnerPlotPosition.Height = 80.0!
        ChartArea1.InnerPlotPosition.Width = 90.0!
        ChartArea1.InnerPlotPosition.X = 10.0!
        ChartArea1.InnerPlotPosition.Y = 10.0!
        ChartArea1.Name = "ChartArea"
        Me.Chart_AFRun.ChartAreas.Add(ChartArea1)
        Me.Chart_AFRun.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Enabled = False
        Legend1.Name = "AFLegends"
        Legend1.Title = "Auto Focus Runs"
        Me.Chart_AFRun.Legends.Add(Legend1)
        Me.Chart_AFRun.Location = New System.Drawing.Point(0, 0)
        Me.Chart_AFRun.Margin = New System.Windows.Forms.Padding(1)
        Me.Chart_AFRun.Name = "Chart_AFRun"
        Series1.ChartArea = "ChartArea"
        Series1.Legend = "AFLegends"
        Series1.Name = "Series1"
        Me.Chart_AFRun.Series.Add(Series1)
        Me.Chart_AFRun.Size = New System.Drawing.Size(626, 384)
        Me.Chart_AFRun.TabIndex = 1
        Me.Chart_AFRun.TabStop = False
        Me.Chart_AFRun.Text = "Auto Focus Run"
        '
        'DropDownList_AFruns
        '
        Me.DropDownList_AFruns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DropDownList_AFruns.FormattingEnabled = True
        Me.DropDownList_AFruns.Location = New System.Drawing.Point(104, 32)
        Me.DropDownList_AFruns.Margin = New System.Windows.Forms.Padding(1)
        Me.DropDownList_AFruns.MaxDropDownItems = 100
        Me.DropDownList_AFruns.Name = "DropDownList_AFruns"
        Me.DropDownList_AFruns.Size = New System.Drawing.Size(339, 21)
        Me.DropDownList_AFruns.TabIndex = 0
        '
        'Button_SaveToBMP
        '
        Me.Button_SaveToBMP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveToBMP.Location = New System.Drawing.Point(0, 363)
        Me.Button_SaveToBMP.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_SaveToBMP.Name = "Button_SaveToBMP"
        Me.Button_SaveToBMP.Size = New System.Drawing.Size(94, 21)
        Me.Button_SaveToBMP.TabIndex = 16
        Me.Button_SaveToBMP.TabStop = False
        Me.Button_SaveToBMP.Text = "Save To JPG"
        Me.Button_SaveToBMP.UseVisualStyleBackColor = True
        '
        'CheckBox_ExInclPoint
        '
        Me.CheckBox_ExInclPoint.AutoSize = True
        Me.CheckBox_ExInclPoint.Location = New System.Drawing.Point(458, 34)
        Me.CheckBox_ExInclPoint.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBox_ExInclPoint.Name = "CheckBox_ExInclPoint"
        Me.CheckBox_ExInclPoint.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_ExInclPoint.TabIndex = 17
        Me.CheckBox_ExInclPoint.TabStop = False
        Me.ToolTip.SetToolTip(Me.CheckBox_ExInclPoint, "Include/exclude AF run from the Position vs. Temperature analysis. Note that this" &
        " has the same affect as clicking on the same data point in the Position-Temp gra" &
        "ph.")
        Me.CheckBox_ExInclPoint.UseVisualStyleBackColor = True
        Me.CheckBox_ExInclPoint.Visible = False
        '
        'ToolTip
        '
        Me.ToolTip.AutoPopDelay = 20000
        Me.ToolTip.InitialDelay = 500
        Me.ToolTip.ReshowDelay = 100
        '
        'FocusRunGraph
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(626, 384)
        Me.Controls.Add(Me.CheckBox_ExInclPoint)
        Me.Controls.Add(Me.Button_SaveToBMP)
        Me.Controls.Add(Me.DropDownList_AFruns)
        Me.Controls.Add(Me.Chart_AFRun)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "FocusRunGraph"
        Me.Text = "Auto Focus Graphs"
        CType(Me.Chart_AFRun, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Chart_AFRun As DataVisualization.Charting.Chart
    Friend WithEvents DropDownList_AFruns As ComboBox
    Friend WithEvents Button_SaveToBMP As Button
    Friend WithEvents CheckBox_ExInclPoint As CheckBox
    Friend WithEvents ToolTip As ToolTip
End Class
