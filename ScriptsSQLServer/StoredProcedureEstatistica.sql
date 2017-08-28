create procedure Estatistica
AS
select COD_CHURRASCO,
       Count(*) as NumeroParticipantes,
       sum(Bebum) as Bebuns,
	   sum(Saudavel) as Saudaveis,
	   sum(ContribEsperada) as ValorASerArrecadado,
	   sum(ValorPago) as ValorJahPago
from
(
	SELECT Churrascos_Participantes.COD_CHURRASCO,
		   Churrascos_Participantes.COD_PARTICIPANTE,
		   iif(COM_BEBIDA=1,1,0) as Bebum,
		   iif(COM_BEBIDA=0,1,0) as Saudavel,
		   dbo.ValorContribSugerida(CONTRIB_COMBEBIDA, CONTRIB_SEMBEBIDA, COM_BEBIDA) as ContribEsperada,
		   dbo.ValorPago(CONTRIB_COMBEBIDA, CONTRIB_SEMBEBIDA, PAGO, COM_BEBIDA) as ValorPago
	FROM Churrascos_Participantes
	INNER JOIN CHURRASCOS on Churrascos_Participantes.COD_CHURRASCO = CHURRASCOS.COD
) as Prepara
GROUP BY COD_CHURRASCO




