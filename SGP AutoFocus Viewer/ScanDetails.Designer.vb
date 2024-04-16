<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScanDetails
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScanDetails))
        Me.TextBox_ScanDetails = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TextBox_ScanDetails
        '
        Me.TextBox_ScanDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_ScanDetails.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_ScanDetails.Margin = New System.Windows.Forms.Padding(1, 1, 1, 1)
        Me.TextBox_ScanDetails.Multiline = True
        Me.TextBox_ScanDetails.Name = "TextBox_ScanDetails"
        Me.TextBox_ScanDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_ScanDetails.Size = New System.Drawing.Size(858, 442)
        Me.TextBox_ScanDetails.TabIndex = 0
        Me.TextBox_ScanDetails.TabStop = False
        '
        'ScanDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(858, 442)
        Me.Controls.Add(Me.TextBox_ScanDetails)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(1, 1, 1, 1)
        Me.Name = "ScanDetails"
        Me.Text = "AF Scan Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox_ScanDetails As TextBox
End Class
