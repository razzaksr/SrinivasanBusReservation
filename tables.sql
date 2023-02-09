Creating Bus table
create table bus(id int not null primary key, name varchar, capacity int);

Creating Journey table
create table journey(jid int not null primary key, doj date not null, bus_id int foreign key references bus(id));

Creating passengers table
create table passengers(pid int not null primary key, age int not null, name varchar, journey_id foreign key references journey(jid));