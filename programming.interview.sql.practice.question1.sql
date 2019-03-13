create table salespersons (id int primary key, name nvarchar(255), age int, salary int)
insert into salespersons values (1, 's1', 55, 10000)
insert into salespersons values (2, 's2', 33, 15000)
insert into salespersons values (3, 's3', 21, 50000)
insert into salespersons values (4, 's4', 43, 75000)

create table customers (id int, name nvarchar(255), city nvarchar(500), ind_type nvarchar)
insert into customers values (1, 'c1', 'city1', 'A')
insert into customers values (2, 'c2', 'city2', 'B')
insert into customers values (3, 'c3', 'city3', 'C')
insert into customers values (4, 'c4', 'city4', 'D')

create table orders (id int primary key, c_id int, s_id int, amount int)
insert into orders values (1, 1, 2, 11) -- c1 -> s2
insert into orders values (2, 2, 1, 13) -- c2 -> s1
insert into orders values (3, 4, 3, 31) -- c4 -> s3
insert into orders values (4, 1, 3, 31) -- c1 -> s3
insert into orders values (5, 1, 3, 44) -- c1 -> s3

-- salespersons who placed order for customer 'c1'
select distinct salespersons.name from orders 
join customers on orders.c_id = customers.id and customers.name = 'c1'
join salespersons on orders.s_id = salespersons.id

-- salespersons who did NOT place order for customer 'c2'
select distinct salespersons.name from orders 
join customers on orders.c_id = customers.id and customers.name <> 'c2'
join salespersons on orders.s_id = salespersons.id

-- salespersons who have placed more than 1 order
select salespersons.name from salespersons 
join orders on salespersons.id = orders.s_id
group by salespersons.name having count(salespersons.name) > 1


-- table of salespersons with more than 10000 salary
drop table highAchievers
select * into highAchievers from salespersons where salary > 10000
select * from highAchievers

-- largest order by each salesperson
select s.name, o2.id, o2.amount, c.name from orders o2 
join salespersons s on o2.s_id = s.id
join customers c on o2.c_id = c.id
where o2.amount in (select max(o1.amount) from orders o1 group by o1.s_id)

-- alternative syntax
select s.name, o2.id, o2.amount, c.name from orders o2 
join salespersons s on o2.s_id = s.id
join customers c on o2.c_id = c.id
join (select max(o1.amount) as maxOrder from orders o1 group by o1.s_id) as o1 on o1.maxOrder = o2.amount


select * from customers where null is null