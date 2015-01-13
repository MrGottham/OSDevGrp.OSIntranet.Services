CREATE TABLE IF NOT EXISTS TranslationInfos (
	TranslationInfoIdentifier VARCHAR(40) NOT NULL,
	CultureName VARCHAR(5) NOT NULL,
	PRIMARY KEY (TranslationInfoIdentifier)
);
CREATE UNIQUE INDEX IF NOT EXISTS IX_TranslationInfos_CultureName ON TranslationInfos (CultureName);
