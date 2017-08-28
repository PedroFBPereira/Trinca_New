Imports ControlesDiversos

Public Class BOParticipante

#Region "Atributos"

    Private _UltimaAlteracao As Nullable(Of Date)
    Private _Tstamp As Nullable(Of Integer)

    Private meuDB As DBParticipante

#End Region

    Public Sub New()
        meuDB = New DBParticipante()
    End Sub

#Region "Propriedades"

    Public Cod As Nullable(Of Integer)
    Public Nome As String

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

    Public Function Carrega(ByVal Cod As Integer, Optional ByVal Trans As Object = Nothing) As Boolean
        Dim bResultado As Boolean
        bResultado = meuDB.Carrega(Me, Cod, Trans)
        Return bResultado
    End Function


    Public Function CarregaAK1(ByVal prmNome As String, Optional ByVal Trans As Object = Nothing) As Boolean
        Return meuDB.CarregaAK1(Me, prmNome, Trans)
    End Function


    Public Function CarregaArray(ByVal Filtro As FiltroParticipante, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As BOParticipante()
        Return meuDB.CarregaArray(Of BOParticipante, FiltroParticipante)(Filtro, nMaxReg, Trans)
    End Function


    Public Function CarregaDT(ByVal Filtro As FiltroParticipante, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As DataTable
        Return meuDB.CarregaDT(Filtro, nMaxReg, Trans)
    End Function

    Public Sub CarregaLista(ByRef ctrlComItems As Object, Optional prmFiltro As FiltroParticipante = Nothing, Optional prmTrans As Object = Nothing)
        ' o parâmetro 'ctrlComItems´ está definido como Object,
        ' mas tem de ser uma combobox 
        ' ou uma listbox
        ' ou outro controle que tenha uma propriedade Itens.
        If prmFiltro Is Nothing Then
            prmFiltro = New FiltroParticipante()
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

