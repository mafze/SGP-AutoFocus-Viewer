Imports System.IO
Imports System.Globalization
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports System.Random
Imports MathNet.Numerics
Imports System.ComponentModel

Public Class MainForm

    Dim TAB_Space As String = "     "
    Dim SGPLogFilenames() As String
    Dim ScanDetailsText As String
    Public LocalDir As String
    Public AutoFocusRunList As New List(Of AutoFocusRun)
    Public FilterList As New List(Of String)
    Public Filter_ExPoints As New List(Of Tuple(Of String, Integer))
    Public FilterColorList As List(Of Color) = New List(Of Color)()
    Public AFrun_FilterSeries_List As New List(Of Tuple(Of Integer, Integer))
    Public FittedTempCoeffs As List(Of Double()) = New List(Of Double())()
    Public QfitAFList As List(Of Integer) = New List(Of Integer)
    Dim TemperatureList As List(Of Double) = New List(Of Double)  'used for logging temps
    Dim MiddleTemp As Double
    Dim screenHeight As Integer = My.Computer.Screen.Bounds.Height
    Dim screenWidth As Integer = My.Computer.Screen.Bounds.Width
    Dim screenFraction As Double = 1.5
    Dim getAllRuns As Boolean
    Dim REPLAY_mode As Boolean
    Dim showTempdata As Boolean = False
    Dim minQuality As Double
    Dim ASTAP As Boolean

    'Color scheme variables
    Dim CustomColorScheme As Boolean
    Dim SelectedColorScheme As Integer
    Dim FormBkgColor As Color
    Dim FormForeColor As Color
    Dim FormFont As Font
    Dim BtnBorderColor As Color
    Dim BtnBkgColor As Color
    Dim BtnForeColor As Color
    Dim BtnFont As Font
    Dim AFplotColor As Color

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim cla As String()     'command line arguments

        SelectedColorScheme = My.Settings.ColorScheme
        OptionsForm.DropDownList_Colors.SelectedIndex = SelectedColorScheme
        If SelectedColorScheme > 0 Then
            CustomColorScheme = True
        Else
            CustomColorScheme = False
        End If

        If CustomColorScheme Then
            'set custom color scheme
            SetColorScheme()
            TabControl.DrawMode = TabDrawMode.OwnerDrawFixed
            AddHandler TabControl.DrawItem, AddressOf OnDrawItem 'Custom code to allow color changes to tabs change 
            ChangeControlColors(Me)
            ChangeControlColors(FileLoadProgressForm)
            ChangeControlColors(OptionsForm)
            ChangeControlColors(PositionTempGraph)
            ChangeControlColors(ScanDetails)
        Else
            TabControl.DrawMode = TabDrawMode.Normal    'make sure tabs are drawn normally
        End If

        'set form size Mainform
        Me.Location = New Point((screenWidth - screenWidth / screenFraction) / 2, (screenHeight - screenHeight / screenFraction) / 2)
        Me.Width = screenWidth / screenFraction
        Me.Height = screenHeight / screenFraction

        'set all buttons to false before first successfully opened file
        Button_ReloadFile.Enabled = False
        Button_ScanDetails.Enabled = False
        Button_AFGraphs.Enabled = False
        Button_PosTempGraph.Enabled = False

        'populate save file path
        OptionsForm.TextBox_SaveFilePath.Text = My.Settings.SaveFilePath

        'check command line arguments (cla)
        cla = Environment.GetCommandLineArgs()
        If cla.Length > 1 Then
            'if cmd line arg contains .log, we assume the first argument is the filename
            If cla(1).Contains(".log") Then
                HandleLoadingAllFiles(New String() {cla(1)})
            End If
        End If

    End Sub

    Private Sub Button_OpenSGPFile_Click(sender As Object, e As EventArgs) Handles Button_OpenSGPFile.Click
        Dim result As DialogResult

        OpenFD.Title = "Select files to open"
        OpenFD.Filter = "SGP logfiles|*.txt;*.log"
        OpenFD.Multiselect = True
        OpenFD.InitialDirectory = My.Settings.LogfilePath

        result = OpenFD.ShowDialog()

        If result = DialogResult.OK Then
            SGPLogFilenames = OpenFD.FileNames

            LocalDir = System.IO.Path.GetDirectoryName(SGPLogFilenames(0))
            My.Settings.LogfilePath = LocalDir
            My.Settings.Save()

            HandleLoadingAllFiles(SGPLogFilenames)
        End If
    End Sub

    Private Sub HandleLoadingAllFiles(ByVal Filnames() As String)
        Dim Splitted_line() As String

        ScanDetailsText = ""                'clear scan details
        TextBox_FocusData.Text = ""         'clear AF run TAB
        TextBox_Analysis.Text = ""          'clear Temp-Pos TAB
        FocusRunGraph.Close()               'close AF graph form
        PositionTempGraph.Close()           'close Position-Temp graph form
        Filter_ExPoints.Clear()             'clear list of excluded points
        AutoFocusRunList.Clear()            'clear the old list of AF runs
        FilterList.Clear()                  'clear list of filters
        FilterColorList.Clear()             'clear list of filter line plot colors

        'set all buttons to false
        Button_ReloadFile.Enabled = False
        Button_ScanDetails.Enabled = False
        Button_AFGraphs.Enabled = False
        Button_PosTempGraph.Enabled = False

        'check if save all runs option is enabled
        If CheckBox_ShowAllRuns.Checked Then
            getAllRuns = True
        Else
            getAllRuns = False
        End If

        'check if replay button is enabled
        If OptionsForm.CheckBox_ReplayMode.Checked Then
            REPLAY_mode = True
        Else
            REPLAY_mode = False
        End If

        Dim file As String
        FileLoadProgressForm.Show()
        FileLoadProgressForm.ProgressBar_FileLoad.Minimum = 0
        FileLoadProgressForm.ProgressBar_FileLoad.Maximum = Filnames.Length
        FileLoadProgressForm.ProgressBar_FileLoad.Step = 1
        FileLoadProgressForm.ProgressBar_FileLoad.Value = 0
        For Each file In Filnames
            Splitted_line = file.Split("\")
            FileLoadProgressForm.Label_Filename.Text = "Loading file: " + Splitted_line(Splitted_line.Length - 1)
            FileLoadProgressForm.ProgressBar_FileLoad.PerformStep()

            ReadSGPLogfile(file)      'read log file

            FileLoadProgressForm.ProgressBar_FileLoad.Refresh()
            Application.DoEvents()
        Next file

        GetFilterList()                             'find list of filters used
        If AutoFocusRunList.Count > 0 Then
            'if hidden option check box Hyperbolic fit is check, refit data
            If OptionsForm.CheckBox_HyperbolicFit.Checked = True Then
                hyperbolic_fit()
            End If

            'populate graphs
            populate_AFChart()
            FocusRunGraph.Show()
            populate_PosTempChart()
        End If
        ShowScanResults()                           'show summary of results
        Refresh_TextBox()                           'refresh scan details textbox

        FileLoadProgressForm.Close()
    End Sub

    Private Sub ReadSGPLogfile(ByVal SGPLogFilename As String)
        Dim LogfileEntry As String
        Dim FileID As String
        Dim LogfileLineNumber As Integer = 0
        Dim AF_Message As String = "SGM_FOCUSER_AUTO_FOCUS"
        Dim Splitted_line() As String
        Dim LineBreak As String = "-------------------------------------------"

        Dim Num_AF_Runs As Integer = 0
        Dim Found_AF_Runs As Boolean = False
        Dim AF_auto_rerun As Boolean = False
        Dim GetValue As Double
        Dim AF_Pos As New ArrayList
        Dim AF_HFR As New ArrayList
        Dim AF_Stars As New ArrayList
        Dim BestFocusPosition As Integer
        Dim BestFocusHFR As Double
        Dim Qfit As Boolean = False
        Dim Qparams As New ArrayList
        Dim Temperature As Double
        Dim Stars As Integer
        Dim Filter As String = ""
        Dim Target As String = ""
        Dim Misc As String = ""
        Dim AFrunStartTime As String = ""
        Dim StartDate As String = ""
        Dim CURRENT_FOCUSER_POSITION As Integer
        Dim COMPLETED_AF_RUN As Boolean = False
        Dim AF_runningTarget As Boolean = False

        TemperatureList.Clear()

        minQuality = 0.9 'default value in SGP

        ASTAP = False 'by default, unless message detected, of course this assumes the whole file uses only ASTAP

        Try
            Dim fs As FileStream = New FileStream(SGPLogFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim objReader As StreamReader = New StreamReader(fs)

            'Get file ID from SGP file name
            Splitted_line = SGPLogFilename.Split("_")
            Splitted_line = Splitted_line(Splitted_line.Length - 1).Split(".")
            FileID = Splitted_line(0)

            'reset target tracker
            AF_runningTarget = False
            'reset tracker of current focuser position
            CURRENT_FOCUSER_POSITION = 0

            AddToTextboxFocusData("Starting reading file: " + SGPLogFilename + "...", "Last")
            Application.DoEvents()  'Force the message above to appear in textbox before reading of file freezes it
            AddToTextboxFocusData(LineBreak, "Last")

            'READ EACH LINE, SEARCH EACH LINE FOR RELEVANT AF MESSAGES, SHOW FOUND MESSAGES IN TEXTBOX, GET AF DATA AND LOOP
            Do Until objReader.EndOfStream
                LogfileEntry = objReader.ReadLine()     'read next line in log file
                LogfileLineNumber = LogfileLineNumber + 1

                'AddToTextboxFocusData(TAB_Space + LogfileEntry,"Last")   '''USE THIS TO SEE ALL MESSAGES

                'Check for SGP version
                FindEntry(LogfileEntry, "S E Q U E N C E   G E N E R A T O R", GetValue)

                'Check if the AF_Message is part of target
                If LogfileEntry.Contains("Waiting for AF to complete...") Then
                    AF_runningTarget = True
                End If
                'Reset AF_InSequence in case AF has finished as part of target
                If LogfileEntry.Contains("AF is complete, continuing sequence...") Then
                    AF_runningTarget = False
                End If

                'Check EventMarker to track current target
                If LogfileEntry.Contains("EventMarker") Then
                    Splitted_line = LogfileEntry.Split(":")
                    If Splitted_line.Count > 3 Then
                        Splitted_line = Splitted_line(3).Split("(")
                        Target = Splitted_line(0)
                    Else
                        Target = ""
                    End If

                End If

                'Look for temperature data
                If LogfileEntry.Contains("Set start frame data") Then
                    Splitted_line = LogfileEntry.Split(New String() {"Temp:"}, StringSplitOptions.None)
                    Splitted_line = Splitted_line(1).Split(New String() {"..."}, StringSplitOptions.None)
                    TemperatureList.Add(Double.Parse(Splitted_line(0).Replace(",", "."), CultureInfo.InvariantCulture))
                End If

                'AF_Message contains the start message of a AF run
                'Loop over AF points, store relevant data
                'Only accept and store COMPLETE runs
                If LogfileEntry.Contains(AF_Message) Then   'check for potential start of AF run
                    Found_AF_Runs = False               'reset found at least ONE AF Run checker
                    COMPLETED_AF_RUN = False            'reset AF complete status
                    AF_auto_rerun = False               'reset auto rerun status
                    Qfit = False                        'reset Qfit param
                    Misc = ""                           'empty Misc message

                    AddToTextboxFocusData(vbNewLine + "AUTO FOCUS RUN = " + (Num_AF_Runs + 1).ToString, "Last")
                    FindEntry(LogfileEntry, AF_Message, GetValue)       'call this just to have the message printed

                    Do
                        LogfileEntry = objReader.ReadLine()
                        LogfileLineNumber = LogfileLineNumber + 1
                        'AddToTextboxFocusData(TAB_Space + LogfileEntry,"Last")   '''USE THIS TO SEE ALL FOCUS MESSAGES

                        'Check if ASTAP is used
                        If FindEntry(LogfileEntry, "Performing ASTAP auto focus using HFD - ASTAP.", GetValue) Then
                            ASTAP = True
                        End If

                        'Check trigger for AF
                        FindEntry(LogfileEntry, "Checking for auto focus...", GetValue)
                        FindEntry(LogfileEntry, "Auto focus required", GetValue)

                        'Get AF filter
                        If FindEntry(LogfileEntry, "Auto focus: setting filter", GetValue) Then
                            Splitted_line = LogfileEntry.Split(" ")
                            Filter = Splitted_line(Splitted_line.Length - 1)
                        End If

                        'Get AF focus run params
                        If LogfileEntry.Contains("Auto focus data") Then
                            If AF_auto_rerun Then
                                AddToTextboxFocusData(vbNewLine + "AUTO FOCUS RUN = " + (Num_AF_Runs + 1).ToString, "Last")
                            End If
                            FindEntry(LogfileEntry, "Auto focus data", GetValue)

                            'Get time of RUN
                            GetTimeDate(LogfileEntry, StartDate, AFrunStartTime)
                        End If
                        FindEntry(LogfileEntry, "Data Points:", GetValue)
                        FindEntry(LogfileEntry, "Step Size:", GetValue)
                        FindEntry(LogfileEntry, "Current Position:", GetValue)

                        'Auto focus frame taking
                        FindEntry(LogfileEntry, "Taking auto focus frame(s)...", GetValue)
                        FindEntry(LogfileEntry, "AF Binning", GetValue)
                        FindEntry(LogfileEntry, "AF Exposure length", GetValue)

                        'Get number of stars in list
                        If ASTAP = False Then 'avoid showing this line if ASTAP is used
                            If FindEntry(LogfileEntry, "Star list contains", GetValue) Then
                                Splitted_line = LogfileEntry.Split("]")
                                Splitted_line = Splitted_line.Last.Split(" ")
                                Stars = Integer.Parse(Splitted_line(4))
                            End If
                        End If

                        'detect change in quadratic_fit_min_quality
                        If FindEntry(LogfileEntry, "AutoFocus: quadratic fit min quality set to -> ", GetValue) Then
                            minQuality = GetValue
                        End If

                        'In case smart focus is enabled
                        FindEntry(LogfileEntry, "Detected focus getting worse!", GetValue)
                        'catastrophic smart focus message
                        If FindEntry(LogfileEntry, "Killing smart focus...", GetValue) Then
                            If getAllRuns Then
                                Misc = "Smart focus abort"
                                COMPLETED_AF_RUN = True
                            End If
                        End If

                        'Track focuser movements, save position in CURRENT_FOCUSER_POSITION
                        FindEntry(LogfileEntry, "Focuser moving to", GetValue)
                        If GetValue > 0 Then
                            CURRENT_FOCUSER_POSITION = Integer.Parse(GetValue)
                        End If

                        'Check if AF focus point has been calculated, store assuming current focuser postion CURRENT_FOCUSER_POSITION
                        If FindEntry(LogfileEntry, "Auto focus HFR calculated at: ", GetValue) Then     'SGP NATIVE
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus Half Flux Radius calculated at: ", GetValue) Then     'SGP NATIVE, CHANGE FROM SGP 4.2 beta
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus HFD calculated at: ", GetValue) Then     'ASTAP  !!OLD!!
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            'get number of stars
                            Splitted_line = LogfileEntry.Split(" ")
                            Stars = Integer.Parse(Splitted_line(Splitted_line.Length - 2))
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus validation frame HFD calculated at: ", GetValue) Then     'ASTAP !!OLD!!
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            Stars = -1  'no stars logged here, put to -1
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus Half Flux Diameter calculated at: ", GetValue) Then     'ASTAP
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            'get number of stars
                            Splitted_line = LogfileEntry.Split(" ")
                            Stars = Integer.Parse(Splitted_line(Splitted_line.Length - 2))
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus validation frame Half Flux Diameter calculated at: ", GetValue) Then     'ASTAP
                            AF_HFR.Add(GetValue)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            Stars = AF_Stars.ToArray.Min()
                            AF_Stars.Add(Stars)
                        ElseIf FindEntry(LogfileEntry, "Auto focus FWHM calculated at: ", GetValue) Then    'PINTPOINT
                            'For some reason there appears to be a factor of 100 in the reported value!
                            AF_HFR.Add(GetValue / 100)
                            AF_Pos.Add(CURRENT_FOCUSER_POSITION)
                            AF_Stars.Add(Stars)
                        End If

                        'Quadratic fit messages:
                        FindEntry(LogfileEntry, "Quadratic Focus Successful with Fit Quality", GetValue)
                        FindEntry(LogfileEntry, "Quadratic Focus Complete but Asymmetric with Fit Quality", GetValue)
                        FindEntry(LogfileEntry, "Quadratic coefficients:", GetValue)
                        'Detect reruns
                        If FindEntry(LogfileEntry, "Focus is asymmetric.", GetValue) Or FindEntry(LogfileEntry, "Focus cannot be determined.", GetValue) Then
                            If Not getAllRuns Then
                                'Clear all previous points
                                AF_Pos = New ArrayList()
                                AF_HFR = New ArrayList()
                                AF_Stars = New ArrayList()
                                Qparams = New ArrayList()
                            Else
                                Misc = "Failed. Auto Rerun."
                                AF_auto_rerun = True
                                COMPLETED_AF_RUN = True
                            End If
                        End If

                        'Detect when aborting due to 3 failed runs
                        If FindEntry(LogfileEntry, "Smart focus failed", GetValue) Then
                            If getAllRuns Then
                                If AutoFocusRunList.Count > 0 Then
                                    'Update MISC message of last added run
                                    AutoFocusRunList.Last.Misc = "Failed. Aborting.  "
                                Else
                                    COMPLETED_AF_RUN = True
                                    Misc = "Failed. Aborting.  "
                                End If
                            End If
                        End If

                        'Get quadratic fit params
                        'message type:  Incremental Fit Result (9): quality=98%; a=5.93598316445617; b=-134.755534876412; c=765.561277581482;
                        If FindEntry(LogfileEntry, "Incremental Fit Result", GetValue) Then
                            Qparams.Clear() 'clear out old points, only keep latest
                            Qfit = True
                            Splitted_line = LogfileEntry.Split(New String() {"a="}, StringSplitOptions.None)(1).Split(";")
                            Qparams.Add(Double.Parse(Splitted_line(0).Replace(",", "."), CultureInfo.InvariantCulture))
                            Splitted_line = LogfileEntry.Split(New String() {"b="}, StringSplitOptions.None)(1).Split(";")
                            Qparams.Add(Double.Parse(Splitted_line(0).Replace(",", "."), CultureInfo.InvariantCulture))
                            Splitted_line = LogfileEntry.Split(New String() {"c="}, StringSplitOptions.None)(1).Split(";")
                            Qparams.Add(Double.Parse(Splitted_line(0).Replace(",", "."), CultureInfo.InvariantCulture))
                            Splitted_line = LogfileEntry.Split(New String() {"quality="}, StringSplitOptions.None)(1).Split("%")
                            If Not Splitted_line(0) = "" And Not Splitted_line(0) = "-∞" Then
                                Qparams.Add(Double.Parse(Splitted_line(0).Replace(",", "."), CultureInfo.InvariantCulture))
                            Else
                                Qparams.Add(0)
                            End If

                            If REPLAY_mode Then
                                AutoFocusRunList.Add(New AutoFocusRun(FileID, BestFocusPosition, BestFocusHFR, Qfit, minQuality, Qparams, Temperature, AF_Pos, AF_HFR, AF_Stars, Filter, AFrunStartTime, StartDate, "", "AF data point"))
                            End If
                        End If

                        'New focus position message
                        '!! This also contains temperature for the run !!
                        If FindEntry(LogfileEntry, "New focus position is at", GetValue) Then
                            'Get temperature, example: "New focus position is at 316110 (@24.23C). "
                            Splitted_line = LogfileEntry.Split("@")
                            If Splitted_line.Length > 1 Then
                                LogfileEntry = Splitted_line(1)
                                Splitted_line = LogfileEntry.Split("C")
                                LogfileEntry = Splitted_line(0)
                                Temperature = Double.Parse(LogfileEntry.Replace(",", "."), CultureInfo.InvariantCulture)
                            Else 'In the rare case when the temp data is missing, eg. the line looks like "New focus position is at 316110."
                                If TemperatureList.Count > 0 Then
                                    'Use start frame temp data if available
                                    Temperature = TemperatureList.Last
                                Else
                                    'CANT FIND ANY TEMP DATA, SET TO -100
                                    Temperature = -100
                                    Misc = "No temp data!"
                                End If
                            End If
                        End If

                        'Check if validation frame is taken
                        FindEntry(LogfileEntry, "Taking validation frame at focus position...", GetValue)

                        'Check validation frame error
                        If FindEntry(LogfileEntry, "Warning! Auto focus validation frame HFR", GetValue) Then
                            If Misc = "" Then
                                Misc = "HFR warning"
                            End If
                        End If

                        'Check If auto focus rerun has been invoked manually since Beta v3.1.0.295 
                        If FindEntry(LogfileEntry, "Rerun auto focus (attempt", GetValue) Then
                            Misc = "Rerun"
                            'Get time or rerun
                            GetTimeDate(LogfileEntry, StartDate, AFrunStartTime)
                        End If
                        'Check If auto focus rerun has been invoked manually
                        If FindEntry(LogfileEntry, "Rerun auto focus...", GetValue) Then
                            Misc = "Rerun"
                            'Get time or rerun
                            GetTimeDate(LogfileEntry, StartDate, AFrunStartTime)
                        End If

                        'Check if AF as been completed succesfully
                        If FindEntry(LogfileEntry, "Auto focus is complete (", GetValue) Then
                            Dim temp_LogfileEntry As String
                            temp_LogfileEntry = LogfileEntry

                            COMPLETED_AF_RUN = True

                            'Remove [] entries
                            Splitted_line = LogfileEntry.Split("]")
                            temp_LogfileEntry = Splitted_line(UBound(Splitted_line))

                            'Get best focus position
                            'since Beta v3.1.0.295 the code must handle both 
                            '"Auto focus is complete (best fit method: 64396; HFR 1.92)..."
                            'and
                            '"Auto focus is complete (64396; HFR 1.92)..."
                            Splitted_line = temp_LogfileEntry.Split(";")
                            Splitted_line = Splitted_line(0).Split("(")
                            If Not Integer.TryParse(Splitted_line(Splitted_line.Length - 1), BestFocusPosition) Then
                                'if the format is the old one
                                Splitted_line = Splitted_line(1).Split(":")
                                Integer.TryParse(Splitted_line(Splitted_line.Length - 1), BestFocusPosition)
                            End If

                            'Get focus HFR
                            Splitted_line = temp_LogfileEntry.Split(";")
                            Splitted_line = Splitted_line(1).Split(")")
                            Splitted_line = Splitted_line(0).Split(" ")
                            BestFocusHFR = Double.Parse(Splitted_line(Splitted_line.Length - 1).Replace(",", "."), CultureInfo.InvariantCulture)

                            'add to the list of data
                            AF_HFR.Add(BestFocusHFR)
                            AF_Pos.Add(BestFocusPosition)
                            AF_Stars.Add(Stars)
                        End If

                        'check for Abort message
                        If FindEntry(LogfileEntry, "AF abort requested", GetValue) Then
                            If Not getAllRuns Then
                                'clear focus points
                                AF_Pos = New ArrayList()
                                AF_HFR = New ArrayList()
                                AF_Stars = New ArrayList()
                                Qparams = New ArrayList()
                            Else
                                Misc = "User aborted."
                                Qfit = False
                                COMPLETED_AF_RUN = True
                            End If
                        End If

                        If COMPLETED_AF_RUN Then
                            If Misc = "" Then
                                Misc = "Successful"
                            End If
                            'Add AF run to list of runs
                            'Only accept runs with the same number of points in each Array
                            'Also reject AF runs without any points
                            If AF_Pos.Count = AF_HFR.Count Then
                                If AF_Pos.Count > 0 Then
                                    Found_AF_Runs = True 'found at least one run
                                    Num_AF_Runs = Num_AF_Runs + 1

                                    If AF_runningTarget Then
                                        AutoFocusRunList.Add(New AutoFocusRun(FileID, BestFocusPosition, BestFocusHFR, Qfit, minQuality, Qparams, Temperature, AF_Pos, AF_HFR, AF_Stars, Filter, AFrunStartTime, StartDate, Target, Misc))
                                    Else
                                        AutoFocusRunList.Add(New AutoFocusRun(FileID, BestFocusPosition, BestFocusHFR, Qfit, minQuality, Qparams, Temperature, AF_Pos, AF_HFR, AF_Stars, Filter, AFrunStartTime, StartDate, "", Misc))
                                    End If
                                End If
                            End If

                            'clear focus points
                            AF_Pos = New ArrayList()
                            AF_HFR = New ArrayList()
                            AF_Stars = New ArrayList()
                            Qparams = New ArrayList()
                            'reset Misc message
                            Misc = ""

                            COMPLETED_AF_RUN = False
                            Qfit = False
                        End If

                        'Look for end of AF run
                        If LogfileEntry.Contains(AF_Message) Then
                            FindEntry(LogfileEntry, AF_Message, GetValue)
                            Exit Do
                        End If

                    Loop Until objReader.EndOfStream 'Continue looking for focus points in this AF run

                    If Not Found_AF_Runs Then
                        'TextBox_FocusData.Text = TextBox_FocusData.Text.Remove(TextBox_FocusData.Text.LastIndexOf("AUTO FOCUS EVENT"))
                        ScanDetailsText = ScanDetailsText.Remove(ScanDetailsText.LastIndexOf("AUTO FOCUS RUN"))
                    End If

                End If

            Loop 'Continue looking for AF runs until end of file

            'End of file message
            AddToTextboxFocusData(LineBreak, "Last")
            AddToTextboxFocusData("Completed reading file!", "Last")
            AddToTextboxFocusData(" ", "Last")
            AddToTextboxFocusData(" ", "Last")

            objReader.Dispose()
        Catch ex As Exception
            MessageBox.Show("Error while reading line " + LogfileLineNumber.ToString + vbNewLine + "Message: " + ex.Message + vbNewLine + "Stacktrace:" + ex.StackTrace)
            Exit Sub
        End Try


    End Sub

    Public Sub ShowScanResults()
        Dim LineBreak As String = "-------------------------------------------"
        Dim FileID As String = ""
        Dim StringTitle As String
        Dim fit_coeffs() As Double
        Dim FilterOffsets() As Double
        Dim Nstars_max, Nstars_min As Integer

        'Create summary message 
        Dim SummaryText As String
        If AutoFocusRunList.Count = 0 Then
            SummaryText = "SUMMARY OF RESULTS:" + vbNewLine
            SummaryText = SummaryText + LineBreak + vbNewLine
            If Not getAllRuns Then
                SummaryText = SummaryText + "Found NO COMPLETE Auto Focus Runs in logfile." + vbNewLine
            Else
                SummaryText = SummaryText + "Found NO Auto Focus Runs in logfile." + vbNewLine
            End If


            'enable reload button
            Button_ReloadFile.Enabled = True
        Else
            SummaryText = "SUMMARY OF RESULTS:" + vbNewLine
            SummaryText = SummaryText + LineBreak + vbNewLine
            If Not getAllRuns Then
                SummaryText = SummaryText + "FOUND " + AutoFocusRunList.Count.ToString + " COMPLETE AUTO FOCUS RUNS." + vbNewLine
            Else
                SummaryText = SummaryText + "FOUND " + AutoFocusRunList.Count.ToString + " AUTO FOCUS RUNS." + vbNewLine
            End If

            If OptionsForm.CheckBox_HyperbolicFit.Checked Then
                SummaryText = SummaryText + vbNewLine + "!! Hyperbolic fit of logged data by AF Logviewer !! " + FileID
                SummaryText = SummaryText + vbNewLine + "Best focus position in the list is now estimated using the hyperbolic fit." + FileID
                SummaryText = SummaryText + vbNewLine + "The Quality Q value is recalculated from the hyperbolic fit." + FileID
                SummaryText = SummaryText + vbNewLine + "HFR value in the list is still from the validation frame measured by SGP." + FileID + vbNewLine
            End If

            'MAKE BIG TABLE
            SummaryText = SummaryText + vbNewLine
            Dim CTitles() As String
            CTitles = {"AF Run", "Date", "Time", "Pos", "HFR", "Temp", "Stars", "Filter", "Q (min Q)", "  Misc"}
            If ASTAP Then
                CTitles(4) = "HFD"
            End If

            Dim Cspaces() As Integer = {0, 8, 4, 4, 4, 4, 4, 4, 4, 10}
            If getAllRuns Then 'to make place for the longer Misc comments
                Cspaces(Cspaces.Count - 1) = 12
            End If

            Dim StringForm As String
            Dim TableLength As Integer = 0
            For k As Integer = 0 To CTitles.Length - 1
                If k = 0 Then
                    StringForm = "{0," + (CTitles(k).Length + Cspaces(k)).ToString + "}"
                    SummaryText = SummaryText + String.Format(StringForm, CTitles(k))
                Else
                    StringForm = "{0}{1," + (CTitles(k).Length + Cspaces(k)).ToString + "}"
                    SummaryText = SummaryText + String.Format(StringForm, vbTab, CTitles(k))
                End If
                TableLength = TableLength + CTitles(k).Length + Cspaces(k) + 12
            Next
            SummaryText = SummaryText + vbTab + "Target"
            SummaryText = SummaryText + vbNewLine
            SummaryText = SummaryText + StrDup(Integer.Parse(Math.Round(TableLength)), "-")
            SummaryText = SummaryText

            For ns As Integer = 0 To AutoFocusRunList.Count - 1
                'Add file ID header each time it changes
                If Not FileID.Equals(AutoFocusRunList(ns).FileID) Then
                    FileID = AutoFocusRunList(ns).FileID
                    SummaryText = SummaryText + vbNewLine + "File ID = " + FileID + vbNewLine
                End If

                'get max and min stars in the AF run
                Nstars_max = AutoFocusRunList(ns).FocusStars.ToArray.Max()
                Nstars_min = AutoFocusRunList(ns).FocusStars.ToArray.Min()

                StringForm = "{0," + (CTitles(0).Length + Cspaces(0)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, ns + 1)
                StringForm = "{0}{1," + (CTitles(1).Length + Cspaces(1)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).StartDate)
                StringForm = "{0}{1," + (CTitles(2).Length + Cspaces(2)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).StartTime)
                StringForm = "{0}{1," + (CTitles(3).Length + Cspaces(3)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).BestFocusPosition)
                StringForm = "{0}{1," + (CTitles(4).Length + Cspaces(4)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).BestFocusHFR)
                StringForm = "{0}{1," + (CTitles(5).Length + Cspaces(5)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).Temperature)
                StringForm = "{0}{1," + (CTitles(6).Length + Cspaces(6)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, Nstars_max.ToString + "(" + Nstars_min.ToString + ")")
                StringForm = "{0}{1," + (CTitles(7).Length + Cspaces(7)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).Filter)

                StringForm = "{0}{1," + (CTitles(8).Length + Cspaces(8)).ToString + "}"
                If AutoFocusRunList(ns).Qfit Then
                    SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).Qparams(3).ToString() + "% (" + (AutoFocusRunList(ns).Qmin * 100).ToString + "%)")
                Else
                    SummaryText = SummaryText + String.Format(StringForm, vbTab, "NA")
                End If

                StringForm = "{0}{1," + (CTitles(9).Length + Cspaces(9)).ToString + "}"
                SummaryText = SummaryText + String.Format(StringForm, vbTab, AutoFocusRunList(ns).Misc)
                SummaryText = SummaryText + vbTab + AutoFocusRunList(ns).Target

                SummaryText = SummaryText + vbNewLine
            Next

            SummaryText = SummaryText + vbNewLine + "Date and Time: start of the AF run" + vbNewLine
            SummaryText = SummaryText + "Pos: best focus position" + vbNewLine
            SummaryText = SummaryText + "HFR: validation frame half flux radius (HFR) taken at best focus position" + vbNewLine
            SummaryText = SummaryText + "Stars: maximum number of stars detected during the AF run (minimum number)" + vbNewLine
            SummaryText = SummaryText + "Q: curve fit quality factor" + vbNewLine
            SummaryText = SummaryText + "Target: name of target if AF run was taken during a sequence, otherwise empty" + vbNewLine
            SummaryText = SummaryText + vbNewLine + "Possible Misc messages:" + vbNewLine
            SummaryText = SummaryText + "--------------------------------------------------------" + vbNewLine
            SummaryText = SummaryText + "Succesful: Requires Q >= min Q. Default min Q = 90% (can be set using the 'quadratic_fit_min_quality' variable)" + vbNewLine
            SummaryText = SummaryText + "HFR warning: HFR validation frame warning" + vbNewLine
            SummaryText = SummaryText + "Rerun: Manually triggered rerun by user" + vbNewLine
            SummaryText = SummaryText + "User aborted: Manually aborted by user" + vbNewLine
            'SummaryText = SummaryText + "No temp data! = could no find temperature data, the temperature is set to -100 in output table" + vbNewLine
            If getAllRuns Then
                SummaryText = SummaryText + "Failed = Error 'Focus cannot be determined' during run" + vbNewLine
                SummaryText = SummaryText + "Smart focus abort = Error 'Killing smart focus... we are lost.  Restoring focuser to best guess position.'" + vbNewLine
            End If
            SummaryText = SummaryText + LineBreak + LineBreak + LineBreak + vbNewLine + vbNewLine

            'put text in box
            TextBox_FocusData.Text = SummaryText


            'Show temp compensation analysis
            SummaryText = "ANALYSIS OF TEMPERATURE COEFFICIENTS " + vbNewLine + StrDup(100, "-") + vbNewLine
            SummaryText = SummaryText + "MODEL:" + vbNewLine
            SummaryText = SummaryText + LineBreak + vbNewLine
            SummaryText = SummaryText + "HFR = a*T + b" + vbNewLine
            SummaryText = SummaryText + "   a: slope" + vbNewLine
            SummaryText = SummaryText + "   b: intercept at T = 0" + vbNewLine
            SummaryText = SummaryText + "SGP temperature compensation factor: -a" + vbNewLine
            SummaryText = SummaryText + "(Negative temperature compensation ratios in SGP indicate that your focuser will move INWARD as temperature drops)" + vbNewLine
            SummaryText = SummaryText + vbNewLine + vbNewLine + "RESULTS:" + vbNewLine
            SummaryText = SummaryText + LineBreak + vbNewLine
            Dim SGP_coeff_average As Double = 0
            For ind As Integer = 0 To FittedTempCoeffs.Count - 1
                fit_coeffs = FittedTempCoeffs(ind)
                SummaryText = SummaryText + "Filter " + vbTab + FilterList(ind) + vbTab + ":   a = " + fit_coeffs(1).ToString("0.0") + " steps/deg," + vbTab + "b = " + fit_coeffs(0).ToString("0") + " steps, "
                Dim SGP_coeff As Double = fit_coeffs(1) * (-1) 'SGP wants negative coefficient if focuser moves inward when temperature drops
                SummaryText = SummaryText + vbTab + "SGP factor: " + SGP_coeff.ToString("0.0") + " steps/deg" + vbNewLine
                SGP_coeff_average = SGP_coeff_average + SGP_coeff / FittedTempCoeffs.Count
            Next
            If FittedTempCoeffs.Count > 1 Then
                SummaryText = SummaryText + vbNewLine + "AVERAGE SGP factor: " + SGP_coeff_average.ToString("0.0") + " steps/deg" + vbNewLine
            End If

            'Show filter offset calculation if >1 filter
            If FittedTempCoeffs.Count > 1 Then
                FilterOffsets = FilterOffsetCalulation(MiddleTemp)

                SummaryText = SummaryText + vbNewLine + vbNewLine + vbNewLine + vbNewLine
                SummaryText = SummaryText + "FILTER OFFSET CALCULATION " + vbNewLine + StrDup(100, "-") + vbNewLine
                SummaryText = SummaryText + "Filter offset calculation based on the position vs. temperature linear regressions." + vbNewLine
                SummaryText = SummaryText + "Offsets are calculated at the middle temperature of " + MiddleTemp.ToString("0.0") + " deg" + vbNewLine + vbNewLine
                For ind As Integer = 0 To FilterOffsets.Count - 1
                    SummaryText = SummaryText + "Filter " + vbTab + FilterList(ind) + vbTab + ":   " + FilterOffsets(ind).ToString("0") + " steps" + vbNewLine
                Next
            End If
            TextBox_Analysis.Text = SummaryText

            'set all buttons to false
            Button_ReloadFile.Enabled = True
            Button_ScanDetails.Enabled = True
            Button_AFGraphs.Enabled = True
            Button_PosTempGraph.Enabled = True

            'if save checkbox is ticked, save to file
            If OptionsForm.CheckBox_AutoSaveMode.CheckState = CheckState.Checked Then
                save_to_file()
            End If

        End If



    End Sub

    Private Function TempCoeff(ByVal Temp As List(Of Double), ByVal Pos As List(Of Double), Filter As String) As Double()
        Dim Temp_Ex As New List(Of Double)
        Dim Pos_Ex As New List(Of Double)
        Dim result(3) As Double

        'go through list of points and exclude selected ones, put in new lists
        For k As Integer = 0 To Temp.Count - 1
            If Not Filter_ExPoints.Contains(New Tuple(Of String, Integer)(Filter, k)) Then
                Temp_Ex.Add(Temp(k))
                Pos_Ex.Add(Pos(k))
            End If
        Next

        Dim resFit As Tuple(Of Double, Double)
        resFit = Fit.Line(Temp_Ex.ToArray, Pos_Ex.ToArray)

        'intercept term
        result(0) = resFit.Item1
        'linear term
        result(1) = resFit.Item2
        'R2 value
        result(2) = R2value(Temp_Ex.ToArray, Pos_Ex.ToArray, 0, result(1), result(0))

        Return result
    End Function

    Private Function R2value(ByVal x() As Double, ByVal y() As Double, ByVal a As Double, ByVal b As Double, ByVal c As Double) As Double
        Dim R2 As Double
        Dim ymean As Double
        Dim yexp As Double
        Dim r As Double
        Dim sum1 As Double
        Dim sum2 As Double
        Dim N As Integer

        N = x.Count

        'calculate mean of y
        ymean = 0
        For k As Integer = 0 To N - 1
            ymean = ymean + y(k)
        Next
        ymean = ymean / N

        'perform R2 calculation
        sum1 = 0
        sum2 = 0
        For k As Integer = 0 To N - 1
            yexp = a * x(k) ^ 2 + b * x(k) + c  'calculate fitted y point at x(k)
            r = y(k) - yexp                     'residual at x(k)

            sum1 = sum1 + r ^ 2
            sum2 = sum2 + (y(k) - ymean) ^ 2
        Next
        R2 = 1 - sum1 / sum2

        Return R2
    End Function

    Private Sub GetTimeDate(ByVal LineSource As String, ByRef StartDate As String, ByRef AFrunStartTime As String)
        Dim Splitted_Line() As String

        Splitted_Line = LineSource.Split("[")
        Splitted_Line = Splitted_Line(1).Split("]")
        Splitted_Line = Splitted_Line(0).Split(" ")         'remove date
        StartDate = Splitted_Line(0)                        'save date
        Splitted_Line = Splitted_Line(1).Split(".")         'remove milliseconds
        AFrunStartTime = Splitted_Line(0)                   'save time
    End Sub

    Private Function FindEntry(ByVal LineSource As String, ByVal message As String, ByRef GetValue As Double) As Boolean
        Dim Splitted_Line() As String
        Dim SPos As Integer

        FindEntry = False 'return False as default

        GetValue = 0 'default return value (if TryParse is not succesful below)

        If LineSource.Contains(message) Then
            FindEntry = True 'found message, return True

            'Format line
            Splitted_Line = LineSource.Split("]")
            LineSource = Splitted_Line(0) + "]" + Splitted_Line(UBound(Splitted_Line))
            'ADD TO SCAN DETAILS
            AddToTextboxFocusData(TAB_Space + LineSource, "Last")

            'Get number after string if present: Try to Parse remaing string to Double
            SPos = LineSource.IndexOf(message)          'look for first index of message
            SPos = SPos + message.Length                'add its length
            LineSource = LineSource.Substring(SPos)     'grab string after the message
            If Not LineSource = "" Then
                If LineSource.IndexOf(" ", 1) = -1 Then
                    Double.TryParse(LineSource.Substring(0).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, GetValue)
                Else
                    Double.TryParse(LineSource.Substring(0, LineSource.IndexOf(" ", 1)).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, GetValue)
                End If
            End If
            'AddToTextboxFocusData(GetValue.ToString + vbCrLf, "Last")
        End If

    End Function

    Private Sub AddToTextboxFocusData(ByVal message As String, ByVal Position As String)
        If Position.Equals("Last") Then
            ScanDetailsText = ScanDetailsText + message + vbNewLine
        End If
        If Position.Equals("First") Then
            ScanDetailsText = message + vbNewLine + ScanDetailsText
        End If
    End Sub

    Private Sub Refresh_TextBox()
        ScanDetails.TextBox_ScanDetails.Text = ScanDetailsText
    End Sub

    Public Sub populate_AFChart()
        Dim RunLegendName As String
        Dim indexAFrun As Integer
        Dim BestFocusIdx As Integer
        Dim ns As Integer
        Dim tempFocusPosition() As Object
        Dim tempFocusHFR() As Object
        Dim old_index As Integer
        Dim Ymax As Double

        'store old selected line
        old_index = FocusRunGraph.DropDownList_AFruns.SelectedIndex
        If old_index = -1 Then 'in case the dropdowlist contains no element (before redrawing)
            old_index = 0 'set to first line
        End If

        FocusRunGraph.Chart_AFRun.Series.Clear()            'Clear chart series before populating 
        QfitAFList.Clear()                                  'clear list of all runs using Quadratic Fits
        FocusRunGraph.DropDownList_AFruns.Items.Clear()     'clear dropdownlist

        'Populate first element of dropdownlist
        FocusRunGraph.DropDownList_AFruns.Items.Add("ALL AUTO FOCUS RUNS")

        'Add all AF curves to chart
        For ns = 0 To AutoFocusRunList.Count - 1
            indexAFrun = ns

            'convert to Arrays
            tempFocusPosition = AutoFocusRunList.Item(indexAFrun).FocusPosition.ToArray()
            tempFocusHFR = AutoFocusRunList.Item(indexAFrun).FocusHFR.ToArray()

            'sort arrays for plotting
            Array.Sort(tempFocusPosition, tempFocusHFR)
            'BestFocusIdx = Array.IndexOf(tempFocusPosition, AutoFocusRunList.Item(indexAFrun).BestFocusPosition)
            BestFocusIdx = Array.IndexOf(tempFocusHFR, AutoFocusRunList.Item(indexAFrun).BestFocusHFR)

            'Name in chart
            If FilterList.Count = 1 Then
                RunLegendName = "File ID: " + AutoFocusRunList.Item(indexAFrun).FileID + ", AF Run: " + (ns + 1).ToString
            Else
                RunLegendName = "File ID: " + AutoFocusRunList.Item(indexAFrun).FileID + ", AF Run: " + (ns + 1).ToString + ", Filter: " + AutoFocusRunList.Item(indexAFrun).Filter
            End If


            'Add to chart
            With FocusRunGraph.Chart_AFRun
                .Series.Add(RunLegendName)
                .Series(ns).Points.DataBindXY(tempFocusPosition, tempFocusHFR)
                .Series(ns).ChartType = SeriesChartType.Line
                .Series(ns).BorderWidth = 2
                .Series(ns).MarkerStyle = MarkerStyle.Square
                .Series(ns).MarkerSize = 8
                .Series(ns).Color = AFplotColor
                .ChartAreas(0).AxisX.Title = "Position (steps)"
                .Series(ns).ToolTip = "#VALX : #VALY"
                If ASTAP Then
                    .ChartAreas(0).AxisY.Title = "HFD"
                Else
                    .ChartAreas(0).AxisY.Title = "HFR"
                End If

            End With

            'highlight best focus position
            If BestFocusIdx > -1 Then
                With FocusRunGraph.Chart_AFRun.Series(ns).Points(BestFocusIdx)
                    .MarkerColor = Color.Black
                    .MarkerSize = 8
                    .MarkerStyle = MarkerStyle.Diamond
                End With
            End If

            'Populate dropdownlist with AF run
            FocusRunGraph.DropDownList_AFruns.Items.Add(RunLegendName)
        Next
        'Select ALL AF RUNS as default
        FocusRunGraph.DropDownList_AFruns.SelectedIndex = 0

        'set scales according to real data points (not the fitted curves)
        FocusRunGraph.SetXYScales()

        'apply color & update
        FocusRunGraph.Chart_AFRun.ApplyPaletteColors()
        FocusRunGraph.Chart_AFRun.Update()

        'add quadratic curve if the run was fitted with Qfit
        For ns = 0 To AutoFocusRunList.Count - 1
            If AutoFocusRunList.Item(ns).Qfit Then
                'add run to list of runs using Qfit
                QfitAFList.Add(ns)

                'Add fitted line to char
                Dim Npoints As Integer = 1000
                Dim x(Npoints - 1) As Double
                Dim y(Npoints - 1) As Double
                Dim MaxX As Double
                Dim MinX As Double
                Dim a, b, c As Double
                Dim Nc As Integer

                Nc = AutoFocusRunList.Count

                a = AutoFocusRunList(ns).Qparams(0) / 1000000.0
                b = AutoFocusRunList(ns).Qparams(1) / 1000.0
                c = AutoFocusRunList(ns).Qparams(2)

                tempFocusPosition = AutoFocusRunList(ns).FocusPosition.ToArray()
                tempFocusHFR = AutoFocusRunList(ns).FocusHFR.ToArray()
                MinX = tempFocusPosition.Min
                MaxX = tempFocusPosition.Max
                'create QF fit line points
                FocusRunGraph.Chart_AFRun.Series.Add("Qfit " + (ns + 1).ToString)
                For k As Integer = 0 To Npoints - 1
                    x(k) = 1.15 * (MaxX - MinX) / Npoints * (k - Npoints / 2) + (MaxX + MinX) / 2 '1.15 is to plot the line over a bit larger range
                    y(k) = a * x(k) ^ 2 + b * x(k) + c

                    'if option Hyperbolic fit is set we need to take square root of y
                    If OptionsForm.CheckBox_HyperbolicFit.Checked = True Then
                        y(k) = Math.Sqrt(y(k))
                    End If
                Next
                'add quality number to dropdownlist text
                FocusRunGraph.DropDownList_AFruns.Items(ns + 1) = FocusRunGraph.DropDownList_AFruns.Items(ns + 1) + ", Quality=" + AutoFocusRunList(ns).Qparams(3).ToString() + "%"

                'add and format points
                With FocusRunGraph.Chart_AFRun
                    .Series.Last.Points.DataBindXY(x, y)
                    .Series.Last.ChartType = SeriesChartType.Line
                    .Series.Last.BorderWidth = 1
                    .Series.Last.MarkerStyle = MarkerStyle.None
                    .Series.Last.Color = .Series(ns).Color
                    .Series.Last.Enabled = True
                    'remove line of corresponding HFR series
                    .Series(ns).BorderWidth = 0
                End With
            End If
        Next
        'set the index dropdownlist
        FocusRunGraph.DropDownList_AFruns.SelectedIndex = old_index

        'set form size Mainform
        FocusRunGraph.Location = New Point((screenWidth - screenWidth / screenFraction) / 2, (screenHeight - screenHeight / screenFraction) / 2)
        FocusRunGraph.Width = screenWidth / screenFraction / 1.5
        FocusRunGraph.Height = screenHeight / screenFraction

        'change color schems
        If CustomColorScheme Then
            ChangeControlColors(FocusRunGraph)
        End If
    End Sub

    Public Sub hyperbolic_fit()
        'Advanced option to re-fit data with hyperbolic function
        'here we fit the data and replace the a,b,c values for each AF run with the hyperbolic ones
        'note that we also need to take the square root of the y curve for plotting

        Dim length As Integer

        For ns = 0 To AutoFocusRunList.Count - 1

            length = AutoFocusRunList(ns).FocusHFR.Count

            If length > 2 Then
                AutoFocusRunList(ns).Qfit = True

                Dim yp(length - 1) As Double
                Dim x(length - 1) As Double

                'calculate the square y^2
                For k = 0 To length - 1
                    yp(k) = AutoFocusRunList(ns).FocusHFR(k) * AutoFocusRunList(ns).FocusHFR(k)
                    x(k) = AutoFocusRunList(ns).FocusPosition(k) / 1000.0   'use same scale down as in SGP
                Next

                'Fit yp=y^2 vs x as a linear regression
                '       y^2 = a*(x-x0)^2 + ymin^2 is the model function
                'then   yp=a*x^2 + b*x + c, where x0=-b/(2*a) and ymin = sqrt(c-x0^2*a)   
                Dim resFit As Double()
                resFit = Fit.Polynomial(x, yp, 2)
                'c=resFit(0), b = resFit(1) and a = resFit(2) 

                Dim x0, ymin As Double
                x0 = -resFit(1) / (2 * resFit(2)) * 1000.0  'note scaling again
                ymin = Math.Sqrt(resFit(0) - x0 ^ 2 * resFit(2) / 1000000.0) 'note scaling again

                'Assign best values
                'Show the estimated best focus point in the list, but keep the measured lowest HFR point in the list
                'This also assures that the best focus point is shown in the AF graph as a black diamond
                AutoFocusRunList(ns).BestFocusPosition = CInt(x0)
                'AutoFocusRunList(ns).BestFocusHFR = Math.Round(ymin, 2)

                'set new a,b,c values
                AutoFocusRunList(ns).Qparams(0) = resFit(2)
                AutoFocusRunList(ns).Qparams(1) = resFit(1)
                AutoFocusRunList(ns).Qparams(2) = resFit(0)

                're-calculate Qfit
                AutoFocusRunList(ns).Qparams(3) = Math.Round(R2value(x, yp, AutoFocusRunList(ns).Qparams(0), AutoFocusRunList(ns).Qparams(1), AutoFocusRunList(ns).Qparams(2)) * 100)
            Else
                AutoFocusRunList(ns).Qfit = False
            End If
        Next
    End Sub

    Public Sub populate_PosTempChart()
        Dim Temp As New List(Of Double)
        Dim Pos As New List(Of Double)
        Dim fit_coeff() As Double
        Dim MinX As Double
        Dim MaxX As Double
        Dim Filter As String
        Dim indFilter As Integer = 0
        Dim indData As Integer = 0
        Dim indLine As Integer = 1
        Dim old_index As Integer

        'store old selected line
        old_index = PositionTempGraph.DropDownList_Filters.SelectedIndex
        If old_index = -1 Then 'in case the dropdowlist contains no element (before redrawing)
            old_index = 0 'set to first line
        End If

        PositionTempGraph.Chart_PosTemp.Series.Clear()      'clear all series
        FittedTempCoeffs.Clear()                            'clear list of fitted linear coefficients

        'initialize to none
        PositionTempGraph.Label_Fit.Text = ""

        'initilize MinX and MaxX
        MinX = AutoFocusRunList(0).Temperature
        MaxX = AutoFocusRunList(0).Temperature

        'initilize 
        PositionTempGraph.DropDownList_Filters.Items.Clear()
        AFrun_FilterSeries_List.Clear()
        For Each Filter In FilterList
            PositionTempGraph.DropDownList_Filters.Items.Add(Filter)

            'Clear lists of Temperature and Position
            Temp.Clear()
            Pos.Clear()

            'create arrays with temp and pos data
            For ns As Integer = 0 To AutoFocusRunList.Count - 1
                'only include point if filter is ForFilter
                If AutoFocusRunList(ns).Filter.Equals(Filter) Then
                    Temp.Add(AutoFocusRunList(ns).Temperature)
                    Pos.Add(AutoFocusRunList(ns).BestFocusPosition)
                    AFrun_FilterSeries_List.Add(New Tuple(Of Integer, Integer)(ns, Temp.Count - 1))
                End If

                If AutoFocusRunList(ns).Temperature > MaxX Then
                    MaxX = AutoFocusRunList(ns).Temperature
                End If
                If AutoFocusRunList(ns).Temperature < MinX Then
                    MinX = AutoFocusRunList(ns).Temperature
                End If
            Next

            'Add data to chart
            With PositionTempGraph.Chart_PosTemp
                .Series.Add(Filter)
                .Series(indData).Points.DataBindXY(Temp.ToArray, Pos.ToArray)
                .Series(indData).ChartType = SeriesChartType.Line
                .Series(indData).BorderWidth = 0
                .Series(indData).MarkerStyle = MarkerStyle.Square
                .Series(indData).MarkerColor = FilterColorList(indFilter)
                .Series(indData).MarkerSize = 10
                .Series(indData).ToolTip = "#VALX : #VALY"
                .ChartAreas(0).AxisX.Title = "Temperature (deg)"
                .ChartAreas(0).AxisY.Title = "Position (steps)"
            End With

            'Add series for line
            PositionTempGraph.Chart_PosTemp.Series.Add(Filter + " - fit")
            If Temp.Count > 1 Then
                'Add fitted line to chart
                Dim Npoints As Integer = 1000
                Dim x As Double
                Dim y As Double

                'do the linear fit
                fit_coeff = TempCoeff(Temp, Pos, Filter)
                FittedTempCoeffs.Add(fit_coeff)

                'add line
                For k As Integer = 0 To Npoints
                    x = 1.25 * (MaxX - MinX) / Npoints * (k - Npoints / 2) + (MaxX + MinX) / 2 '1.25 is to plot the line over a bit larger range
                    y = fit_coeff(0) + fit_coeff(1) * x
                    PositionTempGraph.Chart_PosTemp.Series(indLine).Points.AddXY(x, y)
                Next
                'format line
                With PositionTempGraph.Chart_PosTemp
                    .Series(indLine).ChartType = SeriesChartType.Line
                    .Series(indLine).BorderWidth = 2
                    .Series(indLine).MarkerStyle = MarkerStyle.None
                    .Series(indLine).Color = FilterColorList(indFilter)
                    .Series(indLine).Enabled = False
                End With
            Else
                'in case the data could not be fitted (1 point), then set coeffs to zero
                FittedTempCoeffs.Add({0.0, 0.0, 0.0})
            End If

            'increase Series indices
            indData = indData + 2
            indLine = indLine + 2
            indFilter = indFilter + 1

        Next
        'order AF run vs Filter point series list
        AFrun_FilterSeries_List = AFrun_FilterSeries_List.OrderBy(Function(i) i.Item1).ToList

        'style of excluded points
        For Each point In Filter_ExPoints
            Dim ns As Integer = PositionTempGraph.Chart_PosTemp.Series.IndexOf(point.Item1)
            PositionTempGraph.Chart_PosTemp.Series(ns).Points(point.Item2).MarkerStyle = MarkerStyle.Cross
        Next

        'Select same line as before
        PositionTempGraph.DropDownList_Filters.SelectedIndex = old_index
        PositionTempGraph.Chart_PosTemp.Series(2 * old_index + 1).Enabled = True

        PositionTempGraph.Chart_PosTemp.Update()

        'get middle temp
        MiddleTemp = (MinX + MaxX) / 2

        'change color schems
        If CustomColorScheme Then
            ChangeControlColors(PositionTempGraph)
        End If
    End Sub

    Private Sub GetFilterList()
        Dim Filter As String
        Dim Rand As New System.Random()
        Dim ColorNames As New List(Of String)
        Dim ColorName As System.Type = GetType(System.Drawing.Color)
        Dim ColorPropInfo As System.Reflection.PropertyInfo() = ColorName.GetProperties()

        'Get list of system colors
        For Each CPI As System.Reflection.PropertyInfo In ColorPropInfo
            If CPI.PropertyType.Name = "Color" And CPI.Name <> "Transparent" Then
                'If Color.FromName(CPI.Name).GetBrightness < 0.5 Then 'ONLY ADD DARK COLORS
                If Color.FromName(CPI.Name).GetBrightness > 0.5 Then 'ONLY ADD BRIGHT COLORS
                    ColorNames.Add(CPI.Name)
                End If
            End If
        Next

        'check if there are more different filters
        For ns As Integer = 0 To AutoFocusRunList.Count - 1
            Filter = AutoFocusRunList(ns).Filter
            If FilterList.IndexOf(Filter) = -1 Then
                FilterList.Add(Filter)

                Select Case Filter
                    Case "R", "Red", "RED"
                        FilterColorList.Add(Color.Red)
                    Case "G", "Green", "GREEN"
                        FilterColorList.Add(Color.Green)
                    Case "B", "Blue", "BLUE"
                        FilterColorList.Add(Color.Blue)
                    Case "L", "Lum", "LUM"
                        FilterColorList.Add(Color.FromArgb(60, 60, 60))
                    Case "Ha"
                        FilterColorList.Add(Color.SaddleBrown)
                    Case "OIII"
                        FilterColorList.Add(Color.YellowGreen)
                    Case "SII"
                        FilterColorList.Add(Color.Magenta)
                    Case "None"
                        FilterColorList.Add(Color.FromArgb(60, 60, 60))
                    Case Else
                        FilterColorList.Add(Color.FromName(ColorNames(Rand.Next(0, ColorNames.Count))))
                End Select

            End If
        Next
    End Sub


    Private Function FilterOffsetCalulation(ByVal midT As Double) As Double()
        Dim fit_coeff() As Double
        Dim FilterPos As List(Of Double) = New List(Of Double)
        Dim RefFilter As Integer
        Dim RefPosition As Double

        For k As Integer = 0 To FittedTempCoeffs.Count - 1
            fit_coeff = FittedTempCoeffs(k)
            FilterPos.Add(fit_coeff(0) + midT * fit_coeff(1))
        Next

        If FilterList.Contains("Lum") Then
            RefFilter = FilterList.IndexOf("Lum")
        ElseIf FilterList.Contains("L") Then
            RefFilter = FilterList.IndexOf("L")
        ElseIf FilterList.Contains("LUM") Then
            RefFilter = FilterList.IndexOf("LUM")
        ElseIf FilterList.Contains("Luminosity") Then
            RefFilter = FilterList.IndexOf("Luminosity")
        ElseIf FilterList.Contains("Clear") Then
            RefFilter = FilterList.IndexOf("Clear")
        Else
            RefFilter = 0
        End If

        RefPosition = FilterPos(RefFilter)
        For k As Integer = 0 To FilterPos.Count - 1
            FilterPos(k) = FilterPos(k) - RefPosition
        Next

        Return FilterPos.ToArray
    End Function


    Private Sub save_to_file()
        Dim ns As Integer
        Dim indexAFrun As Integer
        Dim tempFocusPosition() As Object
        Dim tempFocusHFR() As Object
        Dim tempFocusStars() As Object

        'Add all AF curves to file
        For ns = 0 To AutoFocusRunList.Count - 1
            indexAFrun = ns

            'convert to Arrays
            tempFocusPosition = AutoFocusRunList.Item(indexAFrun).FocusPosition.ToArray()
            tempFocusHFR = AutoFocusRunList.Item(indexAFrun).FocusHFR.ToArray()
            tempFocusStars = AutoFocusRunList.Item(indexAFrun).FocusStars.ToArray()

            'write to textfile
            Dim filename As String = OptionsForm.TextBox_SaveFilePath.Text
            Dim filemessage As String
            'Create header
            filemessage = (ns + 1).ToString
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).FileID
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).Temperature.ToString
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).Filter
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).StartDate
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).StartTime
            filemessage = filemessage + vbTab + AutoFocusRunList.Item(indexAFrun).Misc
            filemessage = filemessage.Replace(",", ".")
            My.Computer.FileSystem.WriteAllText(filename, filemessage + vbCrLf, True)
            'Write table of data for run
            Dim k As Integer
            For k = 0 To tempFocusHFR.Length - 1
                filemessage = tempFocusPosition(k).ToString + vbTab + tempFocusHFR(k).ToString + vbTab + tempFocusStars(k).ToString
                filemessage = filemessage.Replace(",", ".")
                My.Computer.FileSystem.WriteAllText(filename, filemessage + vbCrLf, True)
            Next
            'write empty line
            filemessage = ""
            My.Computer.FileSystem.WriteAllText(filename, filemessage + vbCrLf, True)
        Next

    End Sub

    Private Sub Button_ReloadFile_Click(sender As Object, e As EventArgs) Handles Button_ReloadFile.Click
        HandleLoadingAllFiles(SGPLogFilenames)
    End Sub

    Private Sub Button_ScanDetails_Click(sender As Object, e As EventArgs) Handles Button_ScanDetails.Click
        If Not ScanDetails.Visible Then
            Refresh_TextBox()
            ScanDetails.Show()

            'set form size Scan Details
            ScanDetails.Location = New Point((screenWidth - screenWidth / screenFraction) / 2, (screenHeight - screenHeight / screenFraction) / 2)
            ScanDetails.Width = screenWidth / screenFraction
            ScanDetails.Height = screenHeight / screenFraction
        Else
            ScanDetails.BringToFront()
        End If

    End Sub

    Private Sub Button_AFGraphs_Click(sender As Object, e As EventArgs) Handles Button_AFGraphs.Click
        If Not FocusRunGraph.Visible Then
            populate_AFChart()
            FocusRunGraph.Show()
        Else
            FocusRunGraph.BringToFront()
        End If
    End Sub

    Private Sub Button_PosTempGraph_Click(sender As Object, e As EventArgs) Handles Button_PosTempGraph.Click
        If Not PositionTempGraph.Visible Then
            populate_PosTempChart()
            PositionTempGraph.Show()

            'set form size
            PositionTempGraph.Location = New Point((screenWidth - screenWidth / screenFraction) / 2, (screenHeight - screenHeight / screenFraction) / 2)
            PositionTempGraph.Width = screenWidth / screenFraction
            PositionTempGraph.Height = screenHeight / screenFraction
        Else
            PositionTempGraph.BringToFront()
        End If
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.SaveFilePath = OptionsForm.TextBox_SaveFilePath.Text
        My.Settings.ColorScheme = OptionsForm.DropDownList_Colors.SelectedIndex
    End Sub

    Private Sub MainForm_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim DropFiles() As String

        DropFiles = e.Data.GetData(DataFormats.FileDrop)
        HandleLoadingAllFiles(DropFiles)
    End Sub

    Private Sub MainForm_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub Button_Advanced_Click(sender As Object, e As EventArgs) Handles Button_Advanced.Click
        OptionsForm.ShowDialog()
    End Sub

    Public Sub ChangeControlColors(ByRef MainParent As System.Windows.Forms.Control)
        'If MainParent is a form we need to change the Backcolor of the form first
        MainParent.BackColor = FormBkgColor
        MainParent.ForeColor = FormForeColor

        'Now change color of all the controls and their children
        For Each C As System.Windows.Forms.Control In MainParent.Controls()
            If C.HasChildren Then ChangeControlColors(C)
            If Not C.Tag = "NoColorChange" Then
                C.BackColor = FormBkgColor
                C.ForeColor = FormForeColor
                C.Font = FormFont

                If TypeOf C Is Button Then
                    C.BackColor = BtnBkgColor
                    C.ForeColor = BtnForeColor
                    C.Font = BtnFont
                    DirectCast(C, Button).FlatStyle = FlatStyle.Flat
                    DirectCast(C, Button).FlatAppearance.BorderColor = BtnBorderColor
                    DirectCast(C, Button).FlatAppearance.BorderSize = 1
                End If

                If TypeOf C Is Chart Then
                    Dim ChartObj As Chart = DirectCast(C, Chart)

                    ChartObj.BackColor = FormBkgColor
                    ChartObj.ForeColor = FormForeColor
                    ChartObj.Font = FormFont

                    ChartObj.ChartAreas(0).BackColor = FormBkgColor
                    ChartObj.Series(0).Font = FormFont
                    ChartObj.ChartAreas(0).AxisX.LineColor = FormForeColor
                    ChartObj.ChartAreas(0).AxisY.LineColor = FormForeColor

                    ChartObj.ChartAreas(0).AxisX.MajorGrid.LineColor = FormForeColor
                    ChartObj.ChartAreas(0).AxisY.MajorGrid.LineColor = FormForeColor

                    ChartObj.ChartAreas(0).AxisX.LabelStyle.ForeColor = FormForeColor
                    ChartObj.ChartAreas(0).AxisY.LabelStyle.ForeColor = FormForeColor

                    ChartObj.ChartAreas(0).AxisX.TitleForeColor = FormForeColor
                    ChartObj.ChartAreas(0).AxisY.TitleForeColor = FormForeColor

                    ChartObj.ChartAreas(0).AxisX.MajorTickMark.LineColor = FormForeColor
                    ChartObj.ChartAreas(0).AxisY.MajorTickMark.LineColor = FormForeColor
                End If

            End If
        Next
    End Sub

    Private Sub OnDrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
        'This code changes the Tab color my manually painting the tab and putting the tab label on it

        Dim myTabRect As Rectangle
        Dim N As Integer
        Dim g As Graphics = e.Graphics
        Dim format = New System.Drawing.StringFormat

        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center

        N = TabControl.TabPages.Count

        For k As Integer = 0 To N - 1
            myTabRect = TabControl.GetTabRect(k)

            g.FillRectangle(New SolidBrush(BtnBkgColor), myTabRect)
            g.DrawRectangle(New Pen(BtnBorderColor, 2), myTabRect)
            g.DrawString(TabControl.TabPages(k).Text, Font, New SolidBrush(BtnForeColor), myTabRect, format)
        Next
    End Sub

    Private Sub SetColorScheme()

        If SelectedColorScheme = 1 Then
            'Gray-White color scheme
            FormBkgColor = Color.Gray
            FormForeColor = Color.White
            FormFont = New Font("Segoe UI", 8)
            BtnBkgColor = Color.DimGray
            BtnForeColor = Color.White
            BtnBorderColor = Color.White
            BtnFont = New Font("Segoe UI", 8)
            AFplotColor = Color.Ivory
        ElseIf SelectedColorScheme = 2 Then
            'Gray-Gold color scheme
            FormBkgColor = Color.Gray
            FormForeColor = Color.Gold
            FormFont = New Font("Segoe UI", 8)
            BtnBkgColor = Color.DimGray
            BtnForeColor = Color.Gold
            BtnBorderColor = Color.Gold
            BtnFont = New Font("Segoe UI", 8)
            AFplotColor = Color.Gold
        ElseIf SelectedColorScheme = 3 Then
            'Blue-White color scheme
            FormBkgColor = Color.LightSteelBlue
            FormForeColor = Color.Black
            FormFont = New Font("Segoe UI", 8)
            BtnBkgColor = Color.SteelBlue
            BtnForeColor = Color.White
            BtnBorderColor = Color.White
            BtnFont = New Font("Segoe UI", 8)
            AFplotColor = Color.RoyalBlue
        End If

    End Sub

