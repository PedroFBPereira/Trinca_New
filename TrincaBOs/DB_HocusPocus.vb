Imports System.Data.SqlClient
Imports System.Text

    Public MustInherit Class DB_HocusPocus
        ' 
        ' Esta classe é uma variação dos objetos DB criados pelo Gerador de Código.
        ' Como não tenho no momento (Pedro em 05.julho.2017) acesso ao Gerador, e tenho que criar várias classes DB e BO novas, criei esta variação.
        ' A idéia é, para cada novo BD criado a partir deste, alterar apenas as constantes definidas ao início da classe e o array de campos.
        ' (É preciso, também, alterar a rotina "CarregaAK1", que vai variar de acordo com a estrutura da AK que está sendo usada.

        Protected Friend Class Campo
            Property NomePropriedadeObjeto As String
            Property NomeCampoTabela As String
            Property TipoSQL As SqlDbType
            Property Obrigatorio As Boolean
            Property TiposDeFiltro As String()
            Property NomeParaVisualizacao As String
            ReadOnly Property NomeParametroSQL As String
                Get
                    Return "@" & Me.NomeCampoTabela
                End Get
            End Property
            Private Sub New()
                ' apenas para tornar obrigatório o uso do construtor público com parâmetros
            End Sub

            Public Sub New(prmNomePropriedadeObjeto As String, prmNomeCampoTabela As String, prmTipo As SqlDbType, prmObrigatorio As Boolean, prmTiposDeFiltro() As String, Optional prmNomeParaVisualizacao As String = "")
                NomePropriedadeObjeto = prmNomePropriedadeObjeto
                NomeCampoTabela = prmNomeCampoTabela
                TipoSQL = prmTipo
                Obrigatorio = prmObrigatorio
                TiposDeFiltro = prmTiposDeFiltro
                NomeParaVisualizacao = prmNomeParaVisualizacao
            End Sub
        End Class

        ' Array que descreve as propriedades do BO associando-as com campos da tabela
        ' também informa se um campo é obrigatório ou não
        ' e também define que tipos de filtro serão usados para cada um deles.
        ' NOTE QUE A ESTRUTURA DA CLASSE FILTRO DO BO TEM DE "COMBINAR" COM O QUE É DEFINIDO AQUI.
        ' Ou seja, um campo chamado COD que tenha filtros "igual","inicial","final" definidos terá de ter no filtro os campos COD, CODI e CODF.
        ' Um campo chamado LOTE que tenha só um filtro ("like" ou "igual") definido aqui, só precisará do campo LOTE no Filtro.
        ' Um filtro "igual" ou "like" exige um campo no filtro que tenha o mesmo nome da propriedade do objeto.
        ' Um filtro "inicial" exige um campo no filtro chamado nome-da-propriedade-no-objetoI.
        ' E um filtro "final" exige um campo no filtro chamado nome-da-propriedade-no-objetoF.
        ' Para um campo que não exista no Filtro da classe, simplesmente use Nothing como "tipos de filtro".
        Protected arrFiltroVazio(-1) As String ' array que não é nothing mas não contém elementos...
        Protected Campos As Campo()

        ' Este valores tem de ser setados na "classe 'herdeira'"
        Protected CampoChavePrimaria As Campo
        Protected CamposAK As Campo()
        Protected CampoUltimaAlteracao As Campo
        Protected CampoTimeStamp As Campo

        ' constantes usados para mensagens de erro - estes valores devem ser setados na "classe 'herdeira'"
        Protected Friend NomeDoBO As String = "Coloque aqui o nome do Business Object com o qual está trabalhando - por exemplo, 'BOInsumoLote'" ' este é Friend porque está sendo usado nos Bo para fazer LogData...
        Protected NomeCamposAK As String = "Coloque aqui os nomes dos campos que formam a Alternate Key - por exemplo, 'Código do Insumo e Lote'"

        ' Nome da tabela que este objeto DB gerencia - sete este valor na 'classe herdeira'
        Protected NomeDaTabela As String = "Coloque aqui o nome da tabela com que vai trabalhar - por exemplo, 'INSUMOS_LOTES'"

        Protected Overridable Function SqlSelect(prmMaxReg As Integer) As String
            ' Esta rotina monta um select como, por exemplo
            ' SELECT COD ,COD_INSUMO ,LOTE ,FABRICACAO ,VALIDADE ,ULTIMA_ALTERACAO ,CAST([TSTAMP] AS INT) AS [TSTAMP] FROM INSUMOS_LOTES 

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("SELECT ")
            If prmMaxReg > 0 Then
                sb.AppendFormat("TOP {0} ", prmMaxReg.ToString)
            End If

            Dim strVirgula As String = String.Empty
            For Each meuCampo In Campos
                If meuCampo.NomePropriedadeObjeto <> CampoTimeStamp.NomePropriedadeObjeto Then
                    sb.Append(strVirgula & meuCampo.NomeCampoTabela & " ")
                Else
                    sb.Append(strVirgula & "CAST([" & CampoTimeStamp.NomeCampoTabela & "] AS INT) AS [" & CampoTimeStamp.NomeCampoTabela & "]")
                End If
                strVirgula = ","
            Next

            sb.Append(" FROM " & NomeDaTabela)

            Return sb.ToString
        End Function

        Protected Overridable Sub CarregaDR(Of tipodeBO)(ByVal oDR As SqlDataReader, ByRef obj As tipodeBO)
            ' https://docs.microsoft.com/en-us/dotnet/visual-basic/programming-guide/language-features/early-late-binding/calling-a-property-or-method-using-a-string-name 
            ' https://stackoverflow.com/questions/240836/dynamically-invoke-properties-by-string-name-using-vb-net 
            ' http://www.vbforums.com/showthread.php?568401-Callbyname-reflection-working-but-not-really
            For Each meuCampo In Campos
                If oDR(meuCampo.NomeCampoTabela) Is DBNull.Value Then
                    Dim minhaPropriedade As Reflection.PropertyInfo = obj.GetType.GetProperty(meuCampo.NomePropriedadeObjeto)
                    minhaPropriedade.SetValue(obj, Nothing)
                Else
                    CallByName(obj, meuCampo.NomePropriedadeObjeto, CallType.Set, oDR(meuCampo.NomeCampoTabela)) ' seta a propriedade do objeto para o campo do data reader
                End If
            Next
        End Sub


        Protected Overridable Sub MontaFiltros(Of tipodeFiltro)(ByVal Filtro As tipodeFiltro, ByRef sb As StringBuilder, ByRef oCmd As SqlCommand, ByRef bWhere As Boolean)

            If Filtro Is Nothing Then
                Exit Sub
            End If

            For Each meucampo In Campos
                For Each meuFiltro In meucampo.TiposDeFiltro
                    Dim strNomeDoCampoFiltro As String = meucampo.NomePropriedadeObjeto
                    If meuFiltro = "inicial" Then
                        strNomeDoCampoFiltro &= "I"
                    Else
                        If meuFiltro = "final" Then
                            strNomeDoCampoFiltro &= "F"
                        End If
                    End If
                    Dim objValorDoCampoNoFiltro As New Object
                    objValorDoCampoNoFiltro = CallByName(Filtro, strNomeDoCampoFiltro, CallType.Get)
                    If objValorDoCampoNoFiltro IsNot Nothing Then
                        MontaFiltros_MontaCondicao(oCmd, bWhere, sb, meucampo.NomeCampoTabela, objValorDoCampoNoFiltro, meuFiltro)
                    End If
                Next
            Next

        End Sub


    Private Sub MontaFiltros_MontaCondicao(ByRef prmCmd As SqlCommand, ByRef prmWhere As Boolean, ByRef prmStringBuilderDoSQL As StringBuilder, prmNomeDoCampoNaTabela As String, prmCampoDoFiltro As Object, prmIgual_Inicial_Final As String)
        If Not (prmIgual_Inicial_Final = "igual" Or prmIgual_Inicial_Final = "inicial" Or prmIgual_Inicial_Final = "final" Or prmIgual_Inicial_Final = "like") Then
            Throw New ApplicationException("Chamada da Rotina MontaFiltros_MontaCondicao com parâmetro 'prmIgual_Inicial_Final inválido - " & prmIgual_Inicial_Final & ".")
        End If
        If prmIgual_Inicial_Final = "like" Then
            Dim tipoDoCampo As Type = prmCampoDoFiltro.GetType()
            If Not (tipoDoCampo.Equals(GetType(String))) Then
                Throw New ApplicationException("Chamada da Rotina MontaFiltros_MontaCondicao passando operador 'like' para campo não string - " & prmNomeDoCampoNaTabela & ".")
            End If
        End If

        Dim strOperador As String
        Dim strSufixoInicialFinal As String = String.Empty
        If prmIgual_Inicial_Final = "igual" Then
            strOperador = " = "
        Else
            If prmIgual_Inicial_Final = "inicial" Then
                strOperador = " >= "
                strSufixoInicialFinal = "I"
            Else
                If prmIgual_Inicial_Final = "final" Then
                    strOperador = " <= "
                    strSufixoInicialFinal = "F"
                Else ' só pode ser 'like'
                    strOperador = " like "
                End If
            End If
        End If

        prmStringBuilderDoSQL.Append(MontaFiltros_WhereOuAnd(prmWhere))

        Dim strNomeParametro As String = "@" & prmNomeDoCampoNaTabela & strSufixoInicialFinal
        Dim strClausula As String = prmNomeDoCampoNaTabela & strOperador & strNomeParametro
        prmStringBuilderDoSQL.Append(strClausula)

        If Not (strOperador.Trim = "like") Then
            prmCmd.Parameters.AddWithValue(strNomeParametro, prmCampoDoFiltro)
        Else
            prmCmd.Parameters.AddWithValue(strNomeParametro, "%" & prmCampoDoFiltro & "%")
        End If

    End Sub


    Private Function MontaFiltros_WhereOuAnd(ByRef prmwhere As Boolean) As String
        If Not prmwhere Then
            prmwhere = True
            Return " WHERE "
        Else
            Return " AND "
        End If
    End Function


    Public Overridable Function Carrega(Of tipodeBO)(ByRef obj As tipodeBO, ByVal prmChavePrimaria As Integer, Optional ByVal Trans As Object = Nothing) As Boolean
        ' esta rotina monta um SELECT como, por exemplo, 
        ' SELECT COD ,COD_INSUMO ,LOTE ,FABRICACAO ,VALIDADE ,ULTIMA_ALTERACAO ,CAST([TSTAMP] AS INT) AS [TSTAMP] FROM INSUMOS_LOTES WHERE COD = @COD

        'TTCG2005-I-AA
        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
        Dim e As ApplicationException = Nothing
        Dim oDR As SqlDataReader
        Dim bExiste As Boolean = False
        'TTCG2005-F-AA

        'TTCG2005-I-AB
        If Trans Is Nothing Then
            Try
                oConn = DBControl.GetDBConnection()
            Catch ex As Exception
                Throw New ApplicationException(NomeDoBO & ": Erro durante a conexão com o banco de dados.", ex)
            End Try
        Else
            oConn = Trans.Connection
        End If
        'TTCG2005-F-AB

        'TTCG2005-I-AC
        Try
            'TTCG2005-F-AC

            'TTCG2005-I-AD
            sb = New StringBuilder
            sb.Append(SqlSelect(0))
            sb.Append(" WHERE " & CampoChavePrimaria.NomeCampoTabela & " = " & CampoChavePrimaria.NomeParametroSQL)
            'TTCG2005-F-AD

            'TTCG2005-I-AE
            Using oCmd As SqlCommand = New SqlCommand(sb.ToString, oConn)
                oCmd.Parameters.AddWithValue(CampoChavePrimaria.NomeParametroSQL, prmChavePrimaria)
                If Trans IsNot Nothing Then
                    oCmd.Transaction = Trans
                End If
                oDR = oCmd.ExecuteReader
                If oDR.Read Then
                    bExiste = True
                    CarregaDR(oDR, obj)
                End If
                oDR.Close()
            End Using
            'TTCG2005-F-AE

            'TTCG2005-I-AF
        Catch ex As Exception
            'TTCG2005-F-AF

            'TTCG2005-I-AG
            e = New ApplicationException("Erro durante a carga Do " & NomeDoBO & ".", ex)
            'TTCG2005-F-AG

            'TTCG2005-I-AH
        End Try
        'TTCG2005-F-AH

        'TTCG2005-I-AI
        If Trans Is Nothing Then
            DBControl.ReleaseDBConnection(oConn)
        End If
        'TTCG2005-F-AI

        'TTCG2005-I-AJ
        If e IsNot Nothing Then
            Throw e
        End If
        'TTCG2005-F-AJ

        Return bExiste

    End Function


    Public Overridable Function CarregaAK1_(Of tipodeBO)(ByRef obj As tipodeBO, ByVal arrValoresParametros As Object(), ByVal prmLote As String, Optional ByVal Trans As Object = Nothing) As Boolean

        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
        Dim e As ApplicationException = Nothing
        Dim oDR As SqlDataReader
        Dim bExiste As Boolean = False

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
                sb = New StringBuilder
                sb.Append(SqlSelect(0))
                Dim strWhereOuAnd As String = " WHERE "
                For i As Integer = 0 To CamposAK.Length - 1
                    sb.Append(strWhereOuAnd & CamposAK(i).NomeCampoTabela & " = " & CamposAK(i).NomeParametroSQL)
                    strWhereOuAnd = " AND "
                    oCmd.Parameters.AddWithValue(CamposAK(i).NomeParametroSQL, arrValoresParametros(i))
                Next i

                oCmd.CommandText = sb.ToString
                oCmd.Connection = oConn
                If Trans IsNot Nothing Then
                    oCmd.Transaction = Trans
                End If
                oDR = oCmd.ExecuteReader
                If oDR.Read Then
                    bExiste = True
                    CarregaDR(oDR, obj)
                End If
                oDR.Close()
                oDR = Nothing

            End Using

        Catch ex As Exception
            e = New ApplicationException("Erro durante a carga do " & NomeDoBO & ".", ex)
        End Try

        If Trans Is Nothing Then
            DBControl.ReleaseDBConnection(oConn)
        End If

        If e IsNot Nothing Then
            Throw e
        End If

        Return bExiste

    End Function


    Public Overridable Function CarregaDT(Of tipodeFiltro)(ByVal Filtro As tipodeFiltro, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As DataTable

        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
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
                sb = New StringBuilder
                sb.Append(SqlSelect(nMaxReg))
                MontaFiltros(Filtro, sb, oCmd, bWhere)

                oCmd.CommandText = sb.ToString
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


    Public Overridable Function CarregaArray(Of tipodeBO As New, tipodeFiltro)(ByVal Filtro As tipodeFiltro, Optional nMaxReg As Integer = 0, Optional ByVal Trans As Object = Nothing) As tipodeBO()

        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
        Dim e As ApplicationException = Nothing
        Dim oDR As SqlDataReader
        Dim bWhere As Boolean = False
        Dim al As New ArrayList
        Dim obj As tipodeBO

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
                sb = New StringBuilder
                sb.Append(SqlSelect(nMaxReg))
                MontaFiltros(Filtro, sb, oCmd, bWhere)

                oCmd.CommandText = sb.ToString
                oCmd.Connection = oConn
                If Trans IsNot Nothing Then
                    oCmd.Transaction = Trans
                End If
                oDR = oCmd.ExecuteReader
                While oDR.Read
                    obj = New tipodeBO
                    CarregaDR(oDR, obj)
                    al.Add(obj)
                End While
                oDR.Close()
                oDR = Nothing

            End Using

        Catch ex As Exception
            e = New ApplicationException("Erro durante a carga do array de " & NomeDoBO & ".", ex)
        End Try

        If Trans Is Nothing Then
            DBControl.ReleaseDBConnection(oConn)
        End If

        If e IsNot Nothing Then
            Throw e
        End If

        Return DirectCast(al.ToArray(GetType(tipodeBO)), tipodeBO())

    End Function

    Public Overridable Sub Salva(Of tipodeBO)(ByRef obj As tipodeBO, Optional ByVal Trans As Object = Nothing)

        ' Os comandos INSERT e UPDATE são montados completamente em tempo de execução.
        ' Para facilitar o entendimento de como a rotina foi construida, coloco abaixo exemplos de comandos gerados pela rotina:
        ' INSERT INTO INSUMOS_LOTES (COD_INSUMO ,LOTE ,FABRICACAO ,VALIDADE ,ULTIMA_ALTERACAO ) VALUES ( @COD_INSUMO ,@LOTE ,@FABRICACAO ,@VALIDADE ,GETDATE() ) 
        ' A rotina, evidentemente, também gera os parâmetros.

        For Each meuCampo In Campos
            If meuCampo.Obrigatorio Then
                Dim objConteudoCampo As New Object
                objConteudoCampo = CallByName(obj, meuCampo.NomePropriedadeObjeto, CallType.Get)
                If objConteudoCampo = Nothing Then
                    Throw New ApplicationException("Campo obrigatório " & IIf(meuCampo.NomeParaVisualizacao = String.Empty, meuCampo.NomeCampoTabela, meuCampo.NomeParaVisualizacao) & " não preenchido.")
                End If
            End If
        Next

        'TTCG2005-I-DA
        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
        Dim e As ApplicationException = Nothing
        Dim bTExterna As Boolean
        Dim bInsert As Boolean

        Dim strVirgula As String = String.Empty

        'TTCG2005-F-DA

        'TTCG2005-I-DB
        If Trans Is Nothing Then
            bTExterna = False
            Try
                oConn = DBControl.GetDBConnection()
            Catch ex As Exception
                Throw New ApplicationException(NomeDoBO & ": Erro durante a conexão com o banco de dados.", ex)
            End Try
        Else
            bTExterna = True
            oConn = Trans.Connection
        End If
        'TTCG2005-F-DB

        'TTCG2005-I-DC
        Try
            If Not bTExterna Then
                Trans = oConn.BeginTransaction
            End If
            'TTCG2005-F-DC

            'TTCG2005-I-DD
            Using oCmd As SqlCommand = New SqlCommand()
                oCmd.Connection = oConn
                oCmd.Transaction = Trans
                sb = New StringBuilder

                With oCmd
                    Dim objCampoChavePrimaria As Object
                    objCampoChavePrimaria = CallByName(obj, CampoChavePrimaria.NomePropriedadeObjeto, CallType.Get)
                    Dim nulofintCampoChavePrimaria As Nullable(Of Integer) = DirectCast(objCampoChavePrimaria, Nullable(Of Integer))
                    If Not nulofintCampoChavePrimaria.HasValue Then
                        bInsert = True
                        sb.Append("INSERT INTO " & NomeDaTabela & " (")
                        For Each meuCampo In Campos
                            If meuCampo.NomePropriedadeObjeto = CampoChavePrimaria.NomePropriedadeObjeto Then Continue For
                            If meuCampo.NomePropriedadeObjeto = CampoTimeStamp.NomePropriedadeObjeto Then Continue For
                            sb.Append(strVirgula & meuCampo.NomeCampoTabela & " ")
                            strVirgula = ","
                        Next
                        sb.Append(") VALUES ( ")
                        strVirgula = String.Empty
                        For Each meuCampo In Campos
                            If meuCampo.NomePropriedadeObjeto = CampoChavePrimaria.NomePropriedadeObjeto Then Continue For
                            If meuCampo.NomePropriedadeObjeto = CampoTimeStamp.NomePropriedadeObjeto Then Continue For
                            If meuCampo.NomePropriedadeObjeto = CampoUltimaAlteracao.NomePropriedadeObjeto Then
                                sb.Append(strVirgula & "GETDATE() ")
                            Else
                                sb.Append(strVirgula & meuCampo.NomeParametroSQL & " ")
                            End If
                            strVirgula = ","
                        Next
                        sb.Append(") ")
                    Else
                        sb.Append("UPDATE " & NomeDaTabela & " SET ")
                        For Each meuCampo In Campos
                            If meuCampo.NomePropriedadeObjeto = CampoChavePrimaria.NomePropriedadeObjeto Then Continue For
                            If meuCampo.NomePropriedadeObjeto = CampoTimeStamp.NomePropriedadeObjeto Then Continue For
                            If meuCampo.NomePropriedadeObjeto = CampoUltimaAlteracao.NomePropriedadeObjeto Then
                                sb.Append(strVirgula & meuCampo.NomeCampoTabela & " = GETDATE() ")
                            Else
                                sb.Append(strVirgula & meuCampo.NomeCampoTabela & " = " & meuCampo.NomeParametroSQL & " ")
                            End If
                            strVirgula = ","
                        Next
                        sb.Append("WHERE " & CampoChavePrimaria.NomeCampoTabela & " = " & CampoChavePrimaria.NomeParametroSQL)
                        .Parameters.AddWithValue(CampoChavePrimaria.NomeParametroSQL, nulofintCampoChavePrimaria)
                        sb.Append(" AND CAST(" & CampoTimeStamp.NomeParametroSQL & " AS TIMESTAMP) = [" & CampoTimeStamp.NomeCampoTabela & "] ")
                        Dim objTStamp As New Object
                        objTStamp = CallByName(obj, CampoTimeStamp.NomePropriedadeObjeto, CallType.Get)
                        .Parameters.AddWithValue("@TSTAMP", objTStamp)
                    End If

                    For Each meuCampo In Campos
                        If meuCampo.NomePropriedadeObjeto = CampoChavePrimaria.NomePropriedadeObjeto Then Continue For
                        If meuCampo.NomePropriedadeObjeto = CampoUltimaAlteracao.NomePropriedadeObjeto Then Continue For
                        If meuCampo.NomePropriedadeObjeto = CampoTimeStamp.NomePropriedadeObjeto Then Continue For
                        Dim objConteudoCampo As New Object
                        objConteudoCampo = CallByName(obj, meuCampo.NomePropriedadeObjeto, CallType.Get)
                        If objConteudoCampo IsNot Nothing Then
                            .Parameters.AddWithValue(meuCampo.NomeParametroSQL, objConteudoCampo)
                        Else
                            .Parameters.Add(meuCampo.NomeParametroSQL, meuCampo.TipoSQL)
                            .Parameters(meuCampo.NomeParametroSQL).Value = DBNull.Value
                        End If
                    Next

                    'TTCG2005-I-DL
                    .CommandText = sb.ToString
                    If .ExecuteNonQuery() = 0 Then
                        e = New ApplicationException("Este " & NomeDoBO & " foi alterado recentemente por outro usuário. Por favor atualize esta informação e tente novamente.")
                        If Not bTExterna Then
                            Trans.Rollback()
                            Trans.Dispose()
                            Trans = Nothing
                        End If
                        Exit Try
                    End If
                End With
                'TTCG2005-F-DL

                'TTCG2005-I-DM
                If bInsert Then
                    oCmd.Parameters.Clear()
                    oCmd.CommandText = "SELECT @@IDENTITY"
                    Dim novoCod As Integer
                    novoCod = CInt(oCmd.ExecuteScalar)
                    CallByName(obj, CampoChavePrimaria.NomePropriedadeObjeto, CallType.Set, novoCod)
                End If
                'TTCG2005-F-DM

                'TTCG2005-I-DN
            End Using
            'TTCG2005-F-DN

            'TTCG2005-I-DO
            If Not bTExterna Then
                Trans.Commit()
                Trans.Dispose()
                Trans = Nothing
            End If
            'TTCG2005-F-DO

            'TTCG2005-I-DP
        Catch ex As SqlException
            'TTCG2005-F-DP
            'TTCG2005-I-DQ
            If ex.Number = 2601 Or ex.Number = 2627 Then
                e = New ApplicationException("Este " & NomeDoBO & " já está cadastrado no sistema. Não pode haver registros com valores repetidos de " & NomeCamposAK & ".", ex)
            Else
                e = New ApplicationException("Erro durante a gravação do " & NomeDoBO & ".", ex)
            End If
            If Not bTExterna Then
                Trans.Rollback()
                Trans.Dispose()
                Trans = Nothing
            End If
            Throw e
            'TTCG2005-F-DQ
            'TTCG2005-I-DR
        Catch ex As Exception
            'TTCG2005-F-DR

            'TTCG2005-I-DS
            e = New ApplicationException("Erro durante a gravação do " & NomeDoBO & ".", ex)
            If Not bTExterna Then
                Trans.Rollback()
                Trans.Dispose()
                Trans = Nothing
            End If
            Throw e
            'TTCG2005-F-DS

            'TTCG2005-I-DT
        Finally
            'TTCG2005-F-DT

            'TTCG2005-I-DU
            If Not bTExterna Then
                DBControl.ReleaseDBConnection(oConn)
            End If
            'TTCG2005-F-DU

            'TTCG2005-I-DV
        End Try
        'TTCG2005-f-DV
        'TTCG2005-I-DX
        If e IsNot Nothing Then
            Throw e
        End If
        'TTCG2005-F-DX

    End Sub


    Public Overridable Sub Remove(Of tipodeBO)(ByRef obj As tipodeBO, Optional ByVal Trans As Object = Nothing)

        'TTCG2005-I-EA
        Dim oConn As SqlConnection = Nothing
        Dim sb As StringBuilder
        Dim e As ApplicationException = Nothing
        Dim bTExterna As Boolean
        'TTCG2005-F-EA

        'TTCG2005-I-EB
        If Trans Is Nothing Then
            bTExterna = False
            Try
                oConn = DBControl.GetDBConnection()
            Catch ex As Exception
                Throw New ApplicationException(NomeDoBO & ": Erro durante a conexão com o banco de dados.", ex)
            End Try
        Else
            bTExterna = True
            oConn = Trans.Connection
        End If
        'TTCG2005-F-EB

        'TTCG2005-I-EC
        Try
            If Not bTExterna Then
                Trans = oConn.BeginTransaction
            End If
            'TTCG2005-F-EC

            'TTCG2005-I-ED
            Using oCmd As SqlCommand = New SqlCommand()
                With oCmd
                    .Connection = oConn
                    .Transaction = Trans
                    'TTCG2005-F-ED

                    'TTCG2005-I-EE
                    sb = New StringBuilder
                    sb.Append("DELETE FROM " & NomeDaTabela & " ")
                    sb.Append("WHERE " & CampoChavePrimaria.NomeCampoTabela & " = " & CampoChavePrimaria.NomeParametroSQL)
                    Dim objValorDaChavePrimária As New Object
                    objValorDaChavePrimária = CallByName(obj, CampoChavePrimaria.NomePropriedadeObjeto, CallType.Get)
                    .Parameters.AddWithValue("@COD", objValorDaChavePrimária)
                    'TTCG2005-F-EE

                    'TTCG2005-I-EF
                    .CommandText = sb.ToString
                    .ExecuteNonQuery()
                    'TTCG2005-F-EF

                    'TTCG2005-I-EG
                End With
            End Using
            'TTCG2005-F-EG

            'TTCG2005-I-EH
            If Not bTExterna Then
                Trans.Commit()
                Trans.Dispose()
                Trans = Nothing
            End If
            'TTCG2005-F-EH

            'TTCG2005-I-EI
        Catch ex As SqlException
            'TTCG2005-F-EI
            'TTCG2005-I-EJ
            If ex.Number = 547 Then
                e = New ApplicationException("Este " & NomeDoBO & " não pode ser removido porque outros objetos dependem dele.", ex)
            Else
                e = New ApplicationException("Erro durante a remoção do " & NomeDoBO & ".", ex)
            End If
            If Not bTExterna Then
                Trans.Rollback()
                Trans.Dispose()
                Trans = Nothing
            End If
            'TTCG2005-F-EJ
            'TTCG2005-I-EK
        Catch ex As Exception
            'TTCG2005-F-EK

            'TTCG2005-I-EL
            e = New ApplicationException("Erro durante a remoção do " & NomeDoBO & ".", ex)
            If Not bTExterna Then
                Trans.Rollback()
                Trans.Dispose()
                Trans = Nothing
            End If
            'TTCG2005-F-EL

            'TTCG2005-I-EM
        End Try
        'TTCG2005-F-EM

        'TTCG2005-I-EN
        If Not bTExterna Then
            DBControl.ReleaseDBConnection(oConn)
        End If
        'TTCG2005-F-EN

        'TTCG2005-I-EO
        If e IsNot Nothing Then
            Throw e
        End If
        'TTCG2005-F-EO

    End Sub


End Class