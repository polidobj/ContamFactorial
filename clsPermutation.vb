Imports System.Collections.Generic

Public Class clsPermutation

  Dim cStates As List(Of Integer)
    Dim cResultArray As List(Of aPermutation)
    Dim CurrentPerm As aPermutation

    Public Class aPermutation

        Public states As List(Of String)

        Public Sub New()
            states = New List(Of String)()
        End Sub

        Public Function StateString() As String

            Dim i As Integer

            StateString = ""
            For i = 0 To states.Count - 1
                StateString &= states(i) & "_"
            Next
            StateString = StateString.Substring(0, StateString.Length - 1)

        End Function

        Public Function Copy() As aPermutation
            Dim i As Integer
            Dim state As String
            Copy = New aPermutation()

            For i = 0 To states.Count - 1
                state = states(i)
                Copy.states.Add(state)
            Next
        End Function
    End Class

    Public Function CreatePermutations(ByVal States As List(Of Integer)) As List(Of aPermutation)

        Dim intloop As Integer

        cResultArray = New List(Of aPermutation)()
        cStates = States
        For intloop = 0 To States(0) - 1
            CurrentPerm = New aPermutation()
            CurrentPerm.states.Add(intloop.ToString())
            RecursiveForLoops(States.Count - 1)
        Next
        Return cResultArray

    End Function

    Private Sub RecursiveForLoops(ByVal LoopsLeft As Integer)

        Dim intloop As Integer

        If LoopsLeft > 0 Then
            For intloop = 0 To cStates(cStates.Count - LoopsLeft) - 1
                CurrentPerm.states.Add(intloop.ToString)
                RecursiveForLoops(LoopsLeft - 1)
                CurrentPerm.states.RemoveAt(CurrentPerm.states.Count - 1)
            Next
        Else
            cResultArray.Add(CurrentPerm.Copy())
        End If

    End Sub

End Class
