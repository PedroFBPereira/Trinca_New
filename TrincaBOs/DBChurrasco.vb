Imports System.Data.SqlClient
Imports System.Text

Public Class FiltroChurrasco
    Public Cod As Nullable(Of Integer)
    Public Data As Nullable(Of Date)
    Public DataI As Nullable(Of Date)
    Public DataF As Nullable(Of Date)
    Public Porque As String
    Public Obs As String
    Public ContribComBebida As Nullable(Of Decimal)
    Public ContribComBebidaI As Nullable(Of Decimal)
    Public ContribComBebidaF As Nullable(Of Decimal)
    Public ContribSemBebida As Nullable(Of Decimal)
    Public ContribSemBebidaI As Nullable(Of Decimal)
    Public ContribSemBebidaF As Nullable(Of Decimal)
    Public UltimaAlteracao As Nullable(Of Date)
    Public UltimaAlteracaoI As Nullable(Of Date)
    Public UltimaAlteracaoF As Nullable(Of Date)


    Public MaxReg As Nullable(Of Integer)
End Class


Public Class DBChurrasco
    Inherits DB_HocusPocus
    ' 
    ' Esta classe é uma variação dos objetos DB criados pelo Gerador de Código.
    ' Como não tenho no momento (Pedro em 05.julho.2017) acesso ao Gerador, e tenho que criar várias classes DB e BO novas, criei esta variação.
    ' A idéia é, para cada novo BD criado a partir deste, alterar apenas as constantes definidas ao início da classe e o array de campos.
    ' (É preciso, também, alterar a rotina "CarregaAK1", que vai variar de acordo com a estrutura da AK que está sendo usada.


    Public meuFiltro As FiltroChurrasco

    Sub New()
        ' Array que descreve as propriedades do BO associando-as com campos da tabela
        ' também informa se um campo é obrigatório ou não
        ' e também define que tipos de filtro serão usados para cada um deles.
        ' NOTE QUE A ESTRUTURA DA CLASSE FILTRO DO BO TEM DE "COMBINAR" COM O QUE É DEFINIDO AQUI.
        ' Ou seja, um campo chamado COD que tenha filtros "igual","inicial","final" definidos terá de ter no filtro os campos COD, CODI e CODF.
        ' Um campo chamado LOTE que tenha só um filtro ("like" ou "igual") definido aqui, só precisará do campo LOTE no Filtro.
        ' Um filtro "igual" ou "like" exige um campo no filtro que tenha o mesmo nome da propriedade do objeto.
        ' Um filtro "inicial" exige um campo no filtro chamado nome-da-propriedade-no-objetoI.
        ' E um filtro "final" exige um campo no filtro chamado nome-da-propriedade-no-objetoF.
        ' Para um campo que não exista no Filtro da classe, simplesmente use 'arrFiltroVazio' como "tipos de filtro".
        Campos = {
                  New Campo("Cod", "COD", SqlDbType.Int, False, {"igual"}, "Código"),
                  New Campo("Data", "DATA", SqlDbType.DateTime, True, {"igual", "inicial", "final"}, "Data do Churrasco"),
                  New Campo("Porque", "PORQUE", SqlDbType.VarChar, True, {"like"}, "Porque do Churrasco"),
                  New Campo("Obs", "OBS", SqlDbType.VarChar, False, {"like"}, "Observa;óes"),
                  New Campo("ContribComBebida", "CONTRIB_COMBEBIDA", SqlDbType.Decimal, False, {"igual", "inicial", "final"}, "Contribuição com Bebida"),
                  New Campo("ContribSemBebida", "CONTRIB_SEMBEBIDA", SqlDbType.Decimal, False, {"igual", "inicial", "final"}, "Contribuição sem Bebida"),
                  New Campo("UltimaAlteracao", "ULTIMA_ALTERACAO", SqlDbType.DateTime, False, {"igual", "inicial", "final"}, "Data´da Última Alteração"),
                  New Campo("TStamp", "TSTAMP", SqlDbType.Timestamp, False, arrFiltroVazio)
                 }

        CampoChavePrimaria = Campos(0)
        CamposAK = {Campos(2)}
        CampoUltimaAlteracao = Campos(6)
        CampoTimeStamp = Campos(7)

        ' constantes usados para mensagens de erro 
        NomeDoBO = "Churrascos da Trinca"
        NomeCamposAK = "Porque do Churrasco"

        ' Nome da tabela que este objeto DB gerencia
        NomeDaTabela = "dbo.Churrascos"

        meuFiltro = New FiltroChurrasco()
    End Sub

    Public Function CarregaAK1(ByRef obj As BOChurrasco, ByVal prmPorque As String, Optional ByVal Trans As Object = Nothing) As Boolean
        ' Os parâmetros de entrada desta rotina devem ser ajustados para cada objeto DB, de acordo com a estrutura (campos) da alternate key.
        ' O array de parametros, abaixo, deve ser construido a partir dos parametros de entrada
        ' NOTE QUE A ORDEM DOS PARÃMETROS DEVE SER A MESMA DO ARRAY CamposAK, DEFINIDO AO INÍCIO DESTA CLASSE BO 
        ' - O ARRAY DE PARÃMETROS DEVE SER CONSTRUIDO COM O MESMO NÚMERO DE ITENS NO ARRAY DE CamposAK, E NA MESMA ORDEM!
        ' Nada mais precisa ser alterado nesta rotina para trabalhar com uma tabela de banco de dados diferente!

        Dim arrValoresParametros() As Object = {prmPorque}
        Return CarregaAK1_(obj, arrValoresParametros, Trans)

    End Function


    Public Function CarregaDTEstatistica(Optional Trans As Object = Nothing) As DataTable

        Dim oConn As SqlConnection = Nothing
        Dim e As ApplicationException = Nothing
        Dim oDA As SqlDataAdapter
        Dim dt As New DataTable
        Dim bWhere As Boolean = False

        If Trans Is Nothing Then
            Try
                oConn = DBControl.GetDBConnection()
            Catch ex As Exception
                Throw New ApplicationException(NomeDoBO & ": Erro durante a conexão com o banco de dados.", ex)
            End Try
        Else
            oConn = Trans.Connection
        End If

        Try

            Using oCmd As SqlCommand = New SqlCommand()
                oCmd.CommandText = "Estatistica"
                oCmd.CommandType = CommandType.StoredProcedure
                oCmd.Connection = oConn
                If Trans IsNot Nothing Then
                    oCmd.Transaction = Trans
                End If
                oDA = New SqlDataAdapter(oCmd)
                oDA.Fill(dt)
                oDA.Dispose()
                oDA = Nothing

            End Using

        Catch ex As Exception
            e = New ApplicationException("Erro durante a carga do data table de " & NomeDoBO & ".", ex)
        End Try

        If Trans Is Nothing Then
            DBControl.ReleaseDBConnection(oConn)
        End If

        If e IsNot Nothing Then
            Throw e
        End If

        Return dt

    End Function
End Class

