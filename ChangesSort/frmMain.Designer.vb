<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.OpenFileDialogMain = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialogMain = New System.Windows.Forms.SaveFileDialog()
        Me.btnAction = New System.Windows.Forms.Button()
        Me.tbLogs = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'OpenFileDialogMain
        '
        Me.OpenFileDialogMain.DefaultExt = "vb"
        Me.OpenFileDialogMain.FileName = "SharedAssemblyInfo.vb"
        resources.ApplyResources(Me.OpenFileDialogMain, "OpenFileDialogMain")
        '
        'SaveFileDialogMain
        '
        Me.SaveFileDialogMain.DefaultExt = "vb"
        Me.SaveFileDialogMain.FileName = "SharedAssemblyInfo.vb"
        Me.SaveFileDialogMain.RestoreDirectory = True
        resources.ApplyResources(Me.SaveFileDialogMain, "SaveFileDialogMain")
        '
        'btnAction
        '
        resources.ApplyResources(Me.btnAction, "btnAction")
        Me.btnAction.Name = "btnAction"
        Me.btnAction.UseVisualStyleBackColor = True
        '
        'tbLogs
        '
        resources.ApplyResources(Me.tbLogs, "tbLogs")
        Me.tbLogs.Name = "tbLogs"
        Me.tbLogs.ReadOnly = True
        '
        'btnSave
        '
        resources.ApplyResources(Me.btnSave, "btnSave")
        Me.btnSave.Name = "btnSave"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tbLogs)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnAction)
        Me.Name = "frmMain"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialogMain As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialogMain As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnAction As System.Windows.Forms.Button
    Friend WithEvents tbLogs As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button

End Class
