Public Class frmContamFactorial
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
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuOpenProject As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
  Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
	Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmContamFactorial))
	Me.MainMenu1 = New System.Windows.Forms.MainMenu()
	Me.MenuItem1 = New System.Windows.Forms.MenuItem()
	Me.mnuOpenProject = New System.Windows.Forms.MenuItem()
	Me.MenuItem2 = New System.Windows.Forms.MenuItem()
	Me.MenuItem4 = New System.Windows.Forms.MenuItem()
	Me.MenuItem5 = New System.Windows.Forms.MenuItem()
	Me.StatusBar1 = New System.Windows.Forms.StatusBar()
	Me.SuspendLayout()
	'
	'MainMenu1
	'
	Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
	'
	'MenuItem1
	'
	Me.MenuItem1.Index = 0
	Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpenProject, Me.MenuItem2, Me.MenuItem4, Me.MenuItem5})
	Me.MenuItem1.Text = "File"
	'
	'mnuOpenProject
	'
	Me.mnuOpenProject.Index = 0
	Me.mnuOpenProject.Text = "Open Project"
	'
	'MenuItem2
	'
	Me.MenuItem2.Index = 1
	Me.MenuItem2.Text = "Create Project Files"
	'
	'MenuItem4
	'
	Me.MenuItem4.Index = 2
	Me.MenuItem4.Text = "-"
	'
	'MenuItem5
	'
	Me.MenuItem5.Index = 3
	Me.MenuItem5.Text = "Exit"
	'
	'StatusBar1
	'
	Me.StatusBar1.Location = New System.Drawing.Point(0, 244)
	Me.StatusBar1.Name = "StatusBar1"
	Me.StatusBar1.Size = New System.Drawing.Size(408, 22)
	Me.StatusBar1.TabIndex = 1
	Me.StatusBar1.Text = "April 9th, 2004"
	'
	'frmContamFactorial
	'
	Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
	Me.ClientSize = New System.Drawing.Size(408, 266)
	Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.StatusBar1})
	Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
	Me.Menu = Me.MainMenu1
	Me.Name = "frmContamFactorial"
	Me.Text = "Contam Factorial"
	Me.ResumeLayout(False)

  End Sub

#End Region

  Dim clsFactorial As clsFactorial

  Private Sub mnuOpenProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenProject.Click

	Dim OFD As New OpenFileDialog()
	Dim intloop, intloop2 As Integer
	Dim ListVItem As ListViewItem
	Dim VariableStates As ArrayList

	OFD.Title = "Select Values File"
	OFD.Filter = "Tab-delimited File (*.txt)|*.txt"
	OFD.FilterIndex = 1
	OFD.FileName = ""
	If OFD.ShowDialog = DialogResult.OK Then
	  clsFactorial = New clsFactorial()
	  If Not Me.clsFactorial.ReadValuesFile(OFD.FileName) Then
		MessageBox.Show("Error Reading values file.", "Invalid values file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		Exit Sub
	  End If
	  OFD.Title = "Select Project File"
	  OFD.Filter = "Contam Project Files (*.prj)|*.prj"
	  OFD.FilterIndex = 1
	  If OFD.ShowDialog = DialogResult.OK Then
		If Not clsFactorial.ReadProjectFile(OFD.FileName) Then
		  MessageBox.Show("Error reading Project.", "Unable to read project file", MessageBoxButtons.OK, MessageBoxIcon.Error)
		  Exit Sub
		End If
		MessageBox.Show("Project file read successfully.", "Project file read OK", MessageBoxButtons.OK, MessageBoxIcon.Information)
	  End If
	End If

  End Sub

  Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click

	clsFactorial.MakeProjectFiles()

  End Sub

  Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem5.Click
	Me.Close()
  End Sub
End Class
