<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrincipal
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgChurrascos = New System.Windows.Forms.DataGridView()
        Me.dgChurrParticipantes = New System.Windows.Forms.DataGridView()
        Me.dgEstatisticas = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.dgChurrascos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgChurrParticipantes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgEstatisticas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(29, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(330, 24)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Churrascos da Trinca - Dashboard"
        '
        'dgChurrascos
        '
        Me.dgChurrascos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgChurrascos.Location = New System.Drawing.Point(33, 67)
        Me.dgChurrascos.Name = "dgChurrascos"
        Me.dgChurrascos.Size = New System.Drawing.Size(735, 150)
        Me.dgChurrascos.TabIndex = 2
        '
        'dgChurrParticipantes
        '
        Me.dgChurrParticipantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgChurrParticipantes.Location = New System.Drawing.Point(33, 245)
        Me.dgChurrParticipantes.Name = "dgChurrParticipantes"
        Me.dgChurrParticipantes.Size = New System.Drawing.Size(735, 150)
        Me.dgChurrParticipantes.TabIndex = 3
        '
        'dgEstatisticas
        '
        Me.dgEstatisticas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgEstatisticas.Location = New System.Drawing.Point(33, 425)
        Me.dgEstatisticas.Name = "dgEstatisticas"
        Me.dgEstatisticas.Size = New System.Drawing.Size(735, 150)
        Me.dgEstatisticas.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(33, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Churrascos"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(33, 229)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Participantes do Churrasco selecionado"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(33, 409)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(227, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Estatísticas do Churrasco Selecionado"
        '
        'frmPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 601)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgEstatisticas)
        Me.Controls.Add(Me.dgChurrParticipantes)
        Me.Controls.Add(Me.dgChurrascos)
        Me.Controls.Add(Me.Label2)
        Me.Name = "frmPrincipal"
        Me.Text = "Form1"
        CType(Me.dgChurrascos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgChurrParticipantes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgEstatisticas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label2 As Label
    Friend WithEvents dgChurrascos As DataGridView
    Friend WithEvents dgChurrParticipantes As DataGridView
    Friend WithEvents dgEstatisticas As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
