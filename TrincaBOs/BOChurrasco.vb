Imports ControlesDiversos

Public Class BOChurrasco

#Region "Atributos"

    Private _UltimaAlteracao As Nullable(Of Date)
    Private _Tstamp As Nullable(Of Integer)

    Private meuDB As DBChurrasco

#End Region

    Public Sub New()
        meuDB = New DBChurrasco()
    End Sub

#Region "Propriedades"

    Public Cod As Nullable(Of Integer)
    Public Data As Nullable(Of DateTime)
    Public Porque As String
    Public Obs As String
    Public ContribComBebida As Nullable(Of Decimal)
    Public ContribSemBebida As Nullable(Of Decimal)

    Public Property UltimaAlteracao() As Nullable(Of Date)
        Get
            Return _UltimaAlteracao
        End Get
        Set(ByVal Value As Nullable(Of Date))
            _UltimaAlteracao = Value
        End Set
    End Property

    Public Property Tstamp() As Nullable(Of Integer)
        Get
            Return _Tstamp
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            _Tstamp = Value
        End Set
    End Property


#End Region

    Public Function CarregaDTEstatistica(Optional Trans As Object = Nothing) As DataTable
        Return meuDB.CarregaDTEstatistica()
    End Function

    Public Function Carrega(ByVal Cod As Integer, Optional ByVal Trans As Object = Nothing) As Boolean
        Dim bResultado As Boolean
        bResultado = meuDB.Carrega(Me, Cod, Trans)
        Return bResultado
    End Function


    Public Function CarregaAK1(ByVal prmPorque As String, Optional ByVal Trans As Object = Nothing) As Boolean
        Return meuDB.CarregaAK1(Me, prmPorque, Trans)
    End Function


    Public Function CarregaArray(ByVal Filtro As FiltroChurrasco, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As BOChurrasco()
        Return meuDB.CarregaArray(Of BOChurrasco, FiltroChurrasco)(Filtro, nMaxReg, Trans)
    End Function


    Public Function CarregaDT(ByVal Filtro As FiltroChurrasco, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As DataTable
        Return meuDB.CarregaDT(Filtro, nMaxReg, Trans)
    End Function

    Public Sub CarregaLista(ByRef ctrlComItems As Object, Optional prmFiltro As FiltroChurrasco = Nothing, Optional prmTrans As Object = Nothing)
        ' o parâmetro 'ctrlComItems´ está definido como Object,
        ' mas tem de ser uma combobox 
        ' ou uma listbox
        ' ou outro controle que tenha uma propriedade Itens.
        If prmFiltro Is Nothing Then
            prmFiltro = New FiltroChurrasco()
        End If
        Dim meuMaxReg As Integer = 0
        If prmFiltro.MaxReg.HasValue Then
            meuMaxReg = prmFiltro.MaxReg.Value
        End If
        Dim minhaDatatable As DataTable
        minhaDatatable = meuDB.CarregaDT(prmFiltro, meuMaxReg, prmTrans)
        ctrlComItems.Items.Clear
        For Each minhaDataRow As DataRow In minhaDatatable.Rows
            ctrlComItems.Items.Add(New ListItem(minhaDataRow("Nome"), minhaDataRow("COD")))
        Next
    End Sub
    Public Sub Salva(Optional ByVal Trans As Object = Nothing)

        ' Aqui vão as consistências
        ' (a validação dos campos obrigatórios já é feita automaticamente nas classes DB)

        meuDB.Salva(Me, Trans)

    End Sub


    Public Sub Remove(Optional ByVal Trans As Object = Nothing)
        If Not Cod.HasValue Then
            Throw New ApplicationException("Este " & meuDB.NomeDoBO & " ainda não foi salvo no banco de dados para poder ser removido.")
        End If
        If Not _Tstamp.HasValue Then
            Throw New ApplicationException("Este " & meuDB.NomeDoBO & "  ainda não foi salvo no banco de dados para poder ser removido.")
        End If
        meuDB.Remove(Me, Trans)
    End Sub

End Class

