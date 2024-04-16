<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsForm))
        Me.CheckBox_HyperbolicFit = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoScale = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ReplayMode = New System.Windows.Forms.CheckBox()
        Me.Label_SaveFile = New System.Windows.Forms.Label()
        Me.TextBox_SaveFilePath = New System.Windows.Forms.TextBox()
        Me.CheckBox_AutoSaveMode = New System.Windows.Forms.CheckBox()
        Me.DropDownList_Colors = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'CheckBox_HyperbolicFit
        '
        Me.CheckBox_HyperbolicFit.AutoSize = True
        Me.CheckBox_HyperbolicFit.Location = New System.Drawing.Point(12, 35)
        Me.CheckBox_HyperbolicFit.Margin = New System.Windows.Forms.Padding(1)
        Me.CheckBox_HyperbolicFit.Name = "CheckBox_HyperbolicFit"
        Me.CheckBox_HyperbolicFit.Size = New System.Drawing.Size(87, 17)
        Me.CheckBox_HyperbolicFit.TabIndex = 19
        Me.CheckBox_HyperbolicFit.Text = "Hyperbolic fit"
        Me.ToolTip.SetToolTip(Me.CheckBox_HyperbolicFit, "Refits all the AF data series with a hyperbolic function. Note that the logfile m" &
        "ust be re-loaded.")
        Me.CheckBox_HyperbolicFit.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoScale
        '
        Me.CheckBox_AutoScale.AutoSize = True
        Me.CheckBox_AutoScale.Location = New System.Drawing.Point(12, 63)
        Me.CheckBox_AutoScale.Margin = New System.Windows.Forms.Padding(1)
        Me.CheckBox_AutoScale.Name = "CheckBox_AutoScale"
        Me.CheckBox_AutoScale.Size = New System.Drawing.Size(76, 17)
        Me.CheckBox_AutoScale.TabIndex = 18
        Me.CheckBox_AutoScale.Text = "Auto scale"
        Me.ToolTip.SetToolTip(Me.CheckBox_AutoScale, "Auto scales the AF graphs when toggling through the AF runs.")
        Me.CheckBox_AutoScale.UseVisualStyleBackColor = True
        '
        'CheckBox_ReplayMode
        '
        Me.CheckBox_ReplayMode.AutoSize = True
        Me.CheckBox_ReplayMode.Location = New System.Drawing.Point(11, 91)
        Me.CheckBox_ReplayMode.Margin = New System.Windows.Forms.Padding(1)
        Me.CheckBox_ReplayMode.Name = "CheckBox_ReplayMode"
        Me.CheckBox_ReplayMode.Size = New System.Drawing.Size(88, 17)
        Me.CheckBox_ReplayMode.TabIndex = 17
        Me.CheckBox_ReplayMode.Text = "Replay mode"
        Me.ToolTip.SetToolTip(Me.CheckBox_ReplayMode, "Shows the progression of each AF run, with the quadratic curve fits for each new " &
        "datapoint of the run. Note that the logfile must be re-loaded.")
        Me.CheckBox_ReplayMode.UseVisualStyleBackColor = True
        '
        'Label_SaveFile
        '
        Me.Label_SaveFile.AutoSize = True
        Me.Label_SaveFile.Location = New System.Drawing.Point(8, 145)
        Me.Label_SaveFile.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label_SaveFile.Name = "Label_SaveFile"
        Me.Label_SaveFile.Size = New System.Drawing.Size(63, 13)
        Me.Label_SaveFile.TabIndex = 16
        Me.Label_SaveFile.Text = "Save to file:"
        '
        'TextBox_SaveFilePath
        '
        Me.TextBox_SaveFilePath.Location = New System.Drawing.Point(74, 142)
        Me.TextBox_SaveFilePath.Margin = New System.Windows.Forms.Padding(1)
        Me.TextBox_SaveFilePath.Name = "TextBox_SaveFilePath"
        Me.TextBox_SaveFilePath.Size = New System.Drawing.Size(296, 20)
        Me.TextBox_SaveFilePath.TabIndex = 15
        '
        'CheckBox_AutoSaveMode
        '
        Me.CheckBox_AutoSaveMode.AutoSize = True
        Me.CheckBox_AutoSaveMode.Location = New System.Drawing.Point(11, 120)
        Me.CheckBox_AutoSaveMode.Margin = New System.Windows.Forms.Padding(1)
        Me.CheckBox_AutoSaveMode.Name = "CheckBox_AutoSaveMode"
        Me.CheckBox_AutoSaveMode.Size = New System.Drawing.Size(128, 17)
        Me.CheckBox_AutoSaveMode.TabIndex = 14
        Me.CheckBox_AutoSaveMode.Text = "Automatic save mode"
        Me.ToolTip.SetToolTip(Me.CheckBox_AutoSaveMode, "Saves the AF data to the file specified below.")
        Me.CheckBox_AutoSaveMode.UseVisualStyleBackColor = True
        '
        'DropDownList_Colors
        '
        Me.DropDownList_Colors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DropDownList_Colors.FormattingEnabled = True
        Me.DropDownList_Colors.Items.AddRange(New Object() {"Windows Default", "Gray-White", "Gray-Gold", "Blue-White"})
        Me.DropDownList_Colors.Location = New System.Drawing.Point(92, 8)
        Me.DropDownList_Colors.Margin = New System.Windows.Forms.Padding(2)
        Me.DropDownList_Colors.Name = "DropDownList_Colors"
        Me.DropDownList_Colors.Size = New System.Drawing.Size(115, 21)
        Me.DropDownList_Colors.TabIndex = 20
        Me.ToolTip.SetToolTip(Me.DropDownList_Colors, "Select the color scheme of the application. Requires a restart of the AF Logviewe" &
        "r.")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Color scheme"
        '
        'ToolTip
        '
        Me.ToolTip.AutoPopDelay = 20000
        Me.ToolTip.InitialDelay = 500
        Me.ToolTip.ReshowDelay = 100
        '
        'OptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(394, 181)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DropDownList_Colors)
        Me.Controls.Add(Me.CheckBox_HyperbolicFit)
        Me.Controls.Add(Me.CheckBox_AutoScale)
        Me.Controls.Add(Me.CheckBox_ReplayMode)
        Me.Controls.Add(Me.Label_SaveFile)
        Me.Controls.Add(Me.TextBox_SaveFilePath)
        Me.Controls.Add(Me.CheckBox_AutoSaveMode)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "OptionsForm"
        Me.Text = "Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CheckBox_HyperbolicFit As CheckBox
    Friend WithEvents CheckBox_AutoScale As CheckBox
    Friend WithEvents CheckBox_ReplayMode As CheckBox
    Friend WithEvents Label_SaveFile As Label
    Friend WithEvents TextBox_SaveFilePath As TextBox
    Friend WithEvents CheckBox_AutoSaveMode As CheckBox
    Friend WithEvents DropDownList_Colors As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip As ToolTip
End Class
