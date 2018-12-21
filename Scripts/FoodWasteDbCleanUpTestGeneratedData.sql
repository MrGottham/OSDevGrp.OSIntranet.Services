DELETE FROM Translations WHERE Value='Test' OR Value='Testing';
DELETE FROM ForeignKeys WHERE ForeignKeyForTypes LIKE '%IForeignKey%' AND (ForeignKeyValue='Test' OR ForeignKeyValue='Testing' OR ForeignKeyValue='ForeignKeyToFoodGroup');

CREATE TEMPORARY TABLE DeleteFoodGroups AS (
	SELECT
		fg.FoodGroupIdentifier AS FoodGroupIdentifier
	FROM FoodGroups AS fg
	LEFT JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fg.FoodGroupIdentifier
	WHERE
		fk.ForeignKeyForIdentifier IS NULL);

DELETE FROM FoodGroups
WHERE FoodGroupIdentifier IN (SELECT FoodGroupIdentifier FROM DeleteFoodGroups);

DROP TEMPORARY TABLE DeleteFoodGroups;

CREATE TEMPORARY TABLE DeleteFoodItmes AS (
	SELECT
		fi.FoodItemIdentifier AS FoodItemIdentifier
	FROM FoodItems AS fi
	LEFT JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fi.FoodItemIdentifier
	WHERE
		fk.ForeignKeyForIdentifier IS NULL);
        
DELETE FROM FoodItems
WHERE FoodItemIdentifier IN (SELECT FoodItemIdentifier FROM DeleteFoodItmes);

DROP TEMPORARY TABLE DeleteFoodItmes;

CREATE TEMPORARY TABLE DeleteHouseholds AS (
	SELECT
		h.HouseholdIdentifier
	FROM Households AS h
	LEFT JOIN MemberOfHouseholds AS moh ON moh.HouseholdIdentifier=h.HouseholdIdentifier
	WHERE
		moh.HouseholdIdentifier IS NULL);

DELETE FROM Storages
WHERE HouseholdIdentifier IN (SELECT HouseholdIdentifier FROM DeleteHouseholds);

DELETE FROM Households
WHERE HouseholdIdentifier IN (SELECT HouseholdIdentifier FROM DeleteHouseholds);

DROP TEMPORARY TABLE DeleteHouseholds;

CREATE TEMPORARY TABLE DeleteHouseholdMembers AS (
	SELECT
		hm.HouseholdMemberIdentifier
	FROM HouseholdMembers AS hm
	LEFT JOIN MemberOfHouseholds AS moh ON moh.HouseholdMemberIdentifier=hm.HouseholdMemberIdentifier
	WHERE
		moh.HouseholdMemberIdentifier IS NULL);

DELETE FROM HouseholdMembers
WHERE HouseholdMemberIdentifier IN (SELECT HouseholdMemberIdentifier FROM DeleteHouseholdMembers);

DROP TEMPORARY TABLE DeleteHouseholdMembers;

DELETE FROM HouseholdMembers WHERE MailAddress LIKE 'test.%@osdevgrp.dk';

CREATE TEMPORARY TABLE DeleteMemberOfHouseholds AS (
	SELECT
		moh.MemberOfHouseholdIdentifier
	FROM MemberOfHouseholds AS moh
	LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier
	LEFT JOIN Households AS h ON h.HouseholdIdentifier=moh.HouseholdIdentifier
	WHERE
		hm.HouseholdMemberIdentifier IS NULL OR
		h.HouseholdIdentifier IS NULL);

DELETE FROM MemberOfHouseholds
WHERE MemberOfHouseholdIdentifier IN (SELECT MemberOfHouseholdIdentifier FROM DeleteMemberOfHouseholds);

DROP TEMPORARY TABLE DeleteMemberOfHouseholds;