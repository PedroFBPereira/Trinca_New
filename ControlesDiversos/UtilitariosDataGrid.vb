Imports System.Windows.Forms

Public Class UtilitariosDatagrid

    Public Shared Sub AddTextBoxCol(dg As DataGridView, colName As String, dataProperty As String, visible As Boolean, colMode As DataGridViewAutoSizeColumnMode)
        Dim col As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        col.Name = colName
        col.DataPropertyName = dataProperty
        col.Visible = visible
        col.AutoSizeMode = colMode
        dg.Columns.Add(col)
    End Sub

    Public Shared Sub AddCheckBoxCol(dg As DataGridView, colName As String, dataProperty As String, visible As Boolean)
        Dim col As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        col.Name = colName
        col.DataPropertyName = dataProperty
        col.Visible = visible
        dg.Columns.Add(col)
    End Sub


    Public Shared Sub AddComboBoxCol(dg As DataGridView, colName As String, dataProperty As String, visible As Boolean, colMode As DataGridViewAutoSizeColumnMode, colDataSource As DataTable, colDisplay As String, colValue As String)
        Dim col As DataGridViewComboBoxColumn = New DataGridViewComboBoxColumn()
        col.Name = colName
        col.DataPropertyName = dataProperty
        col.Visible = visible
        col.AutoSizeMode = colMode
        col.DataSource = colDataSource
        col.DisplayMember = colDisplay
        col.ValueMember = colValue
        dg.Columns.Add(col)
    End Sub
End Class
