Imports System.Collections.Generic
Imports ContamFactorial.clsPermutation
Imports System.IO

Public Class clsFactorial

  Dim cProjectLines As New List(Of String)()
  Dim cProjectFilename As String
  Public cMaxStates As Integer

  Public cSetsOfChanges As Values.SetsOfChanges

  Public Function ReadProjectFile(ByVal ProjectFileName As String) As Boolean

  Dim intFilenumber As Integer = FreeFile()

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
  While Not EOF(intFilenumber)
    Me.cProjectLines.Add(LineInput(intFilenumber))
  End While
  FileClose(intFilenumber)
  Return True

  End Function

  Public Function ReadValuesFile(ByVal ValuesFileName As String) As Boolean

  Dim intFilenumber As Integer = FreeFile()
  Dim Reader As New clsStringParse()
  Dim NumberOfSets As Integer
  Dim NumberOfVariables As Integer
  Dim ASetOfChanges As Values.ASetofChanges
  Dim aVariable As List(Of String)

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
  For intSetLoop As Integer = 1 To NumberOfSets
    NumberOfVariables = Reader.ReadInteger(intFilenumber, 0)
    ASetOfChanges = New Values.ASetofChanges()
    ASetOfChanges.NumberOfStates = Reader.ReadInteger(intFilenumber, 0)
    ASetOfChanges.SetName = Reader.ReadString(intFilenumber, 0)
    For i As Integer = 1 To ASetOfChanges.NumberOfStates
      ASetOfChanges.StateNames.Add(Reader.ReadString(intFilenumber, 0))
    Next
    For intVarLoop As Integer = 1 To NumberOfVariables
    aVariable = New List(Of String)()
    If ASetOfChanges.NumberOfStates > Me.cMaxStates Then
      Me.cMaxStates = ASetOfChanges.NumberOfStates
    End If
    For intStateLoop As Integer = 1 To ASetOfChanges.NumberOfStates
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

        Dim DollarLocation As Integer
        Dim CurrentFileNumber As Integer
        Dim CurrentPiece As String
        Dim clsPermutations As New clsPermutation()
        Dim permutations As List(Of aPermutation)
        Dim CurrentPermutation As clsPermutation.aPermutation
        Dim intBatchFilenumber As Integer = FreeFile()
        Dim StatesCountArray As List(Of Integer)
        Dim SetInfo As String
        Dim SetName As String
        Dim ChangeIndex As Integer
        Dim StringParser As New clsStringParse()
        Dim ASetOfChanges As Values.ASetofChanges
        Dim AChange As List(Of String)
        Dim Setindex As Integer
        Dim stateNameString As String
        Dim currentFileName As String

        StatesCountArray = Me.CreateStatesArray()
        permutations = clsPermutations.CreatePermutations(StatesCountArray)
        frmProcessing.ProgressBar1.Maximum = permutations.Count - 1
        frmProcessing.Show()
        frmProcessing.Refresh()
        FileOpen(intBatchFilenumber, System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + ".bat", OpenMode.Output, OpenAccess.Write)
        For intFileLoop As Integer = 0 To permutations.Count - 1
            frmProcessing.ProgressBar1.Value = intFileLoop
            frmProcessing.Refresh()
            Application.DoEvents()
            CurrentFileNumber = FreeFile()
            CurrentPermutation = permutations(intFileLoop)
            stateNameString = ""
            For i As Integer = 0 To Me.cSetsOfChanges.NumberOfSets - 1
              stateNameString += Me.cSetsOfChanges.Item(i).StateNames(CurrentPermutation.states(i)) + "_"
            Next
            stateNameString = stateNameString.Substring(0, stateNameString.Length - 1)
            currentFileName = _
              Path.GetDirectoryName(Me.cProjectFilename) & Path.DirectorySeparatorChar & _
              Path.GetFileNameWithoutExtension(Me.cProjectFilename) + _
              "_" + stateNameString + ".prj"
            FileOpen(CurrentFileNumber, currentFileName, OpenMode.Output, OpenAccess.Write)
            PrintLine(intBatchFilenumber, "ContamX3 " + currentFileName)
            For intLinesLoop As Integer = 0 To Me.cProjectLines.Count - 1
                CurrentPiece = Me.cProjectLines(intLinesLoop)
                If intLinesLoop = 1 Then
                    CurrentPiece = WriteDescription(CurrentPiece, stateNameString)
                End If
                DollarLocation = -1
                While True
                    If CurrentPiece.Length <= 0 Then Exit While
                    DollarLocation = CurrentPiece.IndexOf("$", DollarLocation + 1)
                    If DollarLocation >= 0 Then
                        SetInfo = CurrentPiece.Substring(DollarLocation + 2, CurrentPiece.IndexOf(")", DollarLocation + 2) - (DollarLocation + 2))
                        SetName = StringParser.GetItemFromSpaceLine(SetInfo, 1)
                        ChangeIndex = CInt(StringParser.GetItemFromSpaceLine(SetInfo, 2))
                        Setindex = Me.cSetsOfChanges.SetIndex(SetName)
                        ASetOfChanges = Me.cSetsOfChanges.Item(Setindex)
                        AChange = ASetOfChanges.Changes(ChangeIndex - 1)
                        If (CurrentPiece.Substring(0, CurrentPiece.IndexOf("$", 0)).Length > 0) Then
                            CurrentPiece = CurrentPiece.Substring(0, CurrentPiece.IndexOf("$", 0)) & " " & _
                                AChange.Item(CurrentPermutation.states(Setindex)) & " " & _
                                CurrentPiece.Substring(CurrentPiece.IndexOf(")", 0) + 1)
                        Else
                            CurrentPiece = AChange.Item(CurrentPermutation.states(Setindex)) & " " & _
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
        MessageBox.Show("Project files created successfully.  They can be run using the " & _
          System.IO.Path.GetFileNameWithoutExtension(Me.cProjectFilename) + ".bat file." & _
          vbNewLine & vbNewLine & "NOTE: To use the bat file ContamX3.exe must be in the same directory as the bat file and the project files." & _
          vbNewLine & vbNewLine & "v2.0.1 - August 16th, 2012", "Success", MessageBoxButtons.OK, _
          MessageBoxIcon.Information)

    End Function

    Private Function WriteDescription(ByVal desc As String, ByVal StateString As String) As String

        If desc.Length < 72 - StateString.Length + 1 Then
            Return desc & " " & StateString
        Else
            Return desc.Substring(0, 72 - StateString.Length) & " " & StateString
        End If

    End Function

    Private Function CreateStatesArray() As List(Of Integer)

        Dim intSetLoop As Integer
        Dim ASetOfChanges As Values.ASetofChanges
        Dim ResultArray As New List(Of Integer)()

        For intSetLoop = 0 To Me.cSetsOfChanges.NumberOfSets - 1
            ASetOfChanges = Me.cSetsOfChanges.Item(intSetLoop)
            ResultArray.Add(ASetOfChanges.NumberOfStates)
        Next
        Return ResultArray

    End Function

End Class
