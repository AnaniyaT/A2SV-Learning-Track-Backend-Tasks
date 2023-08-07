-- https://leetcode.com/problems/customers-who-never-order/

-- select name as Customers from Customers LEFT JOIN Orders 
-- ON Customers.id = Orders.customerId WHERE Orders.id IS NULL;

SELECT name AS Customers FROM Customers WHERE
Customers.id NOT IN (SELECT customerId FROM Orders);