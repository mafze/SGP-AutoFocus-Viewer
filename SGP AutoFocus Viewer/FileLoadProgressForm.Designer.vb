<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileLoadProgressForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileLoadProgressForm))
        Me.ProgressBar_FileLoad = New System.Windows.Forms.ProgressBar()
        Me.Label_Filename = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ProgressBar_FileLoad
        '
        Me.ProgressBar_FileLoad.Location = New System.Drawing.Point(176, 110)
        Me.ProgressBar_FileLoad.Name = "ProgressBar_FileLoad"
        Me.ProgressBar_FileLoad.Size = New System.Drawing.Size(495, 46)
        Me.ProgressBar_FileLoad.TabIndex = 0
        '
        'Label_Filename
        '
        Me.Label_Filename.AutoSize = True
        Me.Label_Filename.Location = New System.Drawing.Point(28, 34)
        Me.Label_Filename.Name = "Label_Filename"
        Me.Label_Filename.Size = New System.Drawing.Size(102, 32)
        Me.Label_Filename.TabIndex = 1
        Me.Label_Filename.Text = "Label1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 114)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 32)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Progress:"
        '
        'FileLoadProgressForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(16.0!, 31.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 208)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label_Filename)
        Me.Controls.Add(Me.ProgressBar_FileLoad)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FileLoadProgressForm"
        Me.ShowInTaskbar = False
        Me.Text = "Loading files..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ProgressBar_FileLoad As ProgressBar
    Friend WithEvents Label_Filename As Label
    Friend WithEvents Label1 As Label
End Class
