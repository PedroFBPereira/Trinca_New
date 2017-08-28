Public Class ListItem
    Public Valor As Integer
    Public Texto As String

    Private Sub New()
    End Sub

    Public Sub New(prmValor As Integer, prmTexto As String)
        Valor = prmValor
        Texto = prmTexto
    End Sub
End Class
