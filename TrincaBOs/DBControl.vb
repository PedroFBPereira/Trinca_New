
Imports System.Data.SqlClient
Public Class DBControl

    Public Shared Function GetDBConnection() As SqlConnection
        ' Data Source=DESKTOP-CKFFQD3\SQLEXPRESS;Initial Catalog=ChurrascosTrinca;Integrated Security=True
        Const strDataSource = "DESKTOP-CKFFQD3\SQLEXPRESS"
        Const strCatalog = "Churrascos"
        'Const strUserId = "trinca"
        Dim sCS As String = "Data Source=" & strDataSource & "; Initial Catalog=" & strCatalog & "; Integrated Security=SSPI; Pooling=True;"
        Dim oConn = New SqlConnection(sCS)
        Try
            oConn.Open()
        Catch e As SqlException
            Throw e
        End Try
        Return oConn
    End Function

    Public Shared Sub ReleaseDBConnection(ByRef Conn As SqlConnection)
        Try
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        Catch e As SqlException
            Throw e
        Finally
            Conn.Dispose()
        End Try
    End Sub

End Class
