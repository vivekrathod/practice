create table employees (id int primary key, name nvarchar(255), salary int)
insert into employees values(1, 'e1', 10000)
insert into employees values(2, 'e2', 5000)
insert into employees values(3, 'e3', 30000)
insert into employees values(4, 'e4', 13000)
insert into employees values(5, 'e5', 1003)
insert into employees values(6, 'e6', 8000)

-- employees with above avg salaries
select e1.name, e1.salary from employees e1 join (select avg(salary) avg_sal from employees) as e2 on e1.salary > e2.avg_sal

--alternative