End Class

Public Class AutoFocusRun
    Public Property FileID As String
    Public Property BestFocusPosition As Integer
    Public Property BestFocusHFR As Double
    Public Property Qfit As Boolean
    Public Property Qmin As Double
    Public Property Qparams As ArrayList
    Public Property Temperature As Double
    Public Property FocusPosition As ArrayList
    Public Property FocusHFR As ArrayList
    Public Property FocusStars As ArrayList
    Public Property Filter As String
    Public Property StartTime As String
    Public Property StartDate As String
    Public Property Target As String
    Public Property Misc As String

    Public Sub New(ByVal _FileID As String, ByVal _BestFocusPosition As Integer, ByVal _BestFocusHFR As Double, ByVal _Qfit As Boolean, ByVal _Qmin As Double, ByVal _Qparams As ArrayList, ByVal _Temperature As Double, ByVal _FocusPosition As ArrayList, ByVal _FocusHFR As ArrayList, ByVal _FocusStars As ArrayList, ByVal _Filter As String, ByVal _StartTime As String, ByVal _StartDate As String, ByVal _Target As String, ByVal _Misc As String)
        MyClass.FileID = _FileID
        MyClass.BestFocusPosition = _BestFocusPosition
        MyClass.BestFocusHFR = _BestFocusHFR
        MyClass.Qfit = _Qfit
        MyClass.Qmin = _Qmin
        MyClass.Qparams = DirectCast(_Qparams.Clone(), ArrayList)
        MyClass.Temperature = _Temperature
        MyClass.FocusPosition = DirectCast(_FocusPosition.Clone(), ArrayList)
        MyClass.FocusHFR = DirectCast(_FocusHFR.Clone(), ArrayList)
        MyClass.FocusStars = DirectCast(_FocusStars.Clone(), ArrayList)
        MyClass.Filter = _Filter
        MyClass.StartTime = _StartTime
        MyClass.StartDate = _StartDate
        MyClass.Target = _Target
        MyClass.Misc = _Misc
    End Sub
End Class
