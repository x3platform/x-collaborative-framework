DROP FUNCTION IF EXISTS `func_PaddingLeft`;

CREATE DEFINER = `root`@`localhost` FUNCTION `func_PaddingLeft`(Text VARCHAR(100),
 PaddingChar CHAR(1),
 PaddingToLen INT)
 RETURNS varchar(1000)
BEGIN

	DECLARE PaddingText VARCHAR(1000);

	DECLARE OriginalLen INT;

	DECLARE PaddingCount INT;

	SET OriginalLen = LENGTH(Text);

	IF (OriginalLen >= PaddingToLen) THEN
		SET PaddingText = Text;
	ELSE
		SET PaddingText = '';
		
		SET PaddingCount = PaddingToLen - OriginalLen;
		
		WHILE (PaddingCount > 0) DO
			SET PaddingText = CONCAT(PaddingText,PaddingChar);
			SET PaddingCount = PaddingCount - 1;
		END WHILE;

		SET PaddingText = CONCAT(PaddingText, Text);
	END IF;

	RETURN PaddingText;
END;