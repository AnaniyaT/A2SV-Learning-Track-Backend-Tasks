--  https://leetcode.com/problems/combine-two-tables/

SELECT firstName, lastName, city, state from Person LEFT JOIN Address on Person.personId = Address.personId;