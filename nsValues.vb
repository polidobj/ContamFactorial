Imports System.Collections.Generic

Namespace Values


  Public Class SetsOfChanges

  Private cSetsOfChanges As New List(Of ASetofChanges)()
  Public Sub AddSet(ByVal ASetOfChanges As ASetofChanges)
    cSetsOfChanges.Add(ASetOfChanges)
  End Sub

  Public Function SetIndex(ByVal NameOfSet As String) As Integer

    Dim ASet As ASetofChanges
    Dim intloop As Integer

    For intloop = 0 To Me.cSetsOfChanges.Count - 1
    ASet = Me.cSetsOfChanges.Item(intloop)
    If ASet.SetName = NameOfSet Then
      Return intloop
    End If
    Next
    Return Nothing

  End Function

  Public Function NumberOfSets() As Integer
    Return cSetsOfChanges.Count
  End Function

  Public Function Item(ByVal index As Integer) As ASetofChanges
    Return Me.cSetsOfChanges.Item(index)
  End Function
  End Class

  Public Class ASetofChanges

  Public SetName As String
  Public NumberOfStates As Integer
  Public Changes As New List(Of List(Of String))()

  End Class


  Public Class clsVariableArray

  Public cNumberOfIndexes As Integer
  Private cVariableName As String

  Sub New(ByVal VariableName As String)
    cVariableName = VariableName
  End Sub

  End Class

End Namespace
