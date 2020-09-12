create database if not exists onlinewallet;

use onlinewallet;

create table user
(
	id int primary key auto_increment,
    username varchar(20) not null,
    firstname varchar(20) not null,
    middlename varchar(20),
    lastname varchar(30) not null,
    email varchar(100) not null,
    isConfirmed bool
);

create table administrator
(
	id int primary key auto_increment,
    username varchar(20) not null,
    firstname varchar(20) not null,
    middlename varchar(20),
    lastname varchar(30) not null,
    email varchar(100) not null
);

create table account
(
	id int primary key,
    balance float check(balance >= 0 and balance <= 15000),
    isFrozen bool
);

create table transaction
(
	id int primary key,
    amount float,
    _from int,
    _to int,
    _datetime datetime,
    foreign key (_from) references account (id),
    foreign key (_to) references account (id)
);
