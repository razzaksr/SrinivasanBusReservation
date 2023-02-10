Bus table:
create table bus(id int not null primary key, name varchar(255), capacity int);

Passengers table:
create table passengers(sno serial primary key, age int not null, name varchar(255), doj date, bus_id int, CONSTRAINT fk_bus FOREIGN KEY(bus_id) REFERENCES bus(id));