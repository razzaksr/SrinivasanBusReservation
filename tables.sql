Creating Bus table
create table bus(id int not null primary key, name varchar(MAX), capacity int);

Creating passengers table
create table passengers(sno int not null identity(1,1) primary key, age int not null, name varchar(MAX), doj date, bus_id int foreign key references bus(id));