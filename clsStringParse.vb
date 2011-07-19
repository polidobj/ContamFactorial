Option Strict On
Option Explicit On
Option Compare Binary

'VB implementation of the file reader by GWalton.
Public Class clsStringParse

  'reads a charcter from the input file number
  'intfilenumber - the number of the file from which to read.
  'return - the character read
  Private Function ReadCharacter(ByVal intFileNumber As Integer) As String

  If Not EOF(intFileNumber) Then
    Return InputString(intFileNumber, 1)
  Else
    Return ""
  End If

  End Function

  'reads the next line from the input file
  'intfilenumber - the number of the file from which to read
  'intmaxlength - the maximum length of the string to be returned
  'return - the line read from the file
  Private Function NextLine(ByVal intFileNumber As Integer, ByVal intMaxLength As Short) As String

  Dim strCharacter As String
  Dim strLine As String

  Do
    strCharacter = ReadCharacter(intFileNumber)
    If EOF(intFileNumber) Then
    Return ""
    End If
    If strCharacter = Chr(10) Then
    Return Left(strLine, Len(strLine) - 1)
    End If
    strLine = strLine & strCharacter
    If Len(strLine) > intMaxLength Then
    strLine = Left(strLine, intMaxLength)
    End If
  Loop
  Return strLine

  End Function

  'get the next space-delimited word from the file
  'intfilenumber - the of the file from which to read
  'intflag - determines the type of read (-1 - initialize, 0 - next word from current position, 1 - first word on next line, 2 - get reaminder of current line, 3 - get all of next line)
  Private Function NextWord(ByVal intFileNumber As Integer, ByVal intFlag As Short, ByVal intMaxLength As Short) As String

  Static bolNewLine As Boolean
  Dim strCharacter As String
  Dim strWord As String
  Dim bolDone As Boolean
  Dim strJunk As String

  bolDone = False
  If intFlag = -1 Then
    bolNewLine = False
    Return ""
  End If

  strWord = ""
  If intFlag <> 0 And bolNewLine = False Then
    strWord = NextLine(intFileNumber, intMaxLength)
    bolNewLine = True
  End If

  If intFlag = 2 Then
    Return strWord
  End If

  If intFlag = 3 Then
    Do
    strWord = NextLine(intFileNumber, intMaxLength)
    If Left(strWord, 1) <> "!" Then
      Exit Do
    End If
    Loop
    bolNewLine = True
    Return strWord
  End If

  strWord = ""
  While Not bolDone
    strCharacter = ReadCharacter(intFileNumber)
    If EOF(intFileNumber) Then
    Return ""
    End If
    Select Case strCharacter
    Case " "
    Case ","
    Case Chr(13)
    Case Chr(10)
    Case Chr(9)
    Case ""
    Case "!"
      strJunk = NextLine(intFileNumber, 80)
    Case "*"
      Exit Function
    Case Else
      strWord = strWord & strCharacter
      bolDone = True
    End Select
  End While

  bolDone = False
  bolNewLine = False

  While Not bolDone
    strCharacter = ReadCharacter(intFileNumber)
    If EOF(intFileNumber) Then
    Return strWord
    End If
    Select Case strCharacter
    Case Chr(10)
      bolNewLine = True
      bolDone = True
    Case Chr(13)
    Case " "
      bolDone = True
    Case ","
      bolDone = True
    Case Chr(9)
      bolDone = True
    Case ""
    Case Else
      strWord = strWord & strCharacter
      If Len(strWord) > intMaxLength Then
      strWord = Left(strWord, intMaxLength)
      End If
    End Select
  End While
  Return strWord

  End Function

  'reads a double from the file
  'intfilenumber - the number of the file from which to read
  'intflag - determines the typ of read
  'return - the double read from the file
  Public Function ReadDouble(ByVal intFileNumber As Integer, ByVal intFlag As Short) As Double

  Return CDbl(NextWord(intFileNumber, intFlag, 20))

  End Function

  'reads an integer from the file
  'intfilenumber - the number of the file from which to read
  'intflag - determines the typ of read
  'return - the integer read from the file
  Public Function ReadInteger(ByVal intFileNumber As Integer, ByVal intFlag As Short) As Integer

  Return CInt(NextWord(intFileNumber, intFlag, 20))

  End Function

  'reads an single from the file
  'intfilenumber - the number of the file from which to read
  'intflag - determines the typ of read
  'return - the single read from the file
  Public Function ReadSingle(ByVal intFileNumber As Integer, ByVal intFlag As Short) As Single

  Return CSng(NextWord(intFileNumber, intFlag, 20))

  End Function

  'reads a string from the file
  'intfilenumber - the number of the file from which to read
  'intflag - determines the typ of read
  'return - the string read from the file
  Public Function ReadString(ByVal intFileNumber As Integer, ByVal intFlag As Short) As String

  Return NextWord(intFileNumber, intFlag, 160)

  End Function

  'converts the string representaion of time to integer representation of number of seconds after midight
  'strtime - the string representation of time 
  ' return - the integer representation of time
  Public Function StringTimeToIntegerTime(ByVal strTime As String) As Integer

  Dim strTemp As String
  Dim lngHours As Integer
  Dim lngMinutes As Integer
  Dim lngSeconds As Integer

  lngHours = CInt(Left(strTime, 2))
  lngMinutes = CInt(Mid(strTime, 4, 2))
  lngSeconds = CInt(Right(strTime, 2))
  Return (lngHours * 60 + lngMinutes) * 60 + lngSeconds

  End Function

  'converts the integer representaion of time to string representation of time
  'inttime - the integer representation of time 
  ' return - the string representation of time
  Public Function IntegerTimeToStringTime(ByVal intTime As Integer) As String

  Dim strHours As String
  Dim strMinutes As String
  Dim strSeconds As String
  Dim lngHours As Integer
  Dim lngMinutes As Integer
  Dim lngSeconds As Integer

  lngMinutes = intTime \ 60
  lngSeconds = intTime Mod 60
  lngHours = lngMinutes \ 60
  lngMinutes = lngMinutes Mod 60
  If lngHours < 10 Then
    strHours = "0" & CStr(lngHours)
  Else
    strHours = CStr(lngHours)
  End If
  If lngSeconds < 10 Then
    strSeconds = "0" & CStr(lngSeconds)
  Else
    strSeconds = CStr(lngSeconds)
  End If
  If lngMinutes < 10 Then
    strMinutes = "0" & CStr(lngMinutes)
  Else
    strMinutes = CStr(lngMinutes)
  End If
  Return strHours & ":" & strMinutes & ":" & strSeconds

  End Function

  'determines if the integer time given is a valid time value
  'inttime - the integer representaion of time
  'return - result of validation
  Public Overloads Function ValidateTime(ByVal intTime As Integer) As Boolean

  If intTime > 86400 Or intTime < 0 Then
    Return False
  Else
    Return True
  End If

  End Function

  'determines if the string time given is a valid time value
  'inttime - the string time value
  'return - the result of validation
  Public Overloads Function ValidateTime(ByVal Time As String) As Boolean

  Dim intTime As Integer

  'if the string is not equal to 8 characters long we know it is invalid
  If Len(Time) <> 8 Then
    Return False
  End If
  Try
    'convert the string time to integer time
    'if an error occurs we know it is invalid 
    intTime = StringTimeToIntegerTime(Time)
  Catch
    Return False
  End Try
  'validate the integer time
  Return ValidateTime(intTime)

  End Function

  'gets an field from a tab delimited line - (no spaces allowed in the field)
  'strinputstring - string conatining the tab delimited line
  'intitemnumber - the number of the item to be returned
  'return - the field extracted from the input string
  Public Function GetItemFromTabLine(ByVal strInputString As String, ByVal intItemNumber As Integer) As String

  Dim intLoop As Integer
  Dim strTempString As String
  Dim intStringPosition As Integer

  strTempString = Trim(strInputString)
  For intLoop = 1 To intItemNumber - 1
    intStringPosition = InStr(strTempString, Chr(9))
    strTempString = Trim(Mid(strTempString, intStringPosition + 1))
  Next intLoop
  intStringPosition = InStr(strTempString, Chr(9))
  If intStringPosition > 0 Then
    Return Trim(Left(strTempString, intStringPosition - 1))
  Else
    Return strTempString
  End If

  End Function

  'gets an field from a space delimited line
  'strinputstring - string conatining the space delimited line
  'intitemnumber - the number of the item to be returned
  'return - the field extracted from the input string
  Public Function GetItemFromSpaceLine(ByVal strInputString As String, ByVal intItemNumber As Integer) As String

  Dim intLoop As Integer
  Dim strTempString As String
  Dim intStringPosition As Integer

  strTempString = Trim(strInputString)
  For intLoop = 1 To intItemNumber - 1
    intStringPosition = InStr(strTempString, " ")
    strTempString = Trim(Mid(strTempString, intStringPosition + 1))
  Next intLoop
  intStringPosition = InStr(strTempString, " ")
  If intStringPosition > 0 Then
    Return Trim(Left(strTempString, intStringPosition - 1))
  Else
    Return strTempString
  End If

  End Function

  'gets the number of items in a tab delimited string
  'strinputstring - the string from which to find the number of items
  'return - the number of items in the string
  Public Function NumberOfItemsInLine(ByVal strInputString As String) As Integer

  Dim intStringPosition As Integer
  Dim intNumberOfItems As Integer
  Dim strTempString As String

  intNumberOfItems = 0
  strTempString = Trim(strInputString)
  If strTempString = "" Then
    NumberOfItemsInLine = 0
    Exit Function
  End If
  Do
    intStringPosition = InStr(strTempString, Chr(9))
    'edited 04/08/2004 - counted one too many if the line had a tab at the end
    If intStringPosition > 0 And intStringPosition < strTempString.Length() - 1 Then
    intNumberOfItems = intNumberOfItems + 1
    strTempString = Trim(Mid(strTempString, intStringPosition + 1))
    Else
    Return intNumberOfItems + 1
    End If
  Loop

  End Function

End Class