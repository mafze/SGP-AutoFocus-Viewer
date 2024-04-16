<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PositionTempGraph
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
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PositionTempGraph))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DropDownList_Filters = New System.Windows.Forms.ComboBox()
        Me.Label_Fit = New System.Windows.Forms.Label()
        Me.Chart_PosTemp = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Button_SaveToBMP = New System.Windows.Forms.Button()
        Me.Button_ClearExPoints = New System.Windows.Forms.Button()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.Chart_PosTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(130, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Analyze filter"
        '
        'DropDownList_Filters
        '
        Me.DropDownList_Filters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DropDownList_Filters.FormattingEnabled = True
        Me.DropDownList_Filters.Location = New System.Drawing.Point(221, 14)
        Me.DropDownList_Filters.Margin = New System.Windows.Forms.Padding(1)
        Me.DropDownList_Filters.MaxDropDownItems = 100
        Me.DropDownList_Filters.Name = "DropDownList_Filters"
        Me.DropDownList_Filters.Size = New System.Drawing.Size(104, 21)
        Me.DropDownList_Filters.TabIndex = 13
        '
        'Label_Fit
        '
        Me.Label_Fit.AutoSize = True
        Me.Label_Fit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Fit.Location = New System.Drawing.Point(370, 16)
        Me.Label_Fit.Name = "Label_Fit"
        Me.Label_Fit.Size = New System.Drawing.Size(55, 16)
        Me.Label_Fit.TabIndex = 12
        Me.Label_Fit.Text = "Label1"
        '
        'Chart_PosTemp
        '
        ChartArea1.AxisX.Title = "XXX"
        ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.IsStartedFromZero = False
        ChartArea1.AxisY.Title = "XXX"
        ChartArea1.AxisY.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.InnerPlotPosition.Auto = False
        ChartArea1.InnerPlotPosition.Height = 80.0!
        ChartArea1.InnerPlotPosition.Width = 90.0!
        ChartArea1.InnerPlotPosition.X = 10.0!
        ChartArea1.InnerPlotPosition.Y = 8.0!
        ChartArea1.Name = "ChartArea"
        Me.Chart_PosTemp.ChartAreas.Add(ChartArea1)
        Me.Chart_PosTemp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Chart_PosTemp.Location = New System.Drawing.Point(0, 0)
        Me.Chart_PosTemp.Margin = New System.Windows.Forms.Padding(0)
        Me.Chart_PosTemp.Name = "Chart_PosTemp"
        Series1.ChartArea = "ChartArea"
        Series1.Name = "Series1"
        Series2.ChartArea = "ChartArea"
        Series2.Name = "Series2"
        Me.Chart_PosTemp.Series.Add(Series1)
        Me.Chart_PosTemp.Series.Add(Series2)
        Me.Chart_PosTemp.Size = New System.Drawing.Size(556, 404)
        Me.Chart_PosTemp.TabIndex = 11
        Me.Chart_PosTemp.Text = "Focus Position History"
        '
        'Button_SaveToBMP
        '
        Me.Button_SaveToBMP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveToBMP.Location = New System.Drawing.Point(0, 383)
        Me.Button_SaveToBMP.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_SaveToBMP.Name = "Button_SaveToBMP"
        Me.Button_SaveToBMP.Size = New System.Drawing.Size(94, 21)
        Me.Button_SaveToBMP.TabIndex = 15
        Me.Button_SaveToBMP.Text = "Save To JPG"
        Me.Button_SaveToBMP.UseVisualStyleBackColor = True
        '
        'Button_ClearExPoints
        '
        Me.Button_ClearExPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ClearExPoints.Location = New System.Drawing.Point(438, 383)
        Me.Button_ClearExPoints.Margin = New System.Windows.Forms.Padding(1)
        Me.Button_ClearExPoints.Name = "Button_ClearExPoints"
        Me.Button_ClearExPoints.Size = New System.Drawing.Size(118, 21)
        Me.Button_ClearExPoints.TabIndex = 16
        Me.Button_ClearExPoints.Text = "Clear Excluded Points"
        Me.ToolTip.SetToolTip(Me.Button_ClearExPoints, "Clear the entire list of excluded data points. A data point can be excluded/inclu" &
        "ded by clicking on it.")
        Me.Button_ClearExPoints.UseVisualStyleBackColor = True
        '
        'ToolTip
        '
        Me.ToolTip.AutoPopDelay = 20000
        Me.ToolTip.InitialDelay = 500
        Me.ToolTip.ReshowDelay = 100
        '
        'PositionTempGraph
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(556, 404)
        Me.Controls.Add(Me.Button_ClearExPoints)
        Me.Controls.Add(Me.Button_SaveToBMP)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label_Fit)
        Me.Controls.Add(Me.DropDownList_Filters)
        Me.Controls.Add(Me.Chart_PosTemp)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "PositionTempGraph"
        Me.Text = "Position vs Temperature Graph"
        CType(Me.Chart_PosTemp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents DropDownList_Filters As ComboBox
    Friend WithEvents Label_Fit As Label
    Friend WithEvents Chart_PosTemp As DataVisualization.Charting.Chart
    Friend WithEvents Button_SaveToBMP As Button
    Friend WithEvents Button_ClearExPoints As Button
    Friend WithEvents ToolTip As ToolTip
End Class
