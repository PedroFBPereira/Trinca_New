create function ValorPago(@prmValorComBebida as numeric(18,2), 
                          @prmValorSemBebida as numeric(18,2),
                          @prmPago AS bit, 
						  @prmComBebida as bit)
returns numeric(18,2)
AS
BEGIN
	DECLARE @Retorno as Decimal(18,2)
	IF @prmPago = 0 
		SET @Retorno = 0
	ELSE
	SET @Retorno = dbo.ValorContribSugerida(@prmValorComBebida, 
	                                        @prmValorSemBebida,
							    			@prmComBebida)
	                          
	RETURN @Retorno
END
