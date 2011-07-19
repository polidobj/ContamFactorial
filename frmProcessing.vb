Public Class frmProcessing
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
  Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
  Friend WithEvents lblMessage As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
	Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmProcessing))
	Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
	Me.lblMessage = New System.Windows.Forms.Label()
	Me.SuspendLayout()
	'
	'ProgressBar1
	'
	Me.ProgressBar1.Location = New System.Drawing.Point(16, 32)
	Me.ProgressBar1.Name = "ProgressBar1"
	Me.ProgressBar1.Size = New System.Drawing.Size(232, 23)
	Me.ProgressBar1.TabIndex = 0
	'
	'lblMessage
	'
	Me.lblMessage.Location = New System.Drawing.Point(16, 8)
	Me.lblMessage.Name = "lblMessage"
	Me.lblMessage.Size = New System.Drawing.Size(232, 23)
	Me.lblMessage.TabIndex = 1
	Me.lblMessage.Text = "Processing..."
	'
	'frmProcessing
	'
	Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
	Me.ClientSize = New System.Drawing.Size(264, 70)
	Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblMessage, Me.ProgressBar1})
	Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
	Me.MaximizeBox = False
	Me.MinimizeBox = False
	Me.Name = "frmProcessing"
	Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
	Me.Text = "Contam Factorial"
	Me.ResumeLayout(False)

  End Sub

#End Region

End Class
