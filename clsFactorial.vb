Public Class clsFactorial

  Dim cProjectLines As New ArrayList()
  'Public cNumberOfVariables As Integer
  Dim cProjectFilename As String
  'Public cStates As ArrayList
  Public cMaxStates As Integer
  'Public cStatesArray As ArrayList

  Public cSetsOfChanges As Values.SetsOfChanges

  Public Function ReadProjectFile(ByVal ProjectFileName As String) As Boolean

  Dim intFilenumber As Integer = FreeFile()
  Dim CurrentPiece As String

  cProjectFilename = ProjectFileName
  While True
    Try
    FileOpen(intFilenumber, cProjectFilename, OpenMode.Input, OpenAccess.Read)
    Exit While
    Catch When Err.Number = 75
    If MessageBox.Show("Unable to open the project file. Close the file and click retry or click cancel to cancel the project read.", "Unable to open file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) = DialogResult.Cancel Then
      Return False
    End If
    End Try
  End While
  'TODO: verify project file version
  While Not EOF(intFilenumber)
    Me.cProjectLines.Add(LineInput(intFilenumber))
  End While
  FileClose(intFilenumber)
  Return True

  End Function

  Public Function ReadValuesFile(ByVal ValuesFileName As String) As Boolean

  Dim intFilenumber As Integer = FreeFile()
  Dim Reader As New clsStringParse()
  Dim intSetLoop, intVarLoop, intStateLoop As Integer
  Dim NumberOfSets As Integer
  Dim NumberOfVariables As Integer
  Dim ASetOfChanges As Values.ASetofChanges
  Dim aVariable As ArrayList

  While True
    Try
    FileOpen(intFilenumber, ValuesFileName, OpenMode.Input, OpenAccess.Read)
    Exit While
    Catch When Err.Number = 75
    If MessageBox.Show("Unable to open the values file. Close the file and click retry or click cancel to cancel the project read.", "Unable to open file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) = DialogResult.Cancel Then
      Return False
    End If
    End Try
  End While
  Me.cMaxStates = 0
  NumberOfSets = Reader.ReadInteger(intFilenumber, 0)
  Me.cSetsOfChanges = New Values.SetsOfChanges()
  For intSetLoop = 1 To NumberOfSets
    NumberOfVariables = Reader.ReadInteger(intFilenumber, 0)
    ASetOfChanges = New Values.ASetofChanges()
    ASetOfChanges.NumberOfStates = Reader.ReadInteger(intFilenumber, 0)
    ASetOfChanges.SetName = Reader.ReadString(intFilenumber, 0)
    For intVarLoop = 1 To NumberOfVariables
    aVariable = New ArrayList()
    If ASetOfChanges.NumberOfStates > Me.cMaxStates Then Me.cMaxStates = ASetOfChanges.NumberOfStates
    For intStateLoop = 1 To ASetOfChanges.NumberOfStates
      aVariable.Add(Reader.ReadString(intFilenumber, 0))
    Next
    ASetOfChanges.Changes.Add(aVariable)
    Next
    Me.cSetsOfChanges.AddSet(ASetOfChanges)
  Next
  FileClose(intFilenumber)
  Return True

  End Function


    Public Function MakeProjectFiles(ByVal frmProcessing As frmProcessing) As Boolean

        Dim intFileLoop As Integer
        Dim intLinesLoop As Integer
        Dim DollarLocation As Integer
        Dim CurrentFileNumber As Integer
        Dim CurrentPiece As String
        Dim clsPermutations As New clsPermutation()
        Dim permutations As ArrayList
        Dim CurrentPermutation As clsPermutation.aPermutation
        Dim intBatchFilenumber As Integer = FreeFile()
        Dim StatesCountArray As ArrayList
        Dim SetInfo As String
        Dim SetName As String
        Dim ChangeIndex As Integer
        Dim StringParser As New clsStringParse()
        Dim ASetOfChanges As Values.ASetofChanges
        Dim AChange As ArrayList
        Dim Setindex As Integer

        StatesCountArray = Me.CreateStatesArray()
        permutations = clsPermutations.CreatePermutations(StatesCountArray)
        frmProcessing.ProgressBar1.Maximum = permutations.Count - 1
        frmProcessing.Show()
        frmProcessing.Refresh()
        FileOpen(intBatchFilenumber, System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + ".bat", OpenMode.Output, OpenAccess.Write)
        For intFileLoop = 0 To permutations.Count - 1
            frmProcessing.ProgressBar1.Value = intFileLoop
            frmProcessing.Refresh()
            Application.DoEvents()
            CurrentFileNumber = FreeFile()
            CurrentPermutation = CType(permutations(intFileLoop), clsPermutation.aPermutation)
            FileOpen(CurrentFileNumber, System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + "_" + CurrentPermutation.StateString + ".prj", OpenMode.Output, OpenAccess.Write)
            PrintLine(intBatchFilenumber, "ContamX2 " & System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + "_" + CurrentPermutation.StateString + ".prj")
            For intLinesLoop = 0 To Me.cProjectLines.Count - 1
                CurrentPiece = CType(Me.cProjectLines(intLinesLoop), String)
                If intLinesLoop = 1 Then
                    CurrentPiece = WriteDescription(CurrentPiece, CurrentPermutation.StateString)
                End If
                DollarLocation = -1
                While True
                    If CurrentPiece.Length <= 0 Then Exit While
                    DollarLocation = CurrentPiece.IndexOf("$", DollarLocation + 1)
                    If DollarLocation >= 0 Then
                        SetInfo = CurrentPiece.Substring(DollarLocation + 2, CurrentPiece.IndexOf(")", DollarLocation + 2) - (DollarLocation + 2))
                        SetName = StringParser.GetItemFromSpaceLine(SetInfo, 1)
                        ChangeIndex = CInt(StringParser.GetItemFromSpaceLine(SetInfo, 2))
                        'CurrentPermutation = CType(permutations(intFileLoop), String)
                        Setindex = Me.cSetsOfChanges.SetIndex(SetName)
                        ASetOfChanges = Me.cSetsOfChanges.Item(Setindex)
                        AChange = ASetOfChanges.Changes(ChangeIndex - 1)
                        If (CurrentPiece.Substring(0, CurrentPiece.IndexOf("$", 0)).Length > 0) Then
                            CurrentPiece = CurrentPiece.Substring(0, CurrentPiece.IndexOf("$", 0)) & " " & _
                                CType(AChange.Item(CInt(CurrentPermutation.states(Setindex - 1))), String) & " " & _
                                CurrentPiece.Substring(CurrentPiece.IndexOf(")", 0) + 1)
                        Else
                            CurrentPiece = CType(AChange.Item(CInt(CurrentPermutation.states(Setindex - 1))), String) & " " & _
                                CurrentPiece.Substring(CurrentPiece.IndexOf(")", 0) + 1)
                        End If
                    Else
                        Exit While
                    End If
                End While
                PrintLine(CurrentFileNumber, CurrentPiece)
            Next
            FileClose(CurrentFileNumber)
        Next
        FileClose(intBatchFilenumber)
        frmProcessing.Hide()
        MessageBox.Show("Project files created successfully.  They can be run using the " & System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + ".bat file." & vbNewLine & vbNewLine & "NOTE: To use the bat file ContamX2.exe must be in the same directory as the bat file and the project files." & vbNewLine & vbNewLine & "April 16th, 2004", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Function

    Private Function WriteDescription(ByVal desc As String, ByVal StateString As String) As String

        If desc.Length < 72 - StateString.Length + 1 Then
            Return desc & " " & StateString
        Else
            Return desc.Substring(0, 72 - StateString.Length) & " " & StateString
        End If

    End Function

    Private Function CreateStatesArray() As ArrayList

        Dim intSetLoop As Integer
        Dim ASetOfChanges As Values.ASetofChanges
        Dim ResultArray As New ArrayList()

        For intSetLoop = 1 To Me.cSetsOfChanges.NumberOfSets
            ASetOfChanges = Me.cSetsOfChanges.Item(intSetLoop)
            ResultArray.Add(ASetOfChanges.NumberOfStates)
        Next
        Return ResultArray

    End Function

End Class
