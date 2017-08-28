Imports System.Data.SqlClient
Imports System.Text

Public Class FiltroChurrascoParticipante
    Public Cod As Nullable(Of Integer)
    Public CodChurrasco As Nullable(Of Integer)
    Public CodParticipante As Nullable(Of Integer)
    Public ComBebida As Nullable(Of Boolean)
    Public Pago As Nullable(Of Boolean)
    Public Observacoes As String

    Public UltimaAlteracao As Nullable(Of Date)
    Public UltimaAlteracaoI As Nullable(Of Date)
    Public UltimaAlteracaoF As Nullable(Of Date)

    Public MaxReg As Nullable(Of Integer)
End Class


Public Class DBChurrascoParticipante
    Inherits DB_HocusPocus
    ' 
    ' Esta classe é uma variação dos objetos DB criados pelo Gerador de Código.
    ' Como não tenho no momento (Pedro em 05.julho.2017) acesso ao Gerador, e tenho que criar várias classes DB e BO novas, criei esta variação.
    ' A idéia é, para cada novo BD criado a partir deste, alterar apenas as constantes definidas ao início da classe e o array de campos.
    ' (É preciso, também, alterar a rotina "CarregaAK1", que vai variar de acordo com a estrutura da AK que está sendo usada.


    Public meuFiltro As FiltroChurrascoParticipante

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
                  New Campo("CodChurrasco", "COD_CHURRASCO", SqlDbType.Int, True, {"igual"}, "Código do Churrasco"),
                  New Campo("CodParticipante", "COD_PARTICIPANTE", SqlDbType.Int, True, {"igual"}, "Código do Participante"),
                  New Campo("ComBebida", "COM_BEBIDA", SqlDbType.Bit, True, {"igual"}, "Com Bebida"),
                  New Campo("Pago", "PAGO", SqlDbType.Bit, True, {"igual"}, "Pago"),
                  New Campo("Observacoes", "OBS", SqlDbType.VarChar, False, {"like"}, "Observações"),
                  New Campo("UltimaAlteracao", "ULTIMA_ALTERACAO", SqlDbType.DateTime, False, {"igual", "inicial", "final"}, "Data´da Última Alteração"),
                  New Campo("TStamp", "TSTAMP", SqlDbType.Timestamp, False, arrFiltroVazio)
                 }

        CampoChavePrimaria = Campos(0)
        CamposAK = {Campos(1), Campos(2)}
        CampoUltimaAlteracao = Campos(3)
        CampoTimeStamp = Campos(4)

        ' constantes usados para mensagens de erro 
        NomeDoBO = "Participantes em um Churrasco da Trinca"
        NomeCamposAK = "Código do Churrasco, Código do Participante"

        ' Nome da tabela que este objeto DB gerencia
        NomeDaTabela = "Churrascos_Participantes"

        meuFiltro = New FiltroChurrascoParticipante()
    End Sub

    Public Function CarregaAK1(ByRef obj As BOChurrascoParticipante, ByVal prmCodChurrasco As Integer, prmCodParticipante As Integer, Optional ByVal Trans As Object = Nothing) As Boolean
        ' Os parâmetros de entrada desta rotina devem ser ajustados para cada objeto DB, de acordo com a estrutura (campos) da alternate key.
        ' O array de parametros, abaixo, deve ser construido a partir dos parametros de entrada
        ' NOTE QUE A ORDEM DOS PARÃMETROS DEVE SER A MESMA DO ARRAY CamposAK, DEFINIDO AO INÍCIO DESTA CLASSE BO 
        ' - O ARRAY DE PARÃMETROS DEVE SER CONSTRUIDO COM O MESMO NÚMERO DE ITENS NO ARRAY DE CamposAK, E NA MESMA ORDEM!
        ' Nada mais precisa ser alterado nesta rotina para trabalhar com uma tabela de banco de dados diferente!

        Dim arrValoresParametros() As Object = {prmCodChurrasco, prmCodParticipante}
        Return CarregaAK1_(obj, arrValoresParametros, Trans)

    End Function

End Class

