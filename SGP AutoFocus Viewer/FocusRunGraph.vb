Public Class FocusRunGraph

    Dim CheckBox_ExInclPoint_MouseClick As Boolean = True

    Private Sub DropDownList_AFruns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_AFruns.SelectedIndexChanged
        Dim ns As Integer
        Dim NumSeries As Integer
        Dim NumAFruns As Integer
        Dim IndSelected As Integer
        Dim QfitCurveInd As Integer
        Dim FilterRunList As New List(Of Integer)

        NumSeries = Me.Chart_AFRun.Series.Count
        NumAFruns = MainForm.AutoFocusRunList.Count
        IndSelected = Me.DropDownList_AFruns.SelectedIndex

        If IndSelected = 0 Then
            'if ALL AF RUNS is selected, then turn ON all curves, including all Qfit curves
            For ns = 0 To NumSeries - 1
                Me.Chart_AFRun.Series(ns).Enabled = True
            Next

            'hide checkbox
            CheckBox_ExInclPoint.Visible = False
        Else
            'Turn OFF all runs, including all Qfit curves
            For ns = 0 To NumSeries - 1
                Me.Chart_AFRun.Series(ns).Enabled = False
            Next

            'Turn ON the IndSelected curve data
            Me.Chart_AFRun.Series(IndSelected - 1).Enabled = True

            'Check if Run has Qfit curve, if yes Turn ON corresponding Qfit curve
            QfitCurveInd = MainForm.QfitAFList.IndexOf(IndSelected - 1)
            If QfitCurveInd > -1 Then
                Me.Chart_AFRun.Series(NumAFruns + QfitCurveInd).Enabled = True
            End If

            'show included/excluded checkbox
            Dim point As Tuple(Of String, Integer)
            point = New Tuple(Of String, Integer)(MainForm.AutoFocusRunList(IndSelected - 1).Filter, MainForm.AFrun_FilterSeries_List(IndSelected - 1).Item2)
            If MainForm.Filter_ExPoints.Contains(point) Then
                CheckBox_ExInclPoint_MouseClick = False
                CheckBox_ExInclPoint.Checked = False
                CheckBox_ExInclPoint_MouseClick = True
            Else
                CheckBox_ExInclPoint_MouseClick = False
                CheckBox_ExInclPoint.Checked = True
                CheckBox_ExInclPoint_MouseClick = True
            End If

            'show checkbox
            CheckBox_ExInclPoint.Visible = True
        End If

        If OptionsForm.CheckBox_AutoScale.Checked = True Then 'apply if advanced option AutoScale is checked
            Me.Chart_AFRun.ChartAreas(0).RecalculateAxesScale()
        End If
    End Sub

    Public Sub UpdateCheckExIncCheckBox()
        Dim point As Tuple(Of String, Integer)
        Dim IndSelected As Integer

        IndSelected = Me.DropDownList_AFruns.SelectedIndex
        If IndSelected > 0 Then
            point = New Tuple(Of String, Integer)(MainForm.AutoFocusRunList(IndSelected - 1).Filter, MainForm.AFrun_FilterSeries_List(IndSelected - 1).Item2)

            If MainForm.Filter_ExPoints.Contains(point) Then
                CheckBox_ExInclPoint_MouseClick = False
                CheckBox_ExInclPoint.Checked = False
                CheckBox_ExInclPoint_MouseClick = True
            Else
                CheckBox_ExInclPoint_MouseClick = False
                CheckBox_ExInclPoint.Checked = True
                CheckBox_ExInclPoint_MouseClick = True
            End If

        End If
    End Sub

    Public Sub SetXYScales()
        'CHANGE 1.26: Removed a lot of code for the scaling that looked redundant.

        With Me.Chart_AFRun.ChartAreas(0)
            'added in 1.26 to have fixed tick labels and grid lines intervals for Y axis
            .AxisY.MajorGrid.Interval = 0.5
            .AxisY.MajorTickMark.Enabled = True
            .AxisY.MajorTickMark.Interval = 0.5
            .AxisY.LabelStyle.Enabled = True
            .AxisY.LabelStyle.Interval = 0.5
        End With

    End Sub

    Private Sub Button_SaveToBMP_Click(sender As Object, e As EventArgs) Handles Button_SaveToBMP.Click
        Dim control As Control
        Dim bmp_graphics As Graphics
        Dim SaveFileName As String
        Dim FileID As String

        control = Chart_AFRun
        Dim bmp As New Bitmap(control.Width + 20, control.Height + 20)
        bmp_graphics = Graphics.FromImage(bmp)
        bmp_graphics.Clear(Color.White)

        control.DrawToBitmap(bmp, control.Bounds)

        control = DropDownList_AFruns
        control.DrawToBitmap(bmp, control.Bounds)

        FileID = MainForm.AutoFocusRunList(0).FileID
        If Me.DropDownList_AFruns.SelectedIndex = 0 Then
            SaveFileName = MainForm.LocalDir + "\" + FileID + "_AFRun_ALL.jpg"
        Else
            SaveFileName = MainForm.LocalDir + "\" + FileID + "_AFRun_" + Me.DropDownList_AFruns.SelectedIndex.ToString + ".jpg"
        End If


        Try
            bmp.Save(SaveFileName, System.Drawing.Imaging.ImageFormat.Jpeg)

            MessageBox.Show("Saved chart to file: " + vbNewLine + SaveFileName)
        Catch ex As Exception
            MessageBox.Show("ERROR saving chart to file: " + vbNewLine + SaveFileName)
        End Try
    End Sub

    Private Sub CheckBox_ExInclPoint_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_ExInclPoint.CheckedChanged
        'ONLY ACT IF THERE WAS A MOUSE CLICK ON THE CHECKBOX
        If CheckBox_ExInclPoint_MouseClick Then
            Dim point As Tuple(Of String, Integer)
            Dim IndSelected As Integer

            IndSelected = Me.DropDownList_AFruns.SelectedIndex
            point = New Tuple(Of String, Integer)(MainForm.AutoFocusRunList(IndSelected - 1).Filter, MainForm.AFrun_FilterSeries_List(IndSelected - 1).Item2)

            If CheckBox_ExInclPoint.Checked Then
                'remove from excl points list
                MainForm.Filter_ExPoints.Remove(point)
            Else
                'add to excl points list
                MainForm.Filter_ExPoints.Add(point)
            End If

            MainForm.populate_PosTempChart()
            MainForm.ShowScanResults()

            Me.ActiveControl = Me.DropDownList_AFruns
        End If
    End Sub
End Class