Public Class clsPermutation

  Dim cStates As ArrayList
    Dim cResultArray As ArrayList
    Dim CurrentPerm As aPermutation

    Public Class aPermutation

        Public states As ArrayList

        Public Sub New()
            states = New ArrayList()
        End Sub

        Public Function StateString() As String

            Dim i As Integer

            StateString = ""
            For i = 0 To states.Count - 1
                StateString &= CType(states(i), String).ToString & "_"
            Next
            StateString = StateString.Substring(0, StateString.Length - 1)

        End Function

        Public Function Copy() As aPermutation
            Dim i As Integer
            Dim state As String
            Copy = New aPermutation()

            For i = 0 To states.Count - 1
                state = CType(states(i), String)
                Copy.states.Add(state)
            Next
        End Function
    End Class

    Public Function CreatePermutations(ByVal States As ArrayList) As ArrayList

        Dim intloop As Integer

        cResultArray = New ArrayList()
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
