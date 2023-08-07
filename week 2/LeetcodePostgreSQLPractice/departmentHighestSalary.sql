-- https://leetcode.com/problems/department-highest-salary/

SELECT Department.name as Department, Employee.name as Employee, salary as Salary
FROM Employee LEFT JOIN Department ON Employee.departmentId = Department.id
WHERE (departmentId, salary) IN
    (SELECT departmentId,  MAX(salary) as maxSalary
    FROM Employee GROUP BY departmentId);