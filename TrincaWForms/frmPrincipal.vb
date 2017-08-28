Imports TrincaBOs
Imports ControlesDiversos
Imports ControlesDiversos.UtilitariosDatagrid

Public Class frmPrincipal
    ' variáveis do form
    Private meuDataSet As DataSet
    Private meuBOChurrasco As New BOChurrasco()
    Private meuFiltroChurrasco As New FiltroChurrasco()
    Private meuBOParticipante As New BOParticipante()
    Private meuFiltroParticipante As New FiltroParticipante()
    Private meuBOChurrascoParticipante As New BOChurrascoParticipante
    Private meuFiltroChurrascoParticipante As New FiltroChurrascoParticipante()

    Private srcChurrasos As New BindingSource
    Private srcChurrParticipantes As New BindingSource
    Private srcEstatisticas As New BindingSource

    ' FALTOU FAZER A ATUALIZAÇÃO DOS DADOS NA BASE:-(
    ' "Seria" na forma
    ' 
    'ds.Tables["tableMaster"].RowChanged += masterList_RowChanged;
    'ds.Tables["tableMaster"].RowDeleting += masterList_RowDeleting;
    'ds.Tables["tableMaster"].RowDeleted += masterList_RowDeleted;
    '
    ' No RowChanged, eu chamaria o método "Salvar" dos meus BOs
    ' e faria um AcceptChanges do DS...



    Private Sub frmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dgChurrascos.AutoGenerateColumns = False
        dgChurrParticipantes.AutoGenerateColumns = False
        dgEstatisticas.AutoGenerateColumns = False

        Try
            CarregaDados()
            ConfiguraColunasGrids()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub CarregaDados()

        Dim tabChurrascos As DataTable = meuBOChurrasco.CarregaDT(meuFiltroChurrasco, meuFiltroChurrasco.MaxReg.GetValueOrDefault)
        Dim tabParticipantes As DataTable = meuBOParticipante.CarregaDT(meuFiltroParticipante, meuFiltroParticipante.MaxReg.GetValueOrDefault)
        Dim tabChurrascosParticipantes As DataTable = meuBOChurrascoParticipante.CarregaDT(meuFiltroChurrascoParticipante, meuFiltroChurrascoParticipante.MaxReg.GetValueOrDefault)
        Dim tabEstatisticas As DataTable = meuBOChurrasco.CarregaDTEstatistica()

        meuDataSet = New DataSet

        meuDataSet.Tables.Add(tabChurrascos) ' esta vai ser a Table 0
        meuDataSet.Tables(0).TableName = "tabChurrascos"
        meuDataSet.Tables.Add(tabParticipantes) ' Table 1
        meuDataSet.Tables(1).TableName = "tabParticipantes"
        meuDataSet.Tables.Add(tabChurrascosParticipantes) ' Table 2
        meuDataSet.Tables(2).TableName = "tabChurrascosParticipantes"
        meuDataSet.Tables.Add(tabEstatisticas) ' Table 3
        meuDataSet.Tables(3).TableName = "tabEstatisticas"

        Dim relChurrascosParticipantes As DataRelation
        relChurrascosParticipantes = New DataRelation("relChurrascosParticipantes", meuDataSet.Tables("tabChurrascos").Columns("COD"), meuDataSet.Tables("tabChurrascosParticipantes").Columns("COD_CHURRASCO"))
        meuDataSet.Relations.Add(relChurrascosParticipantes)

        Dim relChurrascosEstatisticas As DataRelation
        relChurrascosEstatisticas = New DataRelation("relChurrascosEstatisticas", meuDataSet.Tables("tabChurrascos").Columns("COD"), meuDataSet.Tables("tabEstatisticas").Columns("COD_CHURRASCO"))
        meuDataSet.Relations.Add(relChurrascosEstatisticas)

        srcChurrasos.DataSource = meuDataSet
        srcChurrasos.DataMember = "tabChurrascos"
        dgChurrascos.DataSource = srcChurrasos

        srcChurrParticipantes = New BindingSource()
        srcChurrParticipantes.DataSource = srcChurrasos
        srcChurrParticipantes.DataMember = "relChurrascosParticipantes"
        dgChurrParticipantes.DataSource = srcChurrParticipantes

        srcEstatisticas = New BindingSource()
        srcEstatisticas.DataSource = srcChurrasos
        srcEstatisticas.DataMember = "relChurrascosEstatisticas"
        dgEstatisticas.DataSource = srcEstatisticas

    End Sub

    Private Sub ConfiguraColunasGrids()
        AddTextBoxCol(dgChurrascos, "Data", "DATA", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgChurrascos, "Porque", "PORQUE", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgChurrascos, "Observações", "OBS", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgChurrascos, "Contribuição - COM Bebida", "CONTRIB_COMBEBIDA", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgChurrascos, "Contribuição - SEM Bebida", "CONTRIB_SEMBEBIDA", True, DataGridViewAutoSizeColumnMode.Fill)

        AddComboBoxCol(dgChurrParticipantes, "Participante", "COD_PARTICIPANTE", True, DataGridViewAutoSizeColumnMode.Fill, meuDataSet.Tables("tabParticipantes"), "NOME", "COD")
        AddCheckBoxCol(dgChurrParticipantes, "Com Bebida", "COM_BEBIDA", True)
        AddCheckBoxCol(dgChurrParticipantes, "Pago", "PAGO", True)
        AddTextBoxCol(dgChurrParticipantes, "Observações", "OBS", True, DataGridViewAutoSizeColumnMode.Fill)

        AddTextBoxCol(dgEstatisticas, "Nro.Participantes", "NumeroParticipantes", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgEstatisticas, "Bebuns", "Bebuns", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgEstatisticas, "Saudáveis", "Saudaveis", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgEstatisticas, "Valor Total a ser Arrecadado", "ValorASerArrecadado", True, DataGridViewAutoSizeColumnMode.Fill)
        AddTextBoxCol(dgEstatisticas, "Valor já Pago", "ValorJahPago", True, DataGridViewAutoSizeColumnMode.Fill)
    End Sub

End Class
