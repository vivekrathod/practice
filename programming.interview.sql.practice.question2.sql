create table users (id int primary key, name nvarchar(255), phone nvarchar(20))
insert into users values(1, 'u1', '111-111-1111')
insert into users values(2, 'u2', '111-111-1111')
insert into users values(3, 'u3', '111-111-1111')
insert into users values(4, 'u4', '111-111-1111')
insert into users values(5, 'u5', '111-111-1111')

create table user_history (u_id int, action_date date, action nvarchar(255))
insert into user_history values(1, '05/12/2015', 'logged_on')
insert into user_history values(1, '01/01/2015', 'logged_on')
insert into user_history values(2, '05/1/2015', 'logged_on')
insert into user_history values(2, '01/01/2015', 'logged_on')
insert into user_history values(3, '01/12/2015', 'logged_on')
insert into user_history values(4, '05/19/2015', 'logged_on')


-- find user names, phone and latest date for users logged on after 05/01/2015 
select name, phone, max(action_date) from users
join user_history on users.id = user_history.u_id 
group by name, phone having max(action_date) > '05/01/2015'

-- find user not in user_history
select * from users 
left join user_history on users.id = user_history.u_id
where user_history.u_id is null

