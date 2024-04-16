Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing

Public Class PositionTempGraph
    Private Sub Chart_PosTemp_Click(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Chart_PosTemp.Click

        Try
            Dim found_point As Tuple(Of String, Integer)
            Dim result As HitTestResult
            Dim ns As Integer

            result = Chart_PosTemp.HitTest(e.X, e.Y)
            If result.ChartElementType = ChartElementType.DataPoint Then
                found_point = New Tuple(Of String, Integer)(result.Series.Name, result.PointIndex)

                ns = Chart_PosTemp.Series.IndexOf(found_point.Item1)

                'if the point is not already in the list, add it
                If Not MainForm.Filter_ExPoints.Contains(found_point) Then
                    MainForm.Filter_ExPoints.Add(found_point)        'add point
                    Chart_PosTemp.Series(ns).Points(found_point.Item2).MarkerStyle = MarkerStyle.Cross
                Else 'otherwise remove it
                    MainForm.Filter_ExPoints.Remove(found_point)    'remove point
                    Chart_PosTemp.Series(ns).Points(found_point.Item2).MarkerStyle = MarkerStyle.Square
                End If

                MainForm.populate_PosTempChart()
                FocusRunGraph.UpdateCheckExIncCheckBox()
                MainForm.ShowScanResults()

                Me.BringToFront()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DropDownList_Filters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_Filters.SelectedIndexChanged
        Dim fit_coeff() As Double

        'hide all lines
        For ind As Integer = 0 To MainForm.FilterList.Count - 1
            Me.Chart_PosTemp.Series(ind * 2 + 1).Enabled = False
        Next

        'show selected line
        Me.Chart_PosTemp.Series(Me.DropDownList_Filters.SelectedIndex * 2 + 1).Enabled = True

        'show fitted coeffs for first line
        fit_coeff = MainForm.FittedTempCoeffs(Me.DropDownList_Filters.SelectedIndex)
        Dim slope_sign, SGP_sign As String
        Dim slope As Double
        If fit_coeff(1) < 0 Then
            slope_sign = "-"
            SGP_sign = "+"
            slope = fit_coeff(1) * (-1)
        Else
            slope_sign = "+"
            SGP_sign = "-"
            slope = fit_coeff(1)
        End If
        'display label with fit result
        Me.Label_Fit.Text = "Linear regression = " + fit_coeff(0).ToString("0") + slope_sign + slope.ToString("0.0") + " * T" + "  (R² = " + fit_coeff(2).ToString("0.000") + ")"
        Me.Label_Fit.Text = Me.Label_Fit.Text + vbNewLine + "SGP Temp Compensation Coeff = " + SGP_sign + slope.ToString("0.0") + " steps/deg"
    End Sub

    Private Sub Button_SaveToBMP_Click(sender As Object, e As EventArgs) Handles Button_SaveToBMP.Click
        Dim control As Control
        Dim bmp_graphics As Graphics
        Dim SaveFileName As String
        Dim FileID As String

        control = Chart_PosTemp
        Dim bmp As New Bitmap(control.Width + 20, control.Height + 20)
        bmp_graphics = Graphics.FromImage(bmp)
        bmp_graphics.Clear(Color.White)

        control.DrawToBitmap(bmp, control.Bounds)

        control = Label1
        control.DrawToBitmap(bmp, control.Bounds)

        control = DropDownList_Filters
        control.DrawToBitmap(bmp, control.Bounds)

        control = Label_Fit
        control.DrawToBitmap(bmp, control.Bounds)

        FileID = MainForm.AutoFocusRunList(0).FileID
        SaveFileName = MainForm.LocalDir + "\" + FileID + "_PosTempChart_Filter_" + MainForm.FilterList(Me.DropDownList_Filters.SelectedIndex) + ".jpg"

        Try
            bmp.Save(SaveFileName, System.Drawing.Imaging.ImageFormat.Jpeg)

            MessageBox.Show("Saved chart to file: " + vbNewLine + SaveFileName)
        Catch ex As Exception
            MessageBox.Show("ERROR saving chart to file: " + vbNewLine + SaveFileName)
        End Try

    End Sub

    Private Sub Button_ClearExPoints_Click(sender As Object, e As EventArgs) Handles Button_ClearExPoints.Click
        MainForm.Filter_ExPoints.Clear()

        MainForm.populate_PosTempChart()
        FocusRunGraph.UpdateCheckExIncCheckBox()
        MainForm.ShowScanResults()

        Me.BringToFront()
    End Sub
End Class