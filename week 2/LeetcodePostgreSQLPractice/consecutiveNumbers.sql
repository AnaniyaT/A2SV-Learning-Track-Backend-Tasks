-- https://leetcode.com/problems/consecutive-numbers/

SELECT DISTINCT num as ConsecutiveNums FROM

    (SELECT LAG(num) OVER(ORDER BY id) as befor,
    num,
    LEAD(num) OVER(ORDER BY id) as aftr
    FROM Logs) beforeAndAfter
    
    WHERE befor = num AND aftr = num;