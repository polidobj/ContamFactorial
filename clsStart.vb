Public Class clsStart

  Shared clsFactorial As clsFactorial

  Shared Sub main()

        Dim OFD As New OpenFileDialog()
        Dim frmProcessing As New frmProcessing()
        OFD.Title = "Select Values File"
        OFD.Filter = "Values File (*.txt)|*.txt"
        OFD.FilterIndex = 1
        OFD.FileName = ""
        frmProcessing.Show()

        If OFD.ShowDialog(frmProcessing) = DialogResult.OK Then
            clsFactorial = New clsFactorial()
            If Not clsFactorial.ReadValuesFile(OFD.FileName) Then
                MessageBox.Show("Error Reading values file.", "Invalid values file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            OFD.Title = "Select Project File"
            OFD.Filter = "Contam Project Files (*.prj)|*.prj|All files (*.*)|*.*"
            OFD.FilterIndex = 1
            If OFD.ShowDialog(frmProcessing) = DialogResult.OK Then
                If Not clsFactorial.ReadProjectFile(OFD.FileName) Then
                    MessageBox.Show("Error reading Project.", "Unable to read project file", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
            clsFactorial.MakeProjectFiles(frmProcessing)
        End If
    End Sub

End Class
