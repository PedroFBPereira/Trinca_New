
create function ValorContribSugerida(@prmValorComBebida as numeric(18,2), 
                                     @prmValorSemBebida as numeric(18,2),
						             @prmComBebida as bit)
returns numeric(18,2)
AS
BEGIN
	DECLARE @Retorno as Decimal(18,2)

    IF @prmComBebida = 1
		SET @Retorno = @prmValorComBebida
	ELSE
		SET @Retorno = @prmValorSemBebida

	RETURN @Retorno
END

