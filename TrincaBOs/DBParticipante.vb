Imports System.Data.SqlClient
Imports System.Text

Public Class FiltroParticipante
    Public Cod As Nullable(Of Integer)
    Public Nome As String

    Public UltimaAlteracao As Nullable(Of Date)
    Public UltimaAlteracaoI As Nullable(Of Date)
    Public UltimaAlteracaoF As Nullable(Of Date)

    Public MaxReg As Nullable(Of Integer)
End Class


Public Class DBParticipante
    Inherits DB_HocusPocus
    ' 
    ' Esta classe é uma variação dos objetos DB criados pelo Gerador de Código.
    ' Como não tenho no momento (Pedro em 05.julho.2017) acesso ao Gerador, e tenho que criar várias classes DB e BO novas, criei esta variação.
    ' A idéia é, para cada novo BD criado a partir deste, alterar apenas as constantes definidas ao início da classe e o array de campos.
    ' (É preciso, também, alterar a rotina "CarregaAK1", que vai variar de acordo com a estrutura da AK que está sendo usada.


    Public meuFiltro As FiltroParticipante

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
                  New Campo("Nome", "NOME", SqlDbType.VarChar, True, {"like"}, "Nome do Participante"),
                  New Campo("UltimaAlteracao", "ULTIMA_ALTERACAO", SqlDbType.DateTime, False, {"igual", "inicial", "final"}, "Data´da Última Alteração"),
                  New Campo("TStamp", "TSTAMP", SqlDbType.Timestamp, False, arrFiltroVazio)
                 }

        CampoChavePrimaria = Campos(0)
        CamposAK = {Campos(1)}
        CampoUltimaAlteracao = Campos(2)
        CampoTimeStamp = Campos(3)

        ' constantes usados para mensagens de erro 
        NomeDoBO = "Participantes dos Churrascos da Trinca"
        NomeCamposAK = "Nome do Participante"

        ' Nome da tabela que este objeto DB gerencia
        NomeDaTabela = "Participantes"

        meuFiltro = New FiltroParticipante()
    End Sub

    Public Function CarregaAK1(ByRef obj As BOParticipante, ByVal prmNome As String, Optional ByVal Trans As Object = Nothing) As Boolean
        ' Os parâmetros de entrada desta rotina devem ser ajustados para cada objeto DB, de acordo com a estrutura (campos) da alternate key.
        ' O array de parametros, abaixo, deve ser construido a partir dos parametros de entrada
        ' NOTE QUE A ORDEM DOS PARÃMETROS DEVE SER A MESMA DO ARRAY CamposAK, DEFINIDO AO INÍCIO DESTA CLASSE BO 
        ' - O ARRAY DE PARÃMETROS DEVE SER CONSTRUIDO COM O MESMO NÚMERO DE ITENS NO ARRAY DE CamposAK, E NA MESMA ORDEM!
        ' Nada mais precisa ser alterado nesta rotina para trabalhar com uma tabela de banco de dados diferente!

        Dim arrValoresParametros() As Object = {prmNome}
        Return CarregaAK1_(obj, arrValoresParametros, Trans)

    End Function

End Class

